using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.CollectiveTPLRenewalServiceEEProvider.Resources;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.Utilities.Enums;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLRenewalServiceEEProvider.DAOs
{
    public class RenewalQuotateDAO
    {
        public void QuotateCollectiveRenewal(MassiveLoad massiveLoad)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoad.Id, false);
            collectiveEmission.User.UserId = massiveLoad.User.UserId;
            List<AUTHMO.UserGroupModel> userGroup = new List<AUTHMO.UserGroupModel>();
            userGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(massiveLoad.User.UserId).Select(x => new AUTHMO.UserGroupModel { UserId = massiveLoad.User.UserId, GroupId = x.GroupId }).ToList();
            if (collectiveEmission != null)
            {
                Task.Run(() => QuotateCollectiveRenewal(collectiveEmission, userGroup));
            }
        }

        private void QuotateCollectiveRenewal(CollectiveEmission collectiveEmission, List<AUTHMO.UserGroupModel> userGroup)
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
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmission.TemporalId);
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
                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);

                    this.QuotateCollectiveEmissionRows(collectiveEmission, collectiveLoadProcesses, companyPolicy, authorizationRequests);
                    //if (collectiveEmission.IsAutomatic)
                    //{
                    //    companyPolicy = DelegateService.underwritingService.CalculatePolicyAmounts(companyPolicy, risks);
                    //    companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
                    //    PendingOperation policyPendingOperation;
                    //    if (!Settings.UseReplicatedDatabase())
                    //    {
                    //        policyPendingOperation = DelegateService.utilitiesService.GetPendingOperationById(companyPolicy.Id);
                    //    }
                    //    else
                    //    {
                    //        policyPendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(companyPolicy.Id);
                    //    }
                    //    policyPendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy);
                    //    policyPendingOperation.UserId = companyPolicy.UserId;
                    //    if (!Settings.UseReplicatedDatabase())
                    //    {
                    //        DelegateService.utilitiesService.UpdatePendingOperation(policyPendingOperation);
                    //    }
                    //    else
                    //    {
                    //        DelegateService.pendingOperationEntityService.UpdatePendingOperation(policyPendingOperation);
                    //    }
                    //}

                    //collectiveEmission.Premium = companyPolicy.Summary.FullPremium;
                    //collectiveEmission.Commiss = companyPolicy.Summary.FullPremium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;

                    //if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                    //{

                    //    throw new Exception(string.Format(Errors.PoliciesRestrictive, "</br>" + string.Join("</br>", companyPolicy.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                    //}

                    //if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization))
                    //{
                    //    authorizationRequests.AddRange(DelegateService.collectiveService.ValidateAuthorizationPoliciesPolicy(companyPolicy.InfringementPolicies, collectiveEmission, companyPolicy.Id));

                    //    collectiveEmission.HasEvents = true;
                    //}

                    //if (authorizationRequests.Any())
                    //{
                    //    DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests);
                    //}
                    //else
                    //{
                    //    collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                    //    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                    //}

                    //DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
            }
            catch (Exception e)
            {
                collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = string.Format(Errors.ErrorInQuotateCollectiveLoad, $"</br>{e.Message}");
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
        }

        private void QuotateCollectiveEmissionRows(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy, List<AUTHMO.AuthorizationRequest> authorizationRequests)
        {
            int counter = 0;
            string key = companyPolicy.Prefix.Id + "," + (int)companyPolicy.Product.CoveredRisk.CoveredRiskType;

            int hierarchy = DelegateService.AuthorizationPoliciesService.GetHierarchyByIdUser(10, collectiveEmission.User.UserId);
            List<int> ruleToValidateRisk = DelegateService.AuthorizationPoliciesService.GetRulesToValidate(10, hierarchy, key, FacadeType.RULE_FACADE_RISK);
            List<int> ruleToValidateCoverage = DelegateService.AuthorizationPoliciesService.GetRulesToValidate(10, hierarchy, key, FacadeType.RULE_FACADE_COVERAGE);

            foreach (var collectiveEmissionRow in collectiveEmissionRows.OrderBy(x => x.RowNumber).ToList())
            {
                this.QuotateCollectiveEmissionRow(collectiveEmission, collectiveEmissionRow, companyPolicy, ref counter, hierarchy, ruleToValidateRisk, ruleToValidateCoverage);
            }
        }

        private int GetNextNumerator(ref int counter)
        {
            return Interlocked.Increment(ref counter);
        }

        private void QuotateCollectiveEmissionRow(CollectiveEmission collectiveEmission, CollectiveEmissionRow collectiveEmissionRow, CompanyPolicy companyPolicy, ref int riskNum, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage)
        {
            CompanyTplRisk companyTpl = new CompanyTplRisk();

            try
            {
                PendingOperation riskPendingOperation;
                if (!Settings.UseReplicatedDatabase()) // Without Replicated Database
                {
                    riskPendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmissionRow.Risk.RiskId);
                }
                else // with Replicated Database
                {
                    riskPendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmissionRow.Risk.RiskId);
                }

                companyTpl = DelegateService.tplService.GetCompanyTplRiskByRiskId(collectiveEmissionRow.Risk.Id);

                if (riskPendingOperation != null && companyTpl != null)
                {

                    companyTpl.Risk.Policy = companyPolicy;
                    companyTpl.Risk.Id = riskPendingOperation.Id;
                    companyTpl.Risk.IsPersisted = true;
                    companyTpl = DelegateService.tplService.QuotateThirdPartyLiability(companyTpl, true, true);


                    if (companyTpl.Risk.Premium > 0)
                    {
                        companyTpl.Risk.Coverages = companyTpl.Risk.Coverages;
                        companyTpl.Risk.Policy = new CompanyPolicy()
                        {
                            Id = companyPolicy.Id,
                            Endorsement = companyPolicy.Endorsement
                        };
                        companyTpl.Risk.Number = GetNextNumerator(ref riskNum);
                        riskPendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyTpl);
                        riskPendingOperation.UserId = companyPolicy.UserId;
                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.utilitiesService.UpdatePendingOperation(riskPendingOperation);
                        }
                        else
                        {
                            string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}",
                                JsonConvert.SerializeObject(riskPendingOperation),
                                (char)007, JsonConvert.SerializeObject(collectiveEmissionRow),
                                (char)007, JsonConvert.SerializeObject(collectiveEmission.Id),
                                (char)007, JsonConvert.SerializeObject(collectiveEmission.User),
                                (char)007, hierarchy,
                                (char)007, JsonConvert.SerializeObject(ruleToValidateRisk),
                                (char)007, JsonConvert.SerializeObject(ruleToValidateCoverage),
                                (char)007, nameof(CompanyTplRisk),
                                (char)007, nameof(CollectiveEmissionRow));

                            QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                        }
                    }
                    else
                    {
                        collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = Errors.ErrorPremiumZero + KeySettings.ReportErrorSeparatorMessage();
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                    }
                }
                else
                {
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                    collectiveEmissionRow.HasError = true;
                    collectiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
            }
            catch (Exception ex)
            {
                collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                collectiveEmissionRow.HasError = true;
                collectiveEmissionRow.Observations = ex.Message;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }

        public MassiveLoad IssuanceCollectiveRenewal(MassiveLoad collectiveLoad)
        {
            if (collectiveLoad != null)
            {
                CollectiveEmission colectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
                colectiveEmission.User.UserId = collectiveLoad.User.UserId;
                List<CollectiveEmissionRow> massiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Events);

                //if (massiveEmissionRows.Count > 0)
                if (massiveEmissionRows.Count(x => x.HasEvents == true) > 0)
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.PolicyHasEvents;
                }
                else
                {
                    Task.Run(() => IssuanceCollectiveRenewalRows(collectiveLoad, colectiveEmission, massiveEmissionRows));
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
            companyPolicy.UserId = collectiveEmission.User.UserId;
            //Fecha de emisión
            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
            companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
            collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
            collectiveEmission.EndorsementId = companyPolicy.Endorsement.Id;

            try
            {
                collectiveEmission.Status = MassiveLoadStatus.Issuing;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                ParallelHelper.ForEach(emissionRows, emissionRow =>
                {
                    emissionRow.Status = CollectiveLoadProcessStatus.Issuance;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);

                    string issuanceRiskJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", COMUT.JsonHelper.SerializeObjectToJson(companyPolicy), (char)007, COMUT.JsonHelper.SerializeObjectToJson(emissionRow), (char)007, nameof(CompanyTplRisk), (char)007, nameof(CollectiveEmissionRow));
                    QueueHelper.PutOnQueueJsonByQueue(issuanceRiskJson, "CreatePolicyQuee");
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
            DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Collective, collectiveEmission.Id.ToString(), null, collectiveEmission.EndorsementId.ToString());
        }
    }
}