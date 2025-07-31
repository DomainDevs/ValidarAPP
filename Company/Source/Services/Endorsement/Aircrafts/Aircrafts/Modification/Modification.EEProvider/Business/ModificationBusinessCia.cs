using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Company.Application.AircraftModificationService.EEProvider.Resources;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.AircraftModificationService.EEProvider.Business
{
    public class ModificationBusinessCia
    {
        UnderwritingServiceEEProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModificationBusinessCia" /> class.
        /// </summary>
        public ModificationBusinessCia()
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
                var policy = Services.DelegateService.underwritingService.GetCompanyPolicyByTemporalId(companyEndorsement.TemporalId, false);
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

                    policy = Services.DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    if (policy.Product.IsCollective)
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            List<CompanyAircraft> companyaircrafts = new List<CompanyAircraft>();
                            CompanyAircraft aircraft = new CompanyAircraft();
                            companyEndorsement.Id = Services.DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                            companyaircrafts = Services.DelegateService.aircraftService.GetCompanyAircraftsByEndorsementId(companyEndorsement.Id);
                            aircraft = companyaircrafts.Where(x => x.Make.Description + " - " + x.Model.Description == companyEndorsement.DescriptionRisk).FirstOrDefault();

                            policy.Endorsement.EndorsementType = EndorsementType.Modification;
                            policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                            policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                            policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                            aircraft.Risk.Policy = policy;
                            CompanyAircraft risk = GetDataModification(aircraft, CoverageStatusType.NotModified);

                            List<CompanyAircraft> aircrafts = Services.DelegateService.aircraftService.GetCompanyAircraftsByTemporalId(companyEndorsement.TemporalId);

                            risk.Risk.Policy = policy;
                            risk = Services.DelegateService.aircraftService.CreateCompanyAircraftTemporal(risk);
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

        private CompanyPolicy CreateTempByEndorsement(CompanyEndorsement companyEndorsement)
        {
            try
            {
                if (companyEndorsement == null)
                {
                    throw new ArgumentException(Errors.ErrorEndorsement);
                }
                var policy = Services.DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
                if (policy != null)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();

                    if (policy.Product.IsCollective)
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            companyEndorsement.Id = Services.DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                            if (companyEndorsement.Id != 0)
                            {
                                companyAircrafts = Services.DelegateService.aircraftService.GetCompanyAircraftsByPolicyId(companyEndorsement.PolicyId);
                            }
                            else
                            {
                                throw new BusinessException(Errors.ErrorEndorsement);
                            }
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorEndorsement);
                        }
                    }
                    else
                    {
                        companyAircrafts = Services.DelegateService.aircraftService.GetCompanyAircraftsByPolicyId(companyEndorsement.PolicyId);
                    }

                    //policy.UserId = BusinessContext.Current.UserId;
                    policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                    policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                    policy.EffectPeriod = Services.DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                    policy.Endorsement.Text = new CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    };
                    policy.Product = Services.DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                    policy.Endorsement.EndorsementReasonId = companyEndorsement.EndorsementReasonId;
                    policy.Endorsement.EndorsementType = EndorsementType.Modification;
                    policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                    policy.TemporalType = TemporalType.Endorsement;
                    policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                    var imapper = AutoMapperAssembler.CreateMapCompanyClause();
                    policy.Clauses = imapper.Map<List<Clause>, List<CompanyClause>>(Services.DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                    policy.Summary = new CompanySummary
                    {
                        RiskCount = 0
                    };
                    policy = Services.DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                    companyAircrafts.AsParallel().ForAll(x => x.Risk.Policy = policy);
                    TP.Parallel.ForEach(companyAircrafts, item =>
                    {
                        var risk = GetDataModification(item, CoverageStatusType.NotModified);
                        risk = Services.DelegateService.aircraftService.CreateCompanyAircraftTemporal(risk);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        //risks.Add(risk.Risk);
                    });
                    return policy;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Gets the data modification.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <param name="aircraftPolicy">The aircraft policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        public CompanyAircraft GetDataModification(CompanyAircraft risk, CoverageStatusType coverageStatusType)
        {
            if (risk?.Risk?.Coverages == null)
            {
                throw new Exception(Errors.ErrorRiskEmpty);
            }
            
            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                ConcurrentBag<string> error = new ConcurrentBag<string>();
                if (risk.Risk.Beneficiaries != null)
                {
                    risk.Risk.Beneficiaries.AsParallel().ForAll(
                        item =>
                        {
                            var beneficiary = Services.DelegateService.underwritingServiceCore.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
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
                List<CompanyCoverage> coverages = Services.DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
                if (coverages != null && coverages.Count > 0)
                {
                    //Se dejan  las que vienen el endoso Anterior Definicion Taila
                    List<CompanyCoverage> coveragesAll = new List<CompanyCoverage>(); ;
                    var coveragesData = risk.Risk.Coverages;
                    coveragesData.AsParallel().ForAll(item =>
                    {
                        try
                        {
                            var coverageLocal = coverages.First(z => z.Id == item.Id);
                            item.RuleSetId = coverageLocal.RuleSetId;
                            item.PosRuleSetId = coverageLocal.PosRuleSetId;
                            item.ScriptId = coverageLocal.ScriptId;
                            item.InsuredObject = coverageLocal.InsuredObject;
                        }
                        catch (Exception) { }

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
