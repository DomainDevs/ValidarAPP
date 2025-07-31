using Sistran.Company.Application.ProductParamService.Models;
using Sistran.Company.Application.ModelServices.Models.Product;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;

namespace Sistran.Company.Application.ProductParamService.EEProvider.Assemblers
{
    /// <summary>
    /// Convierte el modelo del negocio al modelo del servicio
    /// </summary>
    public static class ModelsServicesAssembler
    {
        public static List<CiaParamAgentServiceModel> CreateCiaParamAgentsServiceModel(List<CiaParamAgent> ciaParamAgents)
        {
            var imapp = CreateMapParamProductAgent();            
            return imapp.Map<List<CiaParamAgent>, List<CiaParamAgentServiceModel>>(ciaParamAgents);
        }

        public static CiaParamSummaryAgentServiceModel CreateCiaParamAgentServiceModel(CiaParamSummaryAgent ciaParamSummaryAgent)
        {
            CiaParamSummaryAgentServiceModel ciaParamSummaryAgentSM = new CiaParamSummaryAgentServiceModel();
            ciaParamSummaryAgentSM.AgentsCommission = ciaParamSummaryAgent.AgentsCommission;
            ciaParamSummaryAgentSM.AgentsIncentives = ciaParamSummaryAgent.AgentsIncentives;
            ciaParamSummaryAgentSM.AssignedAgents = ciaParamSummaryAgent.AssignedAgents;
            ciaParamSummaryAgentSM.UnassignedAgents = ciaParamSummaryAgent.UnassignedAgents;
            return ciaParamSummaryAgentSM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static CiaParamAgentServiceModel CreateCiaParamAgentServiceModel(CiaParamAgent ciaParamAgent)
        {
            CreateMapParamAgencyCommiss();
            var imapp = CreateMapParamProductAgent();
            return imapp.Map<CiaParamAgent, CiaParamAgentServiceModel>(ciaParamAgent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamDeductiblesCoverages"></param>
        /// <returns></returns>
        public static List<CiaParamDeductiblesCoverageServiceModel> CreateCiaParamDeductiblesCoveragesServiceModel(List<CiaParamDeductiblesCoverage> ciaParamDeductiblesCoverages)
        {
            var imapp = CreateMapParamDeductiblesCoverage();
            return imapp.Map<List<CiaParamDeductiblesCoverage>, List<CiaParamDeductiblesCoverageServiceModel>>(ciaParamDeductiblesCoverages);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamDeductiblesCoverage"></param>
        /// <returns></returns>
        public static CiaParamDeductiblesCoverageServiceModel CreateCiaParamDeductiblesCoverageServiceModel(CiaParamDeductiblesCoverage ciaParamDeductiblesCoverage)
        {
            var imapp = CreateMapParamDeductiblesCoverage();
            return imapp.Map<CiaParamDeductiblesCoverage, CiaParamDeductiblesCoverageServiceModel>(ciaParamDeductiblesCoverage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamBeneficiaryTypes"></param>
        /// <returns></returns>
        public static List<CiaParamBeneficiaryTypeServiceModel> CreateCiaParamBeneficiaryTypesServiceModel(List<CiaParamBeneficiaryType> ciaParamBeneficiaryTypes)
        {
            var imapp = CreateMapParamBeneficiaryType();
            return imapp.Map<List<CiaParamBeneficiaryType>, List<CiaParamBeneficiaryTypeServiceModel>>(ciaParamBeneficiaryTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamBeneficiaryType"></param>
        /// <returns></returns>
        public static CiaParamBeneficiaryTypeServiceModel CreateCiaParamBeneficiaryTypeServiceModel(CiaParamBeneficiaryType ciaParamBeneficiaryType)
        {
            var imapp = CreateMapParamBeneficiaryType();
            return imapp.Map<CiaParamBeneficiaryType, CiaParamBeneficiaryTypeServiceModel>(ciaParamBeneficiaryType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamAssistanceTypes"></param>
        /// <returns></returns>
        public static List<CiaParamFinancialPlanServiceModel> CreateCiaParamFinancialPlansServiceModel(List<CiaParamFinancialPlan> ciaParamFinancialPlan)
        {
            var imapp = CreateMapParamFinancialPlan();
            //CreateMapParamPaymentMethod();
            //CreateMapParamPaymentSchedule();
            //CreateMapParamCurrency();
            return imapp.Map<List<CiaParamFinancialPlan>, List<CiaParamFinancialPlanServiceModel>>(ciaParamFinancialPlan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProduct2G"></param>
        /// <returns></returns>
        public static CiaParamFinancialPlanServiceModel CreateCiaParamFinancialPlanServiceModel(CiaParamFinancialPlan ciaParamFinancialPlan)
        {
            var imapp = CreateMapParamFinancialPlan();
            CreateMapParamPaymentMethod();
            CreateMapParamPaymentSchedule();
            CreateMapParamCurrency();
            return imapp.Map<CiaParamFinancialPlan, CiaParamFinancialPlanServiceModel>(ciaParamFinancialPlan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamAssistanceTypes"></param>
        /// <returns></returns>
        public static List<CiaParamAssistanceTypeServiceModel> CreateCiaParamAssistanceTypesServiceModel(List<CiaParamAssistanceType> ciaParamAssistanceTypes)
        {
            var imapp = CreateMapParamAssistanceType();

            return imapp.Map<List<CiaParamAssistanceType>, List<CiaParamAssistanceTypeServiceModel>>(ciaParamAssistanceTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProduct2G"></param>
        /// <returns></returns>
        public static CiaParamAssistanceTypeServiceModel CreateCiaParamAssistanceTypeServiceModel(CiaParamAssistanceType ciaParamAssistanceType)
        {
            var imapp = CreateMapParamAssistanceType();

            return imapp.Map<CiaParamAssistanceType, CiaParamAssistanceTypeServiceModel>(ciaParamAssistanceType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProducts2G"></param>
        /// <returns></returns>
        public static List<CiaParamProduct2GServiceModel> CreateCiaParamProducts2GServiceModel(List<CiaParamProduct2G> ciaParamProducts2G)
        {
            var imapp = CreateMapParamProduct2G();

            return imapp.Map<List<CiaParamProduct2G>, List<CiaParamProduct2GServiceModel>>(ciaParamProducts2G);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProduct2G"></param>
        /// <returns></returns>
        public static CiaParamProduct2GServiceModel CreateCiaParamProduct2GServiceModel(CiaParamProduct2G ciaParamProduct2G)
        {
            var imapp = CreateMapParamProduct2G();

            return imapp.Map<CiaParamProduct2G, CiaParamProduct2GServiceModel>(ciaParamProduct2G);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProducts"></param>
        /// <returns></returns>
        public static List<CiaParamProductServiceModel> CreateCiaParamProductsServiceModel(List<CiaParamProduct> ciaParamProducts)
        {
            var imapp = CreateMapParamProduct();
            CreateMapParamPrefix();
            CreateMapParamCurrency();
            CreateMapParamProduct2G();
            CreateMapParamPolicyType();
            CreateMapParamRiskType();
            CreateMapParamGroupCoverage();
            CreateMapParamCoverages();
            CreateMapParamCoverage();
            CreateMapParamInsuredObject();
            CreateMapParamFinancialPlan();
            CreateMapParamPaymentMethod();
            CreateMapParamPaymentSchedule();
            CreateMapParamForm();
            return imapp.Map<List<CiaParamProduct>, List<CiaParamProductServiceModel>>(ciaParamProducts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProduct"></param>
        /// <returns></returns>
        public static CiaParamProductServiceModel CreateCiaParamProductServiceModel(CiaParamProduct ciaParamProduct)
        {
            var imapp = CreateMapParamProduct();
            //CreateMapParamPrefix();
            //CreateMapParamCurrency();
            //CreateMapParamProduct2G();
            //CreateMapParamPolicyType();
            //CreateMapParamAssistanceType();
            //CreateMapParamRiskType();
            //CreateMapParamGroupCoverage();
            //CreateMapParamCoverages();
            //CreateMapParamCoverage();
            //CreateMapParamInsuredObject();
            //CreateMapParamFinancialPlan();
            //CreateMapParamPaymentMethod();
            //CreateMapParamPaymentSchedule();
            //CreateMapParamForm();
            return imapp.Map<CiaParamProduct, CiaParamProductServiceModel>(ciaParamProduct);
        }

        public static List<CiaParamLimitsRCServiceModel> CreateCiaParamLimitsRCsServiceModel(List<CiaParamLimitsRC> ciaParamLimitsRC)
        {
            var imapp = CreateMappLimitsRCRel();
            return imapp.Map<List<CiaParamLimitsRC>, List<CiaParamLimitsRCServiceModel>>(ciaParamLimitsRC);
        }
                
        public static CiaParamLimitsRCServiceModel CreateCiaParamLimitsRCServiceModel(CiaParamLimitsRC ciaParamLimitsRC)
        {
            var imapp = CreateMappLimitsRCRel();
            return imapp.Map<CiaParamLimitsRC, CiaParamLimitsRCServiceModel>(ciaParamLimitsRC);
        }

        public static List<CiaParamCommercialClassServiceModel> CreateCiaParamCommercialClassServiceModel(List<CiaParamCommercialClass> ciaParamCommercialClasses)
        {
            var imapp = CreateMappCommercialClass();
            return imapp.Map<List<CiaParamCommercialClass>, List<CiaParamCommercialClassServiceModel>>(ciaParamCommercialClasses);
        }

        public static List<CiaParamDeductibleProductServiceModel> CreateCiaParamDeductiblesProductServiceModel(List<CiaParamDeductibleProduct> ciaParamDeductibleProducts)
        {
            var imapp = CreateMappDeductible();
            return imapp.Map<List<CiaParamDeductibleProduct>, List<CiaParamDeductibleProductServiceModel>>(ciaParamDeductibleProducts);
        }

        public static CiaParamCommercialClassServiceModel CreateCiaParamCommercialClassServiceModel(CiaParamCommercialClass ciaParamCommercialClass)
        {
            var imapp = CreateMappCommercialClass();
            return imapp.Map<CiaParamCommercialClass, CiaParamCommercialClassServiceModel>(ciaParamCommercialClass);
        }

        public static List<CiaParamFormServiceModel> CreateCiaParamFormsServiceModel(List<CiaParamForm> ciaParamForms)
        {
            var imapp = CreateMappForm();
            return imapp.Map<List<CiaParamForm>, List<CiaParamFormServiceModel>>(ciaParamForms);
        }
        public static List<CiaParamDeductibleProductServiceModel> CreateCiaParamDeductibleProductsServiceModel(List<CiaParamDeductibleProduct> ciaParamDeductibleProducts)
        {
            var imapp = CreateMappDeductible();
            return imapp.Map<List<CiaParamDeductibleProduct>, List<CiaParamDeductibleProductServiceModel>>(ciaParamDeductibleProducts);
        }


        public static CiaParamFormServiceModel CreateCiaParamFormServiceModel(CiaParamForm ciaParamFormServiceModel)
        {
            var imapp = CreateMappForm();
            return imapp.Map<CiaParamForm, CiaParamFormServiceModel>(ciaParamFormServiceModel);
        }

        public static CiaParamDeductibleProductServiceModel CreateCiaParamDeductibleProductServiceModel(CiaParamDeductibleProduct ciaParamDeductibleProduct)
        {
            var imapp = CreateMappDeductible();
            return imapp.Map<CiaParamDeductibleProduct, CiaParamDeductibleProductServiceModel>(ciaParamDeductibleProduct);
        }

        #region autommaper
        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamProduct()
        {
            var config = new MapperConfiguration(cfg =>
              {
                  cfg.CreateMap<CiaParamProduct, CiaParamProductServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamPrefix, CiaParamPrefixServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamCurrency, CiaParamCurrencyServiceModel>()
                   .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                   .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamProduct2G, CiaParamProduct2GServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamPolicyType, CiaParamPolicyTypeServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamAssistanceType, CiaParamAssistanceTypeServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamRiskType, CiaParamRiskTypeServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamGroupCoverage, CiaParamGroupCoverageServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamCoverages, CiaParamCoveragesServiceModel>()
                 .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamCoverage, CiaParamCoverageServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamInsuredObject, CiaParamInsuredObjectServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamFinancialPlan, CiaParamFinancialPlanServiceModel>()
                  .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                  .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamPaymentMethod, CiaParamPaymentMethodServiceModel>()
                   .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                   .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamPaymentSchedule, CiaParamPaymentScheduleServiceModel>()
                   .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                   .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamCurrency, CiaParamCurrencyServiceModel>()
                   .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                   .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                   .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamPaymentMethod, CiaParamPaymentMethodServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamPaymentSchedule, CiaParamPaymentScheduleServiceModel>()
               .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
               .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                 .ForAllMembers(opt => opt.Condition(r => r != null));
                  cfg.CreateMap<CiaParamForm, CiaParamFormServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));

              });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamPrefix()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamPrefix, CiaParamPrefixServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamCurrency()
        {
            var config = new MapperConfiguration(cfg =>
              {
                  cfg.CreateMap<CiaParamCurrency, CiaParamCurrencyServiceModel>()
                   .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                   .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                   .ForMember(des => des.DecimalQuantity, opt => opt.MapFrom(src => src.DecimalQuantity))
                  .ForAllMembers(opt => opt.Condition(r => r != null));
              });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamProduct2G()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamProduct2G, CiaParamProduct2GServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamPolicyType()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamPolicyType, CiaParamPolicyTypeServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamAssistanceType()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamAssistanceType, CiaParamAssistanceTypeServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamRiskType()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamRiskType, CiaParamRiskTypeServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamGroupCoverage()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamGroupCoverage, CiaParamGroupCoverageServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamCoverages()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamCoverages, CiaParamCoveragesServiceModel>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamCoverage()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamCoverage, CiaParamCoverageServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamInsuredObject()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamInsuredObject, CiaParamInsuredObjectServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamFinancialPlan()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamFinancialPlan, CiaParamFinancialPlanServiceModel>()
                  .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                  .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPaymentMethod, CiaParamPaymentMethodServiceModel>()
                 .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                 .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                 .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPaymentSchedule, CiaParamPaymentScheduleServiceModel>()
                 .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                 .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                 .ForAllMembers(opt => opt.Condition(r => r != null));                
                cfg.CreateMap<CiaParamCurrency, CiaParamCurrencyServiceModel>()
                 .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                 .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                 .ForAllMembers(opt => opt.Condition(r => r != null));
                
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamDeductiblesCoverage()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamDeductiblesCoverage, CiaParamDeductiblesCoverageServiceModel>()
                 .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                 .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamBeneficiaryType()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamBeneficiaryType, CiaParamBeneficiaryTypeServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamPaymentMethod()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamPaymentMethod, CiaParamPaymentMethodServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamPaymentSchedule()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamPaymentSchedule, CiaParamPaymentScheduleServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamForm()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamForm, CiaParamFormServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }



        public static IMapper CreateMappLimitsRCRel()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamLimitsRC, CiaParamLimitsRCServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMappCommercialClass()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamCommercialClass, CiaParamCommercialClassServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMappForm()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamForm, CiaParamFormServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                });
            return config.CreateMapper();
        }

        public static IMapper CreateMappDeductible()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamDeductibleProduct, CiaParamDeductibleProductServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapParamProductAgent()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamAgent, CiaParamAgentServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamAgencyCommiss, CiaParamAgencyCommissServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapParamAgencyCommiss()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamAgencyCommiss, CiaParamAgencyCommissServiceModel>()
                .BeforeMap((s, d) => d.StatusTypeService = StatusTypeService.Original)
                .BeforeMap((s, d) => d.ErrorServiceModel = new ErrorServiceModel { ErrorTypeService = ErrorTypeService.Ok })
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }
        #endregion autommaper
    }
}
