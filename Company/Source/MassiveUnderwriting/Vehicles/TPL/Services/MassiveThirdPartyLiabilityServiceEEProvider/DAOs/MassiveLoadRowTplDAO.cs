using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Company.Application.Vehicles.MassiveTPLServices.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices.EEProvider.DAOs
{
    public class MassiveLoadRowTplDAO
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
                    List<AUTHMO.UserGroupModel> userGroup = new List<AUTHMO.UserGroupModel>();
                    userGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(massiveLoad.User.UserId).Select(x => new AUTHMO.UserGroupModel { UserId = massiveLoad.User.UserId, GroupId = x.GroupId }).ToList();
                    List<AUTHMO.AuthorizationRequest> authorizationRequests = new List<AUTHMO.AuthorizationRequest>();

                    ParallelHelper.ForEach(massiveEmissionRows, row =>
                    {
                        QuotateMassiveLoadRow(row, massiveLoad, authorizationRequests, userGroup);
                    });

                    if (authorizationRequests.Any())
                    {
                        DelegateService.authorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests);
                    }

                    massiveLoad.Status = MassiveLoadStatus.Tariffed;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
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
        public void QuotateMassiveLoadRow(MassiveEmissionRow massiveEmissionRow, MassiveLoad massiveLoad, List<AUTHMO.AuthorizationRequest> authorizationRequests, List<AUTHMO.UserGroupModel> userGroup)
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
                    CompanyTplRisk companyTplRisk = new CompanyTplRisk();
                    companyTplRisk = JsonConvert.DeserializeObject<CompanyTplRisk>(pendingOperations.First().Operation);
                    companyTplRisk.Risk.Id = pendingOperations.First().Id;
                    companyTplRisk.Risk.IsPersisted = true;
                    companyTplRisk.Risk.Policy = companyPolicy;

                    //int limiteRc = companyTplRisk.LimitRc.Id;
                    companyTplRisk = DelegateService.tplService.QuotateThirdPartyLiability(companyTplRisk, true, true);
                    companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);

                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                    {
                        throw new Exception(string.Format(Errors.PoliciesRestrictive + " ", string.Join(" ", companyPolicy.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                    }

                    if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization))
                    {
                        List<AUTHMO.PoliciesAut> policiesAuts = new List<AUTHMO.PoliciesAut>();
                        policiesAuts.AddRange(companyPolicy.InfringementPolicies);
                        authorizationRequests.AddRange(DelegateService.massiveService.ValidateAuthorizationPolicies(policiesAuts, massiveLoad, companyPolicy.Id));

                        massiveEmissionRow.HasEvents = true;
                    }

                    if (companyTplRisk.Risk.Premium > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();

                        risks.Add(companyTplRisk.Risk);
                        risks[0].Coverages = companyTplRisk.Risk.Coverages;
                        companyPolicy = DelegateService.underwritingService.CalculatePolicyAmounts(companyPolicy, risks);

                        companyTplRisk.Risk.InfringementPolicies = DelegateService.tplService.ValidateAuthorizationPolicies(companyTplRisk);
                        massiveEmissionRow.Risk.Policy.Summary.FullPremium = companyPolicy.Summary.FullPremium;
                        massiveEmissionRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        massiveEmissionRow.Risk.Policy.PolicyType.Description = companyPolicy.PolicyType.Description;

                        if (companyTplRisk.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                        {
                            massiveEmissionRow.HasError = true;
                            throw new Exception(string.Format(Errors.PoliciesRestrictive + " ", string.Join(" ", companyTplRisk.Risk.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                        }

                        if (companyTplRisk.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization))
                        {
                            List<AUTHMO.PoliciesAut> policiesAuts = new List<AUTHMO.PoliciesAut>();
                            policiesAuts.AddRange(companyTplRisk.Risk.InfringementPolicies);
                            authorizationRequests.AddRange(DelegateService.massiveService.ValidateAuthorizationPolicies(policiesAuts, massiveLoad, companyPolicy.Id));

                            massiveEmissionRow.HasEvents = true;
                        }

                        massiveEmissionRow.Status = MassiveLoadProcessStatus.Events;

                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* Without Replicated Database */
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                            DelegateService.utilitiesService.UpdatePendingOperation(pendingOperation);

                            companyTplRisk.Risk.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(companyTplRisk.Risk.InfringementPolicies);
                            companyTplRisk.Risk.Policy = null;

                            pendingOperation = pendingOperations.First();
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyTplRisk);
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
                            companyTplRisk.Risk.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(companyTplRisk.Risk.InfringementPolicies);
                            companyTplRisk.Risk.Policy = null;

                            PendingOperation pendingOperationRisk = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyTplRisk),
                                Id = companyTplRisk.Risk.Id,
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
                    companyTplRisk.Risk.Policy = companyPolicy;
                    massiveEmissionRow.Risk.Description = companyTplRisk.LicensePlate;
                    companyTplRisk = DelegateService.tplService.CreateThirdPartyLiabilityTemporal(companyTplRisk, false);
                }
                else
                {
                    massiveEmissionRow.HasError = true;
                    massiveEmissionRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                    DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
                }

                //DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
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

                    string issuanceJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(row), (char)007, nameof(CompanyTplRisk), (char)007, nameof(MassiveEmissionRow));
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