using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.Resources;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Company.Application.SyBaseEntityService.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.DAOs
{
    public class MassiveRenewalRowPropertyDAO
    {
        public MassiveLoad QuotateMassiveRenewal(MassiveLoad massiveLoad)
        {
            if (massiveLoad != null)
            {
                massiveLoad.Status = MassiveLoadStatus.Tariffing;
                List<MassiveRenewalRow> massiveRenewalRow = DelegateService.massiveRenewalService.GetMassiveLoadProcessByMassiveRenewalProcessId(massiveLoad.Id, MassiveLoadProcessStatus.Validation);
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                TP.Task.Run(() =>
                {
                    List<MassiveRenewalRow> emissionRows = massiveRenewalRow;
                    List<AUTHMO.AuthorizationRequest> authorizationRequests = new List<AUTHMO.AuthorizationRequest>();
                    List<Sinister2G> sinisters2G = null;

                    if (Settings.ImplementValidate2G())
                    {
                        sinisters2G = DelegateService.sybaseEntityService.GetSinisters2GByMassiveLoad(massiveLoad.Id);
                    }

                    if (emissionRows.Any())
                    {
                        MassiveRenewalRow MassiveRenewalRow = emissionRows.FirstOrDefault();
                        ExecuteQuotateProperty(MassiveRenewalRow, massiveLoad, sinisters2G, authorizationRequests);
                        emissionRows.RemoveAt(0);
                        if (emissionRows.Count > 0)
                        {
                            ParallelHelper.ForEach(emissionRows, row =>
                            {
                                ExecuteQuotateProperty(row, massiveLoad, sinisters2G, authorizationRequests);
                            });
                        }
                    }

                    if (authorizationRequests.Any())
                    {
                        DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests);
                    }

                    massiveLoad.Status = MassiveLoadStatus.Tariffed;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                });
            }
            return massiveLoad;
        }

        private void ExecuteQuotateProperty(MassiveRenewalRow massiveRenewalRow, MassiveLoad massiveLoad, List<Sinister2G> sinisters2G, List<AUTHMO.AuthorizationRequest> authorizationRequests)
        {
            massiveRenewalRow.Risk.Policy.Endorsement.Id = 0;
            if (!massiveRenewalRow.TemporalId.HasValue || massiveRenewalRow.TemporalId.Value <= 0)
            {
                return;
            }
            try
            {
                var pendingOperation = new PendingOperation();
                if (!Settings.UseReplicatedDatabase())
                {
                    /* Without Replicated Database */
                    pendingOperation = DelegateService.commonService.GetPendingOperationById(massiveRenewalRow.TemporalId.Value);
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
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(companyPolicy.Id);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(companyPolicy.Id);
                        /* with Replicated Database */
                    }
                    if (pendingOperations.Any())
                    {
                        CompanyPropertyRisk companyProperty = new CompanyPropertyRisk();
                        companyProperty = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperations.First().Operation);
                        companyProperty.Id = pendingOperations.First().Id;
                        companyProperty.IsPersisted = true;
                        companyProperty.CompanyPolicy = companyPolicy;

                        if (Settings.ImplementValidate2G() && sinisters2G != null)
                        {
                            companyPolicy.PortfolioBalance = sinisters2G.Find(x => x.RowId == massiveRenewalRow.Id).PendingDebit;
                        }

                        companyProperty = DelegateService.propertyService.QuotateProperty(companyProperty, true, true);

                        List<Risk> risks = new List<Risk>();
                        risks.Add(companyProperty);
                        risks[0].Coverages = DelegateService.underwritingService.CreateCoverages(companyProperty.CompanyRisk.CompanyCoverages);

                        companyPolicy.PayerComponents = DelegateService.underwritingService.CompanyCalculatePayerComponents(companyPolicy, risks, true);
                        if (companyPolicy.PaymentPlan == null)
                        {
                            throw new ValidationException(Errors.ErrorPaymentPlanIsNull);
                        }

                        companyPolicy.ListFirstPayComponent = DelegateService.underwritingService.GetListFirstPayComponentByFinancialPlanId(companyPolicy.PaymentPlan.Id);
                        Policy corePolicy = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                        companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(corePolicy, risks);

                        companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(corePolicy, risks);
                        foreach (var agency in companyPolicy.Agencies)
                        {
                            ProductAgencyCommiss productAgencyCommiss = DelegateService.underwritingService.GetCommissByAgentIdAgencyIdProductId(agency.Agent.IndividualId, agency.Id, companyPolicy.CompanyProduct.Id);
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
                            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(corePolicy);
                        }

                        //companyProperty.CompanyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyProperty.CompanyPolicy);
                        companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyProperty.CompanyPolicy);

                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* without Replicated Database */
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyProperty.CompanyPolicy);
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);

                            //companyProperty = DelegateService.propertyService.CreatePropertyTemporal(companyProperty);
                            companyProperty.InfringementPolicies = DelegateService.propertyService.ValidateAuthorizationPolicies(companyProperty);

                            companyProperty.CompanyPolicy = null;
                            pendingOperation = pendingOperations.First();
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyProperty);
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);

                            /*without Replicated Database */
                        }
                        else
                        {
                            /* with Replicated Database */
                            companyProperty.CompanyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyProperty.CompanyPolicy);
                            PendingOperation pendingOperationPolicy = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyProperty.CompanyPolicy),
                                Id = companyProperty.CompanyPolicy.Id
                            };
                            //companyProperty = DelegateService.propertyService.CreatePropertyTemporal(companyProperty);
                            companyProperty.InfringementPolicies = DelegateService.propertyService.ValidateAuthorizationPolicies(companyProperty);

                            companyProperty.CompanyPolicy = null;

                            PendingOperation pendingOperationRisk = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyProperty),
                                Id = companyProperty.Id
                            };

                            string pendingOperationJson = string.Format("{0}{1}{2}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(pendingOperationRisk));
                            var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee", serialization: "JSON");
                            queue.PutOnQueue(pendingOperationJson);
                            /* with Replicated Database */
                        }

                        DelegateService.underwritingService.CreateTempSubscription(companyPolicy);

                        massiveRenewalRow.Risk.Policy.Summary.FullPremium = companyPolicy.Summary.FullPremium;
                        massiveRenewalRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        massiveRenewalRow.Risk.Policy.PolicyType.Description = companyPolicy.PolicyType.Description;
                        massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;
                        massiveRenewalRow.Status = MassiveLoadProcessStatus.Events;

                        if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                        {
                            massiveRenewalRow.HasError = true;
                            massiveRenewalRow.Observations = string.Format(Errors.PoliciesRestrictive, string.Join(KeySettings.ReportErrorSeparatorMessage(), companyPolicy.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => x.Message).ToList()));
                        }
                        else
                        {
                            massiveRenewalRow.HasEvents = companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization);

                            companyProperty.Policy = companyPolicy;
                            if ((companyProperty.Policy.InfringementPolicies != null && companyProperty.Policy.InfringementPolicies.Any())
                                || (companyProperty.InfringementPolicies != null && companyProperty.InfringementPolicies.Any()))
                            {
                                authorizationRequests.AddRange(DelegateService.massiveService.ValidateAuthorizationPolicies(companyProperty, massiveLoad));
                            }
                        }

                    }
                    else
                    {
                        massiveRenewalRow.Status = MassiveLoadProcessStatus.Tariff;
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations = Errors.ErrorRiskNotFound + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                else
                {
                    massiveRenewalRow.Status = MassiveLoadProcessStatus.Tariff;
                    massiveRenewalRow.HasError = true;
                    massiveRenewalRow.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                }

                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('|');
                massiveRenewalRow.HasError = true;
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Tariff;
                massiveRenewalRow.Observations = string.Format(Errors.ErrorQuotateMassiveLoad, messages[0]);
                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
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
                List<MassiveRenewalRow> massiveRenewalRow = DelegateService.massiveRenewalService.GetMassiveLoadProcessByMassiveRenewalProcessId(massiveLoad.Id, MassiveLoadProcessStatus.Events);

                if (massiveRenewalRow.Count > 0)
                {
                    TP.Task.Run(() => IssuanceRenewalMassiveEmissionRows(massiveLoad, massiveRenewalRow));
                }
                else
                {
                    massiveLoad.HasError = true;
                    massiveLoad.ErrorDescription = Errors.ErrorRecordsNotFoundToIssue;
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
                    DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
                    //Fecha de emisión
                    companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);

                    PendingOperation pendingOperationPolicy = new PendingOperation();
                    pendingOperationPolicy.Id = companyPolicy.Id;
                    pendingOperationPolicy.UserId = massiveLoad.User.UserId;
                    pendingOperationPolicy.Operation = JsonConvert.SerializeObject(companyPolicy);

                    string issuanceJson = string.Format("{0}{1}{2}{3}{4}{5}{6}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(massiveRenewalRow), (char)007, nameof(CompanyPropertyRisk), (char)007, nameof(MassiveRenewalRow));
                    var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("CreatePolicyQuee", routingKey: "CreatePolicyQuee", serialization: "JSON");
                    queue.PutOnQueue(issuanceJson);
                });

            }
            catch (Exception ex)
            {
                massiveLoad.HasError = true;
                massiveLoad.Status = MassiveLoadStatus.Issued;
                massiveLoad.ErrorDescription = string.Format(Errors.ErrorIssuing, ex.Message);
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
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
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(massiveRenewalRow.Risk.Policy.Id);
                    }
                    else
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(massiveRenewalRow.Risk.Policy.Id);
                    }
                    List<CompanyPropertyRisk> companyPropertyRisks = new List<CompanyPropertyRisk>();
                    foreach (PendingOperation po in pendingOperations)
                    {
                        CompanyPropertyRisk companyPropertyRisk = JsonConvert.DeserializeObject<CompanyPropertyRisk>(po.Operation);
                        companyPropertyRisk.CompanyPolicy = companyPolicy;
                        companyPropertyRisks.Add(companyPropertyRisk);
                    }
                    companyPolicy = DelegateService.propertyService.CreateEndorsement(companyPolicy, companyPropertyRisks);
                    UpdateJSONPolicyAndRecordEndorsementOperation(massiveRenewalRow, companyPolicy);

                    massiveRenewalRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                    massiveRenewalRow.Risk.Policy.Endorsement.Id = companyPolicy.Endorsement.Id;

                }
                else
                {
                    massiveRenewalRow.HasError = true;
                    massiveRenewalRow.Observations = Errors.ErrorTemporalNotFound;
                }
                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
            }

            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;
                massiveRenewalRow.Observations = string.Format(Errors.ErrorIssuing, ex.Message);
                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }

        private void UpdateJSONPolicyAndRecordEndorsementOperation(MassiveRenewalRow massiveRenewalRow, CompanyPolicy companyPolicy)
        {
            var pendingOperation = new PendingOperation();
            if (!Settings.UseReplicatedDatabase())
            {
                /* Without Replicated Database */
                pendingOperation = DelegateService.commonService.GetPendingOperationById(massiveRenewalRow.TemporalId.Value);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                DelegateService.commonService.UpdatePendingOperation(pendingOperation);
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
                var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee", serialization: "JSON");
                queue.PutOnQueue(pendingOperationJson);
                /* with Replicated Database */
            }
        }
    }
}
