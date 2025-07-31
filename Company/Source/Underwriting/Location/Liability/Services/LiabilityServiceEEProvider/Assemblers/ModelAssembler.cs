using AutoMapper;
using Newtonsoft.Json;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Location.LiabilityServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Linq;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using CommonModels = Sistran.Core.Application.CommonService.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using LocationModels = Sistran.Core.Application.Locations.Models;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using UnderwritingModels = Sistran.Core.Application.UnderwritingServices.Models;
using UNMODEL = Sistran.Core.Application.UnderwritingServices.Models.Base;
using UPMB = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Rules = Sistran.Core.Framework.Rules;
using System;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Locations.Models;
using Sistran.Core.Application.Locations.Models;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Entities;
using System.Data;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider.Assemblers
{
    /// <summary>
    /// Constructor de modelos
    /// </summary>
    internal class ModelAssembler
    {
        internal static LiabilityRisk CreateLiabilityRisk(CompanyLiabilityRisk companyLiabilityRisk)
        {
            return Mapper.Map<CompanyLiabilityRisk, LiabilityRisk>(companyLiabilityRisk);
        }

        #region Liability
        /// <summary>
        /// Crear modelo de riesgo
        /// </summary>
        public static Models.CompanyLiabilityRisk CreateLiabilityRisk(ISSEN.Risk risk, ISSEN.RiskLocation riskLocation, ISSEN.EndorsementRisk endorsementRisk)
        {
            Models.CompanyLiabilityRisk liabilityRisk = new Models.CompanyLiabilityRisk
            {

                Risk = new CompanyRisk
                {
                    Description = riskLocation.Street,
                    RiskId = risk.RiskId,
                    CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                    GroupCoverage = new UnderwritingModels.GroupCoverage
                    {
                        Id = risk.CoverGroupId ?? 0,
                        CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                    },
                    Text = new CompanyText
                    {
                        TextBody = risk.ConditionText
                    },
                    Number = endorsementRisk.RiskNum,
                    RiskActivity = new CompanyRiskActivity
                    {
                        Id = riskLocation.CommRiskClassCode.GetValueOrDefault()
                    },
                    Policy = new CompanyPolicy()
                    {
                        Id = endorsementRisk.PolicyId
                    },
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    Status = RiskStatusType.NotModified,
                    IsFacultative = risk.IsFacultative,
                    IsRetention = riskLocation.IsRetention,
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
                 
                },
                NomenclatureAddress = new LocationModels.NomenclatureAddress
                {
                    Type = new LocationModels.RouteType
                    {
                        Id = riskLocation.StreetTypeCode
                    }
                },
                FullAddress = riskLocation.Street,
                PML = riskLocation.EmlPercentage,
                Square = riskLocation.Block,

                RiskType = new RiskType
                {
                    Id = riskLocation.RiskTypeCode.GetValueOrDefault()
                },
                RiskAge = riskLocation.RiskAge.GetValueOrDefault(),
                Latitude = riskLocation.LatitudeEarthquake,
                Longitude = riskLocation.LongitudeEarthquake,
                FloorNumber = (int)riskLocation.FloorNumberEarthquake.GetValueOrDefault(),
                ConstructionYear = (int)riskLocation.ConstructionYearEarthquake.GetValueOrDefault(),
                IsDeclarative = risk.IsFacultative,
                City = new CommonModels.City
                {
                    Id = riskLocation.CityCode.GetValueOrDefault(),
                    State = new CommonModels.State
                    {
                        Id = riskLocation.StateCode,
                        Country = new CommonModels.Country
                        {
                            Id = riskLocation.CountryCode
                        }
                    }
                }
            };

            if (risk.RatingZoneCode.HasValue)
            {
                liabilityRisk.Risk.RatingZone = new CompanyRatingZone
                {
                    Id = risk.RatingZoneCode.GetValueOrDefault()
                };
            }

            if (riskLocation.Apartment != null)
            {
                liabilityRisk.NomenclatureAddress.ApartmentOrOffice = new LocationModels.ApartmentOrOffice
                {
                    Id = int.Parse(riskLocation.Apartment)
                };
            }

            if (riskLocation.InsuranceModeCode != null)
            {
                liabilityRisk.AssuranceMode = new CompanyAssuranceMode
                {
                    Id = Convert.ToInt32(riskLocation.InsuranceModeCode)
                };
            }

            if (riskLocation.RiskCommercialTypeCode != null)
            {
                liabilityRisk.RiskSubActivity = new CompanyRiskSubActivity
                {
                    Id = Convert.ToInt32(riskLocation.RiskCommercialTypeCode)
                };
            }

            liabilityRisk.Risk.IsFacultative = risk.IsFacultative;

            liabilityRisk.Risk.IsRetention = riskLocation.IsRetention;

            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                liabilityRisk.Risk.DynamicProperties.Add(dynamicConcept);
            }          
            return liabilityRisk;


        }


        #endregion

        #region TemporalProperty
        /// <summary>
        /// Modelo de riesgo
        /// </summary>
        public static Models.CompanyLiabilityRisk CreateTemporalLiabilityRisk(TMPEN.TempRisk tempRisk, TMPEN.CoTempRisk coTempRisk, TMPEN.TempRiskLocation tempRiskLocation)
        {

            Models.CompanyLiabilityRisk model = new Models.CompanyLiabilityRisk
            {
                Risk =
                    new CompanyRisk
                    {
                        Id = tempRisk.OperationId.GetValueOrDefault(),
                        RiskId = tempRisk.RiskId,
                        CoveredRiskType = (CoveredRiskType)tempRisk.CoveredRiskTypeCode,
                        Status = (RiskStatusType)tempRisk.RiskStatusCode,
                        GroupCoverage = new GroupCoverage { Id = tempRisk.CoverageGroupId.Value },
                        Description = tempRiskLocation.Street,
                        IsRetention = tempRiskLocation.IsRetention,
                        RiskActivity = new CompanyRiskActivity { Id = tempRiskLocation.CommRiskClassCode.GetValueOrDefault() }
                    },
                NomenclatureAddress = new LocationModels.NomenclatureAddress
                {
                    Type = new LocationModels.RouteType { Id = tempRiskLocation.StreetTypeCode }
                },
                FullAddress = tempRiskLocation.Street,
                PML = tempRiskLocation.EmlPercentage,
                Square = tempRiskLocation.Block,

                RiskType = new RiskType { Id = tempRiskLocation.RiskTypeCode == null ? 0 : tempRiskLocation.RiskTypeCode.Value },
                ConstructionYear = (int)tempRiskLocation.ConstructionYearEarthquake.GetValueOrDefault(),
                RiskAge = (int)tempRiskLocation.RiskAge.GetValueOrDefault(),
                Latitude = tempRiskLocation.LatitudeEarthquake,
                Longitude = tempRiskLocation.LongitudeEarthquake,
                FloorNumber = (int)tempRiskLocation.FloorNumberEarthquake.GetValueOrDefault(),
                IsDeclarative = tempRiskLocation.IsRetention,
                City = new CommonModels.City
                {
                    Id = tempRiskLocation.CityCode == null ? 0 : tempRiskLocation.CityCode.Value,
                    State = new CommonModels.State { Id = tempRiskLocation.StateCode, Country = new CommonModels.Country { Id = tempRiskLocation.CountryCode } }
                }
            };

            if (tempRisk.RatingZoneCode != null)
            {
                model.Risk.RatingZone = new CompanyRatingZone
                {
                    Id = tempRisk.RatingZoneCode.Value
                };
            }

            if (tempRiskLocation.Apartment != null)
            {
                model.NomenclatureAddress.ApartmentOrOffice = new LocationModels.ApartmentOrOffice { Id = int.Parse(tempRiskLocation.Apartment) };
            }

            List<DynamicConcept> dynamicProperties = new List<DynamicConcept>();
            foreach (DynamicProperty item in tempRisk.DynamicProperties)
            {
                DynamicProperty itemDynamic = (DynamicProperty)item.Value;
                DynamicConcept dynamicProperty = new DynamicConcept();
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
                },
                AssociationType =new IssuanceAssociationType { Id =0}
            };
        }
        #endregion asegurado
        #region beneficiario
        public static IMapper CreateMapCompanyBeneficiary()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<BeneficiaryType, CompanyBeneficiaryType>();
                cfg.CreateMap<CompanyIssuanceInsured, CompanyBeneficiary>();
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// Crear modelo de beneficiario
        /// </summary>
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
        public static CompanyBeneficiary CreateCompanyBeneficiary(CompanyBeneficiary beneficiary)
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
        public static CompanyBeneficiary CreateBeneficiaryFromInsured(CompanyIssuanceInsured companyInsured)
        {
            var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
            return new CompanyBeneficiary
            {
                IndividualId = companyInsured.IndividualId,
                IdentificationDocument = companyInsured.IdentificationDocument,
                Name = companyInsured.Name,
                Participation = CommisionValue.Participation,
                CustomerType = companyInsured.CustomerType,
                CompanyName = companyInsured.CompanyName,
                IndividualType = companyInsured.IndividualType,
                BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription },
                BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription,
                AssociationType =new IssuanceAssociationType { Id = companyInsured.AssociationType.Id}
            };
        }

        #endregion

        #region TempRiskClause
        /// <summary>
        /// Crear modelo de Clusulas
        /// </summary>
        public static UnderwritingModels.Clause CreateTempRiskClause(TMPEN.TempRiskClause tempRiskClause)
        {
            return new UnderwritingModels.Clause
            {
                Id = tempRiskClause.ClauseId
            };
        }

        #endregion

        public static Models.CompanyLiabilityRisk CreateLiabilityPolicy(Models.CompanyLiabilityRisk companyLiabilityRisk, Rules.Facade facade)
        {

            companyLiabilityRisk.Risk.Policy.Endorsement.TemporalId = facade.GetConcept<int>(CompanyRuleConceptRisk.TempId);
            companyLiabilityRisk.Risk.RiskId = facade.GetConcept<int>(CompanyRuleConceptRisk.RiskId);
            companyLiabilityRisk.Risk.MainInsured.IndividualId = facade.GetConcept<int>(CompanyRuleConceptRisk.InsuredId);
            companyLiabilityRisk.Risk.MainInsured.CustomerType = facade.GetConcept<CustomerType>(CompanyRuleConceptRisk.CustomerTypeCode);
            companyLiabilityRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType = facade.GetConcept<CoveredRiskType>(CompanyRuleConceptRisk.CoveredRiskTypeCode);
            companyLiabilityRisk.Risk.Status = facade.GetConcept<RiskStatusType>(CompanyRuleConceptRisk.RiskStatusCode);
            companyLiabilityRisk.Risk.OriginalStatus = facade.GetConcept<RiskStatusType>(CompanyRuleConceptRisk.RiskOriginalStatusCode);
            companyLiabilityRisk.Risk.Text.TextBody = facade.GetConcept<string>(CompanyRuleConceptRisk.ConditionText);
            companyLiabilityRisk.Risk.RatingZone.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode);
            companyLiabilityRisk.Risk.GroupCoverage.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            companyLiabilityRisk.Risk.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.OperationId);
            companyLiabilityRisk.Risk.LimitRc.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode);
            companyLiabilityRisk.Risk.LimitRc.LimitSum = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum);
            companyLiabilityRisk.NomenclatureAddress.ApartmentOrOffice.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.Apartment);
            companyLiabilityRisk.City.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CityCode);
            companyLiabilityRisk.RiskAge = facade.GetConcept<int>(CompanyRuleConceptRisk.RiskAge);
            companyLiabilityRisk.PML = facade.GetConcept<int>(CompanyRuleConceptRisk.EmlPercentage);


            return companyLiabilityRisk;
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

        private static DynamicConcept CreateDynamicConcept(Rules.Concept concept)
        {
            return new DynamicConcept
            {
                Id = concept.Id,
                Value = concept.Value,
                EntityId = concept.EntityId
            };
        }

        public static List<Models.CompanyRiskSubActivity> CreateRisksubActivities(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapRisksubActivities();
            return mapper.Map<List<RiskCommercialType>, List<CompanyRiskSubActivity>>(businessCollection.Cast<RiskCommercialType>().ToList());

            //List<CompanyRiskSubActivity> riskSubActivities = new List<CompanyRiskSubActivity>();
            //foreach (RiskCommercialType item in businessCollection)
            //{
            //    riskSubActivities.Add(CreateSuBActivities(item));
            //}

            //return riskSubActivities;
        }

        public static List<Models.CompanyAssuranceMode> CreateAssunaceMode(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapAssuranceMode();
            return mapper.Map<List<InsuranceMode>, List<CompanyAssuranceMode>>(businessCollection.Cast<InsuranceMode>().ToList());

            //List<CompanyAssuranceMode> riskSubActivities = new List<CompanyAssuranceMode>();
            //foreach (InsuranceMode item in businessCollection)
            //{
            //    riskSubActivities.Add(CreateAssuranceMode(item));
            //}

            //return riskSubActivities;
        }

        public static CompanyRiskSubActivity CreateSuBActivities(RiskCommercialType subActivity)
        {
            var mapper = AutoMapperAssembler.CreateMapRisksubActivities();
            return mapper.Map<RiskCommercialType, CompanyRiskSubActivity>(subActivity);

            //return new CompanyRiskSubActivity()
            //{
            //    Id = subActivity.RiskCommercialTypeCode,
            //    Description = subActivity.Description
            //};
        }

        public static CompanyAssuranceMode CreateAssuranceMode(InsuranceMode insuranceMode)
        {
            var mapper = AutoMapperAssembler.CreateMapAssuranceMode();
            return mapper.Map<InsuranceMode, CompanyAssuranceMode>(insuranceMode);

            //return new CompanyAssuranceMode
            //{
            //    Id = insuranceMode.InsuranceModeCode,
            //    Description = insuranceMode.Description
            //};
        }

        public static Models.CompanyLiabilityRisk CreateLiabilityRisk(Models.CompanyLiabilityRisk liabilityRisk, Rules.Facade facade)
        {


            if (facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode) > 0)
            {
                if (liabilityRisk.Risk.RatingZone == null)
                {
                    liabilityRisk.Risk.RatingZone = new CompanyRatingZone();
                }

                liabilityRisk.Risk.RatingZone.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode);
            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId) > 0)
            {
                if (liabilityRisk.Risk.GroupCoverage == null)
                {
                    liabilityRisk.Risk.GroupCoverage = new UnderwritingModels.GroupCoverage();
                }

                liabilityRisk.Risk.GroupCoverage.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            }
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode) > 0)
            {
                if (liabilityRisk.Risk.LimitRc == null)
                {
                    liabilityRisk.Risk.LimitRc = new CompanyLimitRc();
                }

                liabilityRisk.Risk.LimitRc.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode);
            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum) > 0)
            {
                if (liabilityRisk.Risk.LimitRc == null)
                {
                    liabilityRisk.Risk.LimitRc = new CompanyLimitRc();
                }

                liabilityRisk.Risk.LimitRc.LimitSum = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum);
            }

           
            if (liabilityRisk.AssuranceMode != null)
            {
                liabilityRisk.AssuranceMode = new CompanyAssuranceMode
                {
                    Id = Convert.ToInt32(liabilityRisk.AssuranceMode.Id)
                };
            }

            if (liabilityRisk.RiskSubActivity != null)
            {
                liabilityRisk.RiskSubActivity = new CompanyRiskSubActivity
                {
                    Id = Convert.ToInt32(liabilityRisk.RiskSubActivity.Id)
                };
            }
            
            liabilityRisk.IsDeclarative = liabilityRisk.IsDeclarative;

            liabilityRisk.Risk.IsRetention = liabilityRisk.Risk.IsRetention;


            liabilityRisk.Risk.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            return liabilityRisk;
        }

        public static CompanyCoverage CreateCoverage(CompanyCoverage coverage, Rules.Facade facade)
        {
            coverage.IsDeclarative = facade.GetConcept<bool>(CompanyRuleConceptCoverage.IsDeclarative);
            coverage.IsMinPremiumDeposit = facade.GetConcept<bool>(CompanyRuleConceptCoverage.IsMinimumPremiumDeposit);
            coverage.FirstRiskType = (FirstRiskType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.FirstRiskTypeCode);
            coverage.CalculationType = (Sistran.Core.Services.UtilitiesServices.Enums.CalculationType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CalculationTypeCode);
            coverage.DeclaredAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.DeclaredAmount);
            coverage.PremiumAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.PremiumAmount);
            coverage.LimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitAmount);
            coverage.SubLimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.SubLimitAmount);
            coverage.ExcessLimit = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitInExcess);
            coverage.LimitOccurrenceAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitOccurrenceAmount);
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
            coverage.InsuredObject.Amount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.InsuredObjectAmount);
            //coverage.IsPrimary = facade.GetConcept<bool>(CompanyRuleConceptCoverage.IsPrimary);



            if (facade.GetConcept<int?>(CompanyRuleConceptCoverage.DeductId) > -2)
            {
                CreateCoverageDeductible(coverage, facade);
            }
            else
            {
                if ((facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductId) == 0)
                    && (facade.GetConcept<decimal>(CompanyRuleConceptCoverage.DeductValue) > 0)
                    && (facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MinDeductValue) > 0))
                {
                    CreateCoverageDeductible(coverage, facade);
                }
                else
                {
                    coverage.Deductible = null;
                }
            }

            coverage.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            coverage.MinimumPremiumCoverage = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MinimumPremiumCoverage);
            return coverage;
        }


        public static List<CompanyLiabilityRisk> CreateLiabilitiesRisk(BusinessCollection businessCollection)
        {
            List<CompanyLiabilityRisk> companyLiabilityRisk = new List<CompanyLiabilityRisk>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                companyLiabilityRisk.Add(CreateLiabilityRisk(entityEndorsementOperation));
            }

            return companyLiabilityRisk;
        }

        public static CompanyLiabilityRisk CreateLiabilityRisk(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            CompanyLiabilityRisk companyLiability = new CompanyLiabilityRisk();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyLiability = JsonConvert.DeserializeObject<CompanyLiabilityRisk>(entityEndorsementOperation.Operation);
                companyLiability.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
            }

            return companyLiability;
        }
        public static CompanyCoverage CreateCoverageDeductible(CompanyCoverage coverage, Rules.Facade facade)
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
                coverage.Deductible.Currency = new Currency
                {
                    Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.CurrencyCode)
                };
            }

            coverage.Deductible.AccDeductAmt = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccDeductAmt);

            return coverage;
        }

        #region autommaper
        
        #region risk
        

        internal static DataTable GetDataTableTempRiskLocation(CompanyLiabilityRisk companyLiability)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_LOCATION");

            dataTable.Columns.Add("EML_PCT", typeof(decimal));
            dataTable.Columns.Add("ADDRESS_TYPE_CD", typeof(int));
            dataTable.Columns.Add("STREET_TYPE_CD", typeof(int));
            dataTable.Columns.Add("STREET", typeof(string));
            dataTable.Columns.Add("HOUSE_NUMBER", typeof(int));
            dataTable.Columns.Add("FLOOR", typeof(string));
            dataTable.Columns.Add("APARTMENT", typeof(string));
            dataTable.Columns.Add("ZIP_CODE", typeof(string));
            dataTable.Columns.Add("URBANIZATION", typeof(string));
            dataTable.Columns.Add("CITY_CD", typeof(int));
            dataTable.Columns.Add("STATE_CD", typeof(int));
            dataTable.Columns.Add("COUNTRY_CD", typeof(int));
            dataTable.Columns.Add("CRESTA_ZONE_CD", typeof(int));
            dataTable.Columns.Add("RISK_DANGEROUSNESS_CD", typeof(int));
            dataTable.Columns.Add("CONSTRUCTION_CATEGORY_CD", typeof(int));
            dataTable.Columns.Add("IS_MAIN", typeof(bool));
            dataTable.Columns.Add("ECONOMIC_ACTIVITY_CD", typeof(int));
            dataTable.Columns.Add("HOUSING_TYPE_CD", typeof(int));
            dataTable.Columns.Add("OCCUPATION_TYPE_CD", typeof(int));
            dataTable.Columns.Add("COMM_RISK_CLASS_CD", typeof(int));
            dataTable.Columns.Add("RISK_COMMERCIAL_TYPE_CD", typeof(int));
            dataTable.Columns.Add("RISK_COMM_SUBTYPE_CD", typeof(int));
            dataTable.Columns.Add("ADDITIONAL_STREET", typeof(string));
            dataTable.Columns.Add("BLOCK", typeof(string));
            dataTable.Columns.Add("LOCATION_CD", typeof(int));
            dataTable.Columns.Add("RISK_TYPE_CD", typeof(int));
            dataTable.Columns.Add("RISK_AGE", typeof(int));
            dataTable.Columns.Add("IS_RETENTION", typeof(bool));
            dataTable.Columns.Add("INSPECTION_RECOMENDATION", typeof(bool));
            dataTable.Columns.Add("DECLARATIVE_PERIOD_CD", typeof(int));
            dataTable.Columns.Add("PREMIUM_ADJUSTMENT_PERIOD_CD", typeof(int));
            dataTable.Columns.Add("INSURANCE_MODE_CD", typeof(int));
            dataTable.Columns.Add("LONGITUDE_EARTHQUAKE", typeof(decimal));
            dataTable.Columns.Add("LATITUDE_EARTHQUAKE", typeof(decimal));
            dataTable.Columns.Add("CONSTRUCTION_YEAR_EARTHQUAKE", typeof(decimal));
            dataTable.Columns.Add("FLOOR_NUMBER_EARTHQUAKE", typeof(decimal));
            DataRow rows = dataTable.NewRow();

            if (companyLiability.PML != null)
            {
                rows["EML_PCT"] = companyLiability.PML.Value;
            }
            else
            {
                rows["EML_PCT"] = 0;
            }
            rows["ADDRESS_TYPE_CD"] = 1;//validar
            rows["STREET_TYPE_CD"] = 1;
            if (companyLiability.FullAddress != null)
            {
                rows["STREET"] = companyLiability.FullAddress;
            }
            else
            {
                throw new Exception(Errors.ErrorAddressEmpty);
            }
            
            rows["HOUSE_NUMBER"] = DBNull.Value;
            rows["FLOOR"] = companyLiability.FloorNumber.ToString();
            if (companyLiability.NomenclatureAddress?.ApartmentOrOffice != null)
            {
                rows["APARTMENT"] = (companyLiability.NomenclatureAddress.ApartmentOrOffice.Id).ToString();
            }
            else
            {
                rows["APARTMENT"] = 1.ToString();
            }
            rows["ZIP_CODE"] = string.Empty;
            rows["URBANIZATION"] = string.Empty;
            rows["CITY_CD"] = companyLiability.City.Id;
            rows["STATE_CD"] = companyLiability.City.State.Id;
            rows["COUNTRY_CD"] = companyLiability.City.State.Country.Id;
            rows["CRESTA_ZONE_CD"] = DBNull.Value; //validar
            rows["RISK_DANGEROUSNESS_CD"] = 1;
            if (companyLiability.ConstructionType != null && companyLiability.ConstructionType.Id > 0)
            {
                rows["CONSTRUCTION_CATEGORY_CD"] = companyLiability.ConstructionType.Id;
            }
            else
            {
                rows["CONSTRUCTION_CATEGORY_CD"] = 1;
            }
            rows["IS_MAIN"] = 0; //validar 
            if (companyLiability.Risk.RiskActivity == null)
            {
                rows["ECONOMIC_ACTIVITY_CD"] = DBNull.Value;
                rows["COMM_RISK_CLASS_CD"] = DBNull.Value;
            }
            else
            {
                rows["ECONOMIC_ACTIVITY_CD"] = companyLiability.Risk.RiskActivity.Id;
                rows["COMM_RISK_CLASS_CD"] = companyLiability.Risk.RiskActivity.Id;
            }
            rows["HOUSING_TYPE_CD"] = DBNull.Value;//validar
            rows["OCCUPATION_TYPE_CD"] = DBNull.Value;
            

            if (companyLiability.RiskSubActivity == null)
            {
                rows["RISK_COMMERCIAL_TYPE_CD"] = DBNull.Value;
            }
            else
            {
                rows["RISK_COMMERCIAL_TYPE_CD"] = companyLiability.RiskSubActivity.Id;
            }
            rows["RISK_COMM_SUBTYPE_CD"] = DBNull.Value;
            rows["ADDITIONAL_STREET"] = string.Format("-1|{0}||-1||||-1|-1||-1||1", companyLiability.FullAddress);
            if (companyLiability.Square != null)
            {
                rows["BLOCK"] = companyLiability.Square;
            }
            else
            {
                rows["BLOCK"] = DBNull.Value;
            }
            rows["LOCATION_CD"] = DBNull.Value; //validar
            if (companyLiability.RiskType != null && companyLiability.RiskType.Id != 0)
            {
                rows["RISK_TYPE_CD"] = (int)CoveredRiskType.Location;
            }
            else
            {
                rows["RISK_TYPE_CD"] = DBNull.Value;
            }
            rows["RISK_AGE"] = companyLiability.RiskAge;
            rows["IS_RETENTION"] = companyLiability.Risk.IsRetention;
            rows["INSPECTION_RECOMENDATION"] = false;
            rows["DECLARATIVE_PERIOD_CD"] = DBNull.Value; //validar
            rows["PREMIUM_ADJUSTMENT_PERIOD_CD"] = DBNull.Value; //validar
            if (companyLiability.AssuranceMode != null)
            {
                rows["INSURANCE_MODE_CD"] = companyLiability.AssuranceMode.Id; //validar
            }
            else
            {
                rows["INSURANCE_MODE_CD"] = DBNull.Value; //validar
            }
            if (companyLiability.Longitude != null)
            {
                rows["LONGITUDE_EARTHQUAKE"] = companyLiability.Longitude;
            }
            else
            {
                rows["LONGITUDE_EARTHQUAKE"] = DBNull.Value;
            }
            if (companyLiability.Latitude != null)
            {
                rows["LATITUDE_EARTHQUAKE"] = companyLiability.Latitude;
            }
            else
            {
                rows["LATITUDE_EARTHQUAKE"] = DBNull.Value;
            }
            rows["CONSTRUCTION_YEAR_EARTHQUAKE"] = companyLiability.ConstructionYear;
            rows["FLOOR_NUMBER_EARTHQUAKE"] = companyLiability.FloorNumber;

            dataTable.Rows.Add(rows);
            return dataTable;
        }
        #endregion risk

        #endregion autommaper

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
            catch (Exception ex)
            {

            }
            return Event;
        }

        #endregion
    }
}