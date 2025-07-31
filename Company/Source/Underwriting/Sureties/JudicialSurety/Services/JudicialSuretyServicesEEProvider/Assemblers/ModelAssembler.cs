using AutoMapper;
using JudicialSuretyServicesEEProvider;
using Newtonsoft.Json;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Enums;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Linq;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using CommonModels = Sistran.Core.Application.CommonService.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using IssuanceEntities = Sistran.Core.Application.Issuance.Entities;
using UPMB = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using System.Data;
using System;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static CompanyJudgement CreateJudicialSurety(CompanyJudgementMapper companyJudgementMapper)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskPostS();
            var mapperJudgement = mapper.Map<CompanyJudgementMapper, CompanyJudgement>(companyJudgementMapper);

            if (mapperJudgement.Risk.DynamicProperties != null && mapperJudgement.Risk.DynamicProperties.Count > 0)
            {
                foreach (DynamicConcept item in mapperJudgement.Risk.DynamicProperties)
                {
                    DynamicConcept dynamicConcept = new DynamicConcept();
                    dynamicConcept.Id = item.Id;
                    dynamicConcept.Value = item.Value;
                    mapperJudgement.Risk.DynamicProperties.Add(dynamicConcept);
                }

            }
            return mapperJudgement;
        }
      
        #region Beneficiarios
        public static List<CompanyBeneficiary> CreateBeneficiaries(List<IssuanceEntities.RiskBeneficiary> entityBeneficiaries)
        {
            var mapper = AutoMapperAssembler.CreateMapBeneficiary();
            return mapper.Map<List<IssuanceEntities.RiskBeneficiary>, List<CompanyBeneficiary>>(entityBeneficiaries.Cast<IssuanceEntities.RiskBeneficiary>().ToList());

            //List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();

            //foreach (IssuanceEntities.RiskBeneficiary item in entityBeneficiaries)
            //{
            //    beneficiaries.Add(ModelAssembler.CreateBeneficiary(item));
            //}

            //return beneficiaries;
        }

        public static CompanyBeneficiary CreateBeneficiary(IssuanceEntities.RiskBeneficiary riskBeneficiary)
        {
            var mapper = AutoMapperAssembler.CreateMapBeneficiary();
            return mapper.Map<IssuanceEntities.RiskBeneficiary, CompanyBeneficiary>(riskBeneficiary);
        }

        public static Models.CompanyJudgement CreateJudgement(CompanyJudgement judgement, Rules.Facade facade)//Entities.FacadeRiskJudgement facadeJudgement)
        {
            //CompanyJudgement judgement = new Models.CompanyJudgement();
            if (facade.GetConcept<int>(RuleConceptRisk.RatingZoneCode)>0)
            {
                if (judgement.Risk.RatingZone == null)
                {
                    judgement.Risk.RatingZone = new CompanyRatingZone();
                }

                judgement.Risk.RatingZone.Id = facade.GetConcept<int>(RuleConceptRisk.RatingZoneCode);
            }

            if (facade.GetConcept<int>(RuleConceptRisk.CoverageGroupId)>0)
            {
                if (judgement.Risk.GroupCoverage == null)
                {
                    judgement.Risk.GroupCoverage = new GroupCoverage();
                }

                judgement.Risk.GroupCoverage.Id = facade.GetConcept<int>(RuleConceptRisk.CoverageGroupId);
            }

            if (facade.GetConcept<int>(RuleConceptRisk.LimitsRcCode)>0)
            {
                if (judgement.Risk.LimitRc == null)
                {
                    judgement.Risk.LimitRc = new CompanyLimitRc();
                }

                judgement.Risk.LimitRc.Id = facade.GetConcept<int>(RuleConceptRisk.LimitsRcCode);
            }

            if (facade.GetConcept<decimal>(RuleConceptRisk.LimitsRcSum)>0)
            {
                if (judgement.Risk.LimitRc == null)
                {
                    judgement.Risk.LimitRc = new CompanyLimitRc();
                }

                judgement.Risk.LimitRc.LimitSum = facade.GetConcept<decimal>(RuleConceptRisk.LimitsRcSum);
            }

            judgement.Risk.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);

            return judgement;
        }

        #endregion


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
            var mapper = AutoMapperAssembler.CreateMapDynamicConcept();
            return mapper.Map<Rules.Concept, DynamicConcept>(concept);
        }

        #region Cobeerturas

        public static CompanyCoverage CreateCoverage(CompanyCoverage coverage, Rules.Facade facade)//FacadeCoverage facadeCoverage)
        {
            coverage.IsDeclarative = facade.GetConcept<bool>(RuleConceptCoverage.IsDeclarative);
            coverage.IsMinPremiumDeposit = facade.GetConcept<bool>(RuleConceptCoverage.IsMinimumPremiumDeposit);
            coverage.FirstRiskType = (FirstRiskType?)facade.GetConcept<int>(RuleConceptCoverage.FirstRiskTypeCode);
            coverage.CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType?)facade.GetConcept<int>(RuleConceptCoverage.CalculationTypeCode);
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

            if (facade.GetConcept<int>(RuleConceptCoverage.DeductId) > 0)
            {
                if (coverage.Deductible == null)
                {
                    coverage.Deductible = new CompanyDeductible();
                }

                coverage.Deductible.Id = facade.GetConcept<int>(RuleConceptCoverage.DeductId);

                if (facade.GetConcept<int>(RuleConceptCoverage.DeductRateTypeCode)>0)
                {
                    coverage.Deductible.RateType = (RateType)facade.GetConcept<int>(RuleConceptCoverage.DeductRateTypeCode);
                }

                coverage.Deductible.Rate = facade.GetConcept<decimal>(RuleConceptCoverage.DeductRate);
                coverage.Deductible.DeductPremiumAmount = facade.GetConcept<decimal>(RuleConceptCoverage.DeductPremiumAmount);
                coverage.Deductible.DeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.DeductValue);

                if (facade.GetConcept<int>(RuleConceptCoverage.DeductUnitCode)>0)
                {
                    coverage.Deductible.DeductibleUnit = new DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(RuleConceptCoverage.DeductUnitCode)
                    };
                }

                if (facade.GetConcept<int>(RuleConceptCoverage.DeductSubjectCode)>0)
                {
                    coverage.Deductible.DeductibleSubject = new DeductibleSubject
                    {
                        Id = facade.GetConcept<int>(RuleConceptCoverage.DeductSubjectCode)
                    };
                }

                coverage.Deductible.MinDeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.MinDeductValue);

                if (facade.GetConcept<int>(RuleConceptCoverage.MinDeductUnitCode)>0)
                {
                    coverage.Deductible.MinDeductibleUnit = new DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(RuleConceptCoverage.MinDeductUnitCode)
                    };
                }

                if (facade.GetConcept<int>(RuleConceptCoverage.MinDeductSubjectCode)>0)
                {
                    coverage.Deductible.MinDeductibleSubject = new DeductibleSubject
                    {
                        Id = facade.GetConcept<int>(RuleConceptCoverage.MinDeductSubjectCode)
                    };
                }

                coverage.Deductible.MaxDeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.MaxDeductValue);

                if (facade.GetConcept<int>(RuleConceptCoverage.MaxDeductUnitCode)>0)
                {
                    coverage.Deductible.MaxDeductibleUnit = new DeductibleUnit
                    {
                        Id = facade.GetConcept<int>(RuleConceptCoverage.MaxDeductUnitCode)
                    };
                }

                if (facade.GetConcept<int>(RuleConceptCoverage.MaxDeductSubjectCode)>0)
                {
                    coverage.Deductible.MaxDeductibleSubject = new DeductibleSubject
                    {
                        Id = facade.GetConcept<int>(RuleConceptCoverage.MaxDeductSubjectCode)
                    };
                }

                if (facade.GetConcept<int>(RuleConceptCoverage.CurrencyCode)>0)
                {
                    coverage.Deductible.Currency = new CommonModels.Currency
                    {
                        Id = facade.GetConcept<int>(RuleConceptCoverage.CurrencyCode)
                    };
                }

                coverage.Deductible.AccDeductAmt = facade.GetConcept<decimal>(RuleConceptCoverage.AccDeductAmt);
            }
            else
            {
                coverage.Deductible = null;
            }

            coverage.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            coverage.MinimumPremiumCoverage = facade.GetConcept<decimal>(RuleConceptCoverage.MinimumPremiumCoverage);
            return coverage;
        }
        #endregion

        internal static List<CompanyJudgement> CreateJudgements(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<ISSEN.EndorsementOperation>().ToList();
            var mapper = AutoMapperAssembler.CreateMapJudgement();
            return mapper.Map<List<ISSEN.EndorsementOperation>, List<CompanyJudgement>>(objBusiness);
        }

        internal static CompanyJudgement CreateJudgement(ISSEN.EndorsementOperation entityEndorsementOperation)
        {
            var mapper = AutoMapperAssembler.CreateMapJudgement();
            return mapper.Map<ISSEN.EndorsementOperation, CompanyJudgement>(entityEndorsementOperation);
        }
        #region automaper
        #region article
        /// <summary>
        /// Creates the map company coverage.
        /// </summary>
        public static IMapper CreateMapCompanyArticle()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Article, CompanyArticle>();
            });

            return config.CreateMapper();
        }
        #endregion article

        #region asegurado
        /// <summary>
        /// Createts the map company insured.
        /// </summary>
        public static IMapper CreatetMapCompanyInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaPersonModel.CompanyInsured, CompanyIssuanceInsured>();
            
                cfg.CreateMap<CiaPersonModel.CompanyInsured, BaseIssuanceInsured>();
            });
            return config.CreateMapper();

        }
        
        /// <summary>
        /// Createts the map company insured.
        /// </summary>
        public static IMapper CreatetMapCompanyInsuredPerson()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyIssuanceInsured, CiaPersonModel.CompanyInsured>();
                cfg.CreateMap<BaseIssuanceInsured, CiaPersonModel.CompanyInsured>();
            });
            return config.CreateMapper();

        }
        #endregion asegurado

        #region beneficiarios
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
                BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription,
                AssociationType = new IssuanceAssociationType { Id = insured.AssociationType.Id }
            };
        }
		
        public static CompanyBeneficiary CreateBeneficiaryFromIssuanceInsured(CompanyIssuanceInsured insured)
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
                BeneficiaryType = new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription },
                BeneficiaryTypeDescription = companyBeneficiaryType.First(x => x.Id == KeySettings.OnerousBeneficiaryTypeId).SmallDescription,
                AssociationType =new IssuanceAssociationType { Id = insured.AssociationType.Id}
            };
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


        #endregion beneficiarios
        #endregion automapper


        public static DataTable GetDataTableRiskJudicialSurety(CompanyJudgement companyJudgement)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_JUDICIAL_SURETY");
            dataTable.Columns.Add("NAME_NUM", typeof(int));
            dataTable.Columns.Add("INSURED_ID", typeof(int));
            dataTable.Columns.Add("ADDRESS_ID", typeof(int));
            dataTable.Columns.Add("PHONE_ID", typeof(int));
            dataTable.Columns.Add("PROFESSIONAL_CARD_NUM", typeof(string));
            dataTable.Columns.Add("ARTICLE_CD", typeof(int));
            dataTable.Columns.Add("COURT_CD", typeof(int));
            dataTable.Columns.Add("CAPACITY_OF_CD", typeof(int));
            dataTable.Columns.Add("COUNTRY_CD", typeof(int));
            dataTable.Columns.Add("STATE_CD", typeof(int));
            dataTable.Columns.Add("CITY_CD", typeof(int));
            dataTable.Columns.Add("INSURED_CAUTION", typeof(string));
            dataTable.Columns.Add("IS_RETENTION", typeof(bool));
            dataTable.Columns.Add("COURT_NUM", typeof(string));
            dataTable.Columns.Add("ID_CARD_TYPE_CD", typeof(int));
            dataTable.Columns.Add("ID_CARD_NO", typeof(string));
            dataTable.Columns.Add("BID_NUMBER", typeof(string));
            dataTable.Columns.Add("INSURED_VALUE", typeof(decimal));
            dataTable.Columns.Add("IDENTIFICATION_ID_AGENT", typeof(string));
            dataTable.Columns.Add("DOCUMENT_NUMBER_AGENT", typeof(string));
            dataTable.Columns.Add("INSURED_PRINT_NAME", typeof(string));
            DataRow rows = dataTable.NewRow();
            rows["NAME_NUM"] = companyJudgement.Risk.MainInsured.CompanyName.NameNum;
            rows["INSURED_ID"] = companyJudgement.Risk.MainInsured.IndividualId;
            if(companyJudgement.Risk.MainInsured.CompanyName.Address != null)
            rows["ADDRESS_ID"] = companyJudgement.Risk.MainInsured.CompanyName.Address.Id;
            if(companyJudgement.Risk.MainInsured.CompanyName.Phone != null)
            rows["PHONE_ID"] = companyJudgement.Risk.MainInsured.CompanyName.Phone.Id;
            if (companyJudgement.Attorney != null)
            {
                rows["PROFESSIONAL_CARD_NUM"] =  companyJudgement.Attorney.IdProfessionalCard;
                rows["IDENTIFICATION_ID_AGENT"] = companyJudgement?.Attorney?.IdentificationDocument?.DocumentType?.Description;
                rows["DOCUMENT_NUMBER_AGENT"] = companyJudgement?.Attorney?.IdentificationDocument?.Number;
                if (!string.IsNullOrEmpty(companyJudgement.Attorney.InsuredToPrint))
                {
                    rows["INSURED_PRINT_NAME"] = companyJudgement.Attorney.InsuredToPrint;
                }
                else if (!string.IsNullOrEmpty(companyJudgement.Attorney.Name))
                {
                    rows["INSURED_PRINT_NAME"] = companyJudgement.Attorney.Name;
                }
                else
                {
                    rows["INSURED_PRINT_NAME"] = DBNull.Value;
                }
            }
            else 
            {
                rows["PROFESSIONAL_CARD_NUM"] = DBNull.Value;
                rows["IDENTIFICATION_ID_AGENT"] = DBNull.Value;
                rows["IDENTIFICATION_ID_AGENT"] = DBNull.Value;
            }
            rows["ARTICLE_CD"] = companyJudgement.Article.Id;
            rows["COURT_CD"] = companyJudgement.Court.Id;
            rows["CAPACITY_OF_CD"] = (int)companyJudgement.InsuredActAs;
            if (companyJudgement.City != null)
            {
                rows["COUNTRY_CD"] = companyJudgement.City.State.Country.Id;
                rows["STATE_CD"] = companyJudgement.City.State.Id;
                rows["CITY_CD"] = companyJudgement.City.Id;
            }
            else
            {
                rows["COUNTRY_CD"] = DBNull.Value;
                rows["STATE_CD"] = DBNull.Value;  
                rows["CITY_CD"] = DBNull.Value;
            }
            rows["INSURED_CAUTION"] = DBNull.Value;
            rows["IS_RETENTION"] = false;
            rows["COURT_NUM"] = companyJudgement.Court.Description;
            if (companyJudgement.Risk.MainInsured.IdentificationDocument.DocumentType != null)
            {
                rows["ID_CARD_TYPE_CD"] = Convert.ToInt32(companyJudgement.Risk.MainInsured.IdentificationDocument.DocumentType.Id);
            }
            else
            {
                rows["ID_CARD_TYPE_CD"] = DBNull.Value;
            }
            if(companyJudgement.Risk.MainInsured.IdentificationDocument.Number!=null)
            {
                rows["ID_CARD_NO"] = companyJudgement.Risk.MainInsured.IdentificationDocument.Number;
            }
            else
            { 
              rows["ID_CARD_NO"] = DBNull.Value;
            }
            rows["BID_NUMBER"] = companyJudgement.SettledNumber;
            rows["INSURED_VALUE"] = companyJudgement.Risk.AmountInsured;
            dataTable.Rows.Add(rows);
            return dataTable;
        }
       
    }
}