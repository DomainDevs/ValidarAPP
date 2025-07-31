namespace Sistran.Company.Application.ProductParamService.EEProvider.Assemblers
{
    using AutoMapper;
    using Sistran.Company.Application.ProductParamService.Models;
    using CIAPRODEN = Sistran.Company.Application.Product.Entities;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using PRODEN = Sistran.Core.Application.Product.Entities;

    /// <summary>
    /// Convierte el modelo de negocio al  modelo de la entidad 
    /// </summary>
    public static class EntityAssembler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProduct"></param>
        /// <returns></returns>
        public static PRODEN.Product CreateCoreEntityProduct(CiaParamProduct ciaParamProduct)
        {
            return new PRODEN.Product
            {
                ProductId = ciaParamProduct.Id,
                PrefixCode = ciaParamProduct.Prefix.Id,
                AdditionalCommissionPercentage = ciaParamProduct.AdditDisCommissPercentage,
                IsGreen = ciaParamProduct.IsGreen,
                Description = ciaParamProduct.Description,
                SmallDescription = ciaParamProduct.SmallDescription,
                IncCommAdjustFactorPercentage = ciaParamProduct.IncrementCommisionAdjustFactorPercentage,
                DecCommAdjustFactorPercentage = ciaParamProduct.DecrementCommisionAdjustFactorPercentage,
                PreRuleSetId = ciaParamProduct.PreRuleSetId,
                RuleSetId = ciaParamProduct.RuleSetId,
                ScriptId = ciaParamProduct.ScriptId,
                AdditCommissPercentage = ciaParamProduct.AdditionalCommissionPercentage,
                StandardCommissionPercentage = ciaParamProduct.StandardCommissionPercentage,
                StdDiscountCommPercentage = ciaParamProduct.StdDiscountCommPercentage,
                SurchargeCommissionPercentage = ciaParamProduct.SurchargeCommissionPercentage,
                IsCollective = ciaParamProduct.IsCollective,
                IsRequest = ciaParamProduct.IsRequest,
                IsFlatRate = ciaParamProduct.IsFlatRate,
                CurrentFrom = ciaParamProduct.CurrentFrom,
                CurrentTo = ciaParamProduct.CurrentTo,
                Version = ciaParamProduct.Version,
                CalculateMinPremium = ciaParamProduct.CalculateMinPremium,
                IsInteractive = ciaParamProduct.IsInteractive,
                IsMassive = ciaParamProduct.IsMassive,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamProduct"></param>
        /// <returns></returns>
        //public static CIAPRODEN.CptProduct CreateCiaEntityProduct(CiaParamProduct ciaParamProduct)
        //{
        //    return new CIAPRODEN.CptProduct(ciaParamProduct.Id)
        //    {
        //        ProductId = ciaParamProduct.Id,
        //        IsPoliticalProduct = ciaParamProduct.IsPolitical,
        //        IncentiveAmount = ciaParamProduct.IncentiveAmount,
        //        IsEnabled = ciaParamProduct.IsEnabled,
        //        IsScore = ciaParamProduct.IsScore,
        //        IsFine = ciaParamProduct.IsFine,
        //        IsFasecolda = ciaParamProduct.IsFasecolda,
        //        ValidDaysTempPolicy = ciaParamProduct.ValidDaysTempPolicy,
        //        ValidDaysTempQuote = ciaParamProduct.ValidDaysTempQuote,
        //        IsRcAdditional = ciaParamProduct.IsRcAdditional
        //    };
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamFinancialPlan"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static PRODEN.ProductFinancialPlan CreateEntityFinancialPlan(CiaParamFinancialPlan ciaParamFinancialPlan, int productId)
        {
            return new PRODEN.ProductFinancialPlan(productId, ciaParamFinancialPlan.Id)
            {
                IsDefault = ciaParamFinancialPlan.IsDefault
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamRiskType"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static PRODEN.ProductCoverRiskType CreateEntityRiskType(CiaParamRiskType ciaParamRiskType, int productId)
        {
            return new PRODEN.ProductCoverRiskType(productId, ciaParamRiskType.Id)
            {
                MaxRiskQuantity = ciaParamRiskType.MaxRiskQuantity,
                RuleSetId = ciaParamRiskType.RuleSetId,
                PreRuleSetId = ciaParamRiskType.PreRuleSetId,
                ScriptId = ciaParamRiskType.ScriptId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamCoverage"></param>
        /// <param name="productId"></param>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public static PRODEN.ProductGroupCover CreateEntityGroupCover(CiaParamCoverage ciaParamCoverage, int productId, int prefixId)
        {
            return new PRODEN.ProductGroupCover(productId, ciaParamCoverage.Id)
            {
                CoverGroupId = ciaParamCoverage.Id,
                SmallDescription = ciaParamCoverage.Description,
                PrefixCode = prefixId,
                CoveredRiskTypeCode = (int)ciaParamCoverage.RiskTypeId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamInsuredObject"></param>
        /// <param name="productId"></param>
        /// <param name="riskId"></param>
        /// <param name="groupCoverageId"></param>
        /// <returns></returns>
        public static PRODEN.GroupInsuredObject CreateGroupInsuredObject(CiaParamInsuredObject ciaParamInsuredObject, int productId, int riskId, int groupCoverageId)
        {

            return new PRODEN.GroupInsuredObject(productId, groupCoverageId, ciaParamInsuredObject.Id)
            {

                IsMandatory = ciaParamInsuredObject.IsMandatory == null ? false : true,
                IsSelected = ciaParamInsuredObject.IsSelected == null ? false : true,
                CoveredRiskTypeCode = riskId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamGroupCoverage"></param>
        /// <param name="productId"></param>
        /// <param name="riskId"></param>
        /// <param name="groupCoverageId"></param>
        /// <returns></returns>
        public static PRODEN.GroupCoverage CreateCoverage(CiaParamGroupCoverage ciaParamGroupCoverage, int productId, int? riskId, int groupCoverageId)
        {
            return new PRODEN.GroupCoverage(ciaParamGroupCoverage.Id, productId, groupCoverageId)
            {
                CoverGroupId = groupCoverageId,
                ProductId = productId,
                CoverageId = ciaParamGroupCoverage.Id,
                IsMandatory = ciaParamGroupCoverage.IsMandatory,
                IsSelected = ciaParamGroupCoverage.IsSelected,
                CoverNum = ciaParamGroupCoverage.Number,
                RuleSetId = ciaParamGroupCoverage.RuleSetId,
                PosRuleSetId = ciaParamGroupCoverage.PosRuleSetId,
                ScriptId = ciaParamGroupCoverage.ScriptId,
                MainCoverageId = ciaParamGroupCoverage.MainCoverageId,
                CoveredRiskTypeCode = riskId
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciaParamGroupCoverage"></param>
        /// <param name="productId"></param>
        /// <param name="riskId"></param>
        /// <param name="groupCoverageId"></param>
        /// <returns></returns>
        public static CIAPRODEN.CiaGroupCoverage CreateCiaGroupCoverage(CiaParamGroupCoverage ciaParamGroupCoverage, int productId, int groupCoverageId)
        {
            int isPremiumMin = ciaParamGroupCoverage.IsPremiumMin ? 1 : 0;
            int noCalculate = ciaParamGroupCoverage.NoCalculate ? 1 : 0;
            return new CIAPRODEN.CiaGroupCoverage(ciaParamGroupCoverage.Id, productId, groupCoverageId)
            {
                CoverGroupId = groupCoverageId,
                ProductId = productId,
                CoverageId = ciaParamGroupCoverage.Id,
                IsPremiumMin = isPremiumMin,
                NoCalculate = noCalculate
            };
        }

        public static COMMEN.DeductibleProduct CreateDeductibleProduct(CiaParamDeductibleProduct ciaParamDeductibleProduct)
        {
            return new COMMEN.DeductibleProduct(ciaParamDeductibleProduct.DeductId, ciaParamDeductibleProduct.ProductId)
            {
                DeductId = ciaParamDeductibleProduct.DeductId,
                ProductId = ciaParamDeductibleProduct.ProductId
            };
        }

        public static COMMEN.ProductRiskCommercialClass CreateProductRiskCommercialClass(CiaParamCommercialClass ciaParamCommercialClass)
        {
            return new COMMEN.ProductRiskCommercialClass(ciaParamCommercialClass.ProductId, ciaParamCommercialClass.Id)
            {
                ProductId = ciaParamCommercialClass.ProductId,
                RiskCommercialClassCode = ciaParamCommercialClass.Id,
                DefaultRiskCommercial = ciaParamCommercialClass.IsDefault
            };
        }

        public static COMMEN.CoLimitsRcRel CreateCoLimitsRcRel(CiaParamLimitsRC ciaParamLimitsRC)
        {
            return new COMMEN.CoLimitsRcRel(ciaParamLimitsRC.PrefixId, ciaParamLimitsRC.PolicyTypeId, ciaParamLimitsRC.ProductId, ciaParamLimitsRC.Id)
            {
                PrefixCode = ciaParamLimitsRC.PrefixId,
                PolicyTypeCode = ciaParamLimitsRC.PolicyTypeId,
                LimitRcCode = ciaParamLimitsRC.Id,
                ProductId = ciaParamLimitsRC.ProductId,
                IsDefault = ciaParamLimitsRC.IsDefault
            };
        }

        public static PRODEN.ProductForm CreateProductForm(CiaParamForm ciaParamProductForm)
        {
            return new PRODEN.ProductForm(ciaParamProductForm.FormId)
            {
                FormId = ciaParamProductForm.FormId,
                CurrentFrom = ciaParamProductForm.CurrentFrom,
                FormNumber = ciaParamProductForm.FormNumber,
                ProductId = ciaParamProductForm.ProductId,
                CoverGroupId = ciaParamProductForm.CoverGroupId
            };
        }

        //public static CIAPRODEN.CptProductBranchAssistanceType CreateAssistanceType(CiaParamAssistanceType ciaParamAssistanceType, int productId, int prefixId)
        //{

        //    return new CIAPRODEN.CptProductBranchAssistanceType(productId, prefixId, ciaParamAssistanceType.AssistanceId);
        //}

        #region autommaper
        /// <summary>
        /// 
        /// </summary>
        public static IMapper CreateMapCoreProduct()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamProduct, PRODEN.Product>()
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PrefixCode, opt => opt.MapFrom(src => src.Prefix.Id))
                .ForMember(dest => dest.IncCommAdjustFactorPercentage, opt => opt.MapFrom(src => src.IncrementCommisionAdjustFactorPercentage))
                .ForMember(dest => dest.DecCommAdjustFactorPercentage, opt => opt.MapFrom(src => src.DecrementCommisionAdjustFactorPercentage))
                .ForMember(dest => dest.AdditCommissPercentage, opt => opt.MapFrom(src => src.AdditionalCommissionPercentage));
                //.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
            });

            return config.CreateMapper();

        }

        /// <summary>
        /// 
        /// </summary>
        //public static void CreateMapCiaProduct()
        //{
        //    Mapper.CreateMap<CiaParamProduct, CIAPRODEN.CptProduct>()
        //        .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
        //        .ForMember(dest => dest.IsPoliticalProduct, opt => opt.MapFrom(src => src.IsPolitical))
        //        .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        public static IMapper CreateMapFinancialPlan(int productId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamFinancialPlan, PRODEN.ProductFinancialPlan>()
              .ForMember(dest => dest.FinancialPlanId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => productId));
                //.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
            });

            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        public static IMapper CreateMapRiskType(int productId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CiaParamRiskType, PRODEN.ProductCoverRiskType>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => productId))
                .ForMember(dest => dest.CoveredRiskTypeCode, opt => opt.MapFrom(src => src.Id));
                //.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
            });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="prefixId"></param>
        public static IMapper CreateMapGroupCoverage(int productId, int prefixId)
        {
            var config = new MapperConfiguration(cfg =>
              {
                  cfg.CreateMap<CiaParamCoverage, PRODEN.ProductGroupCover>()
                  .ForMember(dest => dest.CoverGroupId, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => productId))
                  .ForMember(dest => dest.CoveredRiskTypeCode, opt => opt.MapFrom(src => src.RiskTypeId))
                  .ForMember(dest => dest.PrefixCode, opt => opt.MapFrom(src => prefixId))
                  .ForMember(dest => dest.SmallDescription, opt => opt.MapFrom(src => src.Description));
                  //.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

              });
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="riskId"></param>
        /// <param name="groupCoverageId"></param>
        public static IMapper CreateMapGroupInsuredObject(int productId, int riskId, int groupCoverageId)
        {
            var config = new MapperConfiguration(cfg =>
              {
                  cfg.CreateMap<CiaParamInsuredObject, PRODEN.GroupInsuredObject>()
                  .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => productId))
                  .ForMember(dest => dest.CoverageGroupCode, opt => opt.MapFrom(src => groupCoverageId))
                  .ForMember(dest => dest.InsuredObject, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.CoveredRiskTypeCode, opt => opt.MapFrom(src => riskId));
                  //.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
            });
            
            return config.CreateMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="riskId"></param>
        /// <param name="groupCoverageId"></param>
        public static IMapper CreateMapCoverage(int productId, int riskId, int groupCoverageId)
        {
            var config = new MapperConfiguration(cfg =>
             {
                 cfg.CreateMap<CiaParamGroupCoverage, PRODEN.GroupCoverage>()
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => productId))
                .ForMember(dest => dest.CoverGroupId, opt => opt.MapFrom(src => groupCoverageId))
                .ForMember(dest => dest.CoverageId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CoverNum, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CoveredRiskTypeCode, opt => opt.MapFrom(src => riskId));
                //.ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
             });
            return config.CreateMapper();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="prefixId"></param>
        //public static void CreateMapAssistanceType(int productId, int prefixId)
        //{
        //    Mapper.CreateMap<CiaParamAssistanceType, CIAPRODEN.CptProductBranchAssistanceType>()
        //        .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => productId))
        //        .ForMember(dest => dest.AssistanceCode, opt => opt.MapFrom(src => src.AssistanceId))
        //        .ForMember(dest => dest.PrefixCode, opt => opt.MapFrom(src => prefixId))
        //        .ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));
        //}
        #endregion autommaper
    }
}
