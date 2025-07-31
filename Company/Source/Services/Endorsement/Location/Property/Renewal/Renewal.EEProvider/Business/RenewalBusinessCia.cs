using AutoMapper;
using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
using Sistran.Company.Application.PropertyRenewalService.EEProvider.Assemblers;
using Sistran.Company.Application.PropertyRenewalService.EEProvider.Resources;
using Sistran.Company.Application.PropertyRenewalService.EEProvider.Services;
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
using Sistran.Core.Application.Utilities.Utility;
using TP = Sistran.Core.Application.Utilities.Utility;

using PEM = Sistran.Company.Application.Location.PropertyServices.Models;

namespace Sistran.Company.Application.PropertyEndorsementRenewalService.EEProvider.Business
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
                    policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                    policy.Endorsement.EndorsementReasonId = companyPolicy.Endorsement.EndorsementReasonId;
                    var immaper = AutoMapperAssembler.CreateMapCompanyClause();
                    policy.Clauses = immaper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

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
                    List<PEM.CompanyPropertyRisk> companyProperty = new List<PEM.CompanyPropertyRisk>();
                    if (id > 0)
                    {
                        companyProperty = DelegateService.PropertyService.GetCompanyPropertiesByTemporalId(id);
                    }
                    else
                    {
                        companyProperty = DelegateService.PropertyService.GetCompanyPropertiesByPolicyId(companyPolicy.Endorsement.PolicyId);
                    }
                    if (companyProperty != null)
                    {
                        ConcurrentBag<CompanyRisk> risks = new ConcurrentBag<CompanyRisk>();
                        Object objectLock = new Object();
                        if (policy.Product.IsCollective && companyPolicy.Endorsement.IsUnderIdenticalConditions)
                        {

                            TP.Parallel.ForEach(companyProperty,  item =>
                            {
                                item.Risk.Id = 0;
                                item.Risk.Policy = policy;
                                item.Risk.OriginalStatus = item.Risk.Status;
                                item.Risk.Status = RiskStatusType.Included;
                                CompanyRisk risk = CreateProperty(item);
                                risks.Add(risk);
                            });
                            policy = provider.CalculatePolicyAmounts(policy, risks.ToList());
                        }
                        else
                        {
                            companyProperty.AsParallel().ForAll(x => x.Risk.Policy = policy);
                            ConcurrentBag<string> errorsRisk = new ConcurrentBag<string>();
                            TP.Parallel.ForEach(companyProperty,  item =>
                            {
                                try
                                {
                                    if (item != null)
                                    {
                                        PEM.CompanyPropertyRisk risk = new PEM.CompanyPropertyRisk();
                                        risk.Risk.Id = 0;
                                        risk.Risk.Policy = policy;
                                        risk.Risk.OriginalStatus = item.Risk.Status;
                                        risk.Risk.Status = RiskStatusType.Included;
                                        
                                        lock (objectLock)
                                        {
                                            risk = GetDataRenewal(item);
                                        }
                                        if (risk != null && risk.Risk != null)
                                        {

                                            risk = DelegateService.PropertyService.CreatePropertyTemporal(risk, false);

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
                        throw new Exception(Errors.ErrorVehicleNotFound);
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
        private PEM.CompanyPropertyRisk GetDataRenewal(PEM.CompanyPropertyRisk risk)
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
                    item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                    item.CurrentTo = risk.Risk.Policy.CurrentTo;
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

            risk = DelegateService.PropertyService.QuotateProperty(risk, true, true);

            return risk;
        }

        /// <summary>
        /// Creates the vehicle.
        /// </summary>
        /// <param name="item">The item.</param>
        private CompanyRisk CreateProperty(PEM.CompanyPropertyRisk item)
        {
            item.Risk.IsPersisted = true;
            PEM.CompanyPropertyRisk property = item;

            property = GetDataRenewal(property);
            property.Risk.RiskId = 0;

            property = DelegateService.PropertyService.CreatePropertyTemporal(property, false);

            return property.Risk;
        }
    }
}
