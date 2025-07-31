using Newtonsoft.Json;
using Sistran.Company.Application.Location.PropertyModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.PropertyModificationService.EEProvider.DAOs
{
    public class PropertyModificationIssueDAO
    {

        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveLoad)
        {
            if (collectiveLoad != null)
            {
                CollectiveEmission colectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
                colectiveEmission.User.UserId = collectiveLoad.User.UserId;
                List<CollectiveEmissionRow> colectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Events);

                if (colectiveEmissionRows.Count(x => x.HasEvents == false) > 0)
                {
                    TP.Task.Run(() => IssuanceCollectiveEmissionRows(collectiveLoad, colectiveEmission, colectiveEmissionRows));
                }
                else
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.ErrorRecordsNotFoundToIssue;
                }
            }

            return collectiveLoad;

        }

        public void IssuanceCollectiveEmissionRows(MassiveLoad collectiveLoad, CollectiveEmission colectiveEmission, List<CollectiveEmissionRow> massiveEmissionRows)
        {
            try
            {
                collectiveLoad.Status = MassiveLoadStatus.Issuing;
                collectiveLoad.TotalRows = massiveEmissionRows.Count;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                ExecuteCreatePolicy(massiveEmissionRows, colectiveEmission);
                collectiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
            }
            catch (Exception e)
            {
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = e.InnerException.Message;
                collectiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
            }

        }

        private void ExecuteCreatePolicy(List<CollectiveEmissionRow> emissionRows, CollectiveEmission collectiveEmission)
        {
            CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetPolicyByOperationId(collectiveEmission.TemporalId);
            //Fecha de emisión
            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
            companyPolicy.UserId = collectiveEmission.User.UserId;//USUARIO

            if (emissionRows.Any(x => x.HasEvents == true))
            {
                List<Risk> risks = new List<Risk>();
                ParallelHelper.ForEach(emissionRows.Where(x => x.HasEvents == false).ToList(), emissionRow =>
                {
                    PendingOperation pendingOperationRisk = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperationRisk = DelegateService.commonService.GetPendingOperationById(emissionRow.Risk.RiskId);
                    }
                    else
                    {
                        pendingOperationRisk = DelegateService.pendingOperationEntityService.GetPendingOperationById(emissionRow.Risk.RiskId);
                    }

                    Risk risk = new Risk();
                    risk = JsonConvert.DeserializeObject<Risk>(pendingOperationRisk.Operation);
                    risks.Add(risk);
                });

                companyPolicy.PayerComponents = DelegateService.underwritingService.CompanyCalculatePayerComponents(companyPolicy, risks, true);
                Policy policyCore = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policyCore, risks);
                companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policyCore);
                companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(policyCore, risks);
                companyPolicy.Summary.Risks = risks;
                PendingOperation pendingOperation = new PendingOperation();
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmission.TemporalId);
                }
                else
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmission.TemporalId);
                }
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.commonService.UpdatePendingOperation(pendingOperation);
            }

            companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
            collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
            collectiveEmission.EndorsementId = companyPolicy.Endorsement.Id;

            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            List<PendingOperation> pendingOperations;
            if (!Settings.UseReplicatedDatabase())
            {
                pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
            }
            else
            {
                pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
            }

            ParallelHelper.ForEach(emissionRows.Where(x => x.HasEvents == false).ToList(), emissionRow =>
            {
                CreateRisk(emissionRow, pendingOperations, companyPolicy);
            });
            UpdateJSONPolicyAndRecordEndorsementOperation(collectiveEmission, companyPolicy);
            DelegateService.underwritingService.CreateCompanyPolicyPayer(companyPolicy);

        }


        private void CreateRisk(CollectiveEmissionRow emissionRow, List<PendingOperation> pendingOperations, CompanyPolicy companyPolicy)
        {
            try
            {
                emissionRow.Status = CollectiveLoadProcessStatus.Issuance;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
                PendingOperation riskPendingOperation = pendingOperations.Find(po => po.Id == emissionRow.Risk.RiskId);
                if (riskPendingOperation == null)
                {
                    emissionRow.HasError = true;
                    emissionRow.Observations = Errors.ErrorRisksNotFound + KeySettings.ReportErrorSeparatorMessage();
                    return;
                }
                var propertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(riskPendingOperation.Operation);
                propertyRisk.CompanyPolicy = companyPolicy;
                DelegateService.propertyService.CreateRisk(propertyRisk);
            }
            catch (Exception ex)
            {
                emissionRow.HasError = true;
                emissionRow.Observations = $"{Errors.ErrorIssuing} | {ex.Message}";
            }
            finally
            {
                emissionRow.Status = CollectiveLoadProcessStatus.Finalized;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
            }
        }


        private void UpdateJSONPolicyAndRecordEndorsementOperation(CollectiveEmission collectiveEmission, CompanyPolicy companyPolicy)
        {
            var pendingOperation = new PendingOperation();
            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmission.TemporalId);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);

                /* Without Replicated Database */
            }
            else
            {
                /* with Replicated Database */
                pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmission.TemporalId);
                PendingOperation pendingOperationPolicy = new PendingOperation
                {
                    UserId = companyPolicy.UserId,
                    Operation = JsonConvert.SerializeObject(companyPolicy),
                    Id = companyPolicy.Id
                };

                string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, companyPolicy.Endorsement.Id, (char)007, companyPolicy.Id, (char)007, nameof(CollectiveEmission));
                var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee", serialization: "JSON");
                queue.PutOnQueue(pendingOperationJson);
                /* with Replicated Database */
            }

        }
    }
}
