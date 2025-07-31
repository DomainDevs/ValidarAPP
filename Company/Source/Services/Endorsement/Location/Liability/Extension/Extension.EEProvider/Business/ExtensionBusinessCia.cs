using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.LiabilityEndorsementExtensionService.EEProvider.Services;
using Sistran.Company.Application.LiabilityEndorsementExtensionService3GProvider.Assembler;
using Sistran.Company.Application.LiabilityEndorsementExtensionService3GProvider.Resources;
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
using baf = Sistran.Core.Framework.BAF;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.LiabilityEndorsementExtensionService.EEProvider.Business
{
    class ExtensionBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiabilityEndorsementBusinessCia" /> class.
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
                List<LEM.CompanyLiabilityRisk> companyLibialitys = new List<LEM.CompanyLiabilityRisk>();
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

                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        policy.Endorsement.IsMassive = companyPolicy.Endorsement.IsMassive;
                        if (companyPolicy.Endorsement.BusinessTypeDescription != 0)
                        {
                            policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyPolicy.Endorsement.BusinessTypeDescription.ToString());
                        }

                        companyLibialitys = DelegateService.LibialityService.GetCompanyLiabilitiesByTemporalId(policy.Id);
                        if (companyLibialitys != null && companyLibialitys.Any())
                        {
                            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                            companyLibialitys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            risks = CreateExtension(companyLibialitys);
                        }
                        else
                        {
                            companyLibialitys = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyLibialitys != null && companyLibialitys.Any())
                            {
                                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                                companyLibialitys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                                risks = CreateExtension(companyLibialitys);
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
                        policy.Id = 0;
                        policy.Endorsement.TemporalId = 0;
                        policy.IssueDate = companyPolicy.IssueDate;
                        policy.UserId = companyPolicy.UserId;
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

                        var immaper = AutoMapperAssembler.CreateMapClause();
                        policy.Clauses = immaper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                        policy.Summary = new CompanySummary
                        {
                            RiskCount = 0
                        };
                        policy.Endorsement.TicketNumber = companyPolicy.Endorsement.TicketNumber;
                        policy.Endorsement.TicketDate = companyPolicy.Endorsement.TicketDate;
                        policy.Endorsement.IsMassive = companyPolicy.Endorsement.IsMassive;
                        if (companyPolicy.Endorsement.BusinessTypeDescription != 0)
                        {
                            policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyPolicy.Endorsement.BusinessTypeDescription.ToString());
                        }
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null)
                        {
                            companyLibialitys = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            if (companyLibialitys != null && companyLibialitys.Any())
                            {
                                companyLibialitys.AsParallel().ForAll(

                                   x =>
                                   {
                                       x.Risk.OriginalStatus = x.Risk.Status;
                                       x.Risk.Status = RiskStatusType.Modified;
                                       x.Risk.Policy = policy;
                                   }
                                   );
                                risks = CreateExtension(companyLibialitys);
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
                    risks.AsParallel().ForAll(x => policy.InfringementPolicies.AddRange(x.InfringementPolicies));
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

        private List<CompanyRisk> CreateExtension(List<LEM.CompanyLiabilityRisk> companyLibialitys)
        {
            if (companyLibialitys != null && companyLibialitys.Any())
            {
                List<CompanyRisk> risks = new List<CompanyRisk>();
                PendingOperation pendingOperation = new PendingOperation();

                if ((bool)companyLibialitys.First()?.Risk?.Policy.Product.IsCollective)
                {
                    if (companyLibialitys.First().Risk.Policy.Endorsement.TemporalId > 0)
                    {
                        companyLibialitys = DelegateService.LibialityService.GetCompanyLiabilitiesByTemporalId(companyLibialitys.First().Risk.Policy.Endorsement.TemporalId);
                        TP.Parallel.ForEach(companyLibialitys, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataExtension(item);
                            risk = DelegateService.LibialityService.CreateLiabilityTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var libialityPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        libialityPolicy = DelegateService.underwritingService.CreatePolicyTemporal(libialityPolicy, false);

                        return risks;
                    }
                    else
                    {
                        companyLibialitys = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyLibialitys.First().Risk.Policy.Endorsement.PolicyId);
                        TP.Parallel.ForEach(companyLibialitys, item =>
                        {
                            item.Risk.IsPersisted = true;
                            var risk = GetDataExtension(item);
                            risk.Risk.Status = RiskStatusType.Original;
                            risk = DelegateService.LibialityService.CreateLiabilityTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            risks.Add(risk.Risk);
                        });

                        var libialityPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                        libialityPolicy = DelegateService.underwritingService.CreatePolicyTemporal(libialityPolicy, false);

                        return risks;
                    }
                }
                else
                {
                    TP.Parallel.ForEach(companyLibialitys, item =>
                    {
                        item.Risk.IsPersisted = true;
                        var risk = GetDataExtension(item);
                        risk.Risk.Status = RiskStatusType.Original;
                        risk = DelegateService.LibialityService.CreateLiabilityTemporal(risk, false);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        risks.Add(risk.Risk);
                    });
                    var libialityPolicy = provider.CalculatePolicyAmounts(risks.First().Policy, risks);

                    /*Actualiza el Pending Operation de la Poliza*/
                    libialityPolicy = DelegateService.underwritingService.CreatePolicyTemporal(libialityPolicy, false);

                    return risks;
                }
            }
            else
            {
                throw new Exception(Errors.ThereAreNoRisks);
            }


        }
        private LEM.CompanyLiabilityRisk GetDataExtension(LEM.CompanyLiabilityRisk risk)
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

                    String coverStatusName = String.Empty;
                    if (item.CoverageOriginalStatus.HasValue)
                    {
                        if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value)) == null)
                        {
                            coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value);
                        }
                        else
                        {
                            coverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverageOriginalStatus.Value));
                        }
                    }

                    CompanyCoverage coverageLocal = coverages.FirstOrDefault(u => u.Id == item.Id);
                    item.CoverStatus = item.CoverageOriginalStatus;
                    item.CoverStatusName = coverStatusName;
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.Description = coverageLocal.Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;

                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.SubLineBusiness = coverageLocal.SubLineBusiness;
                    item.IsSelected = coverageLocal.IsSelected;
                    item.IsMandatory = coverageLocal.IsMandatory;
                    item.IsVisible = coverageLocal.IsVisible;
                    item.EndorsementLimitAmount = 0;
                    item.EndorsementSublimitAmount = 0;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
            risk = DelegateService.LibialityService.QuotateLiability(risk, false, false);
            return risk;
        }
    }
}
