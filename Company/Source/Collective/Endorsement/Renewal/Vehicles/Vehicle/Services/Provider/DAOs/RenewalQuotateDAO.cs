using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
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

namespace Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.DAOs
{
    using Core.Application.RulesScriptsServices.Enums;
    using Sistran.Core.Application.UnderwritingServices.Models;

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
                    companyPolicy.Summary.RiskCount = collectiveLoadProcesses.Count;

                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);

                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyPolicy);

                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperation);
                    }


                    this.QuotateCollectiveEmissionRows(collectiveEmission, collectiveLoadProcesses, companyPolicy);
                }
                else
                {
                    collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
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

        private void QuotateCollectiveEmissionRows(CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy)
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

        private void QuotateCollectiveEmissionRow(CollectiveEmission collectiveEmission, CollectiveEmissionRow collectiveLoadProcess, CompanyPolicy companyPolicy, ref int riskNum, int hierarchy, List<int> ruleToValidateRisk, List<int> ruleToValidateCoverage)
        {
            CompanyVehicle companyVehicle = new CompanyVehicle();

            try
            {
                PendingOperation riskPendingOperation;
                if (!Settings.UseReplicatedDatabase()) // Without Replicated Database
                {
                    riskPendingOperation = DelegateService.utilitiesService.GetPendingOperationById(collectiveLoadProcess.Risk.RiskId);
                }
                else // with Replicated Database
                {
                    riskPendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveLoadProcess.Risk.RiskId);
                }

                companyVehicle = DelegateService.vehicleService.GetCompanyVehicleByRiskId(collectiveLoadProcess.Risk.Id);

                if (riskPendingOperation != null && companyVehicle != null)
                {

                    companyVehicle.Risk.Policy = companyPolicy;
                    companyVehicle.Risk.Id = riskPendingOperation.Id;
                    companyVehicle.Risk.IsPersisted = true;
                    companyVehicle = DelegateService.vehicleService.QuotateVehicle(companyVehicle, true, true, 0);


                    if (companyVehicle.Risk.Premium > 0)
                    {
                        companyVehicle.Risk.Policy = new CompanyPolicy()
                        {
                            Id = companyPolicy.Id,
                            Endorsement = companyPolicy.Endorsement
                        };
                        companyVehicle.Risk.Number = GetNextNumerator(ref riskNum);
                        riskPendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(companyVehicle);
                        riskPendingOperation.UserId = companyPolicy.UserId;
                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.utilitiesService.UpdatePendingOperation(riskPendingOperation);
                        }
                        else
                        {
                            string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}",
                                JsonConvert.SerializeObject(riskPendingOperation),
                                (char)007, JsonConvert.SerializeObject(collectiveLoadProcess),
                                (char)007, JsonConvert.SerializeObject(collectiveEmission.Id),
                                (char)007, JsonConvert.SerializeObject(collectiveEmission.User),
                                (char)007, hierarchy,
                                (char)007, JsonConvert.SerializeObject(ruleToValidateRisk),
                                (char)007, JsonConvert.SerializeObject(ruleToValidateCoverage),
                                (char)007, nameof(CompanyVehicle),
                                (char)007, nameof(CollectiveEmissionRow));

                            QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                        }
                    }
                    else
                    {
                        collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Events;
                        collectiveLoadProcess.HasError = true;
                        collectiveLoadProcess.Observations = Errors.ErrorPremiumZero + KeySettings.ReportErrorSeparatorMessage();
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                    }
                }
                else
                {
                    collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Events;
                    collectiveLoadProcess.HasError = true;
                    collectiveLoadProcess.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                }
            }
            catch (Exception ex)
            {
                collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Events;
                collectiveLoadProcess.HasError = true;
                collectiveLoadProcess.Observations = ex.Message;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
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
            int riskNum = 1;
            CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetCompanyPolicyToIssueByOperationId(collectiveEmission.TemporalId);
            companyPolicy.UserId = collectiveEmission.User.UserId;
            //Fecha de emisión
            companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
            
            companyPolicy = ValidatePolicyAmounts(emissionRows, companyPolicy);

            companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
            collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
            collectiveEmission.EndorsementId = companyPolicy.Endorsement.Id;

            try
            {
                collectiveEmission.Status = MassiveLoadStatus.Issuing;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                emissionRows.ForEach(emissionRow =>
                {
                    if (emissionRow.Risk == null)
                    {
                        emissionRow.Risk = new Risk();
                    }
                    emissionRow.Risk.Number = riskNum;
                    riskNum++;
                });

                ParallelHelper.ForEach(emissionRows, emissionRow =>
                {
                    try
                    {
                        emissionRow.Status = CollectiveLoadProcessStatus.Issuance;
                        DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);

                        string issuanceRiskJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", COMUT.JsonHelper.SerializeObjectToJson(companyPolicy), (char)007, COMUT.JsonHelper.SerializeObjectToJson(emissionRow), (char)007, nameof(CompanyVehicle), (char)007, nameof(CollectiveEmissionRow));
                        QueueHelper.PutOnQueueJsonByQueue(issuanceRiskJson, "CreatePolicyQuee");
                    }
                    catch (Exception)
                    {

                        DataFacadeManager.Dispose();
                    }
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


        public CompanyPolicy ValidatePolicyAmounts(List<CollectiveEmissionRow> emissionRows, CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy.Summary.RiskCount > 0 && companyPolicy.Summary.RiskCount != emissionRows.Count)
                {
                    ConcurrentBag<CompanyRisk> risks = new ConcurrentBag<CompanyRisk>();

                    ParallelHelper.ForEach(emissionRows, emissionRow =>
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
                            CompanyVehicle vehicle = JsonConvert.DeserializeObject<CompanyVehicle>(pendingOperationRisk.Operation);
                            risks.Add(vehicle.Risk);

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

                    companyPolicy = DelegateService.underwritingService.CalculatePolicyAmounts(companyPolicy, risks.ToList());

                    PendingOperation pendingOperation = new PendingOperation();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(companyPolicy.Id);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(companyPolicy.Id);
                    }


                    pendingOperation.Operation = JsonHelper.SerializeObjectToJson(companyPolicy);

                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperation);
                    }

                }

                return companyPolicy;

            }
            catch (Exception)
            {

                throw new ValidationException(Errors.ValidatePolicyAmounts);
            }

        }
    }
}