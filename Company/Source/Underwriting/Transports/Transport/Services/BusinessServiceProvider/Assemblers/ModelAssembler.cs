using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PAREN = Sistran.Core.Application.Parameters.Entities;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Business;

namespace Sistran.Company.Application.Transports.TransportBusinessService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region PolicyType
        public static List<CompanyTransport> CreateCompanyTransports(BusinessCollection businessCollection)
        {
            List<CompanyTransport> companyTransport = new List<CompanyTransport>();

            foreach (ISSEN.EndorsementOperation entityEndorsementOperation in businessCollection)
            {
                companyTransport.Add(CreateCompanyTransport(entityEndorsementOperation));
            }

            return companyTransport;
        }

        public static CompanyTransport CreateCompanyTransport(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            CompanyTransport companyTransport = new CompanyTransport();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyTransport = COMUT.JsonHelper.DeserializeJson<CompanyTransport>(entityEndorsementOperation.Operation);
                companyTransport.Risk.Id = 0;
                companyTransport.Risk.Number = entityEndorsementOperation.RiskNumber.Value;
                companyTransport.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
            }

            return companyTransport;
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
                // LA TABLA COVERAGE DEDUTIBLE NO TIENE DATA CON LAS COVERTURAS QUE ENVIA
                // Deductible = new CompanyDeductible { Id = Coverage.Deductible.Id },
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
        /// Crea las coverturas de transporte
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
            coverage.DeclaredAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.DeclaredAmount);
            coverage.PremiumAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.PremiumAmount);
            coverage.LimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitAmount);
            coverage.SubLimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.SubLimitAmount);
            coverage.ExcessLimit = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitInExcess);
            coverage.LimitOccurrenceAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitOccurrenceAmount); ;
            coverage.LimitClaimantAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.LimitClaimantAmount);
            //coverage.AccumulatedPremiumAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccumulatedPremiumAmount);
            coverage.AccumulatedLimitAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccumulatedLimitAmount);
            coverage.AccumulatedDeductAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.AccumulatedSubLimitAmount);
            coverage.RateType = (RateType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.RateTypeCode);
            coverage.Rate = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.Rate);
            coverage.MainCoverageId = facade.GetConcept<int>(CompanyRuleConceptCoverage.MainCoverageId);
            coverage.MainCoveragePercentage = facade.GetConcept<int>(CompanyRuleConceptCoverage.MainCoveragePercentage);
            //coverage.ShortTermCode= facade.GetConcept<int>(CompanyRuleConceptCoverage.ShortTermCode);
            coverage.ShortTermPercentage = facade.GetConcept<int>(CompanyRuleConceptCoverage.ShortTermPercentage);
            coverage.CurrentFrom = facade.GetConcept<DateTime>(CompanyRuleConceptCoverage.CurrentFrom);
            coverage.CurrentTo = facade.GetConcept<DateTime>(CompanyRuleConceptCoverage.CurrentTo);
            coverage.CoverStatus = (CoverageStatusType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CoverageStatusCode);
            coverage.CoverageOriginalStatus = (CoverageStatusType?)facade.GetConcept<int>(CompanyRuleConceptCoverage.CoverageOriginalStatusCode);

            coverage.MaxLiabilityAmount = facade.GetConcept<decimal>(CompanyRuleConceptCoverage.MaxLiabilityAmount);


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
            coverage.MinimumPremiumCoverage = facade.GetConcept<int>(CompanyRuleConceptCoverage.MinimumPremiumCoverage);
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

        /// <summary>
        /// Crea una lista de conceptos din�micos
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
        /// Crea un conceptos din�mico
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
        /// Crea el transporte
        /// </summary>
        /// <param name="transport"></param>
        /// <param name="facade"></param>
        /// <returns></returns>
        public static CompanyTransport CreateCompanyTransport(CompanyTransport transport, Rules.Facade facade)
        {
            transport.Risk.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.RiskId);
            transport.Risk.GroupCoverage.Coverage.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            transport.CargoType.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.TransportCargoTypeId);
            transport.PackagingType.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.TransportPackagingTypeId);
            transport.CityFrom.State.Country.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CountrySourceId);
            transport.CityFrom.State.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.StateSourceId);
            transport.CityFrom.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CitySourceId);
            transport.CityTo.State.Country.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CountryDestinyId);
            transport.CityTo.State.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.StateDestinyId);
            transport.CityTo.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CityDestinyId);
            transport.ViaType.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.TransportViaTypeId);
            transport.Risk.IsRetention = facade.GetConcept<Boolean>(CompanyRuleConceptRisk.IsRetention);
            transport.Risk.Policy.Holder.IndividualId = facade.GetConcept<int>(CompanyRuleConceptRisk.IndividualId);
            transport.DeclarationPeriod.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.DeclarationPeriodId);
            transport.AdjustPeriod.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.AdjustPeriodId);
            transport.Source = facade.GetConcept<string>(CompanyRuleConceptRisk.Source);
            transport.Destiny = facade.GetConcept<string>(CompanyRuleConceptRisk.Destiny);
            transport.ReleaseAmount = facade.GetConcept<decimal>(CompanyRuleConceptRisk.ReleaseAmount);
            transport.FreightAmount = facade.GetConcept<decimal>(CompanyRuleConceptRisk.FreightAmount);
            transport.LimitMaxReleaseAmount = facade.GetConcept<decimal>(CompanyRuleConceptRisk.LimitMaxReleaseAmount);
            transport.ReleaseDate = facade.GetConcept<DateTime>(CompanyRuleConceptRisk.ReleaseDate);
            transport.MinimumPremium = facade.GetConcept<decimal>(CompanyRuleConceptRisk.PremiunRisk);
            transport.Risk.IsFacultative = facade.GetConcept<Boolean>(CompanyRuleConceptRisk.IsFacultative);
            return transport;
        }

        /// <summary>
        /// Convierte un modelo Deductible a un modelo deducible de la capa compa��a
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
        /// Convierte un listado de modelos Deductible a un conjunto de deducibles de la capa compa��a
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
        public static CompanyTransport CreateTransport(ISSEN.Risk risk, ISSEN.RiskTransport riskTransport, ISSEN.EndorsementRisk endorsementRisk, int[] valores)
        {
            CompanyTransport transport = new CompanyTransport();
            transport.Risk = new CompanyRisk
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
                IsFacultative = risk.IsFacultative,
                IsRetention = riskTransport.Retention,
                RiskActivity = new CompanyRiskActivity
                {
                    Id = (int)risk.RiskCommercialClassCode
                },
                Status = RiskStatusType.NotModified,
                OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
                Policy = new CompanyPolicy()
                {
                    Id = endorsementRisk.PolicyId,
                    Endorsement = new CompanyEndorsement()
                    {
                        Id = endorsementRisk.RiskId,
                    },
                    Product = new ProductServices.Models.CompanyProduct
                    {
                        Id = valores[0]
                    },
                    Prefix = new CompanyPrefix
                    {
                        Id = valores[1]
                    },
                    Branch = new CompanyBranch
                    {
                        Id = valores[2]
                    },
                },
                DynamicProperties = new List<DynamicConcept>(),

            };
            transport.HolderType = new CompanyHolderType
            {
                Id = (int)riskTransport.HolderTypeCode
            };
            transport.Source = riskTransport.Source;
            transport.Destiny = riskTransport.Destiny;
            transport.AdjustPeriod = new AdjustPeriod
            {
                Id = (int)riskTransport.AdjustPeriodCode
            };
            transport.DeclarationPeriod = new DeclarationPeriod
            {
                Id = (int)riskTransport.DeclarationPeriodCode
            };
            transport.CargoType = new CargoType
            {
                Id = riskTransport.TransportCargoTypeCode
            };
            transport.PackagingType = new PackagingType
            {
                Id = riskTransport.TransportPackagingTypeCode
            };
            transport.MinimumPremium = (decimal)riskTransport.MinimumPremium;
            transport.LimitMaxReleaseAmount = riskTransport.LimitMaxReleaseAmount;
            transport.ReleaseAmount = riskTransport.ReleaseAmount.GetValueOrDefault();
            transport.FreightAmount = riskTransport.FreightAmount;

            foreach (DynamicProperty item in risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                transport.Risk.DynamicProperties.Add(dynamicConcept);
            }

            return transport;
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
                CurrentTo = entityEndorsement.CurrentTo ?? DateTime.Now


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

        public static CompanyRiskCommercialClass CreateCompanyRiskCommercialClass(PAREN.RiskCommercialClass riskCommercialClass)
        {
            return new CompanyRiskCommercialClass
            {
                Id = riskCommercialClass.RiskCommercialClassCode,
                Description = riskCommercialClass.Description + " (" + riskCommercialClass.RiskCommercialClassCode + ") ",
                SmallDescription = riskCommercialClass.SmallDescription,
                Enabled = riskCommercialClass.Enabled
            };
        }

        public static List<CompanyRiskCommercialClass> CreateCompanyRiskCommercialClasses(BusinessCollection businessCollection)
        {
            List<CompanyRiskCommercialClass> companyRiskCommercialClasses = new List<CompanyRiskCommercialClass>();

            foreach (PAREN.RiskCommercialClass riskCommercialClass in businessCollection)
            {
                companyRiskCommercialClasses.Add(CreateCompanyRiskCommercialClass(riskCommercialClass));
            }

            return companyRiskCommercialClasses;

        }

        public static CompanyHolderType CreateCompanyHolderType(COMMEN.HolderType holderType)
        {
            return new CompanyHolderType
            {
                Id = holderType.HolderTypeCode,
                Description = holderType.Description,
                SmallDescription = holderType.SmallDescription,
                Enabled = holderType.Enabled
            };
        }

        public static List<CompanyHolderType> CreateCompanyHolderTypes(BusinessCollection businessCollection)
        {
            List<CompanyHolderType> companyHolderTypes = new List<CompanyHolderType>();

            foreach (COMMEN.HolderType holderType in businessCollection)
            {
                companyHolderTypes.Add(CreateCompanyHolderType(holderType));
            }

            return companyHolderTypes.Where(x => x.Enabled).ToList();

        }

        internal static DataTable GetDataTableTempRiskTransport(CompanyTransport companyTransport)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_TRANSPORT ");

            dataTable.Columns.Add("TRANSPORT_CARGO_TYPE_CD", typeof(int));
            dataTable.Columns.Add("TRANSPORT_PACKAGING_TYPE_CD", typeof(int));
            dataTable.Columns.Add("COUNTRY_FROM_CD", typeof(Int16));
            dataTable.Columns.Add("STATE_FROM_CD", typeof(Int16));
            dataTable.Columns.Add("CITY_FROM_CD", typeof(Int16));
            dataTable.Columns.Add("COUNTRY_TO_CD", typeof(Int16));
            dataTable.Columns.Add("STATE_TO_CD", typeof(Int16));
            dataTable.Columns.Add("CITY_TO_CD", typeof(Int16));
            dataTable.Columns.Add("TRANSPORT_VIA_TYPE_CD", typeof(Int16));
            dataTable.Columns.Add("HOLDER_TYPE_CD", typeof(int));
            //tiny
            dataTable.Columns.Add("DECLARATION_PERIOD_CD", typeof(Int16));
            //tiny
            dataTable.Columns.Add("ADJUST_PERIOD_CD", typeof(Int16));
            dataTable.Columns.Add("SOURCE", typeof(string));
            dataTable.Columns.Add("DESTINY", typeof(string));
            dataTable.Columns.Add("RELEASE_AMT", typeof(decimal));
            dataTable.Columns.Add("FREIGHT_AMT", typeof(decimal));
            dataTable.Columns.Add("LIMIT_MAX_RELEASE_AMT", typeof(decimal));


            dataTable.Columns.Add("RELEASE_DATE", typeof(string));
            dataTable.Columns.Add("ANNOUNCEMENT_DATE", typeof(string));


            dataTable.Columns.Add("CUSTOMS_BROKER", typeof(string));
            dataTable.Columns.Add("CUSTOMS_AGENT", typeof(string));
            dataTable.Columns.Add("RETENTION", typeof(bool));
            dataTable.Columns.Add("MINIMUM_PREMIUM", typeof(decimal));

            DataRow rows = dataTable.NewRow();


            rows["TRANSPORT_CARGO_TYPE_CD"] = companyTransport.CargoType.Id;
            rows["TRANSPORT_PACKAGING_TYPE_CD"] = companyTransport.PackagingType.Id;

            rows["COUNTRY_FROM_CD"] = DBNull.Value;
            if (companyTransport.CityFrom.State.Country.Id != 0)
            {
                rows["COUNTRY_FROM_CD"] = companyTransport.CityFrom.State.Country.Id;
            }

            rows["STATE_FROM_CD"] = DBNull.Value;
            if (companyTransport.CityFrom.State.Id != 0)
            {
                rows["STATE_FROM_CD"] = companyTransport.CityFrom.State.Id;
            }

            rows["CITY_FROM_CD"] = DBNull.Value;
            if (companyTransport.CityFrom.Id != 0)
            {
                rows["CITY_FROM_CD"] = companyTransport.CityFrom.Id;
            }

            rows["COUNTRY_TO_CD"] = DBNull.Value;
            if (companyTransport.CityTo.State.Country.Id != 0)
            {
                rows["COUNTRY_TO_CD"] = companyTransport.CityTo.State.Country.Id;
            }

            rows["STATE_TO_CD"] = DBNull.Value;
            if (companyTransport.CityTo.State.Id != 0)
            {
                rows["STATE_TO_CD"] = companyTransport.CityTo.State.Id;
            }

            rows["CITY_TO_CD"] = DBNull.Value;
            if (companyTransport.CityTo.Id != 0)
            {
                rows["CITY_TO_CD"] = companyTransport.CityTo.Id;
            }

            rows["TRANSPORT_VIA_TYPE_CD"] = DBNull.Value;
            if (companyTransport.ViaType.Id != 0)
            {
                rows["TRANSPORT_VIA_TYPE_CD"] = companyTransport.ViaType.Id;
            }

            rows["HOLDER_TYPE_CD"] = companyTransport.HolderType.Id;

            rows["DECLARATION_PERIOD_CD"] = DBNull.Value;
            if (companyTransport.DeclarationPeriod.Id != 0)
            {
                rows["DECLARATION_PERIOD_CD"] = companyTransport.DeclarationPeriod.Id;
            }

            rows["ADJUST_PERIOD_CD"] = DBNull.Value;
            if (companyTransport.AdjustPeriod.Id != 0)
            {
                rows["ADJUST_PERIOD_CD"] = companyTransport.AdjustPeriod.Id;
            }

            rows["SOURCE"] = companyTransport.Source;
            rows["DESTINY"] = companyTransport.Destiny;

            //por verificar
            rows["RELEASE_AMT"] = DBNull.Value;
            if (companyTransport.ReleaseAmount != 0)
            {
                rows["RELEASE_AMT"] = companyTransport.ReleaseAmount;
            }

            rows["FREIGHT_AMT"] = DBNull.Value;
            if (companyTransport.FreightAmount != 0)
            {
                rows["FREIGHT_AMT"] = companyTransport.FreightAmount;
            }

            rows["LIMIT_MAX_RELEASE_AMT"] = DBNull.Value;
            if (companyTransport.LimitMaxReleaseAmount != 0)
            {
                rows["LIMIT_MAX_RELEASE_AMT"] = companyTransport.LimitMaxReleaseAmount;
            }

            rows["RELEASE_DATE"] = DBNull.Value;
            if (companyTransport.ReleaseDate != DateTime.MinValue)
            {
                rows["RELEASE_DATE"] = companyTransport.ReleaseDate;
            }


            rows["ANNOUNCEMENT_DATE"] = DBNull.Value;
            rows["CUSTOMS_BROKER"] = DBNull.Value;
            rows["CUSTOMS_AGENT"] = DBNull.Value;
            rows["RETENTION"] = companyTransport.Risk.IsRetention;
            rows["MINIMUM_PREMIUM"] = companyTransport.MinimumPremium;

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
                throw;
            }
            return Event;
        }

        internal static CompanyTransport CreateCompanyTransportsByRiskTransport(ISSEN.RiskTransport riskTransport)
        {
            if (riskTransport == null)
            {
                return null;
            }

            return new CompanyTransport
            {
                Risk = new CompanyRisk
                {
                    RiskId = riskTransport.RiskId
                },
                CargoType = new CargoType
                {
                    Id = riskTransport.TransportCargoTypeCode
                },
                PackagingType = new PackagingType
                {
                    Id = riskTransport.TransportPackagingTypeCode
                },
                CityFrom = new City
                {
                    Id = Convert.ToInt32(riskTransport.CityFromCode)
                },
                CityTo = new City
                {
                    Id = Convert.ToInt32(riskTransport.CityToCode)
                },
                ViaType = new ViaType
                {
                    Id = Convert.ToInt32(riskTransport.TransportViaTypeCode)
                },
                InsuredObjects = new List<CompanyInsuredObject>(),
                Types = new List<TransportType>()
            };
        }

        public static CompanyEndorsementPeriod CreateCompanyEndorsementPeriod(CompanyPolicy policy, CompanyTransport transport, decimal policyId)
        {

            return new CompanyEndorsementPeriod()
            {
                PolicyId = policy.Endorsement.PolicyId,
                CurrentFrom = policy.CurrentFrom,
                CurrentTo = policy.CurrentTo,
                AdjustPeriod = transport.AdjustPeriod.Id,
                DeclarationPeriod = transport.DeclarationPeriod.Id,
                TotalAdjustment = 0,
                TotalDeclarations = 0,
                Version = 0
            };
        }

        public static List<CompanyEndorsementDetail> CreateCompanyEndorsementDetails(List<CompanyEndorsement> endorsements, List<CompanyTransport> transports, decimal policyId, CompanyEndorsementPeriod endorsementPeriod)
        {

            List<CompanyEndorsementDetail> endorsementDetails = new List<CompanyEndorsementDetail>();
            foreach (CompanyEndorsement item in endorsements)
            {
                CompanyTransport riskTransport = transports.Where(x => x.Risk.RiskId == item.RiskId).FirstOrDefault();
                if (riskTransport == null)
                {
                    riskTransport = transports.Where(x => x.Risk.Number == item.RiskId).FirstOrDefault();
                }
                CompanyInsuredObject insured = riskTransport.InsuredObjects.Where(x => x.Id == item.InsuredObjectId).FirstOrDefault();
                item.Number = riskTransport.Risk.Number;
                endorsementDetails.Add(CreateCompanyEndorsementDetail(riskTransport, item, insured, endorsementPeriod, policyId));
            }
            return endorsementDetails;
        }

        public static CompanyEndorsementDetail CreateCompanyEndorsementDetail(CompanyTransport transport, CompanyEndorsement item, CompanyInsuredObject insuredObject, CompanyEndorsementPeriod endorsementPeriod, decimal documentNumber)
        {
            switch (item.EndorsementType)
            {
                case EndorsementType.AdjustmentEndorsement:
                    TransportBusiness transportBusiness = new TransportBusiness();
                    CompanyEndorsementDetail endorsementDetail = new CompanyEndorsementDetail();
                    List<CompanyEndorsementDetail> detailsList = transportBusiness.GetEndorsementDetailsListByPolicyId(documentNumber, endorsementPeriod.Version);
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

                    CompanyEndorsementDetail companyEndoDetail = new CompanyEndorsementDetail()
                    {
                        DeclarationValue = transport.Risk.Coverages.Where(x => x.IsPrimary == true || (x.MainCoverageId >= 0 && x.AllyCoverageId == null)).Sum(n => n.DeclaredAmount),
                        RiskNum = item.Number,//(int)item.RiskId,
                        EndorsementType = (int)item.EndorsementType,
                        PolicyId = item.PolicyId,
                        InsuredObjectId = (int)item.InsuredObjectId,
                        //PremiumAmmount = (insuredObject.DepositPremiunPercent != 0) ? (item.DeclaredValue * (insuredObject.DepositPremiunPercent/100) * (insuredObject.Rate/100)) : (insuredObject.BillingPeriodDepositPremium * (insuredObject.Rate/100)),
                        PremiumAmount = transport.Risk.Coverages.Where(x => x.IsPrimary == true || (x.MainCoverageId >= 0 && x.AllyCoverageId == null)).Sum(n => n.DeclaredAmount * insuredObject.Rate / 100),
                        EndorsementDate = DateTime.Now,
                        Version = endorsementPeriod.Version,
                        //DeductibleAmmount = transport.Risk.Coverages.Where(x => x.IsPrimary == true || (x.MainCoverageId >= 0 && x.AllyCoverageId == null)).Sum(n => n.Deductible.DeductPremiumAmount)
                    };
                    return companyEndoDetail;
                    break;
            }
            return new CompanyEndorsementDetail();


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
    }
}