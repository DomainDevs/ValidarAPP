using System.Collections.Generic;
using CommonModels = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Application.RulesScriptsServices.Models;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Newtonsoft.Json;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Linq;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.UnderwritingServices.Enums;
using AutoMapper;
using Sistran.Core.Application.Vehicles.Models;
using VEMOD = Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.View;
using System;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Use

        public static Models.Use CreateUse(COMMEN.VehicleUse use)
        {
            return new Models.Use
            {
                Id = use.VehicleUseCode,
                Description = use.SmallDescription,
                PrefixType = new CommonModels.PrefixType { Id = use.PrefixTypeCode }
            };
        }

        public static List<Models.Use> CreateUses(BusinessCollection businessCollection)
        {
            List<Models.Use> uses = new List<Models.Use>();

            foreach (COMMEN.VehicleUse field in businessCollection)
            {
                uses.Add(ModelAssembler.CreateUse(field));
            }

            return uses;
        }

        #endregion

        #region Accessory

        public static Models.Accessory CreateAccessory(QUOEN.Detail detail)
        {
            return new Models.Accessory
            {
                Id = detail.DetailId,
                Description = detail.Description,
                RateType = (RateType?)detail.RateTypeCode,
                Rate = detail.Rate ?? 0,
                Enable = detail.Enabled
            };
        }

        public static List<Models.Accessory> CreateAccessories(BusinessCollection businessCollection)
        {
            List<Models.Accessory> accessories = new List<Models.Accessory>();

            foreach (QUOEN.Detail field in businessCollection)
            {
                if (field.DetailTypeCode == 1)
                    accessories.Add(ModelAssembler.CreateAccessory(field));
            }

            return accessories;
        }

        #endregion

        #region Accesories
        public static Accessory CreateAccessoryTemp(TMPEN.TempRiskCoverDetail tempRiskCoverDetail, TMPEN.TempRiskDetailAccessory tempRiskDetailAccessories)
        {
            int? coverageIdAccNoOriginal = 0;
            bool original = false;

            CommonModels.Parameter parameter = DelegateService.commonServiceCore.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories);
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

        //public static List<DynamicConcept> CreateDynamicConcepts(List<RuleConcept> dynamicConceptValues)
        //{
        //    List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

        //    foreach (RuleConcept dynamicConceptValue in dynamicConceptValues)
        //    {
        //        dynamicConcepts.Add(ModelAssembler.CreateDynamicConcept(dynamicConceptValue));
        //    }

        //    return dynamicConcepts;
        //}

        //private static DynamicConcept CreateDynamicConcept(RuleConcept dynamicConceptValue)
        //{
        //    return new DynamicConcept
        //    {
        //        Id = dynamicConceptValue.Id,
        //        Value = dynamicConceptValue.Value
        //    };
        //}

        #region Parameter

        public static CommonModels.Parameter CreateParameter(COMMEN.Parameter entityParameter)
        {
            return new CommonModels.Parameter
            {
                Id = entityParameter.ParameterId,
                Description = entityParameter.Description,
                BoolParameter = entityParameter.BoolParameter,
                NumberParameter = entityParameter.NumberParameter,
                DateParameter = entityParameter.DateParameter,
                TextParameter = entityParameter.TextParameter,
                PercentageParameter = entityParameter.PercentageParameter,
                AmountParameter = entityParameter.AmountParameter
            };
        }

        public static List<CommonModels.Parameter> CreateParameters(BusinessCollection businessCollection)
        {
            List<CommonModels.Parameter> parameters = new List<CommonModels.Parameter>();

            foreach (COMMEN.Parameter entity in businessCollection)
            {
                parameters.Add(ModelAssembler.CreateParameter(entity));
            }

            return parameters;
        }

        public static CommonModels.Parameter CreateCoParameter(COMMEN.CoParameter entityCoParameter)
        {
            return new CommonModels.Parameter
            {
                Id = entityCoParameter.ParameterId,
                Description = entityCoParameter.Description,
                BoolParameter = entityCoParameter.BoolParameter,
                NumberParameter = entityCoParameter.NumberParameter,
                DateParameter = entityCoParameter.DateParameter,
                TextParameter = entityCoParameter.TextParameter,
                PercentageParameter = entityCoParameter.PercentageParameter,
                AmountParameter = entityCoParameter.AmountParameter
            };
        }

        public static List<CommonModels.Parameter> CreateCoParameters(BusinessCollection businessCollection)
        {
            List<CommonModels.Parameter> parameters = new List<CommonModels.Parameter>();

            foreach (COMMEN.CoParameter entity in businessCollection)
            {
                parameters.Add(ModelAssembler.CreateCoParameter(entity));
            }

            return parameters;
        }

        #endregion

        public static List<Vehicle> CreateVehicles(BusinessCollection businessCollection)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                vehicles.Add(CreateVehicle(entityEndorsementOperation));
            }

            return vehicles;
        }

        public static Vehicle CreateVehicle(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            Vehicle vehicle = new Vehicle();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                vehicle = JsonConvert.DeserializeObject<Vehicle>(entityEndorsementOperation.Operation);
                vehicle.Risk.Id = 0;
                vehicle.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
                vehicle.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
            }

            return vehicle;
        }

        public static Vehicle CreateVehicle(ISSEN.Risk risk, ISSEN.RiskVehicle riskVehicle, ISSEN.EndorsementRisk endorsementRisk)
        {
            Vehicle vehicle = new Vehicle
            {
                Risk = new Risk
                {
                    RiskId = risk.RiskId,
                    Number = endorsementRisk.RiskNum,
                    CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                    GroupCoverage = new GroupCoverage
                    {
                        Id = risk.CoverGroupId.Value,
                        CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                    },
                    MainInsured = new IssuanceInsured
                    {
                        IndividualId = risk.InsuredId,
                        CompanyName = new IssuanceCompanyName
                        {
                            NameNum = risk.NameNum.GetValueOrDefault(),
                            Address = new IssuanceAddress
                            {
                                Id = risk.AddressId.GetValueOrDefault()
                            }
                        }
                    },
                    Text = new Text
                    {
                        TextBody = risk.ConditionText
                    },
                    Description = riskVehicle.LicensePlate,
                    RatingZone = new RatingZone
                    {
                        Id = risk.RatingZoneCode.Value
                    },
                    IsPersisted = true,
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    Policy = endorsementRisk != null && endorsementRisk.EndorsementId > 0 ? new Policy
                    {
                        Endorsement = new Endorsement
                        {
                            Id = endorsementRisk.EndorsementId
                        }
                    } : null,
                    Status = RiskStatusType.NotModified,
                    DynamicProperties = new List<DynamicConcept>()
                },
                ChassisSerial = riskVehicle.ChassisSerNo,
                Color = new Color
                {
                    Id = riskVehicle.VehicleColorCode.Value
                },
                IsNew = riskVehicle.IsNew,
                LicensePlate = riskVehicle.LicensePlate,
                LoadTypeCode = riskVehicle.LoadTypeCode.GetValueOrDefault(),
                Price = riskVehicle.VehiclePrice,
                StandardVehiclePrice = riskVehicle.StdVehiclePrice.GetValueOrDefault(),
                OriginalPrice = riskVehicle.VehiclePrice,
                NewPrice = riskVehicle.NewVehiclePrice.Value,
                EngineSerial = riskVehicle.EngineSerNo,
                Year = riskVehicle.VehicleYear,
                Use = new Use
                {
                    Id = riskVehicle.VehicleUseCode
                },
                Version = new VEMOD.Version
                {
                    Id = riskVehicle.VehicleVersionCode,
                    Fuel = new Fuel
                    {
                        Id = riskVehicle.VehicleFuelCode.GetValueOrDefault(0)
                    },
                    Type = new VEMOD.Type
                    {
                        Id = riskVehicle.VehicleTypeCode
                    },
                    Body = new Body
                    {
                        Id = riskVehicle.VehicleBodyCode
                    }
                },
                Make = new Make
                {
                    Id = riskVehicle.VehicleMakeCode
                },
                Model = new Model
                {
                    Id = riskVehicle.VehicleModelCode
                }
            };
            if (risk?.DynamicProperties?.Count > 0)
            {
                foreach (DynamicProperty item in risk.DynamicProperties.Distinct().ToList())
                {
                    if (item.Value != null)
                    {
                        DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                        DynamicConcept dynamicConcept = new DynamicConcept();
                        dynamicConcept.EntityId = RuleConceptRisk.Id;
                        dynamicConcept.Id = dynamicProperty.Id;
                        dynamicConcept.Value = dynamicProperty.Value;
                        if (vehicle.Risk.DynamicProperties != null && !vehicle.Risk.DynamicProperties.Exists(x => x.Id == dynamicConcept.Id))
                        {
                            vehicle.Risk.DynamicProperties.Add(dynamicConcept);
                        }
                    }

                }
            }


            return vehicle;
        }

        public static Beneficiary CreateBeneficiary(ISSEN.RiskBeneficiary riskBeneficiary)
        {
            return new Beneficiary
            {
                IndividualId = riskBeneficiary.BeneficiaryId,
                IndividualType = riskBeneficiary.BeneficiaryTypeCode == (int)IndividualType.Person ? IndividualType.Person : IndividualType.Company,
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

        public static List<Accessory> CreateAccesories(AccessoryView businessCollection)
        {
            List<Accessory> accesories = new List<Accessory>();

            foreach (ISSEN.RiskCoverDetail field in businessCollection.RiskCoverDetails)
            {
                accesories.Add(ModelAssembler.CreateAccesory(field, businessCollection));
            }

            return accesories;
        }

        public static Accessory CreateAccesory(ISSEN.RiskCoverDetail riskCoverDetail, AccessoryView businessCollection)
        {
            Accessory accessory = new Accessory
            {
                Amount = riskCoverDetail.SublimitAmount.Value,
                Rate = riskCoverDetail.Rate.Value,
                Premium = riskCoverDetail.PremiumAmount.Value,
                AccumulatedPremium = riskCoverDetail.AccPremiumAmount.Value,
                AccessoryId = riskCoverDetail.RiskCoverId.ToString(),
                RiskDetailId = riskCoverDetail.RiskDetailId,
                Status = (int)CoverageStatusType.NotModified


            };

            foreach (ISSEN.RiskDetailAccessory field in businessCollection.RiskDetailAccessories)
            {
                if (riskCoverDetail.RiskDetailId == field.RiskDetailId)
                {
                    accessory.Id = field.DetailId;
                    accessory.Make = field.BrandName;
                    accessory.Description = field.Model;
                    accessory.RateType = RateType.Percentage;
                }
            }

            return accessory;
        }

        public static List<Vehicle> CreateVehiclesByRiskVehicle(BusinessCollection businessCollection)
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            foreach (ISSEN.RiskVehicle entityEndorsementOperation in businessCollection)
            {
                vehicles.Add(CreateVehicleByRiskVehicle(entityEndorsementOperation));
            }

            return vehicles;
        }

        public static Vehicle CreateVehicleByRiskVehicle(ISSEN.RiskVehicle riskVehicle)
        {
            if (riskVehicle == null)
                return null;

            Vehicle Vehicle = new Vehicle
            {
                Risk = new Risk
                {
                    RiskId = riskVehicle.RiskId,
                    Policy = new Policy
                    {
                        Endorsement = new Endorsement
                        {

                        }
                    }
                },
                Make = new Make
                {
                    Id = riskVehicle.VehicleMakeCode
                },
                Model = new Model
                {
                    Id = riskVehicle.VehicleModelCode
                },
                Version = new VEMOD.Version
                {
                    Id = riskVehicle.VehicleVersionCode,
                    Type = new VEMOD.Type
                    {
                        Id = riskVehicle.VehicleTypeCode
                    },
                    Body = new Body
                    {
                        Id = riskVehicle.VehicleBodyCode
                    }
                },
                Use = new Use
                {
                    Id = riskVehicle.VehicleUseCode
                },
                Year = riskVehicle.VehicleYear,
                Price = riskVehicle.VehiclePrice,
                Color = new Color()
                {
                    Id = Convert.ToInt32(riskVehicle.VehicleColorCode)
                }
            };

            if (riskVehicle.LicensePlate != null)
            {
                Vehicle.LicensePlate = riskVehicle.LicensePlate;
                Vehicle.ChassisSerial = riskVehicle.ChassisSerNo;
                Vehicle.EngineSerial = riskVehicle.EngineSerNo;
            }

            return Vehicle;
        }

        public static Vehicle CreateClaimVehicle(ISSEN.RiskVehicle entityRiskVehicle, ISSEN.Risk entityRisk, ISSEN.EndorsementRisk entityEndorsementRisk, ISSEN.Policy entityPolicy)
        {
            Vehicle vehicle = new Vehicle
            {
                Risk = new Risk
                {
                    RiskId = entityRiskVehicle.RiskId,
                    Number = entityEndorsementRisk.RiskNum,
                    CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                    Policy = new Policy
                    {
                        Id = entityEndorsementRisk.PolicyId,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Endorsement = new Endorsement
                        {
                            Id = entityEndorsementRisk.EndorsementId
                        }
                    },
                    MainInsured = new IssuanceInsured
                    {
                        IndividualId = entityRisk.InsuredId,
                        InsuredId = entityRisk.InsuredId
                    }
                },
                Make = new Make
                {
                    Id = entityRiskVehicle.VehicleMakeCode
                },
                Model = new Model
                {
                    Id = entityRiskVehicle.VehicleModelCode
                },
                Version = new VEMOD.Version
                {
                    Id = entityRiskVehicle.VehicleVersionCode,
                    Type = new VEMOD.Type
                    {
                        Id = entityRiskVehicle.VehicleTypeCode
                    },
                    Body = new Body
                    {
                        Id = entityRiskVehicle.VehicleBodyCode
                    }
                },
                Use = new Use
                {
                    Id = entityRiskVehicle.VehicleUseCode
                },
                Year = entityRiskVehicle.VehicleYear,
                Price = entityRiskVehicle.VehiclePrice,
                Color = new Color()
                {
                    Id = Convert.ToInt32(entityRiskVehicle.VehicleColorCode)
                }
            };

            if (entityRiskVehicle.LicensePlate != null)
            {
                vehicle.LicensePlate = entityRiskVehicle.LicensePlate;
                vehicle.ChassisSerial = entityRiskVehicle.ChassisSerNo;
                vehicle.EngineSerial = entityRiskVehicle.EngineSerNo;
            }

            return vehicle;
        }

        public static Risk CreateRisk(ISSEN.Risk entityRisk)
        {
            return new Risk
            {
                Id = entityRisk.RiskId,
                RiskId = entityRisk.RiskId,
                CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                MainInsured = new IssuanceInsured
                {
                    IndividualId = entityRisk.InsuredId,
                    CompanyName = new IssuanceCompanyName
                    {
                        Address = new IssuanceAddress
                        {
                            Id = entityRisk.AddressId.GetValueOrDefault()
                        },
                        Phone = new IssuancePhone
                        {
                            Id = entityRisk.PhoneId.GetValueOrDefault()
                        }
                    }
                },
                GroupCoverage = new GroupCoverage
                {
                    Id = entityRisk.CoverGroupId.GetValueOrDefault()
                },
                Policy = new Policy
                {
                    Endorsement = new Endorsement
                    {

                    }
                }
            };
        }
    }
}
