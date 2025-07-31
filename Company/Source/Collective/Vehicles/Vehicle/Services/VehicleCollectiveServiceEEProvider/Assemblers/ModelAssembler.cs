using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using CommonModels = Sistran.Core.Application.CommonService.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using VEMO = Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;

namespace Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {



        #region TemporalVehicle

        public static CompanyVehicle CreateTemporalVehicle(TMPEN.TempRisk tempRisk, TMPEN.CoTempRisk coTempRisk, TMPEN.TempRiskVehicle tempRiskVehicle, TMPEN.CoTempRiskVehicle coTempRiskVehicle)
        {
            CompanyVehicle model = new CompanyVehicle
            {
                Risk = new CompanyRisk
                {
                    Id = tempRisk.OperationId.GetValueOrDefault(),
                    RiskId = tempRisk.RiskId,
                    Description = tempRiskVehicle.LicensePlate,
                    MainInsured = new CompanyIssuanceInsured
                    {
                        IndividualId = tempRisk.InsuredId.Value,
                        CustomerType = (CustomerType)tempRisk.CustomerTypeCode.Value,
                        CompanyName = new IssuanceCompanyName
                        {
                            NameNum = tempRisk.NameNum.GetValueOrDefault(),
                            Address = new IssuanceAddress
                            {
                                Id = tempRisk.AddressId.GetValueOrDefault()
                            }
                        }
                    },
                    CoveredRiskType = (CoveredRiskType)tempRisk.CoveredRiskTypeCode,
                    Status = (RiskStatusType)tempRisk.RiskStatusCode,
                    RatingZone = new CompanyRatingZone
                    {
                        Id = tempRisk.RatingZoneCode.Value
                    },
                    GroupCoverage = new GroupCoverage
                    {
                        Id = tempRisk.CoverageGroupId.Value
                    },
                    LimitRc = new CompanyLimitRc
                    {
                        Id = coTempRisk.LimitsRcCode.Value,
                        LimitSum = coTempRisk.LimitRcSum.Value
                    }
                },
                Make = new CompanyMake
                {
                    Id = tempRiskVehicle.VehicleMakeCode
                },
                Model = new CompanyModel
                {
                    Id = tempRiskVehicle.VehicleModelCode
                },
                Version = new CompanyVersion
                {
                    Id = tempRiskVehicle.VehicleVersionCode,
                    Type = new Vehicles.Models.CompanyType
                    {
                        Id = tempRiskVehicle.VehicleTypeCode
                    },
                    Fuel = new CompanyFuel
                    {
                        Id = tempRiskVehicle.VehicleFuelCode.HasValue ? tempRiskVehicle.VehicleFuelCode.Value : 0
                    },
                    Body = new CompanyBody
                    {
                        Id = tempRiskVehicle.VehicleBodyCode.Value
                    }
                },
                ServiceType = new CompanyServiceType
                {
                    Id = 1
                },
                NewPrice = tempRiskVehicle.NewVehiclePrice.Value,
                Year = tempRiskVehicle.VehicleYear,
                Use = new CompanyUse
                {
                    Id = tempRiskVehicle.VehicleUseCode
                },
                Price = tempRiskVehicle.VehiclePrice.Value,
                IsNew = tempRiskVehicle.IsNew,
                LicensePlate = tempRiskVehicle.LicensePlate,
                EngineSerial = tempRiskVehicle.EngineSerNo,
                ChassisSerial = tempRiskVehicle.ChassisSerNo,
                Color = new CompanyColor
                {
                    Id = tempRiskVehicle.VehicleColorCode.Value
                },
                StandardVehiclePrice = tempRiskVehicle.StdVehiclePrice.Value,
                Rate = coTempRiskVehicle.FlatRatePercentage.GetValueOrDefault(0)
            };

            List<Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept> dynamicProperties = new List<Core.Application.RulesScriptsServices.Models.DynamicConcept>();

            foreach (DynamicProperty item in tempRisk.DynamicProperties)
            {
                DynamicProperty itemDynamic = (DynamicProperty)item.Value;
                Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept dynamicProperty = new Core.Application.RulesScriptsServices.Models.DynamicConcept();
                dynamicProperty.Id = itemDynamic.Id;
                dynamicProperty.Value = itemDynamic.Value;
                dynamicProperties.Add(dynamicProperty);
            }
            model.Risk.DynamicProperties = dynamicProperties;
            return model;
        }

        #endregion

        #region Beneficiary

        public static Beneficiary CreateBeneficiary(ISSEN.RiskBeneficiary riskBeneficiary)
        {
            return new Beneficiary
            {
                IndividualId = riskBeneficiary.BeneficiaryId,
                CustomerType = CustomerType.Individual,
                BeneficiaryType = new BeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode },
                Participation = riskBeneficiary.BenefitPercentage,
                CompanyName = new IssuanceCompanyName
                {
                    NameNum = riskBeneficiary.NameNum.GetValueOrDefault(),
                    Address = new IssuanceAddress
                    {
                        Id = riskBeneficiary.AddressId
                    }
                }
            };
        }

        public static Beneficiary CreateTemporalBeneficiary(TMPEN.TempRiskBeneficiary tempRiskBeneficiary)
        {
            return new Beneficiary
            {
                IndividualId = tempRiskBeneficiary.BeneficiaryId,
                CustomerType = (CustomerType)tempRiskBeneficiary.CustomerTypeCode,
                BeneficiaryType = new BeneficiaryType { Id = tempRiskBeneficiary.BeneficiaryTypeCode },
                Participation = tempRiskBeneficiary.BenefictPercentage,
                CompanyName = new IssuanceCompanyName
                {
                    NameNum = tempRiskBeneficiary.NameNum.GetValueOrDefault(),
                    Address = new IssuanceAddress
                    {
                        Id = tempRiskBeneficiary.AddressId.GetValueOrDefault()
                    }
                }
            };
        }

        #endregion

        #region TempRiskClause

        public static Clause CreateTempRiskClause(TMPEN.TempRiskClause tempRiskClause)
        {
            return new Clause
            {
                Id = tempRiskClause.ClauseId
            };
        }

        #endregion
        #region Accesories
        public static Accessory CreateAccessoryTemp(TMPEN.TempRiskCoverDetail tempRiskCoverDetail, TMPEN.TempRiskDetailAccessory tempRiskDetailAccessories)
        {
            int? coverageIdAccNoOriginal = 0;
            bool original = false;

            CommonModels.Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories);
            if (parameter != null)
            {
                coverageIdAccNoOriginal = parameter.NumberParameter;
            }

            if (tempRiskCoverDetail.CoverageId != coverageIdAccNoOriginal)
            {
                original = true;
            }

            return new Accessory()
            {
                Id = tempRiskDetailAccessories.DetailId,
                Rate = tempRiskCoverDetail.Rate != null ? (decimal)tempRiskCoverDetail.Rate : 0,
                RateType = tempRiskCoverDetail.RateTypeCode != null ? (RateType)tempRiskCoverDetail.RateTypeCode : RateType.Percentage,
                AccessoryId = tempRiskDetailAccessories.RiskDetailId.ToString(),
                Premium = tempRiskCoverDetail.PremiumAmount != null ? (decimal)tempRiskCoverDetail.PremiumAmount : 0,
                AccumulatedPremium = tempRiskCoverDetail.AccPremiumAmount != null ? (decimal)tempRiskCoverDetail.AccPremiumAmount : 0,
                Amount = tempRiskCoverDetail.SublimitAmount != null ? (decimal)tempRiskCoverDetail.SublimitAmount : 0,
                IsOriginal = original,
                Make = tempRiskDetailAccessories.BrandName,
                Description = ""
            };
        }
        #endregion

        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }

    }
}