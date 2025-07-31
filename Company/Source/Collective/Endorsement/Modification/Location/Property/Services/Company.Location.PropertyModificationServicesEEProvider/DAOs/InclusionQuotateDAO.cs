using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.PropertyModificationService.EEProvider.DAOs
{
    public class InclusionQuotateDAO
    {
        public void QuotateCollectiveInclusion(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, false);
            collectiveEmission.User.UserId = massiveLoad.User.UserId;

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
                List<AUTHMO.AuthorizationRequest> authorizationRequests = new List<AUTHMO.AuthorizationRequest>();
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
                        throw new ValidationException(Errors.ErrorTemporalNotFound);
                    }
                    CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.IsPersisted = true;
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.UserId = collectiveEmission.User.UserId;

                    //Número de Días
                    companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days;

                    List<Risk> risks = QuotateCollectiveEmissionRows(collectiveEmission, collectiveLoadProcesses, companyPolicy, authorizationRequests);
                    if (risks.Any() && collectiveEmission.IsAutomatic)
                    {
                        companyPolicy.PayerComponents = DelegateService.underwritingService.CompanyCalculatePayerComponents(companyPolicy, risks, true);
                        Policy policyCore = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                        companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policyCore, risks);
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policyCore);
                        companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(policyCore, risks);
                        companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
                        pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                        pendingOperation.UserId = companyPolicy.UserId;
                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                        }
                        else
                        {
                            DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperation);
                        }

                        DelegateService.underwritingService.CreateTempSubscription(companyPolicy);

                        collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                        collectiveEmission.Premium = companyPolicy.Summary.Premium;
                        collectiveEmission.Commiss = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
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
                collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = Errors.ErrorInQuotateCollectiveLoad + ex.Message;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
        }

        private List<Risk> QuotateCollectiveEmissionRows(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy, List<AUTHMO.AuthorizationRequest> authorizationRequests)
        {
            int counter = DelegateService.underwritingService.GetCurrentRiskNumByPolicyId(companyPolicy.Endorsement.PolicyId);
            List<Risk> risks = new List<Risk>();
            foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows)
            {
                Risk risk = QuatateCollectiveEmissionRow(collectiveEmission, collectiveEmissionRow, companyPolicy, ref counter, authorizationRequests);

                if (collectiveEmissionRow.HasError != null && risk != null && collectiveEmissionRow.HasError.Value == false)
                {
                    risks.Add(risk);
                }
            }
            return risks;
        }
        private int GetNextNumerator(ref int counter)
        {
            return Interlocked.Increment(ref counter);
        }
        private Risk QuatateCollectiveEmissionRow(CollectiveEmission collectiveEmission, CollectiveEmissionRow collectiveLoadProcess, CompanyPolicy companyPolicy, ref int riskNum, List<AUTHMO.AuthorizationRequest> authorizationRequests)
        {
            Risk risk = null;
            try
            {
                PendingOperation riskPendingOperation;
                if (!Settings.UseReplicatedDatabase())   // Without Replicated Database
                {
                    riskPendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveLoadProcess.Risk.Id);
                }
                else  // with Replicated Database
                {
                    riskPendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveLoadProcess.Risk.Id);
                }
                if (riskPendingOperation != null)
                {
                    CompanyPropertyRisk companyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(riskPendingOperation.Operation);
                    companyRisk.Id = riskPendingOperation.Id;
                    companyRisk.IsPersisted = true;
                    companyRisk.CompanyPolicy = companyPolicy;
                    companyRisk.CompanyRisk.CompanyInsured.IdentificationDocument = companyPolicy.Holder.IdentificationDocument;
                    companyRisk = DelegateService.propertyService.QuotateProperty(companyRisk, true, true);
                    companyPolicy.InfringementPolicies = DelegateService.propertyService.ValidateAuthorizationPolicies(companyRisk);

                    if (companyRisk.Premium > 0)
                    {
                        risk = companyRisk;
                        risk.Coverages = DelegateService.underwritingService.CreateCoverages(companyRisk.CompanyRisk.CompanyCoverages);
                        companyRisk.CompanyPolicy = null;
                        companyRisk.Number = GetNextNumerator(ref riskNum);
                        riskPendingOperation.Operation = JsonConvert.SerializeObject(companyRisk);
                        riskPendingOperation.UserId = companyPolicy.UserId;
                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.commonService.UpdatePendingOperation(riskPendingOperation);
                        }
                        else
                        {
                            string pendingOperationJson = JsonConvert.SerializeObject(riskPendingOperation);
                            var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee", serialization: "JSON");
                            queue.PutOnQueue(pendingOperationJson);
                        }

                        collectiveLoadProcess.Risk.Description = companyRisk.FullAddress;
                        collectiveLoadProcess.Risk.Id = companyRisk.Id;
                        collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Events;
                        collectiveLoadProcess.HasEvents = companyRisk.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;
                        collectiveLoadProcess.Premium = companyRisk.Premium;

                        if (collectiveLoadProcess.HasEvents.Value)
                        {
                            companyRisk.Policy = companyPolicy;
                            authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesRisk(companyRisk, collectiveEmission));
                        }
                    }
                    else
                    {
                        collectiveLoadProcess.HasError = true;
                        collectiveLoadProcess.Observations = Errors.ErrorPremiumZero + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                else
                {
                    collectiveLoadProcess.HasError = true;
                    collectiveLoadProcess.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }
            }
            catch (Exception ex)
            {
                collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Events;
                collectiveLoadProcess.HasError = true;
                string[] messages = ex.Message.Split('|');
                collectiveLoadProcess.Observations += string.Format(Errors.ErrorQuotate, messages[0]);
            }
            finally
            {
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                DataFacadeManager.Dispose();
            }
            return risk;
        }
    }
}