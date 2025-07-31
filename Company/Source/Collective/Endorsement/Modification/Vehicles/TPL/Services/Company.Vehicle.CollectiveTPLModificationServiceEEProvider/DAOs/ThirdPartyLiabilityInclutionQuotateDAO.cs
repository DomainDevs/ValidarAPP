using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.Resources;
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
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLModificationServiceEEProvider.DAOs
{
    public class ThirdPartyLiabilityInclutionQuotateDAO
    {
        public void QuotateCollectiveIncluition(MassiveLoad massiveLoad)
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

        private void QuotateCollectiveLoadProcesses(CollectiveEmission collectiveLoad)
        {
            try
            {
                List<AUTHMO.AuthorizationRequest> authorizationRequests = new List<AUTHMO.AuthorizationRequest>();
                List<CollectiveEmissionRow> collectiveLoadProcesses = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Validation);

                if (collectiveLoadProcesses.Count > 0)
                {
                    collectiveLoad.Status = MassiveLoadStatus.Tariffing;
                    collectiveLoad.TotalRows = collectiveLoadProcesses.Count;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                    PendingOperation pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())   // Without Replicated Database
                    {
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveLoad.TemporalId);
                    }
                    else  // with Replicated Database
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveLoad.TemporalId);
                    }
                    if (pendingOperation == null)
                    {
                        throw new ValidationException(Errors.ErrorTemporalNotFound);
                    }
                    CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.IsPersisted = true;
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.UserId = collectiveLoad.User.UserId;
                    //Número de Días
                    companyPolicy.Endorsement.EndorsementDays = companyPolicy.CurrentTo.Subtract(companyPolicy.CurrentFrom).Days;
                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);

                    this.QuotateCollectiveEmissionRows(collectiveLoad, collectiveLoadProcesses, companyPolicy);
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveLoad.Id);
                }
            }
            catch (Exception ex)
            {
                collectiveLoad.Status = MassiveLoadStatus.Tariffed;
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = Errors.ErrorInQuotateCollectiveLoad + ex.Message;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            }

        }


        private void QuotateCollectiveEmissionRows(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy)
        {
            int counter = DelegateService.underwritingService.GetCurrentRiskNumByPolicyId(companyPolicy.Endorsement.PolicyId);
            string key = companyPolicy.Prefix.Id + "," + (int)companyPolicy.Product.CoveredRisk.CoveredRiskType;

            int hierarchy = DelegateService.AuthorizationPoliciesService.GetHierarchyByIdUser(10, collectiveEmission.User.UserId);
            List<int> ruleToValidateRisk = DelegateService.AuthorizationPoliciesService.GetRulesToValidate(10, hierarchy, key, FacadeType.RULE_FACADE_RISK);
            List<int> ruleToValidateCoverage = DelegateService.AuthorizationPoliciesService.GetRulesToValidate(10, hierarchy, key, FacadeType.RULE_FACADE_COVERAGE);

            foreach (var collectiveEmissionRow in collectiveEmissionRows.OrderBy(x => x.RowNumber).ToList())
            {
                this.QuatateCollectiveEmissionRow(collectiveEmission, collectiveEmissionRow, companyPolicy, ref counter, hierarchy, ruleToValidateRisk, ruleToValidateCoverage);
            }
        }
        private int GetNextNumerator(ref int counter)
        {
            return Interlocked.Increment(ref counter);
        }
        private void QuatateCollectiveEmissionRow(CollectiveEmission collectiveEmission, CollectiveEmissionRow collectiveEmissionRow, CompanyPolicy companyPolicy, ref int riskNum, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage)
        {
            CompanyRisk risk = null;
            try
            {
                PendingOperation riskPendingOperation;
                if (!Settings.UseReplicatedDatabase())   // Without Replicated Database
                {
                    riskPendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                }
                else  // with Replicated Database
                {
                    riskPendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                }

                if (riskPendingOperation != null)
                {
                    CompanyTplRisk companyRisk = JsonConvert.DeserializeObject<CompanyTplRisk>(riskPendingOperation.Operation);

                    if (companyRisk != null)
                    {
                        companyRisk.Risk.Id = riskPendingOperation.Id;
                        companyRisk.Risk.IsPersisted = true;
                        companyRisk.Risk.Policy = companyPolicy;
                        companyRisk = DelegateService.tplService.QuotateThirdPartyLiability(companyRisk, true, true);


                        if (companyRisk.Risk.Premium > 0)
                        {
                            risk = companyRisk.Risk;
                            risk.Coverages = companyRisk.Risk.Coverages;
                            companyRisk.Risk.Policy = new CompanyPolicy()
                            {
                                Id = companyPolicy.Id,
                                Endorsement = companyPolicy.Endorsement
                            };
                            companyRisk.Risk.Number = GetNextNumerator(ref riskNum);

                            riskPendingOperation.Operation = JsonConvert.SerializeObject(companyRisk);
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
                string[] messages = ex.Message.Split('|');
                collectiveEmissionRow.Observations += string.Format(Errors.ErrorQuotate, messages[0]);
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
    }
}