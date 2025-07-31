
using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.QuotationServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniqueUserServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Generic;


namespace Sistran.Company.Application.QuotationServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static Peril CreatePeril(Core.Application.Quotation.Entities.Peril peril)
        {
            return new Peril
            {
                Description = peril.Description,
                SmallDescription = peril.SmallDescription,
                Id = peril.PerilCode
            };
        }
        /// <summary>
        /// Creates the map company policy.
        /// </summary>
        public static IMapper CreateMapCompanyPolicy()
        {
            var config = MapperCache.GetMapper<Policy, CompanyPolicy>(cfg =>
            {
                cfg.CreateMap<Product, CompanyProduct>();
                cfg.CreateMap<CoveredRisk, CompanyCoveredRisk>();
                cfg.CreateMap<Prefix, CompanyPrefix>();
                cfg.CreateMap<Summary, CompanySummary>();
                cfg.CreateMap<Risk, CompanyRisk>();
                cfg.CreateMap<IssuanceInsured, CompanyIssuanceInsured>();
                cfg.CreateMap<Core.Application.UniqueUserServices.Models.User, CompanyUser>();
                #region Coverages
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
                #endregion Coverages
                cfg.CreateMap<Component, CompanyComponent>();
                cfg.CreateMap<PayerComponent, CompanyPayerComponent>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<BillingGroup, CompanyBillingGroup>();
                cfg.CreateMap<PolicyType, CompanyPolicyType>();
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
                cfg.CreateMap<Branch, CompanyBranch>();
                cfg.CreateMap<SalePoint, CompanySalesPoint>();
                cfg.CreateMap<Text, CompanyText>();
                cfg.CreateMap<Policy, CompanyPolicy>();
                cfg.CreateMap<PaymentPlan, CompanyPaymentPlan>();
            });
            return config;
        }
        public static CompanyPolicy CreateCompanyPolicy(Policy policy)
        {
            CompanyPolicy companyPolicy = new CompanyPolicy();
            var mapper = CreateMapCompanyPolicy();
            return mapper.Map(policy, companyPolicy);

        }

        #region Product
        public static IMapper CreateMapCompanyProduct()
        {
            var config = MapperCache.GetMapper<Product, CompanyProduct>(cfg =>
            {
                cfg.CreateMap<Product, CompanyProduct>();
                cfg.CreateMap<Sistran.Core.Application.ProductServices.Models.CoveredRisk, CompanyCoveredRisk>();
                cfg.CreateMap<Prefix, CompanyPrefix>();
            });
            return config;
        }

        #endregion Product

        public static CompanyProduct CreateCompanyProduct(Product coreProduct)
        {
            CompanyProduct companyPoduct = new CompanyProduct();
            var immaper = CreateMapCompanyProduct();
            return immaper.Map<Product, CompanyProduct>(coreProduct);
        }
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

        public static TextPretacalogued CreateText(Sistran.Core.Application.QuotationServices.Models.ConditionTextModel entidad)
        {
            TextPretacalogued textPretacaloguedDto = new TextPretacalogued {
                ConditionLevelCode = entidad.ConditionLevel.Id,
                DescriptionLevel = entidad.ConditionLevel.Description,
                ConditionLevelId = entidad.CondTextLevelModel.ConditionLevelId,
                ConditionTextId = entidad.ConditionTextId,
                ConditionTextIdCod = entidad.ConditionTextIdCod,
                CondTextLevelId = entidad.CondTextLevelModel.CondTextLevelId,
                IsAutomatic = entidad.IsAutomatic,
                TextBody = entidad.TextBody,
                TextTitle = entidad.TextTitle,
                DescriptionCoverange = entidad.coverage?.Description,
                DescriptionBranch = entidad.Prefix?.Description,
                DescriptionRiskCoverange = entidad.RiskTyprCoverage?.Description,
                ErrorServiceModel = new Core.Application.ModelServices.Models.Param.ErrorServiceModel { ErrorTypeService = 0, ErrorDescription = null },
                StatusTypeService = Core.Application.ModelServices.Enums.StatusTypeService.Original,
               
            };
            return textPretacaloguedDto;
        }

        public static List<TextPretacalogued> CreateText(List<Sistran.Core.Application.QuotationServices.Models.ConditionTextModel> entidad)
        {
            var TextPretacaloguedDto = new List<TextPretacalogued>();
            foreach (var item in entidad)
            {
                TextPretacaloguedDto.Add(CreateText(item));
            }
            return TextPretacaloguedDto;
        }


        public static TextPretacalogued CreateTextLevel(Sistran.Core.Application.QuotationServices.Models.CondTextLevelModel entidad)
        {
            TextPretacalogued textPretacaloguedDto = new TextPretacalogued
            {
               
                ConditionTextIdCod = entidad.ConditionTextIdCod,
                CondTextLevelId = entidad.CondTextLevelId,
                IsAutomatic = entidad.IsAutomatic,

            };
            return textPretacaloguedDto;
        }

        public static List<TextPretacalogued> CreateTextLevels(List<Sistran.Core.Application.QuotationServices.Models.CondTextLevelModel> entidad)
        {
            var TextPretacaloguedDto = new List<TextPretacalogued>();
            foreach (var item in entidad)
            {
                TextPretacaloguedDto.Add(CreateTextLevel(item));
            }
            return TextPretacaloguedDto;
        }
        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }
    }
}