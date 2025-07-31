using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Framework;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System.Diagnostics;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Application.ProductServices.Models;
using System.Collections.Concurrent;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.Assemblers;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider.DAOs
{
    public class MassiveLoadRenewalRowVehicleDAO
    {
        public MassiveLoad QuotateMassiveLoad(MassiveLoad massiveLoad)
        {
            if (massiveLoad != null)
            {
                try
                {
                    TP.Task.Run(() => QuotateMassiveLoadProcesses(massiveLoad));
                }
                catch (Exception ex)
                {
                    massiveLoad.ErrorDescription = ex.Message;
                    massiveLoad.Status = MassiveLoadStatus.Tariffed;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }

            return massiveLoad;
        }

        private void QuotateMassiveLoadProcesses(MassiveLoad massiveLoad)
        {
            try
            {
                List<MassiveRenewalRow> massiveRenewalRows = DelegateService.massiveRenewal.GetMassiveLoadProcessByMassiveRenewalProcessId(massiveLoad.Id, MassiveLoadProcessStatus.Validation);

                if (massiveRenewalRows.Count > 0)
                {
                    massiveLoad.Status = MassiveLoadStatus.Tariffing;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                    ConcurrentBag<AUTHMO.AuthorizationRequest> authorizationRequests = new ConcurrentBag<AUTHMO.AuthorizationRequest>();

                    List<AUTHMO.UserGroupModel> userGroup = new List<AUTHMO.UserGroupModel>();
                    userGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(massiveLoad.User.UserId).Select(x => new AUTHMO.UserGroupModel { UserId = massiveLoad.User.UserId, GroupId = x.GroupId }).ToList();

                    MassiveRenewalRow massiveRenewalRow = massiveRenewalRows.FirstOrDefault();
                    QuotateMassiveLoadProcess(massiveRenewalRow, massiveLoad, authorizationRequests, userGroup);
                    massiveRenewalRows.RemoveAt(0);

                    ParallelHelper.ForEach(massiveRenewalRows, row =>
                    {
                        QuotateMassiveLoadProcess(row, massiveLoad, authorizationRequests, userGroup);
                    });

                    if (authorizationRequests.Any())
                    {
                        DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests.ToList());
                    }
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

        public void QuotateMassiveLoadProcess(MassiveRenewalRow massiveRenewalRow, MassiveLoad massiveLoad, /*List<Sinister2G>*/  ConcurrentBag<AUTHMO.AuthorizationRequest> authorizationRequests, List<AUTHMO.UserGroupModel>userGroup)
        {
            try
            {
                PendingOperation pendingOperation = new PendingOperation();
                if (!Settings.UseReplicatedDatabase())
                {
                    /* Without Replicated Database */
                    pendingOperation = DelegateService.utilitiesService.GetPendingOperationById(massiveRenewalRow.TemporalId.Value);
                    /* Without Replicated Database */
                }
                else
                {
                    /* with Replicated Database */
                    pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(massiveRenewalRow.TemporalId.Value);
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
                    companyPolicy = companyVehicle.Risk.Policy;
                    companyVehicle = DelegateService.vehicleService.QuotateVehicle(companyVehicle, true, true,0);
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
                        massiveRenewalRow.HasEvents = true;
                    }
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    risks.Add(companyVehicle.Risk);

                    risks[0].Coverages = companyVehicle.Risk.Coverages;
                    companyPolicy = DelegateService.underwritingService.CalculatePolicyAmounts(companyPolicy, risks);
                    companyPolicy = DelegateService.underwritingService.ValidateApplyPremiumFinance(companyPolicy, companyVehicle.Risk.MainInsured);
                    if (companyPolicy.Request != null
                        && companyPolicy.Request.Id > 0 && companyPolicy.Request.BillingGroupId > 0)
                    {

                        CompanyRequest companyRequest = DelegateService.massiveService.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(companyPolicy.Request.BillingGroupId, companyPolicy.Request.Id.ToString(), null).FirstOrDefault();
                        CompanyRequestEndorsement companyRequestEndorsement = DelegateService.massiveService.GetCompanyRequestEndorsmentPolicyWithRequest(companyPolicy.CurrentFrom, companyRequest);

                        if (companyRequestEndorsement == null)
                        {
                            throw new ValidationException(Errors.CompanyRequestNotRenewed);
                        }

                        companyPolicy.PayerPayments = DelegateService.underwritingService.CalculatePayerPayment(companyPolicy, companyRequestEndorsement.IsOpenEffect, companyRequestEndorsement.CurrentFrom, companyRequestEndorsement.CurrentTo);
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotasWithrequestGroupig(companyPolicy.PayerPayments);
                    }
                    else
                    {
                        companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO { PlanId = companyPolicy.PaymentPlan.Id, CurrentFrom = companyPolicy.CurrentFrom, IssueDate = companyPolicy.IssueDate, ComponentValueDTO = ModelAssembler.CreateCompanyComponentValueDTO(companyPolicy.Summary) });
                    }
                    companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(companyPolicy, risks);
                    foreach (var agency in companyPolicy.Agencies)
                    {
                        ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(agency.Agent.IndividualId, agency.Id, companyPolicy.Product.Id);
                        if (agency.Commissions == null)
                        {
                            continue;
                        }
                        foreach (var commission in agency.Commissions)
                        {
                            commission.Percentage = productAgencyCommiss.CommissPercentage;
                            commission.PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault();
                        }
                    }

                    companyVehicle.Risk.InfringementPolicies = DelegateService.vehicleService.ValidateAuthorizationPolicies(companyVehicle);

                    massiveRenewalRow.Risk.Policy.Summary.FullPremium = companyPolicy.Summary.FullPremium;
                    massiveRenewalRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                    massiveRenewalRow.Risk.Policy.PolicyType.Description = companyPolicy.PolicyType.Description;
                    massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;
                    massiveRenewalRow.Risk.Description = companyVehicle.LicensePlate;
                    massiveRenewalRow.Status = MassiveLoadProcessStatus.Events;

                    if (companyVehicle.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                    {
                        throw new Exception(string.Format(Errors.PoliciesRestrictive + "</br>", string.Join("</br>", companyVehicle.Risk.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => "*" + x.Message).ToList())));
                    }
                    if (companyVehicle.Risk.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization))
                    {
                        List<AUTHMO.PoliciesAut> policiesAuts = new List<AUTHMO.PoliciesAut>();
                        policiesAuts.AddRange(companyVehicle.Risk.InfringementPolicies);
                        List<AUTHMO.AuthorizationRequest> authorizationsRq = (DelegateService.massiveService.ValidateAuthorizationPolicies(policiesAuts, massiveLoad, companyPolicy.Id));
                        authorizationsRq.ForEach(u => authorizationRequests.Add(u));
                        massiveRenewalRow.HasEvents = true;
                    }

                    companyPolicy = DelegateService.underwritingService.SaveTableTemporal(companyPolicy);
                    companyVehicle.Risk.Policy = companyPolicy;
                    companyVehicle = DelegateService.vehicleService.CompanySaveCompanyVehicleTemporal(companyVehicle);
                    if (!Settings.UseReplicatedDatabase())
                    {
                        /* without Replicated Database */
                        pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                        DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperation);
                        companyVehicle.Risk.Policy = null;
                        pendingOperation = pendingOperations.First();
                        pendingOperation.Operation = JsonConvert.SerializeObject(companyVehicle);
                        DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperation);
                        companyVehicle.Risk.Policy = companyPolicy;
                        /* without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        PendingOperation pendingOperationPolicy = new PendingOperation
                        {
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyVehicle.Risk.Policy),
                            Id = companyVehicle.Risk.Policy.Id
                        };

                        companyVehicle.Risk.InfringementPolicies = DelegateService.authorizationPoliciesService.ValidateInfringementPolicies(companyVehicle.Risk.InfringementPolicies);
                        companyVehicle.Risk.Policy = null;

                        PendingOperation pendingOperationRisk = new PendingOperation
                        {
                            UserId = companyPolicy.UserId,
                            Operation = JsonConvert.SerializeObject(companyVehicle),
                            Id = companyVehicle.Risk.Id
                        };

                        string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(pendingOperationRisk), (char)007, JsonConvert.SerializeObject(massiveRenewalRow), (char)007, nameof(MassiveRenewalRow));
                        QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                    }

                }
                else
                {
                    massiveRenewalRow.Status = MassiveLoadProcessStatus.Tariff;
                    massiveRenewalRow.HasError = true;
                    massiveRenewalRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('|');
                massiveRenewalRow.HasError = true;
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Tariff;
                massiveRenewalRow.Observations = messages[0];
                DelegateService.massiveRenewal.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        public MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad)
        {
            if (massiveLoad != null)
            {
                List<MassiveRenewalRow> massiveRenewalRow = DelegateService.massiveRenewal.GetMassiveLoadProcessByMassiveRenewalProcessId(massiveLoad.Id, MassiveLoadProcessStatus.Events);

                if (massiveRenewalRow.Count > 0)
                {
                    TP.Task.Run(() => IssuanceRenewalMassiveEmissionRows(massiveLoad, massiveRenewalRow));
                }
                else
                {
                    massiveLoad.HasError = true;
                    massiveLoad.ErrorDescription = Errors.PolicyHasEvents;

                }
            }

            return massiveLoad;
        }

        public void IssuanceRenewalMassiveEmissionRows(MassiveLoad massiveLoad, List<MassiveRenewalRow> massiveRenewalRows)
        {
            try
            {

                massiveLoad.Status = MassiveLoadStatus.Issuing;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                List<CompanyPolicy> companyPolicies = DelegateService.massiveUnderwritingService.GetCompanyPoliciesToIssueByOperationIds(massiveRenewalRows.Where(x => !x.HasError.Value).Select(x => x.Risk.Policy.Id).ToList());

                ParallelHelper.ForEach(companyPolicies, companyPolicy =>
                {
                    MassiveRenewalRow massiveRenewalRow = massiveRenewalRows.FirstOrDefault(x => x.Risk.Policy.Id == companyPolicy.Id);

                    massiveRenewalRow.Status = MassiveLoadProcessStatus.Issuance;
                    DelegateService.massiveRenewal.UpdateMassiveRenewalRow(massiveRenewalRow);
                    //Fecha de emisión
                    companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

                    PendingOperation pendingOperationPolicy = new PendingOperation();
                    pendingOperationPolicy.Id = companyPolicy.Id;
                    pendingOperationPolicy.UserId = massiveLoad.User.UserId;
                    pendingOperationPolicy.Operation = JsonConvert.SerializeObject(companyPolicy);


                    string issuanceJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(massiveRenewalRow), (char)007, nameof(CompanyVehicle), (char)007, nameof(MassiveRenewalRow));
                    QueueHelper.PutOnQueueJsonByQueue(issuanceJson, "CreatePolicyQuee");
                    });
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.ToString(), EventLogEntryType.Error);
                string[] messages = ex.Message.Split('|');
                massiveLoad.ErrorDescription = messages[0];
                massiveLoad.Status = MassiveLoadStatus.Issued;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
            DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(TypeFunction.Massive, massiveLoad.Id.ToString(), null, null);
        }


        private void ExecuteCreatePolicy(CompanyPolicy companyPolicy, MassiveRenewalRow massiveRenewalRow)
        {
            try
            {
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Issuance;
                if (massiveRenewalRow.Risk.Policy.Id > 0)
                {
                    massiveRenewalRow.Status = MassiveLoadProcessStatus.Finalized;

                    List<PendingOperation> pendingOperations;
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(massiveRenewalRow.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(massiveRenewalRow.Risk.Policy.Id);
                    }
                    List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();
                    foreach (PendingOperation po in pendingOperations)
                    {
                        var companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(po.Operation);
                        companyVehicle.Risk.Policy = companyPolicy;
                        companyVehicles.Add(companyVehicle);
                    }
                    companyPolicy = DelegateService.vehicleService.CreateEndorsement(companyPolicy, companyVehicles);
                    UpdateJSONPolicyAndRecordEndorsementOperation(massiveRenewalRow, companyPolicy);

                    massiveRenewalRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                    massiveRenewalRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;

                }
                else
                {
                    massiveRenewalRow.HasError = true;
                    massiveRenewalRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }
                DelegateService.massiveRenewal.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;
                massiveRenewalRow.Observations = string.Format(Errors.ErrorIssuing, ex.Message);
                DelegateService.massiveRenewal.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
            //DelegateService.AuthorizationPoliciesService.UpdateProcessIdByKeyKey2(massiveRenewalRow.Id.ToString(), null, massiveRenewalRow.TemporalId.ToString());
        }

        private void UpdateJSONPolicyAndRecordEndorsementOperation(MassiveRenewalRow massiveRenewalRow, CompanyPolicy companyPolicy)
        {
            var pendingOperation = new PendingOperation();
            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(massiveRenewalRow.TemporalId.Value);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperation);
                DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, massiveRenewalRow.TemporalId.Value);
                /* Without Replicated Database */
            }
            else
            {
                /* with Replicated Database */
                pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(massiveRenewalRow.TemporalId.Value);
                PendingOperation pendingOperationPolicy = new PendingOperation
                {
                    UserId = companyPolicy.UserId,
                    Operation = JsonConvert.SerializeObject(companyPolicy),
                    Id = companyPolicy.Id
                };

                string pendingOperationJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, companyPolicy.Endorsement.Id, (char)007, companyPolicy.Id, (char)007, nameof(MassiveRenewalRow));
                QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                /* with Replicated Database */
            }
        }
    }
}