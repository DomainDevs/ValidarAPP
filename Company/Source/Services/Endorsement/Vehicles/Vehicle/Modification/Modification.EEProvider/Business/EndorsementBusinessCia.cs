using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.EEProvider;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
namespace Sistran.Company.Application.VehicleModificationService.EEProvider.Business
{
    using Sistran.Company.Application.VehicleModificationService.EEProvider.Assemblers;
    using Sistran.Company.Application.VehicleModificationService.EEProvider.Resources;
    using Sistran.Company.Application.VehicleModificationService.EEProvider.Services;
    using Sistran.Company.Application.Vehicles.VehicleServices.DTOs;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.Helper;
    using Sistran.Core.Application.Utilities.Managers;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using TP = Sistran.Core.Application.Utilities.Utility;

    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class VehicleEndorsementBusinessCia
    {
        UnderwritingServiceEEProvider provider;

        public static object obj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleEndorsementBusinessCia" /> class.
        /// </summary>
        public VehicleEndorsementBusinessCia()
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
                bool isCollective = false; //Antigo Mudulo colectivas no aplica
                CompanyPolicy policy = new CompanyPolicy();
                if (companyEndorsement.TemporalId > 0)
                {
                    policy = CreateTempByTempId(companyEndorsement, isCollective);
                }
                else
                {
                    policy = CreateTempByEndorsement(companyEndorsement, isCollective);
                }

                return policy;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private CompanyPolicy CreateTempByTempId(CompanyEndorsement companyEndorsement, bool isCollective = false)
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
                    #region REQ_221
                    policy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                    policy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                    #endregion

                    if (companyEndorsement.BusinessTypeDescription != 0)
                    {
                        policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyEndorsement.BusinessTypeDescription.ToString());
                    }

                    policy.IssueDate = companyEndorsement.IssueDate;
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                    int riskCount = 0;
                    if (policy.Endorsement.AppRelation == (int)AppSource.R1)
                    {
                        riskCount = DelegateService.vehicleService.GetSummaryRisk(policy.Endorsement);
                    }                    
                    //if (policy.IsCollective) cambio temporal para la marca de colectiva
                    if (policy.PolicyOrigin == PolicyOrigin.Collective)
                    {
                         if (companyEndorsement.DescriptionRisk != null && (companyEndorsement.EndorsementReasonId != 1 || companyEndorsement.EndorsementReasonId != 9))
                          {
                                List<VEM.CompanyVehicle> companyVehicles = new List<VEM.CompanyVehicle>();
                                VEM.CompanyVehicle vehicle = new VEM.CompanyVehicle();
                                companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                                companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByEndorsementId(companyEndorsement.Id);
                                if (companyVehicles == null)
                                {
                                    vehicle = DelegateService.vehicleService.GetVehiclesByPolicyIdEndorsementIdLicensePlate(companyEndorsement.PolicyId, companyEndorsement.Id, companyEndorsement.DescriptionRisk, 0, true).FirstOrDefault();

                                }
                                else
                                {
                                    vehicle = companyVehicles.Where(x => x.LicensePlate == companyEndorsement.DescriptionRisk).FirstOrDefault();
                                }
                                if (DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyEndorsement.TemporalId).Where(x => x.LicensePlate == vehicle.LicensePlate).Count() == 0)
                                {
                                    policy.Endorsement.EndorsementType = EndorsementType.Modification;
                                    policy.Endorsement.EndorsementTypeDescription = EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
                                    policy.CurrentFrom = Convert.ToDateTime(companyEndorsement.CurrentFrom);
                                    policy.CurrentTo = Convert.ToDateTime(companyEndorsement.CurrentTo);
                                    vehicle.Risk.Policy = policy;
                                    VEM.CompanyVehicle risk = GetDataModification(vehicle, CoverageStatusType.NotModified);

                                    List<VEM.CompanyVehicle> vehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyEndorsement.TemporalId);

                                if (policy.Endorsement.EndorsementType == EndorsementType.Emission || (policy.PolicyOrigin == PolicyOrigin.Collective && policy.Endorsement.EndorsementType == EndorsementType.Modification))
                                {

                                        List<string> message = DelegateService.vehicleService.ExistsRisk(vehicles, 0, companyEndorsement.DescriptionRisk, "", "", policy);

                                        if (message.Count == 0)
                                        {
                                            risk.Risk.Policy = policy;
                                            risk.Risk.Coverages.AsParallel().ForAll(x => x.EndorsementType = EndorsementType.Modification);
                                            risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, true);
                                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                                        }
                                        else
                                        {
                                            throw new BusinessException(String.Join(";", message));
                                        }
                                    }
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
        private CompanyPolicy CreateTempByEndorsement(CompanyEndorsement companyEndorsement, bool isCollective = false)
        {
            
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            var policy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(companyEndorsement.Id);
            policy.Id = 0;
            policy.Endorsement.TemporalId = 0;
            if (policy != null)
            {
                Boolean InclusionMassive = false;
                List<VEM.CompanyVehicle> companyVehicles = new List<VEM.CompanyVehicle>();
                int riskCount = 0;
                if (policy.Endorsement.AppRelation == (int)AppSource.R1)
                {
                    riskCount = DelegateService.vehicleService.GetSummaryRisk(policy.Endorsement);
                }
                if (policy.PolicyOrigin == PolicyOrigin.Collective)
                {
                    if (companyEndorsement.EndorsementReasonId == 1 || companyEndorsement.EndorsementReasonId == 9)
                    {
                        InclusionMassive = true;
                    }
                    else
                    {
                        if (companyEndorsement.DescriptionRisk != null)
                        {
                            companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                            if (companyEndorsement.Id != 0)
                            {
                                //companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyEndorsement.PolicyId);
                                companyVehicles = DelegateService.vehicleService.GetVehiclesByPolicyIdEndorsementIdLicensePlate(companyEndorsement.PolicyId, companyEndorsement.Id, companyEndorsement.DescriptionRisk, 0, true);
                                //companyVehicles = companyVehicles.Where(x => x.LicensePlate == companyEndorsement.DescriptionRisk).ToList();
                                companyVehicles.AsParallel().ForAll(
                                 x =>
                                 {
                                     x.Risk.Id = 0;
                                     x.Risk.OriginalStatus = x.Risk.Status;
                                     x.Risk.Status = RiskStatusType.NotModified;
                                     if (x.Accesories?.Count > 0)
                                     {
                                         x.Accesories.AsParallel().ForAll(y =>
                                         {
                                             y.Premium = 0;
                                         });
                                     }
                                 });

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
                }
                else
                {
                    companyVehicles = DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(companyEndorsement.PolicyId);
                    companyVehicles.AsParallel().ForAll(
                     x =>
                     {
                         x.Risk.Id = 0;
                         x.Risk.OriginalStatus = x.Risk.Status;
                         x.Risk.Status = RiskStatusType.NotModified;
                         if (x.Accesories?.Count > 0)
                         {
                             x.Accesories.AsParallel().ForAll(y =>
                             {
                                 y.Premium = 0;
                             });
                         }
                     });
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
                var mapper = ModelAssembler.CreateMapCompanyClause();
                policy.Clauses = mapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(EmissionLevel.General, policy.Prefix.Id).Where(x => x.IsMandatory).ToList());

                policy.Summary = new CompanySummary
                {
                    RiskCount = 0
                };
                #region REQ_561
                policy.Endorsement.TicketNumber = companyEndorsement.TicketNumber;
                policy.Endorsement.TicketDate = companyEndorsement.TicketDate;
                #endregion                
                policy.IssueDate = companyEndorsement.IssueDate;
                if (companyEndorsement.BusinessTypeDescription != 0)
                {
                    policy.CoInsuranceCompanies.Where(x => x.PolicyNumber != null).ToList().ForEach(y => y.PolicyNumber = companyEndorsement.BusinessTypeDescription.ToString());
                }
                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

                if (!InclusionMassive)

                    if (companyVehicles?.Count > 0 && policy != null)
                    { 
                    companyVehicles.AsParallel().ForAll(x =>
                    {
                        x.Risk.Policy = policy;
                    });
                    ConcurrentBag<string> errors = new ConcurrentBag<string>();
                    TP.Parallel.ForEach(companyVehicles, item =>
                    {
                        try
                        {
                            var risk = GetDataModification(item, CoverageStatusType.NotModified);
                            risk.Risk.Status = RiskStatusType.NotModified;
                            risk.Risk.Coverages.AsParallel().ForAll(x => x.EndorsementType = EndorsementType.Modification);
                            if (risk?.Accesories?.Count > 0)
                            {

                                List<AccessoryDTO> accessoryDTOs = DelegateService.vehicleService.GetPremiumAccesory(policy.Endorsement.PolicyId, risk.Risk.Number, QuoteManager.CalculateEffectiveDays(policy.CurrentFrom, policy.CurrentTo));
                                foreach (VEM.CompanyAccessory accessory in risk.Accesories)
                                {
                                    accessory.AccumulatedPremium = accessoryDTOs.Where(m => m.Id == accessory.RiskDetailId).First().premium;
                                }
                            }
                            risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, false);
                            risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                        }
                        catch (Exception ex)
                        {

                            errors.Add(ex.Message);
                        }

                    });

                    if (errors?.Count > 0)
                    {
                        throw new BusinessException(string.Join(Environment.NewLine, errors.ToList()));
                    }
                    else
                    {
                        return policy;
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }
                return policy;
            }
            else
            {
                throw new BusinessException(Errors.ErrorTemporalNotFound);
            }
        }

        private List<VEM.CompanyVehicle> CreateRiskCollective(CompanyEndorsement companyEndorsement)
        {
            List<VEM.CompanyVehicle> companyVehicles = new List<VEM.CompanyVehicle>();
            if (companyEndorsement.DescriptionRisk != null)
            {
                companyEndorsement.Id = DelegateService.underwritingService.GetCurrentEndorsementByPolicyIdLicensePlateId(companyEndorsement.PolicyId, companyEndorsement.DescriptionRisk).Id;
                if (companyEndorsement.Id != 0)
                {
                    companyVehicles = DelegateService.vehicleService.GetVehiclesByPolicyIdEndorsementIdLicensePlate(companyEndorsement.PolicyId, companyEndorsement.Id, companyEndorsement.DescriptionRisk, 0, true);
                }
                else
                {
                    throw new BusinessException(Errors.ErrorLicensePlate);
                }
                return companyVehicles;
            }
            else
            {
                throw new BusinessException(Errors.ErrorLicensePlate);
            }

        }
        /// <summary>
        /// Gets the data modification.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <param name="vehiclePolicy">The vehicle policy.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        public VEM.CompanyVehicle GetDataModification(VEM.CompanyVehicle risk, CoverageStatusType coverageStatusType)
        {
            ConcurrentBag<string> error = new ConcurrentBag<string>();
            if (risk?.Risk?.Coverages == null)
            {
                throw new Exception(Errors.ErrorRiskEmpty);
            }
            String coverStatusName = String.Empty;
            if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType)) == null)
            {
                coverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType);
            }
            else
            {
                coverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(coverageStatusType));
            }
            risk.Risk.Description = risk.LicensePlate;

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
                        throw new Exception(string.Join(Environment.NewLine, error));
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorBeneficiaryEmpty);
                }
            }
            if (risk?.Risk?.Premium == 0)
            {
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
                if (coverages?.Count > 0)
                {
                    //Se dejan  las que vienen el endoso Anterior Definicion Taila
                    var coveragesData = risk.Risk.Coverages;
                    coveragesData.AsParallel().ForAll(item =>
                    {
                        try
                        {
                            var coverageLocal = coverages.FirstOrDefault(z => z.Id == item.Id);
                            if (!(coverageLocal == null))
                            {
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
                                item.CurrentFromOriginal = risk.Risk.Policy.CurrentFrom;
                                item.CurrentToOriginal = risk.Risk.Policy.CurrentTo;
                                item.AccumulatedPremiumAmount = 0;
                                item.PremiumAmount = 0;
                                item.EndorsementLimitAmount = 0;
                                item.EndorsementSublimitAmount = 0;
                                item.OriginalRate = item.Rate;
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
        private void CreateTemRiskCollective(CompanyEndorsement companyEndorsement, CompanyPolicy policy)
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
                vehicle.Risk.Policy = policy;
                VEM.CompanyVehicle risk = GetDataModification(vehicle, CoverageStatusType.NotModified);

                List<VEM.CompanyVehicle> vehicles = DelegateService.vehicleService.GetCompanyVehiclesByTemporalId(companyEndorsement.TemporalId);

                if (policy.Endorsement.EndorsementType == EndorsementType.Emission)
                {

                    List<string> message = DelegateService.vehicleService.ExistsRisk(vehicles, 0, companyEndorsement.DescriptionRisk, "", "", policy);

                    if (message.Count == 0)
                    {
                        risk.Risk.Policy = policy;
                        risk.Risk.Coverages.AsParallel().ForAll(x => x.EndorsementType = EndorsementType.Modification);
                        risk = DelegateService.vehicleService.CreateVehicleTemporal(risk, true);
                        risk.Risk.Policy.InfringementPolicies.AddRange(risk.Risk.InfringementPolicies);
                    }
                    else
                    {
                        throw new BusinessException(String.Join(";", message));
                    }
                }
            }
        }
    }
}
