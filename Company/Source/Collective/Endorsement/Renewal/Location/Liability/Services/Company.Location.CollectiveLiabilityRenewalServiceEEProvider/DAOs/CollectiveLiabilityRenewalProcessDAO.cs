using AutoMapper;
using Company.Location.CollectiveLiabilityRenewalService.EEProvider.Resources;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.CollectiveLiabilityRenewalService.EEProvider;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Company.Location.CollectiveLiabilityRenewalService.EEProvider.DAOs
{
    public class CollectiveLiabilityRenewalProcessDAO
    {
        public void QuotateMassiveCollectiveEmission(int collectiveLoadId)
        {
            CollectiveEmission collectiveLoad = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoadId, false);
            if (collectiveLoad != null)
            {
                try
                {
                    TP.Task.Run(() => QuotateCollectiveEmissionRows(collectiveLoad));
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

        private void QuotateCollectiveEmissionRows(CollectiveEmission collectiveLoad)
        {
            List<CollectiveEmissionRow> collectiveLoadProcesses = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Validation);
            if (collectiveLoadProcesses.Count > 0)
            {
                collectiveLoad.Status = MassiveLoadStatus.Tariffing;
                collectiveLoad.TotalRows = collectiveLoadProcesses.Count;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                PendingOperation pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveLoad.TemporalId);
                CompanyPolicy companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                companyPolicy.IsPersisted = true;
                QuotateCollectiveEmissionRows(collectiveLoadProcesses, companyPolicy, collectiveLoad.IsAutomatic, collectiveLoad.TemporalId);
                collectiveLoad.Status = MassiveLoadStatus.Tariffed;
                collectiveLoad.Premium = companyPolicy.Summary.Premium;
                collectiveLoad.Commiss = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                //collectiveLoad.HasEvents = companyPolicy.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
            }
        }

        private void QuotateCollectiveEmissionRows(List<CollectiveEmissionRow> collectiveLoadProcesses, CompanyPolicy companyPolicy, bool isAutomatic, int tempId)
        {
            List<Risk> risks = new List<Risk>();
            foreach (CollectiveEmissionRow collectiveLoadProcess in collectiveLoadProcesses)
            {
                try
                {
                    collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Tariff;
                    PendingOperation pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveLoadProcess.Risk.Id);
                    if (!Settings.UseReplicatedDatabase())   // Without Replicated Database
                    {
                        pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveLoadProcess.Risk.Id);
                    }
                    else  // with Replicated Database
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveLoadProcess.Risk.Id);
                    }
                    if (pendingOperation != null)
                    {
                        CompanyLiabilityRisk companyLiability = new CompanyLiabilityRisk();
                        companyLiability = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(pendingOperation.Operation);
                        companyLiability.Id = pendingOperation.Id;
                        companyLiability.IsPersisted = true;
                        companyLiability.CompanyPolicy = companyPolicy;
                        companyLiability = DelegateService.liabilityService.Quotate(companyLiability, true, true);
                        if (companyLiability.Premium > 0)
                        {
                            Risk rk = companyLiability;
                            rk.Coverages = DelegateService.underwritingService.CreateCoverages(companyLiability.CompanyRisk.CompanyCoverages);
                            risks.Add(rk);
                            Policy policyCore = DelegateService.underwritingService.CreateCorePolicyByCompanyPolicy(companyPolicy);
                            companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policyCore, risks, true);
                            if (companyPolicy.PaymentPlan == null)
                            {
                                throw new ValidationException(""/*Errors.ErrorPaymentPlanIsNull*/);
                            }
                            companyPolicy.ListFirstPayComponent = DelegateService.underwritingService.GetListFirstPayComponentByFinancialPlanId(companyPolicy.PaymentPlan.Id);
                            companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policyCore, risks);
                            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policyCore);
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
                            if (!Settings.UseReplicatedDatabase())
                            {
                                companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
                                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                                DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                                companyLiability.InfringementPolicies = DelegateService.liabilityService.ValidateAuthorizationPolicies(companyLiability);
                                companyLiability.CompanyPolicy = null;
                                pendingOperation.Operation = JsonConvert.SerializeObject(companyLiability);
                                DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                            }
                            else
                            {
                                companyPolicy.InfringementPolicies = DelegateService.underwritingService.ValidateAuthorizationPolicies(companyPolicy);
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
                                QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                                
                            }
                            //para validar prima
                            collectiveLoadProcess.Risk.Description = companyLiability.FullAddress;
                            collectiveLoadProcess.Risk.Id = companyLiability.Id;
                            collectiveLoadProcess.Premium = companyLiability.Premium;
                            collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Events;
                            collectiveLoadProcess.HasEvents = companyPolicy.InfringementPolicies.Count != 0 || companyLiability.InfringementPolicies.Count != 0;
                        }
                        else
                        {
                            collectiveLoadProcess.HasError = true;
                            collectiveLoadProcess.Observations = Errors.ErrorPremiumZero + KeySettings.ReportErrorSeparatorMessage();
                        }
                        collectiveLoadProcess.Risk.Description = companyLiability.FullAddress;
                    }
                    else
                    {
                        collectiveLoadProcess.HasError = true;
                        collectiveLoadProcess.Observations = Errors.ErrorTemporalNotFound + KeySettings.ReportErrorSeparatorMessage();
                    }
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                }
                catch (Exception ex)
                {
                    collectiveLoadProcess.Status = CollectiveLoadProcessStatus.Tariff;
                    collectiveLoadProcess.HasError = true;
                    collectiveLoadProcess.Observations = ex.Message;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveLoadProcess);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }
        }

        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveLoad)
        {
            if (collectiveLoad != null)
            {
                CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoad.Id, false);
                List<CollectiveEmissionRow> massiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Events);
                if (massiveEmissionRows.Count > 0)
                {
                    TP.Task.Run(() => IssuanceCollectiveEmissionRows(collectiveLoad, collectiveEmission, massiveEmissionRows));
                }
                else
                {
                    collectiveLoad.HasError = true;
                    collectiveLoad.ErrorDescription = Errors.ErrorRecordsNotFoundToIssue;
                }
            }
            return collectiveLoad;
        }

        public void IssuanceCollectiveEmissionRows(MassiveLoad collectiveLoad, CollectiveEmission collectiveEmission, List<CollectiveEmissionRow> massiveEmissionRows)
        {
            try
            {
                collectiveEmission.Status = MassiveLoadStatus.Issuing;
                collectiveEmission.TotalRows = massiveEmissionRows.Count;
                DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                ExecuteCreatePolicy(massiveEmissionRows, collectiveEmission);
                collectiveEmission.Status = MassiveLoadStatus.Issued;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
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
            CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetCompanyPolicyToIssueByOperationId(collectiveEmission.TemporalId);

            companyPolicy.PaymentPlan = DelegateService.underwritingService.GetPaymentPlansByProductId(companyPolicy.CompanyProduct.Id).First();
            companyPolicy.Id = collectiveEmission.TemporalId;
            if (collectiveEmission.TemporalId > 0)
            {
                List<PendingOperation> pendingOperations;
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
                }
                else
                {
                    pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
                }
                List<CompanyLiabilityRisk> companyLiabilities = new List<CompanyLiabilityRisk>();
                foreach (PendingOperation po in pendingOperations)
                {
                    var companyVehicle = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(po.Operation);
                    companyVehicle.CompanyPolicy = companyPolicy;
                    companyLiabilities.Add(companyVehicle);
                }
                companyPolicy = DelegateService.liabilityService.CreateEndorsement(companyPolicy, companyLiabilities);
                if (!Settings.UseReplicatedDatabase())
                {
                    DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
                }
                else
                {
                    DelegateService.pendingOperationEntityService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
                }
                ParallelHelper.ForEach(emissionRows, emissionRow =>
                {
                    emissionRow.Status = CollectiveLoadProcessStatus.Finalized;
                    if (emissionRow.Risk.Policy == null)
                    {
                        emissionRow.Risk.Policy = new Policy();
                        emissionRow.Risk.Policy.Endorsement = new Endorsement();
                        emissionRow.Risk.Policy.Summary = new Summary();
                    }
                    emissionRow.Risk.Policy.DocumentNumber = companyPolicy.DocumentNumber;
                    emissionRow.Risk.Policy.Endorsement.Number = companyPolicy.Endorsement.Number;
                    emissionRow.Risk.Policy.Summary.FullPremium = companyPolicy.Summary.FullPremium;
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
                });
            }
            else
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = Errors.ErrorTemporalNotFound;
            }
            collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
            collectiveEmission.EndorsementNumber = companyPolicy.Endorsement.Number;
            collectiveEmission.Premium = companyPolicy.Summary.FullPremium;
        }
    }
}