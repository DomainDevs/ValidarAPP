using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.LiabilityModificationService.EEProvider.Business
{
    using AutoMapper;
    using Sistran.Company.Application.LiabilityModificationService.EEProvider.Assemblers;
    using Sistran.Company.Application.LiabilityModificationService.EEProvider.Resources;
    using Sistran.Company.Application.LiabilityModificationService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Helper;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Collections.Concurrent;
    using System.Linq;   
    using Sistran.Core.Application.Utilities.Utility;

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class LiabilityEndorsementBusinessCia
    {
        UnderwritingServiceEEProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiabilityEndorsementBusinessCia" /> class.
        /// </summary>
        public LiabilityEndorsementBusinessCia()
        {
            provider = new UnderwritingServiceEEProvider();
        }
        /// <summary>
        /// Creates the policy temporal.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <param name="isSaveTemp">if set to <c>true</c> [is save temporary].</param>
        /// <returns></returns>
        public CompanyPolicy CreatePolicyTemporal(CompanyEndorsement companyEndorsement, bool isMasive, bool isSaveTemp)
        {
            return CreateEndorsement(companyEndorsement);
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        private CompanyPolicy CreateEndorsement(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                if (companyEndorsement.TemporalId > 0)
                {
                    policy = CreateTempByTempId(companyEndorsement);
                }
                else
                {
                    policy = CreateTempByEndorsement(companyEndorsement);
                }

                return policy;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private CompanyPolicy CreateTempByTempId(CompanyEndorsement companyEndorsement)
        {
            try
            {
                if (companyEndorsement == null)
                {
                    throw new ArgumentException(Errors.ErrorEndorsement);
                }
                var policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
                if (policy != null)
                {
                    policy.Agencies?.ForEach(x =>
                    {
                        if (x.DateDeclined != null)
                        {
                            throw new ArgumentException(Errors.ErrorAgentEmpty);
                        }
                    });
                    policy.UserId = companyEndorsement.UserId;
                    policy.CurrentFrom = companyEndorsement.CurrentFrom;
                    policy.CurrentTo = companyEndorsement.CurrentTo;
                    policy.Endorsement.EndorsementReasonId = companyEndorsement.EndorsementReasonId;
                    policy.Endorsement.EndorsementType = EndorsementType.Modification;
                    policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    policy.Endorsement.Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    };

                    policy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                    policy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                    policy.Endorsement.IsMassive = companyEndorsement.IsMassive;
                    if (companyEndorsement.BusinessTypeDescription != 0)
                    {
                        policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyEndorsement.BusinessTypeDescription.ToString());
                    }
                    policy.Endorsement.ModificationTypeId = companyEndorsement.ModificationTypeId;
                    /*NASE-2338*/
                    if (policy.ExchangeRate != null && policy.ExchangeRate.Currency != null)
                    {
                        DateTime IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                        policy.ExchangeRate.SellAmount = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId
                            (IssueDate, policy.ExchangeRate.Currency.Id).SellAmount;
                    }
                    policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    if (policy.Product.IsCollective)
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            List<LEM.CompanyLiabilityRisk> companyLibiality = new List<LEM.CompanyLiabilityRisk>();
                            LEM.CompanyLiabilityRisk libiality = new LEM.CompanyLiabilityRisk();
                            companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                            companyLibiality = DelegateService.LiabilityService.GetCompanyLiabilitiesByEndorsementId(companyEndorsement.Id);
                            
                            policy.Endorsement.EndorsementType = EndorsementType.Modification;
                            policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                            policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                            policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                            libiality.Risk.Policy = policy;
                            LEM.CompanyLiabilityRisk risk = GetDataModification(libiality, CoverageStatusType.NotModified);

                            List<LEM.CompanyLiabilityRisk> ThirdPartyLiabilities = DelegateService.LiabilityService.GetCompanyLiabilitiesByTemporalId(companyEndorsement.TemporalId);

                            risk.Risk.Policy = policy;                    
                            risk = DelegateService.LiabilityService.CreateLiabilityTemporal(risk, true);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);

                        }
                    }
                    return policy;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.GetBaseException().Message, ex);
            }

        }

        /// <summary>
        /// Creates the temporary by endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// </exception>
        private CompanyPolicy CreateTempByEndorsement(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            var policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
            if (policy != null)
            {
                policy.Agencies?.ForEach(x =>
                {
                    if (x.DateDeclined != null)
                    {
                        throw new ArgumentException(Errors.ErrorAgentEmpty);
                    }
                });
                List<LEM.CompanyLiabilityRisk> companyLiabilities = new List<LEM.CompanyLiabilityRisk>();

                if (policy.PolicyOrigin.Equals(PolicyOrigin.Collective))
                {
                    if (companyEndorsement.DescriptionRisk != null)
                    {
                        companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                        if (companyEndorsement.Id != 0)
                        {
                            companyLiabilities = DelegateService.LiabilityService.GetCompanyLiebilitiesByPolicyId(companyEndorsement.PolicyId);
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorLicensePlate);
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorLicensePlate);
                    }
                }
                else
                {
                    companyLiabilities = DelegateService.LiabilityService.GetCompanyLiebilitiesByPolicyId(companyEndorsement.PolicyId);
                }

                policy.UserId = companyEndorsement.UserId;
                policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                policy.Endorsement.Text = new CompanyText
                {
                    TextBody = companyEndorsement.Text.TextBody,
                    Observations = companyEndorsement.Text.Observations
                };
                policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                policy.Endorsement.EndorsementReasonId = companyEndorsement.EndorsementReasonId;
                policy.Endorsement.EndorsementType = EndorsementType.Modification;
                policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                policy.TemporalType = TemporalType.Endorsement;
                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                policy.Endorsement.ModificationTypeId = companyEndorsement.ModificationTypeId;
                policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                var mapper = ModelAssembler.CreateMapCompanyClause();
                List<CompanyClause> companyClauses = mapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());
                if (companyClauses != null && companyClauses.Count > 0)
                {
                    if (policy.Clauses != null && policy.Clauses.Count > 0)
                    {
                        foreach (var item in companyClauses)
                        {
                            if (!policy.Clauses.Exists(x => x.Id == item.Id))
                            {
                                policy.Clauses.Add(item);
                            }
                        }
                    }
                    else
                    {
                        policy.Clauses = companyClauses;
                    }
                }

                policy.Summary = new CompanySummary
                {
                    RiskCount = 0
                };
                if (companyEndorsement.BusinessTypeDescription != 0)
                {
                    policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyEndorsement.BusinessTypeDescription.ToString());
                }
                policy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                policy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                policy.Endorsement.IsMassive = companyEndorsement.IsMassive;
                policy.IssueDate = companyEndorsement.IssueDate;

                if (policy.PayerComponents != null)
                {
                    var companyComponentsPayer = policy.PayerComponents.ToList();
                    foreach (var item in companyComponentsPayer)
                    {

                        item.Amount = 0;
                        item.AmountExpense = 0;
                        item.BaseAmount = 0;
                    }
                    policy.PayerComponents = companyComponentsPayer;
                }

                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                companyLiabilities.AsParallel().ForAll(x => x.Risk.Policy = policy);
                TP.Parallel.ForEach(companyLiabilities, item =>
                {
                    var risk = GetDataModification(item, CoverageStatusType.NotModified);
                    risk.Risk.Coverages.AsParallel().ForAll(x => x.EndorsementType = EndorsementType.Modification);
                    risk = DelegateService.LiabilityService.CreateLiabilityTemporal(risk, false);
                    risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                });
                return policy;
            }
            else
            {
                throw new BusinessException(Errors.ErrorTemporalNotFound);
            }
        }

        /// <summary>
        /// Gets the data modification.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <param name="liabilityPolicy">The liability policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        public LEM.CompanyLiabilityRisk GetDataModification(LEM.CompanyLiabilityRisk risk, CoverageStatusType coverageStatusType)
        {
            ConcurrentBag<string> error = new ConcurrentBag<string>();
            if (risk?.Risk?.Coverages == null)
            {
                throw new Exception(Errors.ErrorRiskEmpty);
            }
           
            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();

                if (risk.Risk.Beneficiaries != null)
                {
                    risk.Risk.Beneficiaries.AsParallel().ForAll(
                        item =>
                        {
                            var beneficiary = DelegateService.underwritingServiceCore.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
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
            if (risk != null && risk.Risk.Premium == 0)
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
                if (coverages != null && coverages.Count > 0)
                {
                    var coveragesData = risk.Risk.Coverages;
                    coveragesData.AsParallel().ForAll(item =>
                    {
                        try
                        {
                            var coverageLocal = coverages.First(z => z.Id == item.Id);
                            if (!(coverageLocal == null))
                            {
                                item.RuleSetId = coverageLocal.RuleSetId;
                                item.PosRuleSetId = coverageLocal.PosRuleSetId;
                                item.ScriptId = coverageLocal.ScriptId;
                                item.OriginalLimitAmount = item.LimitAmount;
                                item.OriginalSubLimitAmount = item.SubLimitAmount;
                                item.CoverageOriginalStatus = item.CoverStatus;
                                item.CoverStatus = coverageStatusType;
                                item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType);
                                item.CurrentFromOriginal = item.CurrentFrom;
                                item.CurrentToOriginal = (DateTime)item.CurrentTo;
                                item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                                item.CurrentTo = risk.Risk.Policy.CurrentTo;
                                item.AccumulatedPremiumAmount = 0;
                                item.FlatRatePorcentage = 0;
                                item.PremiumAmount = 0;
                                item.IsSelected = coverageLocal.IsSelected;
                                item.IsMandatory = coverageLocal.IsMandatory;
                                item.IsVisible = coverageLocal.IsVisible;
                                item.EndorsementLimitAmount = 0;
                                item.EndorsementSublimitAmount = 0;
                                item.OriginalRate = item.Rate;
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            error.Add(ex.Message);
                        }

                    });
                    risk.Risk.Coverages = coveragesData;
                    risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
                }
                else
                {
                    throw new Exception(Errors.ErrorCoverages);
                }
            }
            return risk;

        }

    }
}
