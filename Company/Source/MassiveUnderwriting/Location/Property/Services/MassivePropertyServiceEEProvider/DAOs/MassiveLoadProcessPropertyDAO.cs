using Newtonsoft.Json;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.Resources;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Managers;
using AUTHMO = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.DAOs
{
    public class MassiveLoadProcessPropertyDAO
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
                    List<AUTHMO.AuthorizationRequest> authorizationRequests = new List<AUTHMO.AuthorizationRequest>();
                    massiveLoad.Status = MassiveLoadStatus.Tariffing;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);

                    //Quitar
                    QuotateMassiveLoadRow(massiveEmissionRows.FirstOrDefault(), massiveLoad, authorizationRequests);
                    massiveEmissionRows.RemoveAt(0);
                    //Quitar

                    ParallelHelper.ForEach(massiveEmissionRows, row =>
                    {
                        QuotateMassiveLoadRow(row, massiveLoad, authorizationRequests);
                    });

                    if (authorizationRequests.Any())
                    {
                        DelegateService.AuthorizationPoliciesService.CreateMassiveAutorizationRequest(authorizationRequests);
                    }

                    massiveLoad.Status = MassiveLoadStatus.Tariffed;
                    DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                }
            }
            catch (Exception ex)
            {
                massiveLoad.HasError = true;
                massiveLoad.ErrorDescription = string.Format(Errors.ErrorQuotateMassiveLoad, ex.Message);
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
        private void QuotateMassiveLoadRow(MassiveEmissionRow massiveEmissionRow, MassiveLoad massiveLoad, List<AUTHMO.AuthorizationRequest> authorizationRequests)
        {
            try
            {
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Tariff;
                var pendingOperation = new PendingOperation();
                if (!Settings.UseReplicatedDatabase())
                {
                    /* Without Replicated Database */
                    pendingOperation = DelegateService.commonService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
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
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(companyPolicy.Id);
                        /* Without Replicated Database */
                    }
                    else
                    {
                        /* with Replicated Database */
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(companyPolicy.Id);
                        /* with Replicated Database */
                    }

                    CompanyPropertyRisk companyProperty = new CompanyPropertyRisk();
                    companyProperty = JsonConvert.DeserializeObject<CompanyPropertyRisk>(pendingOperations.First().Operation);
                    companyProperty.Id = pendingOperations.First().Id;
                    companyProperty.IsPersisted = true;
                    companyProperty.CompanyPolicy = companyPolicy;

                    companyProperty = DelegateService.propertyService.QuotateProperty(companyProperty, true, true);

                    if (companyProperty.Premium > 0)
                    {
                        List<Risk> risks = new List<Risk>();

                        risks.Add(companyProperty);
                        risks[0].Coverages = DelegateService.underwritingService.CreateCoverages(companyProperty.CompanyRisk.CompanyCoverages);
                        companyPolicy.PayerComponents = DelegateService.underwritingService.CompanyCalculatePayerComponents(companyPolicy, risks, true);
                        companyPolicy.ListFirstPayComponent = DelegateService.underwritingService.GetListFirstPayComponentByFinancialPlanId(companyPolicy.PaymentPlan.Id);
                        Policy corePolicy = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                        companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(corePolicy, risks);

                        companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(corePolicy, risks);

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
                            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(corePolicy);
                        }

                        if (!Settings.UseReplicatedDatabase())
                        {
                            /* Without Replicated Database */
                            //companyProperty.CompanyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyProperty.CompanyPolicy);
                            companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);

                            pendingOperation.Operation = JsonConvert.SerializeObject(companyProperty.CompanyPolicy);
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);

                            //companyProperty = DelegateService.propertyService.CreatePropertyTemporal(companyProperty);
                            companyProperty.InfringementPolicies = DelegateService.propertyService.ValidateAuthorizationPolicies(companyProperty);
                            companyProperty.CompanyPolicy = null;

                            pendingOperation = pendingOperations.First();
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyProperty);
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                        }
                        else
                        {
                            companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
                            /* with Replicated Database */
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

                        massiveEmissionRow.Risk.Policy.Summary.FullPremium = companyPolicy.Summary.FullPremium;
                        massiveEmissionRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        massiveEmissionRow.Risk.Policy.PolicyType.Description = companyPolicy.PolicyType.Description;
                        massiveEmissionRow.Status = MassiveLoadProcessStatus.Events;

                        if (companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Restrictive))
                        {
                            massiveEmissionRow.HasError = true;
                            massiveEmissionRow.Observations = string.Format(Errors.PoliciesRestrictive, string.Join(KeySettings.ReportErrorSeparatorMessage(), companyPolicy.InfringementPolicies.Where(x => x.Type == TypePolicies.Restrictive).Select(x => x.Message).ToList()));
                        }
                        else
                        {
                            massiveEmissionRow.HasEvents = companyPolicy.InfringementPolicies.Any(x => x.Type == TypePolicies.Authorization);

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
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations = Errors.ErrorPremiumZero + KeySettings.ReportErrorSeparatorMessage();
                    }

                    massiveEmissionRow.Risk.Description = companyProperty.FullAddress;
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
                string[] messages = ex.Message.Split('|');
                massiveEmissionRow.HasError = true;
                massiveEmissionRow.Observations = string.Format(Errors.ErrorQuotateMassiveLoad, messages[0]);
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
    }
}