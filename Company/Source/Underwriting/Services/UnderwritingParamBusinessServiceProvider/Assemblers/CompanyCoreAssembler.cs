using AutoMapper;
using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.UnderwritingParamService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Assemblers
{
    public class CompanyCoreAssembler
    {

        #region MapeoCoCoverage
        public static IMapper MappcoreCoCoverageValue()
        {
            var config = MapperCache.GetMapper<CompanyParamCoCoverageValue, ParamCoCoverageValue>(cfg =>
            {

                cfg.CreateMap<CompanyParamCoCoverageValue, ParamCoCoverageValue>();
                cfg.CreateMap<CompanyParamCoverage, BaseParamCoCoverageValue>();
                cfg.CreateMap<CompanyParamPrefix, BaseParamPrefix>();

            });
            return config;
        }

        /// <summary>
        /// MappConditionTextCore. Modelo Company condition text a modelo Core.
        /// </summary>
        /// <param name="CompanyConditionText">Modelo Company ConditionText</param>
        /// <returns>ParamConditionText. Modelo Core</returns>
        public static ParamConditionText MappConditionTextCore(CompanyParamConditionText companyConditionText)
        {
            ParamConditionText coreConditionText = new ParamConditionText()
            {
                Id = companyConditionText.Id,
                Body = companyConditionText.Body,
                Title = companyConditionText.Title,
                ConditionTextLevel = new BaseConditionTextLevel { Id = companyConditionText.ConditionTextLevel.Id, Description = companyConditionText.ConditionTextLevel.Description??"" },
                ConditionTextLevelType = new BaseConditionTextLevelType { Id = companyConditionText.ConditionTextLevelType.Id, Description=companyConditionText.ConditionTextLevelType.Description??"" }
            };
            return coreConditionText;
        }
            
    
        public static ParamCoCoverageValue MappCompanyCoCoverageValue(CompanyParamCoCoverageValue companyParamCoCoverageValue)
        {
            ParamCoCoverageValue paramCoCoverageValue = new ParamCoCoverageValue();
            paramCoCoverageValue.Percentage = companyParamCoCoverageValue.Percentage;
            paramCoCoverageValue.Coverage = new BaseParamCoverage { Id = companyParamCoCoverageValue.Coverage.Id, Description = companyParamCoCoverageValue.Coverage.Description };
            paramCoCoverageValue.Prefix = new BaseParamPrefix { Id = companyParamCoCoverageValue.Prefix.Id, Description = companyParamCoCoverageValue.Prefix.Description };
            return paramCoCoverageValue;
        }
        #endregion

        #region Ally_Coverage
        public static ParamQueryCoverage MappCompanyAllyCoverage(CompanyParamAllyCoverage companyParamAllyCoverage)
        {
            ParamQueryCoverage paramAllyCoverage = new ParamQueryCoverage();
            paramAllyCoverage.CoveragePrincipal = new Core.Application.UnderwritingParamService.Models.Base.BaseQueryAllyCoverage { Id= companyParamAllyCoverage.CoverageId};//.Id = companyParamAllyCoverage.CoverageId;//Core.Application.UnderwritingParamService.Models.Base.BaseQueryAllyCoverage { }//new Core.Application.UnderwritingParamService.Models.Base.BaseQueryAllyCoverage(companyParamAllyCoverage.CoverageId, companyParamAllyCoverage.CoverageId);
            //paramAllyCoverage.CoveragePrincipal.Id = companyParamAllyCoverage.CoverageId;//Core.Application.UnderwritingParamService.Models.Base.BaseQueryAllyCoverage { }//new Core.Application.UnderwritingParamService.Models.Base.BaseQueryAllyCoverage(companyParamAllyCoverage.CoverageId, companyParamAllyCoverage.CoverageId);
            paramAllyCoverage.AllyCoverage = new Core.Application.UnderwritingParamService.Models.Base.BaseQueryAllyCoverage { Id = companyParamAllyCoverage.AllyCoverageId };//.Id = companyParamAllyCoverage.AllyCoverageId;
            //paramAllyCoverage.AllyCoverage.Id = companyParamAllyCoverage.AllyCoverageId;
            paramAllyCoverage.CoveragePercentage = companyParamAllyCoverage.CoveragePct;
            //paramCoCoverageValue.CoveragePercentage = companyParamCoCoverageValue.CoveragePct;
            //paramCoCoverageValue.Coverage = //new BaseParamCoverage { Id = companyParamCoCoverageValue.Coverage.Id, Description = companyParamCoCoverageValue.Coverage.Description };
            //paramCoCoverageValue.Prefix = //new BaseParamPrefix { Id = companyParamCoCoverageValue.Prefix.Id, Description = companyParamCoCoverageValue.Prefix.Description };
            return paramAllyCoverage;
        }

        public static List<ParamQueryCoverage> MappCompanyQueryAllyCoverage(List<CompanyParamQueryAllyCoverage> companyParamQueryAllyCoverage)
        {
            List<ParamQueryCoverage> paramAllyCoverage = new List<ParamQueryCoverage>();

            companyParamQueryAllyCoverage.ForEach((x) => {
                paramAllyCoverage.Add(new ParamQueryCoverage {
                    CoveragePrincipal = new Core.Application.UnderwritingParamService.Models.Base.BaseQueryAllyCoverage { Id = x.Coverage.Id, Description = x.Coverage.PrintDescription },
                    AllyCoverage = new Core.Application.UnderwritingParamService.Models.Base.BaseQueryAllyCoverage { Id = x.AllyCoverage.Id, Description = x.AllyCoverage.PrintDescription },
                    CoveragePercentage = x.CoveragePct
                });
            });

            return paramAllyCoverage;
        }
        #endregion

        #region Tax

        #region TaxMapper
        /// <summary>
        /// MappCompanyToCoreTax. Instancia de mapper para mapear Modelo Tax de Company a Core
        /// </summary>
        public static IMapper MappCompanyToCoreTax()
        {
            var config = MapperCache.GetMapper<CompanyParamTax, ParamTax> (cfg =>
            {
                cfg.CreateMap<CompanyParamTax, ParamTax>();
            });
            return config;
        }

        /// <summary>
        /// MappTaxValue. Convierte de Modelo Company Tax a Modelo Core.
        /// </summary>
        /// <param name="companyParamTaxModel">Modelo Tax de Company</param>
        /// <returns>ParamTax. Modelo Core</returns>
        public static ParamTax MapTaxCompanyToCore(CompanyParamTax companyParamTaxModel)
        {
            ParamTax paramTaxValue = new ParamTax()
            {
                Id = companyParamTaxModel.Id,
                Description = companyParamTaxModel.Description,
                TinyDescription = companyParamTaxModel.TinyDescription,
                CurrentFrom = companyParamTaxModel.CurrentFrom,
                IsSurPlus = companyParamTaxModel.IsSurPlus,
                IsAdditionalSurPlus = companyParamTaxModel.IsAdditionalSurPlus,
                Enabled = companyParamTaxModel.Enabled,
                IsEarned = companyParamTaxModel.IsEarned,
                IsRetention = companyParamTaxModel.IsRetention,
                RateType = new RateTypeTax
                {
                    Id = companyParamTaxModel.RateType.Id,
                    Description = companyParamTaxModel.RateType.Description
                },
                TaxRoles = companyParamTaxModel.TaxRoles.Select(t => new TaxRole { Id = t.Id, Description = t.Description }).ToList(),
                TaxAttributes = companyParamTaxModel.TaxAttributes.Select(t => new Core.Application.UnderwritingServices.Models.TaxAttribute { Id = t.Id, Description = t.Description }).ToList(),
                RetentionTax = new RetentionTax
                {
                    Id = companyParamTaxModel.RetentionTax.Id,
                    Description = companyParamTaxModel.RetentionTax.Description
                },
                BaseConditionTax = new BaseConditionTax
                {
                    Id = companyParamTaxModel.BaseConditionTax.Id,
                    Description = companyParamTaxModel.BaseConditionTax.Description
                },
                AdditionalRateType = new AdditionalRateType
                {
                    Id = companyParamTaxModel.AdditionalRateType.Id,
                    Description = companyParamTaxModel.AdditionalRateType.Description
                }
            };
            return paramTaxValue;
        }
        #endregion

        #region TaxRateMapper

        /// <summary>
        /// MappCompanyToCoreTaxRate. Instancia de mapper para mapear Modelo TaxRate de Company a Core
        /// </summary>
        public static IMapper MappCompanyToCoreTaxRate()
        {
            var config = MapperCache.GetMapper<CompanyParamTaxRate, ParamTaxRate>(cfg =>
            {
                cfg.CreateMap<CompanyParamTaxRate, ParamTaxRate>();
            });
            return config;

        }

        /// <summary>
        /// MapTaxRateCompanyToCore. Convierte de Modelo Company TaxRate a Modelo Core.
        /// </summary>
        /// <param name="companyParamTaxRateModel">Modelo Company TaxRate</param>
        /// <returns>CompanyParamTaxRate. Modelo Core</returns>
        public static ParamTaxRate MapTaxRateCompanyToCore(CompanyParamTaxRate companyParamTaxRateModel)
        {
            ParamTaxRate paramTaxRateValue = new ParamTaxRate()
            {
                Id = companyParamTaxRateModel.Id,
                IdTax = companyParamTaxRateModel.IdTax,
                TaxCondition = new TaxCondition
                {
                    Id = companyParamTaxRateModel.TaxCondition.Id
                },
                TaxCategory = new TaxCategory
                {
                    Id = companyParamTaxRateModel.TaxCategory.Id
                },
                LineBusiness = new LineBusiness
                {
                    Id = companyParamTaxRateModel.LineBusiness.Id
                },
                TaxState = new TaxState
                {
                    IdState = companyParamTaxRateModel.TaxState.IdState,
                    IdCountry = companyParamTaxRateModel.TaxState.IdCountry,
                    IdCity = companyParamTaxRateModel.TaxState.IdCity
                },
                EconomicActivity = new EconomicActivity
                {
                    Id = companyParamTaxRateModel.EconomicActivity.Id
                },
                Branch = new Branch
                {
                    Id = companyParamTaxRateModel.Branch.Id
                },
                TaxPeriodRate = new TaxPeriodRate
                {
                    CurrentFrom = companyParamTaxRateModel.TaxPeriodRate.CurrentFrom,
                    Rate = companyParamTaxRateModel.TaxPeriodRate.Rate,
                    AdditionalRate = companyParamTaxRateModel.TaxPeriodRate.AdditionalRate,
                    BaseTaxAdditional = companyParamTaxRateModel.TaxPeriodRate.BaseTaxAdditional,
                    MinBaseAMT = companyParamTaxRateModel.TaxPeriodRate.MinBaseAMT,
                    MinAdditionalBaseAMT = companyParamTaxRateModel.TaxPeriodRate.MinAdditionalBaseAMT,
                    MinTaxAMT = companyParamTaxRateModel.TaxPeriodRate.MinTaxAMT,
                    MinAdditionalTaxAMT = companyParamTaxRateModel.TaxPeriodRate.MinAdditionalTaxAMT
                },
                Coverage = new Coverage
                {
                    Id = companyParamTaxRateModel.Coverage.Id
                }
            };
            return paramTaxRateValue;
        }
        #endregion

        #region TaxCategoryMapper

        /// <summary>
        /// MappCompanyToCoreTaxCategory. Instancia de mapper para mapear Modelo TaxCategory de Company a Core
        /// </summary>
        public static IMapper MappCompanyToCoreTaxCategory()
        {
            var config = MapperCache.GetMapper<CompanyParamTaxCategory, ParamTaxCategory>(cfg =>
            {
                cfg.CreateMap<CompanyParamTaxCategory, ParamTaxCategory>();
            });
            return config;
        }

        /// <summary>
        /// MapTaxCategoryCompanyToCore. Convierte de Modelo Company TaxCategory a Modelo Core.
        /// </summary>
        /// <param name="companyParamTaxCategoryModel">Modelo Company TaxCategory</param>
        /// <returns>ParamTaxCategory. Modelo Core</returns>
        public static ParamTaxCategory MapTaxCategoryCompanyToCore(CompanyParamTaxCategory companyParamTaxCategoryModel)
        {
            ParamTaxCategory paramTaxCategoryValue = new ParamTaxCategory()
            {
                Id = companyParamTaxCategoryModel.Id,
                IdTax = companyParamTaxCategoryModel.IdTax,
                Description = companyParamTaxCategoryModel.Description
            };
            return paramTaxCategoryValue;
        }
        #endregion

        #region TaxConditionMapper

        /// <summary>
        /// MappCompanyToCoreTaxCondition. Instancia de mapper para mapear Modelo TaxCondition de Company a Core
        /// </summary>
        public static IMapper MappCompanyToCoreTaxCondition()
        {
            var config = MapperCache.GetMapper<CompanyParamTaxCondition, ParamTaxCondition>(cfg =>
            {
                cfg.CreateMap<CompanyParamTaxCondition, ParamTaxCondition>(); 
            });
            return config;
        }

        /// <summary>
        /// MappTaxConditionValue. Convierte de Modelo Company TaxCondition a Modelo Core.
        /// </summary>
        /// <param name="CompanyParamTaxCondition">Modelo Company TaxCondition</param>
        /// <returns>ParamTaxCondition. Modelo Core</returns>
        public static ParamTaxCondition MapTaxConditionCompanyToCore(CompanyParamTaxCondition companyParamTaxConditionModel)
        {
            ParamTaxCondition paramTaxConditionValue = new ParamTaxCondition()
            {
                Id = companyParamTaxConditionModel.Id,
                IdTax = companyParamTaxConditionModel.IdTax,
                Description = companyParamTaxConditionModel.Description,
                HasNationalRate = companyParamTaxConditionModel.HasNationalRate,
                IsIndependent = companyParamTaxConditionModel.IsIndependent,
                IsDefault = companyParamTaxConditionModel.IsDefault
            };
            return paramTaxConditionValue;
        }
        #endregion

        #endregion
    }
}
