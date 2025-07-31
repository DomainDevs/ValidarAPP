using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.Resources;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.Configuration;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using TS = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.LiabilityModificationService.EEProvider.DAOs
{
    public class ExclusionQuotateDAO
    {
        public void QuotateCollectiveExclusion(int collectiveEmissionId)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(collectiveEmissionId, false);

            if (collectiveEmission != null)
            {
                try
                {
                    TS.Task.Run(() => QuotateCollectiveExclusionRows(collectiveEmission));
                }
                catch (Exception ex)
                {
                    collectiveEmission.HasError = true;
                    collectiveEmission.ErrorDescription = ex.Message;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            }
        }

        private void QuotateCollectiveExclusionRows(CollectiveEmission collectiveEmission)
        {
            try
            {
                List<CollectiveEmissionRow> collectiveEmissionRows = DelegateService.collectiveService.GetCollectiveEmissionRowByMassiveLoadId(collectiveEmission.Id, CollectiveLoadProcessStatus.Validation);
                collectiveEmission.Status = MassiveLoadStatus.Tariffing;
                collectiveEmission.TotalRows = collectiveEmissionRows.Count;
                if (collectiveEmissionRows.Count > 0)
                {

                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);

                    PendingOperation pendingOperation;
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmission.TemporalId);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmission.TemporalId);
                    }
                    if (pendingOperation == null)
                    {
                        throw new ValidationException("El temporal no existe");
                    }

                    CompanyPolicy companyPolicy = new CompanyPolicy();

                    companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.IsPersisted = true;
                    companyPolicy.Id = pendingOperation.Id;
                    companyPolicy.UserId = collectiveEmission.User.UserId;


                    List<Risk> risks = QuotateCollectiveExclusionRows(collectiveEmissionRows, companyPolicy, collectiveEmission.IsAutomatic, collectiveEmission.TemporalId);
                    if (risks.Count > 0)
                    {
                        if (collectiveEmission.IsAutomatic)
                        {
                            Policy policy = companyPolicy;
                            policy.Product = companyPolicy.CompanyProduct;
                            policy.Branch = companyPolicy.CompanyBranch;
                            companyPolicy.PayerComponents = DelegateService.underwritingService.CalculatePayerComponents(policy, risks,true);
                            companyPolicy.Summary = DelegateService.underwritingService.CalculateSummary(policy, risks);
                            companyPolicy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(policy);
                            companyPolicy.Agencies = DelegateService.underwritingService.CalculateCommissions(policy, risks);
                            PendingOperation policyPendingOperation;
                            if (!Settings.UseReplicatedDatabase())
                            {
                                policyPendingOperation = DelegateService.commonService.GetPendingOperationById(companyPolicy.Id);
                            }
                            else
                            {
                                policyPendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(companyPolicy.Id);
                            }
                            policyPendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                            policyPendingOperation.UserId = companyPolicy.UserId;
                            if (!Settings.UseReplicatedDatabase())
                            {
                                DelegateService.commonService.UpdatePendingOperation(policyPendingOperation);
                            }
                            else
                            {
                                DelegateService.pendingOperationEntityService.UpdatePendingOperation(policyPendingOperation);

                            }

                        }

                        collectiveEmission.Premium = companyPolicy.Summary.Premium;
                        collectiveEmission.Commiss = companyPolicy.Summary.Premium * companyPolicy.Agencies[0].Commissions[0].Percentage / 100;
                        collectiveEmission.HasEvents = companyPolicy.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;

                    }

                    collectiveEmission.Status = MassiveLoadStatus.Tariffed;
                    DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
                }
            }
            catch (Exception ex)
            {
                collectiveEmission.HasError = true;
                collectiveEmission.ErrorDescription = ex.Message;
                DelegateService.collectiveService.UpdatCollectiveEmission(collectiveEmission);
            }

        }

        private List<Risk> QuotateCollectiveExclusionRows(List<CollectiveEmissionRow> collectiveEmissionRows, CompanyPolicy companyPolicy, bool isAutomatic, int temporalId)
        {
            List<Risk> risks = new List<Risk>();

            List<int> packages = DataFacadeManager.GetPackageProcesses(collectiveEmissionRows.Count(), "MaxThreadMassive");

            Core.Application.Utilities.Helper.ParallelHelper.ForEach(collectiveEmissionRows, (collectiveEmissionRow) =>
            {
                //foreach (int package in packages)
                //{
                //    List< CollectiveEmissionRow > collectivesEmissionRows = collectiveEmissionRows.Take(package).ToList();
                //    collectiveEmissionRows.RemoveRange(0, package);
                //    Parallel.ForEach(collectivesEmissionRows, collectiveEmissionRow =>
                //    {
                try
                {
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Tariff;
                    PendingOperation pendingOperation;
                    if (!Settings.UseReplicatedDatabase())
                    {
                        pendingOperation = DelegateService.commonService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    }
                    else
                    {
                        pendingOperation = DelegateService.pendingOperationEntityService.GetPendingOperationById(collectiveEmissionRow.Risk.Id);
                    }
                    if (pendingOperation != null)
                    {
                        CompanyLiabilityRisk companyLiabilityRisk = new CompanyLiabilityRisk();
                        companyLiabilityRisk = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(pendingOperation.Operation);
                        companyLiabilityRisk.Id = pendingOperation.Id;
                        companyLiabilityRisk.IsPersisted = true;
                        companyLiabilityRisk.CompanyPolicy = companyPolicy;
                        companyLiabilityRisk.Status = RiskStatusType.Excluded;
                        companyPolicy.InfringementPolicies = DelegateService.liabilityService.ValidateAuthorizationPolicies(companyLiabilityRisk);

                        if (companyLiabilityRisk.CompanyRisk.CompanyCoverages != null)
                        {
                            companyLiabilityRisk.CompanyRisk.CompanyCoverages.ForEach(x =>
                            {
                                x.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                                x.CurrentTo = companyPolicy.CurrentTo;
                                x.CoverStatus = CoverageStatusType.Excluded;
                            });
                        }
                        CompanyLiabilityRisk companyLiabilityQuotated = DelegateService.liabilityService.Quotate(companyLiabilityRisk, true, true);

                        Mapper.CreateMap(companyLiabilityQuotated.GetType(), companyLiabilityRisk.GetType());
                        Mapper.Map(companyLiabilityQuotated, companyLiabilityRisk);
                        Risk risk = companyLiabilityRisk;
                        risk.Coverages = new List<Coverage>();
                        risk.Coverages.AddRange(companyLiabilityRisk.CompanyRisk.CompanyCoverages);
                        risk.MainInsured = companyLiabilityRisk.CompanyRisk.CompanyInsured;
                        risk.Policy = companyLiabilityRisk.CompanyPolicy;
                        risk.Policy.Product = companyLiabilityRisk.CompanyPolicy.CompanyProduct;
                        risk.Policy.Branch = companyLiabilityRisk.CompanyPolicy.CompanyBranch;
                        risks.Add(risk);
                        companyLiabilityRisk.CompanyPolicy = null;
                        pendingOperation.Operation = JsonConvert.SerializeObject(companyLiabilityRisk);
                        pendingOperation.UserId = companyPolicy.UserId;
                        companyPolicy.Holder.CompanyName = companyLiabilityRisk.CompanyRisk.CompanyInsured.CompanyName;

                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                        }
                        else
                        {
                            string pendingOperationJson = JsonConvert.SerializeObject(pendingOperation);
                            QueueHelper.PutOnQueueJsonByQueue(pendingOperationJson, "UpdatePendingOperationQuee");
                            
                        }
                        collectiveEmissionRow.Risk.RiskId = companyLiabilityRisk.Id;
                        collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                        collectiveEmissionRow.Premium = companyLiabilityRisk.Premium;
                        collectiveEmissionRow.HasEvents = companyLiabilityRisk.InfringementPolicies.Count(x => x.Type != TypePolicies.Notification) != 0;

                        pendingOperation.Operation = JsonConvert.SerializeObject(companyLiabilityRisk);

                        if (!Settings.UseReplicatedDatabase())
                        {
                            DelegateService.commonService.UpdatePendingOperation(pendingOperation);
                        }
                        else
                        {
                            DelegateService.pendingOperationEntityService.UpdatePendingOperation(pendingOperation);
                        }
                        collectiveEmissionRow.Risk.Description = companyLiabilityRisk.FullAddress;

                    }
                    else
                    {
                        collectiveEmissionRow.HasError = true;
                        collectiveEmissionRow.Observations = "No se encuentra el temporal" + KeySettings.ReportErrorSeparatorMessage();
                    }
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
                catch (Exception ex)
                {
                    collectiveEmissionRow.Status = CollectiveLoadProcessStatus.Events;
                    collectiveEmissionRow.HasError = true;
                    string[] messages = ex.Message.Split('|');
                    collectiveEmissionRow.Observations += string.Format(Errors.ErrorQuotate, messages[0]);
                    DelegateService.collectiveService.UpdateCollectiveEmissionRow(collectiveEmissionRow);
                }
                finally
                {
                    DataFacadeManager.Dispose();
                }
            });


            return risks;
        }
    }
}
