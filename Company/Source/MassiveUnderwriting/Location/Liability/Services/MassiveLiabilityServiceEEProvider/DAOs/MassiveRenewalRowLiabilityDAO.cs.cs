using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.Resources;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.DAOs
{
    public class MassiveRenewalRowLiabilityDAO
    {
        public MassiveLoad QuotateMassiveRenewal(int massiveLoadId)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
            if (massiveLoad != null)
            {
                massiveLoad.Status = MassiveLoadStatus.Tariffing;
                List<MassiveRenewalRow> massiveRenewalRow = DelegateService.massiveRenewalService.GetMassiveLoadProcessByMassiveRenewalProcessId(massiveLoadId, MassiveLoadProcessStatus.Validation);
                massiveLoad.TotalRows = massiveRenewalRow.Count;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                TP.Task.Run(() =>
                {
                    List<MassiveRenewalRow> emissionRows = massiveRenewalRow;
                    if (emissionRows.Any())
                    {
                        MassiveRenewalRow MassiveRenewalRow = emissionRows.FirstOrDefault();
                        ExecuteQuotateLiability(MassiveRenewalRow, massiveLoad);
                        emissionRows.RemoveAt(0);
                        if (emissionRows.Count > 0)
                        {
                            ParallelHelper.ForEach(emissionRows, row =>
                            {
                                ExecuteQuotateLiability(row, massiveLoad);
                            });
                        }
                    }
                    massiveLoad.Status = MassiveLoadStatus.Tariffed;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                });
            }
            return massiveLoad;
        }

        private void ExecuteQuotateLiability(MassiveRenewalRow massiveRenewalRow, MassiveLoad massiveLoad)
        {
            if (!massiveRenewalRow.TemporalId.HasValue || massiveRenewalRow.TemporalId.Value <= 0)
            {
                return;
            }
            try
            {
                var pendingOperation = new PendingOperation();
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.commonService.GetPendingOperationById(massiveRenewalRow.TemporalId.Value);// Without Replicated Database
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
                    var pendingOperations = new List<PendingOperation>();
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(companyPolicy.Id);// Without Replicated Database
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(companyPolicy.Id);
                        /* with Replicated Database */
                    }
                    if (pendingOperations.Any())
                    {
                        CompanyLiabilityRisk companyLiability = new CompanyLiabilityRisk();
                        companyLiability = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(pendingOperations.First().Operation);
                        companyLiability.Id = pendingOperations.First().Id;
                        companyLiability.IsPersisted = true;
                        companyLiability.CompanyPolicy = companyPolicy;
                        companyLiability = DelegateService.liabilityService.Quotate(companyLiability, true, true);
                        List<Risk> risks = new List<Risk>();
                        Risk risk = companyLiability;
                        risk.Coverages = DelegateService.underwritingService.CreateCoverages(companyLiability.CompanyRisk.CompanyCoverages);
                        risks.Add(risk);
                        //risks.Add(companyLiability);                        
                        if (companyPolicy.PaymentPlan == null)
                        {
                            throw new ValidationException(Errors.ErrorPaymentPlanIsNull);
                        }
                        Policy policyCore = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                        companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policyCore, risks, true);
                        companyPolicy.ListFirstPayComponent = DelegateService.underwritingService.GetListFirstPayComponentByFinancialPlanId(companyPolicy.PaymentPlan.Id);
                        companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policyCore, risks);

                        companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(policyCore, risks);
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
                            companyPolicy.PayerPayments = DelegateService.underwritingService.CalculatePayerPayment(companyPolicy, companyRequestEndorsement.IsOpenEffect, companyRequestEndorsement.CurrentFrom, companyRequestEndorsement.CurrentTo);
                            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotasWithrequestGroupig(companyPolicy.PayerPayments);
                        }
                        else
                        {
                            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policyCore);
                        }
                        //companyProperty.CompanyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyProperty.CompanyPolicy);
                        companyLiability.CompanyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyLiability.CompanyPolicy);

                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* without Replicated Database */
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyLiability.CompanyPolicy);
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                            //companyProperty = DelegateService.propertyService.CreatePropertyTemporal(companyProperty);
                            companyLiability.InfringementPolicies = DelegateService.liabilityService.ValidateAuthorizationPolicies(companyLiability);
                            companyLiability.CompanyPolicy = null;
                            pendingOperation = pendingOperations.First();
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyLiability);
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                            /*without Replicated Database */
                        }
                        else
                        {
                            /* with Replicated Database */
                            companyLiability.CompanyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyLiability.CompanyPolicy);
                            PendingOperation pendingOperationPolicy = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyLiability.CompanyPolicy),
                                Id = companyLiability.CompanyPolicy.Id
                            };
                            //companyProperty = DelegateService.propertyService.CreatePropertyTemporal(companyProperty);
                            companyLiability.InfringementPolicies = DelegateService.liabilityService.ValidateAuthorizationPolicies(companyLiability);

                            companyLiability.CompanyPolicy = null;

                            PendingOperation pendingOperationRisk = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyLiability),
                                Id = companyLiability.Id
                            };

                            string pendingOperationJson = string.Format("{0}{1}{2}", JsonConvert.SerializeObject(pendingOperationPolicy), (char)007, JsonConvert.SerializeObject(pendingOperationRisk));
                            var queue = new Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee");
                            queue.PutOnQueue(pendingOperationJson);
                            /* with Replicated Database */
                        }
                        massiveRenewalRow.Risk.Policy.Summary.FullPremium = companyPolicy.Summary.FullPremium;
                        massiveRenewalRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        massiveRenewalRow.Risk.Policy.PolicyType.Description = companyPolicy.PolicyType.Description;
                        massiveRenewalRow.Risk.Policy.Id = companyPolicy.Id;

                        companyLiability.Policy = companyPolicy;
                        DelegateService.massiveService.ValidateAuthorizationPolicies(companyLiability, massiveLoad);
                    }
                    else
                    {
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations = "RiskNotFound" + KeySettings.ReportErrorSeparatorMessage();
                    }
                }
                else
                {
                    massiveRenewalRow.HasError = true;
                    massiveRenewalRow.Observations = "TemporalNotFound" + KeySettings.ReportErrorSeparatorMessage();
                }
            }
            catch (Exception ex)
            {
                massiveRenewalRow.HasError = true;
                string error = string.Empty;
                if (ex is ValidationException)
                {
                    error = ex.Message;
                }
                else if (ex is BusinessException)
                {
                    var baseException = ex.GetBaseException();
                    if (baseException is ValidationException || baseException is BusinessException)
                    {
                        error = ex.Message;
                    }
                }
                if (string.IsNullOrEmpty(error))
                {
                    error = StringHelper.ConcatenateString(Errors.ErrorTariffing, "|", ex.Message);
                }
                massiveRenewalRow.Observations = error;
            }
            finally
            {
                massiveRenewalRow.Status = MassiveLoadProcessStatus.Tariff;
                DelegateService.massiveRenewalService.UpdateMassiveRenewalRow(massiveRenewalRow);
                DataFacadeManager.Dispose();
            }
        }
    }
}