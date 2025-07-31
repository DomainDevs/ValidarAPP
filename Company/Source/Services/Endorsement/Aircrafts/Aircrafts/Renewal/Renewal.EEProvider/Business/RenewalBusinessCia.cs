using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Renewal.EEProvider.Assemblers;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.AircraftRenewalService.EEProvider.Resources;
using Sistran.Company.Application.AircraftRenewalService.EEProvider.Services;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.AircraftRenewalService.EEProvider.Business
{
    public class RenewalBusinessCia
    {
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
                    policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                    policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                    var imapper = AutoMapperAssembler.CreateMapCompanyClause();
                    policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                }
                policy.Endorsement = companyPolicy.Endorsement;
                policy.Endorsement.EndorsementType = EndorsementType.Renewal;
                policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                policy.TemporalType = TemporalType.Endorsement;
                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                policy.TimeHour = "00";
                policy.CurrentFrom = companyPolicy.Endorsement.CurrentFrom;
                policy.CurrentTo = companyPolicy.Endorsement.CurrentTo;
                policy.IssueDate = companyPolicy.IssueDate;

                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                if (policy != null)
                {
                    if (!companyPolicy.Endorsement.IsUnderIdenticalConditions && policy.Product.IsCollective)
                    {
                        return policy;
                    }
                    List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();
                    if (id > 0)
                    {
                        companyAircrafts = DelegateService.aircraftService.GetCompanyAircraftsByTemporalId(id);
                    }
                    else
                    {
                        companyAircrafts = DelegateService.aircraftService.GetCompanyAircraftsByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                    if (companyAircrafts != null)
                    {
                        ConcurrentBag<CompanyRisk> risks = new ConcurrentBag<CompanyRisk>();
                        Object objectLock = new Object();
                        if (policy.Product.IsCollective && companyPolicy.Endorsement.IsUnderIdenticalConditions)
                        {

                            Parallel.ForEach(companyAircrafts, ParallelHelper.DebugParallelFor(), item =>
                            {
                                item.Risk.Policy = policy;
                                CompanyRisk risk = Createaircraft(item);
                                risks.Add(risk);
                            });
                            policy = provider.CalculatePolicyAmounts(policy, risks.ToList());
                        }
                        else
                        {
                            companyAircrafts.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            ConcurrentBag<string> errorsRisk = new ConcurrentBag<string>();
                            Parallel.ForEach(companyAircrafts, ParallelHelper.DebugParallelFor(), item =>
                            {
                                try
                                {
                                    if (item != null)
                                    {
                                        var risk = new CompanyAircraft();
                                        lock (objectLock)
                                        {
                                            risk = GetDataRenewal(item);
                                        }
                                        if (risk != null && risk.Risk != null)
                                        {

                                            risk = DelegateService.aircraftService.CreateCompanyAircraftTemporal(risk);

                                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies.Where(x => x != null));
                                            risks.Add(risk.Risk);
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
                        throw new Exception(Errors.ErrorAircraftNotFound);
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
        private CompanyAircraft GetDataRenewal(CompanyAircraft risk)
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
                    item.CoverStatus = CoverageStatusType.Original;
                    item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
                    item.Description = coverages.FirstOrDefault(u => u.Id == item.Id).Description;
                    item.EndorsementType = risk.Risk.Policy.Endorsement.EndorsementType;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = coverages.FirstOrDefault(u => u.Id == item.Id).FlatRatePorcentage;
                    item.SubLineBusiness = coverages.First(x => x.Id == item.Id).SubLineBusiness;
                    item.RuleSetId = coverages.First(x => x.Id == item.Id).RuleSetId;
                    item.PosRuleSetId = coverages.First(x => x.Id == item.Id).PosRuleSetId;
                });
                risk.Risk.Coverages = ciaCoverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
            risk.Risk.IsPersisted = true;
            risk = DelegateService.aircraftService.QuotateCompanyAircraft(risk, true, true);

            return risk;
        }

        /// <summary>
        /// Creates the aircraft.
        /// </summary>
        /// <param name="item">The item.</param>
        private CompanyRisk Createaircraft(CompanyAircraft item)
        {
            item.Risk.IsPersisted = true;
            CompanyAircraft aircraft = item;

            aircraft = GetDataRenewal(aircraft);
            aircraft.Risk.RiskId = 0;

            aircraft = DelegateService.aircraftService.CreateCompanyAircraftTemporal(aircraft);

            return aircraft.Risk;
        }
    }
}
