using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Finances.FidelityServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Finances.FidelityServices.Models;
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
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using UnderwritingModels = Sistran.Core.Application.UnderwritingServices.Models;
using UNMODEL = Sistran.Core.Application.UnderwritingServices.Models.Base;
using UPMB = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Rules = Sistran.Core.Framework.Rules;
using System;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Temporary.Entities;
using Sistran.Core.Application.Utilities.Constants;
using System.Data;

namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.Assemblers
{
    /// <summary>
    /// Constructor de modelos
    /// </summary>
    internal class ModelAssembler
    {
        internal static FidelityRisk CreateFidelityRisk(CompanyFidelityRisk companyFidelityRisk)
        {
            return Mapper.Map<CompanyFidelityRisk, FidelityRisk>(companyFidelityRisk);
        }

        #region Fidelity
        /// <summary>
        /// Crear modelo de riesgo
        /// </summary>
        public static Models.CompanyFidelityRisk CreateFidelityRisk(ISSEN.Risk risk, ISSEN.RiskFidelity riskFidelity, ISSEN.EndorsementRisk endorsementRisk)
        {
            Models.CompanyFidelityRisk fidelityRisk = new Models.CompanyFidelityRisk
            {
                Risk = new CompanyRisk
                {
                    Description = riskFidelity.Description,
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
                        Id = riskFidelity.RiskCommercialClassCode
                    },
                    Policy = new CompanyPolicy()
                    {
                        Id = endorsementRisk.PolicyId
                    },
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    Status = RiskStatusType.NotModified,
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
                IsDeclarative = risk.IsPersisted,
                IdOccupation = riskFidelity.OccupationCode,
                DiscoveryDate = riskFidelity.DiscoveryDate,
                Description = riskFidelity.Description
            };

            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                fidelityRisk.Risk.DynamicProperties.Add(dynamicConcept);
            }
            return fidelityRisk;


        }


        #endregion

        #region TemporalProperty
        /// <summary>
        /// Modelo de riesgo
        /// </summary>
        public static Models.CompanyFidelityRisk CreateTemporalFidelityRisk(TMPEN.TempRisk tempRisk, TMPEN.CoTempRisk coTempRisk, TempRiskFidelity tempRiskFidelity)
        {

            Models.CompanyFidelityRisk model = new Models.CompanyFidelityRisk
            {
                Risk =
                    new CompanyRisk
                    {
                        Id = tempRisk.OperationId.GetValueOrDefault(),
                        RiskId = tempRisk.RiskId,
                        CoveredRiskType = (CoveredRiskType)tempRisk.CoveredRiskTypeCode,
                        Status = (RiskStatusType)tempRisk.RiskStatusCode,
                        GroupCoverage = new GroupCoverage { Id = tempRisk.CoverageGroupId.Value },
                        Description = tempRiskFidelity.Description,
                        RiskActivity = new CompanyRiskActivity { Id = tempRiskFidelity.RiskCommercialClassCode }
                    },
                //IsDeclarative = tempRiskFidelity.IsRetention,
            };

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
                IndividualType = riskBeneficiary.BeneficiaryTypeCode != 1 ? IndividualType.Company : IndividualType.Person,
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
        public static CompanyBeneficiary CreateCompanyBeneficiary(CompanyBeneficiary companyBeneficiary)
        {
            var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
            companyBeneficiary.BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription };
            companyBeneficiary.BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription;
            return companyBeneficiary;
        }
        public static CompanyBeneficiary CreateBeneficiaryFromInsured(CompanyIssuanceInsured insured)
        {
            var companyBeneficiaryType = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
            return new CompanyBeneficiary
            {
                IndividualId = insured.IndividualId,
                IndividualType = insured.IndividualType,
                IdentificationDocument = insured.IdentificationDocument,
                Name = insured.Name,
                Participation = CommisionValue.Participation,
                CustomerType = insured.CustomerType,
                CompanyName = insured.CompanyName,
                BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription },
                BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription
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

        #region Guardado en tabla temporales
        public static DataTable GetDataTableRiskFidelity(CompanyFidelityRisk companyJudgement)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_FIDELITY");
            dataTable.Columns.Add("RISK_COMMERCIAL_CLASS_CD", typeof(int));
            dataTable.Columns.Add("OCCUPATION_CD", typeof(int));
            dataTable.Columns.Add("DISCOVERY_DATE", typeof(DateTime));
            dataTable.Columns.Add("DESCRIPTION", typeof(string));
            DataRow rows = dataTable.NewRow();
            /*
              	TEMP_ID, RISK_ID, RISK_COMMERCIAL_CLASS_CD, OCCUPATION_CD, DISCOVERY_DATE, DESCRIPTION */
            if (companyJudgement.Risk != null)
            {
                rows["RISK_COMMERCIAL_CLASS_CD"] = companyJudgement.Risk.RiskActivity.Id;
            }
            else
            {
                rows["RISK_COMMERCIAL_CLASS_CD"] = DBNull.Value;
            }
            rows["OCCUPATION_CD"] = companyJudgement.IdOccupation;
            rows["DISCOVERY_DATE"] = companyJudgement.DiscoveryDate;
            rows["DESCRIPTION"] = companyJudgement.Description;
            dataTable.Rows.Add(rows);
            return dataTable;
        }

        #endregion

        public static Models.CompanyFidelityRisk CreateFidelityPolicy(Models.CompanyFidelityRisk companyFidelityRisk, Rules.Facade facade)
        {

            companyFidelityRisk.Risk.Policy.Endorsement.TemporalId = facade.GetConcept<int>(CompanyRuleConceptRisk.TempId);
            companyFidelityRisk.Risk.RiskId = facade.GetConcept<int>(CompanyRuleConceptRisk.RiskId);
            companyFidelityRisk.Risk.MainInsured.IndividualId = facade.GetConcept<int>(CompanyRuleConceptRisk.InsuredId);
            companyFidelityRisk.Risk.MainInsured.CustomerType = facade.GetConcept<CustomerType>(CompanyRuleConceptRisk.CustomerTypeCode);
            companyFidelityRisk.Risk.Policy.Product.CoveredRisk.CoveredRiskType = facade.GetConcept<CoveredRiskType>(CompanyRuleConceptRisk.CoveredRiskTypeCode);
            companyFidelityRisk.Risk.Status = facade.GetConcept<RiskStatusType>(CompanyRuleConceptRisk.RiskStatusCode);
            companyFidelityRisk.Risk.OriginalStatus = facade.GetConcept<RiskStatusType>(CompanyRuleConceptRisk.RiskOriginalStatusCode);
            companyFidelityRisk.Risk.Text.TextBody = facade.GetConcept<string>(CompanyRuleConceptRisk.ConditionText);
            companyFidelityRisk.Risk.RatingZone.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode);
            companyFidelityRisk.Risk.GroupCoverage.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            companyFidelityRisk.Risk.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.OperationId);
            companyFidelityRisk.Risk.LimitRc.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode);
            companyFidelityRisk.Risk.LimitRc.LimitSum = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum);
            /// jhgomez Creación de conceptos para Riesgo Manejo
            //companyFidelityRisk.IsDeclarative = facade.GetConcept<int>(CompanyRuleConceptRisk.IsDeclarative);


            return companyFidelityRisk;
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


        public static Models.CompanyFidelityRisk CreateFidelityRisk(Models.CompanyFidelityRisk fidelityRisk, Rules.Facade facade)
        {


            if (facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode) > 0)
            {
                if (fidelityRisk.Risk.RatingZone == null)
                {
                    fidelityRisk.Risk.RatingZone = new CompanyRatingZone();
                }

                fidelityRisk.Risk.RatingZone.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode);
            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId) > 0)
            {
                if (fidelityRisk.Risk.GroupCoverage == null)
                {
                    fidelityRisk.Risk.GroupCoverage = new UnderwritingModels.GroupCoverage();
                }

                fidelityRisk.Risk.GroupCoverage.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            }
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode) > 0)
            {
                if (fidelityRisk.Risk.LimitRc == null)
                {
                    fidelityRisk.Risk.LimitRc = new CompanyLimitRc();
                }

                fidelityRisk.Risk.LimitRc.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode);
            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum) > 0)
            {
                if (fidelityRisk.Risk.LimitRc == null)
                {
                    fidelityRisk.Risk.LimitRc = new CompanyLimitRc();
                }

                fidelityRisk.Risk.LimitRc.LimitSum = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum);
            }

            fidelityRisk.Risk.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            return fidelityRisk;
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

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductUnitCode) > 01)
                {
                    coverage.Deductible.DeductibleUnit = new UnderwritingModels.DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductUnitCode)
                    };
                }

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductSubjectCode) > 0)
                {
                    coverage.Deductible.DeductibleSubject = new UnderwritingModels.DeductibleSubject
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductSubjectCode)
                    };
                }

                coverage.Deductible.MinDeductValue = facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductValue);

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductUnitCode) > 0)
                {
                    coverage.Deductible.MinDeductibleUnit = new UnderwritingModels.DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductUnitCode)
                    };
                }

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductSubjectCode) > 0)
                {
                    coverage.Deductible.MinDeductibleSubject = new UnderwritingModels.DeductibleSubject
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductSubjectCode)
                    };
                }

                coverage.Deductible.MaxDeductValue = facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductValue);

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductUnitCode) > 0)
                {
                    coverage.Deductible.MaxDeductibleUnit = new UnderwritingModels.DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductUnitCode)
                    };
                }

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductSubjectCode) > 0)
                {
                    coverage.Deductible.MaxDeductibleSubject = new UnderwritingModels.DeductibleSubject
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
            else
            {
                coverage.Deductible = null;
            }

            //coverage.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facadeCoverage.GetDynamicConcepts());
            //coverage.MinimumPremiumCoverage = facadeCoverage.MinimumPremiumCoverage;
            coverage.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            coverage.MinimumPremiumCoverage = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MinimumPremiumCoverage);
            return coverage;
        }


        public static List<CompanyFidelityRisk> CreateFidelitiesRisk(BusinessCollection businessCollection)
        {
            List<CompanyFidelityRisk> companyFidelityRisk = new List<CompanyFidelityRisk>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                companyFidelityRisk.Add(CreateFidelityRisk(entityEndorsementOperation));
            }

            return companyFidelityRisk;
        }

        public static CompanyFidelityRisk CreateFidelityRisk(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            CompanyFidelityRisk companyFidelity = new CompanyFidelityRisk();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyFidelity = JsonConvert.DeserializeObject<CompanyFidelityRisk>(entityEndorsementOperation.Operation);
                companyFidelity.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
            }

            return companyFidelity;
        }

        #region autommaper
        #region asegurado
        public static IMapper CreateMapCompanyInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaUnderwritingModel.CompanyIssuanceInsured, CiaPersonModel.CompanyInsured>();
            });
            return config.CreateMapper();
        }
        public static IMapper CreateMapPersonInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UPMB.BaseInsured, UNMODEL.BaseIssuanceInsured>();
                cfg.CreateMap<IndividualPaymentMethod, CiaPersonModel.CiaIndividualPaymentMethod>();
                cfg.CreateMap<UPMB.BaseIndividualPaymentMethod, UPMB.BaseIndividualPaymentMethod>();
                cfg.CreateMap<EconomicActivity, UPMB.BaseEconomicActivity>();
            });
            return config.CreateMapper();
        }


        public static IMapper CreateMapPolicy()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyPolicy, CompanyPolicy>();
            });
            return config.CreateMapper();
        }

        #endregion asegurado
        #region beneficiario
        public static IMapper CreateMapCompanyBeneficiary()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyIssuanceInsured, CompanyBeneficiary>();
            });
            return config.CreateMapper();
        }
        #endregion beneficiario
        #endregion autommaper
    }
}