
using AutoMapper;
using Sistran.Company.Application.ModelServices.Models.Product;
using Sistran.Company.Application.ProductParamService.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.ProductParamService.EEProvider.Assemblers
{


    /// <summary>
    /// Convierte el Modelo del servicio al modelo del negocio
    /// </summary>
    public static class ServicesModelsAssembler
    {
        public static CiaParamAgent CreateCiaParamAgent(CiaParamAgentServiceModel ciaParamAgentServiceModel)
        {
            var imapp = CreateMapParamAgent();
            //CreateMapParamAgencyCommiss();
            return imapp.Map<CiaParamAgentServiceModel, CiaParamAgent>(ciaParamAgentServiceModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductsServiceModel"></param>
        /// <returns></returns>
        public static List<CiaParamAgent> CreateCiaParamAgents(List<CiaParamAgentServiceModel> ciaParamAgentsServiceModel)
        {
            var imapp = CreateMapParamAgent();
            //CreateMapParamAgencyCommiss();

            return imapp.Map<List<CiaParamAgentServiceModel>, List<CiaParamAgent>>(ciaParamAgentsServiceModel);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamCopyProductServiceModel"></param>
        /// <returns></returns>
        public static CiaParamCopyProduct CreateCiaParamCopyProduct(CiaParamCopyProductServiceModel ciaParamCopyProductServiceModel)
        {
            var imapp =CreateMapParamCopyProduct();
            return imapp.Map<CiaParamCopyProductServiceModel, CiaParamCopyProduct>(ciaParamCopyProductServiceModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductsServiceModel"></param>
        /// <returns></returns>
        public static List<CiaParamProduct> CreateCiaParamProducts(List<CiaParamProductServiceModel> ciaParamProductsServiceModel)
        {
            var imapp = CreateMapParamProduct();
            //CreateMapParamPrefix();
            //CreateMapParamCurrency();
            //CreateMapParamProduct2G();
            //CreateMapParamPolicyType();

            //CreateMapParamRiskType();
            //CreateMapParamGroupCoverage();
            //CreateMapParamCoverages();
            //CreateMapParamCoverage();
            //CreateMapParamInsuredObject();
            //CreateMapParamFinancialPlan();
            //CreateMapParamPaymentSchedule();
            //CreateMapParamPaymentMethod();
            //CreateMapParamForm();
            //CreateMapParamDeductiblesCoverage();

            return imapp.Map<List<CiaParamProductServiceModel>, List<CiaParamProduct>>(ciaParamProductsServiceModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductServiceModel"></param>
        /// <returns></returns>
        public static CiaParamProduct CreateCiaParamProduct(CiaParamProductServiceModel ciaParamProductServiceModel)
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
            //CreateMapParamPaymentSchedule();
            //CreateMapParamPaymentMethod();
            //CreateMapParamForm();
            //CreateMapParamDeductiblesCoverage();

            return imapp.Map<CiaParamProductServiceModel, CiaParamProduct>(ciaParamProductServiceModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductsServiceModel"></param>
        /// <returns></returns>
        public static List<CiaParamFinancialPlan> CreateCiaParamFinancialPlans(List<CiaParamFinancialPlanServiceModel> ciaParamFinancialPlans)
        {
            //CreateMapParamCurrency();
            var imapp = CreateMapParamFinancialPlan();
            //CreateMapParamPaymentSchedule();
            //CreateMapParamPaymentMethod();

            return imapp.Map<List<CiaParamFinancialPlanServiceModel>, List<CiaParamFinancialPlan>>(ciaParamFinancialPlans);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductServiceModel"></param>
        /// <returns></returns>
        public static CiaParamFinancialPlan CreateCiaParamFinancialPlan(CiaParamFinancialPlanServiceModel ciaParamFinancialPlanServiceModel)
        {
            //CreateMapParamCurrency();
            var imapp = CreateMapParamFinancialPlan();
            //CreateMapParamPaymentSchedule();
            //CreateMapParamPaymentMethod();

            return imapp.Map<CiaParamFinancialPlanServiceModel, CiaParamFinancialPlan>(ciaParamFinancialPlanServiceModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductsServiceModel"></param>
        /// <returns></returns>
        public static List<CiaParamCurrency> CreateCiaParamCurrencies(List<CiaParamCurrencyServiceModel> ciaParamCurrencies)
        {
            var imapp = CreateMapParamCurrency();

            return imapp.Map<List<CiaParamCurrencyServiceModel>, List<CiaParamCurrency>>(ciaParamCurrencies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductServiceModel"></param>
        /// <returns></returns>
        public static CiaParamCurrency CreateCiaParamCurrency(CiaParamCurrencyServiceModel ciaParamCurrencyServiceModel)
        {
            var imapp = CreateMapParamCurrency();

            return imapp.Map<CiaParamCurrencyServiceModel, CiaParamCurrency>(ciaParamCurrencyServiceModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductsServiceModel"></param>
        /// <returns></returns>
        public static List<CiaParamPolicyType> CreateCiaParamPolicyTypes(List<CiaParamPolicyTypeServiceModel> ciaParamPolicyTypes)
        {
            List<CiaParamPolicyType> result = new List<CiaParamPolicyType>();
            foreach (CiaParamPolicyTypeServiceModel itemCiaParamPolicyTypeServiceModel in ciaParamPolicyTypes)
            {
                result.Add(CreateCiaParamPolicyType(itemCiaParamPolicyTypeServiceModel));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductServiceModel"></param>
        /// <returns></returns>
        public static CiaParamPolicyType CreateCiaParamPolicyType(CiaParamPolicyTypeServiceModel ciaParamPolicyTypeServiceModel)
        {
            return new CiaParamPolicyType
            {
                Id = ciaParamPolicyTypeServiceModel.Id,
                IsDefault = ciaParamPolicyTypeServiceModel.IsDefault
            };
        }

        public static List<CiaParamAssistanceType> CreateCiaParamAssistanceTypes(List<CiaParamAssistanceTypeServiceModel> ciaParamAssistanceTypes)
        {
            var imapp = CreateMapParamAssistanceType();

            return imapp.Map<List<CiaParamAssistanceTypeServiceModel>, List<CiaParamAssistanceType>>(ciaParamAssistanceTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductServiceModel"></param>
        /// <returns></returns>
        public static CiaParamAssistanceType CreateCiaParamAssistanceType(CiaParamAssistanceTypeServiceModel ciaParamAssistanceType)
        {
            var imapp = CreateMapParamAssistanceType();

            return imapp.Map<CiaParamAssistanceTypeServiceModel, CiaParamAssistanceType>(ciaParamAssistanceType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProductServiceModel"></param>
        /// <returns></returns>
        public static CiaParamCoverages CreateCiaParamCoverages(CiaParamCoveragesServiceModel ciaParamCoverages)
        {
            var imapp = CreateMapParamCoverages();
            //CreateMapParamCoverage();
            //CreateMapParamDeductiblesCoverage();

            return imapp.Map<CiaParamCoveragesServiceModel, CiaParamCoverages>(ciaParamCoverages);
        }

        public static CiaParamCommercialClass CreateCiaParamCommercialClass(CiaParamCommercialClassServiceModel ciaParamCurrencyServiceModel)
        {
            var imapp = CreateMapCiaParamCommercialClass();

            return imapp.Map<CiaParamCommercialClassServiceModel, CiaParamCommercialClass>(ciaParamCurrencyServiceModel);
        }

        public static CiaParamForm CreateCiaParamForm(CiaParamFormServiceModel ciaParamFormServiceModel)
        {
            var imapp = CreateMapCiaParamForm();

            return imapp.Map<CiaParamFormServiceModel, CiaParamForm>(ciaParamFormServiceModel);
        }

        public static CiaParamLimitsRC CreateCiaParamLimitsRC(CiaParamLimitsRCServiceModel ciaParamLimitsRCServiceModel)
        {
            var imapp = CreateMapCiaParamLimitsRC();
            return imapp.Map<CiaParamLimitsRCServiceModel, CiaParamLimitsRC>(ciaParamLimitsRCServiceModel);
        }
        public static CiaParamDeductibleProduct CreateCiaParamDeductibleProduct(CiaParamDeductibleProductServiceModel ciaParamDeductibleProductServiceModel)
        {
            var imapp = CreateMapCiaParamDeductibleProduct();

            return imapp.Map<CiaParamDeductibleProductServiceModel, CiaParamDeductibleProduct>(ciaParamDeductibleProductServiceModel);
        }


        #region autommaper

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapCiaParamDeductibleProduct()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamDeductibleProductServiceModel, CiaParamDeductibleProduct>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapCiaParamLimitsRC()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamLimitsRCServiceModel, CiaParamLimitsRC>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapCiaParamForm()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamFormServiceModel, CiaParamForm>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapCiaParamCommercialClass()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamCommercialClassServiceModel, CiaParamCommercialClass>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamProduct()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamProductServiceModel, CiaParamProduct>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPrefixServiceModel, CiaParamPrefix>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamCurrencyServiceModel, CiaParamCurrency>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamProduct2GServiceModel, CiaParamProduct2G>()
                 .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPolicyTypeServiceModel, CiaParamPolicyType>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamRiskTypeServiceModel, CiaParamRiskType>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamGroupCoverageServiceModel, CiaParamGroupCoverage>()
                 .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamCoveragesServiceModel, CiaParamCoverages>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamCoverageServiceModel, CiaParamCoverage>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamInsuredObjectServiceModel, CiaParamInsuredObject>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamFinancialPlanServiceModel, CiaParamFinancialPlan>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPaymentScheduleServiceModel, CiaParamPaymentSchedule>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPaymentMethodServiceModel, CiaParamPaymentMethod>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamFormServiceModel, CiaParamForm>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamDeductiblesCoverageServiceModel, CiaParamDeductiblesCoverage>()
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
                cfg.CreateMap<CiaParamPrefixServiceModel, CiaParamPrefix>()
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
                cfg.CreateMap<CiaParamCurrencyServiceModel, CiaParamCurrency>()
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
                cfg.CreateMap<CiaParamProduct2GServiceModel, CiaParamProduct2G>()
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
                cfg.CreateMap<CiaParamPolicyTypeServiceModel, CiaParamPolicyType>()
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
                cfg.CreateMap<CiaParamAssistanceTypeServiceModel, CiaParamAssistanceType>()
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
                 cfg.CreateMap<CiaParamRiskTypeServiceModel, CiaParamRiskType>()
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
                cfg.CreateMap<CiaParamGroupCoverageServiceModel, CiaParamGroupCoverage>()
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
                cfg.CreateMap<CiaParamDeductiblesCoverageServiceModel, CiaParamDeductiblesCoverage>()
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
                cfg.CreateMap<CiaParamCoveragesServiceModel, CiaParamCoverages>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamCoverageServiceModel, CiaParamCoverage>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamDeductiblesCoverageServiceModel, CiaParamDeductiblesCoverage>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamInsuredObjectServiceModel,CiaParamInsuredObject>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamFormServiceModel, CiaParamForm>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));                
                cfg.CreateMap<CiaParamGroupCoverageServiceModel, CiaParamGroupCoverage>()
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
                cfg.CreateMap<CiaParamCoverageServiceModel, CiaParamCoverage>()
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
                cfg.CreateMap<CiaParamInsuredObjectServiceModel, CiaParamInsuredObject>()
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
                cfg.CreateMap<CiaParamFinancialPlanServiceModel, CiaParamFinancialPlan>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamCurrencyServiceModel, CiaParamCurrency>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPaymentScheduleServiceModel, CiaParamPaymentSchedule>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamPaymentMethodServiceModel, CiaParamPaymentMethod>()
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
                cfg.CreateMap<CiaParamPaymentMethodServiceModel, CiaParamPaymentMethod>()
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
                cfg.CreateMap<CiaParamPaymentScheduleServiceModel, CiaParamPaymentSchedule>()
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
                cfg.CreateMap<CiaParamFormServiceModel, CiaParamForm>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapParamCopyProduct()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamCopyProductServiceModel, CiaParamCopyProduct>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapParamAgent()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamAgentServiceModel, CiaParamAgent>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
                cfg.CreateMap<CiaParamAgencyCommissServiceModel, CiaParamAgencyCommiss>()
                 .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }

        public static IMapper CreateMapParamAgencyCommiss()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamAgencyCommissServiceModel, CiaParamAgencyCommiss>()
                  .ForAllMembers(opt => opt.Condition(r => r != null));
            });
            return config.CreateMapper();
        }
        #endregion autommaper
    }
}
