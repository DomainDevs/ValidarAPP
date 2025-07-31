using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityCollectiveService.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Company.Location.LiabilityCollectiveService.EEProvider.Resources;
using Sistran.Core.Framework;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Configuration;

namespace Company.Location.LiabilityCollectiveService.EEProvider.DAOs
{
    public class CollectiveLoadProcessLiabilityDAO
    {
        public void QuotateCollectiveEmission(List<int> collectiveEmissionIds)
        {
            foreach (int collectiveEmissionId in collectiveEmissionIds)
            {
                CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveEmissionId, false);

                if (collectiveEmission != null)
                {
                    try
                    {
                        Task.Run(() => QuotateCollectiveEmissionRows(collectiveEmission));
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
        }

        public void QuotateMassiveCollectiveEmission(int collectiveLoadId)
        {
            //foreach (int collectiveLoadId in collectiveLoadIds)
            //{
            CollectiveEmission collectiveLoad = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveLoadId, false);
            if (collectiveLoad != null)
            {
                try
                {
                    Task.Run(() => QuotateCollectiveEmissionRows(collectiveLoad));
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
            //}
        }

        private void QuotateCollectiveEmissionRows(CollectiveEmission collectiveLoad)
        {
            List<CollectiveEmissionRow> collectiveLoadProcesses = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoad.Id, CollectiveLoadProcessStatus.Validation);
            if (collectiveLoadProcesses.Count > 0)
            {
                collectiveLoad.Status = MassiveLoadStatus.Tariffing;
                collectiveLoad.TotalRows = collectiveLoadProcesses.Count;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveLoad);
                PendingOperation pendingOperation = new PendingOperation();
                if (!Settings.UseReplicatedDatabase())
                {
                    pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveLoad.TemporalId);
                }
                else
                {
                    pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveLoad.TemporalId);
                }
                CompanyPolicy companyPolicy = new CompanyPolicy();
                if (collectiveLoad.IsAutomatic)
                {
                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                }
                else
                {
                    Policy policy = JsonConvert.DeserializeObject<Policy>(pendingOperation.Operation);
                    Mapper.CreateMap(policy.GetType(), companyPolicy.GetType());
                    Mapper.Map(policy, companyPolicy);
                }
                companyPolicy.IsPersisted = true;
                QuotateCollectiveEmissionRows(collectiveLoadProcesses, companyPolicy, (bool)collectiveLoad.IsAutomatic, collectiveLoad.TemporalId);
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
                            companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policyCore, risks,true);
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
                                var queue = new Sistran.Core.Framework.Queues.BaseQueueFactory().CreateQueue("UpdatePendingOperationQuee", routingKey: "UpdatePendingOperationQuee");
                                queue.PutOnQueue(pendingOperationJson);
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
                    collectiveEmission.Status = MassiveLoadStatus.Issuing;
                    collectiveEmission.TotalRows = massiveEmissionRows.Count;
                    DelegateService.massiveService.UpdateMassiveLoad(collectiveLoad);
                    ExecuteCreatePolicy(massiveEmissionRows, collectiveEmission);
                    collectiveEmission.Status = MassiveLoadStatus.Issued;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
                else
                {
                    throw new ValidationException("Error no se encontraron registros para emitir");
                }
            }
            return collectiveLoad;
        }

        private void ExecuteCreatePolicy(List<CollectiveEmissionRow> emissionRows, CollectiveEmission collectiveEmission)
        {
            CompanyPolicy companyPolicy = DelegateService.massiveUnderwritingService.GetCompanyPolicyToIssueByOperationId(collectiveEmission.TemporalId);
            companyPolicy = DelegateService.underwritingService.CreateCompanyPolicy(companyPolicy);
            companyPolicy.Id = collectiveEmission.TemporalId;
            collectiveEmission.DocumentNumber = companyPolicy.DocumentNumber;
            collectiveEmission.EndorsementId = companyPolicy.Endorsement.Id;
            DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            List<PendingOperation> pendingOperations;
            if (!Settings.UseReplicatedDatabase())
            {
                pendingOperations = DelegateService.commonService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
            }
            else
            {
                pendingOperations = DelegateService.pendingOperationEntityService.GetPendingOperationsByParentId(collectiveEmission.TemporalId);
            }
            ParallelHelper.ForEach(emissionRows, emissionRow =>
            {
                CreateRisk(emissionRow, pendingOperations, companyPolicy);
            });
            if (!Settings.UseReplicatedDatabase())
            {
                DelegateService.underwritingService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
            }
            else
            {
                DelegateService.pendingOperationEntityService.RecordEndorsementOperation(companyPolicy.Endorsement.Id, companyPolicy.Id);
            }
        }

        private void CreateRisk(CollectiveEmissionRow emissionRow, List<PendingOperation> pendingOperations, CompanyPolicy companyPolicy)
        {
            try
            {
                emissionRow.Status = CollectiveLoadProcessStatus.Issuance;
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
                PendingOperation riskPendingOperation = pendingOperations.Find(po => po.Id == emissionRow.Risk.RiskId);
                if (riskPendingOperation == null)
                {
                    emissionRow.HasError = true;
                    emissionRow.Observations = Errors.ErrorRisksNotFound + KeySettings.ReportErrorSeparatorMessage();
                    return;
                }
                List<CompanyLiabilityRisk> companyLiability = new List<CompanyLiabilityRisk>();
                CompanyLiabilityRisk risk = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(riskPendingOperation.Operation);
                risk.CompanyPolicy = companyPolicy;
                companyLiability.Add(risk);
                DelegateService.liabilityService.CreateCompanyLiability(companyLiability);
                emissionRow.Status = CollectiveLoadProcessStatus.Finalized;
            }
            catch (Exception ex)
            {
                emissionRow.HasError = true;
                emissionRow.Observations = $"{Errors.ErrorIssuing} | {ex.Message}";
            }
            finally
            {
                DelegateService.collectiveService.UpdateCollectiveEmissionRow(emissionRow);
            }
        }
    }
}