using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.DAOs
{
    public class InclusionQuotateDAO
    {
        public void QuotateCollectiveInclusion(int collectiveEmissionId)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveEmissionId, false);

            if (collectiveEmission != null)
            {
                try
                {
                    TP.Task.Run(() => QuotateCollectiveLoadProcesses(collectiveEmission));
                }
                catch (Exception ex)
                {
                    collectiveEmission.ErrorDescription = ex.Message;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }

        }

        private void QuotateCollectiveLoadProcesses(CollectiveEmission collectiveEmission)
        {
            try
            {
                List<CollectiveEmissionRow> collectiveLoadProcesses = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveEmission.Id, CollectiveLoadProcessStatus.Validation);

                if (collectiveLoadProcesses.Count > 0)
                {
                    collectiveEmission.Status = MassiveLoadStatus.Tariffing;
                    collectiveEmission.TotalRows = collectiveLoadProcesses.Count;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    PendingOperation pendingOperation;
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
                        throw new ValidationException("ErrorTemporalNotFound");
                    }
                    CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.IsPersisted = true;
                    companyPolicy.Id = pendingOperation.Id;
                    //pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                    //DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                    List<Risk> risks = QuotateCollectiveLoadProcesses(collectiveLoadProcesses, companyPolicy, collectiveEmission.IsAutomatic, collectiveEmission.TemporalId);
                    if (risks.Count > 0)
                    {
                        if (collectiveEmission.IsAutomatic)
                        {
                            Policy policy = companyPolicy;
                            policy.Product = companyPolicy.CompanyProduct;
                            
                            companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policy, risks,true);
                            companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policy, risks);
                            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policy);
                            companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(policy, risks);

                            PendingOperation policyPendingOperation;
                            if (!Settings.UseReplicatedDatabase())
                            {
                                policyPendingOperation = DelegateService.commonService.GetPendingOperationById(companyPolicy.Id);
                            }
                            else
                            {
                                policyPendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(companyPolicy.Id);

                            }

                            policyPendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                            policyPendingOperation.UserId = companyPolicy.UserId;
                            if (!Settings.UseReplicatedDatabase())
                            {
                                DelegateService.commonService.UpdatePendingOperation(policyPendingOperation);
                            }
                            else
                            {
                                DelegateService.pendingOperationEntityService.UpdatePendingOperation(policyPendingOperation);
                            }
                        }

                        collectiveEmission.Premium = companyPolicy.Summary.Premium;
                        collectiveEmission.Commiss = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        collectiveEmission.HasEvents = companyPolicy.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;
                    }
                    collectiveEmission.Status = MassiveLoadStatus.Tariffed;
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

        private List<Risk> QuotateCollectiveLoadProcesses(List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy, bool? isAutomatic, int tempId)
        {
            List<Risk> risks = new List<Risk>();

            List<int> packages = DataFacadeManager.GetPackageProcesses(collectiveEmissionRows.Count(), "MaxThreadMassive");

            foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows)
            {
                try
                {
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Tariff;
                    PendingOperation pendingOperation;
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    }
                    if(pendingOperation != null)
                    {
                        CompanyLiabilityRisk companyRisk = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(pendingOperation.Operation);
                        companyRisk.Id = pendingOperation.Id;
                        companyRisk.IsPersisted = true;
                        companyRisk.CompanyPolicy = companyPolicy;
                        companyRisk.CompanyRisk.CompanyInsured.IdentificationDocument = companyPolicy.Holder.IdentificationDocument;
                        companyRisk = DelegateService.liabilityService.Quotate(companyRisk, true, true);
                        companyPolicy.InfringementPolicies = DelegateService.liabilityService.ValidateAuthorizationPolicies(companyRisk);
                        if(companyRisk.Premium > 0)
                        {
                            Risk risk = companyRisk;
                            risk.Coverages = DelegateService.underwritingService.CreateCoverages(companyRisk.CompanyRisk.CompanyCoverages);
                            risks.Add(risk);
                            companyRisk.CompanyPolicy = null;
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyRisk);
                            pendingOperation.UserId = companyPolicy.UserId;
                            if(!Settings.UseReplicatedDatabase())
                            {
                                DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                            }
                            else
                            {
                                string pendingOperationJson = JsonConvert.SerializeObject(pendingOperation);
                                QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                               
                            }
                            collectiveEmissionRow.Risk.Description = companyRisk.FullAddress;
                            collectiveEmissionRow.Risk.Id = companyRisk.Id;
                            collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                            collectiveEmissionRow.HasEvents = companyRisk.InfringementPolicies.Count(p => p.Type != TypePolicies.Notification) != 0;
                            collectiveEmissionRow.Premium = companyRisk.Premium;
                        }
                        else
                        {
                            collectiveEmissionRow.HasError = true;
                            collectiveEmissionRow.Observations = "Prima en 0" + KeySettings.ReportErrorSeparatorMessage();
                        }
                        collectiveEmissionRow.Risk.Description = companyRisk.FullAddress;
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = "No se encuentra el temporal" + KeySettings.ReportErrorSeparatorMessage();
                    }
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
                catch (Exception ex)
                {
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                    collectiveEmissionRow.HasError = true;
                    string[] messages = ex.Message.Split('|');
                    collectiveEmissionRow.Observations += string.Format(Errors.ErrorQuotate, messages[0]);
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
                //});
            }

            return risks;
        }
    }
}
