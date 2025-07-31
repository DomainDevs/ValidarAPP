using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
namespace Sistran.Company.Application.VehicleClauseService.EEProvider.Business
{
    using Sistran.Company.Application.VehicleClauseService.EEProvider.Assemblers;
    using Sistran.Company.Application.VehicleClauseService.EEProvider.Resources;
    using Sistran.Company.Application.VehicleClauseService.EEProvider.Services;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Helper;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Linq;

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class VehicleEndorsementBusinessCia
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleEndorsementBusinessCia" /> class.
        /// </summary>
        public VehicleEndorsementBusinessCia()
        {
            UnderwritingServiceEEProvider provider = new UnderwritingServiceEEProvider();
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

                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                    if (policy.PolicyOrigin == PolicyOrigin.Collective)
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            List<VEM.CompanyVehicle> companyVehicles = new List<VEM.CompanyVehicle>();
                            VEM.CompanyVehicle vehicle = new VEM.CompanyVehicle();
                            companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                            companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByEndorsementId(companyEndorsement.Id);
                            vehicle = companyVehicles.Where(x => x.LicensePlate == companyEndorsement.DescriptionRisk).FirstOrDefault();

                            policy.Endorsement.EndorsementType = EndorsementType.Modification;
                            policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                            policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                            policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);

                            VEM.CompanyVehicle risk = GetDataModification(vehicle, policy, CoverageStatusType.NotModified);

                            List<VEM.CompanyVehicle> vehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyEndorsement.TemporalId);

                            List<string> message = DelegateService.vehicleService.ExistsRisk(vehicles, 0, companyEndorsement.DescriptionRisk, "", "", policy);

                            if (message.Count == 0)
                            {
                                risk.Risk.Policy = policy;
                                risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, true);
                                risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                            }
                            else
                            {
                                throw new BusinessException(String.Join(";", message));
                            }
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
                List<VEM.CompanyVehicle> companyVehicles = new List<VEM.CompanyVehicle>();

                if (policy.PolicyOrigin == PolicyOrigin.Collective)
                {
                    if (companyEndorsement.DescriptionRisk != null)
                    {
                        companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                        if (companyEndorsement.Id != 0)
                        {
                            companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyEndorsement.PolicyId);
                            companyVehicles = companyVehicles.Where(x => x.LicensePlate == companyEndorsement.DescriptionRisk).ToList();
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
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyEndorsement.PolicyId);
                    companyVehicles.AsParallel().ForAll(

                                    x =>
                                    {
                                        if (x.Accesories?.Count > 0)
                                        {
                                            x.Accesories.AsParallel().ForAll(y =>
                                              {
                                                  y.Premium = 0;
                                              });
                                        }
                                    });
                }

                //policy.UserId = BusinessContext.Current.UserId;
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
                var mapper = ModelAssembler.CreateMapCompanyClause();
                policy.Clauses = mapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                policy.Summary = new CompanySummary
                {
                    RiskCount = 0
                };
                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                foreach (VEM.CompanyVehicle item in companyVehicles)
                {
                    VEM.CompanyVehicle risk = GetDataModification(item, policy, CoverageStatusType.NotModified);

                    risk.Risk.Policy = policy;
                    risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, false);
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
        /// <param name="vehiclePolicy">The vehicle policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        private VEM.CompanyVehicle GetDataModification(VEM.CompanyVehicle risk, CompanyPolicy vehiclePolicy, CoverageStatusType coverageStatusType)
        {
            risk.Risk.Description = risk.LicensePlate;
            String coverStatusName = String.Empty;
            if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType)) == null)
            {
                coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType);
            }
            else
            {
                coverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType));
            }
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
                risk.Risk.Status = RiskStatusType.Modified;
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(vehiclePolicy.Product.Id, risk.Risk.GroupCoverage.Id, vehiclePolicy.Prefix.Id);

                coverages = coverages.Where(c => (risk.Risk.Coverages.Any(x => x.Id == c.Id))).ToList();

                foreach (CompanyCoverage item in coverages)
                {
                    item.RiskCoverageId = risk.Risk.Coverages.First(x => x.Id == item.Id).RiskCoverageId;
                    item.CoverageOriginalStatus = risk.Risk.Coverages.First(x => x.Id == item.Id).CoverageOriginalStatus;
                    item.OriginalLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).OriginalLimitAmount;
                    item.OriginalSubLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).OriginalSubLimitAmount;
                    item.CoverStatus = coverageStatusType;
                    item.CoverStatusName = coverStatusName;
                    item.EndorsementType = vehiclePolicy.Endorsement.EndorsementType;
                    item.CurrentFrom = vehiclePolicy.CurrentFrom;
                    item.CurrentTo = vehiclePolicy.CurrentTo;
                    item.Rate = risk.Risk.Coverages.First(x => x.Id == item.Id).Rate;
                    item.RateType = risk.Risk.Coverages.First(x => x.Id == item.Id).RateType;
                    item.LimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).LimitAmount;
                    item.SubLimitAmount = risk.Risk.Coverages.First(x => x.Id == item.Id).SubLimitAmount;
                    item.EndorsementLimitAmount = 0;
                    item.EndorsementSublimitAmount = 0;
                    item.Deductible = risk.Risk.Coverages.First(x => x.Id == item.Id).Deductible;
                    item.DynamicProperties = risk.Risk.Coverages.First(x => x.Id == item.Id).DynamicProperties;
                    item.AccumulatedPremiumAmount = 0;
                    item.FlatRatePorcentage = 0;
                    item.PremiumAmount = 0;
                }
                risk.Risk.Coverages = coverages;
                risk.Risk.AmountInsured = risk.Risk.Coverages.Sum(x => x.LimitAmount);
            }
            return risk;

        }
    }
}
