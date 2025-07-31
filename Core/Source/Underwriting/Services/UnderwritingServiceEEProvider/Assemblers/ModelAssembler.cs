using AutoMapper;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Distribution;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using ISSENT = Sistran.Core.Application.Issuance.Entities;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using ProductModel = Sistran.Core.Application.ProductServices.Models;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TAXMO = Sistran.Core.Application.TaxServices.Models;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using UPMO = Sistran.Core.Application.UniquePersonService.V1.Models;
using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
using UUE = Sistran.Core.Application.Utilities.Entities.Entity;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {
        #region  Clause

        public static Model.Clause CreateClause(QUOEN.Clause clause)
        {
            var config = AutoMapperAssembler.CreateMapClause();
            return config.Map<QUOEN.Clause, Model.Clause>(clause);
        }

        /// <summary>
        /// Creates the clauses.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Model.Clause> CreateClauses(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapClause();
            return mapper.Map<List<QUOEN.Clause>, List<Clause>>(businessCollection.Cast<QUOEN.Clause>().ToList());
        }

        #endregion

        #region Text

        public static Model.Text CreateText(QUOEN.ConditionText conditionText)
        {
            var config = AutoMapperAssembler.CreateMapTexts();
            return config.Map<QUOEN.ConditionText, Model.Text>(conditionText);
        }

        public static List<Model.Text> CreateTexts(BusinessCollection businessCollection)
        {
            var config = AutoMapperAssembler.CreateMapTexts();
            return config.Map<List<QUOEN.ConditionText>, List<Model.Text>>(businessCollection.Cast<QUOEN.ConditionText>().ToList());
        }



        public static List<GroupCoverage> CreateGroupCoverageByPrefixs(BusinessCollection businessCollection)
        {
            var config = AutoMapperAssembler.CreateMapGroupCoverageByPrefixs();
            return config.Map<List<PRODEN.ProductGroupCover>, List<Model.GroupCoverage>>(businessCollection.Cast<PRODEN.ProductGroupCover>().ToList());
        }

        #endregion


        #region Temporales

        public static Model.Policy CreateTempPolicy(TMPEN.TempSubscription tempSubscription)
        {
            var config = AutoMapperAssembler.CreateMapperTempPolicy();
            return config.Map<TMPEN.TempSubscription, Model.Policy>(tempSubscription);
        }


        public static List<Model.Policy> CreateTempPolicys(BusinessCollection businessCollection)
        {
            var config = AutoMapperAssembler.CreateMapperTempPolicy();
            return config.Map<List<TMPEN.TempSubscription>, List<Model.Policy>>(businessCollection.Cast<TMPEN.TempSubscription>().ToList());
        }

        #endregion

        #region BeneficiaryPersonsAndCompanies

        public static Model.Beneficiary CreateBeneficiaryPerson(Person person)
        {
            var config = AutoMapperAssembler.CreateMapBeneficiaryPerson();
            return config.Map<Person, Model.Beneficiary>(person);

        }

        public static Model.Beneficiary CreateBeneficiaryCompany(Company company)
        {
            var config = AutoMapperAssembler.CreateMapBenefeciaryCompany();
            return config.Map<Company, Model.Beneficiary>(company);
        }

        public static List<Model.Beneficiary> CreateBeneficiaryPersonsAndCompanies(BusinessCollection collectionPersons, BusinessCollection collectionCompanies)
        {
            List<Model.Beneficiary> beneficiaries = new List<Model.Beneficiary>();

            foreach (Person item in collectionPersons)
            {
                var config = AutoMapperAssembler.CreateMapBeneficiaryPerson();
                beneficiaries.Add(config.Map<Person, Model.Beneficiary>(item));
            }
            foreach (Company item in collectionCompanies)
            {
                var config = AutoMapperAssembler.CreateMapBenefeciaryCompany();
                beneficiaries.Add(config.Map<Company, Model.Beneficiary>(item));
            }

            return beneficiaries;
        }

        #endregion

        #region PaymentPlan

        public static Model.PaymentPlan CreatePaymentPlan(PRODEN.PaymentSchedule paymentShedule)
        {
            var config = AutoMapperAssembler.CreateMapPaymentPlan();
            var mapper = config.Map<PRODEN.PaymentSchedule, Model.PaymentPlan>(paymentShedule);
            return mapper;
        }

        public static List<Model.PaymentPlan> CreatePaymentPlans(BusinessCollection collection)
        {
            var config = AutoMapperAssembler.CreateMapPaymentPlan();
            return config.Map<List<PRODEN.PaymentSchedule>, List<Model.PaymentPlan>>(collection.Cast<PRODEN.PaymentSchedule>().ToList());
        }

        public static Model.FinancialPlan CreateFinancialPlan(PRODEN.FinancialPlan financialPlan)
        {
            var config = AutoMapperAssembler.CreateMapFinancialPlan();
            return config.Map<PRODEN.FinancialPlan, Model.FinancialPlan>(financialPlan);
        }

        public static List<Model.FinancialPlan> CreateFinancialPlans(BusinessCollection collection)
        {
            var config = AutoMapperAssembler.CreateMapFinancialPlan();
            return config.Map<List<PRODEN.FinancialPlan>, List<Model.FinancialPlan>>(collection.Cast<PRODEN.FinancialPlan>().ToList());
        }

        public static Model.PaymentDistribution CreatePaymentDistribution(PRODEN.PaymentDistribution paymentDistribution)
        {
            var config = AutoMapperAssembler.CreateMapPaymentDistribution();
            return config.Map<PRODEN.PaymentDistribution, Model.PaymentDistribution>(paymentDistribution);
        }

        public static List<Model.PaymentDistribution> CreatePaymentDistributions(BusinessCollection collection)
        {
            var config = AutoMapperAssembler.CreateMapPaymentDistribution();
            return config.Map<List<PRODEN.PaymentDistribution>, List<Model.PaymentDistribution>>(collection.Cast<PRODEN.PaymentDistribution>().ToList());
        }

        #endregion

        #region LimitRC

        public static Model.LimitRc CreateLimitRC(COMMEN.CoLimitsRc limitRC)
        {
            var config = AutoMapperAssembler.CreateMapLimitRC();
            return config.Map<COMMEN.CoLimitsRc, Model.LimitRc>(limitRC);
        }

        public static List<Model.LimitRc> CreateLimitsRC(BusinessCollection businessCollection)
        {
            var config = AutoMapperAssembler.CreateMapLimitRC();
            return config.Map<List<COMMEN.CoLimitsRc>, List<Model.LimitRc>>(businessCollection.Cast<COMMEN.CoLimitsRc>().ToList());
        }

        #endregion

        #region LimitRCRelation

        public static Model.LimitRCRelation CreateLimitRCRelation(COMMEN.CoLimitsRcRel limitRCRelation)
        {
            var config = AutoMapperAssembler.CreateMapLimitRCRelation();
            return config.Map<COMMEN.CoLimitsRcRel, Model.LimitRCRelation>(limitRCRelation);
        }

        public static List<Model.LimitRCRelation> CreateLimitRCRelations(BusinessCollection businessCollection)
        {
            var config = AutoMapperAssembler.CreateMapLimitRCRelation();
            return config.Map<List<COMMEN.CoLimitsRcRel>, List<Model.LimitRCRelation>>(businessCollection.Cast<COMMEN.CoLimitsRcRel>().ToList());
        }

        #endregion

        #region GroupCoverage

        public static Model.GroupCoverage CreateGroupCoverageByProduct(PRODEN.ProductGroupCover productGroupCover)
        {
            var config = AutoMapperAssembler.CreateMapGroupCoverageByProduct();
            return config.Map<PRODEN.ProductGroupCover, Model.GroupCoverage>(productGroupCover);
        }


        public static List<Model.GroupCoverage> CreateGroupCoveragesByProducts(BusinessCollection businessCollection)
        {
            var config = AutoMapperAssembler.CreateMapLimitRCRelation();
            return config.Map<List<PRODEN.ProductGroupCover>, List<Model.GroupCoverage>>(businessCollection.Cast<PRODEN.ProductGroupCover>().ToList());

            //List<Model.GroupCoverage> groupCoverages = new List<Model.GroupCoverage>();

            //foreach (PRODEN.ProductGroupCover field in businessCollection)
            //{
            //    groupCoverages.Add(ModelAssembler.CreateGroupCoverageByProduct(field));
            //}

            //return groupCoverages;
        }

        public static Model.GroupCoverage CreateGroupCoverageByCoverage(PRODEN.GroupCoverage groupCoverage)
        {
            var config = AutoMapperAssembler.CreateMapGroupCoverageByCoverage();
            return config.Map<PRODEN.GroupCoverage, Model.GroupCoverage>(groupCoverage);
        }

        public static List<Model.GroupCoverage> CreateGroupCoveragesByCoverages(BusinessCollection businessCollection)
        {
            var config = AutoMapperAssembler.CreateMapGroupCoverageByCoverage();
            return config.Map<List<PRODEN.GroupCoverage>, List<Model.GroupCoverage>>(businessCollection.Cast<PRODEN.GroupCoverage>().ToList());

        }

        public static Model.InsuredObject CreateInsuredObjectByGroupInsuredObject(QUOEN.InsuredObject insuredObjectEntity, List<QUOEN.InsuredObject> groupInsuredObjects)
        {
            var groupInsureds = groupInsuredObjects.Cast<PRODEN.GroupInsuredObject>().ToList();
            var config = AutoMapperAssembler.CreateMapInsuredObjectByGroupInsuredObject();
            var insuredObject = config.Map<QUOEN.InsuredObject, Model.InsuredObject>(insuredObjectEntity);
            if (groupInsureds.FirstOrDefault(n => n.InsuredObject == insuredObject.Id).InsuredObject == insuredObject.Id)
            {
                insuredObject.IsMandatory = groupInsureds.FirstOrDefault(n => n.InsuredObject == insuredObject.Id).IsMandatory;
                insuredObject.IsSelected = groupInsureds.FirstOrDefault(n => n.InsuredObject == insuredObject.Id).IsSelected;
            }
            return insuredObject;
            //Model.InsuredObject insuredObject = new Model.InsuredObject
            //{
            //    Id = insuredObjectEntity.InsuredObjectId,
            //    Description = insuredObjectEntity.Description,
            //    IsDeclarative = insuredObjectEntity.IsDeclarative,
            //};

            //foreach (PRODEN.GroupInsuredObject field in groupInsuredObjects)
            //{
            //    if (field.InsuredObject == insuredObjectEntity.InsuredObjectId)
            //    {
            //        insuredObject.IsMandatory = field.IsMandatory;
            //        insuredObject.IsSelected = field.IsSelected;
            //    }
            //}
            //return insuredObject;
        }

        public static List<Model.InsuredObject> CreateInsuredObjectByGroupInsuredObjects(BusinessCollection bussinessCollection, BusinessCollection groupInsuredObjects)
        {
            var groupInsureds = groupInsuredObjects.Cast<PRODEN.GroupInsuredObject>().ToList();
            var config = AutoMapperAssembler.CreateMapInsuredObjectByGroupInsuredObject();
            var insuredObjest = config.Map<List<QUOEN.InsuredObject>, List<Model.InsuredObject>>(bussinessCollection.Cast<QUOEN.InsuredObject>().ToList());
            insuredObjest.AsParallel().ForAll(a =>
            {
                if (groupInsureds.FirstOrDefault(n => n.InsuredObject == a.Id).InsuredObject == a.Id)
                {
                    a.IsMandatory = groupInsureds.FirstOrDefault(n => n.InsuredObject == a.Id).IsMandatory;
                    a.IsSelected = groupInsureds.FirstOrDefault(n => n.InsuredObject == a.Id).IsSelected;
                }
            });
            return insuredObjest;
            //List<Model.InsuredObject> insuredObjects = new List<Model.InsuredObject>();

            //foreach (QUOEN.InsuredObject field in bussinessCollection)
            //{
            //    insuredObjects.Add(ModelAssembler.CreateInsuredObjectByGroupInsuredObject(field, groupInsuredObjects));
            //}

            //return insuredObjects;
        }

        internal static List<PayerComponent> CreatePayerComponents(BusinessCollection businessCollection)
        {
            var config = AutoMapperAssembler.CreateMapPayerComponent();
            return config.Map<List<QUOEN.Component>, List<Model.PayerComponent>>(businessCollection.Cast<QUOEN.Component>().ToList());

        }

        private static PayerComponent CreatePayerComponent(QUOEN.Component entityComponent)
        {
            var config = AutoMapperAssembler.CreateMapPayerComponent();
            return config.Map<QUOEN.Component, Model.PayerComponent>(entityComponent);
        }


        #endregion

        #region Coverage

        /// <summary>
        /// Creates the coverage.
        /// </summary>
        /// <param name="coverage">The coverage.</param>
        /// <returns></returns>
        public static Model.Coverage CreateCoverage(QUOEN.Coverage coverage)
        {
            var mapper = AutoMapperAssembler.CreateMapCoverage();
            return mapper.Map<QUOEN.Coverage, Model.Coverage>(coverage);
        }

        /// <summary>
        /// Creates the coverages.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Model.Coverage> CreateCoverages(BusinessCollection businessCollection)
        {
            var coverages = businessCollection.Cast<QUOEN.Coverage>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCoverage();
            return mapper.Map<List<QUOEN.Coverage>, List<Model.Coverage>>(coverages);
        }

        public static Model.CoverDetailType CreateCoverDetailType(QUOEN.CoverDetailType coverDetailType)
        {
            var mapper = AutoMapperAssembler.CreateMapCoverDetailType();
            return mapper.Map<QUOEN.CoverDetailType, Model.CoverDetailType>(coverDetailType);
        }

        public static List<Model.CoverDetailType> CreateCoversDetailType(BusinessCollection businessCollection)
        {
            var coverages = businessCollection.Cast<QUOEN.CoverDetailType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCoverDetailType();
            return mapper.Map<List<QUOEN.CoverDetailType>, List<Model.CoverDetailType>>(coverages);
        }

        #endregion Coverage

        #region Component

        public static Model.Component CreateComponent(QUOEN.Component component)
        {
            var mapper = AutoMapperAssembler.CreateMapComponent();
            return mapper.Map<QUOEN.Component, Model.Component>(component);
        }

        public static List<Model.Component> CreateComponents(BusinessCollection businessCollection)
        {
            var component = businessCollection.Cast<QUOEN.Component>().ToList();
            var mapper = AutoMapperAssembler.CreateMapComponent();
            return mapper.Map<List<QUOEN.Component>, List<Model.Component>>(component);
        }

        #endregion

        #region CoveredRisk
        public static List<ProductModel.CoveredRisk> CreateCoveredRisks(BusinessCollection businessCollection)
        {
            var coveredRisk = businessCollection.Cast<PRODEN.ProductCoverRiskType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCoveredRisk();
            return mapper.Map<List<PRODEN.ProductCoverRiskType>, List<ProductModel.CoveredRisk>>(coveredRisk);
        }

        public static ProductModel.CoveredRisk CreateCoveredRisk(PRODEN.ProductCoverRiskType productCoverRiskType)
        {
            var mapper = AutoMapperAssembler.CreateMapCoveredRisk();
            return mapper.Map<PRODEN.ProductCoverRiskType, ProductModel.CoveredRisk>(productCoverRiskType);
        }

        #endregion

        #region InsuredObjects

        public static Model.InsuredObject CreateInsuredObject(QUOEN.InsuredObject insuredObject)
        {
            var mapper = AutoMapperAssembler.CreateMapInsuresObject();
            return mapper.Map<QUOEN.InsuredObject, Model.InsuredObject>(insuredObject);
        }

        public static List<Model.InsuredObject> CreateInsuredObjects(BusinessCollection businessCollection)
        {
            var insuredObjects = businessCollection.Cast<QUOEN.InsuredObject>().ToList();
            var mapper = AutoMapperAssembler.CreateMapInsuresObject();
            return mapper.Map<List<QUOEN.InsuredObject>, List<Model.InsuredObject>>(insuredObjects);
        }


        public static Model.InsuredObject CreateInsuredObject(ISSEN.RiskInsuredObject insuredObject)
        {
            var mapper = AutoMapperAssembler.CreateMapInsuredObject();
            return mapper.Map<ISSEN.RiskInsuredObject, Model.InsuredObject>(insuredObject);
        }

        public static List<Model.InsuredObject> CreateISSInsuredObjects(BusinessCollection businessCollection)
        {
            var insuredObjects = businessCollection.Cast<ISSEN.RiskInsuredObject>().ToList();
            var mapper = AutoMapperAssembler.CreateMapInsuredObject();
            return mapper.Map<List<ISSEN.RiskInsuredObject>, List<Model.InsuredObject>>(insuredObjects);
        }

        #endregion InsuredObjects

        #region InsObjLineBusiness
        public static Model.InsObjLineBusiness CreateInsObjLineBusiness(QUOEN.InsObjLineBusiness insObjLineBusiness)
        {
            var mapper = AutoMapperAssembler.CreateMapInsObjLineBusiness();
            return mapper.Map<QUOEN.InsObjLineBusiness, Model.InsObjLineBusiness>(insObjLineBusiness);
        }

        public static List<Model.InsObjLineBusiness> CreateInsObjLineBusiness(BusinessCollection businessCollection)
        {
            var objLineBusiness = businessCollection.Cast<QUOEN.InsObjLineBusiness>().ToList();
            var mapper = AutoMapperAssembler.CreateMapInsObjLineBusiness();
            return mapper.Map<List<QUOEN.InsObjLineBusiness>, List<Model.InsObjLineBusiness>>(objLineBusiness);
        }

        #endregion
        #region Deductible

        public static Model.Deductible CreateCoverageDeductible(ISSEN.RiskCoverDeduct riskCoverDeduct)
        {
            var mapper = AutoMapperAssembler.CreateMapCoverageDeductible();
            return mapper.Map<ISSEN.RiskCoverDeduct, Model.Deductible>(riskCoverDeduct);
        }

        public static Model.Deductible CreateDeductible(QUOEN.Deductible deductible)
        {
            var mapper = AutoMapperAssembler.CreateMapDeductible();
            return mapper.Map<QUOEN.Deductible, Model.Deductible>(deductible);
        }

        public static List<Model.Deductible> CreateDeductibles(BusinessCollection businessCollection)
        {
            var objDeductiblesBusiness = businessCollection.Cast<QUOEN.Deductible>().ToList();
            var mapper = AutoMapperAssembler.CreateMapDeductible();
            var objDeductibles = mapper.Map<List<QUOEN.Deductible>, List<Model.Deductible>>(objDeductiblesBusiness);
            int contador = 0;
            foreach (QUOEN.Deductible item in businessCollection)
            {
                objDeductibles[contador].RateType = (UTILEN.RateType)item.RateTypeCode;
                contador++;
            }
            return objDeductibles;
        }

        public static List<Model.Deductible> CreateDeductiblesByRiskCoverage(BusinessCollection businessCollection)
        {
            var objDeductiblesBusiness = businessCollection.Cast<ISSEN.RiskCoverDeduct>().ToList();
            var mapper = AutoMapperAssembler.CreateMapDeductibleByRiskCoverDeduct();
            var objDeductibles = mapper.Map<List<ISSEN.RiskCoverDeduct>, List<Model.Deductible>>(objDeductiblesBusiness);
            int contador = 0;
            foreach (ISSEN.RiskCoverDeduct item in businessCollection)
            {
                objDeductibles[contador].RateType = (UTILEN.RateType)item.RateTypeCode;
                contador++;
            }
            return objDeductibles;
        }

        #endregion

        #region BillingGroup
        public static Models.BillingGroup CreateBillingGroup(ISSEN.BillingGroup billingGroup)
        {
            var mapper = AutoMapperAssembler.CreateMapBillingGroup();
            return mapper.Map<ISSEN.BillingGroup, Model.BillingGroup>(billingGroup);

        }

        public static List<Model.BillingGroup> CreateBillingGroups(BusinessCollection collection)
        {
            var objBillingGroupsBusiness = collection.Cast<ISSEN.BillingGroup>().ToList();
            var mapper = AutoMapperAssembler.CreateMapBillingGroup();
            return mapper.Map<List<ISSEN.BillingGroup>, List<Model.BillingGroup>>(objBillingGroupsBusiness);
        }

        #endregion

        #region TemporalQuota

        public static List<Model.Quota> CreateTemporalQuotas(BusinessCollection businessCollection)
        {
            var objTemoralQuotasBusiness = businessCollection.Cast<ISSEN.PayerPayment>().ToList();
            var mapper = AutoMapperAssembler.CreateMapTemporalQuota();
            return mapper.Map<List<ISSEN.PayerPayment>, List<Model.Quota>>(objTemoralQuotasBusiness);
        }

        public static Model.Quota CreateTemporalQuota(ISSEN.PayerPayment payerPayment)
        {
            var mapper = AutoMapperAssembler.CreateMapTemporalQuota();
            return mapper.Map<ISSEN.PayerPayment, Model.Quota>(payerPayment);
        }

        #endregion

        #region GroupCoverage
        public static List<Model.GroupCoverage> CreateGroupCoverages(BusinessCollection businessCollection)
        {
            var objGroupCoveragesBusiness = businessCollection.Cast<PRODEN.CoverGroupRiskType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapGroupCoverages();
            return mapper.Map<List<PRODEN.CoverGroupRiskType>, List<Model.GroupCoverage>>(objGroupCoveragesBusiness);
        }

        public static Model.GroupCoverage CreateGroupCoverage(PRODEN.CoverGroupRiskType coverGroupRiskType)
        {
            var mapper = AutoMapperAssembler.CreateMapGroupCoverages();
            return mapper.Map<PRODEN.CoverGroupRiskType, Model.GroupCoverage>(coverGroupRiskType);
        }
        #endregion

        #region GroupInsuredObjects
        public static Model.InsuredObject CreateGroupInsuredObject(PRODEN.GroupInsuredObject insuredObject)
        {
            var mapper = AutoMapperAssembler.CreateMapGroupInsuredObject();
            return mapper.Map<PRODEN.GroupInsuredObject, Model.InsuredObject>(insuredObject);
        }

        public static List<Model.InsuredObject> CreateGroupInsuredObjects(BusinessCollection businessCollection)
        {
            var objGroupInsuredObjectsBusiness = businessCollection.Cast<PRODEN.GroupInsuredObject>().ToList();
            var mapper = AutoMapperAssembler.CreateMapGroupInsuredObject();
            return mapper.Map<List<PRODEN.GroupInsuredObject>, List<Model.InsuredObject>>(objGroupInsuredObjectsBusiness);
        }
        #endregion

        #region RiskCommercialClass
        public static List<Model.RiskCommercialClass> CreateRiskCommercialClass(BusinessCollection riskCommercialClass)
        {

            var objRiksCommercialBusiness = riskCommercialClass.Cast<PARAMEN.RiskCommercialClass>().ToList();
            var mapper = AutoMapperAssembler.CreateMapRiskCommercialClass();
            return mapper.Map<List<PARAMEN.RiskCommercialClass>, List<Model.RiskCommercialClass>>(objRiksCommercialBusiness);
        }
        #endregion

        #region PolicyCoverage

        public static List<Coverage> CreatePolicyCoverages(BusinessCollection businessCollection)
        {
            List<Coverage> coverages = new List<Coverage>();

            foreach (ISSEN.RiskCoverage entityRiskCoverage in businessCollection)
            {
                coverages.Add(CreatePolicyCoverage(entityRiskCoverage));
            }

            return coverages;
        }

        public static Model.Coverage CreatePolicyCoverage(ISSEN.RiskCoverage riskCoverage)
        {

            var mapper = AutoMapperAssembler.CreateMapPolicyCoverages();
            var mapperPoliyCoverage = mapper.Map<ISSEN.RiskCoverage, Model.Coverage>(riskCoverage);

            foreach (DynamicConcept item in mapperPoliyCoverage.DynamicProperties)
            {
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = item.Id;
                dynamicConcept.Value = item.Value;
                mapperPoliyCoverage.DynamicProperties.Add(dynamicConcept);
            }


            //foreach (DynamicProperty item in mapperPoliyCoverage.DynamicProperties)
            //{
            //    DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
            //    DynamicConcept dynamicConcept = new DynamicConcept();
            //    dynamicConcept.Id = dynamicProperty.Id;
            //    dynamicConcept.Value = dynamicProperty.Value;
            //    mapperPoliyCoverage.DynamicProperties.Add(dynamicConcept);
            //}

            return mapperPoliyCoverage;
        }

        #endregion

        #region TechnicalPlan

        public static Model.TechnicalPlan CreateTechnicalPlan(PRODEN.TechnicalPlan technicalPlan)
        {
            var mapper = AutoMapperAssembler.CreateMapTechnicalPlan();
            return mapper.Map<PRODEN.TechnicalPlan, Model.TechnicalPlan>(technicalPlan);
        }

        public static List<Model.TechnicalPlan> CreateTechnicalPlans(BusinessCollection businessCollection)
        {
            var objTechninalPlanBusiness = businessCollection.Cast<PRODEN.TechnicalPlan>().ToList();
            var mapper = AutoMapperAssembler.CreateMapTechnicalPlan();
            return mapper.Map<List<PRODEN.TechnicalPlan>, List<Model.TechnicalPlan>>(objTechninalPlanBusiness);
        }

        public static Model.TechnicalPlanCoverage CreateTechnicalPlanCoverage(PRODEN.TechnicalPlanCoverage technicalPlanCoverage)
        {
            var mapper = AutoMapperAssembler.CreateMapTechnicalPlanCoverage();
            return mapper.Map<PRODEN.TechnicalPlanCoverage, Model.TechnicalPlanCoverage>(technicalPlanCoverage);
        }

        public static List<Model.TechnicalPlanCoverage> CreateTechnicalsPlansCoverages(BusinessCollection businessCollection)
        {
            var objTechninalPlansCoveragesBusiness = businessCollection.Cast<PRODEN.TechnicalPlanCoverage>().ToList();
            var mapper = AutoMapperAssembler.CreateMapTechnicalPlanCoverage();
            return mapper.Map<List<PRODEN.TechnicalPlanCoverage>, List<Model.TechnicalPlanCoverage>>(objTechninalPlansCoveragesBusiness);
        }
        #endregion

        #region PolicyType

        public static PolicyType CreatePolicyType(COMMEN.CoPolicyType entityPolicyType)
        {
            var mapper = AutoMapperAssembler.CreateMapPolicyType();
            return mapper.Map<COMMEN.CoPolicyType, PolicyType>(entityPolicyType);
        }

        public static List<PolicyType> CreatePolicyTypes(BusinessCollection businessCollection)
        {
            var objPolicyTypeBusiness = businessCollection.Cast<COMMEN.CoPolicyType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapPolicyType();
            return mapper.Map<List<COMMEN.CoPolicyType>, List<PolicyType>>(objPolicyTypeBusiness);
        }
        #endregion

        #region poliza producto

        public static List<Model.GroupCoverage> GetProductCoverageGroupRisks(BusinessCollection collectionProductGroup, BusinessCollection collectionGroupRiskType)
        {
            if (collectionProductGroup?.Count > 0)

            {
                List<PRODEN.ProductGroupCover> productGroupCovers = collectionProductGroup.Cast<PRODEN.ProductGroupCover>().ToList();
                IMapper immaper = AutoMapperAssembler.CreateMapProductCoverageGroupRisk();
                List<Model.GroupCoverage> groupCoverages = immaper.Map<List<PRODEN.ProductGroupCover>, List<Model.GroupCoverage>>(productGroupCovers);
                List<PRODEN.CoverGroupRiskType> coverGroupRiskTypes = collectionGroupRiskType.Cast<PRODEN.CoverGroupRiskType>().ToList();
                groupCoverages.AsParallel().ForAll(m =>
                {
                    if (coverGroupRiskTypes.FirstOrDefault(r => r.CoveredRiskTypeCode == (int)m.CoveredRiskType) != null)
                        m.Description = coverGroupRiskTypes.FirstOrDefault(r => r.CoveredRiskTypeCode == (int)m.CoveredRiskType && r.CoverageGroupCode == m.Id).Description;
                });
                return groupCoverages;
            }
            else
            {
                return new List<GroupCoverage>();
            }
        }

        public static Model.GroupCoverage CreateProductCoverageGroupRisk(PRODEN.ProductGroupCover productGroupCoverEntity, BusinessCollection collectionGroupRiskType)
        {
            var mapper = AutoMapperAssembler.CreateMapProductCoverageGroupRisk();
            var mapperGroupCoverage = mapper.Map<PRODEN.ProductGroupCover, Model.GroupCoverage>(productGroupCoverEntity);

            var groupRiskType = collectionGroupRiskType.Cast<PRODEN.CoverGroupRiskType>().ToList();
            if (groupRiskType.FirstOrDefault().CoverageGroupCode == mapperGroupCoverage.Id && (CoveredRiskType)groupRiskType.FirstOrDefault().CoveredRiskTypeCode == mapperGroupCoverage.CoveredRiskType)
            {
                mapperGroupCoverage.Description = groupRiskType.FirstOrDefault().SmallDescription;
            }
            return mapperGroupCoverage;

            //Model.GroupCoverage groupCoverage = new Model.GroupCoverage();
            //groupCoverage.Id = productGroupCoverEntity.CoverGroupId;
            //groupCoverage.CoveredRiskType = (CoveredRiskType)productGroupCoverEntity.CoveredRiskTypeCode;

            //foreach (PRODEN.CoverGroupRiskType field in collectionGroupRiskType)
            //{
            //    if (field.CoverageGroupCode == groupCoverage.Id && (CoveredRiskType)field.CoveredRiskTypeCode == groupCoverage.CoveredRiskType)
            //    {
            //        groupCoverage.Description = field.SmallDescription;
            //    }
            //}

            //return groupCoverage;
        }
        #endregion poliza producto

        #region Conceptos Dinamicos




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

        #endregion Conceptos Dinamicos


        #region poliza
        public static void CreatePolicy(Policy policy, Rules.Facade facade)
        {
            policy.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);

        }
        #endregion poliza

        #region riesgo
        public static void CreateRisk(Risk risk, Rules.Facade facade)
        {
            if (facade.GetConcept<int>(RuleConceptRisk.RatingZoneCode) > 0)
            {
                if (risk.RatingZone == null)
                {
                    risk.RatingZone = new RatingZone();
                }

                risk.RatingZone.Id = facade.GetConcept<int>(RuleConceptRisk.RatingZoneCode);
            }

            if (facade.GetConcept<int>(RuleConceptRisk.CoverageGroupId) > 0)
            {
                if (risk.GroupCoverage == null)
                {
                    risk.GroupCoverage = new Models.GroupCoverage();
                }

                risk.GroupCoverage.Id = facade.GetConcept<int>(RuleConceptRisk.CoverageGroupId);
            }

            if (facade.GetConcept<int>(RuleConceptRisk.LimitsRcCode) > 0)
            {
                if (risk.LimitRc == null)
                {
                    risk.LimitRc = new Model.LimitRc();
                }

                risk.LimitRc.Id = facade.GetConcept<int>(RuleConceptRisk.LimitsRcCode);
            }

            if (facade.GetConcept<decimal>(RuleConceptRisk.LimitsRcSum) > 0)
            {
                if (risk.LimitRc == null)
                {
                    risk.LimitRc = new Model.LimitRc();
                }

                risk.LimitRc.LimitSum = facade.GetConcept<decimal>(RuleConceptRisk.LimitsRcSum);
            }
            risk.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);

        }

        #region riskLocation
        internal static RiskLocation CreateRiskLocation(ISSEN.RiskLocation entityRiskLocation)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskLocation();
            return mapper.Map<ISSEN.RiskLocation, RiskLocation>(entityRiskLocation);
            //return new RiskLocation
            //{
            //    Risk = new Risk
            //    {
            //        Id = entityRiskLocation.RiskId
            //    },
            //    ConstructionCategoryId = entityRiskLocation.ConstructionCategoryCode,
            //    RiskDangerousId = entityRiskLocation.RiskDangerousnessCode,
            //    EmlPtc = Convert.ToDecimal(entityRiskLocation.EmlPercentage),
            //    AddressType = entityRiskLocation.AddressTypeCode,
            //    StreetType = entityRiskLocation.StreetTypeCode,
            //    Country = new Country
            //    {
            //        Id = entityRiskLocation.CountryCode
            //    },
            //    State = new State
            //    {
            //        Id = entityRiskLocation.StateCode
            //    },
            //    Street = entityRiskLocation.Street,
            //    City = new City
            //    {
            //        Id = entityRiskLocation.CountryCode
            //    },
            //    HouseNumber = Convert.ToInt32(entityRiskLocation.HouseNumber),
            //    Floor = entityRiskLocation.Floor,
            //    Apartment = entityRiskLocation.Apartment,
            //    ZipCode = entityRiskLocation.ZipCode,
            //    Urbanization = entityRiskLocation.Urbanization,
            //    CrestaZoneId = Convert.ToInt32(entityRiskLocation.CrestaZoneCode),
            //    IsMain = entityRiskLocation.IsMain,
            //    EconomicActivity = Convert.ToInt32(entityRiskLocation.EconomicActivityCode),
            //    HoustingTypeId = Convert.ToInt32(entityRiskLocation.HouseNumber),
            //    OccupationType = Convert.ToInt32(entityRiskLocation.OccupationTypeCode),
            //    CommRiskClass = Convert.ToInt32(entityRiskLocation.CommRiskClassCode),
            //    RiskCommercialtype = Convert.ToInt32(entityRiskLocation.RiskCommercialTypeCode),
            //    RiskCommSubtypeId = Convert.ToInt32(entityRiskLocation.RiskCommSubtypeCode),
            //    AditionalStreet = entityRiskLocation.AdditionalStreet,
            //    Block = entityRiskLocation.Block,
            //    LocationId = Convert.ToInt32(entityRiskLocation.LocationCode),
            //    RiskType = new RiskType
            //    {
            //        Id = Convert.ToInt32(entityRiskLocation.RiskTypeCode)
            //    },
            //    RiskAge = Convert.ToInt32(entityRiskLocation.RiskAge),
            //    IsRetention = Convert.ToBoolean(entityRiskLocation.IsRetention),
            //    InspectionRecomendation = Convert.ToBoolean(entityRiskLocation.InspectionRecomendation),
            //    DeclarativePeriodId = Convert.ToInt32(entityRiskLocation.DeclarativePeriodCode),
            //    PremiumAdjustmentPeriodId = Convert.ToInt32(entityRiskLocation.PremiumAdjustmentPeriodCode),
            //    InsuranceModeId = Convert.ToInt32(entityRiskLocation.InsuranceModeCode),
            //    LongitudeEarthquake = Convert.ToInt32(entityRiskLocation.LongitudeEarthquake),
            //    LatitudEarthquake = Convert.ToInt32(entityRiskLocation.LatitudeEarthquake),
            //    ConstructionYearEarthquake = Convert.ToInt32(entityRiskLocation.ConstructionYearEarthquake),
            //    FloorNumberEarthquake = Convert.ToInt32(entityRiskLocation.FloorNumberEarthquake),
            //    //RiskUse = Convert.ToInt32(entityRiskLocation.RiskUseCode)
            //};
        }

        internal static List<RiskLocation> CreateRiskLocations(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<ISSEN.RiskLocation>().ToList();
            var mapper = AutoMapperAssembler.CreateMapRiskLocation();
            return mapper.Map<List<ISSEN.RiskLocation>, List<Model.RiskLocation>>(objBusiness);
            //List<RiskLocation> riskLocations = new List<RiskLocation>();
            //foreach (ISSEN.RiskLocation entityRiskLocations in businessCollection)
            //{
            //    riskLocations.Add(CreateRiskLocation(entityRiskLocations));
            //}
            //return riskLocations;
        }
        #endregion

        #region riskVehicle
        internal static RiskVehicle CreateRiskVehicle(ISSEN.RiskVehicle entityRiskVehicle)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskVehicle();
            return mapper.Map<ISSEN.RiskVehicle, RiskVehicle>(entityRiskVehicle);

            //return new RiskVehicle
            //{
            //    VehicleYear = Convert.ToInt32(entityRiskVehicle.VehicleYear),
            //    VehiclePrice = Convert.ToDecimal(entityRiskVehicle.VehiclePrice),
            //    IsNew = Convert.ToBoolean(entityRiskVehicle.IsNew),
            //    LicensePlate = entityRiskVehicle.LicensePlate,
            //    EngineNumber = entityRiskVehicle.EngineSerNo,
            //    ChassisNumber = entityRiskVehicle.ChassisSerNo,
            //    LoadTypeId = Convert.ToInt32(entityRiskVehicle.LoadTypeCode),
            //    TrailersQuantity = Convert.ToInt32(entityRiskVehicle.TrailersQuantity),
            //    PassengersQuantity = Convert.ToInt32(entityRiskVehicle.PassengerQuantity),
            //    NewVehiclePrice = Convert.ToDecimal(entityRiskVehicle.NewVehiclePrice),
            //    StandardVehiclePrice = Convert.ToDecimal(entityRiskVehicle.StdVehiclePrice),
            //    VehicleVersion = new VehicleVersion
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.VehicleVersionCode)
            //    },
            //    VehicleModel = new VehicleModel
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.VehicleModelCode)
            //    },
            //    VehicleMake = new VehicleMake
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.VehicleMakeCode)
            //    },
            //    Risk = new Risk
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.RiskId),
            //        MainInsured = new IssuanceInsured
            //        {

            //        }
            //    },
            //    VehicleType = new VehicleType
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.VehicleTypeCode)
            //    },
            //    VehicleUse = new VehicleUse
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.VehicleUseCode)
            //    },
            //    VehicleBody = new VehicleBody
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.VehicleBodyCode)
            //    },
            //    VehicleColor = new VehicleColor
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.VehicleColorCode)
            //    },
            //    VehicleFuel = new VehicleFuel
            //    {
            //        Id = Convert.ToInt32(entityRiskVehicle.VehicleFuelCode)
            //    }
            //};
        }

        internal static List<RiskVehicle> CreateRisksVehicles(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskVehicle();
            return mapper.Map<List<ISSEN.RiskVehicle>, List<RiskVehicle>>(businessCollection.Cast<ISSEN.RiskVehicle>().ToList());

            //List<RiskVehicle> risksVehicles = new List<RiskVehicle>();
            //foreach (ISSEN.RiskVehicle entityRiskVehicles in businessCollection)
            //{
            //    risksVehicles.Add(CreateRiskVehicle(entityRiskVehicles));
            //}
            //return risksVehicles;
        }
        #endregion



        #endregion

        #region RiskActivity

        /// <summary>
        /// Creacion de Actividad del riesgo
        /// </summary>
        /// <param name="riskCommercialClass">Entidad RiskCommercialClass </param>
        /// <returns>Modelo de actividad del riesgo</returns>
        public static RiskActivity CreateRiskActivity(PARAMEN.RiskCommercialClass riskCommercialClass)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskActivity();
            return mapper.Map<PARAMEN.RiskCommercialClass, RiskActivity>(riskCommercialClass);
        }

        /// <summary>
        /// Creacion de lista de actividad del riesgo
        /// </summary>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns>Lista de actividades del riesgo</returns>
        public static List<RiskActivity> CreateRiskActivities(BusinessCollection businessCollection)
        {
            var objRiksActivitiesBusiness = businessCollection.Cast<PARAMEN.RiskCommercialClass>().ToList();
            var mapper = AutoMapperAssembler.CreateMapRiskActivity();
            return mapper.Map<List<PARAMEN.RiskCommercialClass>, List<RiskActivity>>(objRiksActivitiesBusiness);
        }

        /// <summary>
        /// Creacion del tipo de Actividad del riesgo
        /// </summary>
        /// <param name="riskCommercialClass">Entidad RiskCommercialClass </param>
        /// <returns>Modelo de actividad del riesgo</returns>
        public static RiskActivity CreateRiskActivityType(PARAMEN.RiskCommercialType riskCommercialType)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskActivityType();
            return mapper.Map<PARAMEN.RiskCommercialType, RiskActivity>(riskCommercialType);
        }

        /// <summary>
        /// Creacion de lista del tipo de actividad del riesgo
        /// </summary>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns>Lista del tipo de actividades del riesgo</returns>
        public static List<RiskActivity> CreateRiskActivitiesType(BusinessCollection businessCollection)
        {
            var objRiksActivitiesTypeBusiness = businessCollection.Cast<PARAMEN.RiskCommercialType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapRiskActivityType();
            return mapper.Map<List<PARAMEN.RiskCommercialType>, List<RiskActivity>>(objRiksActivitiesTypeBusiness);
        }

        #endregion

        #region SuretyContractCategories

        public static SuretyContractCategories CreateSuretyContractCategory(COMMEN.SuretyContractCategories entitySuretyContractCategories)
        {
            var mapper = AutoMapperAssembler.CreateMapSuretyContractCategory();
            return mapper.Map<COMMEN.SuretyContractCategories, SuretyContractCategories>(entitySuretyContractCategories);
        }

        public static List<SuretyContractCategories> CreateSuretyContractCategories(BusinessCollection businessCollection)
        {
            var objSuretyBusiness = businessCollection.Cast<COMMEN.SuretyContractCategories>().ToList();
            var mapper = AutoMapperAssembler.CreateMapSuretyContractCategory();
            return mapper.Map<List<COMMEN.SuretyContractCategories>, List<SuretyContractCategories>>(objSuretyBusiness);
        }

        #endregion

        #region SuretyContractType

        public static SuretyContractType CreateSuretyContractType(COMMEN.SuretyContractType entitySuretyContractType)
        {
            var mapper = AutoMapperAssembler.CreateMapSuretyContractType();
            return mapper.Map<COMMEN.SuretyContractType, SuretyContractType>(entitySuretyContractType);
        }

        public static List<SuretyContractType> CreateSuretyContractTypes(BusinessCollection businessCollection)
        {
            var objSuretyContractBusiness = businessCollection.Cast<COMMEN.SuretyContractType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapSuretyContractType();
            return mapper.Map<List<COMMEN.SuretyContractType>, List<SuretyContractType>>(objSuretyContractBusiness);
        }

        #endregion

        #region HouseType

        private static UPMO.HouseType CreateHouseType(COMMEN.HouseType entityHouseType)
        {
            var mapper = AutoMapperAssembler.CreateMapHouseType();
            return mapper.Map<COMMEN.HouseType, UPMO.HouseType>(entityHouseType);
        }

        public static List<UPMO.HouseType> CreateHouseTypes(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<COMMEN.HouseType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapHouseType();
            return mapper.Map<List<COMMEN.HouseType>, List<UPMO.HouseType>>(objBusiness);
        }

        #endregion

        #region PrefixEndoTypeEnabled

        public static PrefixEndoTypeEnabled CreatePrefixEndoTypeEnabled(PARAMEN.PrefixEndoTypeEnabled entityPrefixEndoTypeEnabled)
        {
            var mapper = AutoMapperAssembler.CreateMapPrefixEndoTypeEnabled();
            return mapper.Map<PARAMEN.PrefixEndoTypeEnabled, PrefixEndoTypeEnabled>(entityPrefixEndoTypeEnabled);
        }

        public static List<PrefixEndoTypeEnabled> CreatePrefixEndoTypesEnabled(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<PARAMEN.PrefixEndoTypeEnabled>().ToList();
            var mapper = AutoMapperAssembler.CreateMapPrefixEndoTypeEnabled();
            return mapper.Map<List<PARAMEN.PrefixEndoTypeEnabled>, List<PrefixEndoTypeEnabled>>(objBusiness);

        }

        #endregion

        #region RiskType

        public static RiskType CreateRiskType(PARAMEN.CoveredRiskType entityCoveredRiskType)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskType();
            return mapper.Map<PARAMEN.CoveredRiskType, RiskType>(entityCoveredRiskType);
        }

        public static List<RiskType> CreateRiskTypes(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<PARAMEN.CoveredRiskType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapRiskType();
            return mapper.Map<List<PARAMEN.CoveredRiskType>, List<RiskType>>(objBusiness);
        }

        #endregion

        #region RatingZone

        public static RatingZone CreateRatingZone(COMMEN.RatingZone entityRatingZone)
        {
            var mapper = AutoMapperAssembler.CreateMapRatingZone();
            return mapper.Map<COMMEN.RatingZone, RatingZone>(entityRatingZone);
        }

        public static List<RatingZone> CreateRatingZones(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<COMMEN.RatingZone>().ToList();
            var mapper = AutoMapperAssembler.CreateMapRatingZone();
            return mapper.Map<List<COMMEN.RatingZone>, List<RatingZone>>(objBusiness);
        }

        #endregion

        #region mapper


        #region Tomador
        public static IMapper CreateMapHolder()
        {
            var config = MapperCache.GetMapper<IssuanceInsured, Holder>(cfg =>
            {
                cfg.CreateMap<IssuanceCompanyName, IssuanceCompanyName>();
                cfg.CreateMap<IssuancePaymentMethod, IssuancePaymentMethod>();
                cfg.CreateMap<IssuanceInsured, Holder>()
                .ForMember(dest => dest.DeclinedDate, opt => opt.MapFrom(src => src.DeclinedDate ?? DateTime.MinValue))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>

                 src.Name

             ));

            });
            return config;

        }

        #endregion


        #endregion mapper
        #region Issuance


        internal static IssuanceAgency CreateAgency(AgentAgency entityAgentAgency, Agent entityAgent)
        {

            var mapper = AutoMapperAssembler.CreateMapAgencyIssuance();
            var mapperAgentAgency = mapper.Map<AgentAgency, IssuanceAgency>(entityAgentAgency);
            mapperAgentAgency.Agent = new IssuanceAgent
            {
                IndividualId = entityAgent.IndividualId,
                FullName = entityAgent.CheckPayableTo,
                DateDeclined = entityAgent.DeclinedDate,
                AgentType = new IssuanceAgentType
                {
                    Id = entityAgent.AgentTypeCode
                }
            };
            mapperAgentAgency.FullName = entityAgent.CheckPayableTo;

            return mapperAgentAgency;

            //return new IssuanceAgency
            //{
            //    Id = entityAgentAgency.AgentAgencyId,
            //    Code = entityAgentAgency.AgentCode,
            //    FullName = entityAgent.CheckPayableTo,
            //    DateDeclined = entityAgentAgency.DeclinedDate,
            //    AgentType = new IssuanceAgentType
            //    {
            //        Id = entityAgentAgency.AgentTypeCode
            //    },
            //    Agent = new IssuanceAgent
            //    {
            //        IndividualId = entityAgent.IndividualId,
            //        FullName = entityAgent.CheckPayableTo,
            //        DateDeclined = entityAgent.DeclinedDate,
            //        AgentType = new IssuanceAgentType
            //        {
            //            Id = entityAgent.AgentTypeCode
            //        }
            //    }
            //};
        }

        internal static IssuancePhone CreatePhone(Phone entityPhone)
        {
            var mapper = AutoMapperAssembler.CreateMapPhone();
            return mapper.Map<Phone, IssuancePhone>(entityPhone);
        }

        internal static IssuanceEmail CreateEmail(Email entityEmail)
        {
            var mapper = AutoMapperAssembler.CreateMapEmail();
            return mapper.Map<Email, IssuanceEmail>(entityEmail);
        }

        internal static IssuanceAddress CreateAddress(Address entityAddress)
        {
            var mapper = AutoMapperAssembler.CreateMapAddress();
            return mapper.Map<Address, IssuanceAddress>(entityAddress);
        }

        internal static IssuancePaymentMethod CreatePaymentMethod(IndividualPaymentMethod individualPaymentMethod)
        {
            var mapper = AutoMapperAssembler.CreateMapPaymentMethod();
            return mapper.Map<IndividualPaymentMethod, IssuancePaymentMethod>(individualPaymentMethod);
        }

        public static Models.IssuanceCoInsuranceCompany CreateCoInsuranceCompany(COMMEN.CoInsuranceCompany entityCoInsuranceCompany)
        {
            var mapper = AutoMapperAssembler.CreateMapCoInsuranceCompany();
            return mapper.Map<COMMEN.CoInsuranceCompany, IssuanceCoInsuranceCompany>(entityCoInsuranceCompany);
        }
        public static List<Models.IssuanceCoInsuranceCompany> CreateCoInsuranceCompanies(BusinessCollection businessCollection)
        {

            var objBusiness = businessCollection.Cast<COMMEN.CoInsuranceCompany>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCoInsuranceCompany();
            return mapper.Map<List<COMMEN.CoInsuranceCompany>, List<IssuanceCoInsuranceCompany>>(objBusiness);
        }
        #endregion

        #region VehicleType

        /// <summary>
        /// Crea el modelo de tipo de vehiculo
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculos</param>
        /// <returns>Modelo de tipo de vehiculos</returns>
        public static VehicleType CreateVehicleType(COMMEN.VehicleType vehicleType)
        {
            var mapper = AutoMapperAssembler.CreateMapVehicleType();
            return mapper.Map<COMMEN.VehicleType, VehicleType>(vehicleType);
        }

        public static List<VehicleType> CreateVehicleTypes(BusinessCollection vehicleTypes)
        {
            var objBusiness = vehicleTypes.Cast<COMMEN.VehicleType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapVehicleType();
            return mapper.Map<List<COMMEN.VehicleType>, List<VehicleType>>(objBusiness);
        }

        #endregion

        #region VehicleBody

        /// <summary>
        /// Mapeo de la entidad VehicleBody al modelo VehicleBody
        /// </summary>
        /// <param name="vehicleBody">Entidad VehicleBody</param>
        /// <returns>Modelo VehicleBody</returns>
        public static VehicleBody CreateVehicleBody(COMMEN.VehicleBody vehicleBody)
        {
            var mapper = AutoMapperAssembler.CreateMapVehicleBody();
            return mapper.Map<COMMEN.VehicleBody, VehicleBody>(vehicleBody);
        }

        public static List<VehicleBody> CreateVehicleBodies(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<COMMEN.VehicleBody>().ToList();
            var mapper = AutoMapperAssembler.CreateMapVehicleBody();
            return mapper.Map<List<COMMEN.VehicleBody>, List<VehicleBody>>(objBusiness);
        }

        #endregion

        #region VehicleUse

        public static VehicleUse CreateVehicleUse(COMMEN.VehicleUse vehicleUse)
        {
            var mapper = AutoMapperAssembler.CreateMapVehicleUse();
            return mapper.Map<COMMEN.VehicleUse, VehicleUse>(vehicleUse);
        }

        public static List<VehicleUse> CreateVehicleUses(List<COMMEN.VehicleUse> vehicleUses)
        {
            var objBusiness = vehicleUses.Cast<COMMEN.VehicleUse>().ToList();
            var mapper = AutoMapperAssembler.CreateMapVehicleUse();
            return mapper.Map<List<COMMEN.VehicleUse>, List<VehicleUse>>(objBusiness);
        }

        #endregion
        #region SubLineBusiness
        public static SubLineBusiness CreateSubLineBusiness(COMMEN.SubLineBusiness entitySubLineBusiness)
        {
            var mapper = AutoMapperAssembler.CreateMapSubLineBusiness();
            return mapper.Map<COMMEN.SubLineBusiness, SubLineBusiness>(entitySubLineBusiness);
        }
        #endregion

        #region CoCoverage
        public static ParamCoCoverageValue CreateCoCoverageValue(QUOEN.Coverage coverage, COMMEN.Prefix prefix, QUOEN.CoCoverageValue coCoverageValue)
        {
            ParamCoCoverageValue paramCoCoverageValue = new ParamCoCoverageValue
            {
                Coverage = new Model.Base.BaseParamCoverage { Id = coverage.CoverageId, Description = coverage.PrintDescription },
                Prefix = new Model.Base.BaseParamPrefix { Id = prefix.PrefixCode, Description = prefix.Description },
                Percentage = coCoverageValue.ValuePje
            };
            return paramCoCoverageValue;
        }


        #endregion


        #region Tax

        #region Tax Methods

        internal static ParamTax CreateParamTax(Tax.Entities.Tax entityTax)
        {
            var mapper = AutoMapperAssembler.CreateMapParamTax();
            return mapper.Map<Tax.Entities.Tax, ParamTax>(entityTax);
        }

        internal static List<ParamTax> CreateTaxesSearched(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<Tax.Entities.Tax>().ToList();
            var mapper = AutoMapperAssembler.CreateMapParamTax();
            return mapper.Map<List<Tax.Entities.Tax>, List<ParamTax>>(objBusiness);
        }

        #endregion

        #region TaxRole Methods

        internal static Model.TaxRole CreateParamTaxRole(Tax.Entities.TaxRole taxRole)
        {
            var mapper = AutoMapperAssembler.CreateMapParamTaxRole();
            return mapper.Map<Tax.Entities.TaxRole, Model.TaxRole>(taxRole);
        }

        #endregion

        #region TaxAttribute Methods

        internal static Model.TaxAttribute CreateParamTaxAttribute(Tax.Entities.TaxAttribute taxAttribute)
        {
            var mapper = AutoMapperAssembler.CreateMapParamTaxAttribute();
            return mapper.Map<Tax.Entities.TaxAttribute, Model.TaxAttribute>(taxAttribute);
        }
        #endregion

        #region TaxRate Methods

        internal static ParamTaxRate CreateParamTaxRate(Tax.Entities.TaxRate entityTaxRate)
        {
            var mapper = AutoMapperAssembler.CreateMapParamTaxRate();
            return mapper.Map<Tax.Entities.TaxRate, ParamTaxRate>(entityTaxRate);
        }

        internal static List<ParamTaxRate> CreateTaxRatesSearched(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<Tax.Entities.TaxRate>().ToList();
            var mapper = AutoMapperAssembler.CreateMapParamTaxRate();
            return mapper.Map<List<Tax.Entities.TaxRate>, List<ParamTaxRate>>(objBusiness);
        }

        internal static ParamTaxRate CreateTaxRate(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<Tax.Entities.TaxRate>().FirstOrDefault();
            var mapper = AutoMapperAssembler.CreateMapParamTaxRate();
            return mapper.Map<Tax.Entities.TaxRate, ParamTaxRate>(objBusiness);
        }

        #endregion

        #region TaxPeriodRate Methods

        internal static TAXMO.TaxPeriodRate CreateParamTaxPeriodRate(Tax.Entities.TaxPeriodRate taxPeriodRate)
        {
            var mapper = AutoMapperAssembler.CreateMapParamTaxPeriodRate();
            return mapper.Map<Tax.Entities.TaxPeriodRate, TAXMO.TaxPeriodRate>(taxPeriodRate);
        }

        #endregion

        #region TaxCategory Methods

        internal static ParamTaxCategory CreateParamTaxCategory(Tax.Entities.TaxCategory entityTaxCategory)
        {
            var mapper = AutoMapperAssembler.CreateMapParamTaxCategory();
            return mapper.Map<Tax.Entities.TaxCategory, ParamTaxCategory>(entityTaxCategory);
        }

        internal static List<ParamTaxCategory> CreateTaxCategoriesSearched(BusinessCollection businessCollection)
        {

            var objBusiness = businessCollection.Cast<Tax.Entities.TaxCategory>().ToList();
            var mapper = AutoMapperAssembler.CreateMapParamTaxCategory();
            return mapper.Map<List<Tax.Entities.TaxCategory>, List<ParamTaxCategory>>(objBusiness);
        }
        #endregion

        #region TaxCondition Methods

        internal static ParamTaxCondition CreateParamTaxCondition(Tax.Entities.TaxCondition entityTaxCondition)
        {
            var mapper = AutoMapperAssembler.CreateMapParamTaxCondition();
            return mapper.Map<Tax.Entities.TaxCondition, ParamTaxCondition>(entityTaxCondition);
        }

        internal static List<ParamTaxCondition> CreateTaxConditionsSearched(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<Tax.Entities.TaxCondition>().ToList();
            var mapper = AutoMapperAssembler.CreateMapParamTaxCondition();
            return mapper.Map<List<Tax.Entities.TaxCondition>, List<ParamTaxCondition>>(objBusiness);
        }
        #endregion

        #endregion Tax

        #region generic excel export

        /// <summary>
        /// Convierte entidad a modelo
        /// </summary>
        /// <param name="entityFile">Entidad File</param>
        /// <returns>Modelo File</returns>
        public static File CreateFile(COMMEN.File entityFile)
        {

            var mapper = AutoMapperAssembler.CreateMapFile();
            return mapper.Map<COMMEN.File, File>(entityFile);
            //return new File()
            //{
            //    Id = entityFile.Id,
            //    Description = entityFile.Description,
            //    SmallDescription = entityFile.SmallDescription,
            //    Observations = entityFile.Observations,
            //    IsEnabled = entityFile.IsEnabled,
            //    FileType = (UTILEN.FileType)entityFile.FileTypeId,
            //    Templates = new List<Template>()
            //};
        }

        /// <summary>
        /// Convierte a Modelo File
        /// </summary>
        /// <param name="businessCollection">Colección File</param>
        /// <returns>Modelo File</returns>
        public static List<File> CreateFiles(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<COMMEN.File>().ToList();
            var mapper = AutoMapperAssembler.CreateMapFile();
            return mapper.Map<List<COMMEN.File>, List<File>>(objBusiness);

            //List<File> files = new List<File>();

            //foreach (COMMEN.File entity in businessCollection)
            //{
            //    files.Add(ModelAssembler.CreateFile(entity));
            //}

            //return files;
        }

        /// <summary>
        /// Convierte a Modelo FileProcessValue
        /// </summary>
        /// <param name="businessCollection">Coleccion FileProcess</param>
        /// <returns>Modelo FileProcessValue</returns>
        public static FileProcessValue CreateFileProcessValue(BusinessCollection businessCollection)
        {
            FileProcessValue fileProcess = null;

            if (businessCollection.Count > 0)
            {
                foreach (COMMEN.FileProcessValue item in businessCollection)
                {
                    fileProcess = new FileProcessValue()
                    {
                        Id = item.Id,
                        FileId = item.FileId,
                        Key1 = item.Key1,
                        Key2 = item.Key2.HasValue ? item.Key2.Value : 0,
                        Key3 = item.Key3.HasValue ? item.Key3.Value : 0,
                        Key4 = item.Key4.HasValue ? item.Key4.Value : 0,
                        Key5 = item.Key5.HasValue ? item.Key5.Value : 0
                    };
                }
            }

            return fileProcess;
        }

        /// <summary>
        /// Llena los campos
        /// </summary>
        /// <param name="entityFile">Modelo Field</param>
        /// <returns>Propiedades de los campos</returns>
        public static Field CreateField(COMMEN.Field entityFile)
        {
            return new Field()
            {
                Id = entityFile.Id,
                Description = entityFile.Description,
                SmallDescription = entityFile.SmallDescription,
                FieldType = (UTILEN.FieldType)entityFile.FieldTypeId,
                PropertyName = entityFile.PropertyName,
                PropertyLength = entityFile.PropertyLength
            };
        }

        /// <summary>
        /// Crea los campos
        /// </summary>
        /// <param name="entityFileTemplateField">Modelo FileTemplateField</param>
        /// <param name="entityField">Modelo Field</param>
        /// <returns>Datos de los campos</returns>
        public static Field CreateField(COMMEN.FileTemplateField entityFileTemplateField, COMMEN.Field entityField)
        {
            return new Field
            {
                Id = entityField.Id,
                TemplateFieldId = entityFileTemplateField.Id,
                Description = entityFileTemplateField.Description,
                SmallDescription = entityField.SmallDescription,
                FieldType = (UTILEN.FieldType)entityField.FieldTypeId,
                IsEnabled = entityFileTemplateField.IsEnabled,
                IsMandatory = entityFileTemplateField.IsMandatory,
                Order = entityFileTemplateField.Order,
                ColumnSpan = entityFileTemplateField.ColumnSpan,
                RowPosition = entityFileTemplateField.RowPosition,
                PropertyName = entityField.PropertyName,
                PropertyLength = entityField.PropertyLength
            };
        }

        #endregion

        #region automaper
        #region Mapper ContratType
        public static List<ComboDTO> CreateSuretyContractTypes(List<SuretyContractType> suretyContractTypes)
        {
            IMapper mapper = AutoMapperAssembler.CreateMapContratType();
            return mapper.Map<List<SuretyContractType>, List<ComboDTO>>(suretyContractTypes);
        }

        public static List<ComboDTO> CreateContractCategories(List<SuretyContractCategories> suretyContractCategories)
        {
            IMapper mapper = AutoMapperAssembler.CreateMapContractCategories();
            return mapper.Map<List<SuretyContractCategories>, List<ComboDTO>>(suretyContractCategories);
        }

        public static List<ComboDTO> CreateRiskGroupCoverages(List<GroupCoverage> suretyGroupCoverage)
        {
            if (suretyGroupCoverage?.Count > 0)
            {
                IMapper mapper = AutoMapperAssembler.CreateMapGroupCoverages();
                return mapper.Map<List<GroupCoverage>, List<ComboDTO>>(suretyGroupCoverage);
            }
            else
            {
                return null;
            }

        }
        #endregion
        #endregion automaper

        #region Agency
        public static IssuanceAgency CreateAgency(AgentAgency agency)
        {
            var mapper = AutoMapperAssembler.CreateMapAgency();
            return mapper.Map<AgentAgency, IssuanceAgency>(agency);
        }

        public static List<IssuanceAgency> CreateAgencies(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<AgentAgency>().ToList();
            var mapper = AutoMapperAssembler.CreateMapAgency();
            return mapper.Map<List<AgentAgency>, List<IssuanceAgency>>(objBusiness);
        }

        #endregion


        #region IdentityCardType

        public static IssuanceDocumentType CreateIdentityCardType(IdentityCardType identityCardType)
        {
            var mapper = AutoMapperAssembler.CreateMapIdentityCardTyp();
            return mapper.Map<IdentityCardType, IssuanceDocumentType>(identityCardType);
        }

        public static List<IssuanceDocumentType> CreateIdentityCardTypes(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<IdentityCardType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapIdentityCardTyp();
            return mapper.Map<List<IdentityCardType>, List<IssuanceDocumentType>>(objBusiness);
        }
        #endregion

        #region TributaryIdentityType

        public static IssuanceDocumentType CreateTributaryIdentityType(TributaryIdentityType tributaryIdentityType)
        {
            var config = AutoMapperAssembler.CreateMapTributaryIdentityType();
            return config.Map<TributaryIdentityType, Model.IssuanceDocumentType>(tributaryIdentityType);
        }

        public static List<IssuanceDocumentType> CreateTributaryIdentityTypes(BusinessCollection businessCollection)
        {
            var objTributaryIdentityBusiness = businessCollection.Cast<TributaryIdentityType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapTributaryIdentityType();
            return mapper.Map<List<TributaryIdentityType>, List<IssuanceDocumentType>>(objTributaryIdentityBusiness);
        }
        #endregion

        #region DocumenType
        /// <summary>
        /// Creates the type of the document.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns></returns>
        private static IssuanceDocumentType CreateDocumentType(DocumentType documentType)
        {
            var config = AutoMapperAssembler.CreateMapDocumentType();
            return config.Map<DocumentType, Model.IssuanceDocumentType>(documentType);
        }

        /// <summary>
        /// Creates the document types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<IssuanceDocumentType> CreateDocumentTypes(BusinessCollection businessCollection)
        {
            var objDocumentTypesBusiness = businessCollection.Cast<DocumentType>().ToList();
            var mapper = AutoMapperAssembler.CreateMapDocumentType();
            return mapper.Map<List<DocumentType>, List<IssuanceDocumentType>>(objDocumentTypesBusiness);
        }
        #endregion

        #region Claim

        public static Policy CreateClaimPolicy(ISSEN.Policy policy, ISSEN.CoPolicy coPolicy, ISSEN.Endorsement endorsement, PARAMEN.BusinessType businessType, COMMEN.Currency entityCurrency, COMMEN.CoPolicyType entityCoPolicyType, PARAMEN.EndorsementType entityEndorsementType)
        {
            var config = AutoMapperAssembler.CreateMapClaimPolicy();
            var mapperPolicy = config.Map<ISSEN.Policy, Policy>(policy);
            mapperPolicy.Endorsement.Id = endorsement.EndorsementId;
            mapperPolicy.Endorsement.CurrentFrom = endorsement.CurrentFrom;
            mapperPolicy.Endorsement.CurrentTo = endorsement.CurrentTo.Value;
            mapperPolicy.Endorsement.EndorsementType = (EndorsementType)endorsement.EndoTypeCode;
            mapperPolicy.Endorsement.EndorsementTypeDescription = entityEndorsementType.Description;
            mapperPolicy.Endorsement.Number = endorsement.DocumentNum;
            mapperPolicy.BusinessTypeDescription = businessType.SmallDescription;
            mapperPolicy.ExchangeRate.BuyAmount = endorsement.ExchangeRate;
            mapperPolicy.ExchangeRate.Currency.Description = entityCurrency.Description;
            mapperPolicy.PolicyType = new PolicyType
            {
                Id = coPolicy.PolicyTypeCode,
                Description = entityCoPolicyType.Description

            };
            mapperPolicy.Holder = new Holder
            {
                IndividualId = Convert.ToInt32(policy.PolicyholderId)
            };
            mapperPolicy.IssueDate = endorsement.IssueDate;
            mapperPolicy.Summary = new Summary();
            mapperPolicy.CoInsuranceCompanies = new List<IssuanceCoInsuranceCompany>();
            mapperPolicy.Agencies = new List<IssuanceAgency>();
            mapperPolicy.CoInsuranceCompanies.Add(new IssuanceCoInsuranceCompany
            {
                ParticipationPercentageOwn = policy.CoissuePercentage.Value
            });
            mapperPolicy.Product = new ProductModel.Product
            {
                Id = Convert.ToInt32(policy.ProductId)
            };
            return mapperPolicy;
            //Policy policyModel = new Policy
            //{
            //    CurrentFrom = policy.CurrentFrom,
            //    CurrentTo = policy.CurrentTo.Value,
            //    DocumentNumber = policy.DocumentNumber,
            //    Prefix = new Prefix
            //    {
            //        Id = policy.PrefixCode
            //    },
            //    Endorsement = new Endorsement
            //    {
            //        ///propiedad
            //        Id = endorsement.EndorsementId,
            //        PolicyId = policy.PolicyId,
            //        CurrentFrom = endorsement.CurrentFrom,
            //        CurrentTo = endorsement.CurrentTo.Value,
            //        EndorsementType = (EndorsementType)endorsement.EndoTypeCode,
            //        EndorsementTypeDescription = entityEndorsementType.Description,
            //        Number = endorsement.DocumentNum
            //    },
            //    Product = new ProductModel.Product
            //    {
            //        Id = policy.ProductId.Value
            //    },
            //    Branch = new Branch
            //    {
            //        Id = policy.BranchCode,
            //        SalePoints = policy.SalePointCode.HasValue ? new List<SalePoint>
            //        {
            //            new SalePoint {
            //                Id = (int)policy.SalePointCode
            //            }
            //        } : null
            //    },
            //    BusinessType = (BusinessType)policy.BusinessTypeCode,
            //    BusinessTypeDescription = businessType.SmallDescription,
            //    PolicyType = new PolicyType
            //    {
            //        Id = coPolicy.PolicyTypeCode,
            //        Description = entityCoPolicyType.Description
            //    },
            //    Holder = new Holder
            //    {
            //        IndividualId = policy.PolicyholderId.Value,
            //    },
            //    ExchangeRate = new CommonModel.ExchangeRate
            //    {
            //        BuyAmount = endorsement.ExchangeRate,
            //        Currency = new CommonModel.Currency
            //        {
            //            Id = policy.CurrencyCode,
            //            Description = entityCurrency.Description
            //        }
            //    },
            //    IssueDate = endorsement.IssueDate,
            //    Summary = new Summary(),
            //    CoInsuranceCompanies = new List<IssuanceCoInsuranceCompany>(),
            //    Agencies = new List<IssuanceAgency>()
            //};

            //policyModel.CoInsuranceCompanies.Add(new IssuanceCoInsuranceCompany
            //{
            //    ParticipationPercentageOwn = policy.CoissuePercentage.Value
            //});

            //return policyModel;
        }


        public static IssuanceCoInsuranceCompany CreateCoInsurancesAssigned(ISSEN.CoinsuranceAssigned coinsuranceAssigned)
        {
            var config = AutoMapperAssembler.CreateMapCoInsurancesAssigned();
            return config.Map<ISSEN.CoinsuranceAssigned, IssuanceCoInsuranceCompany>(coinsuranceAssigned);

            //IssuanceCoInsuranceCompany coInsuranceCompany = new IssuanceCoInsuranceCompany()
            //{
            //    Id = coinsuranceAssigned.InsuranceCompanyId,
            //    ExpensesPercentage = coinsuranceAssigned.ExpensesPercentage,
            //    ParticipationPercentage = coinsuranceAssigned.PartCiaPercentage,
            //    EndorsementNumber = coinsuranceAssigned.CompanyNum.ToString()
            //};

            //return coInsuranceCompany;
        }
        public static IssuanceCoInsuranceCompany CreateCoInsuranceAccepted(ISSEN.CoinsuranceAccepted coinsuranceAccepted)
        {
            var config = AutoMapperAssembler.CreateMapCoInsuranceAccepted();
            return config.Map<ISSEN.CoinsuranceAccepted, IssuanceCoInsuranceCompany>(coinsuranceAccepted);
        }
        public static List<IssuanceCoInsuranceCompany> CreateCoInsuranceAccepted(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<ISSEN.CoinsuranceAccepted>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCoInsuranceAccepted();
            return mapper.Map<List<ISSEN.CoinsuranceAccepted>, List<IssuanceCoInsuranceCompany>>(objBusiness);
        }

        public static List<IssuanceCoInsuranceCompany> CreateCoInsurancesAssigned(BusinessCollection businessCollection)
        {
            var objBusiness = businessCollection.Cast<ISSEN.CoinsuranceAssigned>().ToList();
            var mapper = AutoMapperAssembler.CreateMapCoInsurancesAssigned();
            return mapper.Map<List<ISSEN.CoinsuranceAssigned>, List<IssuanceCoInsuranceCompany>>(objBusiness);
        }
        #endregion

        public static List<DynamicConcept> CreateDynamicConcepts(List<DynamicConceptValue> dynamicConceptValue, List<DynamicConceptRelation> dynamicConceptRelations)
        {
            List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

            foreach (var conceptValue in dynamicConceptValue)
            {
                var conceptRelations = dynamicConceptRelations.Where(x => x.DynamicId == conceptValue.DynamicId).FirstOrDefault();
                dynamicConcepts.Add(CreateDynamicConcepts(conceptValue, conceptRelations));
            }

            return dynamicConcepts;
        }

        public static DynamicConcept CreateDynamicConcepts(DynamicConceptValue dynamicConceptValue, DynamicConceptRelation conceptRelations)
        {
            DynamicConcept dynamicConcept = new DynamicConcept
            {
                Id = dynamicConceptValue.ConceptId,
                Value = dynamicConceptValue.ConceptValue,
                EntityId = Convert.ToInt16(UUE.EntityTypes.FacadeRisk),
                ValueType = dynamicConceptValue.ConceptValue.GetType().ToString()
            };

            if (dynamicConcept.Value != null &&
                        DateTime.TryParseExact(dynamicConcept.Value.ToString(),
                        StringHelper.DateFormatCompatibleRulesR1(),
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                dynamicConcept.Value = dateTime;
            }
            else if (dynamicConcept.Value != null && dynamicConcept.ValueType == typeof(DateTime).ToString())
            {
                dynamicConcept.Value = DateTime.Parse(dynamicConcept.Value.ToString());
            }
            dynamicConcept.ValueType = dynamicConcept.Value != null ? dynamicConcept.Value.GetType().FullName : "Null";

            return dynamicConcept;
        }

        internal static List<CoInsuranceAssigned> CreateCoinsuranceAssigneds(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapCoinsuranceAssigned();
            return mapper.Map<List<ISSENT.CoinsuranceAssigned>, List<CoInsuranceAssigned>>(businessCollection.Cast<ISSENT.CoinsuranceAssigned>().ToList());
        }

        internal static CoInsuranceAssigned CreateCoinsuranceAssigned(ISSENT.CoinsuranceAssigned entityCoinsuranceAssigned)
        {
            var mapper = AutoMapperAssembler.CreateMapCoinsuranceAssigned();
            return mapper.Map<ISSENT.CoinsuranceAssigned, CoInsuranceAssigned>(entityCoinsuranceAssigned);
        }
        #region Componentes
        /// <summary>
        /// Creates the payment distribution components.
        /// </summary>
        /// <param name="paymentDistributionComponents">The payment distribution components.</param>
        /// <returns></returns>
        public static List<PaymentDistributionComp> CreatePaymentDistributionComponents(List<PRODEN.CoPaymentDistributionComponent> paymentDistributionComponents)
        {
            var mapper = AutoMapperAssembler.CreateMapPaymentDistributionComponents();
            return mapper.Map<List<PRODEN.CoPaymentDistributionComponent>, List<PaymentDistributionComp>>(paymentDistributionComponents);
        }

        public static FinancialPaymentPlan CreateFinancialPaymentPlan(FinancialPaymentSchedule financialPaymentSchedule)
        {
            var mapper = AutoMapperAssembler.CreateMapFinancialPaymentPlan();
            return mapper.Map<FinancialPaymentSchedule, FinancialPaymentPlan>(financialPaymentSchedule);
        }
        public static ComponentValue CreateComponentValueDTO(ComponentValueDTO componentValueDTO)
        {
            var mapper = AutoMapperAssembler.CreateMapComponentValueDTO();
            return mapper.Map<ComponentValueDTO, ComponentValue>(componentValueDTO);
        }

        #endregion
    }
}