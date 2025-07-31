using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Issuance.Entities;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities.Views;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ModelServices.Models.VehicleParam;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using COCOMMEN = Sistran.Company.Application.Common.Entities;
using COMM = Sistran.Core.Application.Common.Entities;
using CommonModels = Sistran.Core.Application.CommonService.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PROD = Sistran.Company.Application.Product.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using UPMB = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using VEBMO = Sistran.Company.Application.Vehicles.Models;
using VEMOD = Sistran.Core.Application.Vehicles.Models;
using VEMODCO = Sistran.Company.Application.Vehicles.Models;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Assemblers
{
    internal class ModelAssembler
    {

        #region Fasecolda

        public static Models.Fasecolda CreateFasecolda(COCOMMEN.CoVehicleVersionFasecolda fasecolda)
        {
            return new Models.Fasecolda
            {
                Model = fasecolda.FasecoldaModelId.Trim(),
                Make = fasecolda.FasecoldaMakeId.Trim()
            };
        }

        #endregion

        #region Causes
        public static Models.CompanyNotInsurableCause CreateCause(COCOMMEN.CiaNonInsurableCauses NonInsCauses)
        {
            return new Models.CompanyNotInsurableCause
            {
                Id = NonInsCauses.CauseId,
                Description = NonInsCauses.CauseDescription
            };
        }

        public static List<Models.CompanyNotInsurableCause> CreateCauses(BusinessCollection businessCollection)
        {
            List<Models.CompanyNotInsurableCause> causes = new List<Models.CompanyNotInsurableCause>();

            foreach (COCOMMEN.CiaNonInsurableCauses field in businessCollection)
            {
                causes.Add(ModelAssembler.CreateCause(field));
            }

            return causes;
        }
        #endregion
        #region ValidationPlate
        public static List<ValidationPlateServiceModel> CreateValidationPlatesService(List<Models.CompanyValidationPlate> paramLimitRc)
        {
            List<ValidationPlateServiceModel> validationPlateServiceModel = new List<ValidationPlateServiceModel>();
            foreach (var item in paramLimitRc)
            {
                validationPlateServiceModel.Add(CreateValidationPlateService(item));
            }

            return validationPlateServiceModel;
        }
        public static ValidationPlateServiceModel CreateValidationPlateService(Models.CompanyValidationPlate paramValidationV)
        {
            return new ValidationPlateServiceModel()
            {
                Id = paramValidationV.Id,
                Chassis = paramValidationV.Chassis,
                CodCause = paramValidationV.CodCause,
                CodFasecolda = paramValidationV.CodFasecolda,
                CodMake = paramValidationV.CodMake,
                CodModel = paramValidationV.CodModel,
                CodVersion = paramValidationV.CodVersion,
                IsEnabled = paramValidationV.IsEnabled,
                Plate = paramValidationV.Plate,
                Motor = paramValidationV.Motor,
                ErrorServiceModel = new Core.Application.ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorTypeService = Core.Application.ModelServices.Enums.ErrorTypeService.Ok
                }
            };

        }

        public static Models.CompanyValidationPlate CreateValidationPlate(COCOMMEN.CiaVehicleEnabled vehicleEnab)
        {
            return new Models.CompanyValidationPlate
            {
                Id = vehicleEnab.VehicleEnabledId,
                Chassis = vehicleEnab.ChassisSerNo,
                CodCause = vehicleEnab.NonInsurableCauseId,
                CodFasecolda = vehicleEnab.FasecoldaCode,
                IsEnabled = vehicleEnab.IsEnabled,
                Plate = vehicleEnab.LicensePlate,
                Motor = vehicleEnab.EngineSerNo
            };
        }

        public static List<Models.CompanyValidationPlate> CreateValidationPlates(BusinessCollection businessCollection)
        {
            List<Models.CompanyValidationPlate> ValidationPlate = new List<Models.CompanyValidationPlate>();

            foreach (COCOMMEN.CiaVehicleEnabled field in businessCollection)
            {
                ValidationPlate.Add(ModelAssembler.CreateValidationPlate(field));
            }

            return ValidationPlate;
        }
        #endregion
        #region Accesories

        public static CompanyAccessory CreateAccesory(ISSEN.RiskCoverDetail riskCoverDetail, AccessoryView businessCollection)
        {
            CompanyAccessory accessory = new CompanyAccessory
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

        public static List<CompanyAccessory> CreateAccesories(AccessoryView businessCollection)
        {
            List<CompanyAccessory> accesories = new List<CompanyAccessory>();

            foreach (ISSEN.RiskCoverDetail field in businessCollection.RiskCoverDetails)
            {
                accesories.Add(ModelAssembler.CreateAccesory(field, businessCollection));
            }

            return accesories;
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

        #region TemporalVehicle

        public static Models.CompanyVehicle CreateTemporalVehicle(TMPEN.TempRisk tempRisk, TMPEN.CoTempRisk coTempRisk, TMPEN.TempRiskVehicle tempRiskVehicle, TMPEN.CoTempRiskVehicle coTempRiskVehicle)
        {
            Models.CompanyVehicle model = new Models.CompanyVehicle();

            model.Risk.Id = tempRisk.OperationId.GetValueOrDefault();
            model.Risk.RiskId = tempRisk.RiskId;
            model.Risk.Description = tempRiskVehicle.LicensePlate;
            model.Risk.CoveredRiskType = (CoveredRiskType)tempRisk.CoveredRiskTypeCode;
            model.Risk.Status = (RiskStatusType)tempRisk.RiskStatusCode;
            model.Risk.RatingZone = new CompanyRatingZone
            {
                Id = tempRisk.RatingZoneCode.Value
            };
            model.Risk.GroupCoverage = new GroupCoverage
            {
                Id = tempRisk.CoverageGroupId.Value
            };
            model.Risk.LimitRc = new CompanyLimitRc
            {
                Id = coTempRisk.LimitsRcCode.Value,
                LimitSum = coTempRisk.LimitRcSum.Value
            };
            model.Make = new CompanyMake
            {
                Id = tempRiskVehicle.VehicleMakeCode
            };
            model.Model = new CompanyModel
            {
                Id = tempRiskVehicle.VehicleModelCode
            };
            model.Version = new VEMODCO.CompanyVersion
            {
                Id = tempRiskVehicle.VehicleVersionCode,
                Type = new VEBMO.CompanyType
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
            };
            model.ServiceType = new CompanyServiceType
            {
                Id = 1
            };
            model.NewPrice = tempRiskVehicle.NewVehiclePrice.Value;
            model.Year = tempRiskVehicle.VehicleYear;
            model.Use = new CompanyUse
            {
                Id = tempRiskVehicle.VehicleUseCode
            };
            model.Price = tempRiskVehicle.VehiclePrice.Value;
            model.IsNew = tempRiskVehicle.IsNew;
            model.LicensePlate = tempRiskVehicle.LicensePlate;
            model.EngineSerial = tempRiskVehicle.EngineSerNo;
            model.ChassisSerial = tempRiskVehicle.ChassisSerNo;
            model.Color = new CompanyColor
            {
                Id = tempRiskVehicle.VehicleColorCode.Value
            };
            model.NewPrice = tempRiskVehicle.NewVehiclePrice.Value;
            model.StandardVehiclePrice = tempRiskVehicle.StdVehiclePrice.Value;
            model.Rate = coTempRiskVehicle.FlatRatePercentage.GetValueOrDefault(0);
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

        public static CompanyBeneficiary CreateBeneficiary(ISSEN.RiskBeneficiary riskBeneficiary)
        {
            return new CompanyBeneficiary
            {
                IndividualId = riskBeneficiary.BeneficiaryId,
                IndividualType = riskBeneficiary.BeneficiaryTypeCode == (int)IndividualType.Person ? IndividualType.Person : IndividualType.Company, 
                CustomerType = CustomerType.Individual,
                BeneficiaryType = new CompanyBeneficiaryType { Id = riskBeneficiary.BeneficiaryTypeCode },
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

        public static CompanyBeneficiary CreateTemporalBeneficiary(TMPEN.TempRiskBeneficiary tempRiskBeneficiary)
        {
            return new CompanyBeneficiary
            {
                IndividualId = tempRiskBeneficiary.BeneficiaryId,
                CustomerType = (CustomerType)tempRiskBeneficiary.CustomerTypeCode,
                BeneficiaryType = new CompanyBeneficiaryType { Id = tempRiskBeneficiary.BeneficiaryTypeCode },
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


        public static CompanyBeneficiary CreateCompanyBeneficiary(Beneficiary beneficiary)
        {
            return new CompanyBeneficiary
            {
                IndividualId = beneficiary.IndividualId,
                IndividualType = beneficiary.IndividualType,
                CustomerType = beneficiary.CustomerType,
                BeneficiaryType = new CompanyBeneficiaryType { Id = beneficiary.BeneficiaryType.Id },
                Participation = beneficiary.Participation,
                CompanyName = beneficiary.CompanyName == null ? null : new IssuanceCompanyName
                {
                    NameNum = beneficiary.CompanyName.NameNum,
                    Address = new IssuanceAddress
                    {
                        Id = beneficiary.CompanyName.Address.Id
                    }
                },
                Name = beneficiary.Name,
                IdentificationDocument = beneficiary.IdentificationDocument
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

        #region Vehicle

        public static Models.CompanyVehicle CreateVehicle(ISSEN.Risk risk, CompanyCoRisk coRisk, ISSEN.RiskVehicle riskVehicle, CompanyCoRiskVehicle coRiskVehicle, ISSEN.EndorsementRisk endorsementRisk)
        {
            Models.CompanyVehicle vehicle = new Models.CompanyVehicle
            {
                Risk = new CompanyRisk
                {
                    RiskId = risk.RiskId,
                    Number = endorsementRisk.RiskNum,
                    CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                    GroupCoverage = new GroupCoverage
                    {
                        Id = risk.CoverGroupId.Value,
                        CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                    },
                    MainInsured = new CompanyIssuanceInsured
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
                    Text = new CompanyText
                    {
                        TextBody = risk.ConditionText
                    },
                    WorkerType = coRiskVehicle.WorkerType,

                    Description = riskVehicle.LicensePlate,

                    RatingZone = new CompanyRatingZone
                    {
                        Id = risk.RatingZoneCode.Value
                    },
                    LimitRc = new CompanyLimitRc
                    {
                        Id = coRisk.LimitsRcCode.Value,
                        LimitSum = coRisk.LimitRcSum.Value
                    },
                    IsPersisted = true,
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    Policy = endorsementRisk != null && endorsementRisk.EndorsementId > 0 ? new CompanyPolicy
                    {
                        Endorsement = new UnderwritingServices.CompanyEndorsement
                        {
                            Id = endorsementRisk.EndorsementId
                        } 
                    } : null,
                    Status = RiskStatusType.NotModified,
                    DynamicProperties = new List<DynamicConcept>()
                },
                ChassisSerial = riskVehicle.ChassisSerNo,
                Color = new CompanyColor
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
                Use = new CompanyUse
                {
                    Id = riskVehicle.VehicleUseCode
                },
                Version = new VEMODCO.CompanyVersion
                {
                    Id = riskVehicle.VehicleVersionCode,
                    Fuel = new CompanyFuel
                    {
                        Id = riskVehicle.VehicleFuelCode.GetValueOrDefault(0)
                    },
                    Type = new VEBMO.CompanyType
                    {
                        Id = riskVehicle.VehicleTypeCode
                    },
                    Body = new CompanyBody
                    {
                        Id = riskVehicle.VehicleBodyCode
                    }
                },
                Make = new CompanyMake
                {
                    Id = riskVehicle.VehicleMakeCode
                },
                Model = new CompanyModel
                {
                    Id = riskVehicle.VehicleModelCode
                },

                Rate = coRiskVehicle.FlatRatePercentage.GetValueOrDefault(),
                ServiceType = new CompanyServiceType()
                {
                    Id = coRiskVehicle.ServiceTypeCode ?? 0
                },
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
            else
            {
                vehicle.Risk.DynamicProperties= DelegateService.underwritingService.GetDynamicConceptsByEndorsementIdRiskNumPolicyIdRiskId(endorsementRisk.EndorsementId, endorsementRisk.RiskNum, endorsementRisk.PolicyId, risk.RiskId);
            }


            return vehicle;
        }
        public static CompanyVehicle CreateVehicle(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            CompanyVehicle companyVehicle = new CompanyVehicle();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyVehicle = JsonConvert.DeserializeObject<CompanyVehicle>(entityEndorsementOperation.Operation);
                companyVehicle.Risk.Id = 0;
                companyVehicle.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
                companyVehicle.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
            }

            return companyVehicle;
        }
        public static Models.CompanyVehicle CreateVehicle(ISSEN.RiskVehicle riskVehicle)
        {
            Models.CompanyVehicle Vehicle = new Models.CompanyVehicle
            {
                Make = new CompanyMake
                {
                    Id = riskVehicle.VehicleMakeCode
                },
                Model = new CompanyModel
                {
                    Id = riskVehicle.VehicleModelCode
                },
                Version = new VEMODCO.CompanyVersion
                {
                    Id = riskVehicle.VehicleVersionCode,
                    Type = new VEBMO.CompanyType
                    {
                        Id = riskVehicle.VehicleTypeCode
                    },
                    Body = new CompanyBody
                    {
                        Id = riskVehicle.VehicleBodyCode
                    }
                },
                Use = new CompanyUse
                {
                    Id = riskVehicle.VehicleUseCode
                },
                Year = riskVehicle.VehicleYear,
                Price = riskVehicle.VehiclePrice,
                Color = new CompanyColor()
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

        public static Models.CompanyVehicle CreateClaimVehicle(ISSEN.RiskVehicle riskVehicle)
        {
            Models.CompanyVehicle Vehicle = new Models.CompanyVehicle
            {
                Risk = new CompanyRisk
                {
                    RiskId = riskVehicle.RiskId
                },
                Make = new CompanyMake
                {
                    Id = riskVehicle.VehicleMakeCode
                },
                Model = new CompanyModel
                {
                    Id = riskVehicle.VehicleModelCode
                },
                Version = new VEMODCO.CompanyVersion
                {
                    Id = riskVehicle.VehicleVersionCode,
                    Type = new VEBMO.CompanyType
                    {
                        Id = riskVehicle.VehicleTypeCode
                    },
                    Body = new CompanyBody
                    {
                        Id = riskVehicle.VehicleBodyCode
                    }
                },
                Use = new CompanyUse
                {
                    Id = riskVehicle.VehicleUseCode
                },
                Year = riskVehicle.VehicleYear,
                Price = riskVehicle.VehiclePrice,
                Color = new CompanyColor()
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

        #endregion



        public static List<CompanyVehicle> CreateCompanyVehiclesByRiskVehicle(BusinessCollection businessCollection)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            foreach (ISSEN.RiskVehicle entityEndorsementOperation in businessCollection)
            {
                companyVehicles.Add(CreateVehicle(entityEndorsementOperation));
            }

            return companyVehicles;
        }

        public static List<CompanyVehicle> CreateCompanyClaimVehiclesByRiskVehicle(BusinessCollection businessCollection)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            foreach (ISSEN.RiskVehicle entityEndorsementOperation in businessCollection)
            {
                companyVehicles.Add(CreateClaimVehicle(entityEndorsementOperation));
            }

            return companyVehicles;
        }

        public static List<CompanyVehicle> CreateVehicles(BusinessCollection businessCollection)
        {
            List<CompanyVehicle> companyVehicles = new List<CompanyVehicle>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                companyVehicles.Add(CreateVehicle(entityEndorsementOperation));
            }

            return companyVehicles;
        }
        public static List<DynamicConcept> CreateDynamicConcepts(Rules.Facade facade)
        {
            List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

            foreach (Rules.Concept concept in facade.Concepts.Where(x => x.IsStatic == false))
            {
                if (dynamicConcepts.Any(x => x.Id == concept.Id && x.EntityId == concept.EntityId))
                {
                    dynamicConcepts.First(x => x.Id == concept.Id && x.EntityId == concept.EntityId).Value = concept.Value;
                }
                else
                {
                    DynamicConcept dynamicConcept = CreateDynamicConcept(concept);
                    dynamicConcept.QuestionId = dynamicConcepts?.FirstOrDefault(x => x.Id == concept.Id && x.EntityId == concept.EntityId)?.QuestionId;
                    dynamicConcepts.Add(dynamicConcept);
                }
                //dynamicConcepts.Add(CreateDynamicConcept(concept));
            }

            return dynamicConcepts;
        }

        private static DynamicConcept CreateDynamicConcept(Rules.Concept concept)
        {
            return new DynamicConcept
            {
                Id = concept.Id,
                Value = concept.Value,
                EntityId = concept.EntityId
            };
        }

        public static Models.CompanyVehicle CreateVehicle(Models.CompanyVehicle vehicle, Rules.Facade facade)
        {

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode) > 0)
            {
                if (vehicle.Risk.RatingZone == null)
                {
                    vehicle.Risk.RatingZone = new CompanyRatingZone();
                }
                vehicle.Risk.RatingZone.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode);

            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId) > 0)
            {
                if (vehicle.Risk.GroupCoverage == null)
                {
                    vehicle.Risk.GroupCoverage = new GroupCoverage();
                }

                vehicle.Risk.GroupCoverage.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode) > 0)
            {
                if (vehicle.Risk.LimitRc == null)
                {
                    vehicle.Risk.LimitRc = new CompanyLimitRc();
                }

                vehicle.Risk.LimitRc.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode);
            }

            if (facade.GetConcept<decimal>(CompanyRuleConceptRisk.LimitsRcSum) > 0)
            {
                if (vehicle.Risk.LimitRc == null)
                {
                    vehicle.Risk.LimitRc = new CompanyLimitRc();
                }

                vehicle.Risk.LimitRc.LimitSum = facade.GetConcept<decimal>(CompanyRuleConceptRisk.LimitsRcSum);
            }

            if (vehicle.Version == null)
            {
                vehicle.Version = new VEMODCO.CompanyVersion();
            }

            vehicle.Version.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleVersionCode);

            if (vehicle.Model == null)
            {
                vehicle.Model = new CompanyModel();
            }
            vehicle.Model.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleModelCode);

            if (vehicle.Make == null)
            {
                vehicle.Make = new CompanyMake();
            }

            vehicle.Make.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleMakeCode);
            vehicle.Year = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleYear);

            if (vehicle.Version.Type == null)
            {
                vehicle.Version.Type = new VEBMO.CompanyType();
            }

            vehicle.Version.Type.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleTypeCode);

            if (vehicle.Use == null)
            {
                vehicle.Use = new CompanyUse();
            }

            vehicle.Use.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleUseCode);

            if (vehicle.Version.Body == null)
            {
                vehicle.Version.Body = new VEBMO.CompanyBody();
            }
            vehicle.Version.Body.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleBodyCode);
            vehicle.Price = facade.GetConcept<decimal>(CompanyRuleConceptRisk.VehiclePrice);
            vehicle.IsNew = facade.GetConcept<bool>(CompanyRuleConceptRisk.IsNew);
            vehicle.LicensePlate = facade.GetConcept<string>(CompanyRuleConceptRisk.LicensePlate);
            vehicle.EngineSerial = facade.GetConcept<string>(CompanyRuleConceptRisk.EngineSerialNumber);
            vehicle.ChassisSerial = facade.GetConcept<string>(CompanyRuleConceptRisk.ChassisSerialNumber);

            if (vehicle.Color == null)
            {
                vehicle.Color = new CompanyColor();
            }
            vehicle.Color.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleColorCode);
            vehicle.Rate = facade.GetConcept<decimal>(CompanyRuleConceptRisk.FlatRatePercentage);
            vehicle.LoadTypeCode = facade.GetConcept<int>(CompanyRuleConceptRisk.LoadTypeCode);
            vehicle.TrailersQuantity = facade.GetConcept<int>(CompanyRuleConceptRisk.TrailersQuantity);
            vehicle.PassengerQuantity = facade.GetConcept<int>(CompanyRuleConceptRisk.PassengerQuantity);
            vehicle.NewPrice = facade.GetConcept<decimal>(CompanyRuleConceptRisk.NewVehiclePrice);
            vehicle.StandardVehiclePrice = facade.GetConcept<decimal>(CompanyRuleConceptRisk.StandardVehiclePrice);
            vehicle.Risk.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            return vehicle;
        }

        public static CompanyCoverage CreateCoverage(CompanyCoverage coverage, Rules.Facade facade)
        {
            coverage.IsDeclarative = facade.GetConcept<bool>(CompanyRuleConceptCoverage.IsDeclarative);
            coverage.IsMinPremiumDeposit = facade.GetConcept<bool>(CompanyRuleConceptCoverage.IsMinimumPremiumDeposit);
            coverage.FirstRiskType = (FirstRiskType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.FirstRiskTypeCode);
            coverage.CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CalculationTypeCode);
            coverage.DeclaredAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.DeclaredAmount);
            coverage.PremiumAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.PremiumAmount);
            coverage.LimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitAmount);
            coverage.SubLimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.SubLimitAmount);
            coverage.LimitOccurrenceAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitOccurrenceAmount); ;
            coverage.LimitClaimantAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitClaimantAmount);
            coverage.AccumulatedLimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccumulatedLimitAmount);
            coverage.AccumulatedDeductAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccumulatedSubLimitAmount);
            coverage.CurrentFrom = facade.GetConcept<DateTime>(CompanyRuleConceptCoverage.CurrentFrom);
            coverage.RateType = (RateType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.RateTypeCode);
            coverage.Rate = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.Rate);
            coverage.CurrentTo = facade.GetConcept<DateTime>(CompanyRuleConceptCoverage.CurrentTo);
            coverage.MainCoverageId = facade.GetConcept<int>(CompanyRuleConceptCoverage.MainCoverageId);
            coverage.MainCoveragePercentage = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MainCoveragePercentage);
            coverage.CoverStatus = (CoverageStatusType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CoverageStatusCode);
            coverage.CoverageOriginalStatus = (CoverageStatusType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CoverageOriginalStatusCode);

            coverage.MaxLiabilityAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MaxLiabilityAmount);


            if (facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductId) > 0)
            {
                if (coverage.Deductible == null)
                {
                    coverage.Deductible = new CompanyDeductible();
                }

                coverage.Deductible.Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductId);

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductRateTypeCode) > 0)
                {
                    coverage.Deductible.RateType = (RateType)facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductRateTypeCode);
                }
                coverage.Deductible.Rate = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.DeductRate);
                coverage.Deductible.DeductPremiumAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.DeductPremiumAmount);
                coverage.Deductible.DeductValue = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.DeductValue);



                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductUnitCode) > 0)
                {
                    coverage.Deductible.DeductibleUnit = new DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductUnitCode)
                    };
                }

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductSubjectCode) > 0)
                {
                    coverage.Deductible.DeductibleSubject = new DeductibleSubject
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductSubjectCode)
                    };
                }
                coverage.Deductible.MinDeductValue = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MinDeductValue);


                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductUnitCode) > 0)
                {
                    coverage.Deductible.MinDeductibleUnit = new DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductUnitCode)
                    };
                }

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductSubjectCode) > 0)
                {
                    coverage.Deductible.MinDeductibleSubject = new DeductibleSubject
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductSubjectCode)
                    };
                }

                coverage.Deductible.MaxDeductValue = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MaxDeductValue);

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductUnitCode) > 0)
                {
                    coverage.Deductible.MaxDeductibleUnit = new DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductUnitCode)
                    };
                }
                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductSubjectCode) > 0)
                {
                    coverage.Deductible.MaxDeductibleSubject = new DeductibleSubject
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductSubjectCode)
                    };
                }

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.CurrencyCode) > 0)
                {
                    coverage.Deductible.Currency = new CommonModels.Currency
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.CurrencyCode)
                    };
                }
                coverage.Deductible.AccDeductAmt = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccDeductAmt);
            }
            else if (coverage.Deductible?.Id != -1)
            {
                coverage.Deductible = null;
            }

            coverage.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            coverage.MinimumPremiumCoverage = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MinimumPremiumCoverage);
            return coverage;
        }

        public static Models.CompanyVehicle CreateVehicle(ISSEN.RiskVehicle riskVehicle, ISSEN.Policy policy)
        {
            Models.CompanyVehicle Vehicle = new Models.CompanyVehicle
            {
                Risk = new CompanyRisk
                {
                    Policy = new CompanyPolicy()
                    {
                        Id = policy.PolicyId,
                        Branch = new CompanyBranch()
                        {
                            Id = policy.BranchCode
                        },
                        Prefix = new CompanyPrefix()
                        {
                            Id = policy.PrefixCode
                        },
                        DocumentNumber = policy.DocumentNumber
                    }
                },
                LicensePlate = riskVehicle.LicensePlate,
                ChassisSerial = riskVehicle.ChassisSerNo,
                EngineSerial = riskVehicle.EngineSerNo,
                Make = new CompanyMake
                {
                    Id = riskVehicle.VehicleMakeCode
                },
                Model = new CompanyModel
                {
                    Id = riskVehicle.VehicleModelCode
                },
                Version = new VEMODCO.CompanyVersion
                {
                    Id = riskVehicle.VehicleVersionCode,
                    Type = new VEBMO.CompanyType
                    {
                        Id = riskVehicle.VehicleTypeCode
                    },
                    Body = new CompanyBody
                    {
                        Id = riskVehicle.VehicleBodyCode
                    }
                },
                Use = new CompanyUse
                {
                    Id = riskVehicle.VehicleUseCode
                },
                Year = riskVehicle.VehicleYear,
                Price = riskVehicle.VehiclePrice,
                Color = new CompanyColor()
                {
                    Id = Convert.ToInt32(riskVehicle.VehicleColorCode)
                }
            };

            if (riskVehicle.LicensePlate != null)
            {
                Vehicle.LicensePlate = riskVehicle.LicensePlate;
            }

            return Vehicle;
        }
        #region automapper

        #region mapper fasecolda
        /// <summary>
        /// Creates the map company coverage.
        /// </summary>
        public static IMapper CreateMapCompanyFasecolda()
        {
            var config = MapperCache.GetMapper<VEMOD.Make, CompanyMake>(cfg =>
            {
                cfg.CreateMap<VEMOD.Make, CompanyMake>();
                cfg.CreateMap<VEMOD.Version, VEMODCO.CompanyVersion>();
                cfg.CreateMap<VEMOD.Model, CompanyModel>();
                cfg.CreateMap<VEMOD.Type, VEMODCO.CompanyType>();
                cfg.CreateMap<VEMOD.Fuel, VEMODCO.CompanyFuel>();
                cfg.CreateMap<VEMOD.Engine, VEMODCO.CompanyEngine>();
                cfg.CreateMap<VEMOD.TransmissionType, VEMODCO.CompanyTransmissionType>();
                cfg.CreateMap<VEMOD.ServiceType, VEMODCO.CompanyServiceType>();
                cfg.CreateMap<VEMOD.Body, VEMODCO.CompanyBody>();
            });
            return config;
        }
        #endregion mapper fasecolda
        #region Clausulas
        public static IMapper CreateMapClause()
        {
            var config = MapperCache.GetMapper<CompanyClause, Clause>(cfg =>
            {
                cfg.CreateMap<CompanyClause, Clause>();
            });

            return config;
        }
        public static IMapper CreateMapCompanyClause()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });

            return config;
        }

        #endregion Clausulas
        #region Asegurado
        public static IMapper CreateMapInsured()
        {
            var config = MapperCache.GetMapper<CompanyIssuanceInsured, IssuanceInsured>(cfg =>
            {
                cfg.CreateMap<CompanyIssuanceInsured, IssuanceInsured>();
            });

            return config;
        }
        public static IMapper CreateMapCompanyInsured()
        {
            var config = MapperCache.GetMapper<CompanyIssuanceInsured, CompanyInsured>(cfg =>
            {
                cfg.CreateMap<CompanyIssuanceInsured, CompanyInsured>();
            });

            return config;
        }

        public static IMapper CreateMapPersonInsured()
        {
            var config = MapperCache.GetMapper<CompanyInsured, CompanyIssuanceInsured>(cfg =>
            {
                cfg.CreateMap<UPMB.BaseInsured, Core.Application.UnderwritingServices.Models.Base.BaseIssuanceInsured>();
                cfg.CreateMap<IndividualPaymentMethod, CiaPersonModel.CiaIndividualPaymentMethod>();
                cfg.CreateMap<UPMB.BaseIndividualPaymentMethod, BaseIndividualPaymentMethod>();
                cfg.CreateMap<EconomicActivity, BaseEconomicActivity>();
                cfg.CreateMap<CompanyInsured, CompanyIssuanceInsured>();
            });

            return config;
        }




        #endregion Asegurado
        #region Beneficiario
        public static IMapper CreateMapBeneficiary()
        {
            var config = MapperCache.GetMapper<CompanyBeneficiary, Beneficiary>(cfg =>
            {
                cfg.CreateMap<CompanyBeneficiary, Beneficiary>();
            });

            return config;
        }
        public static IMapper CreateMapCompanyBeneficiary()
        {
            var config = MapperCache.GetMapper<Beneficiary, CompanyBeneficiary>(cfg =>
            {
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<BeneficiaryType, CompanyBeneficiaryType>();
            });

            return config;

        }

        #endregion Beneficiario
        #region version
        public static IMapper CreateMapCompanyVersion()
        {
            var config = MapperCache.GetMapper<VEMOD.Version, CompanyVersion>(cfg =>
            {
                cfg.CreateMap<VEMOD.Model, CompanyModel>();
                cfg.CreateMap<VEMOD.Make, CompanyMake>();
                cfg.CreateMap<VEMOD.Type, VEBMO.CompanyType>();
                cfg.CreateMap<VEMOD.Fuel, CompanyFuel>();
                cfg.CreateMap<VEMOD.EngineType, CompanyEngineType>();
                cfg.CreateMap<VEMOD.Engine, CompanyEngine>();
                cfg.CreateMap<VEMOD.TransmissionType, CompanyTransmissionType>();
                cfg.CreateMap<VEMOD.ServiceType, CompanyServiceType>();
                cfg.CreateMap<VEMOD.Body, CompanyBody>();
                cfg.CreateMap<VEMOD.Version, CompanyVersion>();
            });
            return config;
        }
        #endregion version
        #region year
        public static IMapper CreateMapCompanyYear()
        {
            var config = MapperCache.GetMapper<VEMOD.Year, CompanyYear>(cfg =>
            {
                cfg.CreateMap<VEMOD.Version, CompanyVersion>();
                cfg.CreateMap<VEMOD.Model, CompanyModel>();
                cfg.CreateMap<VEMOD.Make, CompanyMake>();
                cfg.CreateMap<VEMOD.Year, CompanyYear>();
            });
            return config;
        }
        #endregion year
        #endregion automapper

        #region beneficiarios
        public static CompanyBeneficiary CreateBeneficiaryFromInsured(CompanyIssuanceInsured insured)
        {
            var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
            return new CompanyBeneficiary
            {
                IndividualId = insured.IndividualId,
                IdentificationDocument = insured.IdentificationDocument,
                Name = insured.Name,
                Participation = CommisionValue.Participation,
                CustomerType = insured.CustomerType,
                CompanyName = insured.CompanyName,
                IndividualType = insured.IndividualType,
                BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription },
                BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription
            };
        }
        #endregion

        internal static VEMOD.Version CreateVersion(CompanyVersion companyVersion)
        {
            var imapper = CreateMapCompanyVersion();
            return imapper.Map<CompanyVersion, VEMOD.Version>(companyVersion);
        }

        internal static CompanyVersion CreateCompanyVersion(VEMOD.Version version)
        {
            var imapper = CreateMapCompanyVersion();
            return imapper.Map<VEMOD.Version, CompanyVersion>(version);
        }

        internal static List<CompanyVersion> CreateCompanyVersions(List<VEMOD.Version> modelVersions)
        {

            List<CompanyVersion> listCompanyVersions = new List<CompanyVersion>();
            foreach (VEMOD.Version item in modelVersions)
            {
                listCompanyVersions.Add(CreateCompanyVersion(item));
            }
            return listCompanyVersions;
        }

        #region VEHICLE  TO DATAtABLE
        public static DataTable GetDataTableTempRISK(CompanyVehicle companyVehicle)
        {
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable = new DataTable("INSERT_TEMP_RISK");
            #region FirsParms

            #region Columns
            dataTable.Columns.Add("OPERATION_ID", typeof(int));
            dataTable.Columns.Add("TEMP_ID", typeof(int));
            dataTable.Columns.Add("INSURED_ID", typeof(int));
            dataTable.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dataTable.Columns.Add("COVERED_RISK_TYPE_CD", typeof(int));
            dataTable.Columns.Add("RISK_NUM", typeof(int));
            dataTable.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dataTable.Columns.Add("POLICY_ID", typeof(int));
            dataTable.Columns.Add("RISK_STATUS_CD", typeof(int));
            dataTable.Columns.Add("RISK_ORIGINAL_STATUS_CD", typeof(int));
            dataTable.Columns.Add("RISK_INSP_TYPE_CD", typeof(int));
            dataTable.Columns.Add("INSPECTION_ID", typeof(int));
            dataTable.Columns.Add("CONDITION_TEXT", typeof(string));
            dataTable.Columns.Add("RATING_ZONE_CD", typeof(int));
            dataTable.Columns.Add("COVER_GROUP_ID", typeof(int));
            dataTable.Columns.Add("PREFIX_CD", typeof(int));
            dataTable.Columns.Add("IS_FACULTATIVE", typeof(bool));
            dataTable.Columns.Add("NAME_NUM", typeof(int));
            dataTable.Columns.Add("ADDRESS_ID", typeof(int));
            dataTable.Columns.Add("PHONE_ID", typeof(int));
            dataTable.Columns.Add("RISK_COMMERCIAL_CLASS_CD", typeof(int));
            dataTable.Columns.Add("RISK_COMMERCIAL_TYPE_CD", typeof(int));
            dataTable.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dataTable.Columns.Add("SECONDARY_INSURED_ID", typeof(int));
            #endregion

            DataRow rows = dataTable.NewRow();

            #region DataRows
            rows["OPERATION_ID"] = companyVehicle.Risk.Id;
            rows["TEMP_ID"] = companyVehicle.Risk.Policy.Endorsement.TemporalId;
            rows["INSURED_ID"] = companyVehicle.Risk.MainInsured.IndividualId;
            rows["CUSTOMER_TYPE_CD"] = companyVehicle.Risk.MainInsured.CustomerType;
            rows["COVERED_RISK_TYPE_CD"] = companyVehicle.Risk.CoveredRiskType;
            rows["RISK_NUM"] = companyVehicle.Risk.Number > 0 ? companyVehicle.Risk.Number : 1;
            if (companyVehicle.Risk.Policy.Endorsement.Id > 0)
            {
                rows["ENDORSEMENT_ID"] = companyVehicle.Risk.Policy.Endorsement.Id;
            }
            rows["POLICY_ID"] = companyVehicle.Risk.Policy.Endorsement.PolicyId > 0 ? companyVehicle.Risk.Policy.Endorsement.PolicyId : rows["POLICY_ID"];
            rows["RISK_STATUS_CD"] = companyVehicle.Risk.Status;
            if (companyVehicle.Risk.OriginalStatus != null)
            {
                rows["RISK_ORIGINAL_STATUS_CD"] = companyVehicle.Risk.OriginalStatus;
            }
            if (companyVehicle.Inspection != null && companyVehicle.Inspection.InspectionType > 0)
            {
                rows["RISK_INSP_TYPE_CD"] = companyVehicle.Inspection.InspectionType;
            }
            else
            {
                rows["RISK_INSP_TYPE_CD"] = 1;
            }
            rows["INSPECTION_ID"] = DBNull.Value;
            if (companyVehicle.Risk.Text != null)
            {
                rows["CONDITION_TEXT"] = companyVehicle.Risk.Text.TextBody;
            }
            rows["RATING_ZONE_CD"] = companyVehicle.Risk.RatingZone.Id;
            rows["COVER_GROUP_ID"] = companyVehicle.Risk.GroupCoverage.Id;
            rows["PREFIX_CD"] = companyVehicle.Risk.Policy.Prefix.Id;
            rows["IS_FACULTATIVE"] = false;
            if (companyVehicle.Risk.MainInsured.CompanyName != null)
            {
                if (companyVehicle.Risk.MainInsured.CompanyName.NameNum > 0)
                {
                    rows["NAME_NUM"] = companyVehicle.Risk.MainInsured.CompanyName.NameNum;
                }
                if (companyVehicle.Risk.MainInsured.CompanyName.Address != null && companyVehicle.Risk.MainInsured.CompanyName.Address.Id > 0)
                {
                    rows["ADDRESS_ID"] = companyVehicle.Risk.MainInsured.CompanyName.Address.Id;
                }
                if (companyVehicle.Risk.MainInsured.CompanyName.Phone != null && companyVehicle.Risk.MainInsured.CompanyName.Phone.Id > 0)
                {
                    rows["PHONE_ID"] = companyVehicle.Risk.MainInsured.CompanyName.Phone.Id;
                }
            }
            rows["RISK_COMMERCIAL_CLASS_CD"] = DBNull.Value;
            rows["RISK_COMMERCIAL_TYPE_CD"] = DBNull.Value;
            if (companyVehicle.Risk.DynamicProperties != null && companyVehicle.Risk.DynamicProperties?.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionPolicy = new DynamicPropertiesCollection();

                for (int i = 0; i < companyVehicle.Risk.DynamicProperties?.Count; i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = companyVehicle.Risk.DynamicProperties[i].Id;
                    dinamycProperty.Value = companyVehicle.Risk.DynamicProperties[i].Value;
                    dynamicCollectionPolicy[i] = dinamycProperty;
                }

                rows["DYNAMIC_PROPERTIES"] = dynamicPropertiesSerializer.Serialize(dynamicCollectionPolicy);//--Serialize;
            }
            if (companyVehicle.Risk.SecondInsured != null)
            {
                rows["SECONDARY_INSURED_ID"] = companyVehicle.Risk.SecondInsured.IndividualId;
            }

            #endregion

            dataTable.Rows.Add(rows);
            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableCOTempRisk(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_CO_TEMP_RISK");
            #region twoParams

            #region Columns
            dataTable.Columns.Add("LIMITS_RC_CD", typeof(int));
            dataTable.Columns.Add("LIMIT_RC_SUM", typeof(decimal));
            dataTable.Columns.Add("100_RETENTION", typeof(bool));
            dataTable.Columns.Add("SINISTER_PCT", typeof(decimal));
            dataTable.Columns.Add("HAS_SINISTER", typeof(bool));
            dataTable.Columns.Add("ASSISTANCE_CD", typeof(int));
            dataTable.Columns.Add("SINISTER_QTY", typeof(int));
            dataTable.Columns.Add("ACTUAL_DATE_MOVEMENT", typeof(DateTime));
            #endregion

            DataRow rows = dataTable.NewRow();

            #region Rows
            rows["LIMITS_RC_CD"] = companyVehicle.Risk.LimitRc.Id;
            rows["LIMIT_RC_SUM"] = companyVehicle.Risk.LimitRc.LimitSum;
            rows["100_RETENTION"] = DBNull.Value;
            rows["SINISTER_PCT"] = DBNull.Value;
            rows["HAS_SINISTER"] = companyVehicle.Risk.HasSinister;
            if (companyVehicle.Risk?.AssistanceType?.Id == null)
            {
                rows["ASSISTANCE_CD"] = DBNull.Value;
            }
            else
            {
                rows["ASSISTANCE_CD"] = companyVehicle.Risk?.AssistanceType?.Id;
            }
            rows["SINISTER_QTY"] = DBNull.Value;
            rows["ACTUAL_DATE_MOVEMENT"] = (companyVehicle.Risk.ActualDateMovement == DateTime.MinValue) ? DateTime.Now : companyVehicle.Risk.ActualDateMovement;
            dataTable.Rows.Add(rows);
            #endregion

            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableCPTTempRisk(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_CPT_TEMP_RISK");
            #region RISK


            dataTable.Columns.Add("FINE_DATE", typeof(DateTime));
            dataTable.Columns.Add("IS_FINE", typeof(bool));
            dataTable.Columns.Add("SCORE_DATE", typeof(DateTime));
            dataTable.Columns.Add("IS_SCORE", typeof(bool));
            dataTable.Columns.Add("SCORE", typeof(string));
            dataTable.Columns.Add("GROUP_FINE_ID", typeof(int));
            dataTable.Columns.Add("NEW_RENOVATED", typeof(int));
            dataTable.Columns.Add("RENEWAL_NUMBER", typeof(int));

            DataRow rows = dataTable.NewRow();

            if (companyVehicle.InfringementSimit != null)
            {
                if (companyVehicle.InfringementSimit.FineDate.GetValueOrDefault() != DateTime.MinValue)
                {
                    rows["FINE_DATE"] = companyVehicle.InfringementSimit.FineDate.GetValueOrDefault();
                }
                rows["IS_FINE"] = companyVehicle.InfringementSimit.IsFine;
                if (companyVehicle.InfringementSimit.GroupFineId.HasValue && companyVehicle.InfringementSimit.GroupFineId.Value > 0)
                {
                    rows["GROUP_FINE_ID"] = companyVehicle.InfringementSimit.GroupFineId.Value;
                }
            }

            if (companyVehicle.Risk.MainInsured.ScoreCredit != null)
            {
                if (companyVehicle.Risk.MainInsured.ScoreCredit.DateRequest != DateTime.MinValue)
                {
                    rows["SCORE_DATE"] = companyVehicle.Risk.MainInsured.ScoreCredit.DateRequest;
                }
                rows["IS_SCORE"] = companyVehicle.Risk.MainInsured.ScoreCredit.IsScore;
                rows["SCORE"] = companyVehicle.Risk.MainInsured.ScoreCredit.Score;
            }
            else
            {
                rows["SCORE_DATE"] = DBNull.Value;
                rows["IS_SCORE"] = DBNull.Value;
                rows["SCORE"] = DBNull.Value;
            }

            rows["NEW_RENOVATED"] = companyVehicle.NewRenovated.GetValueOrDefault();
            rows["RENEWAL_NUMBER"] = companyVehicle.RenewallNum.GetValueOrDefault();

            dataTable.Rows.Add(rows);
            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableRiskBeneficiary(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_BENEFICIARY_ADD");
            #region BENEFICARY


            dataTable.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dataTable.Columns.Add("BENEFICIARY_ID", typeof(int));
            dataTable.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
            dataTable.Columns.Add("BENEFICT_PCT", typeof(decimal));
            dataTable.Columns.Add("ADDRESS_ID", typeof(int));
            dataTable.Columns.Add("NAME_NUM", typeof(int));

            if (companyVehicle.Risk.Beneficiaries != null)
            {
                foreach (CompanyBeneficiary companyBeneficiary in companyVehicle.Risk.Beneficiaries)
                {
                    DataRow rows = dataTable.NewRow();

                    rows["CUSTOMER_TYPE_CD"] = companyBeneficiary.CustomerType;
                    rows["BENEFICIARY_ID"] = companyBeneficiary.IndividualId;
                    rows["BENEFICIARY_TYPE_CD"] = companyBeneficiary.BeneficiaryType.Id;
                    rows["BENEFICT_PCT"] = companyBeneficiary.Participation;
                    rows["ADDRESS_ID"] = companyBeneficiary.CompanyName.Address.Id;
                    rows["NAME_NUM"] = companyBeneficiary.CompanyName.NameNum;

                    dataTable.Rows.Add(rows);
                }
            }
            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableRiskPayer(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_PAYER");
            #region PAYER
            dataTable.Columns.Add("PAYER_ID", typeof(int));
            dataTable.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dataTable.Columns.Add("PAYER_NUM", typeof(int));
            dataTable.Columns.Add("PREMIUM_PART_PCT", typeof(decimal));
            dataTable.Columns.Add("ENDORSEMENT_ID", typeof(int));

            DataRow rows = dataTable.NewRow();
            rows["PAYER_ID"] = companyVehicle.Risk.Policy.Holder.IndividualId;
            rows["CUSTOMER_TYPE_CD"] = companyVehicle.Risk.Policy.Holder.CustomerType;
            rows["PAYER_NUM"] = 1;
            rows["PREMIUM_PART_PCT"] = 100;
            if (companyVehicle.Risk.Policy.Endorsement.Id > 0)
            {
                rows["ENDORSEMENT_ID"] = companyVehicle.Risk.Policy.Endorsement.Id;
            }

            dataTable.Rows.Add(rows);
            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableRiskClause(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_CLAUSE");
            dataTable.Columns.Add("CLAUSE_ID", typeof(int));
            dataTable.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dataTable.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dataTable.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyVehicle.Risk.Clauses != null && companyVehicle.Risk.Clauses.Count > 0)
            {
                foreach (CompanyClause companyClause in companyVehicle.Risk.Clauses)
                {
                    DataRow rows = dataTable.NewRow();

                    rows["CLAUSE_ID"] = companyClause.Id;
                    if (companyVehicle.Risk.Policy.Endorsement?.Id > 0)
                    {
                        rows["ENDORSEMENT_ID"] = companyVehicle.Risk.Policy.Endorsement.Id;
                    }
                    rows["CLAUSE_STATUS_CD"] = DBNull.Value;
                    rows["CLAUSE_ORIG_STATUS_CD"] = DBNull.Value;

                    dataTable.Rows.Add(rows);
                }
            }
            return dataTable;
        }

        public static DataTable GetDataTableRisCoverage(CompanyVehicle companyVehicle)
        {
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_COVERAGE_ALL");

            dataTable.Columns.Add("COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("IS_DECLARATIVE", typeof(bool));
            dataTable.Columns.Add("IS_MIN_PREMIUM_DEPOSIT", typeof(bool));
            dataTable.Columns.Add("FIRST_RISK_TYPE_CD", typeof(int));
            dataTable.Columns.Add("CALCULATION_TYPE_CD", typeof(int));
            dataTable.Columns.Add("DECLARED_AMT", typeof(decimal));
            dataTable.Columns.Add("PREMIUM_AMT", typeof(decimal));
            dataTable.Columns.Add("LIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("SUBLIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("LIMIT_IN_EXCESS", typeof(decimal));
            dataTable.Columns.Add("LIMIT_OCCURRENCE_AMT", typeof(decimal));
            dataTable.Columns.Add("LIMIT_CLAIMANT_AMT", typeof(decimal));
            dataTable.Columns.Add("ACC_PREMIUM_AMT", typeof(decimal));
            dataTable.Columns.Add("ACC_LIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("ACC_SUBLIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dataTable.Columns.Add("RATE_TYPE_CD", typeof(int));
            dataTable.Columns.Add("RATE", typeof(decimal));
            dataTable.Columns.Add("CURRENT_TO", typeof(DateTime));
            dataTable.Columns.Add("COVER_NUM", typeof(int));
            dataTable.Columns.Add("RISK_COVER_ID", typeof(int));
            dataTable.Columns.Add("COVER_STATUS_CD", typeof(int));
            dataTable.Columns.Add("COVER_ORIGINAL_STATUS_CD", typeof(int));
            dataTable.Columns.Add("CONDITION_TEXT", typeof(string));
            dataTable.Columns.Add("ENDORSEMENT_LIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));

            dataTable.Columns.Add("FLAT_RATE_PCT", typeof(decimal));
            dataTable.Columns.Add("CONTRACT_AMOUNT_PCT", typeof(decimal));
            dataTable.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dataTable.Columns.Add("SHORT_TERM_PCT", typeof(decimal));
            dataTable.Columns.Add("MAIN_COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("MAIN_COVERAGE_PCT", typeof(decimal));
            dataTable.Columns.Add("DIFF_MIN_PREMIUM_AMT", typeof(decimal));


            if (companyVehicle.Risk.Coverages != null)
            {
                foreach (CompanyCoverage companyCoverage in companyVehicle.Risk.Coverages)
                {
                    DataRow rows = dataTable.NewRow();

                    rows["COVERAGE_ID"] = companyCoverage.Id;
                    rows["IS_DECLARATIVE"] = companyCoverage.IsDeclarative;
                    rows["IS_MIN_PREMIUM_DEPOSIT"] = companyCoverage.IsMinPremiumDeposit;
                    rows["FIRST_RISK_TYPE_CD"] = (int)Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType.None;
                    rows["CALCULATION_TYPE_CD"] = companyCoverage.CalculationType.Value;
                    rows["DECLARED_AMT"] = companyCoverage.DeclaredAmount;
                    rows["PREMIUM_AMT"] = companyCoverage.PremiumAmount;
                    rows["LIMIT_AMT"] = companyCoverage.LimitAmount;
                    rows["SUBLIMIT_AMT"] = companyCoverage.SubLimitAmount;
                    rows["LIMIT_IN_EXCESS"] = companyCoverage.ExcessLimit;
                    rows["LIMIT_OCCURRENCE_AMT"] = companyCoverage.LimitOccurrenceAmount;
                    rows["LIMIT_CLAIMANT_AMT"] = companyCoverage.LimitClaimantAmount;
                    rows["ACC_PREMIUM_AMT"] = companyCoverage.AccumulatedPremiumAmount;
                    rows["ACC_LIMIT_AMT"] = companyCoverage.AccumulatedLimitAmount;
                    rows["ACC_SUBLIMIT_AMT"] = companyCoverage.AccumulatedSubLimitAmount;
                    rows["CURRENT_FROM"] = companyCoverage.CurrentFrom;
                    rows["RATE_TYPE_CD"] = companyCoverage.RateType;
                    rows["RATE"] = (object)companyCoverage.Rate ?? DBNull.Value;
                    if (companyCoverage.CurrentTo.GetValueOrDefault() != null && companyCoverage.CurrentTo.GetValueOrDefault() != DateTime.MinValue)
                    {
                        rows["CURRENT_TO"] = companyCoverage.CurrentTo;
                    }
                    else
                    {
                        rows["CURRENT_TO"] = DBNull.Value;
                    }
                    rows["COVER_NUM"] = companyCoverage.Number;
                    if (companyCoverage.RiskCoverageId > 0)
                    {
                        rows["RISK_COVER_ID"] = companyCoverage.RiskCoverageId;
                    }

                    if (companyCoverage.CoverStatus.HasValue)
                    {
                        rows["COVER_STATUS_CD"] = companyCoverage.CoverStatus.Value;
                    }
                    else
                    {
                        rows["COVER_STATUS_CD"] = CoverageStatusType.Original;
                    }
                    rows["COVER_ORIGINAL_STATUS_CD"] = DBNull.Value;
                    if (companyCoverage.Text != null)
                    {
                        rows["CONDITION_TEXT"] = companyCoverage.Text.TextBody;
                    }
                    else
                    {
                        rows["CONDITION_TEXT"] = DBNull.Value;
                    }
                    rows["ENDORSEMENT_LIMIT_AMT"] = companyCoverage.EndorsementLimitAmount;
                    rows["ENDORSEMENT_SUBLIMIT_AMT"] = companyCoverage.EndorsementSublimitAmount;
                    if (companyCoverage.FlatRatePorcentage > 0)
                    {
                        rows["FLAT_RATE_PCT"] = companyCoverage.FlatRatePorcentage;
                    }
                    rows["CONTRACT_AMOUNT_PCT"] = companyCoverage.ContractAmountPercentage;
                    if (companyCoverage.DynamicProperties != null && companyCoverage.DynamicProperties.Count > 0)
                    {
                        DynamicPropertiesCollection dynamicCollectionCoverage = new DynamicPropertiesCollection();
                        for (int i = 0; i < companyCoverage.DynamicProperties.Count(); i++)
                        {
                            DynamicProperty dinamycProperty = new DynamicProperty();
                            dinamycProperty.Id = companyCoverage.DynamicProperties[i].Id;
                            dinamycProperty.Value = companyCoverage.DynamicProperties[i].Value;
                            dynamicCollectionCoverage[i] = dinamycProperty;
                        }

                        byte[] serializedValuesCoverage = dynamicPropertiesSerializer.Serialize(dynamicCollectionCoverage);
                        rows["DYNAMIC_PROPERTIES"] = serializedValuesCoverage;
                    }
                    rows["SHORT_TERM_PCT"] = companyCoverage.ShortTermPercentage;
                    if (companyCoverage.MainCoverageId != null)
                    {
                        rows["MAIN_COVERAGE_ID"] = companyCoverage.MainCoverageId;
                    }
                    else
                    {
                        rows["MAIN_COVERAGE_ID"] = 0;
                    }

                    rows["MAIN_COVERAGE_PCT"] = companyCoverage.MainCoveragePercentage.GetValueOrDefault();
                    rows["DIFF_MIN_PREMIUM_AMT"] = companyCoverage.DiffMinPremiumAmount.GetValueOrDefault();
                    dataTable.Rows.Add(rows);
                }
            }

            return dataTable;
        }

        public static DataTable GetDataTableDeduct(CompanyVehicle companyVehicle)
        {
            DataTable dtTableDeduct = new DataTable("INSERT_TEMP_RISK_COVER_DEDUCT");

            dtTableDeduct.Columns.Add("COVERAGE_ID", typeof(int));
            dtTableDeduct.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtTableDeduct.Columns.Add("RATE", typeof(decimal));
            dtTableDeduct.Columns.Add("DEDUCT_PREMIUM_AMT", typeof(decimal));
            dtTableDeduct.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            dtTableDeduct.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtTableDeduct.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtTableDeduct.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtTableDeduct.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));
            dtTableDeduct.Columns.Add("MIN_DEDUCT_SUBJECT_CD", typeof(int));
            dtTableDeduct.Columns.Add("MAX_DEDUCT_VALUE", typeof(decimal));
            dtTableDeduct.Columns.Add("MAX_DEDUCT_UNIT_CD", typeof(int));
            dtTableDeduct.Columns.Add("MAX_DEDUCT_SUBJECT_CD", typeof(int));
            dtTableDeduct.Columns.Add("CURRENCY_CD", typeof(int));
            dtTableDeduct.Columns.Add("ACC_DEDUCT_AMT", typeof(decimal));
            dtTableDeduct.Columns.Add("DEDUCT_ID", typeof(int));

            if (companyVehicle.Risk.Coverages != null)
            {
                foreach (CompanyCoverage companyCoverage in companyVehicle.Risk.Coverages)
                {
                    DataRow rowDeduct = dtTableDeduct.NewRow();

                    if (companyCoverage.Deductible != null)
                    {
                        rowDeduct["COVERAGE_ID"] = companyCoverage.Id;
                        rowDeduct["RATE_TYPE_CD"] = companyCoverage.Deductible.RateType;
                        rowDeduct["RATE"] = companyCoverage.Deductible.Rate;
                        if (companyCoverage.Deductible != null)
                        {
                            rowDeduct["DEDUCT_PREMIUM_AMT"] = companyCoverage.Deductible.DeductPremiumAmount;
                            rowDeduct["DEDUCT_VALUE"] = companyCoverage.Deductible.DeductValue;
                        }
                        if (companyCoverage.Deductible.DeductibleUnit != null && companyCoverage.Deductible.DeductibleUnit.Id != 0)
                        {
                            rowDeduct["DEDUCT_UNIT_CD"] = companyCoverage.Deductible.DeductibleUnit.Id;
                        }
                        if (companyCoverage.Deductible.DeductibleSubject != null)
                        {
                            rowDeduct["DEDUCT_SUBJECT_CD"] = companyCoverage.Deductible.DeductibleSubject.Id;
                        }
                        if (companyCoverage.Deductible.MinDeductValue.HasValue)
                        {
                            rowDeduct["MIN_DEDUCT_VALUE"] = companyCoverage.Deductible.MinDeductValue.Value;
                        }
                        if (companyCoverage.Deductible.MinDeductibleUnit != null && companyCoverage.Deductible.MinDeductibleUnit.Id != 0)
                        {
                            rowDeduct["MIN_DEDUCT_UNIT_CD"] = companyCoverage.Deductible.MinDeductibleUnit.Id;
                        }
                        if (companyCoverage.Deductible.MinDeductibleSubject != null && companyCoverage.Deductible.MinDeductibleSubject.Id != 0)
                        {
                            rowDeduct["MIN_DEDUCT_SUBJECT_CD"] = companyCoverage.Deductible.MinDeductibleSubject.Id;
                        }
                        if (companyCoverage.Deductible.MaxDeductValue.HasValue)
                        {
                            rowDeduct["MAX_DEDUCT_VALUE"] = companyCoverage.Deductible.MaxDeductValue.Value;
                        }
                        if (companyCoverage.Deductible.MaxDeductibleUnit != null && companyCoverage.Deductible.MaxDeductibleUnit.Id != 0)
                        {
                            rowDeduct["MAX_DEDUCT_UNIT_CD"] = companyCoverage.Deductible.MaxDeductibleUnit.Id;
                        }
                        if (companyCoverage.Deductible.MaxDeductibleSubject != null && companyCoverage.Deductible.MaxDeductibleSubject.Id != 0)
                        {
                            rowDeduct["MAX_DEDUCT_SUBJECT_CD"] = companyCoverage.Deductible.MaxDeductibleSubject.Id;
                        }
                        if (companyCoverage.Deductible.Currency != null)
                        {
                            rowDeduct["CURRENCY_CD"] = companyCoverage.Deductible.Currency.Id;
                        }
                        rowDeduct["ACC_DEDUCT_AMT"] = companyCoverage.Deductible.AccDeductAmt;
                        rowDeduct["DEDUCT_ID"] = companyCoverage.Deductible.Id;
                        dtTableDeduct.Rows.Add(rowDeduct);
                    }
                }
            }
            return dtTableDeduct;
        }

        public static DataTable GetDataTableCoverClause(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_COVER_CLAUSE");

            dataTable.Columns.Add("COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("CLAUSE_ID", typeof(int));
            dataTable.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dataTable.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (companyVehicle.Risk.Coverages != null)
            {
                foreach (CompanyCoverage companyCoverage in companyVehicle.Risk.Coverages)
                {
                    if (companyCoverage.Clauses != null)
                    {
                        foreach (CompanyClause companyClause in companyCoverage.Clauses)
                        {
                            DataRow rowCoverClause = dataTable.NewRow();

                            rowCoverClause["COVERAGE_ID"] = companyCoverage.Id;
                            rowCoverClause["CLAUSE_ID"] = companyClause.Id;
                            rowCoverClause["CLAUSE_STATUS_CD"] = DBNull.Value;
                            rowCoverClause["CLAUSE_ORIG_STATUS_CD"] = DBNull.Value;

                            dataTable.Rows.Add(rowCoverClause);
                        }
                    }
                }
            }
            return dataTable;
        }

        public static DataTable GetDataTableAccessory(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_DETAIL_ACCESSORY");

            dataTable.Columns.Add("SUBLIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("RATE_TYPE_CD", typeof(int));
            dataTable.Columns.Add("RATE", typeof(decimal));
            dataTable.Columns.Add("PREMIUM_AMT", typeof(decimal));
            dataTable.Columns.Add("ACC_PREMIUM_AMT", typeof(decimal));
            dataTable.Columns.Add("BRAND_NAME", typeof(string));
            dataTable.Columns.Add("MODEL", typeof(string));
            dataTable.Columns.Add("DETAIL_ID", typeof(int));
            dataTable.Columns.Add("COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("COVER_STATUS_CD", typeof(Int16));

            if (companyVehicle.Accesories != null)
            {
                CommonModels.Parameter parameterOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.OriginalAccessories);
                CommonModels.Parameter parameterNoOriginal = DelegateService.commonService.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories);

                foreach (CompanyAccessory companyAccessory in companyVehicle.Accesories)
                {
                    DataRow rowAccessory = dataTable.NewRow();

                    rowAccessory["SUBLIMIT_AMT"] = companyAccessory.Amount;
                    rowAccessory["RATE_TYPE_CD"] = companyAccessory.RateType;
                    rowAccessory["RATE"] = Math.Round(companyAccessory.Rate, 6);
                    rowAccessory["PREMIUM_AMT"] = Math.Round(companyAccessory.Premium, 2);
                    rowAccessory["ACC_PREMIUM_AMT"] = Math.Round(companyAccessory.AccumulatedPremium, 2);
                    rowAccessory["BRAND_NAME"] = companyAccessory.Make ?? "";
                    rowAccessory["MODEL"] = companyAccessory.Description ?? "";
                    rowAccessory["DETAIL_ID"] = companyAccessory.Id;
                    if (companyAccessory.IsOriginal)
                    {
                        rowAccessory["COVERAGE_ID"] = parameterOriginal.NumberParameter.Value;
                    }
                    else
                    {
                        rowAccessory["COVERAGE_ID"] = parameterNoOriginal.NumberParameter.Value;
                    }
                    rowAccessory["COVER_STATUS_CD"] = companyAccessory.Status;
                    dataTable.Rows.Add(rowAccessory);
                }
            }
            return dataTable;
        }

        public static DataTable GetDataTableDynamic(CompanyVehicle companyVehicle)
        {
            DataTable dtTableDynamicProperties = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES_GENERAL");


            dtTableDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
            dtTableDynamicProperties.Columns.Add("ENTITY_ID", typeof(int));
            dtTableDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));
            dtTableDynamicProperties.Columns.Add("QUESTION_ID", typeof(int));

            if (companyVehicle.Risk.DynamicProperties != null)
            {
                foreach (DynamicConcept item in companyVehicle.Risk.DynamicProperties)
                {
                    if (item.Value != null)
                    {
                        DataRow dataRow = dtTableDynamicProperties.NewRow();
                        dataRow["DYNAMIC_ID"] = item.Id;
                        dataRow["ENTITY_ID"] = item.EntityId;
                        dataRow["CONCEPT_VALUE"] = item.Value;

                        if (item.QuestionId.HasValue)
                        {
                            dataRow["QUESTION_ID"] = item.QuestionId;
                        }
                        else
                        {
                            dataRow["QUESTION_ID"] = DBNull.Value;
                        }

                        dtTableDynamicProperties.Rows.Add(dataRow);
                    }

                }
            }

            return dtTableDynamicProperties;
        }

        public static DataTable GetDataTableDynamicCoverage(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES_COVERGE");

            dataTable.Columns.Add("DYNAMIC_ID", typeof(int));
            dataTable.Columns.Add("COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("ENTITY_ID", typeof(int));
            dataTable.Columns.Add("CONCEPT_VALUE", typeof(string));
            dataTable.Columns.Add("QUESTION_ID", typeof(int));

            if (companyVehicle.Risk.Coverages != null)
            {
                foreach (CompanyCoverage coverage in companyVehicle.Risk.Coverages)
                {
                    if (coverage.DynamicProperties != null)
                    {
                        foreach (DynamicConcept dynamicConcept in coverage.DynamicProperties)
                        {
                            if (dynamicConcept.Value != null)
                            {
                                DataRow dataRow = dataTable.NewRow();
                                dataRow["DYNAMIC_ID"] = dynamicConcept.Id;
                                dataRow["COVERAGE_ID"] = coverage.Id;
                                dataRow["ENTITY_ID"] = dynamicConcept.EntityId;
                                dataRow["CONCEPT_VALUE"] = dynamicConcept.Value;

                                if (dynamicConcept.QuestionId.HasValue)
                                {
                                    dataRow["QUESTION_ID"] = dynamicConcept.QuestionId;
                                }
                                else
                                {
                                    dataRow["QUESTION_ID"] = DBNull.Value;
                                }

                                dataTable.Rows.Add(dataRow);
                            }

                        }
                    }
                }
            }

            return dataTable;
        }

        public static DataTable GetDataTableRiskVehicle(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_VEHICLE");

            dataTable.Columns.Add("VEHICLE_MAKE_CD", typeof(int));
            dataTable.Columns.Add("VEHICLE_MODEL_CD", typeof(int));
            dataTable.Columns.Add("VEHICLE_VERSION_CD", typeof(int));
            dataTable.Columns.Add("VEHICLE_YEAR", typeof(int));
            dataTable.Columns.Add("VEHICLE_TYPE_CD", typeof(int));
            dataTable.Columns.Add("VEHICLE_USE_CD", typeof(int));
            dataTable.Columns.Add("VEHICLE_BODY_CD", typeof(int));
            dataTable.Columns.Add("VEHICLE_PRICE", typeof(decimal));
            dataTable.Columns.Add("IS_NEW", typeof(bool));
            dataTable.Columns.Add("LICENSE_PLATE", typeof(string));
            dataTable.Columns.Add("ENGINE_SER_NO", typeof(string));
            dataTable.Columns.Add("CHASSIS_SER_NO", typeof(string));
            dataTable.Columns.Add("VEHICLE_COLOR_CD", typeof(int));
            dataTable.Columns.Add("LOAD_TYPE_CD", typeof(int));
            dataTable.Columns.Add("TRAILERS_QTY", typeof(int));
            dataTable.Columns.Add("PASSENGER_QTY", typeof(int));
            dataTable.Columns.Add("NEW_VEHICLE_PRICE", typeof(decimal));
            dataTable.Columns.Add("VEHICLE_FUEL_CD", typeof(int));
            dataTable.Columns.Add("STD_VEHICLE_PRICE", typeof(decimal));

            DataRow rows = dataTable.NewRow();

            rows["VEHICLE_MAKE_CD"] = companyVehicle.Make.Id;
            rows["VEHICLE_MODEL_CD"] = companyVehicle.Model.Id;
            rows["VEHICLE_VERSION_CD"] = companyVehicle.Version.Id;
            rows["VEHICLE_YEAR"] = companyVehicle.Year;
            rows["VEHICLE_TYPE_CD"] = companyVehicle.Version.Type.Id;
            rows["VEHICLE_USE_CD"] = companyVehicle.Use.Id;
            rows["VEHICLE_BODY_CD"] = companyVehicle.Version.Body.Id;
            rows["VEHICLE_PRICE"] = companyVehicle.Price;
            rows["IS_NEW"] = companyVehicle.IsNew;
            rows["LICENSE_PLATE"] = companyVehicle.LicensePlate;
            rows["ENGINE_SER_NO"] = companyVehicle.EngineSerial;
            rows["CHASSIS_SER_NO"] = companyVehicle.ChassisSerial;
            rows["VEHICLE_COLOR_CD"] = companyVehicle.Color.Id;
            rows["LOAD_TYPE_CD"] = DBNull.Value;
            if (companyVehicle.TrailersQuantity > 0)
            {
                rows["TRAILERS_QTY"] = companyVehicle.TrailersQuantity;
            }
            if (companyVehicle.PassengerQuantity > 0)
            {
                rows["PASSENGER_QTY"] = companyVehicle.PassengerQuantity;
            }
            rows["NEW_VEHICLE_PRICE"] = companyVehicle.NewPrice;
            rows["VEHICLE_FUEL_CD"] = companyVehicle.Version.Fuel.Id;
            rows["STD_VEHICLE_PRICE"] = companyVehicle.StandardVehiclePrice;

            dataTable.Rows.Add(rows);
            return dataTable;
        }
        public static DataTable GetDataTableTemRiskVehicle(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_CO_TEMP_RISK_VEHICLE");

            dataTable.Columns.Add("FLAT_RATE_PCT", typeof(decimal));
            dataTable.Columns.Add("SHUTTLE_CD", typeof(int));
            dataTable.Columns.Add("DEDUCT_ID", typeof(int));
            dataTable.Columns.Add("SERVICE_TYPE_CD", typeof(int));
            dataTable.Columns.Add("MOBILE_NUM", typeof(string));
            dataTable.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dataTable.Columns.Add("POLICY_ID", typeof(int));
            dataTable.Columns.Add("TONS_QTY", typeof(int));
            dataTable.Columns.Add("EXCESS", typeof(bool));
            dataTable.Columns.Add("RATE_TYPE_CD", typeof(int));
            dataTable.Columns.Add("GOOD_EXPERIENCE_NUM", typeof(decimal));
            dataTable.Columns.Add("WORKER_TYPE", typeof(int));
            dataTable.Columns.Add("BIRTH_DATE", typeof(DateTime));
            dataTable.Columns.Add("IS_NEW_RATE", typeof(bool));
            dataTable.Columns.Add("FIRST_INSURED_BIRTH_DATE", typeof(DateTime));
            dataTable.Columns.Add("FIRST_INSURED_GENDER", typeof(string));
            dataTable.Columns.Add("GOOD_EXP_NUM_RATE", typeof(string));
            dataTable.Columns.Add("GOOD_EXP_NUM_PRINTER", typeof(int));

            DataRow rows = dataTable.NewRow();

            rows["FLAT_RATE_PCT"] = companyVehicle.Rate;
            rows["SHUTTLE_CD"] = DBNull.Value;
            rows["DEDUCT_ID"] = DBNull.Value;
            if (companyVehicle.ServiceType != null && companyVehicle.ServiceType.Id > 0)
            {
                rows["SERVICE_TYPE_CD"] = companyVehicle.ServiceType.Id;
            }
            rows["MOBILE_NUM"] = DBNull.Value;
            if (companyVehicle.Risk.Policy.Endorsement.Id > 0)
            {
                rows["ENDORSEMENT_ID"] = companyVehicle.Risk.Policy.Endorsement.Id;
            }
            if (companyVehicle.Risk.Policy.Endorsement.PolicyId > 0)
            {
                rows["POLICY_ID"] = companyVehicle.Risk.Policy.Endorsement.PolicyId;
            }
            rows["TONS_QTY"] = DBNull.Value;
            rows["EXCESS"] = false;
            if (companyVehicle.RateType.HasValue && companyVehicle.RateType.Value > 0)
            {
                rows["RATE_TYPE_CD"] = companyVehicle.RateType.Value;
            }
            rows["WORKER_TYPE"] = companyVehicle.Risk.WorkerType.GetValueOrDefault();
            if (companyVehicle.BirthDateEldestson.GetValueOrDefault() != null && companyVehicle.BirthDateEldestson.GetValueOrDefault() != DateTime.MinValue)
            {
                rows["BIRTH_DATE"] = companyVehicle.BirthDateEldestson.GetValueOrDefault();
            }
            rows["IS_NEW_RATE"] = false;
            if (companyVehicle.Risk.MainInsured.BirthDate.GetValueOrDefault() != null && companyVehicle.Risk.MainInsured.BirthDate.GetValueOrDefault() != DateTime.MinValue)
            {
                rows["FIRST_INSURED_BIRTH_DATE"] = companyVehicle.Risk.MainInsured.BirthDate.GetValueOrDefault();
            }
            if (companyVehicle.Risk.MainInsured.Gender != null)
            {
                rows["FIRST_INSURED_GENDER"] = companyVehicle.Risk.MainInsured.Gender;
            }
            if (companyVehicle.GoodExperienceYear != null)
            {
                rows["GOOD_EXPERIENCE_NUM"] = companyVehicle.GoodExperienceYear.GoodExperienceNum;
                rows["GOOD_EXP_NUM_RATE"] = companyVehicle.GoodExperienceYear.GoodExpNumRate;
                rows["GOOD_EXP_NUM_PRINTER"] = companyVehicle.GoodExperienceYear.GoodExpNumPrinter;
            }

            dataTable.Rows.Add(rows);

            return dataTable;
        }
        public static DataTable GetDataTableInfringement(CompanyVehicle companyVehicle)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_CO_CPT_RISK_INFRINGEMENT");

            dataTable.Columns.Add("GROUP_INFRINGEMENT_CD", typeof(int));
            dataTable.Columns.Add("LAST_YEARS_INFRINGEMENT", typeof(int));
            dataTable.Columns.Add("INFRINGEMENT_ONE_YEAR", typeof(int));
            dataTable.Columns.Add("INFRINGEMENT_THREE_YEARS", typeof(int));

            if (companyVehicle.InfringementSimit != null && companyVehicle.InfringementSimit.ListInfringementCount != null)
            {
                foreach (CompanyInfringementCount infringementCount in companyVehicle.InfringementSimit.ListInfringementCount)
                {
                    DataRow rows = dataTable.NewRow();

                    rows["GROUP_INFRINGEMENT_CD"] = infringementCount.GroupInfringementCode;
                    rows["LAST_YEARS_INFRINGEMENT"] = infringementCount.InfringementsLastYear;
                    rows["INFRINGEMENT_ONE_YEAR"] = infringementCount.InfringementsPeriodOne;
                    rows["INFRINGEMENT_THREE_YEARS"] = infringementCount.InfringementsPeriodTwo;

                    dataTable.Rows.Add(rows);
                }
            }

            return dataTable;
        }
        #endregion


        public static CompanyMinPremium CreateCompanyMinPremium(COCOMMEN.MinPremiumRelation minPremiumRelation)
        {
            var config = MapperCache.GetMapper<COCOMMEN.MinPremiumRelation, CompanyMinPremium>(cfg =>
            {
                cfg.CreateMap<COCOMMEN.MinPremiumRelation, CompanyMinPremium>()
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Key1));
            });
            return config.Map<COCOMMEN.MinPremiumRelation, CompanyMinPremium>(minPremiumRelation);
        }


        public static List<CompanyCiaGroupCoverage> CreateCompanyCiaGroupCoverage(BusinessCollection businessCollection)
        {
            List<CompanyCiaGroupCoverage> companyCiaGroupCoverage = new List<CompanyCiaGroupCoverage>();
            foreach (PROD.CiaGroupCoverage item in businessCollection)
            {
                companyCiaGroupCoverage.Add(CreateCompanyCiaGroupCoverage(item));
            }
            return companyCiaGroupCoverage;
        }
        public static CompanyCiaGroupCoverage CreateCompanyCiaGroupCoverage(PROD.CiaGroupCoverage CiaGroupCoverage)
        {

            var config = MapperCache.GetMapper<PROD.CiaGroupCoverage, CompanyCiaGroupCoverage>(cfg =>
            {
                cfg.CreateMap<PROD.CiaGroupCoverage, CompanyCiaGroupCoverage>();
            });
            return config.Map<PROD.CiaGroupCoverage, CompanyCiaGroupCoverage>(CiaGroupCoverage);
        }

        public static CompanyServiceType CreateCompanyServiceMap(COMM.ServiceType CommServiceType)
        {
            var config = MapperCache.GetMapper<COMM.ServiceType, CompanyServiceType>(cfg =>
            {
                cfg.CreateMap<COMM.ServiceType, CompanyServiceType>();
            });
            return config.Map<COMM.ServiceType, CompanyServiceType>(CommServiceType);
        }

        public static List<CompanyServiceType> CreateCompanyService(BusinessCollection businessCollection)
        {
            List<CompanyServiceType> list = new List<CompanyServiceType>();
            foreach (COMM.ServiceType item in businessCollection)
            {
                list.Add(new CompanyServiceType()
                {
                    Description = item.Description,
                    Id = item.ServiceTypeCode,
                    Enabled = item.Enabled
                });
            }
            return list;

        }

        #region Event Work Flow Emision

        /// <summary>
        /// Creates the EventAuthorization by CompanyPolicy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static EventAuthorization CreateCompanyEventAuthorizationEmision(CompanyPolicy policy, int userId)
        {
            EventAuthorization Event = new EventAuthorization();
            try
            {
                Event.OPERATION1_ID = policy.Endorsement.TicketNumber.ToString();
                Event.OPERATION2_ID = policy.Endorsement.Id.ToString();
                Event.AUTHO_USER_ID = userId;
                Event.EVENT_ID = (int)UnderwritingServices.Enums.EventTypes.Subscription;
            }
            catch (Exception)
            {
            }
            return Event;
        }

        #endregion
		
		public static CompanyRisk CreateRisk(ISSEN.Risk entityRisk)
        {
            return new CompanyRisk
            {
                Id = entityRisk.RiskId,
                RiskId = entityRisk.RiskId,
                CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                MainInsured = new CompanyIssuanceInsured
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
                Policy = new CompanyPolicy
                {
                    Endorsement = new CompanyEndorsement
                    {

                    }
                }
            };
        }
        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }
    }
}