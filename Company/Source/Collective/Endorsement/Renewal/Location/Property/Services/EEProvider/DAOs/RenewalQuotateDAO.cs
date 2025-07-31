using Newtonsoft.Json;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sistran.Company.Application.Location.CollectivePropertyRenewalServiceEEProvider.Resources;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Framework.BAF;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.CollectivePropertyRenewalServiceEEProvider.DAOs
{
    public class RenewalQuotateDAO
    {
        public void QuotateCollectiveRenewal(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, false);
            collectiveEmission.User.UserId = massiveLoad.User.UserId;
            if (collectiveEmission != null)
            {
                TP.Task.Run(() => QuotateCollectiveRenewal(collectiveEmission));
            }

        }

        private void QuotateCollectiveRenewal(CollectiveEmission collectiveEmission)
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
                    List<Risk> risks = QuotateCollectiveEmissionRows(collectiveEmission, collectiveLoadProcesses, companyPolicy, authorizationRequests);
                    if (collectiveEmission.IsAutomatic)
                    {
                        companyPolicy.PayerComponents = DelegateService.underwritingService.CompanyCalculatePayerComponents(companyPolicy, risks, true);
                        Policy policyCore = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                        companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policyCore, risks);
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policyCore);
                        companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(policyCore, risks);
                        companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
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
                    DelegateService.underwritingService.CreateTempSubscription(companyPolicy);
                    collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                    collectiveEmission.Premium = companyPolicy.Summary.FullPremium;
                    collectiveEmission.Commiss = companyPolicy.Summary.FullPremium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;

                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                    {
                        throw new Exception(string.Format(Errors.PoliciesRestrictive, "</br>" + string.Join("</br>", companyPolicy.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                    }

                    collectiveEmission.HasEvents = companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization);
                    if (collectiveEmission.HasEvents.Value)
                    {
                        authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesPolicy(companyPolicy, collectiveEmission));
                    }

                    if (authorizationRequests.Any())
                    {
                        DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests);
                    }

                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('|');
                collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = string.Format(Errors.ErrorInQuotateCollectiveRenewal, $"</br> {messages[0]}");
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
        }

        private List<Risk> QuotateCollectiveEmissionRows(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy, List<AUTHMO.AuthorizationRequest> authorizationRequests)
        {
            int counter = 0;
            ConcurrentBag<Risk> risks = new ConcurrentBag<Risk>();

            //// Se ajecuta la tarifación de uno antes de hacerlo en paralelo para evitar problemas de concurrencia
            //var firstCollectiveEmissionRow = collectiveEmissionRows[0];
            //Risk firstRisk = QuotateColectiveEmissionRow(firstCollectiveEmissionRow, companyPolicy, ref counter);
            //if (firstRisk != null)
            //{
            //    risks.Add(firstRisk);
            //}
            //collectiveEmissionRows.Remove(firstCollectiveEmissionRow);

            foreach (var collectiveEmissionRow in collectiveEmissionRows)
            //ParallelHelper.ForEach(collectiveEmissionRows, collectiveEmissionRow =>
            {
                Risk risk = QuotateCollectiveEmissionRow(collectiveEmission, collectiveEmissionRow, companyPolicy, ref counter, authorizationRequests);
                if (risk != null)
                {
                    risks.Add(risk);
                }
            }//);
            return risks.ToList();
        }

        private int GetNextNumerator(ref int counter)
        {
            return Interlocked.Increment(ref counter);
        }


        private Risk QuotateCollectiveEmissionRow(CollectiveEmission collectiveEmission, CollectiveEmissionRow collectiveEmissionRow, CompanyPolicy companyPolicy, ref int riskNum, List<AUTHMO.AuthorizationRequest> authorizationRequests)
        {
            Risk risk = null;
            try
            {
                PendingOperation riskPendingOperation;
                if (!Settings.UseReplicatedDatabase()) // Without Replicated Database
                {
                    riskPendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmissionRow.Risk.RiskId);
                }
                else // with Replicated Database
                {
                    riskPendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmissionRow.Risk.RiskId);
                }
                if (riskPendingOperation != null)
                {
                    var companyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(riskPendingOperation.Operation);
                    companyRisk.Id = riskPendingOperation.Id;
                    companyRisk.IsPersisted = true;
                    companyRisk.CompanyPolicy = companyPolicy;
                    companyRisk = DelegateService.propertyService.QuotateProperty(companyRisk, true, true);
                    companyRisk.InfringementPolicies = DelegateService.propertyService.ValidateAuthorizationPolicies(companyRisk);

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

                        collectiveEmissionRow.Risk.Description = companyRisk.FullAddress;
                        collectiveEmissionRow.Risk.Id = companyRisk.Id;
                        collectiveEmissionRow.Premium = companyRisk.Premium;
                        if (companyRisk.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                        {
                            throw new Exception(string.Format(Errors.PoliciesRestrictive, string.Join("|", companyRisk.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => x.Message).ToList())));
                        }

                        collectiveEmissionRow.HasEvents = companyRisk.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization);
                        if (collectiveEmissionRow.HasEvents.Value)
                        {
                            companyRisk.Policy = companyPolicy;
                            authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesRisk(companyRisk, collectiveEmission));
                        }
                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.ErrorPremiumZero + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                else
                {
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }

            }
            catch (Exception ex)
            {

                collectiveEmissionRow.HasError = true;
                collectiveEmissionRow.Observations = ex.Message;

            }
            finally
            {
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                DataFacadeManager.Dispose();
            }
            return risk;
        }


        public MassiveLoad IssuanceCollectiveRenewal(MassiveLoad collectiveLoad)
        {
            if (collectiveLoad != null)
            {
                CollectiveEmission colectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
                colectiveEmission.User.UserId = collectiveLoad.User.UserId;
                List<CollectiveEmissionRow> colectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Events);

                if (colectiveEmissionRows.Count(x => x.HasEvents == false) > 0)
                {
                    TP.Task.Run(() => IssuanceCollectiveRenewalRows(collectiveLoad, colectiveEmission, colectiveEmissionRows));
                }
                else
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.ErrorRecordsNotFoundToIssue;
                }
            }
            return collectiveLoad;
        }

        public void IssuanceCollectiveRenewalRows(MassiveLoad collectiveLoad, CollectiveEmission colectiveEmission, List<CollectiveEmissionRow> massiveEmissionRows)
        {
            try
            {
                collectiveLoad.Status = MassiveLoadStatus.Issuing;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                ExecuteCreatePolicy(massiveEmissionRows, colectiveEmission);
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
            CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetCompanyPolicyToIssueByOperationId(collectiveEmission.TemporalId);
            //Fecha de emisión
            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

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
                companyPolicy.UserId = collectiveEmission.User.UserId;

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

            try
            {
                collectiveEmission.Status = MassiveLoadStatus.Issuing;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                ParallelHelper.ForEach(emissionRows.Where(x => x.HasEvents == false).ToList(), emissionRow =>
                {
                    emissionRow.Status = CollectiveLoadProcessStatus.Issuance;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);

                    PendingOperation pendingOperationCompanyRisk = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperationCompanyRisk = DelegateService.commonService.GetPendingOperationById(emissionRow.Risk.RiskId);
                    }
                    else
                    {
                        pendingOperationCompanyRisk = DelegateService.pendingOperationEntityService.GetPendingOperationById(emissionRow.Risk.RiskId);
                    }

                    CompanyPropertyRisk companyPropertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperationCompanyRisk.Operation);
                    companyPropertyRisk.CompanyPolicy = companyPolicy;
                    pendingOperationCompanyRisk.Operation = JsonConvert.SerializeObject(companyPropertyRisk);

                    string issuanceRiskJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationCompanyRisk), (char)007, JsonConvert.SerializeObject(emissionRow), (char)007, nameof(CompanyPropertyRisk), (char)007, nameof(CollectiveEmissionRow));
                    var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePolicyQuee", routingKey: "CreatePolicyQuee", serialization: "JSON");
                    queue.PutOnQueue(issuanceRiskJson);
                });
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
