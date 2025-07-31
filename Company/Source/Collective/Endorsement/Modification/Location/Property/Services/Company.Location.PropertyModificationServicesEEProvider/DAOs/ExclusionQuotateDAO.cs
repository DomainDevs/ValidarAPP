using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.PropertyModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.PropertyModificationService.EEProvider.DAOs
{
    public class ExclusionQuotateDAO
    {
        public void QuotateCollectiveExclusion(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, false);
            collectiveEmission.User.UserId = massiveLoad.User.UserId;

            if (collectiveEmission != null)
            {
                try
                {
                    TP.Task.Run(() => QuotateCollectiveExclusionRows(collectiveEmission));
                }
                catch (Exception ex)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = ex.Message;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }
        }

        private void QuotateCollectiveExclusionRows(CollectiveEmission collectiveEmission)
        {
            try
            {
                List<AUTHMO.AuthorizationRequest> authorizationRequests = new List<AUTHMO.AuthorizationRequest>();
                List<CollectiveEmissionRow> collectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveEmission.Id, CollectiveLoadProcessStatus.Validation);

                if (collectiveEmissionRows.Count > 0)
                {
                    collectiveEmission.Status = MassiveLoadStatus.Tariffing;
                    collectiveEmission.TotalRows = collectiveEmissionRows.Count;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                    PendingOperation pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmission.TemporalId);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmission.TemporalId);
                    }

                    if (pendingOperation == null)
                    {
                        throw new ValidationException("El temporal no existe");
                    }

                    CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.IsPersisted = true;
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.UserId = collectiveEmission.User.UserId;
                    //Número de Días
                    companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days;

                    List<Risk> risks = QuotateCollectiveExclusionRows(collectiveEmission, collectiveEmissionRows, companyPolicy, collectiveEmission.IsAutomatic, collectiveEmission.TemporalId, authorizationRequests);
                    if (risks.Any())
                    {
                        Policy policyCore = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                        companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policyCore, risks, true);
                        policyCore.PayerComponents = companyPolicy.PayerComponents;
                        companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policyCore, risks);
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policyCore);
                        companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(policyCore, risks);
                        pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                        pendingOperation.UserId = companyPolicy.UserId;
                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                        }
                        else
                        {
                            string pendingOperationJson = JsonConvert.SerializeObject(pendingOperation);
                            QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                            
                        }

                        DelegateService.underwritingService.CreateTempSubscription(companyPolicy);

                        collectiveEmission.Premium = companyPolicy.Summary.Premium;
                        collectiveEmission.Commiss = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                        collectiveEmission.HasEvents = companyPolicy.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;

                        if (collectiveEmission.HasEvents.Value)
                        {
                            authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesPolicy(companyPolicy, collectiveEmission));
                        }

                        if (authorizationRequests.Any())
                        {
                            DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests);
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }

                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
            }
            catch (Exception ex)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = ex.Message;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
        }

        private List<Risk> QuotateCollectiveExclusionRows(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy, bool isAutomatic, int temporalId, List<AUTHMO.AuthorizationRequest> authorizationRequests)
        {
            List<Risk> risks = new List<Risk>();
            foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows)
            {
                try
                {
                    PendingOperation pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase()) // Without Replicated Database
                    {
                        pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    }
                    else // with Replicated Database
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    }

                    if (pendingOperation != null)
                    {
                        CompanyPropertyRisk companyPropertyRisk = new CompanyPropertyRisk();
                        companyPropertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperation.Operation);
                        companyPropertyRisk.Id = pendingOperation.Id;
                        companyPropertyRisk.IsPersisted = true;
                        companyPropertyRisk.CompanyPolicy = companyPolicy;
                        companyPropertyRisk.Status = RiskStatusType.Excluded;
                        //Fecha del riesgo excluido debe ser el menor en la vigencia desde de la póliza
                        DateTime currentFrom = companyPropertyRisk.CompanyRisk.CompanyCoverages.FirstOrDefault().CurrentFrom;
                        companyPolicy.CurrentFrom = currentFrom;
                        if (currentFrom < companyPolicy.CurrentFrom)
                        {
                            companyPolicy.CurrentFrom = currentFrom;
                        }

                        if (companyPropertyRisk.CompanyRisk.CompanyCoverages != null)
                        {
                            companyPropertyRisk.CompanyRisk.CompanyCoverages.ForEach(x =>
                            {
                                x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                x.CurrentTo = companyPolicy.CurrentTo;
                                x.CoverStatus = CoverageStatusType.Excluded;
                            });
                        }

                        CompanyPropertyRisk companyPropertyQuotated = DelegateService.propertyService.QuotateProperty(companyPropertyRisk, true, true);
                        companyPropertyQuotated.InfringementPolicies = DelegateService.propertyService.ValidateAuthorizationPolicies(companyPropertyRisk);

                        companyPolicy.Holder.CompanyName = companyPropertyRisk.CompanyRisk.CompanyInsured.CompanyName;
                        Risk risk = companyPropertyQuotated;
                        risk.Coverages = DelegateService.underwritingService.CreateCoverages(companyPropertyQuotated.CompanyRisk.CompanyCoverages);
                        risks.Add(risk);
                        companyPropertyQuotated.CompanyPolicy = null;
                        pendingOperation.Operation = JsonConvert.SerializeObject(companyPropertyQuotated);
                        pendingOperation.UserId = companyPolicy.UserId;

                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                        }
                        else
                        {
                            string pendingOperationJson = JsonConvert.SerializeObject(pendingOperation);
                            QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                            
                        }

                        collectiveEmissionRow.Risk.RiskId = companyPropertyRisk.Id;
                        collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                        collectiveEmissionRow.Premium = companyPropertyRisk.Premium;
                        collectiveEmissionRow.HasEvents = companyPropertyRisk.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;

                        if (collectiveEmissionRow.HasEvents.Value)
                        {
                            companyPropertyRisk.Policy = companyPolicy;
                            authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesRisk(companyPropertyRisk, collectiveEmission));
                        }
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = "No se encuentra el temporal" + KeySettings.ReportErrorSeparatorMessage();
                    }

                }
                catch (Exception ex)
                {
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                    collectiveEmissionRow.HasError = true;
                    string[] messages = ex.Message.Split('|');
                    collectiveEmissionRow.Observations += string.Format(Errors.ErrorQuotate, messages[0]);
                }
                finally
                {
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                    DataFacadeManager.Dispose();
                }
            }

            return risks;
        }
    }
}