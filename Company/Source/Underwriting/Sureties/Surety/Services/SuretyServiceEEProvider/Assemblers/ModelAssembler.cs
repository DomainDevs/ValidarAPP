using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Models;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Linq;
using CompanyISSModel = Sistran.Company.Application.UnderwritingServices.Models;
using CompanyPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PEMO = Sistran.Core.Application.UniquePersonService.V1.Models;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;
using UPMB = Sistran.Core.Application.UniquePersonService.V1.Models.Base;

using Sistran.Core.Application.Utilities.RulesEngine;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.UnderwritingServices;
using ENT = Sistran.Company.Application.Issuance.Entities;
using System;
using System.Data;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Contracts Risk
        /// <summary>
        /// Crear Riesgo de cumplimiento
        /// </summary>
        /// <param name="risk">entidad risk</param>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns></returns>
        public static CompanyContract CreateContract(ContractDto contractDto)
        {
            var mapper = CreatetMapCompanyContract();
            var contract = mapper.Map<ContractDto, CompanyContract>(contractDto);
            contract.Isfacultative = contractDto.Risk.IsFacultative;
            contract.Risk.IsFacultative = contractDto.Risk.IsFacultative;
            foreach (DynamicProperty item in contractDto.Risk.DynamicProperties)
            {
                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = dynamicProperty.Id;
                dynamicConcept.Value = dynamicProperty.Value;
                contract.Risk.DynamicProperties.Add(dynamicConcept);
            }

            return contract;
        }
        public static CompanyRiskSuretyPost CreateRiskPostS(ENT.PrvRiskCoveragePost prvRiskCoveragePost)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskPostS();
            return mapper.Map<ENT.PrvRiskCoveragePost, CompanyRiskSuretyPost>(prvRiskCoveragePost);
            //CompanyRiskSuretyPost companyTempRiskSuretyPost = new CompanyRiskSuretyPost();

            ////companyTempRiskSuretyPost.ContractDate = prvRiskCoveragePost.ContractDate;

            //if (prvRiskCoveragePost.CurrentToPost != null)
            //{
            //    companyTempRiskSuretyPost.ChkContractDate = true;
            //    companyTempRiskSuretyPost.ContractDate = prvRiskCoveragePost.ContractDate;
            //}
            //if (prvRiskCoveragePost.DeliveryDate != null)
            //{
            //    companyTempRiskSuretyPost.ChkContractFinalyDate = true;
            //    companyTempRiskSuretyPost.IssueDate = prvRiskCoveragePost.DeliveryDate;
            //}
            //companyTempRiskSuretyPost.UserId = prvRiskCoveragePost.UserId;

            //return companyTempRiskSuretyPost;
        }

        #endregion

        /// <summary>
        /// Creates the contract.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="facadeRiskContract">The facade risk contract.</param>
        /// <returns></returns>
        public static void CreateContract(CompanyContract contract, Rules.Facade facade)
        {

            facade.SetConcept(RuleConceptRisk.RiskId, contract.Risk.RiskId);

            if (facade.GetConcept<int>(RuleConceptRisk.InsuredId) > 0)
            {
                if (contract.Risk.MainInsured == null)
                {
                    contract.Risk.MainInsured = new CompanyIssuanceInsured();
                }
                contract.Risk.MainInsured.IndividualId = facade.GetConcept<int>(RuleConceptRisk.InsuredId);
            }
            if (contract.Risk.MainInsured == null)
            {
                contract.Risk.MainInsured = new CompanyIssuanceInsured();
            }
            contract.Risk.MainInsured.CustomerType = (CustomerType)facade.GetConcept<int>(RuleConceptRisk.CustomerTypeCode);
            if (contract.Risk.Text == null)
            {
                contract.Risk.Text = new CompanyText();
            }
            contract.Risk.Text.TextBody = facade.GetConcept<string>(RuleConceptRisk.ConditionText);
            if (facade.GetConcept<int>(RuleConceptRisk.RatingZoneCode) > 0)
            {
                if (contract.Risk.RatingZone == null)
                {
                    contract.Risk.RatingZone = new CompanyRatingZone();
                }
                contract.Risk.RatingZone.Id = facade.GetConcept<int>(RuleConceptRisk.RatingZoneCode);
            }
            if (facade.GetConcept<int>(RuleConceptRisk.CoverageGroupId) > 0)
            {
                if (contract.Risk.GroupCoverage == null)
                {
                    contract.Risk.GroupCoverage = new GroupCoverage();
                }
                contract.Risk.GroupCoverage.Id = facade.GetConcept<int>(RuleConceptRisk.CoverageGroupId);
            }
            if (facade.GetConcept<int>(RuleConceptRisk.LimitsRcCode) > 0)
            {
                if (contract.Risk.LimitRc == null)
                {
                    contract.Risk.LimitRc = new CompanyLimitRc();
                }
                contract.Risk.LimitRc.Id = facade.GetConcept<int>(RuleConceptRisk.LimitsRcCode);
            }
            if (facade.GetConcept<int>(RuleConceptRisk.LimitsRcSum) > 0)
            {
                if (contract.Risk.LimitRc == null)
                {
                    contract.Risk.LimitRc = new CompanyLimitRc();
                }
                contract.Risk.LimitRc.LimitSum = facade.GetConcept<decimal>(RuleConceptRisk.LimitsRcSum);
            }
            if (contract.ContractType == null)
            {
                contract.ContractType = new CompanyContractType();
            }
            contract.ContractType.Id = facade.GetConcept<int>(RuleConceptRisk.SuretyContractType);
            if (contract.Contractor == null)
            {
                contract.Contractor = new CompanyContractor();
            }
            contract.Contractor.IndividualId = facade.GetConcept<int>(RuleConceptRisk.IndividualId);
            if (facade.GetConcept<int>(RuleConceptRisk.SuretyContractCategoriesCode) > 0)
            {
                if (contract.Class == null)
                {
                    contract.Class = new CompanyContractClass();
                }
                contract.Class.Id = facade.GetConcept<int>(RuleConceptRisk.SuretyContractCategoriesCode);
            }
            if (contract.Value == null)
            {
                contract.Value = new Amount();
            }
            contract.Value.Value = facade.GetConcept<decimal>(RuleConceptRisk.ContractAmount);
            contract.Aggregate = facade.GetConcept<decimal>(RuleConceptRisk.OperatingPile);
            if (contract.OperatingQuota == null)
            {
                contract.OperatingQuota = new OperatingQuota();

            }
            contract.OperatingQuota.Amount = facade.GetConcept<decimal>(RuleConceptRisk.OperatingQuotaAmount);
            contract.Risk.DynamicProperties = CreateDynamicConcepts(facade);
        }

        #region Beneficiary

        public static CompanyBeneficiary CreateBeneficiary(ISSEN.RiskBeneficiary riskBeneficiary)
        {
            var mapper = AutoMapperAssembler.CreateMapBeneficiary();
            return mapper.Map<ISSEN.RiskBeneficiary, CompanyBeneficiary>(riskBeneficiary);
        }
        #endregion


        /// <summary>
        /// Creates the coverage.
        /// </summary>
        /// <param name="coverage">The coverage.</param>
        /// <param name="facadeCoverage">The facade coverage.</param>
        /// <returns></returns>
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

            if (facade.GetConcept<int>(RuleConceptCoverage.DeductId) > 0)
            {
                if (coverage.Deductible == null)
                {
                    coverage.Deductible = new CompanyDeductible();
                }

                coverage.Deductible.Id = facade.GetConcept<int>(RuleConceptCoverage.DeductId);

                if ((RateType)facade.GetConcept<int>(RuleConceptCoverage.DeductRateTypeCode) > 0)
                {
                    coverage.Deductible.RateType = (RateType)facade.GetConcept<int>(RuleConceptCoverage.DeductRateTypeCode);
                }

                coverage.Deductible.Rate = facade.GetConcept<decimal>(RuleConceptCoverage.DeductRate);
                coverage.Deductible.DeductPremiumAmount = facade.GetConcept<decimal>(RuleConceptCoverage.DeductPremiumAmount);
                coverage.Deductible.DeductValue = facade.GetConcept<decimal>(RuleConceptCoverage.DeductValue);

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
            }
            else
            {
                coverage.Deductible = null;
            }

            coverage.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            coverage.MinimumPremiumCoverage = facade.GetConcept<decimal>(RuleConceptCoverage.MinimumPremiumCoverage);
            coverage.IsEnabledMinimumPremium = facade.GetConcept<bool>(RuleConceptCoverage.IsEnabledMinimumPremium);
            //Facade.GetConcept<List<CompanyCoverage>>(CompanyRuleConceptRisk.Coverages)
            return coverage;
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
            var mapper = AutoMapperAssembler.CreateMapDynamicConcept();
            return mapper.Map<Rules.Concept, DynamicConcept>(concept);
        }


        #region beneficiarios
        public static CompanyBeneficiary CreateBeneficiaryFromInsured(CompanyIssuanceInsured insured)
        {
            var mapper = AutoMapperAssembler.CreateMapBeneficiaryFromInsured();
            return mapper.Map<CompanyIssuanceInsured, CompanyBeneficiary>(insured);

        }
        #endregion
        #region automapper
        #region Asegurado
        ///// <summary>
        ///// Createts the map company insured.
        ///// </summary>       
        public static IMapper CreateMapCompanyPersonInsured()
        {
            var config = MapperCache.GetMapper<Sistran.Core.Application.UniquePersonService.V1.Models.EconomicActivity, BaseEconomicActivity>(cfg =>
             {
                 cfg.CreateMap<UPMB.BaseInsured, Core.Application.UnderwritingServices.Models.Base.BaseIssuanceInsured>();
                 cfg.CreateMap<IndividualPaymentMethod, CompanyPersonModel.CiaIndividualPaymentMethod>();
                 cfg.CreateMap<UPMB.BaseIndividualPaymentMethod, BaseIndividualPaymentMethod>();
                 cfg.CreateMap<Sistran.Core.Application.UniquePersonService.V1.Models.EconomicActivity, BaseEconomicActivity>();
             });
            return config;
        }
        //}

        public static IMapper CreatetMapCompanyContract()
        {
            var config = MapperCache.GetMapper<ContractDto, CompanyContract>(cfg =>
            {
                cfg.CreateMap<ISSEN.Risk, CompanyRisk>();
                cfg.CreateMap<UNMO.GroupCoverage, UNMO.GroupCoverage>();
                cfg.CreateMap<ContractDto, CompanyContract>()

            .ForMember(dest => dest.Risk, opt => opt.MapFrom(src => new CompanyRisk
            {
                RiskId = src.Risk.RiskId,
                CoveredRiskType = (CoveredRiskType)src.Risk.CoveredRiskTypeCode,
                GroupCoverage = new UNMO.GroupCoverage
                {
                    Id = src.Risk.CoverGroupId.Value,
                    CoveredRiskType = (CoveredRiskType)src.Risk.CoveredRiskTypeCode
                },
                MainInsured = new CompanyIssuanceInsured
                {
                    IndividualId = src.Risk.InsuredId,
                    CompanyName = new IssuanceCompanyName
                    {
                        NameNum = src.Risk.NameNum.GetValueOrDefault(),
                        Address = new IssuanceAddress
                        {
                            Id = src.Risk.AddressId.GetValueOrDefault()
                        }
                    }
                },
                Policy = new CompanyPolicy
                {
                    DocumentNumber = src.Policy.DocumentNumber,
                    Endorsement = new CompanyEndorsement { Id = src.EndorsementRisk.EndorsementId, PolicyId = src.Policy.PolicyId }
                },
                Number = src.EndorsementRisk.RiskNum,
                OriginalStatus = (RiskStatusType)src.EndorsementRisk.RiskStatusCode,
                Status = RiskStatusType.NotModified,
                Text = new CompanyText
                {
                    TextBody = src.Risk.ConditionText
                },
                DynamicProperties = new List<DynamicConcept>(),
                IsNational = src.CoRiskSurety == null ? false : src.CoRiskSurety.IsNational
            }))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => new City
            {
                Id = src.CoRiskSurety.CityCode == null ? 0 : (int)src.CoRiskSurety.CityCode
            }))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => new Country
            {

                Id = src.CoRiskSurety.CountryCode == null ? 0 : (int)src.CoRiskSurety.CountryCode
            }))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => new State

            {
                Id = src.CoRiskSurety.StateCode == null ? 0 : (int)src.CoRiskSurety.StateCode
            }))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => new Amount
            {
                Value = src.RiskSurety.ContractAmount
            }))
            .ForMember(dest => dest.ContractType, opt => opt.MapFrom(src => new CompanyContractType
            {
                Id = src.RiskSurety.SuretyContractTypeCode
            }))
            .ForMember(dest => dest.Isfacultative, opt => opt.MapFrom(src => src.RiskSurety.IsFacultative))
            .ForMember(dest => dest.Class, opt => opt.MapFrom(src => new CompanyContractClass
            {
                Id = Convert.ToInt32(src.RiskSurety.SuretyContractCategoriesCode)
            }))
            .ForMember(dest => dest.Contractor, opt => opt.MapFrom(src => new CompanyContractor
            {
                IndividualId = src.RiskSurety.IndividualId
            }))
            .ForMember(dest => dest.SettledNumber, opt => opt.MapFrom(src => src.RiskSurety.BidNumber))
            .ForMember(dest => dest.ContractObject, opt => opt.MapFrom(src => new CompanyText
            {
                TextBody = src.RiskSuretyContract.ObjectContract ?? ""
            }));
            });


            return config;
        }
        #endregion Asegurado
        #endregion automapper

        #region Event Work Flow Emision

        /// <summary>
        /// Creates the EventAuthorization by CompanyPolicy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static EventAuthorization CreateCompanyEventAuthorizationEmision(CompanyPolicy policy, int userId)
        {
            EventAuthorization eventAuthorization = new EventAuthorization();
            try
            {

                eventAuthorization.OPERATION1_ID = policy.Endorsement.TicketNumber.ToString();
                eventAuthorization.OPERATION2_ID = policy.Endorsement.Id.ToString();
                eventAuthorization.EVENT_ID = (int)UnderwritingServices.Enums.EventTypes.Subscription;
                eventAuthorization.AUTHO_USER_ID = userId;
            }
            catch (System.Exception)
            {

            }
            return eventAuthorization;
        }

        #endregion

        #region Temporales

        public static DataTable GetDataTableTempRISKSurety(CompanyContract companyContract)
        {
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_SURETY");

            #region Columns
            dataTable.Columns.Add("INDIVIDUAL_ID", typeof(int));
            dataTable.Columns.Add("SURETY_CONTRACT_TYPE_CD", typeof(int));
            dataTable.Columns.Add("SURETY_CONTRACT_CATEGORIES_CD", typeof(int));
            dataTable.Columns.Add("BID_NUMBER", typeof(string));
            dataTable.Columns.Add("PROYECT_NAME", typeof(string));
            dataTable.Columns.Add("FUNDED_BY", typeof(string));
            dataTable.Columns.Add("CONTRACT_ADDRESS", typeof(string));
            dataTable.Columns.Add("CONTRACT_AMT", typeof(decimal));
            dataTable.Columns.Add("PILE_AMT", typeof(decimal));
            dataTable.Columns.Add("IS_FACULTATIVE", typeof(bool));
            dataTable.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            #endregion

            #region DataRows
            DataRow rows = dataTable.NewRow();

            rows["INDIVIDUAL_ID"] = companyContract.Contractor.IndividualId;
            rows["SURETY_CONTRACT_TYPE_CD"] = companyContract.ContractType.Id;
            if (companyContract.Class.Id != null && companyContract.Class.Id > 0)
                rows["SURETY_CONTRACT_CATEGORIES_CD"] = companyContract.Class.Id;
            else
                rows["SURETY_CONTRACT_CATEGORIES_CD"] = DBNull.Value;
            if (companyContract.SettledNumber != null)
            {
                rows["BID_NUMBER"] = companyContract.SettledNumber;
            }
            else
            {
                rows["BID_NUMBER"] = DBNull.Value;
            }

            rows["PROYECT_NAME"] = DBNull.Value;
            rows["FUNDED_BY"] = DBNull.Value;

            if (companyContract.Contractor != null && companyContract.Contractor.CompanyName.Address.Description == null)
            {
                rows["CONTRACT_ADDRESS"] = DBNull.Value;
            }
            else
            {
                rows["CONTRACT_ADDRESS"] = companyContract.Contractor.CompanyName.Address.Description;
            }

            rows["CONTRACT_AMT"] = companyContract.Value.Value;
            rows["PILE_AMT"] = Math.Truncate(companyContract.Aggregate);
            rows["IS_FACULTATIVE"] = false;
            rows["CUSTOMER_TYPE_CD"] = companyContract.Risk.Policy.Holder.CustomerType;

            #endregion

            dataTable.Rows.Add(rows);

            return dataTable;
        }

        public static DataTable GetDataTableTempSuretyAvailable(CompanyContract companyContract)
        {
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_SURETY_AVAILABLE");

            #region Columns

            dataTable.Columns.Add("INDIVIDUAL_HOLDER_ID", typeof(int));
            dataTable.Columns.Add("INDIVIDUAL_ID", typeof(int));
            dataTable.Columns.Add("PILE_AMT", typeof(decimal));
            dataTable.Columns.Add("OPERATING_QUOTA_AMT", typeof(decimal));
            dataTable.Columns.Add("AVAILABLE_AMT", typeof(decimal));


            #endregion

            #region DataRows
            if (companyContract.Risk.MainInsured.CustomerType != CustomerType.Prospect && companyContract.Risk.Policy.Holder.CustomerType != CustomerType.Prospect)
            {
                DataRow rows = dataTable.NewRow();

                rows["INDIVIDUAL_HOLDER_ID"] = companyContract.Risk.Policy.Holder.IndividualId;
                rows["INDIVIDUAL_ID"] = companyContract.Risk.MainInsured.IndividualId;
                rows["PILE_AMT"] = companyContract.Aggregate;
                rows["OPERATING_QUOTA_AMT"] = companyContract?.OperatingQuota?.Amount ?? 0;
                rows["AVAILABLE_AMT"] = Math.Round(companyContract?.Available ?? 0, 2);


                #endregion

                dataTable.Rows.Add(rows);
            }
            return dataTable;
        }
        public static DataTable GetDataTableTempSuretyContract(CompanyContract companyContract)
        {
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_SURETY_CONTRACT");

            #region Columns

            dataTable.Columns.Add("OBJECT_CONTRACT", typeof(string));

            #endregion

            #region DataRows
            DataRow rows = dataTable.NewRow();
            //se agrega validacion de obejto del contrato vacio por comptabilidad con R1
            if (companyContract.ContractObject != null)
            {
                rows["OBJECT_CONTRACT"] = companyContract.ContractObject.TextBody;
            }
            #endregion

            dataTable.Rows.Add(rows);

            return dataTable;
        }
        public static DataTable GetDataTableTempSuretyGuarantee(CompanyContract companyContract)
        {
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_SURETY_GUARANTEE");


            #region Columns

            dataTable.Columns.Add("GUARANTEE_ID", typeof(int));

            #endregion

            #region DataRows
            if (companyContract.Guarantees != null)
            {
                foreach (Sureties.Models.CiaRiskSuretyGuarantee guarantee in companyContract.Guarantees)
                {
                    DataRow rows = dataTable.NewRow();

                    rows["GUARANTEE_ID"] = guarantee.InsuredGuarantee.Id;
                    #endregion

                    dataTable.Rows.Add(rows);
                }
            }
            return dataTable;
        }
        public static DataTable GetDataTableTempSuretyPost(CompanyContract companyContract)
        {
            Core.Framework.DAF.Engine.IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new Core.Framework.DAF.Engine.DynamicPropertiesSerializer();

            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_SURETY_POST");

            #region Columns

            dataTable.Columns.Add("ISSUE_DATE", typeof(DateTime));
            dataTable.Columns.Add("CONTRACT_DATE", typeof(DateTime));
            dataTable.Columns.Add("DELIVERY_DATE", typeof(DateTime));
            dataTable.Columns.Add("USER_ID", typeof(int));

            #endregion

            #region DataRows

            DataRow rows = dataTable.NewRow();

            rows["ISSUE_DATE"] = (Convert.ToDateTime(companyContract.RiskSuretyPost?.IssueDate) < Convert.ToDateTime("01/01/1900")) ? Convert.ToDateTime("01/01/1900") : companyContract.RiskSuretyPost?.IssueDate ?? Convert.ToDateTime("01/01/1900");
            rows["CONTRACT_DATE"] = (Convert.ToDateTime(companyContract.RiskSuretyPost?.ContractDate) < Convert.ToDateTime("01/01/1900")) ? Convert.ToDateTime("01/01/1900") : companyContract.RiskSuretyPost?.ContractDate ?? Convert.ToDateTime("01/01/1900");
            rows["DELIVERY_DATE"] = (Convert.ToDateTime(companyContract.RiskSuretyPost?.IssueDate) < Convert.ToDateTime("01/01/1900")) ? Convert.ToDateTime("01/01/1900") : companyContract.RiskSuretyPost?.IssueDate ?? Convert.ToDateTime("01/01/1900");
            rows["USER_ID"] = companyContract.RiskSuretyPost?.UserId ?? 0;

            #endregion

            dataTable.Rows.Add(rows);

            return dataTable;
        }

        #endregion

        //#region Consulta de Poliza por Id
        //public static CompanyContract CreateVehicle(ISSEN.Risk risk, ISSEN.CoRisk coRisk, ISSEN.RiskSurety riskVehicle, ISSEN.CoRiskSurety coRiskVehicle, ISSEN.EndorsementRisk endorsementRisk)
        //{
        //    CompanyContract vehicle = new CompanyContract
        //    {
        //        Risk = new CompanyRisk
        //        {
        //            RiskId = risk.RiskId,
        //            Number = endorsementRisk.RiskNum,
        //            CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
        //            GroupCoverage = new GroupCoverage
        //            {
        //                Id = risk.CoverGroupId.Value,
        //                CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode
        //            },
        //            MainInsured = new CompanyIssuanceInsured
        //            {
        //                IndividualId = risk.InsuredId,
        //                CompanyName = new IssuanceCompanyName
        //                {
        //                    NameNum = risk.NameNum.GetValueOrDefault(),
        //                    Address = new IssuanceAddress
        //                    {
        //                        Id = risk.AddressId.GetValueOrDefault()
        //                    }
        //                }
        //            },
        //            Text = new CompanyText
        //            {
        //                TextBody = risk.ConditionText
        //            },
        //            // WorkerType = coRiskVehicle.WorkerType,

        //            //Description = riskVehicle.LicensePlate,

        //            RatingZone = new CompanyRatingZone
        //            {
        //                Id = risk.RatingZoneCode.Value
        //            },
        //            LimitRc = new CompanyLimitRc
        //            {
        //                Id = coRisk.LimitsRcCode.Value,
        //                LimitSum = coRisk.LimitRcSum.Value
        //            },
        //            IsPersisted = true,
        //            OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode,
        //            Policy = endorsementRisk != null && endorsementRisk.EndorsementId > 0 ? new CompanyPolicy
        //            {
        //                Endorsement = new UnderwritingServices.CompanyEndorsement
        //                {
        //                    Id = endorsementRisk.EndorsementId
        //                }
        //            } : null,
        //            Status = RiskStatusType.NotModified,
        //            DynamicProperties = new List<DynamicConcept>()
        //        },
        //        //ChassisSerial = riskVehicle.ChassisSerNo,
        //        //Color = new CompanyColor
        //        //{
        //        //    Id = riskVehicle.VehicleColorCode.Value
        //        //},
        //        //IsNew = riskVehicle.IsNew,
        //        //LicensePlate = riskVehicle.LicensePlate,
        //        //LoadTypeCode = riskVehicle.LoadTypeCode.GetValueOrDefault(),
        //        //Price = riskVehicle.VehiclePrice,
        //        //StandardVehiclePrice = riskVehicle.StdVehiclePrice.GetValueOrDefault(),
        //        //OriginalPrice = riskVehicle.VehiclePrice,
        //        //NewPrice = riskVehicle.NewVehiclePrice.Value,
        //        //EngineSerial = riskVehicle.EngineSerNo,
        //        //Year = riskVehicle.VehicleYear,
        //        //Use = new CompanyUse
        //        //{
        //        //    Id = riskVehicle.VehicleUseCode
        //        //},
        //        //Version = new VEMODCO.CompanyVersion
        //        //{
        //        //    Id = riskVehicle.VehicleVersionCode,
        //        //    Fuel = new CompanyFuel
        //        //    {
        //        //        Id = riskVehicle.VehicleFuelCode.GetValueOrDefault(0)
        //        //    },
        //        //    Type = new VEBMO.CompanyType
        //        //    {
        //        //        Id = riskVehicle.VehicleTypeCode
        //        //    },
        //        //    Body = new CompanyBody
        //        //    {
        //        //        Id = riskVehicle.VehicleBodyCode
        //        //    }
        //        //},
        //        //Make = new CompanyMake
        //        //{
        //        //    Id = riskVehicle.VehicleMakeCode
        //        //},
        //        //Model = new CompanyModel
        //        //{
        //        //    Id = riskVehicle.VehicleModelCode
        //        //},

        //        //Rate = coRiskVehicle.FlatRatePercentage.GetValueOrDefault(),
        //        //ServiceType = new CompanyServiceType()
        //        //{
        //        //    Id = coRiskVehicle.ServiceTypeCode ?? 0
        //        //},
        //    };
        //    if (risk?.DynamicProperties?.Count > 0)
        //    {
        //        foreach (DynamicProperty item in risk.DynamicProperties.Distinct().ToList())
        //        {
        //            if (item.Value != null)
        //            {
        //                DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
        //                DynamicConcept dynamicConcept = new DynamicConcept();
        //                dynamicConcept.EntityId = RuleConceptRisk.Id;
        //                dynamicConcept.Id = dynamicProperty.Id;
        //                dynamicConcept.Value = dynamicProperty.Value;
        //                if (vehicle.Risk.DynamicProperties != null && !vehicle.Risk.DynamicProperties.Exists(x => x.Id == dynamicConcept.Id))
        //                {
        //                    vehicle.Risk.DynamicProperties.Add(dynamicConcept);
        //                }
        //            }

        //        }
        //    }
        //    else
        //    {
        //        vehicle.Risk.DynamicProperties = DelegateService.underwritingService.GetDynamicConceptsByEndorsementIdRiskNumPolicyIdRiskId(endorsementRisk.EndorsementId, endorsementRisk.RiskNum, endorsementRisk.PolicyId, risk.RiskId);
        //    }


        //    return vehicle;
        //}

        //#endregion

    }

}
