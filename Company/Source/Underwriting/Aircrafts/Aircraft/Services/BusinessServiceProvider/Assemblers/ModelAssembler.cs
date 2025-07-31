using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region PolicyType
        public static List<CompanyAircraft> CreateCompanyAircrafts(BusinessCollection businessCollection)
        {
            List<CompanyAircraft> companyAircraft = new List<CompanyAircraft>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                companyAircraft.Add(CreateCompanyAircraft(entityEndorsementOperation));
            }

            return companyAircraft;
        }

        public static CompanyAircraft CreateCompanyAircraft(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            CompanyAircraft companyAircraft = new CompanyAircraft();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyAircraft = JsonConvert.DeserializeObject<CompanyAircraft>(entityEndorsementOperation.Operation);
                companyAircraft.Risk.Id = 0;
                companyAircraft.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
                companyAircraft.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
            }

            return companyAircraft;
        }


        public static PolicyType CreatePolicyType(COMMEN.CoPolicyType entityPolicyType)
        {
            return new PolicyType
            {
                Id = entityPolicyType.PolicyTypeCode,
                Description = entityPolicyType.Description,
                IsFloating = entityPolicyType.Floating,
                Prefix = new Prefix
                {
                    Id = entityPolicyType.PrefixCode
                }
            };
        }
        #endregion

        /// <summary>
        /// Crea la lista de coberturas
        /// </summary>
        /// <param name="coverages">Coverage</param>
        /// <returns></returns>
        public static List<CompanyCoverage> CreateCompanyCoverages(List<Coverage> coverages)
        {
            List<CompanyCoverage> companyCoverages = new List<CompanyCoverage>();
            foreach (var coverage in coverages)
            {
                companyCoverages.Add(CreateCompanyCoverage(coverage));
            }
            return companyCoverages;
        }

        /// <summary>
        /// Crea una cobertura
        /// </summary>
        /// <param name="Coverages"></param>
        /// <returns></returns>
        public static CompanyCoverage CreateCompanyCoverage(Coverage Coverage)
        {
            if (Coverage == null)
                return null;

            return new CompanyCoverage
            {
                CalculationType = Coverage.CalculationType,
                CurrentFrom = Coverage.CurrentFrom,
                CurrentTo = Coverage.CurrentTo,
                Days = Coverage.Days,
                DeclaredAmount = Coverage.DeclaredAmount,
                Id = Coverage.Id,
                IsDeclarative = Coverage.IsDeclarative,
                LimitAmount = Coverage.LimitAmount,
                LimitClaimantAmount = Coverage.LimitClaimantAmount,
                LimitOccurrenceAmount = Coverage.LimitOccurrenceAmount,
                MaxLiabilityAmount = Coverage.MaxLiabilityAmount,
                PremiumAmount = Coverage.PremiumAmount,
                Rate = Coverage.Rate,
                RateType = Coverage.RateType,
                SubLimitAmount = Coverage.SubLimitAmount,
                Description = Coverage.Description
            };
        }

        /// <summary>
        /// Crea las coverturas de Aircrafte
        /// </summary>
        /// <param name="coverage"></param>
        /// <param name="facade"></param>
        /// <returns></returns>
        public static CompanyCoverage CreateCompanyCoverage(CompanyCoverage coverage, Rules.Facade facade)
        {
            coverage.InsuredObject.Id = facade.GetConcept<int>(CompanyRuleConceptCoverage.InsuredObjectId);
            coverage.RiskCoverageId = facade.GetConcept<int>(CompanyRuleConceptCoverage.RiskCoverageId);
            coverage.IsDeclarative = facade.GetConcept<bool>(CompanyRuleConceptCoverage.IsDeclarative);
            coverage.IsMinPremiumDeposit = facade.GetConcept<bool>(CompanyRuleConceptCoverage.IsMinimumPremiumDeposit);
            coverage.FirstRiskType = (FirstRiskType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.FirstRiskTypeCode);
            coverage.CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CalculationTypeCode);
            coverage.DeclaredAmount = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeclaredAmount);
            coverage.PremiumAmount = facade.GetConcept<int>(CompanyRuleConceptCoverage.PremiumAmount);
            coverage.LimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitAmount);
            coverage.SubLimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.SubLimitAmount);
            coverage.ExcessLimit = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitInExcess);
            coverage.LimitOccurrenceAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitOccurrenceAmount); ;
            coverage.LimitClaimantAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitClaimantAmount);
            coverage.AccumulatedLimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccumulatedLimitAmount);
            coverage.AccumulatedDeductAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccumulatedSubLimitAmount);
            coverage.RateType = (RateType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.RateTypeCode);
            coverage.Rate = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.Rate);
            coverage.MainCoverageId = facade.GetConcept<int>(CompanyRuleConceptCoverage.MainCoverageId);
            coverage.MainCoveragePercentage = facade.GetConcept<int>(CompanyRuleConceptCoverage.MainCoveragePercentage);
            coverage.ShortTermPercentage = facade.GetConcept<int>(CompanyRuleConceptCoverage.ShortTermPercentage);
            coverage.CurrentFrom = facade.GetConcept<DateTime>(CompanyRuleConceptCoverage.CurrentFrom);
            coverage.CurrentTo = facade.GetConcept<DateTime>(CompanyRuleConceptCoverage.CurrentTo);
            coverage.CoverStatus = (CoverageStatusType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CoverageStatusCode);
            coverage.CoverageOriginalStatus = (CoverageStatusType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CoverageOriginalStatusCode);

            coverage.MaxLiabilityAmount = facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxLiabilityAmount);


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
                coverage.Deductible.Rate = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductRate);
                coverage.Deductible.DeductPremiumAmount = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductPremiumAmount);
                coverage.Deductible.DeductValue = facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductValue);
                coverage.EndorsementId = facade.GetConcept<int>(CompanyRuleConceptCoverage.EndorsementId);
                coverage.IsAccMinPremium = facade.GetConcept<Boolean>(CompanyRuleConceptCoverage.IsEnabledMinimumPremium);

                if (facade.GetConcept<int>(CompanyRuleConceptCoverage.DeductUnitCode) > 0)
                {
                    coverage.Deductible.DeductibleUnit = new Core.Application.UnderwritingServices.Models.DeductibleUnit
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
                coverage.Deductible.MinDeductValue = facade.GetConcept<int>(CompanyRuleConceptCoverage.MinDeductValue);


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

                coverage.Deductible.MaxDeductValue = facade.GetConcept<int>(CompanyRuleConceptCoverage.MaxDeductValue);

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
                coverage.Deductible.AccDeductAmt = facade.GetConcept<int>(CompanyRuleConceptCoverage.AccDeductAmt);
            }
            else
            {
                coverage.Deductible = null;
            }
            coverage.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            coverage.MinimumPremiumCoverage = facade.GetConcept<int>(CompanyRuleConceptCoverage.MinimumPremiumCoverage);
            return coverage;
        }

        /// <summary>
        /// Crea una lista de conceptos dinámicos
        /// </summary>
        /// <param name="facade"></param>
        /// <returns></returns>
        public static List<DynamicConcept> CreateDynamicConcepts(Rules.Facade facade)
        {
            List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

            foreach (Rules.Concept concept in facade.Concepts.Where(x => x.IsStatic == false))
            {
                dynamicConcepts.Add(CreateDynamicConcept(concept));
            }

            return dynamicConcepts;
        }

        /// <summary>
        /// Crea un conceptos dinámico
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        private static DynamicConcept CreateDynamicConcept(Rules.Concept concept)
        {
            return new DynamicConcept
            {
                Id = concept.Id,
                Value = concept.Value,
                EntityId = concept.EntityId
            };
        }

        /// <summary>
        /// Crea el Aircrafte
        /// </summary>
        /// <param name="Aircraft"></param>
        /// <param name="facade"></param>
        /// <returns></returns>
        public static CompanyAircraft CreateCompanyAircraft(CompanyAircraft Aircraft, Rules.Facade facade)
        {
            Aircraft.Risk.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.RiskId);
            Aircraft.Risk.GroupCoverage.Coverage.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            Aircraft.Risk.Policy.Holder.IndividualId = facade.GetConcept<int>(CompanyRuleConceptRisk.IndividualId);

            return Aircraft;
        }

        /// <summary>
        /// Convierte un modelo Deductible a un modelo deducible de la capa compañía
        /// </summary>
        /// <param name="deductible">Modelo Deductible</param>
        /// <returns>Deducible</returns>
        internal static CompanyDeductible CreateCompanyDeducible(Deductible deductible)
        {
            if (deductible == null)
                return null;

            return new CompanyDeductible
            {
                Id = deductible.Id,
                Description = deductible.Description
            };
        }

        /// <summary>
        /// Convierte un listado de modelos Deductible a un conjunto de deducibles de la capa compañía
        /// </summary>
        /// <param name="deductibles">Listado de modelo Deductible</param>
        /// <returns>Listado de deducibles</returns>
        internal static List<CompanyDeductible> CreateCompanyDeducibles(List<Deductible> deductibles)
        {
            List<CompanyDeductible> companyDeductibles = new List<CompanyDeductible>();

            foreach (var deductible in deductibles)
            {
                companyDeductibles.Add(CreateCompanyDeducible(deductible));
            }
            return companyDeductibles;
        }
        public static CompanyAircraft CreateAircraft(ISSEN.Risk risk, ISSEN.RiskAircraft riskAircraft, ISSEN.EndorsementRisk endorsementRisk)
        {
            CompanyAircraft Aircraft = new CompanyAircraft
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

                    Status = RiskStatusType.NotModified,
                    OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                    DynamicProperties = new List<DynamicConcept>()
                },
            };

            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                Aircraft.Risk.DynamicProperties.Add(dynamicConcept);
            }

            return Aircraft;
        }
       
        public static IMapper CreateMapCompanyBeneficiary()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<BeneficiaryType, CompanyBeneficiaryType>();
            });
            return config.CreateMapper();

        }

        public static CompanyEndorsement CreateCompanyEndorsement(ISSEN.Endorsement entityEndorsement)
        {
            CompanyEndorsement companyEndorsement = new CompanyEndorsement
            {
                Id = entityEndorsement.EndorsementId,
                EndorsementType = (EndorsementType)entityEndorsement.EndoTypeCode,
                CurrentFrom = entityEndorsement.CurrentFrom,
                CurrentTo = entityEndorsement.CurrentTo ?? DateTime.Now,

            };
            return companyEndorsement;
        }

        public static List<CompanyEndorsement> CreateCompanyEndorsements(List<ISSEN.Endorsement> entityEndorsements)
        {
            List<CompanyEndorsement> companyEndorsements = new List<CompanyEndorsement>();
            foreach (var entityEndorsement in entityEndorsements)
            {
                companyEndorsements.Add(CreateCompanyEndorsement(entityEndorsement));
            }
            return companyEndorsements;
        }

        public static List<CompanyAircraft> CreateAircrafts(BusinessCollection businessCollection)
        {
            List<CompanyAircraft> companyAircrafts = new List<CompanyAircraft>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                companyAircrafts.Add(CreateAircraft(entityEndorsementOperation));
            }

            return companyAircrafts;
        }

        private static CompanyAircraft CreateAircraft(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            CompanyAircraft companyAircraft = new CompanyAircraft();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyAircraft = JsonConvert.DeserializeObject<CompanyAircraft>(entityEndorsementOperation.Operation);
                companyAircraft.Risk.Id = 0;
                companyAircraft.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
                if (companyAircraft.Risk.Coverages != null)
                {
                    companyAircraft.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
                }
            }
            return companyAircraft;
        }

        public static DataTable GetDataTableRiskAirCraft(CompanyAircraft companyAircraft)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_AIRCRAFT");

            dataTable.Columns.Add("AIRCRAFT_TYPE_CD", typeof(int));
            dataTable.Columns.Add("AIRCRAFT_MAKE_CD", typeof(int));
            dataTable.Columns.Add("AIRCRAFT_MODEL_CD", typeof(int));
            dataTable.Columns.Add("AIRCRAFT_REGISTER_CD", typeof(int));
            dataTable.Columns.Add("AIRCRAFT_TERRITORY_CD", typeof(int));
            dataTable.Columns.Add("AIRCRAFT_OPERATOR_CD", typeof(int));
            dataTable.Columns.Add("AIRCRAFT_USE_CD", typeof(int));
            dataTable.Columns.Add("MOTOR_TYPE_CD", typeof(int));
            dataTable.Columns.Add("MATERIAL_HULL_CD", typeof(int));
            dataTable.Columns.Add("PASSENGER_QTY", typeof(int));
            dataTable.Columns.Add("CREW_QTY", typeof(int));
            dataTable.Columns.Add("OPERATIONS_BASE", typeof(string));
            dataTable.Columns.Add("SERIAL_NO", typeof(string));
            dataTable.Columns.Add("AIRCRAFT_YEAR", typeof(int));
            dataTable.Columns.Add("AIRCRAFT_DESCRIPTION", typeof(string));
            dataTable.Columns.Add("LOAD_CAPACITY", typeof(decimal));
            dataTable.Columns.Add("OVERHAULING", typeof(bool));
            dataTable.Columns.Add("REGISTER_NO", typeof(string));

            DataRow rows = dataTable.NewRow();

            rows["AIRCRAFT_TYPE_CD"] = companyAircraft.TypeId; //1 Casco Aviación
            rows["AIRCRAFT_MAKE_CD"] = companyAircraft.MakeId;
            rows["AIRCRAFT_MODEL_CD"] = companyAircraft.ModelId;
            rows["AIRCRAFT_REGISTER_CD"] = companyAircraft.RegisterId;
            rows["AIRCRAFT_TERRITORY_CD"] = DBNull.Value;
            rows["AIRCRAFT_OPERATOR_CD"] = companyAircraft.OperatorId;
            rows["AIRCRAFT_USE_CD"] = companyAircraft.UseId;
            rows["MOTOR_TYPE_CD"] = DBNull.Value;
            rows["MATERIAL_HULL_CD"] = DBNull.Value;
            rows["PASSENGER_QTY"] = DBNull.Value;
            rows["CREW_QTY"] = DBNull.Value;
            rows["OPERATIONS_BASE"] = DBNull.Value;
            rows["SERIAL_NO"] = DBNull.Value;
            rows["AIRCRAFT_YEAR"] = companyAircraft.CurrentManufacturing;
            rows["AIRCRAFT_DESCRIPTION"] = DBNull.Value;
            rows["LOAD_CAPACITY"] = DBNull.Value;
            rows["OVERHAULING"] = false;
            rows["REGISTER_NO"] = companyAircraft.NumberRegister;

            dataTable.Rows.Add(rows);
            return dataTable;
        }
    }
}