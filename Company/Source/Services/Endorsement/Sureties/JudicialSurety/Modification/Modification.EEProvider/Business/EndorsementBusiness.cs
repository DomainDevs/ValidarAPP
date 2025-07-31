using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using JSEM = Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
namespace Sistran.Company.Application.JudicialSuretyModificationService.EEProvider.Business
{
    using AutoMapper;
    using Sistran.Company.Application.JudicialSuretyModificationService.EEProvider.Assemblers;
    using Sistran.Company.Application.JudicialSuretyModificationService.EEProvider.Resources;
    using Sistran.Company.Application.JudicialSuretyModificationService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Helper;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using TP = Sistran.Core.Application.Utilities.Utility;

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class JudicialSuretyEndorsementBusinessCompany
    {
        UnderwritingServiceEEProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuretyEndorsementBusinessCompany" /> class.
        /// </summary>
        public JudicialSuretyEndorsementBusinessCompany()
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
                    policy.CurrentFrom = companyEndorsement.CurrentFrom;
                    policy.CurrentTo = companyEndorsement.CurrentTo;
                    policy.Endorsement.EndorsementReasonId = companyEndorsement.EndorsementReasonId;
                    policy.Endorsement.EndorsementType = EndorsementType.Modification;
                    policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();

                    policy.Endorsement.Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    };
                    policy.Endorsement.ModificationTypeId = companyEndorsement.ModificationTypeId;
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    if (policy.Product.IsCollective)
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            List<JSEM.CompanyJudgement> companySuretys = new List<JSEM.CompanyJudgement>();
                            JSEM.CompanyJudgement surety = new JSEM.CompanyJudgement();
                            companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;

                            companySuretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByEndorsementId(companyEndorsement.Id);
                            surety = companySuretys.Where(x => x.Risk.Description == companyEndorsement.DescriptionRisk).FirstOrDefault();

                            policy.Endorsement.EndorsementType = EndorsementType.Modification;
                            policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                            policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                            policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                            surety.Risk.Policy = policy;
                            JSEM.CompanyJudgement risk = GetDataModification(surety, CoverageStatusType.NotModified);


                            List<JSEM.CompanyJudgement> suretys = DelegateService.judicialsuretyService.GetCompanyJudgementsByTemporalId(companyEndorsement.TemporalId);

                            risk.Risk.Policy = policy;
                            risk = DelegateService.judicialsuretyService.CreateJudgementTemporal(risk, true);
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
                List<JSEM.CompanyJudgement> companySureties = new List<JSEM.CompanyJudgement>();

                if (policy.Product.IsCollective)
                {
                    if (companyEndorsement.DescriptionRisk != null)
                    {
                        companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                        if (companyEndorsement.Id != 0)
                        {
                            companySureties = DelegateService.judicialsuretyService.GetCompanyJudicialSuretyByPolicyId(companyEndorsement.PolicyId);
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
                    companySureties = DelegateService.judicialsuretyService.GetCompanyJudicialSuretyByPolicyId(companyEndorsement.PolicyId);
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
                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                companySureties.AsParallel().ForAll(x => x.Risk.Policy = policy);
                TP.Parallel.ForEach(companySureties, item =>
                {
                    var risk = GetDataModification(item, CoverageStatusType.NotModified);
                    risk = DelegateService.judicialsuretyService.CreateJudgementTemporal(risk, false);
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
        /// <param name="suretyPolicy">The surety policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        public JSEM.CompanyJudgement GetDataModification(JSEM.CompanyJudgement risk, CoverageStatusType coverageStatusType)
        {
            if (risk?.Risk?.Coverages == null)
            {
                throw new Exception(Errors.ErrorRiskEmpty);
            }
            risk.Risk.Description = risk.Risk.MainInsured.Name;

            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                ConcurrentBag<string> error = new ConcurrentBag<string>();
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
                    //Se dejan  las que vienen el endoso Anterior Definicion Taila
                    List<CompanyCoverage> coveragesAll = new List<CompanyCoverage>(); ;
                    var coveragesData = risk.Risk.Coverages;
                    coveragesData.AsParallel().ForAll(item =>
                    {
                        var coverageLocal = coverages.First(z => z.Id == item.Id);
                        item.RuleSetId = coverageLocal.RuleSetId;
                        item.PosRuleSetId = coverageLocal.PosRuleSetId;
                        item.ScriptId = coverageLocal.ScriptId;
                        item.InsuredObject = coverageLocal.InsuredObject;
                        item.OriginalLimitAmount = item.LimitAmount;
                        item.OriginalSubLimitAmount = item.SubLimitAmount;
                        item.CoverageOriginalStatus = item.CoverStatus;
                        item.CoverStatus = coverageStatusType;
                        item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType);
                        item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                        item.CurrentTo = risk.Risk.Policy.CurrentTo;
                        item.AccumulatedPremiumAmount = 0;
                        item.FlatRatePorcentage = 0;
                        item.PremiumAmount = 0;
                        item.IsSelected = coverageLocal.IsSelected;
                        item.IsMandatory = coverageLocal.IsMandatory;
                        item.IsVisible = coverageLocal.IsVisible;
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
