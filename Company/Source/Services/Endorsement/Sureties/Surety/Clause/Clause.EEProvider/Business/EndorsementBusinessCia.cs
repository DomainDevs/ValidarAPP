using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
namespace Sistran.Company.Application.SuretyClauseService.EEProvider.Business
{
    using AutoMapper;
    using Sistran.Company.Application.SuretyClauseService.EEProvider.Assemblers;
    using Sistran.Company.Application.SuretyClauseService.EEProvider.Resources;
    using Sistran.Company.Application.SuretyClauseService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Helper;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Linq;

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class SuretyEndorsementBusinessCia
    {
        UnderwritingServiceEEProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyEndorsementBusinessCia" /> class.
        /// </summary>
        public SuretyEndorsementBusinessCia()
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

                return null;
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
                    policy.UserId = companyEndorsement.UserId;
                    policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    if (policy.Product.IsCollective)
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            List<SEM.CompanyContract> companySuretys = new List<SEM.CompanyContract>();
                            SEM.CompanyContract surety = new SEM.CompanyContract();
                            companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;

                            companySuretys = DelegateService.suretyService.GetCompanySuretyByEndorsementId(companyEndorsement.Id);
                            surety = companySuretys.Where(x => x.Risk.Description == companyEndorsement.DescriptionRisk).FirstOrDefault();

                            policy.Endorsement.EndorsementType = EndorsementType.Modification;
                            policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                            policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                            policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);

                            SEM.CompanyContract risk = GetDataModification(surety, policy, CoverageStatusType.NotModified);

                            List<SEM.CompanyContract> suretys = DelegateService.suretyService.GetCompanySuretiesByTemporalId(companyEndorsement.TemporalId);



                            risk.Risk.Policy = policy;
                            risk = DelegateService.suretyService.CreateSuretyTemporal(risk, true);
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
                List<SEM.CompanyContract> companySuretys = new List<SEM.CompanyContract>();

                if (policy.Product.IsCollective)
                {
                    if (companyEndorsement.DescriptionRisk != null)
                    {
                        companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                        if (companyEndorsement.Id != 0)
                        {
                            companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyEndorsement.PolicyId);
                            companySuretys = companySuretys.Where(x => x.Risk.Description == companyEndorsement.DescriptionRisk).ToList();
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
                    companySuretys = DelegateService.suretyService.GetCompanySuretiesByPolicyId(companyEndorsement.PolicyId);
                }

                policy.UserId = BusinessContext.Current?.UserId ?? companyEndorsement.UserId; 
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
                policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                var mapper = ModelAssembler.CreateMapCompanyClause();
                policy.Clauses = mapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                policy.Summary = new CompanySummary
                {
                    RiskCount = 0
                };
                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                foreach (SEM.CompanyContract item in companySuretys)
                {
                    SEM.CompanyContract risk = GetDataModification(item, policy, CoverageStatusType.NotModified);

                    risk.Risk.Policy = policy;
                    risk = DelegateService.suretyService.CreateSuretyTemporal(risk, false);
                    risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                }
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
        /// <param name="suretyPolicy">The surety policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        private SEM.CompanyContract GetDataModification(SEM.CompanyContract risk, CompanyPolicy suretyPolicy, CoverageStatusType coverageStatusType)
        {
            

            if (risk.Risk.Beneficiaries[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();

                foreach (CompanyBeneficiary item in risk.Risk.Beneficiaries)
                {
                    Beneficiary beneficiary = new Beneficiary();
                    beneficiary = DelegateService.underwritingServiceCore.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                    item.IdentificationDocument = beneficiary.IdentificationDocument;
                    item.Name = beneficiary.Name;
                }
            }

            if (risk.Risk.Premium == 0)
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(suretyPolicy.Product.Id, risk.Risk.GroupCoverage.Id, suretyPolicy.Prefix.Id);

                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();

                foreach (CompanyCoverage item in coverages)
                {
                    item.RiskCoverageId = risk.Risk.Coverages.First(x => x.Id == item.Id).RiskCoverageId;
                    item.CoverageOriginalStatus = risk.Risk.Coverages.First(x => x.Id == item.Id).CoverageOriginalStatus;
                    item.OriginalLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).OriginalLimitAmount;
                    item.OriginalSubLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).OriginalSubLimitAmount;
                    item.CoverStatus = coverageStatusType;
                    item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType);
                    item.EndorsementType = suretyPolicy.Endorsement.EndorsementType;
                    item.CurrentFrom = suretyPolicy.CurrentFrom;
                    item.CurrentTo = suretyPolicy.CurrentTo;
                    item.Rate = risk.Risk.Coverages.First(x => x.Id == item.Id).Rate;
                    item.RateType = risk.Risk.Coverages.First(x => x.Id == item.Id).RateType;
                    item.LimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).LimitAmount;
                    item.SubLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).SubLimitAmount;
                    item.EndorsementLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).EndorsementLimitAmount;
                    item.EndorsementSublimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).EndorsementSublimitAmount;
                    item.Deductible = risk.Risk.Coverages.First(x => x.Id == item.Id).Deductible;
                    item.DynamicProperties = risk.Risk.Coverages.First(x => x.Id == item.Id).DynamicProperties;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.PremiumAmount = 0;
                    //  }
                }
                risk.Risk.Coverages = coverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            return risk;

        }
    }
}
