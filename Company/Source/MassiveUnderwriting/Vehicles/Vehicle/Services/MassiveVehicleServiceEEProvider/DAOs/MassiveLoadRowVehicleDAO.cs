using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.DAOs
{
    public class MassiveLoadRowVehicleDAO
    {
        /// <summary>
        /// Tarifar Cargue
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad QuotateMassiveLoad(MassiveLoad massiveLoad)
        {

            if (massiveLoad != null)
            {
                TP.Task.Run(() => QuotateMassiveLoadRows(massiveLoad));
            }

            return massiveLoad;
        }

        /// <summary>
        /// Tarifar Filas
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        private void QuotateMassiveLoadRows(MassiveLoad massiveLoad)
        {
            try
            {
                List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoad.Id, MassiveLoadProcessStatus.Validation, false, null);

                if (massiveEmissionRows.Count > 0)
                {
                    massiveLoad.Status = MassiveLoadStatus.Tariffing;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                    ConcurrentBag<AUTHMO.AuthorizationRequest> authorizationRequests = new ConcurrentBag<AUTHMO.AuthorizationRequest>();
                    List<AUTHMO.UserGroupModel> userGroup = new List<AUTHMO.UserGroupModel>();
                    userGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(massiveLoad.User.UserId).Select(x => new AUTHMO.UserGroupModel { UserId = massiveLoad.User.UserId, GroupId = x.GroupId }).ToList();

                    MassiveEmissionRow  massiveEmissionRow = massiveEmissionRows.FirstOrDefault();
                    QuotateMassiveLoadRow(massiveEmissionRow, massiveLoad, authorizationRequests, userGroup);
                    massiveEmissionRows.RemoveAt(0);

                    ParallelHelper.ForEach(massiveEmissionRows, row =>
                    {
                        QuotateMassiveLoadRow(row, massiveLoad, authorizationRequests, userGroup);
                    });

                    if (authorizationRequests.Any())
                    {
                        DelegateService.authorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests.ToList());
                    }
                    DelegateService.massiveService.UpdateMassiveLoadStatusIfComplete(massiveLoad.Id);
                }
            }
            catch (Exception ex)
            {
                massiveLoad.Status = MassiveLoadStatus.Tariffed;
                massiveLoad.ErrorDescription = string.Format(Errors.ErrorTariffing, ex.Message);
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        /// <summary>
        /// Tarifar Fila
        /// </summary>
        /// <param name="massiveEmissionRow">Fila</param>
        public void QuotateMassiveLoadRow(MassiveEmissionRow massiveEmissionRow, MassiveLoad massiveLoad, ConcurrentBag<AUTHMO.AuthorizationRequest> authorizationRequests, List<AUTHMO.UserGroupModel> userGroup)
        {
            try
            {
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Tariff;
                var pendingOperation = new PendingOperation();

                if (!Settings.UseReplicatedDatabase())
                {
                    /* Without Replicated Database */
                    pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                    /* Without Replicated Database */
                }
                else
                {
                    /* with Replicated Database */
                    pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                    /* with Replicated Database */
                }
                if (pendingOperation != null)
                {
                    CompanyPolicy companyPolicy = new CompanyPolicy();
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.IsPersisted = true;
                    companyPolicy.UserId = massiveLoad.User.UserId;


                    var pendingOperations = new List<PendingOperation>();

                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* Without Replicated Database */
                        pendingOperations = DelegateService.utilitiesService.GetPendingOperationsByParentId(companyPolicy.Id);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(companyPolicy.Id);
                        /* with Replicated Database */
                    }
                    CompanyVehicle companyVehicle = new CompanyVehicle();
                    companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(pendingOperations.First().Operation);
                    companyVehicle.Risk.Id = pendingOperations.First().Id;
                    companyVehicle.Risk.IsPersisted = true;
                    companyVehicle.Risk.Policy = companyPolicy;
                    companyVehicle.Risk.Number = 1;

                    //int limiteRc = companyVehicle.LimitRc.Id;
                    companyVehicle = DelegateService.vehicleService.QuotateVehicle(companyVehicle, true, true, 0);
                    companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);

                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                    {
                        throw new Exception(string.Format(Errors.PoliciesRestrictive + " ", string.Join(" ", companyPolicy.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                    }

                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization))
                    {
                        List<AUTHMO.PoliciesAut> policiesAuts = new List<AUTHMO.PoliciesAut>();
                        policiesAuts.AddRange(companyPolicy.InfringementPolicies);
                        List<AUTHMO.AuthorizationRequest> authorizationsRq = (DelegateService.massiveService.ValidateAuthorizationPolicies(policiesAuts, massiveLoad, companyPolicy.Id));
                        authorizationsRq.ForEach(u => authorizationRequests.Add(u));
                        massiveEmissionRow.HasEvents = true;
                    }

                    if (companyVehicle.Risk.Premium > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();

                        risks.Add(companyVehicle.Risk);
                        risks[0].Coverages = companyVehicle.Risk.Coverages;
                        companyPolicy = DelegateService.underwritingService.CalculatePolicyAmounts(companyPolicy, risks);
                        companyPolicy = DelegateService.underwritingService.ValidateApplyPremiumFinance(companyPolicy, companyVehicle.Risk.MainInsured);
                        companyVehicle.Risk.InfringementPolicies = DelegateService.vehicleService.ValidateAuthorizationPolicies(companyVehicle);
                        massiveEmissionRow.Risk.Policy.Summary.FullPremium = companyPolicy.Summary.FullPremium;
                        massiveEmissionRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        massiveEmissionRow.Risk.Policy.PolicyType.Description = companyPolicy.PolicyType.Description;


                        if (companyVehicle.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                        {
                            throw new Exception(string.Format(Errors.PoliciesRestrictive + " ", string.Join(" ", companyVehicle.Risk.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                        }
                        if (companyVehicle.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization))
                        {
                            List<AUTHMO.PoliciesAut> policiesAuts = new List<AUTHMO.PoliciesAut>();
                            policiesAuts.AddRange(companyVehicle.Risk.InfringementPolicies);
                            List<AUTHMO.AuthorizationRequest> authorizationsRq = (DelegateService.massiveService.ValidateAuthorizationPolicies(policiesAuts, massiveLoad, companyPolicy.Id));
                            authorizationsRq.ForEach(authorization => authorizationRequests.Add(authorization));
                            massiveEmissionRow.HasEvents = true;
                        }
                        massiveEmissionRow.Status = MassiveLoadProcessStatus.Events;

                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* Without Replicated Database */
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                            DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);

                            companyVehicle.Risk.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(companyVehicle.Risk.InfringementPolicies);
                            companyVehicle.Risk.Policy = null;

                            pendingOperation = pendingOperations.First();
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyVehicle);
                            DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                            /* Without Replicated Database */
                        }
                        else
                        {
                            /* with Replicated Database */
                            PendingOperation pendingOperationPolicy = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyPolicy),
                                Id = companyPolicy.Id,
                                IsMassive = true
                            };
                            companyVehicle.Risk.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(companyVehicle.Risk.InfringementPolicies);
                            companyVehicle.Risk.Policy = null;

                            PendingOperation pendingOperationRisk = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyVehicle),
                                Id = companyVehicle.Risk.Id,
                                IsMassive = true
                            };
                            string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(pendingOperationRisk), (char)007, JsonConvert.SerializeObject(massiveEmissionRow), (char)007, nameof(MassiveEmissionRow));

                            QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                            /* with Replicated Database */
                        }
                    }
                    else
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations = Errors.ErrorPremiumZero + KeySettings.ReportErrorSeparatorMessage();
                        DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                    }
                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);
                    companyVehicle.Risk.Policy = companyPolicy;
                    massiveEmissionRow.Risk.Description = companyVehicle.LicensePlate;
                    companyVehicle = DelegateService.vehicleService.CompanySaveCompanyVehicleTemporal(companyVehicle);

                }
                else
                {
                    massiveEmissionRow.HasError = true;
                    massiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                    DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                }
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('|');
                massiveEmissionRow.HasError = true;
                massiveEmissionRow.Observations = string.Format(Errors.ErrorTariffing, messages[0]);
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad)
        {
            if (massiveLoad != null)
            {
                List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoad.Id, MassiveLoadProcessStatus.Events, false, false);

                if (massiveEmissionRows.Count(x => x.HasEvents == true) > 0)
                {
                    massiveLoad.HasError = true;
                    massiveLoad.ErrorDescription = Errors.PolicyHasEvents;
                }
                else
                {
                    TP.Task.Run(() => IssuanceMassiveEmissionRows(massiveLoad, massiveEmissionRows));
                }
            }

            return massiveLoad;
        }

        public void IssuanceMassiveEmissionRows(MassiveLoad massiveLoad, List<MassiveEmissionRow> massiveEmissionRows)
        {
            try
            {
                massiveLoad.Status = MassiveLoadStatus.Issuing;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                List<CompanyPolicy> companyPolicies = DelegateService.massiveUnderwritingService.GetCompanyPoliciesToIssueByOperationIds(massiveEmissionRows.Where(x => !x.HasError).Select(x => x.Risk.Policy.Id).ToList());

                ParallelHelper.ForEach(companyPolicies, companyPolicy =>
                {
                    MassiveEmissionRow row = massiveEmissionRows.FirstOrDefault(x => x.Risk.Policy.Id == companyPolicy.Id);

                    row.Status = MassiveLoadProcessStatus.Issuance;
                    DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(row);
                    //Fecha de emisión
                    companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

                    PendingOperation pendingOperationPolicy = new PendingOperation();
                    pendingOperationPolicy.Id = companyPolicy.Id;
                    pendingOperationPolicy.UserId = massiveLoad.User.UserId;
                    pendingOperationPolicy.Operation = JsonConvert.SerializeObject(companyPolicy);

                    string issuanceJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(row), (char)007, nameof(CompanyVehicle), (char)007, nameof(MassiveEmissionRow));
                    QueueHelper.PutOnQueueJsonByQueue(issuanceJson, "CreatePolicyQuee");
                });
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
                massiveLoad.ErrorDescription = string.Format(Errors.ErrorIssuing, ex.Message);
                massiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
            DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Massive, massiveLoad.Id.ToString(), null, null);
        }
        private void ExecuteCreatePolicy(CompanyPolicy companyPolicy, MassiveEmissionRow massiveEmissionRow)
        {
            try
            {
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Issuance;
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);

                if (massiveEmissionRow.Risk.Policy.Id > 0)
                {



                    //massiveEmissionRow.Status = MassiveLoadProcessStatus.Finalized;

                    //List<PendingOperation> pendingOperations;
                    //if (!Settings.UseReplicatedDatabase())
                    //{
                    //    pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(companyPolicy.Id);
                    //}
                    //else
                    //{
                    //    pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(companyPolicy.Id);
                    //}
                    //List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
                    //foreach (PendingOperation po in pendingOperations)
                    //{
                    //    var companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(po.Operation);
                    //    companyVehicle.CompanyPolicy = companyPolicy;
                    //    companyVehicles.Add(companyVehicle);
                    //}
                    //companyPolicy = DelegateService.vehicleService.CreateEndorsement(companyPolicy, companyVehicles);
                    //UpdateJSONPolicyAndRecordEndorsementOperation(massiveEmissionRow, companyPolicy);

                    //massiveEmissionRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                    //massiveEmissionRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;

                }
                else
                {
                    massiveEmissionRow.HasError = true;
                    massiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            catch (Exception ex)
            {
                massiveEmissionRow.HasError = true;
                massiveEmissionRow.Observations = string.Format(Errors.ErrorIssuing, ex.Message);
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }

        }

        private void UpdateJSONPolicyAndRecordEndorsementOperation(MassiveEmissionRow massiveEmissionRow, CompanyPolicy companyPolicy)
        {
            var pendingOperation = new PendingOperation();
            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);
                DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, massiveEmissionRow.Risk.Policy.Id);
                /* Without Replicated Database */
            }
            else
            {
                /* with Replicated Database */
                pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                PendingOperation pendingOperationPolicy = new PendingOperation
                {
                    UserId = companyPolicy.UserId,
                    Operation = JsonConvert.SerializeObject(companyPolicy),
                    Id = companyPolicy.Id
                };


                string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, companyPolicy.Endorsement.Id, (char)007, companyPolicy.Id, (char)007, nameof(MassiveEmissionRow));
                QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                /* with Replicated Database */
            }
        }
    }
}