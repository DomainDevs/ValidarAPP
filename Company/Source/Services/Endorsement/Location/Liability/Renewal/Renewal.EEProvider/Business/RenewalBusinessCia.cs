using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.LiabilityRenewalService.EEProvider.Assemblers;
using Sistran.Company.Application.LiabilityRenewalService.EEProvider.Resources;
using Sistran.Company.Application.LiabilityRenewalService.EEProvider.Services;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TM=System.Threading.Tasks;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.LiabilityRenewalService.EEProvider.Business
{
    public class RenewalBusinessCia
    {
        public static readonly string MindateDate = "01/01/1900";
        public static DateTime GetMinDate() => DateTime.Parse(MindateDate);
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewalBusinessCia" /> class.
        /// </summary>
        public RenewalBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy)
         {
            try
            {
                if (companyPolicy == null || companyPolicy.Endorsement == null)
                {
                    throw new Exception(Errors.ErrorNotDataPolicy);
                }
                CompanyPolicy policy = null;
                int id = 0;
                if (companyPolicy.Endorsement.TemporalId > 0)
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyPolicy.Endorsement.TemporalId, false);
                    id = policy.Id;
                }
                else
                {
                    policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyPolicy.Endorsement.Id);
                    policy.Id = 0;
                    policy.Endorsement.TemporalId = 0;
                    policy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    policy.UserId = companyPolicy.UserId;
                    policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                    if (policy.Endorsement == null)
                    {
                        policy.Endorsement = new CompanyEndorsement();
                    }
                    policy.Endorsement.IsUnderIdenticalConditions = companyPolicy.Endorsement.IsUnderIdenticalConditions;
                    policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                    var immaper = AutoMapperAssembler.CreateMapClause();
                    policy.Clauses = immaper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                }
                int? version;
                int temporalId = 0;
                version = policy.Endorsement.AppRelation;
                temporalId = policy.Endorsement.TemporalId;
                policy.Endorsement = companyPolicy.Endorsement;
                policy.Endorsement.TemporalId = temporalId;
                policy.Endorsement.EndorsementType = EndorsementType.Renewal;
                policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                policy.TemporalType = TemporalType.Endorsement;
                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                policy.TimeHour = "00";
                //Define la fecha hasta de la renovación a partir de la vigencia del endoso anterior
                policy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom == GetMinDate() ? policy.CurrentTo : companyPolicy.Endorsement.CurrentFrom;
                policy.CurrentTo = companyPolicy.Endorsement.CurrentTo == GetMinDate() ? policy.CurrentFrom.AddYears(1) : companyPolicy.Endorsement.CurrentTo;
                policy.IssueDate = DelegateService.commonService.GetDate();
                policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                if (policy != null)
                {
                    if (!companyPolicy.Endorsement.IsUnderIdenticalConditions && policy.Product.IsCollective)
                    {
                        return policy;
                    }
                    List<LEM.CompanyLiabilityRisk> companyLibialitys = new List<LEM.CompanyLiabilityRisk>();
                    if (id > 0)
                    {
                        companyLibialitys = DelegateService.LibialityService.GetCompanyLiabilitiesByTemporalId(id);
                        if (companyLibialitys == null && companyLibialitys.Count < 1)
                        {
                            companyLibialitys = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                            companyLibialitys.AsParallel().ForAll(
                          x =>
                          {
                              x.Risk.Id = 0;
                              x.Risk.OriginalStatus = x.Risk.Status;
                              x.Risk.Status = RiskStatusType.Included;
                          });
                        }
                    }
                    else
                    {
                        companyLibialitys = DelegateService.LibialityService.GetCompanyLiebilitiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                        companyLibialitys.AsParallel().ForAll(
                           x =>
                           {
                               x.Risk.Id = 0;
                               x.Risk.OriginalStatus = x.Risk.Status;
                               x.Risk.Status = RiskStatusType.Included;
                           });
                    }
                    if (companyLibialitys != null)
                    {
                        ConcurrentBag<CompanyRisk> risks = new ConcurrentBag<CompanyRisk>();
                        Object objectLock = new Object();
                        if (policy.Product.IsCollective && companyPolicy.Endorsement.IsUnderIdenticalConditions)
                        {

                            TP.Parallel.ForEach(companyLibialitys, item =>
                            {
                                item.Risk.Policy = policy;
                                CompanyRisk risk = Createlibiality(item, companyPolicy);
                                risks.Add(risk);
                            });
                            policy = provider.CalculatePolicyAmounts(policy, risks.ToList());
                        }
                        else
                        {
                            companyLibialitys.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            ConcurrentBag<string> errorsRisk = new ConcurrentBag<string>();
                            TP.Parallel.ForEach(companyLibialitys,  item =>
                            {
                                try
                                {
                                    if (item != null)
                                    {
                                        var risk = new LEM.CompanyLiabilityRisk();
                                        lock (objectLock)
                                        {
                                            risk = GetDataRenewal(item, companyPolicy);
                                        }
                                        if (risk != null && risk.Risk != null)
                                        {
                                            List<CompanyRiskInsured> companyRisksInsured = new List<CompanyRiskInsured>();
                                            foreach (var companyRisk in companyLibialitys)
                                            {
                                                CompanyRiskInsured companyRiskInsured = new CompanyRiskInsured();
                                                companyRiskInsured.Beneficiaries = new List<CompanyBeneficiary>();
                                                foreach (var beneficiary in companyRisk.Risk.Beneficiaries)
                                                {
                                                    companyRiskInsured.Beneficiaries.Add(beneficiary);
                                                }
                                                companyRiskInsured.Insured = companyRisk.Risk.MainInsured;
                                                companyRisksInsured.Add(companyRiskInsured);
                                            }
                                            risk.Risk.Policy.Summary.RisksInsured = companyRisksInsured;

                                            risk = DelegateService.LibialityService.CreateLiabilityTemporal(risk, false);

                                            if (risk?.Risk?.Policy?.InfringementPolicies != null)
                                            {
                                                risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies.Where(x => x != null));
                                            }
                                            if (risk.Risk != null)
                                            {
                                                risks.Add(risk.Risk);
                                            }
                                            else
                                            {
                                                errorsRisk.Add(Errors.ErrorCreateTemporalPolicy);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        errorsRisk.Add(Errors.RiskNotFound);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorsRisk.Add(ex.GetBaseException().Message);
                                }

                            });
                            if (errorsRisk != null && errorsRisk.Count > 0)
                            {
                                throw new Exception(string.Join(" ", errorsRisk));
                            }
                            policy = provider.CalculatePolicyAmounts(policy, risks.ToList());
                        }

                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        if (policy != null && policy.InfringementPolicies != null && policy.InfringementPolicies.Count > 0)
                        {
                            var infPolicies = risks.SelectMany(x => x.InfringementPolicies).ToList();
                            policy.InfringementPolicies.AddRange(infPolicies);
                        }

                        return policy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorLibialityNotFound);
                    }

                }
                else
                {
                    throw new Exception(Errors.ErrorCreateTemporalPolicy);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the data renewal.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        private LEM.CompanyLiabilityRisk GetDataRenewal(LEM.CompanyLiabilityRisk risk, CompanyPolicy policy)
        {
            risk.Risk.Status = RiskStatusType.Original;
            

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
                                item.CustomerType = beneficiary.CustomerType;
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
                String coverStatusName = String.Empty;
                if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original)) == null)
                {
                    coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                }
                else
                {
                    coverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));
                }

                ciaCoverages.AsParallel().ForAll(item =>
                {

                    var coverageLocal = coverages.FirstOrDefault(u => u.Id == item.Id);

                    item.CoverStatus = CoverageStatusType.Original;
                    item.CoverStatusName = coverStatusName;
                    item.CurrentFrom = policy.Endorsement.CurrentFrom;
                    item.CurrentTo = policy.Endorsement.CurrentTo;
                    item.Description = coverageLocal.Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = coverageLocal.FlatRatePorcentage;
                    item.SubLineBusiness = coverageLocal.SubLineBusiness;
                    item.RuleSetId = coverageLocal.RuleSetId;
                    item.PosRuleSetId = coverageLocal.PosRuleSetId;
                    item.ScriptId = coverageLocal.ScriptId;
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
            risk.Risk.IsPersisted = true;

            risk = DelegateService.LibialityService.QuotateLiability(risk, false, true);

            return risk;
        }

        /// <summary>
        /// Creates the liability.
        /// </summary>
        /// <param name="item">The item.</param>
        private CompanyRisk Createlibiality(LEM.CompanyLiabilityRisk item, CompanyPolicy companyPolicy)
        {
            item.Risk.IsPersisted = true;
            LEM.CompanyLiabilityRisk libiality = item;

            libiality = GetDataRenewal(libiality, companyPolicy);
            libiality.Risk.RiskId = 0;

            libiality = DelegateService.LibialityService.CreateLiabilityTemporal(libiality, false);

            return libiality.Risk;
        }
    }
}
