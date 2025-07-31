using AutoMapper;
using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.UnderwritingParamService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Generic;
using System.Linq;
using BASE = Sistran.Core.Application.UnderwritingServices.Models.Base;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Assemblers
{
    public class CoreCompanyAssembler
    {
        #region MapeoCoCOverageValue
        public static IMapper MappCompanyCoCoverageValue()
        {
            var config = MapperCache.GetMapper<ParamCoCoverageValue, CompanyParamCoCoverageValue> (cfg =>
            {
                cfg.CreateMap<ParamCoCoverageValue, CompanyParamCoCoverageValue>();
                cfg.CreateMap<BaseParamCoCoverageValue, CompanyParamCoverage>();
                cfg.CreateMap<BASE.BaseParamPrefix, CompanyParamPrefix>();
            });
            return config;
        }

        /// <summary>
        /// MappConditionTextCore. Modelo Company condition text a modelo Core.
        /// </summary>
        /// <param name="CompanyConditionText">Modelo Company ConditionText</param>
        /// <returns>ParamConditionText. Modelo Core</returns>
        public static CompanyParamConditionText MappConditionTextCore(ParamConditionText coreConditionText)
        {
            CompanyParamConditionText ConditionTextCompany = new CompanyParamConditionText();
            ConditionTextCompany.Id = coreConditionText.Id;
            ConditionTextCompany.Title = coreConditionText.Title;
            ConditionTextCompany.Body = coreConditionText.Body;
            ConditionTextCompany.ConditionTextLevel = new BaseConditionTextLevel { Id = coreConditionText.ConditionTextLevel.Id,  Description= coreConditionText.ConditionTextLevel.Description??""};
            ConditionTextCompany.ConditionTextLevelType = new BaseConditionTextLevelType { Id = coreConditionText.ConditionTextLevelType.Id, Description = coreConditionText.ConditionTextLevelType.Description ?? "" };
            return ConditionTextCompany;
        }

        public static List<CompanyParamCoCoverageValue> MappCompanyCoCoverageValues(List<ParamCoCoverageValue> paramCoCoverageValue)
        {
            List<CompanyParamCoCoverageValue> paramCoCoverageValues = new List<CompanyParamCoCoverageValue>();
            foreach (var item in paramCoCoverageValue)
            {
                paramCoCoverageValues.Add(MappCompanyParamCoCoverageValue(item));
            }
            return paramCoCoverageValues;


        }
        public static CompanyParamCoCoverageValue MappCompanyParamCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            CompanyParamCoCoverageValue companyParamCoCoverageValue = new CompanyParamCoCoverageValue();
            companyParamCoCoverageValue.Percentage = paramCoCoverageValue.Percentage;
            companyParamCoCoverageValue.Coverage = new CompanyParamCoverage { Id = paramCoCoverageValue.Coverage.Id, Description = paramCoCoverageValue.Coverage.Description };
            companyParamCoCoverageValue.Prefix = new CompanyParamPrefix { Id = paramCoCoverageValue.Prefix.Id, Description = paramCoCoverageValue.Prefix.Description };
            return companyParamCoCoverageValue;
        }

        public static List<CompanyParamCoverage> MappBaseCoverages(List<BaseParamCoverage> baseParamCoverage)
        {
            List<CompanyParamCoverage> companyParamCoverage = new List<CompanyParamCoverage>();
            foreach (var item in baseParamCoverage)
            {
                companyParamCoverage.Add(MappBaseCoverage(item));
            }
            return companyParamCoverage;
        }

        public static CompanyParamCoverage MappBaseCoverage(BaseParamCoverage baseParamCoverage)
        {
            CompanyParamCoverage companyParamCoverage = new CompanyParamCoverage();
            companyParamCoverage.Id = baseParamCoverage.Id;
            companyParamCoverage.Description = baseParamCoverage.Description;
            return companyParamCoverage;
        }

        public static CompanyParamCoCoverageValue MappcoreCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            CompanyParamCoCoverageValue companyParamCoCoverageValue = new CompanyParamCoCoverageValue();
            companyParamCoCoverageValue.Percentage = paramCoCoverageValue.Percentage;
            companyParamCoCoverageValue.Coverage = new CompanyParamCoverage { Id = paramCoCoverageValue.Coverage.Id, Description = paramCoCoverageValue.Coverage.Description };
            companyParamCoCoverageValue.Prefix = new CompanyParamPrefix { Id = paramCoCoverageValue.Prefix.Id, Description = paramCoCoverageValue.Prefix.Description };
            return companyParamCoCoverageValue;
        }
        #endregion
        #region Ally_Coverage
        public static CompanyParamAllyCoverage MappcoreAllyCoverage(ParamQueryCoverage paramAllyCoverage)
        {
            var companyParamAllyCoverage = new CompanyParamAllyCoverage();

            companyParamAllyCoverage.CoverageId = paramAllyCoverage.CoveragePrincipal.Id;//Percentage = paramCoCoverageValue.Percentage;
            companyParamAllyCoverage.AllyCoverageId = paramAllyCoverage.AllyCoverage.Id;//Coverage = new CompanyParamCoverage { Id = paramCoCoverageValue.Coverage.Id, Description = paramCoCoverageValue.Coverage.Description };
            companyParamAllyCoverage.CoveragePct = int.Parse(paramAllyCoverage.CoveragePercentage.ToString());//Prefix = new CompanyParamPrefix { Id = paramCoCoverageValue.Prefix.Id, Description = paramCoCoverageValue.Prefix.Description };
            return companyParamAllyCoverage;
        }
        #endregion

        #region Tax

        #region TaxMapper
        public static CompanyParamTax MapTaxCoreToCompany(ParamTax coreTax)
        {
            if(coreTax.TaxRoles == null)
            {
                coreTax.TaxRoles = new List<TaxRole>();
            }
            if (coreTax.TaxAttributes == null)
            {
                coreTax.TaxAttributes = new List<Core.Application.UnderwritingServices.Models.TaxAttribute>();
            }
            if (coreTax.TaxRates == null)
            {
                coreTax.TaxRates = new List<ParamTaxRate>();
            }
            if (coreTax.TaxCategories == null)
            {
                coreTax.TaxCategories = new List<ParamTaxCategory>();
            }
            if (coreTax.TaxConditions == null)
            {
                coreTax.TaxConditions = new List<ParamTaxCondition>();
            }
            CompanyParamTax paramTaxValue = new CompanyParamTax()
            {
                Id = coreTax.Id,
                Description = coreTax.Description,
                TinyDescription = coreTax.TinyDescription,
                CurrentFrom = coreTax.CurrentFrom,
                IsSurPlus = coreTax.IsSurPlus,
                IsAdditionalSurPlus = coreTax.IsAdditionalSurPlus,
                Enabled = coreTax.Enabled,
                IsEarned = coreTax.IsEarned,
                IsRetention = coreTax.IsRetention,
                RateType = new CompanyTaxRate
                {
                    Id = coreTax.RateType.Id,
                    Description = coreTax.RateType.Description != null ? coreTax.RateType.Description : string.Empty
                },
                TaxRoles = coreTax.TaxRoles.Select(t => new CompanyTaxRole { Id = t.Id, Description = t.Description }).ToList(),
                TaxAttributes = coreTax.TaxAttributes.Select(t => new CompanyTaxAttribute { Id = t.Id, Description = t.Description }).ToList(),
                RetentionTax = new CompanyTax
                {
                    Id = coreTax.RetentionTax.Id,
                    Description = coreTax.RetentionTax.Description != null ? coreTax.RetentionTax.Description : string.Empty
                },
                BaseConditionTax = new CompanyTax
                {
                    Id = coreTax.BaseConditionTax.Id,
                    Description = coreTax.BaseConditionTax.Description != null ? coreTax.BaseConditionTax.Description : string.Empty
                },
                AdditionalRateType = new CompanyTaxRate
                {
                    Id = coreTax.AdditionalRateType.Id,
                    Description = coreTax.AdditionalRateType.Description != null ? coreTax.AdditionalRateType.Description : string.Empty
                },
                TaxRates = coreTax.TaxRates.Select(t => new CompanyParamTaxRate { 
                    Id = t.Id,
                    IdTax = t.IdTax,
                    TaxCondition = new CompanyParamTaxCondition
                    {
                        Id = t.TaxCondition.Id,
                        Description = t.TaxCondition.Description
                    },
                    TaxCategory = new CompanyParamTaxCategory
                    {
                        Id = t.TaxCategory.Id,
                        Description = t.TaxCategory.Description
                    },
                    LineBusiness = new CompanyParamLineBusiness
                    {
                        Id = t.LineBusiness.Id,
                        Description = t.LineBusiness.Description
                    },
                    TaxState = new CompanyParamTaxState
                    {
                        DescriptionState = t.TaxState.DescriptionState,
                        DescriptionCity = t.TaxState.DescriptionCity,
                        DescriptionCountry = t.TaxState.DescriptionCountry,
                        IdState = t.TaxState.IdState,
                        IdCountry = t.TaxState.IdCountry,
                        IdCity = t.TaxState.IdCity
                    },
                    EconomicActivity = new CompanyParamEconomicActivity
                    {
                        Id = t.EconomicActivity.Id,
                        Description = t.EconomicActivity.Description
                    },
                    Branch = new CompanyParamBranch
                    {
                        Id = t.Branch.Id,
                        Description = t.Branch.Description
                    },
                    TaxPeriodRate = new CompanyParamTaxPeriodRate
                    {
                        Id = t.TaxPeriodRate.Id,
                        CurrentFrom = t.TaxPeriodRate.CurrentFrom,
                        Rate = t.TaxPeriodRate.Rate,
                        AdditionalRate = t.TaxPeriodRate.AdditionalRate,
                        BaseTaxAdditional = t.TaxPeriodRate.BaseTaxAdditional,
                        MinBaseAMT = t.TaxPeriodRate.MinBaseAMT,
                        MinAdditionalBaseAMT = t.TaxPeriodRate.MinAdditionalBaseAMT,
                        MinTaxAMT = t.TaxPeriodRate.MinTaxAMT,
                        MinAdditionalTaxAMT = t.TaxPeriodRate.MinAdditionalTaxAMT
                    },
                    Coverage = new CompanyParamCoverage
                    {
                        Id = t.Coverage.Id,
                        Description = t.Coverage.Description
                    }
                }).ToList(),
                TaxCategories = coreTax.TaxCategories.Select(t => new CompanyParamTaxCategory { 
                    Id = t.Id,
                    IdTax = t.IdTax,
                    Description = t.Description
                }).ToList(),
                TaxConditions = coreTax.TaxConditions.Select(t => new CompanyParamTaxCondition { 
                    Id = t.Id,
                    IdTax = t.IdTax,
                    Description = t.Description,
                    HasNationalRate = t.HasNationalRate,
                    IsIndependent = t.IsIndependent,
                    IsDefault = t.IsDefault
                }).ToList()
            };
            return paramTaxValue;
        }

        public static List<CompanyParamTax> MapTaxesCoreTocompanyParamTaxes(List<ParamTax> paramTaxes)
        {
            List<CompanyParamTax> companyParamTaxes = new List<CompanyParamTax>();
            foreach (ParamTax item in paramTaxes)
            {
                companyParamTaxes.Add(MapTaxCoreToCompany(item));
            }
            return companyParamTaxes;
        }
        #endregion


        #region TaxRateMapper

        public static CompanyParamTaxRate MapTaxRateCoreToCompany(ParamTaxRate coreTaxRate)
        {
            CompanyParamTaxRate paramTaxRateValue = new CompanyParamTaxRate()
            {
                Id = coreTaxRate.Id,
                IdTax = coreTaxRate.IdTax,
                TaxCondition = new CompanyParamTaxCondition
                {
                    Id = coreTaxRate.TaxCondition.Id
                },
                TaxCategory = new CompanyParamTaxCategory
                {
                    Id = coreTaxRate.TaxCategory.Id
                },
                LineBusiness = new CompanyParamLineBusiness
                {
                    Id = coreTaxRate.LineBusiness.Id
                },
                TaxState = new CompanyParamTaxState
                {
                    IdState = coreTaxRate.TaxState.IdState,
                    IdCountry = coreTaxRate.TaxState.IdCountry
                },
                EconomicActivity = new CompanyParamEconomicActivity
                {
                    Id = coreTaxRate.EconomicActivity.Id
                },
                Branch = new CompanyParamBranch
                {
                    Id = coreTaxRate.Branch.Id
                },
                TaxPeriodRate = new CompanyParamTaxPeriodRate
                {
                    Id = coreTaxRate.TaxPeriodRate.Id,
                    CurrentFrom = coreTaxRate.TaxPeriodRate.CurrentFrom,
                    Rate = coreTaxRate.TaxPeriodRate.Rate,
                    AdditionalRate = coreTaxRate.TaxPeriodRate.AdditionalRate,
                    BaseTaxAdditional = coreTaxRate.TaxPeriodRate.BaseTaxAdditional,
                    MinBaseAMT = coreTaxRate.TaxPeriodRate.MinBaseAMT,
                    MinAdditionalBaseAMT = coreTaxRate.TaxPeriodRate.MinAdditionalBaseAMT,
                    MinTaxAMT = coreTaxRate.TaxPeriodRate.MinTaxAMT,
                    MinAdditionalTaxAMT = coreTaxRate.TaxPeriodRate.MinAdditionalTaxAMT
                },
                Coverage = new CompanyParamCoverage
                {
                    Id = coreTaxRate.Coverage.Id
                }
            };
            return paramTaxRateValue;
        }

        public static List<CompanyParamTaxRate> MapTaxRatesCoreToCompanyParamTaxRates(List<ParamTaxRate> paramTaxRates)
        {
            List<CompanyParamTaxRate> companyParamTaxRates = new List<CompanyParamTaxRate>();
            foreach (ParamTaxRate item in paramTaxRates)
            {
                companyParamTaxRates.Add(MapTaxRateCoreToCompany(item));
            }
            return companyParamTaxRates;
        }
        #endregion


        #region TaxCategoryMapper

        public static CompanyParamTaxCategory MapTaxCategoryCoreToCompany(ParamTaxCategory coreTaxCategory)
        {
            CompanyParamTaxCategory paramTaxCategoryValue = new CompanyParamTaxCategory()
            {
                Id = coreTaxCategory.Id,
                IdTax = coreTaxCategory.IdTax,
                Description = coreTaxCategory.Description
            };
            return paramTaxCategoryValue;
        }

        public static List<CompanyParamTaxCategory> MapTaxCategoriesCoreToCompanyParamTaxCategories(List<ParamTaxCategory> paramTaxCategories)
        {
            List<CompanyParamTaxCategory> companyParamTaxCategories = new List<CompanyParamTaxCategory>();
            foreach (ParamTaxCategory item in paramTaxCategories)
            {
                companyParamTaxCategories.Add(MapTaxCategoryCoreToCompany(item));
            }
            return companyParamTaxCategories;
        }
        #endregion


        #region TaxConditionMapper

        public static CompanyParamTaxCondition MapTaxConditionCoreToCompany(ParamTaxCondition coreTaxCondition)
        {
            CompanyParamTaxCondition paramTaxConditionValue = new CompanyParamTaxCondition()
            {
                Id = coreTaxCondition.Id,
                IdTax = coreTaxCondition.IdTax,
                Description = coreTaxCondition.Description,
                HasNationalRate = coreTaxCondition.HasNationalRate,
                IsIndependent = coreTaxCondition.IsIndependent,
                IsDefault = coreTaxCondition.IsDefault
            };
            return paramTaxConditionValue;
        }

        public static List<CompanyParamTaxCondition> MapTaxConditionsCoreToCompanyParamTaxConditions(List<ParamTaxCondition> paramTaxConditions)
        {
            List<CompanyParamTaxCondition> companyParamTaxConditions = new List<CompanyParamTaxCondition>();
            foreach (ParamTaxCondition item in paramTaxConditions)
            {
                companyParamTaxConditions.Add(MapTaxConditionCoreToCompany(item));
            }
            return companyParamTaxConditions;
        }
        #endregion

        #endregion
    }
}
