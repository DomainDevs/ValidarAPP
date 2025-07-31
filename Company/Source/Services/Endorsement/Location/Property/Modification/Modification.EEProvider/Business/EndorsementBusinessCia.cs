using COISSE = Sistran.Company.Application.UnderwritingServices;
using COISSProvider = Sistran.Company.Application.UnderwritingServices.EEProvider;
using ISSModel = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;
using COISSModel = Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.PropertyModificationService.EEProvider.Services;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.PropertyModificationService.EEProvider.Assemblers;
using AutoMapper;
using System.Linq;
using TM = System.Threading.Tasks;
using System.Collections.Concurrent;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.PropertyModificationService.EEProvider.Resources;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.PropertyModificationService.EEProvider.Business
{

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class PropertyEndorsementBusinessCia
    {
        COISSProvider.UnderwritingServiceEEProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyEndorsementBusinessCia" /> class.
        /// </summary>
        public PropertyEndorsementBusinessCia()
        {
            provider = new COISSProvider.UnderwritingServiceEEProvider();
        }
        /// <summary>
        /// Creates the policy temporal.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <param name="isSaveTemp">if set to <c>true</c> [is save temporary].</param>
        /// <returns></returns>
        public COISSModel.CompanyPolicy CreatePolicyTemporal(COISSE.CompanyEndorsement companyEndorsement, bool isMasive, bool isSaveTemp)
        {
            return CreateEndorsement(companyEndorsement);
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        private COISSModel.CompanyPolicy CreateEndorsement(COISSE.CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                COISSModel.CompanyPolicy policy = new COISSModel.CompanyPolicy();
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
        private COISSModel.CompanyPolicy CreateTempByTempId(COISSE.CompanyEndorsement companyEndorsement)
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
                    policy.Endorsement.Text = new COISSModel.CompanyText
                    {
                        TextBody = companyEndorsement.Text.TextBody,
                        Observations = companyEndorsement.Text.Observations
                    };
                    policy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                    policy.Endorsement.TicketNumber = (int)companyEndorsement.TicketNumber;
                    policy.Endorsement.ModificationTypeId = companyEndorsement.ModificationTypeId;
                    if (companyEndorsement.BusinessTypeDescription != 0)
                    {
                        policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyEndorsement.BusinessTypeDescription.ToString());
                    }
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    if (policy.PolicyOrigin.Equals(PolicyOrigin.Collective))
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            List<PEM.CompanyPropertyRisk> companyProperty = new List<PEM.CompanyPropertyRisk>();
                            PEM.CompanyPropertyRisk property = new PEM.CompanyPropertyRisk();
                            companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                            companyProperty = DelegateService.propertyService.GetCompanyPropertiesByEndorsementId(companyEndorsement.Id);
                            property = companyProperty.Where(x => x.FullAddress == companyEndorsement.DescriptionRisk).FirstOrDefault();

                            policy.Endorsement.EndorsementType = EndorsementType.Modification;
                            policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                            policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                            policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                            property.Risk.Policy = policy;
                            PEM.CompanyPropertyRisk risk = GetDataModification(property, CoverageStatusType.NotModified);

                            List<PEM.CompanyPropertyRisk> properties = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(companyEndorsement.TemporalId);

                            risk.Risk.Policy = policy;
                            risk = DelegateService.propertyService.CreatePropertyTemporal(risk, true);
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
        private COISSModel.CompanyPolicy CreateTempByEndorsement(COISSE.CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            var policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
            if (policy != null)
            {
                List<PEM.CompanyPropertyRisk> companyPropeties = new List<PEM.CompanyPropertyRisk>();

                if (policy.PolicyOrigin.Equals(PolicyOrigin.Collective))
                {
                    if (companyEndorsement.DescriptionRisk != null)
                    {
                        companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                        if (companyEndorsement.Id != 0)
                        {
                            companyPropeties = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyEndorsement.PolicyId);

                            companyPropeties = companyPropeties.Where(x => x.FullAddress == companyEndorsement.DescriptionRisk).ToList();
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
                    companyPropeties = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(companyEndorsement.PolicyId);
                }

                policy.UserId = companyEndorsement.UserId;
                policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                policy.Endorsement.Text = new COISSModel.CompanyText
                {
                    TextBody = companyEndorsement.Text.TextBody,
                    Observations = companyEndorsement.Text.Observations
                };
                policy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                policy.Endorsement.TicketNumber = (int)companyEndorsement.TicketNumber;
                policy.Endorsement.ModificationTypeId = companyEndorsement.ModificationTypeId;
                if (companyEndorsement.BusinessTypeDescription != 0)
                {
                    policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyEndorsement.BusinessTypeDescription.ToString());
                }
                policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                policy.Endorsement.EndorsementReasonId = companyEndorsement.EndorsementReasonId;
                policy.Endorsement.EndorsementType = EndorsementType.Modification;
                policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                policy.TemporalType = TemporalType.Endorsement;
                policy.TemporalTypeDescription = EnumHelper.GetItemName<TemporalType>(policy.TemporalType);
                var mapper = ModelAssembler.CreateMapCompanyClause();
                policy.Clauses = mapper.Map<List<ISSModel.Clause>, List<COISSModel.CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                policy.Summary = new COISSModel.CompanySummary
                {
                    RiskCount = 0
                };
                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                if (companyPropeties?.Count > 0)
                {
                    companyPropeties.AsParallel().ForAll(x => x.Risk.Policy = policy);
                    TP.Parallel.ForEach(companyPropeties, item =>
                    {
                        var risk = GetDataModification(item, CoverageStatusType.NotModified);
                        risk.Risk.Coverages.AsParallel().ForAll(x => x.EndorsementType = EndorsementType.Modification);
                        risk = DelegateService.propertyService.CreatePropertyTemporal(risk, false);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                    });
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
        /// <param name="vehiclePolicy">The vehicle policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        public PEM.CompanyPropertyRisk GetDataModification(PEM.CompanyPropertyRisk risk, CoverageStatusType coverageStatusType)
        {
            ConcurrentBag<string> error = new ConcurrentBag<string>();
            if (risk?.Risk?.Coverages == null)
            {
                throw new Exception(Errors.ErrorRiskEmpty);
            }
            risk.Risk.Description = risk.FullAddress;

            if (risk.Risk?.Beneficiaries?[0].IdentificationDocument == null)
            {
                List<COISSModel.CompanyBeneficiary> beneficiaries = new List<COISSModel.CompanyBeneficiary>();

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
                List<COISSModel.CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
                //var insuredObjects = DelegateService.underwritingService.GetInsuredObjectsByRiskId(risk.Risk.Id);
                if (coverages?.Count > 0)
                {
                    //Se toma de base vehiculos
                    //Se dejan  las que vienen del endoso Anterior Definicion Taila
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
                                item.CurrentFrom = risk.Risk.Policy.CurrentFrom;
                                item.CurrentTo = risk.Risk.Policy.CurrentTo;
                                item.CurrentFromOriginal= risk.Risk.Policy.CurrentFrom;
                                item.CurrentToOriginal = risk.Risk.Policy.CurrentTo;
                                item.AccumulatedPremiumAmount = 0;
                                item.FlatRatePorcentage = 0;
                                item.PremiumAmount = 0;
                                item.MainCoverageId = coverageLocal.MainCoverageId;
                                item.IsSelected = coverageLocal.IsSelected;
                                item.IsMandatory = coverageLocal.IsMandatory;
                                item.IsVisible = coverageLocal.IsVisible;
                            }
                        }
                        catch (Exception ex)
                        {
                            error.Add(ex.Message);
                        }
                    });
                    if (error.Any())
                    {
                        throw new Exception(string.Join(Environment.NewLine, error));
                    }
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
