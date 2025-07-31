using AutoMapper;
using Newtonsoft.Json;
using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.Quotation.Entities;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.EEProvider.ModelsMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CIAISS = Sistran.Company.Application.Issuance.Entities;
using COISSEN = Sistran.Company.Application.Issuance.Entities;
using COMMENT = Sistran.Core.Application.Common.Entities;
using CommonModel = Sistran.Core.Application.CommonService.Models;
using ENTI = Sistran.Core.Application.UniquePerson.Entities;
using EnumsUnCo = Sistran.Core.Application.UnderwritingServices.Enums;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using IssueCoreModel = Sistran.Core.Application.UnderwritingServices.Models;
using IssueModel = Sistran.Company.Application.UnderwritingServices.Models;
using ProductModel = Sistran.Core.Application.ProductServices.Models;
using PROEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
using UniqueModelCia = Sistran.Company.Application.UniquePersonServices.V1.Models;
using UPMOCO = Sistran.Core.Application.UniquePersonService.V1.Models;
using INTEN = Sistran.Core.Application.Integration.Entities;


namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers
{
    internal class ModelAssembler
    {
        public static Holder Holder { get; private set; }
        #region Policy

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="coPolicy">The co policy.</param>
        /// <param name="endorsement">The endorsement.</param>
        /// <param name="endorsementPayer">The endorsement payer.</param>
        /// <returns></returns> 
        public static CompanyPolicy CreateCompanyPolicy(CompanyPolicyMapper companyPolicyMapper)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyPolicy();
            var mapperPoliy = mapper.Map<CompanyPolicyMapper, CompanyPolicy>(companyPolicyMapper);
            mapperPoliy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
            {
                ParticipationPercentageOwn = companyPolicyMapper.policy.CoissuePercentage.Value
            });
            foreach (DynamicConcept item in mapperPoliy.DynamicProperties)
            {
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = item.Id;
                dynamicConcept.Value = item.Value;
                mapperPoliy.DynamicProperties.Add(dynamicConcept);
            }

            return mapperPoliy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="coPolicy"></param>
        /// <param name="endorsement"></param>
        /// <param name="endorsementPayer"></param>
        /// <returns></returns>
        public static CompanyPolicy CreateCompanyClaimPolicy(ClaimPolicyMapper claimPolicyMapper)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyClaimPolicy();
            var mapperPoliy = mapper.Map<ClaimPolicyMapper, CompanyPolicy>(claimPolicyMapper);
            mapperPoliy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
            {
                ParticipationPercentageOwn = claimPolicyMapper.policy.CoissuePercentage.Value
            });

            return mapperPoliy;
        }

        /// <summary>
        /// Creates the risks.
        /// </summary>
        /// <param name="companyRisks">The company risks.</param>
        /// <returns></returns>
        internal static List<Risk> CreateRisks(List<CompanyRisk> companyRisks)
        {
            List<Risk> risks = new List<Risk>();
            var immaper = CreateMapRisk();
            return immaper.Map<List<CompanyRisk>, List<Risk>>(companyRisks);
        }

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="facadeGeneral">The facade general.</param>
        /// <returns></returns>
        internal static CompanyPolicy CreateCompanyPolicy(CompanyPolicy companyPolicy, Rules.Facade facade)
        {
            companyPolicy.Endorsement.TemporalId = facade.GetConcept<int>(CompanyRuleConceptGeneral.TempId);
            companyPolicy.Endorsement.QuotationId = facade.GetConcept<int>(CompanyRuleConceptGeneral.QuotationId);
            companyPolicy.DocumentNumber = facade.GetConcept<int>(CompanyRuleConceptGeneral.DocumentNumber);
            companyPolicy.Prefix.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.PrefixCode);
            companyPolicy.Endorsement.EndorsementType = (EnumsUnCo.EndorsementType)facade.GetConcept<int>(CompanyRuleConceptGeneral.EndorsementTypeCode);
            companyPolicy.ExchangeRate.Currency.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.CurrencyCode);
            companyPolicy.UserId = facade.GetConcept<int>(CompanyRuleConceptGeneral.UserId);
            companyPolicy.ExchangeRate.SellAmount = facade.GetConcept<int>(CompanyRuleConceptGeneral.ExchangeRate);
            companyPolicy.IssueDate = facade.GetConcept<DateTime>(CompanyRuleConceptGeneral.IssueDate);
            companyPolicy.CurrentFrom = facade.GetConcept<DateTime>(CompanyRuleConceptGeneral.CurrentFrom);
            companyPolicy.CurrentTo = facade.GetConcept<DateTime>(CompanyRuleConceptGeneral.CurrentTo);
            companyPolicy.BeginDate = facade.GetConcept<DateTime>(CompanyRuleConceptGeneral.BeginDate);
            //companyPolicy.BillingDate = facade.GetConcept<DateTime>(CompanyRuleConceptGeneral.BillingDate);
            //companyPolicy.BillingGroup.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.BillingGroupCode);
            companyPolicy.Product.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.ProductId);
            companyPolicy.Endorsement.PolicyId = facade.GetConcept<int>(CompanyRuleConceptGeneral.PolicyId);
            companyPolicy.Endorsement.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.EndorsementId);
            //companyPolicy.Text.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.ConditionTextId);
            //companyPolicy.Text.TextBody = facade.GetConcept<string>(CompanyRuleConceptGeneral.ConditionText);
            //companyPolicy.Text.Observations = facade.GetConcept<string>(CompanyRuleConceptGeneral.ConditionTextObservations);
            companyPolicy.BusinessType = (EnumsUnCo.BusinessType)facade.GetConcept<int>(CompanyRuleConceptGeneral.BusinessTypeCode);
            companyPolicy.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.OperationId);
            companyPolicy.PolicyType.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.PolicyTypeCode);
            //companyPolicy.Request.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.RequestId);
            companyPolicy.Product.ScriptId = facade.GetConcept<int>(CompanyRuleConceptGeneral.ScriptId);
            companyPolicy.Product.RuleSetId = facade.GetConcept<int>(CompanyRuleConceptGeneral.RuleSetId);
            companyPolicy.Product.StandardCommissionPercentage = facade.GetConcept<int>(CompanyRuleConceptGeneral.StandardCommissionPercentage);
            companyPolicy.Product.IsGreen = facade.GetConcept<bool>(CompanyRuleConceptGeneral.IsGreen);
            //companyPolicy.PaymentPlan.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.PaymentScheduleId);
            companyPolicy.CalculateMinPremium = facade.GetConcept<bool>(CompanyRuleConceptGeneral.CalculateMinPremium);
            companyPolicy.Summary.RiskCount = facade.GetConcept<int>(CompanyRuleConceptGeneral.RisksQuantity);
            companyPolicy.Summary.AmountInsured = facade.GetConcept<int>(CompanyRuleConceptGeneral.AmountInsured);
            companyPolicy.Summary.Premium = facade.GetConcept<int>(CompanyRuleConceptGeneral.Premium);
            companyPolicy.Summary.Expenses = facade.GetConcept<int>(CompanyRuleConceptGeneral.Expenses);
            companyPolicy.Summary.Taxes = facade.GetConcept<int>(CompanyRuleConceptGeneral.Taxes);
            companyPolicy.Summary.FullPremium = facade.GetConcept<int>(CompanyRuleConceptGeneral.FullPremium);
            companyPolicy.Holder.IndividualId = facade.GetConcept<int>(CompanyRuleConceptGeneral.PolicyHolderId);
            companyPolicy.Holder.CustomerType = (CustomerType)facade.GetConcept<int>(CompanyRuleConceptGeneral.CustomerTypeCode);
            companyPolicy.Holder.IdentificationDocument.Number = facade.GetConcept<string>(CompanyRuleConceptGeneral.HolderIdentificationDocument);
            companyPolicy.Holder.CompanyName.Address.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.MailAddressId);
            //companyPolicy.holderAge = facade.GetConcept<int>(CompanyRuleConceptGeneral.HolderAge);
            companyPolicy.Holder.BirthDate = facade.GetConcept<DateTime>(CompanyRuleConceptGeneral.HolderBirthDate);
            companyPolicy.Holder.Gender = facade.GetConcept<string>(CompanyRuleConceptGeneral.HolderGender);
            companyPolicy.Branch.Id = facade.GetConcept<int>(CompanyRuleConceptGeneral.BranchCode);
            //companyPolicy.JustificationSarlaft.JustificationReasonCode = facade.GetConcept<int>(CompanyRuleConceptGeneral.JustificationSarlaft);





            /*
             * Estas tres propiedades no se asignaron por que no es posible asignar una lista
             */
            //facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentId, companyPolicy.Agencies.First(x => x.IsPrincipal).Agent.IndividualId);
            //facade.SetConcept(CompanyRuleConceptGeneral.PrimaryAgentCode, companyPolicy.Agencies.First(x => x.IsPrincipal).Code);
            //facade.SetConcept(CompanyRuleConceptGeneral.IsPrimary, companyPolicy.Agencies.First(x => x.IsPrincipal).IsPrincipal);
            //facade.SetConcept(CompanyRuleConceptGeneral.SalePointCode, companyPolicy.Branch.SalePoints.First().Id);



            return companyPolicy;
        }

        internal static CompanyRisk CreateCiaRisk(CompanyRisk risk, Rules.Facade facade)
        {
            if (facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode) > 0)
            {
                if (risk.RatingZone == null)
                {
                    risk.RatingZone = new CompanyRatingZone();
                }

                risk.RatingZone.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.RatingZoneCode);
            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId) > 0)
            {
                if (risk.GroupCoverage == null)
                {
                    risk.GroupCoverage = new GroupCoverage();
                }

                risk.GroupCoverage.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.CoverageGroupId);
            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode) > 0)
            {
                if (risk.LimitRc == null)
                {
                    risk.LimitRc = new CompanyLimitRc();
                }

                risk.LimitRc.Id = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcCode);
            }

            if (facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum) > 0)
            {
                if (risk.LimitRc == null)
                {
                    risk.LimitRc = new CompanyLimitRc();
                }

                risk.LimitRc.LimitSum = facade.GetConcept<int>(CompanyRuleConceptRisk.LimitsRcSum);
            }

            risk.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);

            return risk;
        }
        /// <summary>
        /// Creates the dynamic concepts.
        /// </summary>
        /// <param name="dynamicConceptValues">The dynamic concept values.</param>
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

        private static DynamicConcept CreateDynamicConcept(Rules.Concept concept)
        {
            var mapper = AutoMapperAssembler.CreateMapDynamicConcept();
            return mapper.Map<Rules.Concept, DynamicConcept>(concept);
        }

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="entityEndorsementOperation">The entity endorsement operation.</param>
        /// <param name="entityPolicy">The entity policy.</param>
        /// <param name="entityEndorsement">The entity endorsement.</param>
        /// <returns></returns>
        internal static CompanyPolicy CreateCompanyPolicyByEndorsement(ISSEN.EndorsementOperation entityEndorsementOperation, ISSEN.Policy entityPolicy, ISSEN.Endorsement entityEndorsement, ISSEN.TempPolicyControl tempPolicyControl)
        {
            CompanyPolicy companyPolicy = new CompanyPolicy();

            if (!string.IsNullOrEmpty(entityEndorsementOperation.Operation))
            {
                companyPolicy = JsonConvert.DeserializeObject<CompanyPolicy>(entityEndorsementOperation.Operation);
                companyPolicy.Id = 0;
                companyPolicy.DocumentNumber = entityPolicy.DocumentNumber;
                companyPolicy.Endorsement.Id = entityEndorsementOperation.EndorsementId;
                companyPolicy.Endorsement.TemporalId = 0;
                companyPolicy.Endorsement.Number = entityEndorsement.DocumentNum;
                companyPolicy.Endorsement.CurrentFrom = companyPolicy.CurrentFrom;
                companyPolicy.Endorsement.CurrentTo = companyPolicy.CurrentTo;
                companyPolicy.Endorsement.PolicyId = entityPolicy.PolicyId;
                companyPolicy.Endorsement.AppRelation = tempPolicyControl.AppVersionId; 
                companyPolicy.Endorsement.ExchangeRate = entityEndorsement.ExchangeRate;
                companyPolicy.PolicyOrigin = (EnumsUnCo.PolicyOrigin)tempPolicyControl.PolicyOrigin;
                companyPolicy.AppSourceR2 = tempPolicyControl.AppVersionId == (int)EnumsUnCo.AppSource.R2;
            }

            return companyPolicy;
        }

        /// <summary>
        /// Creates the policy.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        public static Policy CreatePolicy(CompanyPolicy companyPolicy)
        {
            //CreateMapPolicy();
            //return Mapper.Map<CompanyPolicy, Policy>(companyPolicy);
            return CreateCorePolicyByCompanyPolicy(companyPolicy);
        }

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public static CompanyPolicy CreateCompanyPolicy(Policy policy)
        {
            CompanyPolicy companyPolicy = new CompanyPolicy();
            var config = CreateMapCompanyPolicy();
            return config.Map(policy, companyPolicy);
        }

        /// <summary>
        /// Creates the risk.
        /// </summary>
        /// <param name="companyRisk">The company risk.</param>
        /// <returns></returns>
        public static Risk CreateRisk(CompanyRisk companyRisk)
        {

            var immaper = CreateMapRisk();
            return immaper.Map<CompanyRisk, Risk>(companyRisk);
        }

        /// <summary>
        /// Creates the company risk.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <returns></returns>
        public static CompanyRisk CreateCompanyRisk(Risk risk)
        {
            var immaper = CreateMapCompanyRisk();
            return immaper.Map<Risk, CompanyRisk>(risk);
        }
        #endregion

        #region companySurcharge
        public static CompanySurchargeComponent CreateCompanySurcharge(CompanySurchargeComponentMapper companySurchargeComponentMapper)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanySurcharge();
            return mapper.Map<CompanySurchargeComponentMapper, CompanySurchargeComponent>(companySurchargeComponentMapper);
        }

        #endregion

        #region Agency

        /// <summary>
        /// Creates the agency.
        /// </summary>
        /// <param name="policyAgent">The policy agent.</param>
        /// <returns></returns>
        public static IssuanceAgency CreateAgency(ISSEN.PolicyAgent policyAgent)
        {
            var mapper = AutoMapperAssembler.CreateMapAgency();
            return mapper.Map<ISSEN.PolicyAgent, IssuanceAgency>(policyAgent);
        }

        public static List<IssuanceAgency> CreateAgencies(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<ISSEN.PolicyAgent>().ToList();
            var mapper = AutoMapperAssembler.CreateMapAgency();
            return mapper.Map<List<ISSEN.PolicyAgent>, List<IssuanceAgency>>(objBusiness);
        }

        /// <summary>
        /// Creates the coverages.
        /// </summary>
        /// <param name="companyCoverages">The company coverages.</param>
        /// <returns></returns>
        internal static List<Coverage> CreateCoverages(List<CompanyCoverage> companyCoverages)
        {
            var config = CreateMapCoverage();
            return config.Map<List<CompanyCoverage>, List<Coverage>>(companyCoverages);
        }

        internal static Coverage CreateCoverage(CompanyCoverage companyCoverage)
        {
            var config = CreateMapCoverage();
            return config.Map<CompanyCoverage, Coverage>(companyCoverage);
        }

        internal static CompanyLimitRc CreateCompanyLimitRc(LimitRc limitRc)
        {
            var imapper = CreateMapCompanyLimitRc();
            return imapper.Map<LimitRc, CompanyLimitRc>(limitRc);
        }

        public static List<CompanyCoverage> CreateCompanyCoverages(BusinessCollection businessCollection)
        {
            ConcurrentBag<CompanyCoverage> coverages = new ConcurrentBag<CompanyCoverage>();
            TP.Parallel.ForEach(businessCollection.Cast<ISSEN.RiskCoverage>().ToList(), item =>
            {
                coverages.Add(ModelAssembler.CreateCompanyCoverage(item));
            });

            return coverages.ToList();
        }

        /// <summary>
        /// Creates the company coverages.
        /// </summary>
        /// <param name="coverages">The coverages.</param>
        /// <returns></returns>
        internal static List<CompanyCoverage> CreateCompanyCoverages(List<Coverage> coverages)
        {
            var config = CreateMapCompanyCoverage();
            return config.Map<List<Coverage>, List<CompanyCoverage>>(coverages);
        }

        internal static CompanyCoverage CreateCompanyCoverage(Coverage coverage)
        {
            var config = CreateMapCompanyCoverage();
            return config.Map<Coverage, CompanyCoverage>(coverage);
        }

        public static CompanyCoverage CreateCompanyCoverage(ISSEN.RiskCoverage riskCoverage)
        {
            List<Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept> dynamicProperties = new List<Core.Application.RulesScriptsServices.Models.DynamicConcept>();

            if (riskCoverage.DynamicProperties?.Count > 0)
            {
                TP.Parallel.ForEach(riskCoverage.DynamicProperties, item =>
                {
                    DynamicProperty itemDynamic = (DynamicProperty)item.Value;
                    Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept dynamicProperty = new Core.Application.RulesScriptsServices.Models.DynamicConcept();
                    dynamicProperty.Id = itemDynamic.Id;
                    dynamicProperty.Value = itemDynamic.Value;
                    dynamicProperties.Add(dynamicProperty);
                });
            }
            return new CompanyCoverage
            {
                Id = riskCoverage.CoverageId,
                RiskCoverageId = riskCoverage.RiskCoverId,
                IsDeclarative = riskCoverage.IsDeclarative,
                IsMinPremiumDeposit = riskCoverage.IsMinPremiumDeposit,
                FirstRiskType = (EnumsUnCo.FirstRiskType)riskCoverage.FirstRiskTypeCode,
                CalculationType = (Core.Services.UtilitiesServices.Enums.CalculationType)riskCoverage.CalculationTypeCode,
                DeclaredAmount = riskCoverage.DeclaredAmount,
                PremiumAmount = riskCoverage.PremiumAmount,
                LimitAmount = riskCoverage.LimitAmount,
                ExcessLimit = riskCoverage.LimitInExcess,
                LimitOccurrenceAmount = riskCoverage.LimitOccurrenceAmount,
                LimitClaimantAmount = riskCoverage.LimitClaimantAmount,
                AccumulatedPremiumAmount = riskCoverage.AccPremiumAmount,
                AccumulatedLimitAmount = riskCoverage.AccLimitAmount,
                AccumulatedSubLimitAmount = riskCoverage.AccSublimitAmount,
                CurrentFrom = (DateTime)riskCoverage.CurrentFrom,
                CurrentTo = riskCoverage.CurrentTo,
                CurrentFromOriginal = (DateTime)riskCoverage.CurrentFrom,
                CurrentToOriginal = (DateTime)riskCoverage.CurrentTo,
                Rate = riskCoverage.Rate,
                RateType = (RateType)riskCoverage.RateTypeCode,
                //Number = riskCoverage.CoverageNumber.Value,
                //CoverStatus = (CoverageStatusType)riskCoverage.CoverageStatusCode,
                FlatRatePorcentage = riskCoverage.FlatRatePercentage.HasValue ? riskCoverage.FlatRatePercentage.Value : 0,
                DynamicProperties = dynamicProperties,
                DepositPremiumPercent = (decimal)(riskCoverage?.PremiumAmtDepositPercent ?? 0)
            };
        }

        public static List<IssuanceCommission> CreateCommissions(List<ISSEN.CommissAgent> commissAgencies)
        {

            var mapper = AutoMapperAssembler.CreateMapCommission();
            return mapper.Map<List<ISSEN.CommissAgent>, List<IssuanceCommission>>(commissAgencies);
        }

        public static IssuanceCommission CreateCommission(ISSEN.CommissAgent commissAgent)
        {
            var config = AutoMapperAssembler.CreateMapCommission();
            return config.Map<ISSEN.CommissAgent, IssuanceCommission>(commissAgent);
        }

        #endregion

        #region PayerComponent

        public static PayerComponent CreatePayerComponent(ISSEN.PayerComp payercomp)
        {
            var config = AutoMapperAssembler.CreateMapPayerComponent();
            return config.Map<ISSEN.PayerComp, PayerComponent>(payercomp);
        }

        public static List<PayerComponent> CreatePayerComponents(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<ISSEN.PayerComp>().ToList();
            var mapper = AutoMapperAssembler.CreateMapPayerComponent();
            return mapper.Map<List<ISSEN.PayerComp>, List<PayerComponent>>(objBusiness);
        }

        #endregion

        #region TemporalPolicy

        public static CompanyPolicy CreateTemporalPolicy(TemporalPolicyMapper temporalPolicyMapper)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalPolicy();
            var mapperPoliy = mapper.Map<TemporalPolicyMapper, CompanyPolicy>(temporalPolicyMapper);
            mapperPoliy.CoInsuranceCompanies.Add(new CompanyIssuanceCoInsuranceCompany
            {
                ParticipationPercentageOwn = temporalPolicyMapper.tempSubscription.CoissuePercentage.Value
            });

            if (temporalPolicyMapper.tempSubscription.DocumentNumber.GetValueOrDefault() > 0)
            {
                mapperPoliy.DocumentNumber = temporalPolicyMapper.tempSubscription.DocumentNumber.Value;
                mapperPoliy.Endorsement.PolicyId = temporalPolicyMapper.tempSubscription.PolicyId.Value;
                mapperPoliy.Endorsement.Id = temporalPolicyMapper.tempSubscription.EndorsementId.Value;
                mapperPoliy.Endorsement.EndorsementReasonId = temporalPolicyMapper.tempSubscription.EndoReasonCode.GetValueOrDefault();
                if (temporalPolicyMapper.tempSubscription.RefEndorsementId.HasValue)
                {
                    mapperPoliy.Endorsement.ReferenceEndorsementId = temporalPolicyMapper.tempSubscription.RefEndorsementId.Value;
                }
            }

            List<Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept> dynamicProperties = new List<Core.Application.RulesScriptsServices.Models.DynamicConcept>();
            foreach (DynamicProperty item in temporalPolicyMapper.tempSubscription.DynamicProperties)
            {
                DynamicProperty itemDynamic = (DynamicProperty)item.Value;
                Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept dynamicProperty = new Core.Application.RulesScriptsServices.Models.DynamicConcept();
                dynamicProperty.Id = itemDynamic.Id;
                dynamicProperty.Value = itemDynamic.Value;
                dynamicProperties.Add(dynamicProperty);
            }
            mapperPoliy.DynamicProperties = dynamicProperties;


            return mapperPoliy;
        }

        #endregion

        #region TemporalAgency

        public static List<IssuanceAgency> CreateTemporalAgencies(BusinessCollection collectionAgencies, BusinessCollection collectionCommissions)
        {
            List<IssuanceAgency> agencies = new List<IssuanceAgency>();
            List<TMPEN.TempCommissAgent> commissions = collectionCommissions.Cast<TMPEN.TempCommissAgent>().ToList();

            foreach (TMPEN.TempSubscriptionAgent field in collectionAgencies)
            {
                IssuanceAgency agency = new IssuanceAgency();
                agency = ModelAssembler.CreateTemporalAgency(field);
                agency.Commissions = new List<IssuanceCommission>();
                foreach (TMPEN.TempCommissAgent item in commissions.Where(x => x.IndividualId == agency.Agent.IndividualId && x.AgentAgencyId == agency.Id).ToList())
                {
                    agency.Participation = item.AgentPartPercentage;
                    agency.Commissions.Add(ModelAssembler.CreateTemporalAgencyCommission(item));
                }

                agencies.Add(agency);
            }

            return agencies;
        }

        public static IssuanceAgency CreateTemporalAgency(TMPEN.TempSubscriptionAgent tempSubscriptionAgent)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalAgency();
            return mapper.Map<TMPEN.TempSubscriptionAgent, IssuanceAgency>(tempSubscriptionAgent);
        }

        public static IssuanceCommission CreateTemporalAgencyCommission(TMPEN.TempCommissAgent tempCommissAgent)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalAgencyCommission();
            return mapper.Map<TMPEN.TempCommissAgent, IssuanceCommission>(tempCommissAgent);
        }

        #endregion

        #region TemporalQuota

        public static List<Quota> CreateTemporalQuotas(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalQuota();
            return mapper.Map<List<TMPEN.TempPayerPayment>, List<Quota>>(businessCollection.Cast<TMPEN.TempPayerPayment>().ToList());

            //List<Quota> quotas = new List<Quota>();

            //foreach (TMPEN.TempPayerPayment item in businessCollection)
            //{
            //    quotas.Add(ModelAssembler.CreateTemporalQuota(item));
            //}

            //return quotas;
        }

        public static Quota CreateTemporalQuota(TMPEN.TempPayerPayment tempPayerPayment)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalQuota();
            return mapper.Map<TMPEN.TempPayerPayment, Quota>(tempPayerPayment);
        }

        #endregion

        #region TemporalPayerComponent

        public static List<PayerComponent> CreateTemporalPayerComponents(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<TMPEN.TempPayerComponent>().ToList();
            var mapper = AutoMapperAssembler.CreateMapTemporalPayerComponent();
            return mapper.Map<List<TMPEN.TempPayerComponent>, List<PayerComponent>>(objBusiness);
        }

        public static PayerComponent CreateTemporalPayerComponent(TMPEN.TempPayerComponent tempPayerComponent)
        {

            var mapper = AutoMapperAssembler.CreateMapTemporalPayerComponent();
            var payerComponent = mapper.Map<TMPEN.TempPayerComponent, PayerComponent>(tempPayerComponent);
            List<Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept> dynamicProperties = new List<Core.Application.RulesScriptsServices.Models.DynamicConcept>();
            foreach (DynamicProperty item in tempPayerComponent.DynamicProperties)
            {
                DynamicProperty itemDynamic = (DynamicProperty)item.Value;
                Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept dynamicProperty = new Core.Application.RulesScriptsServices.Models.DynamicConcept();
                dynamicProperty.Id = itemDynamic.Id;
                dynamicProperty.Value = itemDynamic.Value;
                dynamicProperties.Add(dynamicProperty);
            }
            payerComponent.DynamicProperties = dynamicProperties;
            return payerComponent;
        }

        #endregion

        #region TemporalClause

        public static List<Clause> CreateTemporalClauses(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalClause();
            return mapper.Map<List<TMPEN.TempClause>, List<Clause>>(businessCollection.Cast<TMPEN.TempClause>().ToList());

            //List<Clause> clauses = new List<Clause>();

            //foreach (TMPEN.TempClause field in businessCollection)
            //{
            //    clauses.Add(ModelAssembler.CreateTemporalClause(field));
            //}

            //return clauses;
        }

        public static Clause CreateTemporalClause(TMPEN.TempClause tempClause)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalClause();
            return mapper.Map<TMPEN.TempClause, Clause>(tempClause);
        }

        #endregion

        #region TemporalCoverage

        public static CompanyCoverage CreateTemporalCompanyCoverage(TMPEN.TempRiskCoverage tempRiskCoverage)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalCompanyCoverage();
            var temporalCompanyCoverage = mapper.Map<TMPEN.TempRiskCoverage, CompanyCoverage>(tempRiskCoverage);

            List<Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept> dynamicProperties = new List<Core.Application.RulesScriptsServices.Models.DynamicConcept>();

            TP.Parallel.ForEach(tempRiskCoverage.DynamicProperties, item =>
            {
                DynamicProperty itemDynamic = (DynamicProperty)item.Value;
                Sistran.Core.Application.RulesScriptsServices.Models.DynamicConcept dynamicProperty = new Core.Application.RulesScriptsServices.Models.DynamicConcept();
                dynamicProperty.Id = itemDynamic.Id;
                dynamicProperty.Value = itemDynamic.Value;
                dynamicProperties.Add(dynamicProperty);
            });
            temporalCompanyCoverage.DynamicProperties = dynamicProperties;
            return temporalCompanyCoverage;

        }

        public static List<CompanyCoverage> CreateTemporalCoverages(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<TMPEN.TempRiskCoverage>().ToList();
            var mapper = AutoMapperAssembler.CreateMapTemporalCompanyCoverage();
            return mapper.Map<List<TMPEN.TempRiskCoverage>, List<CompanyCoverage>>(objBusiness);
        }

        #endregion

        #region TemporalDeductible

        public static CompanyDeductible CreateTemporalDeductible(TMPEN.TempRiskCoverDeduct tempRiskCoverDeduct)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalDeductible();
            return mapper.Map<TMPEN.TempRiskCoverDeduct, CompanyDeductible>(tempRiskCoverDeduct);
        }

        #endregion

        #region SubLineBusiness
        public static CompanySubLineBusiness CreateSubLineBusiness(QUOEN.Coverage coverage)
        {
            var mapper = AutoMapperAssembler.CreateMapSubLineBusiness();
            return mapper.Map<QUOEN.Coverage, CompanySubLineBusiness>(coverage);
        }
        #endregion

        #region Coinsurance
        internal static List<CompanyAcceptCoInsuranceAgent> CreateCiaCoInsuranceAccepteds(BusinessCollection businessCollection)
        {
            List<CompanyAcceptCoInsuranceAgent> companyAcceptCoInsuranceAgents = new List<CompanyAcceptCoInsuranceAgent>();

            foreach (CIAISS.CiaCoinsuranceAccepted ciaCoinsuranceAccepted in businessCollection)
            {
                companyAcceptCoInsuranceAgents.Add(CreateCiaCoInsuranceAccepted(ciaCoinsuranceAccepted));
            }

            return companyAcceptCoInsuranceAgents;
        }

        public static CompanyAcceptCoInsuranceAgent CreateCiaCoInsuranceAccepted(CIAISS.CiaCoinsuranceAccepted ciaCoinsuranceAccepted)
        {
            return new CompanyAcceptCoInsuranceAgent
            {
                ParticipationPercentage = ciaCoinsuranceAccepted.PartCiaPercentage,
                Agent = new CompanyPolicyAgent
                {
                    AgentId = ciaCoinsuranceAccepted.AgentAgencyId,
                    IndividualId = ciaCoinsuranceAccepted.IndividualId,
                    Id = ciaCoinsuranceAccepted.AgentAgencyId
                }
            };
            //var mapper = AutoMapperAssembler.CreateMapCiaCoInsuranceAccepted();
            //return mapper.Map<CIAISS.CiaCoinsuranceAccepted, CompanyAcceptCoInsuranceAgent>(ciaCoinsuranceAccepted);
        }
        public static List<IssuanceCoInsuranceCompany> CreateCoInsurancesAssigneds(BusinessCollection businessCollection)
        {
            List<IssuanceCoInsuranceCompany> companyIssuanceCoInsuranceCompanies = new List<IssuanceCoInsuranceCompany>();

            foreach (ISSEN.CoinsuranceAssigned entityCoinsuranceAssigned in businessCollection)
            {
                companyIssuanceCoInsuranceCompanies.Add(CreateExchangeRate(entityCoinsuranceAssigned));
            }
            return companyIssuanceCoInsuranceCompanies;
        }

        private static IssuanceCoInsuranceCompany CreateExchangeRate(ISSEN.CoinsuranceAssigned entityCoinsuranceAssigned)
        {
            return new IssuanceCoInsuranceCompany
            {
                EndorsementNumber = Convert.ToString(entityCoinsuranceAssigned.EndorsementId),
                PolicyNumber = Convert.ToString(entityCoinsuranceAssigned.PolicyId),
                Id = entityCoinsuranceAssigned.InsuranceCompanyId,
                ParticipationPercentageOwn = entityCoinsuranceAssigned.PartCiaPercentage,
                ParticipationPercentage = entityCoinsuranceAssigned.PartCiaPercentage,
                ExpensesPercentage = entityCoinsuranceAssigned.ExpensesPercentage
            };
        }

        public static CompanyIssuanceCoInsuranceCompany CreateCoInsuranceAccepted(ISSEN.CoinsuranceAccepted coinsuranceAccepted)
        {
            var mapper = AutoMapperAssembler.CreateMapCoInsuranceAccepted();
            return mapper.Map<ISSEN.CoinsuranceAccepted, CompanyIssuanceCoInsuranceCompany>(coinsuranceAccepted);
        }
        public static List<CompanyIssuanceCoInsuranceCompany> CreateCoInsuranceAccepted(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<ISSEN.CoinsuranceAccepted>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCoInsuranceAccepted();
            return mapper.Map<List<ISSEN.CoinsuranceAccepted>, List<CompanyIssuanceCoInsuranceCompany>>(objBusiness);
        }

        public static List<CompanyIssuanceCoInsuranceCompany> CreateCoInsurancesAssigned(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<ISSEN.CoinsuranceAssigned>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCoInsurancesAssigned();
            return mapper.Map<List<ISSEN.CoinsuranceAssigned>, List<CompanyIssuanceCoInsuranceCompany>>(objBusiness);
        }
        #endregion

        #region Clauses
        public static Clause CreateClause(QUOEN.Clause clause)
        {
            var mapper = AutoMapperAssembler.CreateMapClauses();
            return mapper.Map<QUOEN.Clause, Clause>(clause);
        }

        public static List<Clause> CreateClauses(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<QUOEN.Clause>().ToList();
            var mapper = AutoMapperAssembler.CreateMapClauses();
            return mapper.Map<List<QUOEN.Clause>, List<Clause>>(objBusiness);
        }
        #endregion

        #region objeto del seguro
        internal static CompanyInsuredObject CreateCompanyInsuredObject(QUOEN.InsuredObject entityInsuredObject)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyInsuredObject();
            return mapper.Map<QUOEN.InsuredObject, CompanyInsuredObject>(entityInsuredObject);
        }

        internal static CompanyInsuredObject CreateCompanyInsuredObject(InsuredObject insuredObject)
        {
            if (insuredObject != null)
            {
                var mapper = CreateMapCompanyInsuredObject();
                return mapper.Map<InsuredObject, CompanyInsuredObject>(insuredObject);
            }
            else
            {
                return null;
            }

        }


        internal static List<CompanyInsuredObject> CreateCompanyInsuredObjects(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyInsuredObject();
            return mapper.Map<List<QUOEN.InsuredObject>, List<CompanyInsuredObject>>(businessCollection.Cast<QUOEN.InsuredObject>().ToList());

            //List<CompanyInsuredObject> companyInsuredObjects = new List<CompanyInsuredObject>();

            //foreach (QUOEN.InsuredObject entityInsuredObject in businessCollection)
            //{
            //    companyInsuredObjects.Add(CreateCompanyInsuredObject(entityInsuredObject));
            //}

            //return companyInsuredObjects;
        }

        /// <summary>
        /// Creates the company insured objects.
        /// </summary>
        /// <param name="insuredObjects">The insured objects.</param>
        /// <returns></returns>
        internal static List<CompanyInsuredObject> CreateCompanyInsuredObjects(List<InsuredObject> insuredObjects)
        {
            ConcurrentBag<CompanyInsuredObject> companyInsuredObjects = new ConcurrentBag<CompanyInsuredObject>();
            if (insuredObjects != null && insuredObjects.Count > 0)
            {
                TP.Parallel.ForEach(insuredObjects, insuredObject =>
                 {
                     companyInsuredObjects.Add(CreateCompanyInsuredObject(insuredObject));
                 });
            }
            return companyInsuredObjects.ToList();
        }
        #endregion objeto del seguro
        #region DetailTypes
        public static CoverDetailType CreateCoverDetailType(QUOEN.CoverDetailType detailType)
        {
            var mapper = AutoMapperAssembler.CreateMapCoverDetailType();
            return mapper.Map<QUOEN.CoverDetailType, CoverDetailType>(detailType);
        }

        public static List<CoverDetailType> CreateCoverDetailTypes(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapCoverDetailType();
            return mapper.Map<List<QUOEN.CoverDetailType>, List<CoverDetailType>>(businessCollection.Cast<QUOEN.CoverDetailType>().ToList());
            //List<CoverDetailType> coverDetailTypes = new List<CoverDetailType>();

            //foreach (QUOEN.CoverDetailType field in businessCollection)
            //{
            //    coverDetailTypes.Add(ModelAssembler.CreateCoverDetailType(field));
            //}

            //return coverDetailTypes;
        }
        #endregion

        #region poliza
        public static Policy CreateCorePolicyByCompanyPolicy(CompanyPolicy companyPolicy)
        {
            var config = CreateMapPolicy();
            return config.Map<CompanyPolicy, Policy>(companyPolicy);
        }

        #endregion poliza
        #region zona tarifacion
        /// <summary>
        /// Creates the company rating zone.
        /// </summary>
        /// <param name="ratingZone">The rating zone.</param>
        /// <returns></returns>
        public static CompanyRatingZone CreateCompanyRatingZone(RatingZone ratingZone)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyRatingZones();
            return mapper.Map<RatingZone, CompanyRatingZone>(ratingZone);
        }
        public static IssueModel.CiaRatingZoneBranch CreateCiaRatingZoneBranch(COMMENT.CiaRatingZoneBranch entityCiaRatingZoneBranch)
        {
            var mapper = AutoMapperAssembler.CreateMapCiaRatingZoneBranch();
            return mapper.Map<COMMENT.CiaRatingZoneBranch, IssueModel.CiaRatingZoneBranch>(entityCiaRatingZoneBranch);
        }



        public static List<IssueModel.CiaRatingZoneBranch> CreateCiaRatingZoneBranchs(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<COMMENT.CiaRatingZoneBranch>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCiaRatingZoneBranch();
            return mapper.Map<List<COMMENT.CiaRatingZoneBranch>, List<IssueModel.CiaRatingZoneBranch>>(objBusiness);
        }



        public static List<CompanyRatingZone> CreateCompanyRatingZones(List<RatingZone> ratingZones)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyRatingZones();
            return mapper.Map<List<RatingZone>, List<CompanyRatingZone>>(ratingZones);

            //List<CompanyRatingZone> companyRatingZones = new List<CompanyRatingZone>();
            //foreach (RatingZone item in ratingZones)
            //{
            //    companyRatingZones.Add(CreateCompanyRatingZone(item));
            //}

            //return companyRatingZones;
        }

        #endregion zona tarifacion
        #region personas
        #region beneficiarios
        /// <summary>
        /// Creates the company beneficiaries.
        /// </summary>
        /// <param name="beneficiaries">The beneficiaries.</param>
        /// <returns></returns>
        public static List<CompanyBeneficiary> CreateCompanyBeneficiaries(List<Beneficiary> beneficiaries)
        {
            var imapper = CreateMapCompanyBeneficiary();
            return imapper.Map<List<Beneficiary>, List<CompanyBeneficiary>>(beneficiaries);
        }


        public static List<CompanyBeneficiaryType> CreateCompanyBeneficiaryTypes(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<QUOEN.BeneficiaryType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCompanyBeneficiaryType();
            return mapper.Map<List<QUOEN.BeneficiaryType>, List<CompanyBeneficiaryType>>(objBusiness);
        }
        public static CompanyBeneficiaryType CreateCompanyBeneficiaryType(QUOEN.BeneficiaryType beneficiaryType)
        {
            var mapper = AutoMapperAssembler.CreateMapCompanyBeneficiaryType();
            return mapper.Map<QUOEN.BeneficiaryType, CompanyBeneficiaryType>(beneficiaryType);
        }
        #endregion beneficiarios
        #endregion personas
        #region PaymentSchedule

        /// <summary>
        /// Crear una Payment Schedule
        /// </summary>
        /// <param name="paymentScheduleEntity">The PaymentSchedule Entity.</param>
        /// <returns>PaymentSchedule</returns>
        public static CompanyPaymentSchedule CreatePaymentSchedule(Core.Application.Product.Entities.PaymentSchedule paymentScheduleEntity)
        {
            if (paymentScheduleEntity != null)
            {
                var mapper = AutoMapperAssembler.CreateMapPaymentSchedule();
                return mapper.Map<Core.Application.Product.Entities.PaymentSchedule, CompanyPaymentSchedule>(paymentScheduleEntity);
            }

            return null;
        }

        #endregion PaymentSchedule
        #region FirstPayComponent

        /// <summary>
        /// Crear una First Pay Component
        /// </summary>
        /// <param name="paymentScheduleEntity">The PaymentSchedule Entity.</param>
        /// <returns>PaymentSchedule</returns>
        public static Sistran.Company.Application.UnderwritingServices.Models.FirstPayComponent CreateFirstPayComponent(COISSEN.FirstPayComponent firstPayComponentEntity)
        {
            if (firstPayComponentEntity != null)
            {
                var mapper = AutoMapperAssembler.CreateMapFirstPayComponent();
                return mapper.Map<COISSEN.FirstPayComponent, Sistran.Company.Application.UnderwritingServices.Models.FirstPayComponent>(firstPayComponentEntity);
            }

            return null;
        }

        public static List<Sistran.Company.Application.UnderwritingServices.Models.FirstPayComponent> CreateFirstPayComponents(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapFirstPayComponent();
            return mapper.Map<List<COISSEN.FirstPayComponent>, List<Sistran.Company.Application.UnderwritingServices.Models.FirstPayComponent>>(businessCollection.Cast<COISSEN.FirstPayComponent>().ToList());

            //List<Sistran.Company.Application.UnderwritingServices.Models.FirstPayComponent> listFirstPayComponent = new List<Sistran.Company.Application.UnderwritingServices.Models.FirstPayComponent>();

            //foreach (COISSEN.FirstPayComponent field in businessCollection)
            //{
            //    listFirstPayComponent.Add(ModelAssembler.CreateFirstPayComponent(field));
            //}

            //return listFirstPayComponent;
        }

        #endregion FirstPayComponent
        #region Component

        public static Component CreateComponent(QUOEN.Component component)
        {
            var mapper = AutoMapperAssembler.CreateMapComponent();
            return mapper.Map<QUOEN.Component, Component>(component);
        }

        public static List<Component> CreateComponents(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<QUOEN.Component>().ToList();
            var mapper = AutoMapperAssembler.CreateMapComponent();
            return mapper.Map<List<QUOEN.Component>, List<Component>>(objBusiness);
        }
        #endregion
        #region Deductible

        public static Deductible CreateCoverageDeductible(ISSEN.RiskCoverDeduct riskCoverDeduct)
        {
            var mapper = AutoMapperAssembler.CreateMapCoverageDeductible();
            return mapper.Map<ISSEN.RiskCoverDeduct, Deductible>(riskCoverDeduct);
        }

        public static Deductible CreateDeductible(QUOEN.Deductible deductible)
        {
            var mapper = AutoMapperAssembler.CreateMapDeductible();
            return mapper.Map<QUOEN.Deductible, Deductible>(deductible);
        }

        public static List<Deductible> CreateDeductibles(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapDeductible();
            return mapper.Map<List<QUOEN.Deductible>, List<Deductible>>(businessCollection.Cast<QUOEN.Deductible>().ToList());

            //List<Deductible> deductibles = new List<Deductible>();

            //foreach (QUOEN.Deductible field in businessCollection)
            //{
            //    deductibles.Add(ModelAssembler.CreateDeductible(field));
            //}

            //return deductibles;
        }

        #endregion
        #region automapper
        #region mapper Cobertura
        /// <summary>
        /// Creates the map company coverage.
        /// </summary>
        public static IMapper CreateMapCompanyCoverage()
        {
            var config = MapperCache.GetMapper<Coverage, CompanyCoverage>(cfg =>
            {

                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
            });
            return config;

        }
        /// <summary>
        /// Creates the map coverage.
        /// </summary>
        public static IMapper CreateMapCoverage()
        {
            var config = MapperCache.GetMapper<CompanyCoverage, Coverage>(cfg =>
            {

                cfg.CreateMap<CompanyText, Text>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyDeductible, Deductible>();
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanySubLineBusiness, SubLineBusiness>();
                cfg.CreateMap<CompanyCoverage, Coverage>();
            });

            return config;
        }

        public static CiaCoverage CreateCompanyToPrvCoverage(CompanyPrvCoverage companyPrvCoverage)
        {
            return new CiaCoverage(companyPrvCoverage.CoverageId, companyPrvCoverage.CoverageNum)
            {
                CoverageId = companyPrvCoverage.CoverageId,
                CoverageNum = companyPrvCoverage.CoverageNum,
                IsPost = companyPrvCoverage.IsPost,
                BeginDate = companyPrvCoverage.BeginDate
            };
        }

        public static CompanyPrvCoverage CreatePrvCoverageToCompany(CiaCoverage entityCiaCoverage)
        {
            return new CompanyPrvCoverage
            {
                CoverageId = entityCiaCoverage.CoverageId,
                CoverageNum = entityCiaCoverage.CoverageNum,
                IsPost = entityCiaCoverage.IsPost,
                BeginDate = entityCiaCoverage.BeginDate
            };
        }
        #endregion mapper Cobertura
        #region Resumen
        public static IMapper CreateMapSummary()
        {
            var config = MapperCache.GetMapper<CompanySummary, Summary>(cfg =>
            {
                cfg.CreateMap<CompanyRisk, Risk>();
                cfg.CreateMap<CompanyCoverage, Coverage>(); ;
                cfg.CreateMap<CompanySubLineBusiness, SubLineBusiness>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanyDeductible, Deductible>();
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
                cfg.CreateMap<IssueModel.CompanyIssuanceInsured, IssueCoreModel.IssuanceInsured>();
                cfg.CreateMap<UniqueModelCia.CiaIndividualPaymentMethod, UPMOCO.IndividualPaymentMethod>();
                cfg.CreateMap<CompanyPolicy, Policy>();
                cfg.CreateMap<CompanyProduct, ProductModel.Product>();
                cfg.CreateMap<CompanyCoveredRisk, ProductModel.CoveredRisk>();
                cfg.CreateMap<CompanyPrefix, Prefix>();
                cfg.CreateMap<CompanySummary, Summary>();
                cfg.CreateMap<CompanyPrefixType, PrefixType>();
                cfg.CreateMap<CompanyPayerComponent, PayerComponent>();
                cfg.CreateMap<CompanyBillingGroup, BillingGroup>();
                cfg.CreateMap<CompanyBranch, Branch>();
                cfg.CreateMap<CompanySalesPoint, SalePoint>();
                cfg.CreateMap<CompanyPolicyType, PolicyType>();
                cfg.CreateMap<CompanyEndorsement, Endorsement>();
                cfg.CreateMap<CompanyBeneficiary, Beneficiary>();
                cfg.CreateMap<CompanyBeneficiaryType, BeneficiaryType>();
                cfg.CreateMap<CompanyLimitRc, LimitRc>();
                cfg.CreateMap<CompanyRiskActivity, RiskActivity>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyRatingZone, RatingZone>();
                cfg.CreateMap<CompanyText, Text>();
                cfg.CreateMap<CompanySummary, Summary>();
            });
            return config;
        }
        public static IMapper CreateMapCompanySummary()
        {
            var config = MapperCache.GetMapper<Summary, CompanySummary>(cfg =>
            {
                cfg.CreateMap<Risk, CompanyRisk>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<IssueCoreModel.InsuredObject, IssueModel.CompanyIssuanceInsured>();
                cfg.CreateMap<UPMOCO.IndividualPaymentMethod, UniqueModelCia.CiaIndividualPaymentMethod>();
                cfg.CreateMap<Policy, CompanyPolicy>();
                cfg.CreateMap<ProductModel.Product, CompanyProduct>();
                cfg.CreateMap<ProductModel.CoveredRisk, CompanyCoveredRisk>();
                cfg.CreateMap<Prefix, CompanyPrefix>();
                cfg.CreateMap<Summary, CompanySummary>();
                cfg.CreateMap<PrefixType, CompanyPrefixType>();
                cfg.CreateMap<PayerComponent, CompanyPayerComponent>();
                cfg.CreateMap<BillingGroup, CompanyBillingGroup>();
                cfg.CreateMap<Branch, CompanyBranch>();
                cfg.CreateMap<SalePoint, CompanySalesPoint>();
                cfg.CreateMap<PolicyType, CompanyPolicyType>();
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<BeneficiaryType, CompanyBeneficiaryType>();
                cfg.CreateMap<LimitRc, CompanyLimitRc>();
                cfg.CreateMap<RiskActivity, CompanyRiskActivity>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<RatingZone, CompanyRatingZone>();
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Summary, CompanySummary>();
            });
            return config;
        }

        #endregion Resumen

        #region mapper Riesgo
        /// <summary>
        /// Creates the map risk.
        /// </summary>
        public static IMapper CreateMapRisk()
        {
            var config = MapperCache.GetMapper<CompanyRisk, Risk>(cfg =>
            {
                cfg.CreateMap<CompanyRisk, Risk>();
                cfg.CreateMap<CompanyCoverage, Coverage>(); ;
                cfg.CreateMap<CompanySubLineBusiness, SubLineBusiness>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanyDeductible, Deductible>();
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
                cfg.CreateMap<CompanyIssuanceInsured, IssuanceInsured>();
                cfg.CreateMap<UniqueModelCia.CiaIndividualPaymentMethod, UPMOCO.IndividualPaymentMethod>();
                #region poliza
                cfg.CreateMap<CompanyPolicy, Policy>();
                cfg.CreateMap<CompanyProduct, ProductModel.Product>();
                cfg.CreateMap<CompanyCoveredRisk, ProductModel.CoveredRisk>();
                cfg.CreateMap<CompanyPrefix, Prefix>();
                cfg.CreateMap<CompanySummary, Summary>();
                cfg.CreateMap<CompanyPrefixType, PrefixType>();
                cfg.CreateMap<CompanyPayerComponent, PayerComponent>();
                cfg.CreateMap<CompanyBillingGroup, BillingGroup>();
                cfg.CreateMap<CompanyBranch, Branch>();
                cfg.CreateMap<CompanySalesPoint, SalePoint>();
                cfg.CreateMap<CompanyPolicyType, PolicyType>();
                cfg.CreateMap<CompanyEndorsement, Endorsement>();
                #endregion poliza
                cfg.CreateMap<CompanyBeneficiary, Beneficiary>();
                cfg.CreateMap<CompanyBeneficiaryType, BeneficiaryType>();
                cfg.CreateMap<CompanyLimitRc, LimitRc>();
                cfg.CreateMap<CompanyRiskActivity, RiskActivity>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyRatingZone, RatingZone>();
                cfg.CreateMap<CompanyText, Text>();
            });
            return config;
        }


        /// <summary>
        /// Creates the map insured.
        /// </summary>
        public static IMapper CreateMapInsured()
        {
            var config = MapperCache.GetMapper<CompanyIssuanceInsured, IssuanceInsured>(cfg =>
            {

                cfg.CreateMap<IssueModel.CompanyIssuanceInsured, IssueCoreModel.IssuanceInsured>();
                cfg.CreateMap<UniqueModelCia.CiaIndividualPaymentMethod, UPMOCO.IndividualPaymentMethod>();
            });
            return config;
        }
        /// <summary>
        /// Creates the map insured.
        /// </summary>
        public static IMapper CreateMapIssueCompanyInsured()
        {
            var config = MapperCache.GetMapper<IssuanceInsured, CompanyIssuanceInsured>(cfg =>
            {

                cfg.CreateMap<IssueCoreModel.IssuanceInsured, IssueModel.CompanyIssuanceInsured>();
                cfg.CreateMap<UPMOCO.IndividualPaymentMethod, UniqueModelCia.CiaIndividualPaymentMethod>();
            });
            return config;
        }

        /// <summary>
        /// Creates the map company insured.
        /// </summary>
        public static IMapper CreateMapCompanyInsured()
        {
            var config = MapperCache.GetMapper<InsuredObject, CompanyIssuanceInsured>(cfg =>
            {

                cfg.CreateMap<IssueCoreModel.InsuredObject, IssueModel.CompanyIssuanceInsured>();
                cfg.CreateMap<UPMOCO.IndividualPaymentMethod, UniqueModelCia.CiaIndividualPaymentMethod>();
            });
            return config;
        }
        /// <summary>
        /// Creates the map company risk.
        /// </summary>
        public static IMapper CreateMapCompanyRisk()
        {
            var config = MapperCache.GetMapper<Risk, CompanyRisk>(cfg =>
            {
                cfg.CreateMap<Risk, CompanyRisk>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<IssuanceInsured, CompanyIssuanceInsured>();
                cfg.CreateMap<UPMOCO.IndividualPaymentMethod, UniqueModelCia.CiaIndividualPaymentMethod>();
                #region poliza
                cfg.CreateMap<Policy, CompanyPolicy>();
                cfg.CreateMap<ProductModel.Product, CompanyProduct>();
                cfg.CreateMap<ProductModel.CoveredRisk, CompanyCoveredRisk>();
                cfg.CreateMap<Prefix, CompanyPrefix>();
                cfg.CreateMap<Summary, CompanySummary>();
                cfg.CreateMap<PrefixType, CompanyPrefixType>();
                cfg.CreateMap<PayerComponent, CompanyPayerComponent>();
                cfg.CreateMap<BillingGroup, CompanyBillingGroup>();
                cfg.CreateMap<Branch, CompanyBranch>();
                cfg.CreateMap<SalePoint, CompanySalesPoint>();
                cfg.CreateMap<PolicyType, CompanyPolicyType>();
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
                #endregion
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<BeneficiaryType, CompanyBeneficiaryType>();
                cfg.CreateMap<LimitRc, CompanyLimitRc>();
                cfg.CreateMap<RiskActivity, CompanyRiskActivity>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<RatingZone, CompanyRatingZone>();
                cfg.CreateMap<Text, CompanyText>();
            });
            return config;
        }
        #endregion mapper Riesgo
        #region mapper poliza
        /// <summary>
        /// Creates the map policy.
        /// </summary>
        public static IMapper CreateMapPolicy()
        {
            var config = MapperCache.GetMapper<CompanyPolicy, Policy>(cfg =>
            {
                cfg.CreateMap<CompanyProduct, ProductModel.Product>();
                cfg.CreateMap<CompanyCoveredRisk, ProductModel.CoveredRisk>();
                cfg.CreateMap<CompanySummary, Summary>();
                cfg.CreateMap<CompanyPayerComponent, PayerComponent>();
                cfg.CreateMap<CompanyBillingGroup, BillingGroup>();
                cfg.CreateMap<CompanyPolicyType, PolicyType>();
                cfg.CreateMap<CompanyPaymentPlan, PaymentPlan>();
                cfg.CreateMap<CompanyBranch, Branch>();
                cfg.CreateMap<CompanyPrefix, Prefix>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyText, Text>();
                cfg.CreateMap<CompanyEndorsement, Endorsement>();
                cfg.CreateMap<CompanyComponent, Component>();
                cfg.CreateMap<CompanyCoverage, Coverage>();
                cfg.CreateMap<CompanyBeneficiary, Beneficiary>();
                cfg.CreateMap<CompanySalesPoint, CommonModel.SalePoint>();
                cfg.CreateMap<CompanySubLineBusiness, SubLineBusiness>();
                cfg.CreateMap<CompanyPolicy, Policy>();
                cfg.CreateMap<CompanyText, Text>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyDeductible, Deductible>();
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanySubLineBusiness, SubLineBusiness>();
                cfg.CreateMap<CompanyCoverage, Coverage>();
                cfg.CreateMap<CompanyPaymentPlan, PaymentPlan>();
            });
            return config;

        }

        public static IMapper CreateMapPolicyProduct()
        {
            var config = MapperCache.GetMapper<CompanyProduct, ProductModel.Product>(cfg =>
            {
                cfg.CreateMap<CompanyProduct, ProductModel.Product>();
                cfg.CreateMap<CompanyCoveredRisk, ProductModel.CoveredRisk>();
            });
            return config;
        }
        /// <summary>
        /// Creates the map company policy.
        /// </summary>
        public static IMapper CreateMapCompanyPolicy()
        {
            var config = MapperCache.GetMapper<Policy, CompanyPolicy>(cfg =>
            {
                cfg.CreateMap<ProductModel.Product, CompanyProduct>();
                cfg.CreateMap<ProductModel.CoveredRisk, CompanyCoveredRisk>();
                cfg.CreateMap<Prefix, CompanyPrefix>();
                cfg.CreateMap<Summary, CompanySummary>();
                cfg.CreateMap<PayerComponent, CompanyPayerComponent>();
                cfg.CreateMap<CompanyComponent, Component>();
                cfg.CreateMap<CompanyCoverage, Coverage>();
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<BillingGroup, CompanyBillingGroup>();
                cfg.CreateMap<PolicyType, CompanyPolicyType>();
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
                cfg.CreateMap<Branch, CompanyBranch>();
                cfg.CreateMap<CommonModel.SalePoint, CompanySalesPoint>();
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Policy, CompanyPolicy>();
                cfg.CreateMap<PaymentPlan, CompanyPaymentPlan>();
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
                cfg.CreateMap<IssuanceCoInsuranceCompany, CompanyIssuanceCoInsuranceCompany>();
            });
            return config;
        }
        public static IMapper CreateMapCompanyProduct()
        {
            var config = MapperCache.GetMapper<ProductModel.Product, CompanyProduct>(cfg =>
            {
                cfg.CreateMap<ProductModel.Product, CompanyProduct>();
                cfg.CreateMap<ProductModel.CoveredRisk, CompanyCoveredRisk>();
            });
            return config;
        }
        #endregion mapper poliza
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

        #endregion
        #region personas
        public static IMapper CreateMapBeneficiary()
        {
            var config = MapperCache.GetMapper<CompanyBeneficiary, Beneficiary>(cfg =>
            {
                cfg.CreateMap<CompanyBeneficiary, Beneficiary>();
                cfg.CreateMap<CompanyBeneficiaryType, BeneficiaryType>();
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
        #endregion personas
        #region Planes de pago

        public static IMapper CreateMapPaymentPlan()
        {
            var config = MapperCache.GetMapper<CompanyPaymentPlan, PaymentPlan>(cfg =>
            {
                cfg.CreateMap<CompanyPaymentPlan, PaymentPlan>();
                cfg.CreateMap<Quota, Quota>();
            });
            return config;
        }

        public static IMapper CreateMapCiaPaymentPlan()
        {
            var config = MapperCache.GetMapper<PaymentPlan, CompanyPaymentPlan>(cfg =>
            {
                cfg.CreateMap<PaymentPlan, CompanyPaymentPlan>();
                cfg.CreateMap<Quota, Quota>();
            });
            return config;
        }
        #endregion  Planes de pago
        #region Prefix

        public static IMapper CreateCompanyPrefix()
        {
            var config = MapperCache.GetMapper<Prefix, CompanyPrefix>(cfg =>
            {
                cfg.CreateMap<Prefix, CompanyPrefix>();
                cfg.CreateMap<PrefixType, CompanyPrefixType>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
            });
            return config;
        }

        #endregion
        #region Branch

        public static IMapper CreateCompanyBranch()
        {
            var config = MapperCache.GetMapper<Branch, CompanyBranch>(cfg =>
            {
                cfg.CreateMap<Branch, CompanyBranch>();
                cfg.CreateMap<CommonModel.SalePoint, CompanySalesPoint>();
            });
            return config;
        }

        #endregion
        #region Product


        /// <summary>
        /// Creates the company product.
        /// </summary>
        /// <param name="coreProduct">The core product.</param>
        /// <returns></returns>
        public static IMapper CreateCompanyProduct()
        {
            var config = MapperCache.GetMapper<Sistran.Core.Application.ProductServices.Models.Product, CompanyProduct>(cfg =>
            {
                cfg.CreateMap<Sistran.Core.Application.ProductServices.Models.Product, CompanyProduct>();
                cfg.CreateMap<Sistran.Core.Application.ProductServices.Models.CoveredRisk, CompanyCoveredRisk>();
                cfg.CreateMap<Prefix, CompanyPrefix>();
            });
            return config;
        }

        #endregion
        #region insured object
        public static IMapper CreateMapCompanyInsuredObject()
        {
            var config = MapperCache.GetMapper<InsuredObject, CompanyInsuredObject>(cfg =>
            {
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
            });
            return config;
        }
        #endregion insured object

        #region MapperLimitRC
        public static IMapper CreateMapCompanyLimitRc()
        {
            var config = MapperCache.GetMapper<LimitRc, CompanyLimitRc>(cfg =>
            {
                cfg.CreateMap<LimitRc, CompanyLimitRc>();
            });
            return config;
        }

        #endregion MapperLimitRC

        #region Mapper Insured

        public static IMapper CreateMapCompanyInsureds()
        {
            var config = MapperCache.GetMapper<IssuanceInsured, CompanyIssuanceInsured>(cfg =>
            {
                cfg.CreateMap<CompanyIssuanceInsured, IssuanceInsured>();
                cfg.CreateMap<IssuanceInsured, CompanyIssuanceInsured>();
            });

            return config;
        }

        #endregion Mapper Insured

        #region Mapper GetListType

        public static IMapper CreateMapInsuredListType()
        {
            var config = MapperCache.GetMapper<IssuanceInsured, CompanyIssuanceInsured>(cfg =>
            {
                cfg.CreateMap<IssuanceInsured, CompanyIssuanceInsured>();
            });

            return config;
        }

        #endregion Mapper GetListType

        #endregion automaper


        #region VehicleType

        /* Company to Core */

        public static VehicleType CreateVehicleType(CompanyVehicleType companyVehicleType)
        {
            //return new VehicleType
            //{
            //    Id = companyVehicleType.Id,
            //    Description = companyVehicleType.Description,
            //    SmallDescription = companyVehicleType.SmallDescription,
            //    IsTruck = companyVehicleType.IsTruck,
            //    IsActive = companyVehicleType.IsActive,
            //    IsElectronicPolicy = companyVehicleType.IsElectronicPolicy,
            //    State = companyVehicleType.State,
            //    VehicleBodies = CreateVehicleBodies(companyVehicleType.VehicleBodies)
            //};

            var config = MapperCache.GetMapper<CompanyVehicleType, VehicleType>(cfg =>
            {
                cfg.CreateMap<CompanyVehicleType, VehicleType>();
            });

            return config.Map<CompanyVehicleType, VehicleType>(companyVehicleType);
        }

        public static List<VehicleType> CreateVehicleTypes(List<CompanyVehicleType> companyVehicleTypes)
        {
            List<VehicleType> vehicleTypes = new List<VehicleType>();

            foreach (CompanyVehicleType item in companyVehicleTypes)
            {
                vehicleTypes.Add(CreateVehicleType(item));
            }

            return vehicleTypes;
        }
        /* Company to Core */
        /* Core to Company */

        public static CompanyVehicleType CreateCompanyVehicleType(VehicleType vehicleType)
        {
            //return new CompanyVehicleType
            //{
            //    Id = vehicleType.Id,
            //    Description = vehicleType.Description,
            //    SmallDescription = vehicleType.SmallDescription,
            //    IsTruck = vehicleType.IsTruck,
            //    IsActive = vehicleType.IsActive,
            //    IsElectronicPolicy = vehicleType.IsElectronicPolicy,
            //    State = vehicleType.State,
            //    VehicleBodies = CreateCompanyVehicleBodies(vehicleType.VehicleBodies)
            //};

            var config = MapperCache.GetMapper<VehicleType, CompanyVehicleType>(cfg =>
            {
                cfg.CreateMap<VehicleType, CompanyVehicleType>();
            });

            return config.Map<VehicleType, CompanyVehicleType>(vehicleType);
        }

        public static List<CompanyVehicleType> CreateCompanyVehicleTypes(List<VehicleType> vehicleTypes)
        {
            List<CompanyVehicleType> companyVehicleTypes = new List<CompanyVehicleType>();

            foreach (VehicleType item in vehicleTypes)
            {
                companyVehicleTypes.Add(CreateCompanyVehicleType(item));
            }

            return companyVehicleTypes;
        }

        /* Core to Company */


        #endregion

        #region VehicleBody
        /* Company to Core */
        public static VehicleBody CreateVehicleBody(CompanyVehicleBody companyVehicleBody)
        {
            //return new VehicleBody
            //{
            //    Id = companyVehicleBody.Id,
            //    SmallDescription = companyVehicleBody.SmallDescription,
            //    State = companyVehicleBody.State,
            //    VehicleUses = CreateVehicleUses(companyVehicleBody.VehicleUses)
            //};
            var config = MapperCache.GetMapper<CompanyVehicleBody, VehicleBody>(cfg =>
            {
                cfg.CreateMap<CompanyVehicleBody, VehicleBody>();
            });
            return config.Map<CompanyVehicleBody, VehicleBody>(companyVehicleBody);
        }

        public static List<VehicleBody> CreateVehicleBodies(List<CompanyVehicleBody> companyVehicleBodies)
        {
            List<VehicleBody> vehicleBodies = new List<VehicleBody>();

            foreach (CompanyVehicleBody item in companyVehicleBodies)
            {
                vehicleBodies.Add(CreateVehicleBody(item));
            }

            return vehicleBodies;
        }
        /* Company to Core */

        /* Core to Company */

        public static CompanyVehicleBody CreateCompanyVehicleBody(VehicleBody vehicleBody)
        {
            //return new CompanyVehicleBody
            //{
            //    Id = vehicleBody.Id,
            //    SmallDescription = vehicleBody.SmallDescription,
            //    State = vehicleBody.State,
            //    VehicleUses = CreateCompanyVehicleUses(vehicleBody.VehicleUses)
            //};

            var config = MapperCache.GetMapper<VehicleBody, CompanyVehicleBody>(cfg =>
            {
                cfg.CreateMap<VehicleBody, CompanyVehicleBody>();
            });
            return config.Map<VehicleBody, CompanyVehicleBody>(vehicleBody);
        }

        public static List<CompanyVehicleBody> CreateCompanyVehicleBodies(List<VehicleBody> vehicleBodies)
        {
            List<CompanyVehicleBody> companyVehicleBodies = new List<CompanyVehicleBody>();

            foreach (VehicleBody item in vehicleBodies)
            {
                companyVehicleBodies.Add(CreateCompanyVehicleBody(item));
            }

            return companyVehicleBodies;
        }

        /* Core to Company */
        #endregion

        #region VehicleUse
        /* Company to Core */
        public static VehicleUse CreateVehicleUse(CompanyVehicleUse companyVehicleUse)
        {
            //return new VehicleUse
            //{
            //    Id = companyVehicleUse.Id,
            //    SmallDescription = companyVehicleUse.SmallDescription,
            //    PrefixId = companyVehicleUse.PrefixId
            //};

            var config = MapperCache.GetMapper<CompanyVehicleUse, VehicleUse>(cfg =>
            {
                cfg.CreateMap<CompanyVehicleUse, VehicleUse>();
            });

            return config.Map<CompanyVehicleUse, VehicleUse>(companyVehicleUse);
        }

        public static List<VehicleUse> CreateVehicleUses(List<CompanyVehicleUse> companyVehicleUses)
        {
            List<VehicleUse> vehicleUses = new List<VehicleUse>();

            foreach (CompanyVehicleUse item in companyVehicleUses)
            {
                vehicleUses.Add(CreateVehicleUse(item));
            }

            return vehicleUses;
        }
        /* Company to Core */

        /* Core to Company */

        public static CompanyVehicleUse CreateCompanyVehicleUse(VehicleUse vehicleUse)
        {
            //return new CompanyVehicleUse
            //{
            //    Id = vehicleUse.Id,
            //    SmallDescription = vehicleUse.SmallDescription,
            //    PrefixId = vehicleUse.PrefixId
            //};

            var config = MapperCache.GetMapper<VehicleUse, CompanyVehicleUse>(cfg =>
            {
                cfg.CreateMap<VehicleUse, CompanyVehicleUse>();
            });

            return config.Map<VehicleUse, CompanyVehicleUse>(vehicleUse);
        }

        public static List<CompanyVehicleUse> CreateCompanyVehicleUses(List<VehicleUse> vehicleUses)
        {
            List<CompanyVehicleUse> companyVehicleUses = new List<CompanyVehicleUse>();

            foreach (VehicleUse item in vehicleUses)
            {
                companyVehicleUses.Add(CreateCompanyVehicleUse(item));
            }

            return companyVehicleUses;
        }

        public static List<CompanyJustificationSarlaft> JustificationReasonList(BusinessCollection justificationReason)
        {
            List<CompanyJustificationSarlaft> companyJustificationList = new List<CompanyJustificationSarlaft>();

            foreach (JustificationReason item in justificationReason)
            {
                companyJustificationList.Add(CreateJustification(item));
            }

            return companyJustificationList;
        }

        public static CompanyJustificationSarlaft CreateJustification(JustificationReason justificationReason)
        {
            return new CompanyJustificationSarlaft()
            {
                Description = justificationReason.Description,
                Enabled = justificationReason.Enabled,
                JustificationReasonCode = justificationReason.JustificationReasonCode,
                SmallDescription = justificationReason.SmallDescription
            };
        }
        public static CompanyCoverageDeductible CreateCompanyCoverageDeductible(Deductible deductible)
        {
            if (deductible == null)
            {
                return null;
            };

            return new CompanyCoverageDeductible
            {
                Code = deductible.Id,
                Description = deductible.Description
            };
        }

        public static CompanyRiskVehicle CreateRiskVehicle(RiskVehicle entityRiskVehicle)
        {
            return new CompanyRiskVehicle
            {
                VehicleYear = entityRiskVehicle.VehicleYear,
                VehiclePrice = entityRiskVehicle.VehiclePrice,
                IsNew = entityRiskVehicle.IsNew,
                LicensePlate = entityRiskVehicle.LicensePlate,
                EngineNumber = entityRiskVehicle.EngineNumber,
                ChassisNumber = entityRiskVehicle.ChassisNumber,
                LoadTypeId = entityRiskVehicle.LoadTypeId,
                TrailersQuantity = entityRiskVehicle.TrailersQuantity,
                PassengersQuantity = entityRiskVehicle.PassengersQuantity,
                NewVehiclePrice = entityRiskVehicle.NewVehiclePrice,
                StandardVehiclePrice = entityRiskVehicle.StandardVehiclePrice,
                VehicleVersion = new VehicleVersion
                {
                    Id = entityRiskVehicle.VehicleVersion.Id
                },
                VehicleModel = new VehicleModel
                {
                    Id = entityRiskVehicle.VehicleModel.Id
                },
                VehicleMake = new VehicleMake
                {
                    Id = entityRiskVehicle.VehicleMake.Id
                },
                Risk = new Risk
                {
                    Id = entityRiskVehicle.Risk.Id
                },
                VehicleType = new VehicleType
                {
                    Id = entityRiskVehicle.VehicleType.Id
                },
                VehicleUse = new VehicleUse
                {
                    Id = entityRiskVehicle.VehicleUse.Id
                },
                VehicleBody = new VehicleBody
                {
                    Id = entityRiskVehicle.VehicleBody.Id
                },
                VehicleColor = new VehicleColor
                {
                    Id = entityRiskVehicle.VehicleColor.Id
                },
                VehicleFuel = new VehicleFuel
                {
                    Id = entityRiskVehicle.VehicleFuel.Id
                }
            };
        }
        internal static List<CompanyRiskVehicle> CreateRiskVehicles(List<RiskVehicle> businessCollection)
        {
            ConcurrentBag<CompanyRiskVehicle> riskVehicles = new ConcurrentBag<CompanyRiskVehicle>();
            TP.Parallel.ForEach(businessCollection.Cast<RiskVehicle>().ToList(), item =>
            {
                riskVehicles.Add(ModelAssembler.CreateRiskVehicle(item));
            });
            return riskVehicles.ToList();
        }
        public static Endorsement CreateEndorsementByTempSubscription(TMPEN.TempSubscription tempSubscription)
        {
            Endorsement endorsement = new Endorsement
            {
                Id = tempSubscription.EndorsementId.Value,
                PolicyId = tempSubscription.PolicyId.Value,
                EndorsementType = (EnumsUnCo.EndorsementType)tempSubscription.EndorsementTypeCode.Value
            };

            if (tempSubscription.OperationId.HasValue)
            {
                endorsement.TemporalId = tempSubscription.OperationId.Value;
            }
            else
            {
                endorsement.TemporalId = tempSubscription.TempId;
            }

            return endorsement;
        }

        public static CompanyPolicyControl CreatePolicyControl(ISSEN.TempPolicyControl tmpPolicyControl)
        {

            CompanyPolicyControl objPolicyControl = new CompanyPolicyControl()
            {
                EndorsementId = tmpPolicyControl.EndorsementId,
                AppSource = tmpPolicyControl.AppVersionId,
                PolicyOrigin = (EnumsUnCo.PolicyOrigin)tmpPolicyControl.PolicyOrigin,
                PolicyId = tmpPolicyControl.PolicyId,
                TempId = tmpPolicyControl.TempId
            };
            return objPolicyControl;

        }


        /* Core to Company */
        #endregion

        #region Holder details
        internal static IssuancePhone CreatePhone(ENTI.Phone entityPhone)
        {
            var mapper = AutoMapperAssembler.CreateMapPhone();
            return mapper.Map<ENTI.Phone, IssuancePhone>(entityPhone);
        }

        internal static IssuanceEmail CreateEmail(ENTI.Email entityEmail)
        {
            var mapper = AutoMapperAssembler.CreateMapEmail();
            return mapper.Map<ENTI.Email, IssuanceEmail>(entityEmail);
        }

        internal static IssuanceAddress CreateAddress(ENTI.Address entityAddress)
        {
            var mapper = AutoMapperAssembler.CreateMapAddress();
            return mapper.Map<ENTI.Address, IssuanceAddress>(entityAddress);
        }
        #endregion

        #region PaymentPlan
        public static CompanyPaymentPlan CreatePaymentPlan(PROEN.PaymentSchedule paymentShedule)
        {
            var mapper = AutoMapperAssembler.CreateMapPaymentPlan();
            return mapper.Map<PROEN.PaymentSchedule, CompanyPaymentPlan>(paymentShedule);
        }
        #endregion PaymentPlan
        #region PayerFinancialPremium
        public static CompanyPremiumFinance MappCompanyFinancialPremium(COISSEN.PayerFinancialPremium payerFinancialPremium)
        {
            var config = MapperCache.GetMapper<COISSEN.PayerFinancialPremium, CompanyPremiumFinance>(cfg =>
            {
                cfg.CreateMap<COISSEN.PayerFinancialPremium, CompanyPremiumFinance>();
            });

            return config.Map<COISSEN.PayerFinancialPremium, CompanyPremiumFinance>(payerFinancialPremium);
        }
        #endregion

        #region Endosos
        public static List<CompanyEndorsement> CreateMapCompanyEndorsement(List<Endorsement> endorsement)
        {
            var config = MapperCache.GetMapper<Endorsement, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
            });
            return config.Map<List<Endorsement>, List<CompanyEndorsement>>(endorsement);

        }
        #endregion
        #region Lista  endosos
        public static List<EndorsementCompanyDTO> CreateMapCompanyEndorsements(List<ISSEN.Endorsement> endorsement)
        {
            var config = AutoMapperAssembler.CreateMapEndorsementCompany();
            return config.Map<List<ISSEN.Endorsement>, List<EndorsementCompanyDTO>>(endorsement);
        }


        #endregion

        #region EndoChangeText

        internal static List<Endorsement> CreateMapCompanyEndorsements(BusinessCollection businessObjects)
        {
            List<Endorsement> endorsements = new List<Endorsement>();

            foreach (ISSEN.Endorsement entityEndorsement in businessObjects)
            {
                endorsements.Add(CreateCompanyEndorsement(entityEndorsement));
            }

            return endorsements;
        }
        internal static Endorsement CreateCompanyEndorsement(ISSEN.Endorsement entityEndorsement)
        {
            return new Endorsement
            {
                Description = entityEndorsement.Annotations,
                BeginDate = entityEndorsement.BeginDate,
                CapacityOfCode = Convert.ToInt32(entityEndorsement.CapacityOfCode),
                CommitDate = Convert.ToDateTime(entityEndorsement.CommitDate),
                Text = new Text
                {
                    TextBody = entityEndorsement.ConditionText
                },
                CurrentFrom = entityEndorsement.CurrentFrom,
                CurrentTo = Convert.ToDateTime(entityEndorsement.CurrentTo),
                Number = entityEndorsement.DocumentNum,
                EndorsementReasonId = Convert.ToInt32(entityEndorsement.EndoReasonCode),
                Id = entityEndorsement.EndorsementId,
                EndorsementType = (EnumsUnCo.EndorsementType)entityEndorsement.EndoTypeCode,
                ExchangeRate = entityEndorsement.ExchangeRate,
                IsMassive = entityEndorsement.IsMassive,
                IssueDate = entityEndorsement.IssueDate,
                PolicyId = entityEndorsement.PolicyId,
                PrintedDate = Convert.ToDateTime(entityEndorsement.PrintedDate),
                QuotationId = Convert.ToInt32(entityEndorsement.QuotationId),
                SubscriptionReqId = Convert.ToInt32(entityEndorsement.SubscriptionReqId),
                UserId = entityEndorsement.UserId
            };
        }
        internal static Endorsement CreateCompanyEndorsement(ISSEN.Endorsement entityEndorsement, ISSEN.Risk entityRisk)
        {
            return new Endorsement
            {
                Description = entityEndorsement.Annotations,
                BeginDate = entityEndorsement.BeginDate,
                CapacityOfCode = Convert.ToInt32(entityEndorsement.CapacityOfCode),
                CommitDate = Convert.ToDateTime(entityEndorsement.CommitDate),
                Text = new Text
                {
                    TextBody = entityEndorsement.ConditionText
                },
                CurrentFrom = entityEndorsement.CurrentFrom,
                CurrentTo = Convert.ToDateTime(entityEndorsement.CurrentTo),
                Number = entityEndorsement.DocumentNum,
                EndorsementReasonId = Convert.ToInt32(entityEndorsement.EndoReasonCode),
                Id = entityEndorsement.EndorsementId,
                EndorsementType = (EnumsUnCo.EndorsementType)entityEndorsement.EndoTypeCode,
                ExchangeRate = entityEndorsement.ExchangeRate,
                IsMassive = entityEndorsement.IsMassive,
                IssueDate = entityEndorsement.IssueDate,
                PolicyId = entityEndorsement.PolicyId,
                PrintedDate = Convert.ToDateTime(entityEndorsement.PrintedDate),
                QuotationId = Convert.ToInt32(entityEndorsement.QuotationId),
                SubscriptionReqId = Convert.ToInt32(entityEndorsement.SubscriptionReqId),
                UserId = entityEndorsement.UserId,
                Risk = CreateCompanyRisk(entityRisk)
            };
        }

        internal static List<RiskChangeText> CreateMapCompanyRisk(BusinessCollection businessObjects1)
        {
            List<RiskChangeText> risks = new List<RiskChangeText>();

            foreach (ISSEN.Risk entityRisk in businessObjects1)
            {
                risks.Add(CreateCompanyRisk(entityRisk));
            }

            return risks;
        }

        internal static RiskChangeText CreateCompanyRisk(ISSEN.Risk entityRisk)
        {
            if (entityRisk == null)
            {
                return null;
            }
            return new RiskChangeText()
            {
                RatingZoneCode = entityRisk.RatingZoneCode,
                AddressId = entityRisk.AddressId,
                ConditionText = entityRisk.ConditionText,
                CoveredRiskTypeCode = entityRisk.CoveredRiskTypeCode,
                CoverGroupId = entityRisk.CoverGroupId,
                InsuredId = entityRisk.InsuredId,
                IsFacultative = entityRisk.IsFacultative,
                NameNum = entityRisk.NameNum,
                PhoneId = entityRisk.PhoneId,
                RiskCommercialClassCode = entityRisk.RiskCommercialClassCode,
                RiskCommercialTypeCode = entityRisk.RiskCommercialTypeCode,
                RiskId = entityRisk.RiskId,
                SecondaryInsuredId = entityRisk.SecondaryInsuredId

            };
            #endregion
        }

        internal static EndoChangeText CreateEndoChangeText(ISSEN.EndoChangeText entityEndoChangeText)
        {
            return new EndoChangeText
            {
                Id = entityEndoChangeText.EndoChangeTextCode,
                endorsementId = entityEndoChangeText.EndorsementId,
                policiId = entityEndoChangeText.PolicyId,
                reason = entityEndoChangeText.Reason,
                riskId = entityEndoChangeText.RiskId,
                textNewPolicy = entityEndoChangeText.TextNewPolicy,
                textnewRisk = entityEndoChangeText.TextNewRisk,
                textOldPolicy = entityEndoChangeText.TextOldPolicy,
                textOldRisk = entityEndoChangeText.TextOldRisk,
                userId = entityEndoChangeText.UserId
            };
        }
        #region automaper
        internal static QuotaFilterDTO CreateMapFinancialQuotaPlan(Policy policy)
        {
            var imaper = AutoMapperAssembler.CreateMapFinancialQuotaPlan();
            return imaper.Map<Policy, QuotaFilterDTO>(policy);
        }
        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }
        #endregion  automaper

        public static void CreateCoEndoChangeTextControl(INTEN.IssCoEndoChangeTextControl entityCoIndividualControl)
        {
            
        }
    }
}