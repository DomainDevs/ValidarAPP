using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.Resources;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
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
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.DAOs
{
    public class CollectiveLoadProcessThirdPartyLiabilityDAO
    {
        public void QuotateCollectiveLoad(List<int> collectiveLoadIds)
        {
            foreach (int collectiveLoadId in collectiveLoadIds)
            {
                CollectiveEmission collectiveLoad = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoadId, false);

                if (collectiveLoad != null)
                {
                    try
                    {
                        TP.Task.Run(() => QuotateCollectiveLoadProcesses(collectiveLoad));
                    }
                    catch (Exception ex)
                    {
                        collectiveLoad.ErrorDescription = ex.Message;
                        DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                    }
                    finally
                    {
                        DataFacadeManager.Dispose();
                    }
                }
            }
        }

        public void QuotateMassiveCollectiveEmission(MassiveLoad massiveLoad)
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

        public CollectiveEmissionRow GetCollectiveEmissionRowById(int id)
        {
            CollectiveEmissionRow collectiveEmissionRow = new CollectiveEmissionRow();
            collectiveEmissionRow = DelegateService.collectiveService.GetCollectiveEmissionRowById(id);
            return collectiveEmissionRow;
        }

        private void QuotateCollectiveLoadProcesses(CollectiveEmission collectiveLoad)
        {
            try
            {
                List<CollectiveEmissionRow> collectiveLoadProcesses = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Validation);
                if (collectiveLoadProcesses != null && collectiveLoadProcesses.Count > 0)
                {
                    collectiveLoad.Status = MassiveLoadStatus.Tariffing;
                    collectiveLoad.TotalRows = collectiveLoadProcesses.Count;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);

                    PendingOperation pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveLoad.TemporalId);
                    }
                    else
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
                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);

                    this.QuotateCollectiveEmissionRows(collectiveLoad, collectiveLoadProcesses, companyPolicy);
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(collectiveLoad.Id);
                }
            }
            catch (Exception e)
            {
                collectiveLoad.Status = MassiveLoadStatus.Tariffed;
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = string.Format(Errors.ErrorInQuotateCollectiveLoad, $"</br>{e.Message}");
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void QuotateCollectiveEmissionRows(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy)
        {
            int counter = 0;
            string key = companyPolicy.Prefix.Id + "," + (int)companyPolicy.Product.CoveredRisk.CoveredRiskType;

            int hierarchy = DelegateService.AuthorizationPoliciesService.GetHierarchyByIdUser(10, collectiveEmission.User.UserId);
            List<int> ruleToValidateRisk = DelegateService.AuthorizationPoliciesService.GetRulesToValidate(10, hierarchy, key, 
                FacadeType.RULE_FACADE_RISK);
            List<int> ruleToValidateCoverage = DelegateService.AuthorizationPoliciesService.GetRulesToValidate(10, hierarchy, key, FacadeType.RULE_FACADE_COVERAGE);

            foreach (CollectiveEmissionRow collectiveEmissionRow in collectiveEmissionRows.OrderBy(x => x.RowNumber))
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
                        List<IGrouping<int, CompanyCoverage>> coverageDuplicate = companyRisk?.Risk?.Coverages?.GroupBy(x => x.Id).Where(z => z != null && z.Count() > 1)?.ToList();
                        if (coverageDuplicate != null && coverageDuplicate.Any())
                        {
                            throw new Exception(Errors.ErrorCoverageDuplicate);
                        }
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
                            companyRisk.Risk.Number = this.GetNextNumerator(ref riskNum);
                            riskPendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyRisk);
                            riskPendingOperation.UserId = companyPolicy.UserId;
                            if (!Settings.UseReplicatedDatabase())
                            {
                                DelegateService.utilitiesService.UpdatePendingOperation(riskPendingOperation);
                            }
                            else
                            {
                                string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}",
                                    COMUT.JsonHelper.SerializeObjectToJson(riskPendingOperation),
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveEmissionRow),
                                    (char)007, collectiveEmission.Id,
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(collectiveEmission.User),
                                    (char)007, hierarchy,
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(ruleToValidateRisk),
                                    (char)007, COMUT.JsonHelper.SerializeObjectToJson(ruleToValidateCoverage),
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
                risk = null;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveLoad)
        {
            if (collectiveLoad != null)
            {
                CollectiveEmission colectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
                colectiveEmission.User.UserId = collectiveLoad.User.UserId;
                List<CollectiveEmissionRow> colectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Events);

                if (colectiveEmissionRows.Count(x => x.HasEvents == true) > 0)
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.PolicyHasEvents;
                }
                else
                {
                    TP.Task.Run(() => this.IssuanceCollectiveEmission(colectiveEmission, colectiveEmissionRows));

                }
            }
            return collectiveLoad;
        }



        public void IssuanceCollectiveEmission(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> colectiveEmissionRows)
        {
            try
            {
                collectiveEmission.Status = MassiveLoadStatus.Issuing;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                this.ExecuteCreatePolicy(colectiveEmissionRows, collectiveEmission);
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('|');
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = messages[0];
                collectiveEmission.Status = MassiveLoadStatus.Issued;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void ExecuteCreatePolicy(List<CollectiveEmissionRow> emissionRows, CollectiveEmission collectiveEmission)
        {
            CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetCompanyPolicyToIssueByOperationId(collectiveEmission.TemporalId);
            companyPolicy.UserId = collectiveEmission.User.UserId;
            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

            if (emissionRows.Any(x => x.HasEvents == true))
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();

                ParallelHelper.ForEach(emissionRows.Where(x => x.HasEvents == false).ToList(), emissionRow =>
                {
                    try
                    {
                        PendingOperation pendingOperationRisk = new PendingOperation();
                        if (!Settings.UseReplicatedDatabase())
                        {
                            pendingOperationRisk = DelegateService.utilitiesService.GetPendingOperationById(emissionRow.Risk.Id);
                        }
                        else
                        {
                            pendingOperationRisk = DelegateService.pendingOperationEntityService.GetPendingOperationById(emissionRow.Risk.RiskId);
                        }

                        CompanyRisk risk = new CompanyRisk();
                        risk = JsonConvert.DeserializeObject<CompanyRisk>(pendingOperationRisk.Operation);
                        risks.Add(risk);
                    }
                    catch (Exception ex)
                    {
                        emissionRow.HasError = true;
                        emissionRow.Observations = ex.Message;
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
                    }
                    finally
                    {
                        DataFacadeManager.Dispose();
                    }
                });

                companyPolicy = DelegateService.underwritingService.CalculatePolicyAmounts(companyPolicy, risks);

                PendingOperation pendingOperation = new PendingOperation();
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveEmission.TemporalId);
                }
                else
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmission.TemporalId);
                }
                pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy);
                DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
            }

            companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
            collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
            collectiveEmission.EndorsementId = companyPolicy.Endorsement.Id;

            if (!companyPolicy.Endorsement.EndorsementType.HasValue)
            {
                throw new ValidationException(Errors.ErrorPolicyEndorsementTypeIsNull);
            }

            collectiveEmission.Status = MassiveLoadStatus.Issuing;
            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

            emissionRows = emissionRows.Where(x => x.HasEvents == false).ToList().OrderBy(x => x.RowNumber).ToList();
            int riskNum = 1;
            foreach (CollectiveEmissionRow emissionRow in emissionRows.Where(x => x.HasEvents == false).ToList().OrderBy(x => x.RowNumber).ToList())
            {
                try
                {
                    if (emissionRow.Risk == null)
                    {
                        emissionRow.Risk = new Risk();
                    }
                    emissionRow.Risk.Number = riskNum;
                    riskNum++;
                    emissionRow.Status = CollectiveLoadProcessStatus.Issuance;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
                    string issuanceRiskJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", COMUT.JsonHelper.SerializeObjectToJson(companyPolicy), (char)007, COMUT.JsonHelper.SerializeObjectToJson(emissionRow), (char)007, nameof(CompanyTplRisk), (char)007, nameof(CollectiveEmissionRow));
                    QueueHelper.PutOnQueueJsonByQueue(issuanceRiskJson, "CreatePolicyQuee");
                }
                catch (Exception ex)
                {
                    emissionRow.HasError = true;
                    emissionRow.Observations = ex.Message;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }
            DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Collective, collectiveEmission.Id.ToString(), null, collectiveEmission.EndorsementId.ToString());

        }
    }
}