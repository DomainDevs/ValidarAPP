using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Linq;
using Rules = Sistran.Core.Framework.Rules;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using COISSEN = Sistran.Company.Application.Issuance.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using UPMB = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Company.Application.Utilities.RulesEngine;
using System;
using Sistran.Company.Application.Locations.Models;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities;
using System.Data;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using Sistran.Co.Application.Data;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Business;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.DAOs;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Assemblers

{
    /// <summary>
    /// Constructor de modelos
    /// </summary>
    public class ModelAssembler
    {
        #region Property

        public static List<Models.CompanyRiskSubActivity> CreateRisksubActivities(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapSuBActivities();
            return mapper.Map<List<RiskCommercialType>, List<CompanyRiskSubActivity>>(businessCollection.Cast<RiskCommercialType>().ToList());

            //List<CompanyRiskSubActivity> riskSubActivities = new List<CompanyRiskSubActivity>();
            //foreach (RiskCommercialType item in businessCollection)
            //{
            //    riskSubActivities.Add(CreateSuBActivities(item));
            //}

            //return riskSubActivities;
        }

        public static CompanyPolicy CreateCompanyPolicy(ISSEN.Policy policy)
        {
            return new CompanyPolicy
            {
                Branch = new CompanyBranch
                {
                    Id = policy.BranchCode
                },
                Prefix = new CompanyPrefix
                {
                    Id = policy.PrefixCode
                },
                Product = new ProductServices.Models.CompanyProduct
                {
                    Id = (int)policy.ProductId
                },
                DocumentNumber = policy.DocumentNumber,
                CurrentFrom = policy.CurrentFrom,
                CurrentTo = (DateTime)policy.CurrentTo,
            };
        }

        public static CompanyRiskSubActivity CreateSuBActivities(RiskCommercialType subActivity)
        {
            var mapper = AutoMapperAssembler.CreateMapSuBActivities();
            return mapper.Map<RiskCommercialType, CompanyRiskSubActivity>(subActivity);

            //return new CompanyRiskSubActivity()
            //{
            //    Id = subActivity.RiskCommercialTypeCode,
            //    Description = subActivity.Description
            //};
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

        public static Models.CompanyPropertyRisk CreatePropertyRisk(ISSEN.Risk risk, ISSEN.RiskLocation riskLocation, ISSEN.EndorsementRisk endorsementRisk)
        {
            Models.CompanyPropertyRisk propertyRisk = new Models.CompanyPropertyRisk
            {
                Risk = new CompanyRisk
                {
                    RiskId = risk.RiskId,
                    Number = endorsementRisk.RiskNum,
                    CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
                    GroupCoverage = new UNMO.GroupCoverage
                    {
                        Id = risk.CoverGroupId.Value,
                        CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
                    },
                    MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
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
                    Description = riskLocation.Street,
                    RiskActivity = new CompanyRiskActivity
                    {
                        Id = riskLocation.CommRiskClassCode.GetValueOrDefault()
                    },
                    Policy = new CompanyPolicy()
                    {
                        Id = endorsementRisk.PolicyId,
                        Endorsement = new CompanyEndorsement()
                        {
                            Id = endorsementRisk.RiskId,
                        }
                    },
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    Status = RiskStatusType.NotModified,
                    DynamicProperties = new List<DynamicConcept>()
                },
                NomenclatureAddress = new CompanyNomenclatureAddress
                {
                    Type = new CompanyRouteType
                    {
                        Id = riskLocation.StreetTypeCode
                    }
                },
                FullAddress = riskLocation.Street,

                PML = riskLocation.EmlPercentage,
                Square = riskLocation.Block,
                FloorNumber = (int)riskLocation.FloorNumberEarthquake.GetValueOrDefault(),
                Latitude = riskLocation.LatitudeEarthquake,
                Longitude = riskLocation.LongitudeEarthquake,
                ConstructionType = new CompanyConstructionType
                {
                    Id = riskLocation.ConstructionCategoryCode
                },
                RiskUse = new CompanyRiskUse
                {
                    Id = (int)riskLocation.RiskUseCode.GetValueOrDefault()
                },
                ConstructionYear = (int)riskLocation.ConstructionYearEarthquake.GetValueOrDefault(),
                RiskAge = riskLocation.RiskAge.GetValueOrDefault(),

                RiskActivity = new CompanyRiskActivity
                {
                    Id = riskLocation.RiskCommercialTypeCode.GetValueOrDefault()
                },
                RiskType = new RiskType
                {
                    Id = riskLocation.RiskTypeCode.GetValueOrDefault()
                },

                IsDeclarative = riskLocation.IsRetention,
                City = new City
                {
                    Id = riskLocation.CityCode.GetValueOrDefault(),
                    State = new State
                    {
                        Id = riskLocation.StateCode,
                        Country = new Country
                        {
                            Id = riskLocation.CountryCode
                        }
                    }
                },
                BillingPeriodDepositPremium = Convert.ToInt32(riskLocation.PremiumAdjustmentPeriodCode),
                DeclarationPeriodCode = Convert.ToInt32(riskLocation.DeclarativePeriodCode),
                AdjustPeriod = new AdjustPeriod()
                {
                    Id = Convert.ToInt32(riskLocation.PremiumAdjustmentPeriodCode)
                },
                DeclarationPeriod = new DeclarationPeriod
                {
                    Id = Convert.ToInt32(riskLocation.DeclarativePeriodCode)
                }
            };

            if (risk.SecondaryInsuredId != null)
            {
                propertyRisk.Risk.SecondInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                {
                    IndividualId = risk.SecondaryInsuredId.Value
                };
            }
            if (risk.RatingZoneCode != null)
            {
                propertyRisk.Risk.RatingZone = new CompanyRatingZone
                {
                    Id = risk.RatingZoneCode.Value
                };
            }

            if (riskLocation.Apartment != null)
            {
                propertyRisk.NomenclatureAddress.ApartmentOrOffice = new CompanyApartmentOrOffice
                {
                    Id = int.Parse(riskLocation.Apartment)
                };
            }


            if (riskLocation.InsuranceModeCode != null)
            {
                propertyRisk.AssuranceMode = new CompanyAssuranceMode
                {
                    Id = Convert.ToInt32(riskLocation.InsuranceModeCode)
                };
            }

            if (riskLocation.RiskCommercialTypeCode != null)
            {
                propertyRisk.RiskSubActivity = new CompanyRiskSubActivity
                {
                    Id = Convert.ToInt32(riskLocation.RiskCommercialTypeCode)
                };
            }

            propertyRisk.Risk.IsFacultative = risk.IsFacultative;

            propertyRisk.Risk.IsRetention = riskLocation.IsRetention;


            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                propertyRisk.Risk.DynamicProperties.Add(dynamicConcept);
            }

            return propertyRisk;
        }

        public static Models.CompanyPropertyRisk CreatePropertyRisk(ISSEN.RiskLocation riskLocation, ISSEN.Policy policy)
        {
            Models.CompanyPropertyRisk propertyRisk = new Models.CompanyPropertyRisk
            {
                Risk = new CompanyRisk
                {
                    RiskId = riskLocation.RiskId,
                    Policy = new CompanyPolicy()
                    {
                        Id = policy.PolicyId,
                        Branch = new CompanyBranch
                        {
                            Id = policy.BranchCode
                        },
                        Prefix = new CompanyPrefix()
                        {
                            Id = policy.PrefixCode
                        },
                        DocumentNumber = policy.DocumentNumber
                    },
                    Status = RiskStatusType.NotModified,
                },
                FullAddress = riskLocation.Street,
                City = new City
                {
                    Id = riskLocation.CityCode.GetValueOrDefault(),
                    State = new State
                    {
                        Id = riskLocation.StateCode,
                        Country = new Country
                        {
                            Id = riskLocation.CountryCode
                        }
                    }
                }

            };

            return propertyRisk;
        }
        #endregion

        #region TemporalProperty

        public static Models.CompanyPropertyRisk CreateTemporalPropertyRisk(TMPEN.TempRisk tempRisk, TMPEN.CoTempRisk coTempRisk, TMPEN.TempRiskLocation tempRiskLocation)
        {
            Models.CompanyPropertyRisk model = new Models.CompanyPropertyRisk();

            model.Risk.Id = tempRisk.OperationId.GetValueOrDefault();
            model.Risk.RiskId = tempRisk.RiskId;
            model.Risk.CoveredRiskType = (CoveredRiskType)tempRisk.CoveredRiskTypeCode;
            model.Risk.Status = (RiskStatusType)tempRisk.RiskStatusCode;
            model.Risk.RatingZone = new CompanyRatingZone
            {
                Id = tempRisk.RatingZoneCode.Value
            };
            model.Risk.GroupCoverage = new UNMO.GroupCoverage
            {
                Id = tempRisk.CoverageGroupId.Value
            };

            model.NomenclatureAddress = new CompanyNomenclatureAddress
            {
                Type = new CompanyRouteType
                {
                    Id = tempRiskLocation.StreetTypeCode
                },
            };
            if (tempRiskLocation.Apartment != null)
            {
                model.NomenclatureAddress.ApartmentOrOffice = new CompanyApartmentOrOffice
                {
                    Id = int.Parse(tempRiskLocation.Apartment)
                };
            }
            model.FullAddress = tempRiskLocation.Street;
            model.PML = tempRiskLocation.EmlPercentage;
            model.Square = tempRiskLocation.Block;
            model.Risk.RiskActivity = new CompanyRiskActivity
            {
                Id = tempRiskLocation.CommRiskClassCode.GetValueOrDefault()
            };
            model.RiskType = new RiskType
            {
                Id = tempRiskLocation.RiskTypeCode.GetValueOrDefault()
            };
            model.ConstructionYear = (int)tempRiskLocation.ConstructionYearEarthquake.GetValueOrDefault();
            model.RiskAge = (int)tempRiskLocation.RiskAge.GetValueOrDefault();
            model.Latitude = tempRiskLocation.LatitudeEarthquake;
            model.Longitude = tempRiskLocation.LongitudeEarthquake;
            model.FloorNumber = (int)tempRiskLocation.FloorNumberEarthquake.GetValueOrDefault();
            model.IsDeclarative = tempRiskLocation.IsRetention;
            //model.DeclarationPeriod = Convert.ToInt32(tempRiskLocation.DeclarativePeriodCode);
            //model.BillingPeriodDepositPremium = Convert.ToInt32(tempRiskLocation.PremiumAdjustmentPeriodCode);
            model.City = new City
            {
                Id = tempRiskLocation.CityCode == null ? 0 : tempRiskLocation.CityCode.Value,
                State = new State { Id = tempRiskLocation.StateCode, Country = new Country { Id = tempRiskLocation.CountryCode } }
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

        public static CompanyBeneficiary CreateBeneficiary(ISSEN.RiskBeneficiary riskBeneficiary)
        {
            return new CompanyBeneficiary
            {
                IndividualId = riskBeneficiary.BeneficiaryId,
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

        public static CompanyBeneficiary CreateBeneficiaryFromInsured(CompanyIssuanceInsured companyInsured)
        {
            var imapper = ModelAssembler.CreateMapCompanyBeneficiary();
            var companyBeneficiary = imapper.Map<CompanyIssuanceInsured, CompanyBeneficiary>(companyInsured);
            var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
            companyBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.NotApplyBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.NotApplyBeneficiaryTypeId).SmallDescription };
            companyBeneficiary.BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.NotApplyBeneficiaryTypeId).SmallDescription;
            companyBeneficiary.Participation = 100;
            return companyBeneficiary;
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

        public static UNMO.Clause CreateTempRiskClause(TMPEN.TempRiskClause tempRiskClause)
        {
            return new UNMO.Clause
            {
                Id = tempRiskClause.ClauseId
            };
        }

        #endregion


        public static Models.CompanyPropertyRisk CreatePropertyPolicy(Models.CompanyPropertyRisk propertyPolicy, Rules.Facade FacadeGeneral) //Core.Application.Utilities.RulesEngine.Facades.FacadeGeneral FacadeGeneral)
        {
            return propertyPolicy;
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

        public static Models.CompanyPropertyRisk CreatePropertyRisk(Models.CompanyPropertyRisk propertyRisk, Rules.Facade facadeRiskProperty) //Entities.FacadeRiskProperty facadeRiskProperty)
        {
            Rules.Facade facade = new Rules.Facade();
            if (facadeRiskProperty.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode) > 0)
            {
                if (propertyRisk.Risk.RatingZone == null)
                {
                    propertyRisk.Risk.RatingZone = new CompanyRatingZone();
                }


                propertyRisk.Risk.RatingZone.Id = facadeRiskProperty.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode);
            }

            if (facadeRiskProperty.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode) > 0)
            {
                if (propertyRisk.Risk.GroupCoverage == null)
                {
                    propertyRisk.Risk.GroupCoverage = new GroupCoverage();
                }

                propertyRisk.Risk.GroupCoverage.Id = facadeRiskProperty.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            }

            if (facadeRiskProperty.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode) > 0)
            {
                if (propertyRisk.Risk.LimitRc == null)
                {
                    propertyRisk.Risk.LimitRc = new CompanyLimitRc();
                }

                propertyRisk.Risk.LimitRc.Id = facadeRiskProperty.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode);
            }

            if (facadeRiskProperty.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum) > 0)
            {
                if (propertyRisk.Risk.LimitRc == null)
                {
                    propertyRisk.Risk.LimitRc = new CompanyLimitRc();
                }

                propertyRisk.Risk.LimitRc.LimitSum = facadeRiskProperty.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum);
            }

            propertyRisk.Risk.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);

            return propertyRisk;
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
            coverage.InsuredObject.Amount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.InsuredObjectAmount);

            coverage.MaxLiabilityAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MaxLiabilityAmount);
            coverage.InsuredObject.BillingPeriodDepositPremium = facade.GetConcept<int>(CompanyRuleConceptCoverage.BillingPeriodDepositPremium);
            coverage.InsuredObject.DeclarationPeriod = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeclarationPeriod);

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

        public static List<CompanyPropertyRisk> CreatePropertyRisk(BusinessCollection businessCollection)
        {
            List<CompanyPropertyRisk> companyPropertyRisk = new List<CompanyPropertyRisk>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                companyPropertyRisk.Add(CreatePropertyRisk(entityEndorsementOperation));
            }

            return companyPropertyRisk;
        }

        public static CompanyPropertyRisk CreatePropertyRisk(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            CompanyPropertyRisk companyProperty = new CompanyPropertyRisk();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyProperty = COMUT.JsonHelper.DeserializeJson<CompanyPropertyRisk>(entityEndorsementOperation.Operation);
                companyProperty.Risk.Id = 0;
                companyProperty.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
                companyProperty.Risk.Coverages.AsParallel().ForAll(x => x.CoverageOriginalStatus = x.CoverStatus);

            }

            return companyProperty;
        }

        public static CompanyInsuredObject CreateCompanyInsuredObject(COISSEN.CoRiskInsuredObject insuredObjectEntity, ISSEN.RiskLocation insuredObjPeriod)
        {
            CompanyInsuredObject insuredObject = new CompanyInsuredObject();
            insuredObject.Id = insuredObjectEntity.InsuredObjectId;
            insuredObject.Amount = insuredObjectEntity.InsuredValue.GetValueOrDefault();
            insuredObject.BillingPeriodDepositPremium = Convert.ToInt32(insuredObjPeriod.PremiumAdjustmentPeriodCode);
            insuredObject.DeclarationPeriod = Convert.ToInt32(insuredObjPeriod.DeclarativePeriodCode);

            return insuredObject;
        }

        public static List<CompanyPropertyRisk> CreateProperties(BusinessCollection businessCollection)
        {
            List<CompanyPropertyRisk> companyProperty = new List<CompanyPropertyRisk>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                companyProperty.Add(ModelAssembler.CreateProperty(entityEndorsementOperation));
            }

            return companyProperty;
        }

        public static CompanyPropertyRisk CreateProperty(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            return COMUT.JsonHelper.DeserializeJson<CompanyPropertyRisk>(entityEndorsementOperation.Operation);
        }

        internal static PropertyRisk CreateCompanyPropertyRisk(CompanyPropertyRisk property)
        {
            var imapper = CreateMapPropertyRisk();
            return imapper.Map<CompanyPropertyRisk, PropertyRisk>(property);
        }

        #region Asegurado
        public static IMapper CreateMapPersonInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UPMB.BaseInsured, Core.Application.UnderwritingServices.Models.Base.BaseIssuanceInsured>();
                cfg.CreateMap<CiaPersonModel.CompanyInsured, CiaUnderwritingModel.CompanyIssuanceInsured>();

                cfg.CreateMap<IndividualPaymentMethod, CiaPersonModel.CiaIndividualPaymentMethod>();
                cfg.CreateMap<UPMB.BaseIndividualPaymentMethod, BaseIndividualPaymentMethod>();
                cfg.CreateMap<Sistran.Core.Application.UniquePersonService.V1.Models.EconomicActivity, BaseEconomicActivity>();
            });

            return config.CreateMapper();
        }
        #endregion Asegurado

        #region autommaper

        #region Asegurado
        public static IMapper CreateMapCompanyInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaUnderwritingModel.CompanyIssuanceInsured, CiaPersonModel.CompanyInsured>();
            });

            return config.CreateMapper();
        }
        public static IMapper CreateMapCompanyPersonInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaPersonModel.CompanyInsured, CompanyIssuanceInsured>();

                cfg.CreateMap<UPMB.BaseInsured, BaseInsured>();
                cfg.CreateMap<IndividualPaymentMethod, CiaPersonModel.CiaIndividualPaymentMethod>();
                cfg.CreateMap<UPMB.BaseIndividualPaymentMethod, BaseIndividualPaymentMethod>();
                cfg.CreateMap<Sistran.Core.Application.UniquePersonService.V1.Models.EconomicActivity, BaseEconomicActivity>();
            });
            return config.CreateMapper();
        }
        #endregion Asegurado
        #region Beneficiario
        public static IMapper CreateMapBeneficiary()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyIssuanceInsured, CompanyBeneficiary>();
                cfg.CreateMap<CompanyBeneficiary, Beneficiary>();
                cfg.CreateMap<CompanyBeneficiaryType, BeneficiaryType>();
            });

            return config.CreateMapper();
        }
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

        #endregion Beneficiario
        #region Risk
        public static IMapper CreateMapRiskActivity()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RiskActivity, CompanyRiskActivity>();
            });
            return config.CreateMapper();
        }
        #endregion Risk

        #region InsuredObject
        public static IMapper CreateMapInsuredObject()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
            });

            return config.CreateMapper();
        }
        public static IMapper CreateMapCompanyInsuredObjects()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<ISSEN.RiskInsuredObject, CompanyInsuredObject>();
            });

            return config.CreateMapper();
        }

        #endregion InsuredObject

        #region PropertyRisk
        public static IMapper CreateMapPropertyRisk()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyPropertyRisk, PropertyRisk>();
                cfg.CreateMap<CompanyRisk, Risk>();
                cfg.CreateMap<CompanyRatingZone, RatingZone>();
                cfg.CreateMap<CompanyPrefix, Prefix>();
                cfg.CreateMap<CompanyPrefixType, PrefixType>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanyBranch, Branch>();
                cfg.CreateMap<CompanySalesPoint, SalePoint>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyRiskActivity, RiskActivity>();
                cfg.CreateMap<CompanyLimitRc, LimitRc>();
                cfg.CreateMap<CompanyBeneficiary, Beneficiary>();
                cfg.CreateMap<CompanyBeneficiaryType, BeneficiaryType>();
                cfg.CreateMap<CompanyCoverage, Coverage>();
                cfg.CreateMap<CompanySubLineBusiness, SubLineBusiness>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanyText, Text>();
                cfg.CreateMap<CompanyDeductible, Deductible>();
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
                cfg.CreateMap<CompanyIssuanceInsured, IssuanceInsured>();
                cfg.CreateMap<CompanyPolicy, Policy>();
                cfg.CreateMap<CompanyPayerComponent, PayerComponent>();
                cfg.CreateMap<CompanyComponent, Component>();
                cfg.CreateMap<CompanySummary, Summary>();
                cfg.CreateMap<CompanyBillingGroup, BillingGroup>();
                cfg.CreateMap<CompanyPolicyType, PolicyType>();
                cfg.CreateMap<CompanyEndorsement, Endorsement>();
                cfg.CreateMap<CompanyPaymentPlan, PaymentPlan>();
                cfg.CreateMap<CompanyName, IssuanceCompanyName>();
                cfg.CreateMap<Address, IssuanceAddress>();
                cfg.CreateMap<Phone, IssuancePhone>();
                cfg.CreateMap<Email, IssuanceEmail>();
                cfg.CreateMap<IssuanceIdentificationDocument, IdentificationDocument>();
                cfg.CreateMap<IssuanceDocumentType, DocumentType>();
            });
            return config.CreateMapper();
        }

        #endregion PropertyRisk

        public static List<CompanyRiskLocation> CreateCompanyLocationsByRiskLocation(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyLocationsByRiskLocation();
            return mapper.Map<List<ISSEN.RiskLocation>, List<CompanyRiskLocation>>(businessCollection.Cast<ISSEN.RiskLocation>().ToList());

            //List<CompanyRiskLocation> riskLocation = new List<CompanyRiskLocation>();

            //foreach (ISSEN.RiskLocation entityRiskLocation in businessCollection)
            //{
            //    riskLocation.Add(CreateCompanyLocationsByRiskLocation(entityRiskLocation));
            //}

            //return riskLocation;
        }

        public static CompanyRiskLocation CreateCompanyLocationsByRiskLocation(ISSEN.RiskLocation riskLocation)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyLocationsByRiskLocation();
            return mapper.Map<ISSEN.RiskLocation, CompanyRiskLocation>(riskLocation);

            //CompanyRiskLocation location = new CompanyRiskLocation
            //{
            //    Risk = new Risk
            //    {
            //        RiskId = riskLocation.RiskId
            //    },
            //    Street = riskLocation.Street

            //};

            //return location;
        }

        public static List<CompanyRiskLocation> CreateClaimCompanyLocationsByRiskLocation(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapClaimCompanyLocationsByRiskLocation();
            return mapper.Map<List<ISSEN.RiskLocation>, List<CompanyRiskLocation>>(businessCollection.Cast<ISSEN.RiskLocation>().ToList());

            //List<CompanyRiskLocation> riskLocation = new List<CompanyRiskLocation>();

            //foreach (ISSEN.RiskLocation entityRiskLocation in businessCollection)
            //{
            //    riskLocation.Add(CreateClaimCompanyLocationsByRiskLocation(entityRiskLocation));
            //}

            //return riskLocation;
        }

        public static CompanyRiskLocation CreateClaimCompanyLocationsByRiskLocation(ISSEN.RiskLocation riskLocation)
        {
            var mapper = AutoMapperAssembler.CreateMapClaimCompanyLocationsByRiskLocation();
            return mapper.Map<ISSEN.RiskLocation, CompanyRiskLocation>(riskLocation);

            //CompanyRiskLocation location = new CompanyRiskLocation
            //{
            //    Risk = new Risk
            //    {
            //        RiskId = riskLocation.RiskId
            //    },
            //    Street = riskLocation.Street,
            //    City = new City
            //    {
            //        Id = riskLocation.CityCode.Value
            //    },
            //    Country = new Country
            //    {
            //        Id = riskLocation.CountryCode
            //    },
            //    State = new State
            //    {
            //        Id = riskLocation.StateCode
            //    }
            //};

            //return location;
        }

        #endregion autommaper

        internal static DataTable GetDataTableTempRiskProperty(CompanyPropertyRisk companyProperty)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_LOCATION ");

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

            if (companyProperty.PML != null)
            {
                rows["EML_PCT"] = companyProperty.PML.Value;
            }
            else
            {
                rows["EML_PCT"] = 0;
            }
            rows["ADDRESS_TYPE_CD"] = 1;//validar
            rows["STREET_TYPE_CD"] = 1;
            rows["STREET"] = companyProperty.FullAddress;
            rows["HOUSE_NUMBER"] = DBNull.Value;
            rows["FLOOR"] = companyProperty.FloorNumber.ToString();
            if (companyProperty.NomenclatureAddress?.ApartmentOrOffice != null)
            {
                rows["APARTMENT"] = (companyProperty.NomenclatureAddress.ApartmentOrOffice.Id).ToString();
            }
            else
            {
                rows["APARTMENT"] = 1.ToString();
            }
            rows["ZIP_CODE"] = string.Empty;
            rows["URBANIZATION"] = string.Empty;
            rows["CITY_CD"] = companyProperty.City.Id;
            rows["STATE_CD"] = companyProperty.City.State.Id;
            rows["COUNTRY_CD"] = companyProperty.City.State.Country.Id;
            rows["CRESTA_ZONE_CD"] = DBNull.Value; //validar
            rows["RISK_DANGEROUSNESS_CD"] = 1;
            if (companyProperty.ConstructionType != null && companyProperty.ConstructionType.Id > 0)
            {
                rows["CONSTRUCTION_CATEGORY_CD"] = companyProperty.ConstructionType.Id;
            }
            else
            {
                rows["CONSTRUCTION_CATEGORY_CD"] = 1;
            }
            rows["IS_MAIN"] = 0; //validar 
            rows["ECONOMIC_ACTIVITY_CD"] = companyProperty.Risk.RiskActivity.Id;// validar
            rows["HOUSING_TYPE_CD"] = DBNull.Value;//validar
            rows["OCCUPATION_TYPE_CD"] = DBNull.Value;
            rows["COMM_RISK_CLASS_CD"] = companyProperty.Risk.RiskActivity.Id;

            if (companyProperty.RiskSubActivity == null)
            {
                rows["RISK_COMMERCIAL_TYPE_CD"] = DBNull.Value;
            }
            else
            {
                rows["RISK_COMMERCIAL_TYPE_CD"] = companyProperty.RiskSubActivity.Id;
            }
            rows["RISK_COMM_SUBTYPE_CD"] = DBNull.Value;
            rows["ADDITIONAL_STREET"] = string.Format("-1|{0}||-1||||-1|-1||-1||1", companyProperty.FullAddress);
            if (companyProperty.Square != null)
            {
                rows["BLOCK"] = companyProperty.Square;
            }
            else
            {
                rows["BLOCK"] = DBNull.Value;
            }
            rows["LOCATION_CD"] = DBNull.Value; //validar
            if (companyProperty.RiskType != null && companyProperty.RiskType.Id != 0)
            {
                rows["RISK_TYPE_CD"] = (int)CoveredRiskType.Location;
            }
            else
            {
                rows["RISK_TYPE_CD"] = DBNull.Value;
            }
            rows["RISK_AGE"] = companyProperty.RiskAge;
            rows["IS_RETENTION"] = companyProperty.Risk.IsRetention;
            rows["INSPECTION_RECOMENDATION"] = false;
            rows["DECLARATIVE_PERIOD_CD"] = companyProperty.DeclarationPeriod.Id;
            rows["PREMIUM_ADJUSTMENT_PERIOD_CD"] = companyProperty.BillingPeriodDepositPremium;
            if (companyProperty.AssuranceMode != null)
            {
                rows["INSURANCE_MODE_CD"] = companyProperty.AssuranceMode.Id; //validar
            }
            else
            {
                rows["INSURANCE_MODE_CD"] = DBNull.Value; //validar
            }
            if (companyProperty.Longitude != null)
            {
                rows["LONGITUDE_EARTHQUAKE"] = companyProperty.Longitude;
            }
            else
            {
                rows["LONGITUDE_EARTHQUAKE"] = DBNull.Value;
            }
            if (companyProperty.Latitude != null)
            {
                rows["LATITUDE_EARTHQUAKE"] = companyProperty.Latitude;
            }
            else
            {
                rows["LATITUDE_EARTHQUAKE"] = DBNull.Value;
            }
            rows["CONSTRUCTION_YEAR_EARTHQUAKE"] = companyProperty.ConstructionYear;
            rows["FLOOR_NUMBER_EARTHQUAKE"] = companyProperty.FloorNumber;

            dataTable.Rows.Add(rows);
            return dataTable;
        }

        internal static DataTable GetDataTableTempRiskLocation(CompanyPropertyRisk companyProperty)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_LOCATION ");

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

            if (companyProperty.PML != null)
            {
                rows["EML_PCT"] = companyProperty.PML.Value;
            }
            else
            {
                rows["EML_PCT"] = 0;
            }
            rows["ADDRESS_TYPE_CD"] = 1;//validar
            rows["STREET_TYPE_CD"] = 1;
            rows["STREET"] = companyProperty.FullAddress;
            rows["HOUSE_NUMBER"] = DBNull.Value;
            rows["FLOOR"] = companyProperty.FloorNumber.ToString();
            if (companyProperty.NomenclatureAddress?.ApartmentOrOffice != null)
            {
                rows["APARTMENT"] = (companyProperty.NomenclatureAddress.ApartmentOrOffice.Id).ToString();
            }
            else
            {
                rows["APARTMENT"] = 1.ToString();
            }
            rows["ZIP_CODE"] = string.Empty;
            rows["URBANIZATION"] = string.Empty;
            rows["CITY_CD"] = companyProperty.City.Id;
            rows["STATE_CD"] = companyProperty.City.State.Id;
            rows["COUNTRY_CD"] = companyProperty.City.State.Country.Id;
            rows["CRESTA_ZONE_CD"] = DBNull.Value; //validar
            rows["RISK_DANGEROUSNESS_CD"] = 1;
            if (companyProperty.ConstructionType != null && companyProperty.ConstructionType.Id > 0)
            {
                rows["CONSTRUCTION_CATEGORY_CD"] = companyProperty.ConstructionType.Id;
            }
            else
            {
                rows["CONSTRUCTION_CATEGORY_CD"] = 1;
            }
            rows["IS_MAIN"] = 0; //validar 
            rows["ECONOMIC_ACTIVITY_CD"] = companyProperty.Risk.RiskActivity.Id;// validar
            rows["HOUSING_TYPE_CD"] = DBNull.Value;//validar
            rows["OCCUPATION_TYPE_CD"] = DBNull.Value;
            rows["COMM_RISK_CLASS_CD"] = companyProperty.Risk.RiskActivity.Id;

            if (companyProperty.RiskSubActivity == null)
            {
                rows["RISK_COMMERCIAL_TYPE_CD"] = DBNull.Value;
            }
            else
            {
                rows["RISK_COMMERCIAL_TYPE_CD"] = companyProperty.RiskSubActivity.Id;
            }
            rows["RISK_COMM_SUBTYPE_CD"] = DBNull.Value;

            rows["ADDITIONAL_STREET"] = string.Format("-1|{0}||-1||||-1|-1||-1||1", companyProperty.FullAddress);
            if (companyProperty.Square != null)
            {
                rows["BLOCK"] = companyProperty.Square;
            }
            else
            {
                rows["BLOCK"] = DBNull.Value;
            }
            rows["LOCATION_CD"] = DBNull.Value; //validar
            if (companyProperty.RiskType != null && companyProperty.RiskType.Id != 0)
            {
                rows["RISK_TYPE_CD"] = (int)CoveredRiskType.Location;
            }
            else
            {
                rows["RISK_TYPE_CD"] = DBNull.Value;
            }
            rows["RISK_AGE"] = companyProperty.RiskAge;
            rows["IS_RETENTION"] = companyProperty.Risk.IsRetention;
            rows["INSPECTION_RECOMENDATION"] = false;
            rows["DECLARATIVE_PERIOD_CD"] = companyProperty.DeclarationPeriodCode;
            rows["PREMIUM_ADJUSTMENT_PERIOD_CD"] = companyProperty.BillingPeriodDepositPremium;
            if (companyProperty.AssuranceMode != null)
            {
                rows["INSURANCE_MODE_CD"] = companyProperty.AssuranceMode.Id; //validar
            }
            else
            {
                rows["INSURANCE_MODE_CD"] = DBNull.Value; //validar
            }
            if (companyProperty.Longitude != null)
            {
                rows["LONGITUDE_EARTHQUAKE"] = companyProperty.Longitude;
            }
            else
            {
                rows["LONGITUDE_EARTHQUAKE"] = DBNull.Value;
            }
            if (companyProperty.Latitude != null)
            {
                rows["LATITUDE_EARTHQUAKE"] = companyProperty.Latitude;
            }
            else
            {
                rows["LATITUDE_EARTHQUAKE"] = DBNull.Value;
            }
            rows["CONSTRUCTION_YEAR_EARTHQUAKE"] = companyProperty.ConstructionYear;
            rows["FLOOR_NUMBER_EARTHQUAKE"] = companyProperty.FloorNumber;

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
                Event.AUTHO_USER_ID = companyPolicy.UserId;
                Event.EVENT_ID = (int)UnderwritingServices.Enums.EventTypes.Subscription;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Event;
        }

        internal static List<CompanyDeclarationPeriod> CreateDeclarationPeriodTypes(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapDeclarationPeriodType();
            return mapper.Map<List<PARAMEN.DeclarationPeriod>, List<CompanyDeclarationPeriod>>(businessCollection.Cast<PARAMEN.DeclarationPeriod>().ToList());

            //List<CompanyDeclarationPeriod> DeclarationPeriod = new List<CompanyDeclarationPeriod>();
            //foreach (PARAMEN.DeclarationPeriod entityDeclarationPriod in businessCollection)
            //{
            //    DeclarationPeriod.Add(CreateDeclarationPeriodType(entityDeclarationPriod));
            //}
            //return DeclarationPeriod;
        }

        private static CompanyDeclarationPeriod CreateDeclarationPeriodType(BusinessObject businessObject)
        {
            var mapper = AutoMapperAssembler.CreateMapDeclarationPeriodType();
            return mapper.Map<PARAMEN.DeclarationPeriod, CompanyDeclarationPeriod>((PARAMEN.DeclarationPeriod)businessObject);

            //PARAMEN.DeclarationPeriod entityDeclarationPriod = (PARAMEN.DeclarationPeriod)businessObject;
            //if (businessObject != null)
            //{
            //    return new CompanyDeclarationPeriod
            //    {
            //        Id = entityDeclarationPriod.DeclarationPeriodCode,
            //        Description = entityDeclarationPriod.Description,
            //        IsEnabled = entityDeclarationPriod.IsEnabled
            //    };
            //}
            //else
            //{
            //    return null;
            //}
        }

        internal static List<CompanyAdjustPeriod> CreateAdjustPeriods(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapAdjustPeriod();
            return mapper.Map<List<PARAMEN.BillingPeriod>, List<CompanyAdjustPeriod>>(businessCollection.Cast<PARAMEN.BillingPeriod>().ToList());

            //List<CompanyAdjustPeriod> adjustPeriods = new List<CompanyAdjustPeriod>();
            //foreach (PARAMEN.BillingPeriod entityAdjustPeriod in businessCollection)
            //{
            //    adjustPeriods.Add(CreateAdjustPeriod(entityAdjustPeriod));
            //}
            //return adjustPeriods;
        }

        private static CompanyAdjustPeriod CreateAdjustPeriod(BusinessObject businessObject)
        {
            var mapper = AutoMapperAssembler.CreateMapAdjustPeriod();
            return mapper.Map<PARAMEN.BillingPeriod, CompanyAdjustPeriod>((PARAMEN.BillingPeriod)businessObject);

            //PARAMEN.BillingPeriod entityAdjustPeriod = (PARAMEN.BillingPeriod)businessObject;
            //if (businessObject != null)
            //{
            //    return new CompanyAdjustPeriod
            //    {
            //        Id = entityAdjustPeriod.BillingPeriodCode,
            //        Description = entityAdjustPeriod.Description,
            //        IsEnabled = entityAdjustPeriod.IsEnabled
            //    };
            //}
            //else
            //{
            //    return null;
            //}
        }

        public static List<CompanyEndorsement> CreateCompanyEndorsements(List<ISSEN.Endorsement> entityEndorsements)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyEndorsement();
            return mapper.Map<List<ISSEN.Endorsement>, List<CompanyEndorsement>>(entityEndorsements);

            //List<CompanyEndorsement> companyEndorsements = new List<CompanyEndorsement>();
            //foreach (var entityEndorsement in entityEndorsements)
            //{
            //    companyEndorsements.Add(CreateCompanyEndorsement(entityEndorsement));
            //}
            //return companyEndorsements;
        }

        public static CompanyEndorsement CreateCompanyEndorsement(ISSEN.Endorsement entityEndorsement)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyEndorsement();
            return mapper.Map<ISSEN.Endorsement, CompanyEndorsement>(entityEndorsement);

            //CompanyEndorsement companyEndorsement = new CompanyEndorsement
            //{
            //    Id = entityEndorsement.EndorsementId,
            //    EndorsementType = (EndorsementType)entityEndorsement.EndoTypeCode,
            //    CurrentFrom = entityEndorsement.CurrentFrom,
            //    CurrentTo = entityEndorsement.CurrentTo ?? DateTime.Now,

            //};
            //return companyEndorsement;
        }

        public static CompanyEndorsementPeriod CreateCompanyEndorsementPeriod(CompanyPolicy policy, CompanyPropertyRisk propertyRisk, decimal policyId)
        {

            return new CompanyEndorsementPeriod()
            {
                PolicyId = policy.Endorsement.PolicyId,
                CurrentFrom = policy.CurrentFrom,
                CurrentTo = policy.CurrentTo,
                AdjustPeriod = propertyRisk.AdjustPeriod.Id,
                DeclarationPeriod = propertyRisk.DeclarationPeriod.Id,
                TotalAdjustment = 0,
                TotalDeclarations = 0,
                Version = 0
            };
        }
        public static List<CompanyEndorsementDetail> CreateCompanyEndorsementDetails(List<CompanyEndorsement> endorsements, List<CompanyPropertyRisk> propertyRisk, decimal policyId, CompanyEndorsementPeriod endorsementPeriod)
        {

            List<CompanyEndorsementDetail> endorsementDetails = new List<CompanyEndorsementDetail>();
            foreach (CompanyEndorsement item in endorsements)
            {
                CompanyPropertyRisk riskProperty = propertyRisk.Where(x => x.Risk.RiskId == item.RiskId).FirstOrDefault();
                if (riskProperty == null)
                {
                    riskProperty = propertyRisk.Where(x => x.Risk.Number == item.RiskId).FirstOrDefault();
                }
                CompanyInsuredObject insured = riskProperty.InsuredObjects.Where(x => x.Id == item.InsuredObjectId).FirstOrDefault();
                item.Number = riskProperty.Risk.Number;
                endorsementDetails.Add(CreateCompanyEndorsementDetail(riskProperty, item, insured, endorsementPeriod, policyId));
            }
            return endorsementDetails;
        }
        public static CompanyEndorsementDetail CreateCompanyEndorsementDetail(CompanyPropertyRisk propertyRisk, CompanyEndorsement item, CompanyInsuredObject insuredObject, CompanyEndorsementPeriod endorsementPeriod, decimal documentNumber)
        {
            switch (item.EndorsementType)
            {
                case EndorsementType.AdjustmentEndorsement:
                    PropertyDAO propertyDAO = new PropertyDAO();
                    CompanyEndorsementDetail endorsementDetail = new CompanyEndorsementDetail();
                    List<CompanyEndorsementDetail> detailsList = propertyDAO.GetEndorsementDetailsListByPolicyId(documentNumber, endorsementPeriod.Version);
                    decimal endorsementSum = (decimal)detailsList.Where(x => x.PolicyId == endorsementPeriod.PolicyId && x.RiskNum == item.Number && x.InsuredObjectId == insuredObject.Id && x.Version == endorsementPeriod.Version && x.EndorsementType == (int)EndorsementType.DeclarationEndorsement).Sum(n => n.DeclarationValue);
                    if (insuredObject.DepositPremiunPercent > 0)
                    {
                        endorsementDetail.PremiumAmount = (endorsementSum * insuredObject.Rate) - ((insuredObject.Amount * insuredObject.DepositPremiunPercent) * insuredObject.Rate);
                    }
                    else
                    {
                        endorsementDetail.PremiumAmount = 0;
                    }
                    endorsementDetail.DeclarationValue = 0; // item.DeclaredValue;
                    endorsementDetail.RiskNum = item.Number;
                    endorsementDetail.EndorsementType = (int)item.EndorsementType;
                    endorsementDetail.PolicyId = item.PolicyId;
                    endorsementDetail.InsuredObjectId = (int)item.InsuredObjectId;
                    endorsementDetail.EndorsementDate = DateTime.Now;
                    endorsementDetail.Version = endorsementPeriod.Version;
                    return endorsementDetail;
                    break;
                case EndorsementType.DeclarationEndorsement:

                    return new CompanyEndorsementDetail()
                    {
                        DeclarationValue = item.DeclaredValue,
                        RiskNum = item.Number,//(int)item.RiskId,
                        EndorsementType = (int)item.EndorsementType,
                        PolicyId = item.PolicyId,
                        InsuredObjectId = (int)item.InsuredObjectId,
                        //PremiumAmmount = (insuredObject.DepositPremiunPercent != 0) ? (item.DeclaredValue * (insuredObject.DepositPremiunPercent/100) * (insuredObject.Rate/100)) : (insuredObject.BillingPeriodDepositPremium * (insuredObject.Rate/100)),
                        PremiumAmount = (item.DeclaredValue * (insuredObject.Rate / 100)),
                        EndorsementDate = DateTime.Now,
                        Version = endorsementPeriod.Version
                    };
                    break;
            }
            return new CompanyEndorsementDetail();


        }

    }
}
