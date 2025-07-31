using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.Resources;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.DAOs
{
    public class MassiveLoadProcessLiabilityDAO
    {
        public MassiveLoad QuotateMassiveLoad(int massiveLoadId)
        {
            MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
            if (massiveLoad != null)
            {
                TP.Task.Run(() => QuotateMassiveLoadProcesses(massiveLoad));
            }
            return massiveLoad;
        }

        private void QuotateMassiveLoadProcesses(MassiveLoad massiveLoad)
        {
            List<MassiveEmissionRow> massiveEmissionRows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoad.Id, MassiveLoadProcessStatus.Validation, false, null);
            if (massiveEmissionRows.Count > 0)
            {
                massiveLoad.Status = MassiveLoadStatus.Tariffing;
                massiveLoad.TotalRows = massiveEmissionRows.Count;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                MassiveEmissionRow massiveEmissionRow = massiveEmissionRows.FirstOrDefault();
                //QuotateMassiveLoadProcess(massiveEmissionRow);
                //massiveEmissionRows.RemoveAt(0);
                List<int> packages = DataFacadeManager.GetPackageProcesses(massiveEmissionRows.Count(), "MaxThreadMassive");
                foreach (int package in packages)
                {
                    List<MassiveEmissionRow> packageMassiveEmissionRows = massiveEmissionRows.Take(package).ToList();
                    massiveEmissionRows.RemoveRange(0, package);

                    ParallelHelper.ForEach(massiveEmissionRows, row =>
                    {
                        QuotateMassiveLoadProcess(row, massiveLoad);
                    });
                }
                massiveLoad.Status = MassiveLoadStatus.Tariffed;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
            }
        }

        public void QuotateMassiveLoadProcess(MassiveEmissionRow massiveEmissionRow, MassiveLoad massiveLoad)
        {
            try
            {
                massiveEmissionRow.Status = MassiveLoadProcessStatus.Tariff;
                PendingOperation pendingOperation = DelegateService.commonService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                if (!Settings.UseReplicatedDatabase())   // Without Replicated Database
                {
                    pendingOperation = DelegateService.commonService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                }
                else  // with Replicated Database
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(massiveEmissionRow.Risk.Policy.Id);
                }
                if (pendingOperation != null)
                {
                    CompanyPolicy companyPolicy = new CompanyPolicy();
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.IsPersisted = true;
                    List<PendingOperation> pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(companyPolicy.Id);
                    if (!Settings.UseReplicatedDatabase())   // Without Replicated Database
                    {
                        pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(companyPolicy.Id);
                    }
                    else  // with Replicated Database
                    {
                        pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(companyPolicy.Id);
                    }
                    CompanyLiabilityRisk companyLiability = new CompanyLiabilityRisk();
                    companyLiability = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(pendingOperations.First().Operation);
                    companyLiability.Id = pendingOperations.First().Id;
                    companyLiability.IsPersisted = true;
                    companyLiability.CompanyPolicy = companyPolicy;
                    companyLiability = DelegateService.liabilityService.Quotate(companyLiability, true, true);
                    if (companyLiability.Premium > 0)
                    {
                        Policy policyCore = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                        List<Risk> risks = new List<Risk>();
                        risks.Add(companyLiability);
                        risks[0].Coverages = DelegateService.underwritingService.CreateCoverages(companyLiability.CompanyRisk.CompanyCoverages);
                        policyCore.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policyCore, risks, true);
                        policyCore.Summary = DelegateService.underwritingService.CalculateSummary(policyCore, risks);
                        policyCore.Agencies = DelegateService.underwritingService.CalculateCommissions(policyCore, risks);
                        companyPolicy.PayerComponents = policyCore.PayerComponents;
                        companyPolicy.Summary = policyCore.Summary;
                        companyPolicy.PaymentPlan = policyCore.PaymentPlan;
                        companyPolicy.Agencies = policyCore.Agencies;
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
                            policyCore.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policyCore);
                        }
                        companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
                        if (!Settings.UseReplicatedDatabase())
                        {
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                            companyLiability.InfringementPolicies = DelegateService.liabilityService.ValidateAuthorizationPolicies(companyLiability);
                            companyLiability.CompanyPolicy = null;
                            pendingOperation = pendingOperations.First();
                            pendingOperation.Operation = JsonConvert.SerializeObject(companyLiability);
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                        }
                        else
                        {
                            PendingOperation pendingOperationPolicy = new PendingOperation
                            {
                                UserId = companyPolicy.UserId,
                                Operation = JsonConvert.SerializeObject(companyLiability.CompanyPolicy),
                                Id = companyLiability.CompanyPolicy.Id
                            };
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
                        }
                        //para validar prima
                        massiveEmissionRow.Risk.Policy.DocumentNumber = companyPolicy.Endorsement.TemporalId;
                        massiveEmissionRow.Risk.Description = companyLiability.FullAddress;
                        massiveEmissionRow.Risk.Policy.Summary.FullPremium = companyPolicy.Summary.FullPremium;
                        massiveEmissionRow.TotalCommission = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        massiveEmissionRow.Risk.Policy.PolicyType.Description = companyPolicy.PolicyType.Description;
                        massiveEmissionRow.Risk.Policy.Id = companyPolicy.Id;
                        massiveEmissionRow.Status = MassiveLoadProcessStatus.Events;
                        massiveEmissionRow.HasEvents = companyPolicy.InfringementPolicies.Count != 0 || companyLiability.InfringementPolicies.Count != 0;

                        companyLiability.Policy = companyPolicy;
                        DelegateService.massiveService.ValidateAuthorizationPolicies(companyLiability, massiveLoad);
                    }
                    else
                    {
                        massiveEmissionRow.HasError = true;
                        massiveEmissionRow.Observations = Errors.ErrorPremiumZero + KeySettings.ReportErrorSeparatorMessage();
                    }
                    massiveEmissionRow.Risk.Description = companyLiability.FullAddress;
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
                massiveEmissionRow.Observations = ex.Message;
                DelegateService.massiveUnderwritingService.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
    }
}