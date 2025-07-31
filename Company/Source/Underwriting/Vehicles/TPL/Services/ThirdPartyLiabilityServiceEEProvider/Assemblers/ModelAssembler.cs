using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Linq;
using COMM = Sistran.Core.Application.Common.Entities;
using CoreTPLModels = Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using VEMOC = Sistran.Company.Application.Vehicles.Models;
using CiaUnderwritingM = Sistran.Company.Application.UnderwritingServices.Models;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Company.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using Trans = Sistran.Core.Application.Transports.TransportBusinessService.Models;
using System.Data;
using System;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Enum;
using System.ComponentModel;
using System.Reflection;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using VEMO = Sistran.Core.Application.Vehicles.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Assemblers
{
    internal static class ModelAssembler
    {
        internal static CoreTPLModels.TplRisk CreateThirdPartyLiability(CompanyTplRisk companyTplRisk)
        {
            return Mapper.Map<CompanyTplRisk, CoreTPLModels.TplRisk>(companyTplRisk);
        }

        #region CompanyTplRisk


        public static IMapper CreateMapCompanyInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaUnderwritingM.CompanyIssuanceInsured, CiaPersonModel.CompanyInsured>();
            });

            return config.CreateMapper();
        }
        internal static CompanyTplRisk CreateThirdPartyLiability(ISSEN.Risk risk, ISSEN.RiskVehicle riskVehicle, ISSEN.CoRiskVehicle coRiskVehicle, ISSEN.EndorsementRisk endorsementRisk, ISSEN.CoRisk coRisk)
        {
            CompanyTplRisk tplRisk = new CompanyTplRisk
            {
                Risk = new CompanyRisk
                {
                    RiskId = risk.RiskId,
                    Number = endorsementRisk.RiskNum,
                    LimitRc = new CompanyLimitRc
                    {
                        Id = coRisk.LimitsRcCode == null ? 0 : coRisk.LimitsRcCode.Value,
                        LimitSum = coRisk.LimitRcSum.Value
                    },
                    CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                    GroupCoverage = new GroupCoverage
                    {
                        Id = risk.CoverGroupId.Value
                    },
                    DynamicProperties = new List<DynamicConcept>(),
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
                    Description = riskVehicle.LicensePlate,
                    Status = RiskStatusType.NotModified,
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    IsRetention = coRisk.IsRetention,
                    IsFacultative = risk.IsFacultative,

                },
                Version = new CompanyVersion
                {
                    Id = riskVehicle.VehicleVersionCode,
                    Fuel = new CompanyFuel
                    {
                        Id = riskVehicle.VehicleFuelCode.Value
                    },
                    Body = new CompanyBody
                    {
                        Id = riskVehicle.VehicleBodyCode
                    },
                    Type = new VEMOC.CompanyType
                    {
                        Id = riskVehicle.VehicleTypeCode
                    }
                },
                Model = new CompanyModel
                {
                    Id = riskVehicle.VehicleModelCode
                },
                Make = new CompanyMake
                {
                    Id = riskVehicle.VehicleMakeCode
                },
                Fuel = new CompanyFuel
                {
                    Id = (int)FuelType.NoAplica,
                    Description = GetDescription(FuelType.NoAplica)
                },
                Use = new CompanyUse
                {
                    Id = (int)UseType.NoAplica,
                    Description = GetDescription(UseType.NoAplica)
                },
                Year = riskVehicle.VehicleYear,

                LicensePlate = riskVehicle.LicensePlate,
                EngineSerial = riskVehicle.EngineSerNo,
                ChassisSerial = riskVehicle.ChassisSerNo,
                Color = new CompanyColor
                {
                    Id = riskVehicle.VehicleColorCode.Value
                },
                Shuttle = new CompanyShuttle
                {
                    Id = coRiskVehicle.ShuttleCode.GetValueOrDefault()
                },

                PassengerQuantity = riskVehicle.PassengerQuantity.GetValueOrDefault(),
                TypeCargoId = riskVehicle.LoadTypeCode.GetValueOrDefault(),
                TrailerQuantity = riskVehicle.TrailersQuantity.GetValueOrDefault(),
                Tons = coRiskVehicle.TonsQuantity.GetValueOrDefault(),
                PhoneNumber = coRiskVehicle.MobileNum,
                RateType = (RateType?)coRiskVehicle.RateTypeCode,
                Rate = coRiskVehicle.FlatRatePercentage.GetValueOrDefault(),
                Deductible = new CompanyDeductible
                {
                    Id = coRiskVehicle.DeductId.GetValueOrDefault()
                },
                ServiceType = new CompanyServiceType
                {
                    Id = coRiskVehicle.ServiceTypeCode.GetValueOrDefault()
                }

            };
            if (risk.RatingZoneCode != null)
            {
                tplRisk.Risk.RatingZone = new CompanyRatingZone
                {
                    Id = risk.RatingZoneCode.Value
                };

            }

            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                tplRisk.Risk.DynamicProperties.Add(dynamicConcept);
            }


            return tplRisk;
        }

        public static string GetDescription(System.Enum input)
        {
            System.Type type = input.GetType();
            MemberInfo[] memInfo = type.GetMember(input.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return input.ToString();
        }

        public static CompanyTplRisk CreateTemporalThirdPartyLiability(TMPEN.TempRisk risk, TMPEN.CoTempRisk coRisk, TMPEN.TempRiskVehicle riskVehicle, TMPEN.CoTempRiskVehicle coRiskVehicle)
        {
            CompanyTplRisk tplRisk = new CompanyTplRisk
            {
                Risk = new CompanyRisk
                {
                    RiskId = risk.RiskId,
                    LimitRc = new CompanyLimitRc
                    {
                        Id = coRisk.LimitsRcCode == null ? 0 : coRisk.LimitsRcCode.Value,
                        LimitSum = coRisk.LimitRcSum.Value
                    },
                    CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                    DynamicProperties = new List<DynamicConcept>(),
                    MainInsured = new CompanyIssuanceInsured
                    {
                        IndividualId = risk.InsuredId.GetValueOrDefault(),
                        CompanyName = new IssuanceCompanyName
                        {
                            NameNum = risk.NameNum.GetValueOrDefault(),
                            Address = new IssuanceAddress
                            {
                                Id = risk.AddressId.GetValueOrDefault()
                            }
                        }
                    },
                    Description = riskVehicle.LicensePlate,
                    Status = RiskStatusType.NotModified
                },
                Version = new CompanyVersion
                {
                    Id = riskVehicle.VehicleVersionCode,
                    Fuel = new CompanyFuel
                    {
                        Id = riskVehicle.VehicleFuelCode.Value
                    },
                    Body = new CompanyBody
                    {
                        Id = riskVehicle.VehicleBodyCode.GetValueOrDefault()
                    },
                    Type = new VEMOC.CompanyType
                    {
                        Id = riskVehicle.VehicleTypeCode
                    }
                },
                Model = new CompanyModel
                {
                    Id = riskVehicle.VehicleModelCode
                },
                Make = new CompanyMake
                {
                    Id = riskVehicle.VehicleMakeCode
                },
                Year = riskVehicle.VehicleYear,

                LicensePlate = riskVehicle.LicensePlate,
                EngineSerial = riskVehicle.EngineSerNo,
                ChassisSerial = riskVehicle.ChassisSerNo,
                Color = new CompanyColor
                {
                    Id = riskVehicle.VehicleColorCode.Value
                },
                Shuttle = new CompanyShuttle
                {
                    Id = coRiskVehicle.ShuttleCode.GetValueOrDefault()
                },

                PassengerQuantity = riskVehicle.PassengerQuantity.GetValueOrDefault(),
                TypeCargoId = riskVehicle.LoadTypeCode.GetValueOrDefault(),
                TrailerQuantity = riskVehicle.TrailersQuantity.GetValueOrDefault(),
                Tons = coRiskVehicle.TonsQuantity.GetValueOrDefault(),
                PhoneNumber = coRiskVehicle.MobileNum,
                RateType = (RateType?)coRiskVehicle.RateTypeCode,
                Rate = coRiskVehicle.FlatRatePercentage.GetValueOrDefault(),
                Deductible = new CompanyDeductible
                {
                    Id = coRiskVehicle.DeductId.GetValueOrDefault()
                },
                ServiceType = new CompanyServiceType
                {
                    Id = coRiskVehicle.ServiceTypeCode.GetValueOrDefault()
                }

            };
            if (risk.RatingZoneCode != null)
            {
                tplRisk.Risk.RatingZone = new CompanyRatingZone
                {
                    Id = risk.RatingZoneCode.Value
                };
            }

            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                tplRisk.Risk.DynamicProperties.Add(dynamicConcept);
            }

            return tplRisk;
        }

        #endregion

        #region Beneficiary
        public static List<CompanyBeneficiary> CreateBeneficiaries(List<ISSEN.RiskBeneficiary> entityBeneficiaries)
        {
            List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();

            foreach (ISSEN.RiskBeneficiary item in entityBeneficiaries)
            {
                beneficiaries.Add(CreateBeneficiary(item));
            }

            return beneficiaries;
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

        public static CompanyPolicy CreateThirdPartyLiabilityPolicy(CompanyPolicy tplPolicy, Rules.Facade facade)
        {
            return tplPolicy;
        }

        public static List<DynamicConcept> CreateDynamicConcepts(Rules.Facade facade)
        {
            List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

            foreach (Rules.Concept concept in facade.Concepts.Where(x => x.IsStatic == false))
            {
                dynamicConcepts.Add(CreateDynamicConcept(concept));
            }

            return dynamicConcepts;
        }

        internal static List<CompanyTplRisk> CreateTemporalThirdPartyLiabilities(BusinessCollection businessCollection)
        {
            List<CompanyTplRisk> thirdPartyLiabilities = new List<CompanyTplRisk>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                thirdPartyLiabilities.Add(CreateTemporalThirdPartyLiability(entityEndorsementOperation));
            }

            return thirdPartyLiabilities;
        }

        internal static CompanyTplRisk CreateTemporalThirdPartyLiability(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            CompanyTplRisk companyTplRisk = new CompanyTplRisk();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyTplRisk = COMUT.JsonHelper.DeserializeJson<CompanyTplRisk>(entityEndorsementOperation.Operation);
                companyTplRisk.Risk.Id = 0;
                companyTplRisk.Risk.Number = entityEndorsementOperation.RiskNumber.GetValueOrDefault();
                companyTplRisk.Risk.Coverages.AsParallel().ForAll(x => x.CoverageOriginalStatus = x.CoverStatus);
            }

            return companyTplRisk;
        }

        private static DynamicConcept CreateDynamicConcept(Rules.Concept dynamicConceptValue)
        {
            return new DynamicConcept
            {
                Id = dynamicConceptValue.Id,
                Value = dynamicConceptValue.Value,
                EntityId = dynamicConceptValue.EntityId
            };
        }

        public static void CreateThirdPartyLiability(Models.CompanyTplRisk tplRisk, Rules.Facade facade)
        {
            facade.SetConcept(CompanyRuleConceptRisk.RiskId, tplRisk.Risk.RiskId);

            if (tplRisk.Make == null)
            {
                tplRisk.Make = new CompanyMake();
            }

            tplRisk.Make.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleMakeCode);
            tplRisk.Year = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleYear);
            if (tplRisk.Version.Type == null)
            {
                tplRisk.Version.Type = new VEMOC.CompanyType();
            }
            tplRisk.Version.Type.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleTypeCode);
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleBodyCode) > 0)
            {
                if (tplRisk.Version.Body == null)
                {
                    tplRisk.Version.Body = new CompanyBody();
                }
                tplRisk.Version.Body.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleBodyCode);
            }
            tplRisk.LicensePlate = facade.GetConcept<string>(CompanyRuleConceptRisk.LicensePlate);
            tplRisk.EngineSerial = facade.GetConcept<string>(CompanyRuleConceptRisk.EngineSerialNumber);
            tplRisk.ChassisSerial = facade.GetConcept<string>(CompanyRuleConceptRisk.ChassisSerialNumber);
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleColorCode) > 0)
            {
                if (tplRisk.Color == null)
                {
                    tplRisk.Color = new CompanyColor();
                }
                tplRisk.Color.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleColorCode);
            }
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.PassengerQuantity) > 0)
            {
                tplRisk.PassengerQuantity = facade.GetConcept<int>(CompanyRuleConceptRisk.PassengerQuantity);
            }
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.FlatRatePercentage) > 0)
            {
                tplRisk.Rate = facade.GetConcept<int>(CompanyRuleConceptRisk.PassengerQuantity);
            }
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.DeductId) > 0)
            {
                if (tplRisk.Deductible == null)
                {
                    tplRisk.Deductible = new CompanyDeductible();
                }
                tplRisk.Deductible.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.DeductId);
            }
            if ((RateType)facade.GetConcept<int>(CompanyRuleConceptRisk.RateTypeCode) > 0)
            {
                tplRisk.Rate = facade.GetConcept<int>(CompanyRuleConceptRisk.RateTypeCode);
            }
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode) > 0)
            {
                tplRisk.Risk.LimitRc = new CompanyLimitRc
                {
                    Id = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode),
                    LimitSum = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum)
                };
            }

            tplRisk.TrailerQuantity = facade.GetConcept<int>(CompanyRuleConceptRisk.TrailersQuantity);
            tplRisk.Tons = facade.GetConcept<int>(CompanyRuleConceptRisk.TonsQty);
            //tplRisk.PhoneNumber = facade.GetConcept<string>(CompanyRuleConceptRisk.MobileNum);


        }

        public static CompanyCoverage CreateCoverage(CompanyCoverage coverage, Rules.Facade facade)
        {
            coverage.IsDeclarative = facade.GetConcept<bool>(RuleConceptCoverage.IsDeclarative);
            coverage.IsMinPremiumDeposit = facade.GetConcept<bool>(RuleConceptCoverage.IsMinimumPremiumDeposit);
            coverage.FirstRiskType = (FirstRiskType?)facade.GetConcept<int>(RuleConceptCoverage.FirstRiskTypeCode);
            coverage.CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType)facade.GetConcept<int>(RuleConceptCoverage.CalculationTypeCode);
            coverage.DeclaredAmount = facade.GetConcept<decimal>(RuleConceptCoverage.DeclaredAmount);
            coverage.PremiumAmount = facade.GetConcept<decimal>(RuleConceptCoverage.PremiumAmount);
            coverage.LimitAmount = facade.GetConcept<decimal>(RuleConceptCoverage.LimitAmount);
            coverage.SubLimitAmount = facade.GetConcept<decimal>(RuleConceptCoverage.SubLimitAmount);
            coverage.ExcessLimit = facade.GetConcept<decimal>(RuleConceptCoverage.LimitInExcess);
            coverage.LimitOccurrenceAmount = facade.GetConcept<decimal>(RuleConceptCoverage.LimitOccurrenceAmount);
            coverage.LimitClaimantAmount = facade.GetConcept<decimal>(RuleConceptCoverage.LimitClaimantAmount);
            coverage.AccumulatedLimitAmount = facade.GetConcept<decimal>(RuleConceptCoverage.AccumulatedLimitAmount);
            coverage.AccumulatedDeductAmount = facade.GetConcept<decimal>(RuleConceptCoverage.AccumulatedSubLimitAmount);
            coverage.CurrentFrom = facade.GetConcept<System.DateTime>(RuleConceptCoverage.CurrentFrom);
            coverage.RateType = (RateType?)facade.GetConcept<int>(RuleConceptCoverage.RateTypeCode);
            coverage.Rate = facade.GetConcept<decimal>(RuleConceptCoverage.Rate);
            coverage.CurrentTo = facade.GetConcept<System.DateTime>(RuleConceptCoverage.CurrentTo);
            coverage.MainCoverageId = facade.GetConcept<int>(RuleConceptCoverage.MainCoverageId);
            coverage.MainCoveragePercentage = facade.GetConcept<decimal>(RuleConceptCoverage.MainCoveragePercentage);
            coverage.CoverStatus = (CoverageStatusType?)facade.GetConcept<int>(RuleConceptCoverage.CoverageStatusCode);
            coverage.CoverageOriginalStatus = (CoverageStatusType?)facade.GetConcept<int>(RuleConceptCoverage.CoverageOriginalStatusCode);
            coverage.MaxLiabilityAmount = facade.GetConcept<decimal>(RuleConceptCoverage.MaxLiabilityAmount);
            coverage.MinimumPremiumCoverage = facade.GetConcept<decimal>(RuleConceptCoverage.MinimumPremiumCoverage);

            if (facade.GetConcept<int?>(RuleConceptCoverage.DeductId) > -2)
            {
                CreateCoverageDeductible(coverage, facade);
            }
            else {
                if ((facade.GetConcept<int>(RuleConceptCoverage.DeductId) == 0)
                  && (facade.GetConcept<decimal>(RuleConceptCoverage.DeductValue) > 0)
                  && (facade.GetConcept<decimal>(RuleConceptCoverage.MinDeductValue) > 0))
                {
                    CreateCoverageDeductible(coverage, facade);
                }
                else
                {
                    coverage.Deductible = null;
                }
            }

            //if (facade.GetConcept<int>(RuleConceptCoverage.DeductId) > 0)
            //{
            //    if (coverage.Deductible == null)
            //    {
            //        coverage.Deductible = new CompanyDeductible();
            //    }

            //    coverage.Deductible.Id = facade.GetConcept<int>(RuleConceptCoverage.DeductId);

            //    if ((RateType)facade.GetConcept<int>(RuleConceptCoverage.DeductRateTypeCode) > 0)
            //    {
            //        coverage.Deductible.RateType = (RateType)facade.GetConcept<int>(RuleConceptCoverage.DeductRateTypeCode);
            //    }

            //    coverage.Deductible.Rate = facade.GetConcept<decimal>(RuleConceptCoverage.DeductRate);
            //    coverage.Deductible.DeductPremiumAmount = facade.GetConcept<decimal>(RuleConceptCoverage.DeductPremiumAmount);
            //    coverage.Deductible.DeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.DeductValue);

            //    if (facade.GetConcept<int>(RuleConceptCoverage.DeductUnitCode) > 0)
            //    {
            //        coverage.Deductible.DeductibleUnit = new DeductibleUnit
            //        {
            //            Id = facade.GetConcept<int>(RuleConceptCoverage.DeductUnitCode)
            //        };
            //    }

            //    if (facade.GetConcept<int>(RuleConceptCoverage.DeductSubjectCode) > 0)
            //    {
            //        coverage.Deductible.DeductibleSubject = new DeductibleSubject
            //        {
            //            Id = facade.GetConcept<int>(RuleConceptCoverage.DeductSubjectCode)
            //        };
            //    }

            //    coverage.Deductible.MinDeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.MinDeductValue);

            //    if (facade.GetConcept<int>(RuleConceptCoverage.MinDeductUnitCode) > 0)
            //    {
            //        coverage.Deductible.MinDeductibleUnit = new DeductibleUnit
            //        {
            //            Id = facade.GetConcept<int>(RuleConceptCoverage.MinDeductUnitCode)
            //        };
            //    }

            //    if (facade.GetConcept<int>(RuleConceptCoverage.MinDeductSubjectCode) > 0)
            //    {
            //        coverage.Deductible.MinDeductibleSubject = new DeductibleSubject
            //        {
            //            Id = facade.GetConcept<int>(RuleConceptCoverage.MinDeductSubjectCode)
            //        };
            //    }

            //    coverage.Deductible.MaxDeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.MaxDeductValue);

            //    if (facade.GetConcept<int>(RuleConceptCoverage.MaxDeductUnitCode) > 0)
            //    {
            //        coverage.Deductible.MaxDeductibleUnit = new DeductibleUnit
            //        {
            //            Id = facade.GetConcept<int>(RuleConceptCoverage.MaxDeductUnitCode)
            //        };
            //    }

            //    if (facade.GetConcept<int>(RuleConceptCoverage.MaxDeductSubjectCode) > 0)
            //    {
            //        coverage.Deductible.MaxDeductibleSubject = new DeductibleSubject
            //        {
            //            Id = facade.GetConcept<int>(RuleConceptCoverage.MaxDeductSubjectCode)
            //        };
            //    }

            //    if (facade.GetConcept<int>(RuleConceptCoverage.CurrencyCode) > 0)
            //    {
            //        coverage.Deductible.Currency = new Currency
            //        {
            //            Id = facade.GetConcept<int>(RuleConceptCoverage.CurrencyCode)
            //        };
            //    }

            //    coverage.Deductible.AccDeductAmt = facade.GetConcept<decimal>(RuleConceptCoverage.AccDeductAmt);
            //}
            //else
            //{
            //    coverage.Deductible = null;
            //}

            coverage.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            coverage.MinimumPremiumCoverage = facade.GetConcept<decimal>(RuleConceptCoverage.MinimumPremiumCoverage);
            return coverage;
        }


        public static CompanyCoverage CreateCoverageDeductible(CompanyCoverage coverage, Rules.Facade facade)
        {
            if (coverage.Deductible == null)
            {
                coverage.Deductible = new CompanyDeductible();
            }

            coverage.Deductible.Id = facade.GetConcept<int>(RuleConceptCoverage.DeductId);

            if (facade.GetConcept<int>(RuleConceptCoverage.DeductRateTypeCode) > 0)
            {
                coverage.Deductible.RateType = (RateType)facade.GetConcept<int>(RuleConceptCoverage.DeductRateTypeCode);
            }

            coverage.Deductible.Rate = facade.GetConcept<decimal>(RuleConceptCoverage.DeductRate);
            coverage.Deductible.DeductPremiumAmount = facade.GetConcept<decimal>(RuleConceptCoverage.DeductPremiumAmount);

            if (facade.GetConcept<int>(RuleConceptCoverage.DeductUnitCode) > 0)
            {
                coverage.Deductible.DeductibleUnit = new DeductibleUnit
                {
                    Id = facade.GetConcept<int>(RuleConceptCoverage.DeductUnitCode)
                };
            }

            if (facade.GetConcept<int>(RuleConceptCoverage.DeductSubjectCode) > 0)
            {
                coverage.Deductible.DeductibleSubject = new DeductibleSubject
                {
                    Id = facade.GetConcept<int>(RuleConceptCoverage.DeductSubjectCode)
                };
            }
            coverage.Deductible.MinDeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.MinDeductValue);

            if (facade.GetConcept<int>(RuleConceptCoverage.MinDeductUnitCode) > 0)
            {
                coverage.Deductible.MinDeductibleUnit = new DeductibleUnit
                {
                    Id = facade.GetConcept<int>(RuleConceptCoverage.MinDeductUnitCode)
                };
            }

            if (facade.GetConcept<int>(RuleConceptCoverage.MinDeductSubjectCode) > 0)
            {
                coverage.Deductible.MinDeductibleSubject = new DeductibleSubject
                {
                    Id = facade.GetConcept<int>(RuleConceptCoverage.MinDeductSubjectCode)
                };
            }

            coverage.Deductible.MaxDeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.MaxDeductValue);

            if (facade.GetConcept<int>(RuleConceptCoverage.MaxDeductUnitCode) > 0)
            {
                coverage.Deductible.MaxDeductibleUnit = new DeductibleUnit
                {
                    Id = facade.GetConcept<int>(RuleConceptCoverage.MaxDeductUnitCode)
                };
            }

            if (facade.GetConcept<int>(RuleConceptCoverage.MaxDeductSubjectCode) > 0)
            {
                coverage.Deductible.MaxDeductibleSubject = new DeductibleSubject
                {
                    Id = facade.GetConcept<int>(RuleConceptCoverage.MaxDeductSubjectCode)
                };
            }

            if (facade.GetConcept<int>(RuleConceptCoverage.CurrencyCode) > 0)
            {
                coverage.Deductible.Currency = new Currency
                {
                    Id = facade.GetConcept<int>(RuleConceptCoverage.CurrencyCode)
                };
            }

            coverage.Deductible.AccDeductAmt = facade.GetConcept<decimal>(RuleConceptCoverage.AccDeductAmt);


            return coverage;
        }

            public static ServiceType CreateServiceType(COMM.ServiceType serviceType)
        {
            return new ServiceType
            {
                Id = serviceType.ServiceTypeCode,
                Description = serviceType.Description,
                SmallDescription = serviceType.SmallDescription,
                Enabled = serviceType.Enabled
            };
        }


        public static IMapper CreateMapBeneficiary()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyIssuanceInsured, CompanyBeneficiary>();
            });

            return config.CreateMapper();
        }

        /// <summary>
        /// Crear beneficiario
        /// </summary>
        /// <param name="riskModel"></param>
        /// <returns></returns>      
        #region auotmapper
        public static IMapper CreateMapBeneficiaryFromInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
                cfg.CreateMap<Address, Address>();
                cfg.CreateMap<Phone, Phone>();
                cfg.CreateMap<Email, Email>();
                cfg.CreateMap<CompanyIssuanceInsured, CompanyBeneficiary>()
            .ForMember(dest => dest.BeneficiaryType, ori => ori.MapFrom(scr => new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription }))
            .ForMember(dest => dest.Participation, ori => ori.MapFrom(scr => 100))
            .ForMember(dest => dest.BeneficiaryTypeDescription, ori => ori.MapFrom(scr => companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription));
            });
            return config.CreateMapper();
        }
        //public static CompanyBeneficiary CreateBeneficiaryFromInsured(CompanyInsured companyInsured)
        //{
        //    var companyBeneficiary = Mapper.Map<CompanyInsured, CompanyBeneficiary>(companyInsured);
        //    var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
        //    companyBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription };
        //    companyBeneficiary.BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription;
        //    return companyBeneficiary;
        //}

        #endregion
        #region Beneficiarios

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

        /// <summary>
        /// Crea un tipo de mercancia
        /// </summary>
        /// <param name="cargoType">tipo de mercancia</param>
        public static CargoType CreateCargoType(Trans.CargoType cargoType)
        {
            if (cargoType == null)
            {
                return null;
            }

            return new CargoType
            {
                Id = cargoType.Id,
                Description = cargoType.Description
            };
        }

        /// <summary>
        /// Crea una lista del tipo de mercancia
        /// </summary>
        /// <param name="cargoTypes">listado de tipos de mercancias</param>
        public static List<CargoType> CreateCargoTypes(List<Trans.CargoType> cargoTypes)
        {
            List<CargoType> cargoTypestpl = new List<CargoType>();

            foreach (var cargoType in cargoTypes)
            {
                cargoTypestpl.Add(CreateCargoType(cargoType));
            }

            return cargoTypestpl;
        }

        internal static DataTable SetDataTableTempRiskVehicle(CompanyTplRisk companyTplRisk)
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

            rows["VEHICLE_MAKE_CD"] = companyTplRisk.Make.Id;
            rows["VEHICLE_MODEL_CD"] = companyTplRisk.Model.Id;
            rows["VEHICLE_VERSION_CD"] = companyTplRisk.Version.Id;
            rows["VEHICLE_YEAR"] = companyTplRisk.Year;
            rows["VEHICLE_TYPE_CD"] = companyTplRisk.Version.Type.Id;
            rows["VEHICLE_USE_CD"] =  companyTplRisk.Use.Id;
            rows["VEHICLE_BODY_CD"] = companyTplRisk.Version.Body.Id;
            rows["VEHICLE_PRICE"] = 0;
            rows["IS_NEW"] = companyTplRisk.IsNew;
            rows["LICENSE_PLATE"] = companyTplRisk.LicensePlate;
            rows["ENGINE_SER_NO"] = companyTplRisk.EngineSerial;
            rows["CHASSIS_SER_NO"] = companyTplRisk.ChassisSerial;
            rows["VEHICLE_COLOR_CD"] = companyTplRisk.Color.Id;
            rows["LOAD_TYPE_CD"] = companyTplRisk.TypeCargoId;
            rows["TRAILERS_QTY"] = companyTplRisk.TrailerQuantity;
            rows["PASSENGER_QTY"] = companyTplRisk.PassengerQuantity;
            rows["NEW_VEHICLE_PRICE"] = companyTplRisk.NewPrice;
            rows["VEHICLE_FUEL_CD"] = companyTplRisk.Fuel.Id;
            rows["STD_VEHICLE_PRICE"] = companyTplRisk.StandardPrice;

            dataTable.Rows.Add(rows);
            return dataTable;
        }

        //internal static DataTable SetDataTableTempCiaRiskVehicle(CompanyTplRisk companyTplRisk)
        //{
        //    DataTable dataTable = new DataTable("INSERT_TEMP_CIA_RISK_VEHICLE");
        //    dataTable.Columns.Add("TEMP_ID", typeof(int));
        //    dataTable.Columns.Add("RISK_ID", typeof(int));
        //    dataTable.Columns.Add("POLICY_ID", typeof(int));
        //    dataTable.Columns.Add("ENDORSEMENT_ID", typeof(int));
        //    dataTable.Columns.Add("TRANSPORT_CARGO_TYPE_CD", typeof(int));
        //    dataTable.Columns.Add("GALLON_QTY", typeof(decimal));
        //    dataTable.Columns.Add("IS_TRANSFORM_VEHICLE", typeof(bool));
        //    dataTable.Columns.Add("YEAR_TRANSFORM_VEHICLE", typeof(int));

        //    DataRow rows = dataTable.NewRow();
        //    rows["TEMP_ID"] = companyTplRisk.Risk.Policy.Id;
        //    rows["RISK_ID"] = companyTplRisk.Risk.Id;
        //    rows["POLICY_ID"] = companyTplRisk.Risk.Policy.Id;
        //    rows["ENDORSEMENT_ID"] = companyTplRisk.Risk.Policy.Endorsement.Id;

        //    rows["TRANSPORT_CARGO_TYPE_CD"] = companyTplRisk.TypeCargoId;
        //    rows["GALLON_QTY"] = companyTplRisk.GallonTankCapacity;
        //    rows["IS_TRANSFORM_VEHICLE"] = companyTplRisk.RePoweredVehicle;
        //    rows["YEAR_TRANSFORM_VEHICLE"] = companyTplRisk.RepoweringYear;

        //    dataTable.Rows.Add(rows);
        //    return dataTable;
        //}

        public static DataTable GetDataTableTempRISK(CompanyTplRisk companyTplRisk)
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
            rows["OPERATION_ID"] = companyTplRisk.Risk.Id;
            rows["TEMP_ID"] = companyTplRisk.Risk.Policy.Endorsement.TemporalId;
            rows["INSURED_ID"] = companyTplRisk.Risk.MainInsured.IndividualId;
            rows["CUSTOMER_TYPE_CD"] = companyTplRisk.Risk.MainInsured.CustomerType;
            rows["COVERED_RISK_TYPE_CD"] = companyTplRisk.Risk.CoveredRiskType;
            rows["RISK_NUM"] = companyTplRisk.Risk.Number > 0 ? companyTplRisk.Risk.Number : 1;
            if (companyTplRisk.Risk.Policy.Endorsement.Id > 0)
            {
                rows["ENDORSEMENT_ID"] = companyTplRisk.Risk.Policy.Endorsement.Id;
            }
            rows["POLICY_ID"] = companyTplRisk.Risk.Policy.Endorsement.PolicyId > 0 ? companyTplRisk.Risk.Policy.Endorsement.PolicyId : rows["POLICY_ID"];
            rows["RISK_STATUS_CD"] = companyTplRisk.Risk.Status;
            if (companyTplRisk.Risk.OriginalStatus != null)
            {
                rows["RISK_ORIGINAL_STATUS_CD"] = companyTplRisk.Risk.OriginalStatus;
            }

            rows["RISK_INSP_TYPE_CD"] = 1;

            rows["INSPECTION_ID"] = DBNull.Value;
            if (companyTplRisk.Risk.Text != null)
            {
                rows["CONDITION_TEXT"] = companyTplRisk.Risk.Text.TextBody;
            }
            rows["RATING_ZONE_CD"] = companyTplRisk.Risk.RatingZone.Id;
            rows["COVER_GROUP_ID"] = companyTplRisk.Risk.GroupCoverage.Id;
            rows["PREFIX_CD"] = companyTplRisk.Risk.Policy.Prefix.Id;
            rows["IS_FACULTATIVE"] = companyTplRisk.Risk.IsFacultative;
            if (companyTplRisk.Risk.MainInsured.CompanyName != null)
            {
                if (companyTplRisk.Risk.MainInsured.CompanyName.NameNum > 0)
                {
                    rows["NAME_NUM"] = companyTplRisk.Risk.MainInsured.CompanyName.NameNum;
                }
                if (companyTplRisk.Risk.MainInsured.CompanyName.Address != null && companyTplRisk.Risk.MainInsured.CompanyName.Address.Id > 0)
                {
                    rows["ADDRESS_ID"] = companyTplRisk.Risk.MainInsured.CompanyName.Address.Id;
                }
                if (companyTplRisk.Risk.MainInsured.CompanyName.Phone != null && companyTplRisk.Risk.MainInsured.CompanyName.Phone.Id > 0)
                {
                    rows["PHONE_ID"] = companyTplRisk.Risk.MainInsured.CompanyName.Phone.Id;
                }
            }
            rows["RISK_COMMERCIAL_CLASS_CD"] = DBNull.Value;
            rows["RISK_COMMERCIAL_TYPE_CD"] = DBNull.Value;
            if (companyTplRisk.Risk.DynamicProperties != null && companyTplRisk.Risk.DynamicProperties?.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionPolicy = new DynamicPropertiesCollection();

                for (int i = 0; i < companyTplRisk.Risk.DynamicProperties?.Count; i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = companyTplRisk.Risk.DynamicProperties[i].Id;
                    dinamycProperty.Value = companyTplRisk.Risk.DynamicProperties[i].Value;
                    dynamicCollectionPolicy[i] = dinamycProperty;
                }

                rows["DYNAMIC_PROPERTIES"] = dynamicPropertiesSerializer.Serialize(dynamicCollectionPolicy);//--Serialize;
            }
            if (companyTplRisk.Risk.SecondInsured != null)
            {
                rows["SECONDARY_INSURED_ID"] = companyTplRisk.Risk.SecondInsured.IndividualId;
            }

            #endregion

            dataTable.Rows.Add(rows);
            #endregion
            return dataTable;
        }
        public static DataTable GetDataTableCOTempRisk(CompanyTplRisk companyTplRisk)
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

            rows["LIMITS_RC_CD"] = 80;
            rows["LIMIT_RC_SUM"] = 0;
            
            rows["100_RETENTION"] = companyTplRisk.Risk.IsRetention;
            rows["SINISTER_PCT"] = DBNull.Value;
            rows["HAS_SINISTER"] = companyTplRisk.Risk.HasSinister;
            if (companyTplRisk.Risk?.AssistanceType?.Id == null)
            {
                rows["ASSISTANCE_CD"] = DBNull.Value;
            }
            else
            {
                rows["ASSISTANCE_CD"] = companyTplRisk.Risk?.AssistanceType?.Id;
            }
            rows["SINISTER_QTY"] = DBNull.Value;
            rows["ACTUAL_DATE_MOVEMENT"] = (companyTplRisk.Risk.ActualDateMovement == DateTime.MinValue) ? DateTime.Now : companyTplRisk.Risk.ActualDateMovement;
            dataTable.Rows.Add(rows);
            #endregion

            #endregion
            return dataTable;
        }
        public static DataTable GetDataTableRiskVehicle(CompanyTplRisk companyTplRisk)
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

            rows["VEHICLE_MAKE_CD"] = companyTplRisk.Make.Id;
            rows["VEHICLE_MODEL_CD"] = companyTplRisk.Model.Id;
            rows["VEHICLE_VERSION_CD"] = companyTplRisk.Version.Id;
            rows["VEHICLE_YEAR"] = companyTplRisk.Year;
            rows["VEHICLE_TYPE_CD"] = companyTplRisk.Version.Type.Id;
            rows["VEHICLE_USE_CD"] =  companyTplRisk.Use.Id;
            rows["VEHICLE_BODY_CD"] = companyTplRisk.Version.Body.Id;
            rows["VEHICLE_PRICE"] = 0;
            rows["IS_NEW"] = companyTplRisk.IsNew;
            rows["LICENSE_PLATE"] = companyTplRisk.LicensePlate;
            rows["ENGINE_SER_NO"] = companyTplRisk.EngineSerial;
            rows["CHASSIS_SER_NO"] = companyTplRisk.ChassisSerial;
            rows["VEHICLE_COLOR_CD"] = companyTplRisk.Color.Id;
            rows["LOAD_TYPE_CD"] = DBNull.Value;
            rows["TRAILERS_QTY"] = companyTplRisk.TrailerQuantity;

            if (companyTplRisk.PassengerQuantity > 0)
            {
                rows["PASSENGER_QTY"] = companyTplRisk.PassengerQuantity;
            }
            rows["NEW_VEHICLE_PRICE"] = companyTplRisk.NewPrice;
            rows["VEHICLE_FUEL_CD"] = 0;
            rows["STD_VEHICLE_PRICE"] = 0;

            dataTable.Rows.Add(rows);
            return dataTable;
        }
        public static DataTable GetDataTableTemRiskVehicle(CompanyTplRisk companyTplRisk)
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

            rows["FLAT_RATE_PCT"] = companyTplRisk.Rate;
            rows["SHUTTLE_CD"] = DBNull.Value;
            rows["DEDUCT_ID"] = DBNull.Value;
            if (companyTplRisk.ServiceType != null && companyTplRisk.ServiceType.Id > 0)
            {
                rows["SERVICE_TYPE_CD"] = companyTplRisk.ServiceType.Id;
            }
            rows["MOBILE_NUM"] = companyTplRisk.PhoneNumber;
            if (companyTplRisk.Risk.Policy.Endorsement.Id > 0)
            {
                rows["ENDORSEMENT_ID"] = companyTplRisk.Risk.Policy.Endorsement.Id;
            }
            if (companyTplRisk.Risk.Policy.Endorsement.PolicyId > 0)
            {
                rows["POLICY_ID"] = companyTplRisk.Risk.Policy.Endorsement.PolicyId;
            }
            rows["TONS_QTY"] = companyTplRisk.Tons;
            rows["EXCESS"] = false;
            if (companyTplRisk.RateType.HasValue && companyTplRisk.RateType.Value > 0)
            {
                rows["RATE_TYPE_CD"] = companyTplRisk.RateType.Value;
            }
            rows["WORKER_TYPE"] = companyTplRisk.Risk.WorkerType.GetValueOrDefault();
            rows["BIRTH_DATE"] = DBNull.Value;
            rows["IS_NEW_RATE"] = false;
            if (companyTplRisk.Risk.MainInsured.BirthDate.GetValueOrDefault() != null && companyTplRisk.Risk.MainInsured.BirthDate.GetValueOrDefault() != DateTime.MinValue)
            {
                rows["FIRST_INSURED_BIRTH_DATE"] = companyTplRisk.Risk.MainInsured.BirthDate.GetValueOrDefault();
            }
            if (companyTplRisk.Risk.MainInsured.Gender != null)
            {
                rows["FIRST_INSURED_GENDER"] = companyTplRisk.Risk.MainInsured.Gender;
            }
            rows["GOOD_EXPERIENCE_NUM"] = DBNull.Value;
            rows["GOOD_EXP_NUM_RATE"] = DBNull.Value;


            dataTable.Rows.Add(rows);

            return dataTable;
        }

        public static EventAuthorization CreateCompanyEventAuthorizationEmision(CompanyPolicy companyPolicy, int userId)
        {
            EventAuthorization Event = new EventAuthorization();
            try
            {
                Event.OPERATION1_ID = companyPolicy.Endorsement.TicketNumber.ToString();
                Event.OPERATION2_ID = companyPolicy.Endorsement.Id.ToString();
                Event.AUTHO_USER_ID = userId;
                Event.EVENT_ID = Convert.ToInt32(UnderwritingServices.Enums.EventTypes.Subscription);
            }
            catch (Exception)
            {

            }
            return Event;
        }
        public static VEMO.Version CreateVersion(COMMEN.VehicleVersionYear vehicleVersionYear)
        {
            VEMO.Version versionModel = new VEMO.Version
            {
                Id = vehicleVersionYear.VehicleVersionCode,
                Model = new VEMO.Model
                {
                    Id = vehicleVersionYear.VehicleModelCode
                },
                Make = new VEMO.Make
                {
                    Id = vehicleVersionYear.VehicleMakeCode
                },
                NewVehiclePrice = vehicleVersionYear.VehiclePrice

            };

            return versionModel;
        }


    }
}
