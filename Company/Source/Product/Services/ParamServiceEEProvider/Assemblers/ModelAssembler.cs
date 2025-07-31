// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ProductParamServices.EEProvider.Assemblers
{
    using System.Collections;
    using System.Collections.Generic;
    using AutoMapper;
    using Sistran.Company.Application.ProductParamService.Models;
    using PRODEN = Sistran.Core.Application.Product.Entities;
    using CIAPRODEN = Sistran.Company.Application.Product.Entities;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using CIACOMMEN = Sistran.Company.Application.Common.Entities;
    using INTEN = Sistran.Company.Application.Integration.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Utilities.Cache;

    /// <summary>
    /// Clase enmbladora para mapear entidades a modelos de negocio.
    /// </summary>
    public static class ModelAssembler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityAssistanceTypes"></param>
        /// <returns></returns>
        public static List<CiaParamBeneficiaryType> CreateCiaParamBeneficiaryTypes(List<QUOEN.BeneficiaryType> entityBeneficiaryTypes)
        {
            var config = CreateMapBeneficiaryType();
            List<CiaParamBeneficiaryType> ciaParamBeneficiaryTypes = config.Map<List<QUOEN.BeneficiaryType>, List<CiaParamBeneficiaryType>>(entityBeneficiaryTypes);
            return ciaParamBeneficiaryTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cptAssistanceType"></param>
        /// <returns></returns>
        public static CiaParamBeneficiaryType CreateCiaParamBeneficiaryType(QUOEN.BeneficiaryType beneficiaryType)
        {
            CiaParamBeneficiaryType ciaParamBeneficiaryType = new CiaParamBeneficiaryType();
            var config = CreateMapBeneficiaryType();
            config.Map<QUOEN.BeneficiaryType, CiaParamBeneficiaryType>(beneficiaryType, ciaParamBeneficiaryType);
            return ciaParamBeneficiaryType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productGroupCovers"></param>
        /// <returns></returns>
        public static List<CiaParamInsuredObject> CreateCiaParamInsuredObjects(List<PRODEN.GroupInsuredObject> groupInsuredObjects)
        {
            var config = CreateMapProductInsuredObject();
            List<CiaParamInsuredObject> ciaParamInsuredObjects = config.Map<List<PRODEN.GroupInsuredObject>, List<CiaParamInsuredObject>>(groupInsuredObjects);
            return ciaParamInsuredObjects;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCoverRiskType"></param>
        /// <returns></returns>
        public static CiaParamInsuredObject CreateCiaParamInsuredObject(PRODEN.GroupInsuredObject groupInsuredObject)
        {
            CiaParamInsuredObject ciaParamInsuredObject = new CiaParamInsuredObject();
            var configProductCoverage = CreateMapProductCoverage();
            var configProductInsuredObject = CreateMapProductInsuredObject();
            configProductInsuredObject.Map<PRODEN.GroupInsuredObject, CiaParamInsuredObject>(groupInsuredObject, ciaParamInsuredObject);
            return ciaParamInsuredObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productGroupCovers"></param>
        /// <returns></returns>
        public static List<CiaParamCoverage> CreateCiaParamGroupCoverages(List<PRODEN.ProductGroupCover> productGroupCovers)
        {
            var config = CreateMapProductGroupCoverage();
            List<CiaParamCoverage> ciaParamCoverages = config.Map<List<PRODEN.ProductGroupCover>, List<CiaParamCoverage>>(productGroupCovers);
            return ciaParamCoverages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCoverRiskType"></param>
        /// <returns></returns>
        public static CiaParamCoverage CreateCiaParamCoverage(PRODEN.ProductGroupCover productGroupCover)
        {
            CiaParamCoverage ciaParamCoverage = new CiaParamCoverage();
            var config = CreateMapProductGroupCoverage();
            config.Map<PRODEN.ProductGroupCover, CiaParamCoverage>(productGroupCover, ciaParamCoverage);
            return ciaParamCoverage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCoverRiskType"></param>
        /// <returns></returns>
        public static CiaParamGroupCoverage CreateCiaParamFullCoverage(PRODEN.GroupCoverage groupCover, QUOEN.Coverage coverage)
        {
            CiaParamGroupCoverage ciaParamCoverage = new CiaParamGroupCoverage();
            var configProductCoverage =  CreateMapProductCoverage();
            var configCoverage = CreateMapCoverage();
            configCoverage.Map<QUOEN.Coverage, CiaParamGroupCoverage>(coverage, ciaParamCoverage);
            configProductCoverage.Map<PRODEN.GroupCoverage, CiaParamGroupCoverage>(groupCover, ciaParamCoverage);
            return ciaParamCoverage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCoverRiskType"></param>
        /// <returns></returns>
        public static CiaParamRiskType CreateCiaParamRiskType(PRODEN.ProductCoverRiskType productCoverRiskType)
        {
            CiaParamRiskType ciaParamRiskType = new CiaParamRiskType();
            var config = CreateMapProductRiskType();
            config.Map<PRODEN.ProductCoverRiskType, CiaParamRiskType>(productCoverRiskType, ciaParamRiskType);
            return ciaParamRiskType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productCoverRiskType"></param>
        /// <returns></returns>
        public static List<CiaParamRiskType> CreateCiaParamRiskTypes(List<PRODEN.ProductCoverRiskType> productCoverRiskTypes)
        {
            var config = CreateMapProductRiskType();
            List<CiaParamRiskType> ciaParamRiskTypes = config.Map<List<PRODEN.ProductCoverRiskType>, List<CiaParamRiskType>>(productCoverRiskTypes);
            return ciaParamRiskTypes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productFinancialPlans"></param>
        /// <returns></returns>
        public static List<CiaParamGroupCoverage> CreateCiaParamGroupCoverages(List<QUOEN.Coverage> coverages)
        {
            var configCoverage = CreateMapCoverage();
            List<CiaParamGroupCoverage> ciaParamGroupCoverages = configCoverage.Map<List<QUOEN.Coverage>, List<CiaParamGroupCoverage>>(coverages);
            return ciaParamGroupCoverages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productFinancialPlan"></param>
        /// <returns></returns>
        public static CiaParamGroupCoverage CreateCiaParamGroupCoverage(QUOEN.Coverage coverage)
        {
            CiaParamGroupCoverage ciaParamGroupCoverage = new CiaParamGroupCoverage();
            var configCoverage = CreateMapCoverage();
            configCoverage.Map<QUOEN.Coverage, CiaParamGroupCoverage>(coverage, ciaParamGroupCoverage);
            return ciaParamGroupCoverage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productFinancialPlans"></param>
        /// <returns></returns>
        public static List<CiaParamFinancialPlan> CreateCiaParamListFinancialPlans(List<PRODEN.FinancialPlan> financialPlans)
        {
            var config = CreateMapFinancialPlan();
            List<CiaParamFinancialPlan> ciaParamFinancialPlans = config.Map<List<PRODEN.FinancialPlan>, List<CiaParamFinancialPlan>>(financialPlans);
            return ciaParamFinancialPlans;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productFinancialPlan"></param>
        /// <returns></returns>
        public static CiaParamFinancialPlan CreateCiaParamListFinancialPlan(PRODEN.FinancialPlan financialPlan)
        {
            CiaParamFinancialPlan ciaParamFinancialPlan = new CiaParamFinancialPlan();
            var config = CreateMapFinancialPlan();
            config.Map<PRODEN.FinancialPlan, CiaParamFinancialPlan>(financialPlan, ciaParamFinancialPlan);
            return ciaParamFinancialPlan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productFinancialPlans"></param>
        /// <returns></returns>
        public static List<CiaParamFinancialPlan> CreateCiaParamFinancialPlans(List<PRODEN.ProductFinancialPlan> productFinancialPlans)
        {
            var config = CreateMapProductFinancialPlan();
            List<CiaParamFinancialPlan> ciaParamFinancialPlans = config.Map<List<PRODEN.ProductFinancialPlan>, List<CiaParamFinancialPlan>>(productFinancialPlans);
            return ciaParamFinancialPlans;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productFinancialPlan"></param>
        /// <returns></returns>
        public static CiaParamFinancialPlan CreateCiaParamFinancialPlan(PRODEN.ProductFinancialPlan productFinancialPlan)
        {
            CiaParamFinancialPlan ciaParamFinancialPlan = new CiaParamFinancialPlan();
            var config = CreateMapProductFinancialPlan();
            config.Map<PRODEN.ProductFinancialPlan, CiaParamFinancialPlan>(productFinancialPlan, ciaParamFinancialPlan);
            return ciaParamFinancialPlan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityAssistanceTypes"></param>
        /// <returns></returns>
        //public static List<CiaParamAssistanceType> CreateCiaParamAssistanceTypes(List<CIACOMMEN.CptAssistanceType> entityAssistanceTypes)
        //{
        //    List<CiaParamAssistanceType> result = new List<CiaParamAssistanceType>();
        //    foreach (CIACOMMEN.CptAssistanceType itemAssitance in entityAssistanceTypes)
        //    {
        //        result.Add(CreateCiaParamAssitanceType(itemAssitance));
        //    }
        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cptAssistanceType"></param>
        /// <returns></returns>
        //public static CiaParamAssistanceType CreateCiaParamAssitanceType(CIACOMMEN.CptAssistanceType cptAssistanceType)
        //{
        //    CiaParamAssistanceType ciaParamAssistanceType = new CiaParamAssistanceType();
        //    CreateMapAssistanceType();
        //    Mapper.Map<CIACOMMEN.CptAssistanceType, CiaParamAssistanceType>(cptAssistanceType, ciaParamAssistanceType);
        //    return ciaParamAssistanceType;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityProducts"></param>
        /// <returns></returns>
        public static List<CiaParamProduct2G> CreateCiaParamProducts2G(List<CIAPRODEN.CoProduct2g> entityProducts)
        {
            var config = CreateMapProduct2G();
            List<CiaParamProduct2G> ciaParamProduct2G = config.Map<List<CIAPRODEN.CoProduct2g>, List<CiaParamProduct2G>>(entityProducts);
            return ciaParamProduct2G;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coProduct2G"></param>
        /// <returns></returns>
        public static CiaParamProduct2G CreateCiaParamProduct2G(CIAPRODEN.CoProduct2g coProduct2G)
        {
            CiaParamProduct2G ciaParamProduct2G = new CiaParamProduct2G();
            var config= CreateMapProduct2G();
            config.Map<CIAPRODEN.CoProduct2g, CiaParamProduct2G>(coProduct2G, ciaParamProduct2G);
            return ciaParamProduct2G;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityProducts"></param>
        /// <returns></returns>
        public static List<CiaParamProduct> CreateCiaParamProducts(IList entityProducts)
        {
            List<CiaParamProduct> result = new List<CiaParamProduct>();
            foreach (PRODEN.Product itemProduct in entityProducts)
            {
                //result.Add(CreateCiaParamProduct(itemProduct));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityProduct"></param>
        /// <returns></returns>
        public static CiaParamProduct CreateCiaParamProduct(COMMEN.Prefix productPrefix, PRODEN.Product entityProduct, List<PRODEN.ProductCurrency> productCurrency, INTEN.CoEquivalenceProduct coEquivalenceProduct, List<PRODEN.CoProductPolicyType> coProductPolicyType, List<PRODEN.ProductCoverRiskType> productCoverRiskType)
        {
            CiaParamProduct ciaParamProduct = new CiaParamProduct();
            ciaParamProduct.Prefix = new CiaParamPrefix();
            if (productCurrency.Count > 0)
            {
                ciaParamProduct.Currency = new List<CiaParamCurrency>();
            }
            if (coEquivalenceProduct!=null)
            {
                ciaParamProduct.Product2G = new CiaParamProduct2G();
            }
            if (coProductPolicyType.Count > 0)
            {
                ciaParamProduct.PolicyType = new List<CiaParamPolicyType>();
            }
            //if (cptProductBranchAssistanceType.Count > 0)
            //{
            //    ciaParamProduct.AssistanceType = new List<CiaParamAssistanceType>();
            //}
            if (productCoverRiskType.Count > 0)
            {
                ciaParamProduct.RiskTypes = new List<CiaParamRiskType>();
            }
            //CreateMapCompanyProduct();
            var configPrefix = CreateMapPrefix();
            var configCoreProduct = CreateMapCoreProduct();
            var configProductCurrency = CreateMapProductCurrency();
            var configProductProduct2G = CreateMapProductProduct2G();
            var configProductPolicyType = CreateMapProductPolicyType();
            //CreateMapProductAssistanceType();
            var ProductRiskType = CreateMapProductRiskType();
            configCoreProduct.Map<PRODEN.Product, CiaParamProduct>(entityProduct, ciaParamProduct);
            configPrefix.Map<COMMEN.Prefix, CiaParamPrefix>(productPrefix, ciaParamProduct.Prefix);
            configProductCurrency.Map<List<PRODEN.ProductCurrency>, List<CiaParamCurrency>>(productCurrency, ciaParamProduct.Currency);
            configProductProduct2G.Map<INTEN.CoEquivalenceProduct, CiaParamProduct2G>(coEquivalenceProduct, ciaParamProduct.Product2G);
            configProductPolicyType.Map<List<PRODEN.CoProductPolicyType>, List<CiaParamPolicyType>>(coProductPolicyType, ciaParamProduct.PolicyType);
            ProductRiskType.Map<List<PRODEN.ProductCoverRiskType>, List<CiaParamRiskType>>(productCoverRiskType, ciaParamProduct.RiskTypes);
            
            return ciaParamProduct;
        }

        #region autommaper        
        /// <summary>
        /// Creates the map core product.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapCoreProduct()
        {
            var config = MapperCache.GetMapper<PRODEN.Product, CiaParamProduct>(cfg =>
             {
                 cfg.CreateMap<PRODEN.Product, CiaParamProduct>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
             });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapProductPrefix()
        {
            var config = MapperCache.GetMapper < PRODEN.Product, CiaParamPrefix>(cfg =>
              {
                  cfg.CreateMap<PRODEN.Product, CiaParamPrefix>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PrefixCode))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
              });

            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapPrefix()
        {
            var config = MapperCache.GetMapper<COMMEN.Prefix, CiaParamPrefix>(cfg =>
             {
                 cfg.CreateMap<COMMEN.Prefix, CiaParamPrefix>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PrefixCode))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
             });
            return config;
        }

        /// <summary>
        /// Creates the map product2 g.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapProduct2G()
        {
            var config = MapperCache.GetMapper<CIAPRODEN.CoProduct2g, CiaParamProduct2G>(cfg =>
              {
                  cfg.CreateMap<CIAPRODEN.CoProduct2g, CiaParamProduct2G>()
                  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
              });
            return config;
        }

        /// <summary>
        /// Creates the type of the map beneficiary.
        /// </summary>
        public static IMapper CreateMapBeneficiaryType()
        {
            var config = MapperCache.GetMapper<QUOEN.BeneficiaryType, CiaParamBeneficiaryType>(cfg =>
            {
                cfg.CreateMap<QUOEN.BeneficiaryType, CiaParamBeneficiaryType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BeneficiaryTypeCode))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapProductCurrency()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductCurrency, CiaParamCurrency>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductCurrency, CiaParamCurrency>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CurrencyCode))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapProductProduct2G()
        {
            var config = MapperCache.GetMapper<INTEN.CoEquivalenceProduct, CiaParamProduct2G>(cfg =>
            {

                cfg.CreateMap<INTEN.CoEquivalenceProduct, CiaParamProduct2G>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Product2gId))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapProductPolicyType()
        {
            var config = MapperCache.GetMapper<PRODEN.CoProductPolicyType, CiaParamPolicyType>(cfg =>
            {
                cfg.CreateMap<PRODEN.CoProductPolicyType, CiaParamPolicyType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PolicyTypeCode))
                .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.PrefixCode))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }
        /// <summary>
        /// Creates the type of the map product risk.
        /// </summary>
        public static IMapper CreateMapProductRiskType()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductCoverRiskType, CiaParamRiskType>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductCoverRiskType, CiaParamRiskType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoveredRiskTypeCode))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapProductGroupCoverage()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductGroupCover, CiaParamCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductGroupCover, CiaParamCoverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverGroupId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription))
                .ForMember(dest => dest.RiskTypeId, opt => opt.MapFrom(src => src.CoveredRiskTypeCode))
                .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.PrefixCode))
                .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapProductCoverage()
        {
            var config = MapperCache.GetMapper<PRODEN.GroupCoverage, CiaParamGroupCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.GroupCoverage, CiaParamGroupCoverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverageId))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.CoverNum))
                .ForAllMembers(opt => opt.Condition(r => r != null));

            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapCoverage()
        {
            var config = MapperCache.GetMapper <QUOEN.Coverage, CiaParamGroupCoverage>(cfg =>
            {
                cfg.CreateMap<QUOEN.Coverage, CiaParamGroupCoverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverageId))
                .ForMember(dest => dest.SubLineBusinessId, opt => opt.MapFrom(src => src.SubLineBusinessCode))
                .ForMember(dest => dest.LineBusinessId, opt => opt.MapFrom(src => src.LineBusinessCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.PrintDescription))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapProductInsuredObject()
        {
            var config = MapperCache.GetMapper<PRODEN.GroupInsuredObject, CiaParamInsuredObject>(cfg =>
            {
                cfg.CreateMap<PRODEN.GroupInsuredObject, CiaParamInsuredObject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuredObject))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapFinancialPlan()
        {
            var config = MapperCache.GetMapper<PRODEN.FinancialPlan, CiaParamFinancialPlan>(cfg =>
            {
                cfg.CreateMap<PRODEN.FinancialPlan, CiaParamFinancialPlan>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FinancialPlanId))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => new CiaParamCurrency { Id = src.CurrencyCode }))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => new CiaParamPaymentMethod { Id = src.PaymentMethodCode }))
                .ForMember(dest => dest.PaymentSchedule, opt => opt.MapFrom(src => new CiaParamPaymentSchedule { Id = src.PaymentScheduleId }))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapProductFinancialPlan()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductFinancialPlan, CiaParamFinancialPlan>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductFinancialPlan, CiaParamFinancialPlan>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FinancialPlanId))
                    .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }


        public static IMapper MappLimitsRC()
        {
            var config = MapperCache.GetMapper<COMMEN.CoLimitsRc, CiaParamLimitsRC>(cfg =>
            {
                cfg.CreateMap<COMMEN.CoLimitsRc, CiaParamLimitsRC>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LimitRcCode))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        public static IMapper MappLimitsRCRel()
        {
            var config = MapperCache.GetMapper<COMMEN.CoLimitsRcRel, CiaParamLimitsRC>(cfg =>
            {
                cfg.CreateMap<COMMEN.CoLimitsRcRel, CiaParamLimitsRC>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LimitRcCode))
                    .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.PrefixCode))
                    .ForMember(dest => dest.PolicyTypeId, opt => opt.MapFrom(src => src.PolicyTypeCode))
                    .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                    .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
                    .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        public static IMapper MappRiskCommercialClass()
        {
            var config = MapperCache.GetMapper<RiskCommercialClass, CiaParamCommercialClass>(cfg =>
            {
                cfg.CreateMap<RiskCommercialClass, CiaParamCommercialClass>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RiskCommercialClassCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        public static IMapper MappProductRiskCommercialClass()
        {
            var config = MapperCache.GetMapper<COMMEN.ProductRiskCommercialClass, CiaParamCommercialClass>(cfg =>
            {
                cfg.CreateMap<COMMEN.ProductRiskCommercialClass, CiaParamCommercialClass>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RiskCommercialClassCode))
                 .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.DefaultRiskCommercial))
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                 .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        public static IMapper MappProductForm()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductForm, CiaParamForm>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductForm, CiaParamForm>()
                 .ForMember(dest => dest.StrCurrentFrom, opt => opt.MapFrom(src => src.CurrentFrom.Date.ToString()))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        public static IMapper MappDeductibleProduct()
        {
            var config = MapperCache.GetMapper<QUOEN.Deductible, CiaParamDeductibleProduct>(cfg =>
            {
                cfg.CreateMap<QUOEN.Deductible, CiaParamDeductibleProduct>()
                 .ForMember(dest => dest.DeductId, opt => opt.MapFrom(src => src.DeductId))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false))				 
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        public static IMapper MappDeductibleByProduct()
        {
            var config = MapperCache.GetMapper<COMMEN.DeductibleProduct, CiaParamDeductibleProduct>(cfg =>
            {
                cfg.CreateMap<COMMEN.DeductibleProduct, CiaParamDeductibleProduct>()
                 .ForMember(dest => dest.DeductId, opt => opt.MapFrom(src => src.DeductId))
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                 .ForMember(dest => dest.IsSelected, opt => opt.MapFrom(src => false))
                 .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }
        public static IMapper MappPrefixLineBusiness()
        {
            var config = MapperCache.GetMapper<COMMEN.PrefixLineBusiness, CiaParamPrefixLineBusiness>(cfg =>
            {
                cfg.CreateMap<COMMEN.PrefixLineBusiness, CiaParamPrefixLineBusiness>()
                 .ForMember(dest => dest.PrefixCode, opt => opt.MapFrom(src => src.PrefixCode))
                 .ForMember(dest => dest.LineBusinessCode, opt => opt.MapFrom(src => src.LineBusinessCode))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }

        public static CiaParamLimitsRC MapCiaParamLimitRC(COMMEN.CoLimitsRc limitRCEntity)
        {
            CiaParamLimitsRC ciaParamLimitsRC = new CiaParamLimitsRC();
            var configLimitsRC = MappLimitsRC();
            configLimitsRC.Map<COMMEN.CoLimitsRc, CiaParamLimitsRC>(limitRCEntity, ciaParamLimitsRC);
            return ciaParamLimitsRC;
        }

        public static List<CiaParamLimitsRC> MapCiaParamLimitRCs(List<COMMEN.CoLimitsRc> listLimitRCEntity)
        {
            var configLimitsRC = MappLimitsRC();
            List<CiaParamLimitsRC> ciaParamLimitsRC = configLimitsRC.Map<List<COMMEN.CoLimitsRc>, List<CiaParamLimitsRC>>(listLimitRCEntity);
            return ciaParamLimitsRC;
        }
        
        public static List<CiaParamLimitsRC> MapCiaParamLimitRCRels(List<COMMEN.CoLimitsRcRel> listLimitRCRelEntity)
        {
            var config = MappLimitsRCRel();
            List<CiaParamLimitsRC> ciaParamLimitsRC = config.Map<List<COMMEN.CoLimitsRcRel>, List<CiaParamLimitsRC>>(listLimitRCRelEntity);
            return ciaParamLimitsRC;
        }

        public static CiaParamLimitsRC MapCiaParamLimitRCRel(COMMEN.CoLimitsRcRel limitRCRelEntity)
        {
            CiaParamLimitsRC ciaParamLimitsRC = new CiaParamLimitsRC();
            var config =  MappLimitsRCRel();
            config.Map<COMMEN.CoLimitsRcRel, CiaParamLimitsRC>(limitRCRelEntity, ciaParamLimitsRC);
            return ciaParamLimitsRC;
        }

        public static List<CiaParamCommercialClass> CreateMapRiskCommercialClass(List<RiskCommercialClass> listRiskCommercialClassEntity)
        {
            var config = MappRiskCommercialClass();
            List<CiaParamCommercialClass> ciaParamLimitsRC = config.Map<List<RiskCommercialClass>, List<CiaParamCommercialClass>>(listRiskCommercialClassEntity);
            return ciaParamLimitsRC;
        }

        public static List<CiaParamCommercialClass> CreateMapProductRiskCommercialClass(List<COMMEN.ProductRiskCommercialClass> listProductRiskCommercialClassEntity)
        {
            var config = MappProductRiskCommercialClass();
            List<CiaParamCommercialClass> ciaParamLimitsRC = config.Map<List<COMMEN.ProductRiskCommercialClass>, List<CiaParamCommercialClass>>(listProductRiskCommercialClassEntity);
            return ciaParamLimitsRC;
        }

        public static List<CiaParamForm> CreateMapProductForm(List<PRODEN.ProductForm> listProductFormEntity)
        {
            var config = MappProductForm();
            List<CiaParamForm> ciaParamForm = config.Map<List<PRODEN.ProductForm>, List<CiaParamForm>>(listProductFormEntity);
            return ciaParamForm;
        }

        public static List<CiaParamDeductibleProduct> CreateMapDeductibleProduct(List<QUOEN.Deductible> listDeductibleProductEntity)
        {
            var config = MappDeductibleProduct();
            List<CiaParamDeductibleProduct> ciaParamDeductibleProducts = config.Map<List<QUOEN.Deductible>, List<CiaParamDeductibleProduct>>(listDeductibleProductEntity);
            return ciaParamDeductibleProducts;
        }

        public static List<CiaParamDeductibleProduct> CreateMapDeductibleByProduct(List<COMMEN.DeductibleProduct> listDeductibleProductEntity)
        {
            var config = MappDeductibleByProduct();
            List<CiaParamDeductibleProduct> ciaParamDeductibleProduct = config.Map<List<COMMEN.DeductibleProduct>, List<CiaParamDeductibleProduct>>(listDeductibleProductEntity);
            return ciaParamDeductibleProduct;
        }

        public static List<CiaParamPrefixLineBusiness> CreateMapPrefixLineBusiness(List<COMMEN.PrefixLineBusiness> listPrefixLineBusinessEntity)
        {
            var config = MappPrefixLineBusiness();
            List<CiaParamPrefixLineBusiness> ciaParamPrefixLineBusiness = config.Map<List<COMMEN.PrefixLineBusiness>, List<CiaParamPrefixLineBusiness>>(listPrefixLineBusinessEntity);
            return ciaParamPrefixLineBusiness;
        }

        public static CiaParamCommercialClass MapCiaParamRiskCommercialClass(RiskCommercialClass riskCommercialClassEntity)
        {
            CiaParamCommercialClass ciaParamLimitsRC = new CiaParamCommercialClass();
            var config = MappRiskCommercialClass();
            config.Map<RiskCommercialClass, CiaParamCommercialClass>(riskCommercialClassEntity, ciaParamLimitsRC);
            return ciaParamLimitsRC;
        }

        public static CiaParamCommercialClass MapCiaParamProductRiskCommercialClass(COMMEN.ProductRiskCommercialClass productRiskCommercialClassEntity)
        {
            CiaParamCommercialClass ciaParamLimitsRC = new CiaParamCommercialClass();
            var config = MappProductRiskCommercialClass();
            config.Map<COMMEN.ProductRiskCommercialClass, CiaParamCommercialClass>(productRiskCommercialClassEntity, ciaParamLimitsRC);
            return ciaParamLimitsRC;
        }

        public static CiaParamForm MapCiaParamProductForm(PRODEN.ProductForm productFormEntity)
        {
            CiaParamForm ciaParamForm = new CiaParamForm();
            var config = MappProductForm();
            config.Map<PRODEN.ProductForm, CiaParamForm>(productFormEntity, ciaParamForm);
            return ciaParamForm;
        }

        public static CiaParamDeductibleProduct MapCiaParamDeductibleProduct(QUOEN.Deductible deductibleProductEntity)
        {
            CiaParamDeductibleProduct ciaParamDeductibleProduct = new CiaParamDeductibleProduct();
            var config = MappDeductibleProduct();
            config.Map<QUOEN.Deductible, CiaParamDeductibleProduct>(deductibleProductEntity, ciaParamDeductibleProduct);
            return ciaParamDeductibleProduct;
        }

        public static CiaParamDeductibleProduct MapCiaParamDeductibleByProduct(COMMEN.DeductibleProduct deductibleProductEntity)
        {
            CiaParamDeductibleProduct ciaParamDeductibleProduct = new CiaParamDeductibleProduct();
            var config = MappDeductibleByProduct();
            config.Map<COMMEN.DeductibleProduct, CiaParamDeductibleProduct>(deductibleProductEntity, ciaParamDeductibleProduct);
            return ciaParamDeductibleProduct;
        }

        public static CiaParamPrefixLineBusiness MapCiaPrefixLineBusiness(COMMEN.PrefixLineBusiness prefixLineBusinessEntity)
        {
            CiaParamPrefixLineBusiness ciaParamPrefixLineBusiness = new CiaParamPrefixLineBusiness();
            var config = MappPrefixLineBusiness();
            config.Map<COMMEN.PrefixLineBusiness, CiaParamPrefixLineBusiness>(prefixLineBusinessEntity, ciaParamPrefixLineBusiness);
            return ciaParamPrefixLineBusiness;
        }

        #endregion autommaper
    }
}
