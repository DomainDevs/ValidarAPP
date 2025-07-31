using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    using AutoMapper;
    using Sistran.Company.Application.ModelServices.Models.Product;
    using Sistran.Company.Application.UnderwritingServices.Models;
    using Sistran.Company.Application.UniquePersonServices.V1.Models;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Linq;    
    using Sistran.Core.Application.Utilities.Cache;

    internal class ModelAssembler
    {        
        internal static Currency CreateCurrency(CurrencyModelsView currencyModelsView)
        {
            return new Currency
            {
                Id = currencyModelsView.Id,
                Description = currencyModelsView.Description
            };
        }
        
        internal static PolicyType CreatePolicyType(PolicyTypeModelsView policyTypeModelsView)
        {
            return new PolicyType
            {
                Id = policyTypeModelsView.Id,
                Description = policyTypeModelsView.Description,
                IsDefault = policyTypeModelsView.IsDefault
            };
        }

        public static List<ProductFinancialPlanModelsView> CreateProductFinancialPlansModelsView(List<CiaParamFinancialPlanServiceModel> ciaParamFinancialPlansServiceModel)
        {
            var immap = CreateProductFinancial();
            List<ProductFinancialPlanModelsView> productFinancialPlanModelsView = immap.Map<List<CiaParamFinancialPlanServiceModel>, List<ProductFinancialPlanModelsView>>(ciaParamFinancialPlansServiceModel);
            return productFinancialPlanModelsView;
        }

        public static IMapper CreateProductFinancial()
        {
            var config = MapperCache.GetMapper<CiaParamFinancialPlanServiceModel, ProductFinancialPlanModelsView>(cfg =>
            {
                cfg.CreateMap<CiaParamFinancialPlanServiceModel, ProductFinancialPlanModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamCurrencyServiceModel, CurrencyModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPaymentMethodServiceModel, PaymentMethodModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPaymentScheduleServiceModel, PaymentScheduleModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config;
        }
        public static ProductFinancialPlanModelsView CreateProductFinancialPlanModelsView(CiaParamFinancialPlanServiceModel ciaParamFinancialPlanServiceModel)
        {

            ProductFinancialPlanModelsView productFinancialPlanModelsView = new ProductFinancialPlanModelsView();
            var immap = CreateProductFinancial();
            immap.Map<CiaParamFinancialPlanServiceModel, ProductFinancialPlanModelsView>(ciaParamFinancialPlanServiceModel, productFinancialPlanModelsView);
            return productFinancialPlanModelsView;
        }

        public static List<DeductByCoverProductModelView> CreateDeductiblesByCoverProductModelView(List<CiaParamDeductiblesCoverageServiceModel> ciaParamDeductiblesCoverageServiceModels)
        {
            if (ciaParamDeductiblesCoverageServiceModels != null)
            {
                var mapper = CreateMapDeductibles();
                List<DeductByCoverProductModelView> deductByCoverProductModelView = mapper.Map<List<CiaParamDeductiblesCoverageServiceModel>, List<DeductByCoverProductModelView>>(ciaParamDeductiblesCoverageServiceModels);
                return deductByCoverProductModelView;
            }
            else
            {
                return null;
            }

        }
        public static DeductByCoverProductModelView CreateDeductByCoverProductModelView(CiaParamDeductiblesCoverageServiceModel ciaParamDeductiblesCoverageServiceModel)
        {
            DeductByCoverProductModelView deductByCoverProductModelView = new DeductByCoverProductModelView();
            var mapper = CreateMapDeductibles();
            mapper.Map<CiaParamDeductiblesCoverageServiceModel, DeductByCoverProductModelView>(ciaParamDeductiblesCoverageServiceModel, deductByCoverProductModelView);
            return deductByCoverProductModelView;
        }

        public static List<ProductAssistanceTypeModelsView> CreateAssistanceTypesViewModel(List<CiaParamAssistanceTypeServiceModel> ciaParamAssistanceTypeServiceModels)
        {
            var mapper = CreateMapAssistanceType();
            List<ProductAssistanceTypeModelsView> productAssistanceTypeModelsView = mapper.Map<List<CiaParamAssistanceTypeServiceModel>, List<ProductAssistanceTypeModelsView>>(ciaParamAssistanceTypeServiceModels);
            return productAssistanceTypeModelsView;
        }
        public static ProductAssistanceTypeModelsView CreateAssistanceTypeViewModel(CiaParamAssistanceTypeServiceModel ciaParamAssistanceTypeServiceModel)
        {
            ProductAssistanceTypeModelsView productAssistanceTypeModelsView = new ProductAssistanceTypeModelsView();
            var mapper = CreateMapAssistanceType();
            mapper.Map<CiaParamAssistanceTypeServiceModel, ProductAssistanceTypeModelsView>(ciaParamAssistanceTypeServiceModel, productAssistanceTypeModelsView);
            return productAssistanceTypeModelsView;
        }

        public static List<ProductModelsView> CreateProductsViewModel(List<CiaParamProductServiceModel> ciaParamProductServiceModels)
        {
            var immap = CreateProductView();
            List<ProductModelsView> productModelsView = immap.Map<List<CiaParamProductServiceModel>, List<ProductModelsView>>(ciaParamProductServiceModels);
            return productModelsView;
        }

        public static ProductModelsView CreateProductViewModel(CiaParamProductServiceModel ciaParamProductServiceModel)
        {
            ProductModelsView productModelsView = new ProductModelsView();
            var immap = CreateProductView();
            immap.Map<CiaParamProductServiceModel, ProductModelsView>(ciaParamProductServiceModel, productModelsView);
            return productModelsView;
        }

        public static CiaParamProductServiceModel CreateProductServiceModel(ProductModelsView productModelsView)
        {
            var immap = CreateProductService();
            return immap.Map<ProductModelsView, CiaParamProductServiceModel>(productModelsView);
        }

        
        #region autommaper base
        public static IMapper CreateMapParamProduct()
        {
            var config = MapperCache.GetMapper<CiaParamProductServiceModel, ProductModelsView>(cfg =>
            {

                cfg.CreateMap<CiaParamProductServiceModel, ProductModelsView>()
                .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.Prefix.Id))
                .ForMember(dest => dest.PolicyType, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.DescriptionReduced, opt => opt.MapFrom(src => src.SmallDescription))
                .ForMember(dest => dest.CurrentDate, opt => opt.MapFrom(src => src.CurrentFrom))
                .ForMember(dest => dest.DisabledDate, opt => opt.MapFrom(src => src.CurrentTo))
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.StandardCommissionPercentage))
                .ForMember(dest => dest.PercentageAdditional, opt => opt.MapFrom(src => src.AdditionalCommissionPercentage))
                .ForMember(dest => dest.AdditDisCommissPercentage, opt => opt.MapFrom(src => src.AdditionalCommissionPercentage))
                .ForMember(dest => dest.IsPremium, opt => opt.MapFrom(src => src.CalculateMinPremium))
                .ForMember(dest => dest.VersionId, opt => opt.MapFrom(src => src.Version))
                .ForMember(dest => dest.Product2G, opt => opt.MapFrom(src => src.Product2G.Id))                
                .ForMember(dest => dest.PolicyTypes, opt => opt.MapFrom(src => src.PolicyType))
                .ForMember(dest => dest.Currencies, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ProductCoveredRisks, opt => opt.MapFrom(src => src.RiskTypes))
                .ForAllMembers(opt => opt.Condition(r => r != null));
               
            });
            return config;

        }

        public static IMapper CreateMapParamCurrency()
        {
            var config = MapperCache.GetMapper<CiaParamCurrencyServiceModel, CurrencyModelsView>(cfg =>
            {
                cfg.CreateMap<CiaParamCurrencyServiceModel, CurrencyModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config;

        }

        public static IMapper CreateMapPolicyType()
        {
            var config = MapperCache.GetMapper<CiaParamPolicyTypeServiceModel, PolicyTypeModelsView>(cfg =>
            {

                cfg.CreateMap<CiaParamPolicyTypeServiceModel, PolicyTypeModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config;
        }

        public static IMapper CreateMapLimitRC()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamLimitsRCServiceModel, LimitRCModelView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapRiskType()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamRiskTypeServiceModel, RiskTypesModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapGroupCoverage()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamCoverageServiceModel, ProductGroupCoverageModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapInsuredObject()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamInsuredObjectServiceModel, InsuredObjectModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapForm()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamFormServiceModel, FormsModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapCoverages()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamCoveragesServiceModel, ProductCoveragesModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapCoverage()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamGroupCoverageServiceModel, ProductCoverageModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapDeductibles()
        {
            var config = MapperCache.GetMapper<CiaParamDeductiblesCoverageServiceModel, DeductByCoverProductModelView>(cfg =>
            {
                cfg.CreateMap<CiaParamDeductiblesCoverageServiceModel, DeductByCoverProductModelView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config;
        }

        public static IMapper CreateMapAssistanceType()
        {
            var config = MapperCache.GetMapper<CiaParamAssistanceTypeServiceModel, ProductAssistanceTypeModelsView>(cfg =>
            {
                cfg.CreateMap<CiaParamAssistanceTypeServiceModel, ProductAssistanceTypeModelsView>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssistanceId))
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config;
        }

        public static IMapper CreateMapFinancialPlan()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamFinancialPlanServiceModel, ProductFinancialPlanModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapPaymentMethod()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamPaymentMethodServiceModel, PaymentMethodModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapPaymentSchedule()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamPaymentScheduleServiceModel, PaymentScheduleModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapParamProductServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModelsView, CiaParamProductServiceModel>()
                .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => new CiaParamPrefixServiceModel { Id = src.PrefixId, StatusTypeService = (StatusTypeService)src.StatusTypeService }))

                .ForMember(dest => dest.SmallDescription, opt => opt.MapFrom(src => src.DescriptionReduced))
                .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => src.CurrentDate))
                .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => src.DisabledDate))
                .ForMember(dest => dest.StandardCommissionPercentage, opt => opt.MapFrom(src => src.Percentage))
                .ForMember(dest => dest.AdditionalCommissionPercentage, opt => opt.MapFrom(src => src.PercentageAdditional))
                .ForMember(dest => dest.CalculateMinPremium, opt => opt.MapFrom(src => src.IsPremium))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.VersionId))                
                .ForMember(dest => dest.PolicyType, opt => opt.MapFrom(src => src.PolicyTypes))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currencies))
                .ForMember(dest => dest.RiskTypes, opt => opt.MapFrom(src => src.ProductCoveredRisks))
                .ForAllMembers(opt => opt.Condition(srs => srs != null));
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapParamCurrencyServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CurrencyModelsView, CiaParamCurrencyServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapPolicyTypeServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PolicyTypeModelsView, CiaParamPolicyTypeServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapLimitRCServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LimitRCModelView, CiaParamLimitsRCServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapRiskTypeServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RiskTypesModelsView, CiaParamRiskTypeServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapGroupCoverageServiceModel()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductGroupCoverageModelsView, CiaParamCoverageServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapInsuredObjectServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InsuredObjectModelsView, CiaParamInsuredObjectServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapFormServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FormsModelsView, CiaParamFormServiceModel>()
             .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapCoveragesServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
          {
              cfg.CreateMap<ProductCoveragesModelsView, CiaParamCoveragesServiceModel>()
                .ForAllMembers(opt => opt.Condition(r => r != null));              
          });
            return config.CreateMapper();
        }

        public static IMapper CreateMapCoverageServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductCoverageModelsView, CiaParamGroupCoverageServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapDeductiblesServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DeductByCoverProductModelView, CiaParamDeductiblesCoverageServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                

            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapFinancialPlanServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductFinancialPlanModelsView, CiaParamFinancialPlanServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapPaymentMethodServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentMethodModelsView, CiaParamPaymentMethodServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapPaymentScheduleServiceModel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentScheduleModelsView, CiaParamPaymentScheduleServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));                
            });
            return config.CreateMapper();
        }
        public static IMapper CreateMapCompanyInsured()
        {
            var config = MapperCache.GetMapper<CompanyInsured, CompanyIssuanceInsured>(cfg =>
            {
                cfg.CreateMap<CompanyInsured, CompanyIssuanceInsured>()
             .ForMember(x => x.IndividualType, opts => opts.MapFrom(src => IndividualType.Person))
             .ForMember(x => x.CustomerType, opts => opts.MapFrom(src => CustomerType.Individual))
             .ForMember(x => x.CompanyName, opts => opts.MapFrom(src => new IssuanceCompanyName
             {                 
                 Address = new IssuanceAddress
                 {                     
                 },
                 Phone = new IssuancePhone
                 {                     
                 },
                 Email = new IssuanceEmail
                 {                     
                 }
             }))
               .ForMember(x => x.PaymentMethod, opts => opts.MapFrom(src => new IssuancePaymentMethod
               {                   
               }))
               .ForMember(x => x.EconomicActivity, opts => opts.MapFrom(src => new IssuanceEconomicActivity
               {                   
               }));
                cfg.CreateMap<List<CompanyInsured>, List<CompanyIssuanceInsured>>();
            });
            return config;
        }
        #endregion autommaper
        #region automapper


        public static IMapper CreateProductView()
        {
            var config = MapperCache.GetMapper<CiaParamProductServiceModel, ProductModelsView>(cfg =>
            {

                cfg.CreateMap<CiaParamProductServiceModel, ProductModelsView>()
                     .ForMember(dest => dest.PercentageAdditional, opt => opt.MapFrom(src => src.AdditCommissPercentage))
                     .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.Prefix.Id))
                     .ForMember(dest => dest.PolicyType, opt => opt.MapFrom(src => 0))
                     .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => 0))
                     .ForMember(dest => dest.DescriptionReduced, opt => opt.MapFrom(src => src.SmallDescription))
                     .ForMember(dest => dest.CurrentDate, opt => opt.MapFrom(src => src.CurrentFrom))
                     .ForMember(dest => dest.DisabledDate, opt => opt.MapFrom(src => src.CurrentTo))
                     .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.StandardCommissionPercentage))                     
                     .ForMember(dest => dest.IsPremium, opt => opt.MapFrom(src => src.CalculateMinPremium))
                     .ForMember(dest => dest.VersionId, opt => opt.MapFrom(src => src.Version))
                     .ForMember(dest => dest.Product2G, opt => opt.MapFrom(src => src.Product2G.Id))                     
                     .ForMember(dest => dest.PolicyTypes, opt => opt.MapFrom(src => src.PolicyType))
                     .ForMember(dest => dest.Currencies, opt => opt.MapFrom(src => src.Currency))
                     .ForMember(dest => dest.ProductCoveredRisks, opt => opt.MapFrom(src => src.RiskTypes))                     
                     .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamCurrencyServiceModel, CurrencyModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                

                cfg.CreateMap<CiaParamPolicyTypeServiceModel, PolicyTypeModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));                
                cfg.CreateMap<CiaParamLimitsRCServiceModel, LimitRCModelView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamRiskTypeServiceModel, RiskTypesModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamCoverageServiceModel, ProductGroupCoverageModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamInsuredObjectServiceModel, InsuredObjectModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamFormServiceModel, FormsModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamCoveragesServiceModel, ProductCoveragesModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamGroupCoverageServiceModel, ProductCoverageModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamDeductiblesCoverageServiceModel, DeductByCoverProductModelView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamAssistanceTypeServiceModel, ProductAssistanceTypeModelsView>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AssistanceId))
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamFinancialPlanServiceModel, ProductFinancialPlanModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamPaymentMethodServiceModel, PaymentMethodModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CiaParamPaymentScheduleServiceModel, PaymentScheduleModelsView>()
                .ForAllMembers(opt => opt.Condition(r => r != null));
                
            });
            return config;
        }


        public static IMapper CreateProductService()
        {
            var config = MapperCache.GetMapper<ProductModelsView, CiaParamProductServiceModel>(cfg =>
            {
                cfg.CreateMap<ProductModelsView, CiaParamProductServiceModel>()
                .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => new CiaParamPrefixServiceModel { Id = src.PrefixId, StatusTypeService = (StatusTypeService)src.StatusTypeService }))

                .ForMember(dest => dest.SmallDescription, opt => opt.MapFrom(src => src.DescriptionReduced))
                .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => src.CurrentDate))
                .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => src.DisabledDate))
                .ForMember(dest => dest.StandardCommissionPercentage, opt => opt.MapFrom(src => src.Percentage))
                .ForMember(dest => dest.AdditionalCommissionPercentage, opt => opt.MapFrom(src => src.PercentageAdditional))
                .ForMember(dest => dest.CalculateMinPremium, opt => opt.MapFrom(src => src.IsPremium))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.VersionId))
                .ForMember(dest => dest.Product2G, opt => opt.MapFrom(src => new CiaParamProduct2GServiceModel { Id = (src.Product2G == null) ? 0 : (int)src.Product2G, StatusTypeService = (StatusTypeService)src.StatusTypeService }))
                
                .ForMember(dest => dest.PolicyType, opt => opt.MapFrom(src => src.PolicyTypes))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currencies))
                .ForMember(dest => dest.RiskTypes, opt => opt.MapFrom(src => src.ProductCoveredRisks))
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<CurrencyModelsView, CiaParamCurrencyServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<PolicyTypeModelsView, CiaParamPolicyTypeServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<LimitRCModelView, CiaParamLimitsRCServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<RiskTypesModelsView, CiaParamRiskTypeServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<ProductGroupCoverageModelsView, CiaParamCoverageServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<InsuredObjectModelsView, CiaParamInsuredObjectServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<FormsModelsView, CiaParamFormServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<ProductCoveragesModelsView, CiaParamCoveragesServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<ProductCoverageModelsView, CiaParamGroupCoverageServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<DeductByCoverProductModelView, CiaParamDeductiblesCoverageServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<ProductFinancialPlanModelsView, CiaParamFinancialPlanServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<PaymentMethodModelsView, CiaParamPaymentMethodServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<PaymentScheduleModelsView, CiaParamPaymentScheduleServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
                cfg.CreateMap<ProductPrefixModelsView, CiaParamPrefixServiceModel>()
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                
            });
            return config;
        }
        #endregion
    }
}
