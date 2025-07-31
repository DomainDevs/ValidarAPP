using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.FidelityEndorsementExtensionService.EEProvider.Services;
using Sistran.Company.Application.FidelityEndorsementExtensionService3GProvider.Assemblers;
using Sistran.Company.Application.FidelityEndorsementExtensionService3GProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using baf = Sistran.Core.Framework.BAF;
using LEM = Sistran.Company.Application.Finances.FidelityServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.FidelityEndorsementExtensionService.EEProvider.Business
{
    class ExtensionBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FidelityEndorsementBusinessCia" /> class.
        /// </summary>
        public ExtensionBusinessCia()
        {
            provider = new BaseBusinessCia();
        }
        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        public CompanyPolicy CreateEndorsementExtension(CompanyPolicy companyPolicy)
        {
            try
            {
                if (companyPolicy == null)
                {
                    throw new ArgumentException(Errors.EmptyPolicy);
                }
                List<CompanyRisk> risks = new List<CompanyRisk>();
                CompanyPolicy policy;
                List<LEM.CompanyFidelityRisk> companyFidelitys = new List<LEM.CompanyFidelityRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if (companyPolicy?.Endorsement?.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    if (policy != null)
                    {

                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text?.TextBody,
                            Observations = companyPolicy.Endorsement.Text?.Observations
                        };

                        companyFidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByTemporalId(policy.Id);
                        if (companyFidelitys != null && companyFidelitys.Any())
                        {
                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companyFidelitys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            risks = CreateExtension(companyFidelitys);
                        }
                        else
                        {
                            companyFidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyFidelitys != null && companyFidelitys.Any())
                            {
                                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                                companyFidelitys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companyFidelitys);
                            }
                            else
                            {
                                throw new Exception(Errors.ErrorObtainingRisk);
                            }
                        }

                    }
                    else
                    {
                        throw new ArgumentException(Errors.TemporaryPolicyFound);
                    }

                }
                else
                {

                    policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
                    if (policy != null)
                    {
                        policy.UserId = companyPolicy.UserId;

                        policy.UserId = policy.UserId;
                        policy.CurrentFrom = policy.CurrentTo;
                        policy.CurrentTo = Convert.ToDateTime(companyPolicy.Endorsement.CurrentTo);
                        policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                        if (policy.Endorsement == null)
                        {
                            policy.Endorsement = new CompanyEndorsement();
                        }
                        policy.Endorsement.Text = new CompanyText
                        {
                            TextBody = companyPolicy.Endorsement.Text.TextBody,
                            Observations = companyPolicy.Endorsement.Text.Observations
                        };

                        policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                        policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                        policy.Endorsement.EndorsementType = EndorsementType.EffectiveExtension;
                        policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                        policy.TemporalType = TemporalType.Endorsement;
                        policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);

                        var imapper = ModelAssembler.CreateMapCompanyClause();
                        policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companyFidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyFidelitys != null && companyFidelitys.Any())
                            {
                                companyFidelitys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companyFidelitys);
                            }
                            else
                            {
                                throw new Exception(Errors.ErrorObtainingRisk);
                            }


                        }
                        else
                        {
                            throw new Exception(Errors.ErrorCreatingTemporary);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(Errors.PolicyNotFound);
                    }
                }
                if (policy.InfringementPolicies != null && policy.InfringementPolicies.Count() > 0)
                {
                    risks.ForEach(x => policy.InfringementPolicies.AddRange(x.InfringementPolicies));
                }
                if (risks != null && risks.Count != 0)
                {
                    policy.Summary = risks.First().Policy.Summary;
                }
                return policy;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<CompanyRisk> CreateExtension(List<LEM.CompanyFidelityRisk> companyFidelitys)
        {
            if (companyFidelitys != null && companyFidelitys.Any())
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if ((bool)companyFidelitys.First()?.Risk?.Policy.Product.IsCollective)
                {
                    if (companyFidelitys.First().Risk.Policy.Endorsement.TemporalId > 0)
                    {
                        companyFidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByTemporalId(companyFidelitys.First().Risk.Policy.Endorsement.TemporalId);
                        TP.Parallel.ForEach(companyFidelitys, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataExtension(item);
                            risk = DelegateService.FidelityService.CreateFidelityTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var fidelityPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        fidelityPolicy = DelegateService.underwritingService.CreatePolicyTemporal(fidelityPolicy, false);

                        return risks;
                    }
                    else
                    {
                        companyFidelitys = DelegateService.FidelityService.GetCompanyFidelitiesByPolicyId(companyFidelitys.First().Risk.Policy.Endorsement.PolicyId);
                        TP.Parallel.ForEach(companyFidelitys, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataExtension(item);
                            risk = DelegateService.FidelityService.CreateFidelityTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var fidelityPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        fidelityPolicy = DelegateService.underwritingService.CreatePolicyTemporal(fidelityPolicy, false);

                        return risks;
                    }
                }
                else
                {
                    TP.Parallel.ForEach(companyFidelitys, item =>
                    {
                        item.Risk.IsPersisted = true;
                        var risk = GetDataExtension(item);
                        risk = DelegateService.FidelityService.CreateFidelityTemporal(risk, false);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        risks.Add(risk.Risk);
                    });
                    var fidelityPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                    /*Actualiza el Pending Operation de la Poliza*/
                    fidelityPolicy = DelegateService.underwritingService.CreatePolicyTemporal(fidelityPolicy, false);

                    return risks;
                }
            }
            else
            {
                throw new Exception(Errors.ThereAreNoRisks);
            }


        }
        private LEM.CompanyFidelityRisk GetDataExtension(LEM.CompanyFidelityRisk risk)
        {
            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                ConcurrentBag<string> error = new ConcurrentBag<string>();
                if (risk.Risk.Beneficiaries != null)
                {
                    risk.Risk.Beneficiaries.AsParallel().ForAll(
                        item =>
                        {
                            var beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                            if (beneficiary != null)
                            {
                                item.IdentificationDocument = beneficiary.IdentificationDocument;
                                item.Name = beneficiary.Name;
                            }
                            else
                            {
                                error.Add(Errors.ErrorBeneficiaryNotFound);
                            }
                        }
                        );
                    if (error.Any())
                    {
                        throw new Exception(string.Join(",", error));
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorBeneficiaryEmpty);
                }
            }
            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
            if (coverages != null && coverages.Count > 0)
            {
                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();
                var ciaCoverages = risk.Risk.Coverages.Where(x => coverages.Select(z => z.Id).Contains(x.Id)).ToList();
                ciaCoverages.AsParallel().ForAll(item =>
                {
                    item.CoverStatus = item.CoverageOriginalStatus;
                    item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value));
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.Description = coverages.FirstOrDefault(u => u.Id == item.Id).Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverages.First(x => x.Id == item.Id).SubLineBusiness;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
            risk = DelegateService.FidelityService.QuotateFidelity(risk, false, false);
            return risk;
        }
    }
}
