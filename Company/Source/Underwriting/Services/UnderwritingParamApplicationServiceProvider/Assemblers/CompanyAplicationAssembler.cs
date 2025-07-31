// -----------------------------------------------------------------------
// <copyright file="CompanyAplicationAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>William Martin</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamApplicationServiceProvider.Assemblers
{
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
    using Sistran.Company.Application.Utilities.DTO;
    using AutoMapper;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// CompanyAplicationAssembler. Interfaz De servicio de aplicación
    /// </summary>
    public class CompanyAplicationAssembler
    {
        #region VehicleType

        public static CompanyVehicleType CreateCompanyVehicleType(VehicleTypeDTO vehicleType)
        {
            return new CompanyVehicleType
            {
                Id = Convert.ToInt32(vehicleType.Id),
                Description = vehicleType.Description,
                SmallDescription = vehicleType.SmallDescription,
                IsTruck = vehicleType.IsTruck,
                IsActive = vehicleType.IsActive,
                State = vehicleType.State,
                VehicleBodies = vehicleType.VehicleBodies != null ? CreateCompanyVehicleBodies(vehicleType.VehicleBodies) : new List<CompanyVehicleBody>()
            };
        }

        public static List<CompanyVehicleType> CreateCompanyVehicleTypes(List<VehicleTypeDTO> vehicleTypes)
        {
            List<CompanyVehicleType> companyVehicleTypes = new List<CompanyVehicleType>();

            foreach (VehicleTypeDTO item in vehicleTypes)
            {
                companyVehicleTypes.Add(CreateCompanyVehicleType(item));
            }

            return companyVehicleTypes;
        }

        #endregion

        #region VehicleBody

        public static CompanyVehicleBody CreateCompanyVehicleBody(VehicleBodyDTO vehicleBody)
        {
            return new CompanyVehicleBody
            {
                Id = Convert.ToInt32(vehicleBody.Id),
                SmallDescription = vehicleBody.Description,
                State = vehicleBody.State
            };
        }

        public static List<CompanyVehicleBody> CreateCompanyVehicleBodies(List<VehicleBodyDTO> vehicleBodies)
        {
            List<CompanyVehicleBody> companyVehicleBodies = new List<CompanyVehicleBody>();

            foreach (VehicleBodyDTO item in vehicleBodies)
            {
                companyVehicleBodies.Add(CreateCompanyVehicleBody(item));
            }

            return companyVehicleBodies;
        }
        #endregion
        #region AllyCoverage

        /// <summary>
        /// Mapping List from CompanyParam list-Objet to DTO list-object
        /// </summary>
        /// <param name="ally_coverages"></param>
        /// <returns></returns>
        public static List<AllyCoverageDTO> MappParamAllyCoverages(List<CompanyParamAllyCoverage> ally_coverages)
        {
            List<AllyCoverageDTO> coverages = new List<AllyCoverageDTO>();
            foreach (var item in ally_coverages)
            {
                coverages.Add(MappParamAllyCoverage(item));
            }
            return coverages;
        }
        /// <summary>
        /// Mapping from CompanyParam Objet to DTO object
        /// </summary>
        /// <param name="ally_coverages"></param>
        /// <returns></returns>
        public static AllyCoverageDTO MappParamAllyCoverage(CompanyParamAllyCoverage ally_coverage)
        {
            AllyCoverageDTO allyCoverageDTO = new AllyCoverageDTO
            {
                AllyCoverageId = ally_coverage.AllyCoverageId,
                CoverageId = ally_coverage.CoverageId,
                CoveragePct = ally_coverage.CoverageId
            };

            return allyCoverageDTO;
        }

        public static void MappParamQueryAllyCoverage()
        {

        }

        #endregion
        #region MinPrimiumRelation
        public static MinPremiunRelationDTO Mapper(CompanyParamMinPremiunRelation companyParam)
        {
            var response = new MinPremiunRelationDTO()
            {
                Id = companyParam.Id,
                ErrorDTO = new ErrorDTO(),
                Branch = (companyParam.Branch != null ? new BranchDTO() { Id = companyParam.Branch.Id, Description = companyParam.Branch.Description } : null),
                Currency = (companyParam.Currency != null ? new CurrencyDTO() { Id = companyParam.Currency.Id, Description = companyParam.Currency.Description } : null),
                EndorsementType = (companyParam.EndorsementType != null ? new EndorsementTypeDTO() { Id = companyParam.EndorsementType.Id, Description = companyParam.EndorsementType.Description } : null),
                GroupCoverage = (companyParam.GroupCoverage != null ? new GroupCoverageDTO() { Id = companyParam.GroupCoverage.Id.Value, Description = companyParam.GroupCoverage.Description } : null),
                MinPremiunRange = (companyParam.MinPremiunRange != null ? new MinPremiunRangeDTO() { Id = companyParam.MinPremiunRange.Id, Description = companyParam.MinPremiunRange.Description } : null),
                Prefix = (companyParam.Prefix != null ? new PrefixDTO { Id = companyParam.Prefix.Id, Description = companyParam.Prefix.Description } : null),
                Product = (companyParam.Product != null ? new ProductDTO { Id = companyParam.Product.Id.Value, Description = companyParam.Product.Description } : null),
                RiskMinPremiun = companyParam.RiskMinPremiun.HasValue ? companyParam.RiskMinPremiun.Value : 0,
                SubMinPremiun = companyParam.SubMinPremiun.HasValue ? companyParam.SubMinPremiun.Value : 0
            };
            return response;
        }
        public static MinPremiunRelationQueryDTO Mapper(List<CompanyParamMinPremiunRelation> companyParam)
        {
            var result = new MinPremiunRelationQueryDTO()
            {
                ErrorDTO = new ErrorDTO(),
                MinPremiunRelationDTO = new List<MinPremiunRelationDTO>()
            };

            if (companyParam != null)
            {
                companyParam.ForEach(x => result.MinPremiunRelationDTO.Add(Mapper(x)));
            }

            return result;
        }
        #endregion

        #region mapeadorCoCoverageValue
        public static CoCoverageValueDTO MappCoCoverageValueQuery(CompanyParamCoCoverageValue companyParamCoCoverageValue)
        {
            CoCoverageValueDTO coCoverageValueDTO = new CoCoverageValueDTO
            {
                Coverage = new CoverageDTO { Id = companyParamCoCoverageValue.Coverage.Id, Description = companyParamCoCoverageValue.Coverage.Description },
                Prefix = new PrefixDTO { Id = companyParamCoCoverageValue.Prefix.Id, Description = companyParamCoCoverageValue.Prefix.Description },
                Porcentage = companyParamCoCoverageValue.Percentage
            };

            return coCoverageValueDTO;
        }

        public static List<CoCoverageValueDTO> MappCoCoverageValues(List<CompanyParamCoCoverageValue> companyParamCoCoverageValue)
        {
            List<CoCoverageValueDTO> coCoverageValues = new List<CoCoverageValueDTO>();
            foreach (var item in companyParamCoCoverageValue)
            {
                coCoverageValues.Add(MappCoCoverageValueQuery(item));
            }
            return coCoverageValues;
        }
        #endregion
        /// <summary>
        /// MappCompanyConditionalText. Modelo Company condition text a modelo DTO.
        /// </summary>
        /// <param name="ConditionTextDto">Modelo Company ConditionText</param>
        /// <returns>CompanyParamConditionText. Modelo DTO</returns>
        public static ConditionTextDTO MappConditionTextDTO(CompanyParamConditionText CompanyConditionText)
        {
            ConditionTextDTO ConditionTextDto = new ConditionTextDTO();
            ConditionTextDto.Id = CompanyConditionText.Id;
            ConditionTextDto.Title = CompanyConditionText.Title;
            ConditionTextDto.Body = CompanyConditionText.Body;
            ConditionTextDto.ConditionTextLevel = new ConditionTextLevelDTO { Id = CompanyConditionText.ConditionTextLevel.Id, Description = CompanyConditionText.ConditionTextLevel.Description };
            ConditionTextDto.ConditionTextLevelType = new ConditionTextLevelTypeDTO { Id = CompanyConditionText.ConditionTextLevelType.Id, Description = CompanyConditionText.ConditionTextLevelType.Description ?? "" };
            return ConditionTextDto;
        }
        /// <summary>
        /// MappCoverage: metodo para cambiar de companyparamcoverga a covergaDTO, par ala carga de combo cobertura
        /// </summary>
        /// <param name="companyParamCoverage"></param>
        /// <returns></returns>
        public static CoverageDTO MappCoverage(CompanyParamCoverage companyParamCoverage)
        {
            CoverageDTO overageDTO = new CoverageDTO
            {
                Id = companyParamCoverage.Id,
                Description = companyParamCoverage.Description
            };

            return overageDTO;
        }

        /// <summary>
        /// MappCoverages:metodo para cambiar de companyparamcoverga a covergaDTO, par ala carga de combo cobertura
        /// </summary>
        /// <param name="companyParamCoverage"></param>
        /// <returns></returns>
        public static List<CoverageDTO> MappCoverages(List<CompanyParamCoverage> companyParamCoverage)
        {
            List<CoverageDTO> Coverages = new List<CoverageDTO>();
            foreach (var item in companyParamCoverage)
            {
                Coverages.Add(MappCoverage(item));
            }
            return Coverages;
        }

        public static ExcelFileDTO MappExcelFile(CompanyExcel companyExcel)
        {
            ExcelFileDTO excel = new ExcelFileDTO
            {
                File = companyExcel.FileData,
            };

            return excel;
        }

        #region ConditionText
        public static List<ConditionTextDTO> MappConditionalTextsDTO(List<CompanyParamConditionText> conditionTexts)
        {
            List<ConditionTextDTO> listConditionTextDTO = new List<ConditionTextDTO>();
            foreach (var item in conditionTexts)
            {
                listConditionTextDTO.Add(MappConditionTextDTO(item));
            }
            return listConditionTextDTO;
        }
        #endregion


        #region Tax

        #region TaxMethods
        public static TaxDTO MappTaxCompanytoDTO(CompanyParamTax companyParamTax)
        {
            if (companyParamTax.TaxRoles == null)
            {
                companyParamTax.TaxRoles = new List<CompanyTaxRole>();
            }
            if (companyParamTax.TaxAttributes == null)
            {
                companyParamTax.TaxAttributes = new List<CompanyTaxAttribute>();
            }
            if (companyParamTax.TaxRates == null)
            {
                companyParamTax.TaxRates = new List<CompanyParamTaxRate>();
            }
            if (companyParamTax.TaxCategories == null)
            {
                companyParamTax.TaxCategories = new List<CompanyParamTaxCategory>();
            }
            if (companyParamTax.TaxConditions == null)
            {
                companyParamTax.TaxConditions = new List<CompanyParamTaxCondition>();
            }
            TaxDTO taxDTO = new TaxDTO
            {
                Id = companyParamTax.Id,
                Description = companyParamTax.Description,
                TinyDescription = companyParamTax.TinyDescription,
                CurrentFrom = companyParamTax.CurrentFrom,
                IsSurPlus = companyParamTax.IsSurPlus,
                IsAdditionalSurPlus = companyParamTax.IsAdditionalSurPlus,
                Enabled = companyParamTax.Enabled,
                IsEarned = companyParamTax.IsEarned,
                IsRetention = companyParamTax.IsRetention,
                RateType = new RateTypeDTO
                {
                    Id = companyParamTax.RateType.Id,
                    Description = companyParamTax.RateType.Description != null ? companyParamTax.RateType.Description : string.Empty
                },
                TaxRoles = companyParamTax.TaxRoles.Select(t => new TaxRoleDTO { Id = t.Id, Description = t.Description }).ToList(),
                TaxAttributes = companyParamTax.TaxAttributes.Select(t => new TaxAttributeDTO { Id = t.Id, Description = t.Description }).ToList(),
                RetentionTax = new BaseTaxDTO
                {
                    Id = companyParamTax.RetentionTax.Id,
                    Description = companyParamTax.RetentionTax.Description != null ? companyParamTax.RetentionTax.Description : string.Empty
                },
                BaseConditionTax = new BaseTaxDTO
                {
                    Id = companyParamTax.BaseConditionTax.Id,
                    Description = companyParamTax.BaseConditionTax.Description != null ? companyParamTax.BaseConditionTax.Description : string.Empty
                },
                AdditionalRateType = new RateTypeDTO
                {
                    Id = companyParamTax.AdditionalRateType.Id,
                    Description = companyParamTax.AdditionalRateType.Description != null ? companyParamTax.AdditionalRateType.Description : string.Empty
                },
                TaxRates = companyParamTax.TaxRates.Select(t => new TaxRateDTO
                {
                    Id = t.Id,
                    IdTax = t.IdTax,
                    TaxCondition = new TaxConditionDTO
                    {
                        Id = t.TaxCondition.Id,
                        Description = t.TaxCondition.Description
                    },
                    TaxCategory = new TaxCategoryDTO
                    {
                        Id = t.TaxCategory.Id,
                        Description = t.TaxCategory.Description
                    },
                    LineBusiness = new LineBusinnessDTO
                    {
                        Id = t.LineBusiness.Id,
                        Description = t.LineBusiness.Description
                    },
                    TaxState = new TaxStateDTO
                    {
                        DescriptionCity = t.TaxState.DescriptionCity,
                        DescriptionCountry = t.TaxState.DescriptionCountry,
                        DescriptionState = t.TaxState.DescriptionState,
                        IdState = t.TaxState.IdState,
                        IdCountry = t.TaxState.IdCountry,
                        IdCity  = t.TaxState.IdCity
                    },
                    EconomicActivity = new EconomicActivityDTO
                    {
                        Id = t.EconomicActivity.Id,
                        Description = t.EconomicActivity.Description
                    },
                    Branch = new BranchDTO
                    {
                        Id = t.Branch.Id,
                        Description = t.Branch.Description
                    },
                    TaxPeriodRate = new TaxPeriodRateDTO
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
                    Coverage = new CoverageDTO
                    {
                        Id = t.Coverage.Id,
                        Description = t.Coverage.Description
                    }
                }
                ).ToList(),
                TaxCategories = companyParamTax.TaxCategories.Select(t => new TaxCategoryDTO { 
                    Id = t.Id,
                    IdTax = t.IdTax,
                    Description = t.Description
                }).ToList(),
                TaxConditions = companyParamTax.TaxConditions.Select(t => new TaxConditionDTO { 
                    Id = t.Id,
                    IdTax = t.IdTax,
                    Description = t.Description, 
                    HasNationalRate = t.HasNationalRate,
                    IsIndependent = t.IsIndependent,
                    IsDefault = t.IsDefault
                }).ToList()
            };
            return taxDTO;
        }

        public static List<TaxDTO> MappTaxesCompanytoDTOs(List<CompanyParamTax> companyParamTaxes)
        {
            List<TaxDTO> taxDTOs = new List<TaxDTO>();
            foreach (CompanyParamTax item in companyParamTaxes)
            {
                taxDTOs.Add(MappTaxCompanytoDTO(item));
            }
            return taxDTOs;
        }
        #endregion

        #region TaxRate Methods
        public static TaxRateDTO MappTaxRateCompanytoDTO(CompanyParamTaxRate companyParamTaxRate)
        {
            TaxRateDTO taxRateDTO = new TaxRateDTO
            {
                Id = companyParamTaxRate.Id,
                IdTax = companyParamTaxRate.IdTax,
                TaxCondition = new TaxConditionDTO
                {
                    Id = companyParamTaxRate.TaxCondition.Id
                },
                TaxCategory = new TaxCategoryDTO
                {
                    Id = companyParamTaxRate.TaxCategory.Id
                },
                LineBusiness = new LineBusinnessDTO
                {
                    Id = companyParamTaxRate.LineBusiness.Id
                },
                TaxState = new TaxStateDTO
                {
                    IdState = companyParamTaxRate.TaxState.IdState,
                    IdCountry = companyParamTaxRate.TaxState.IdCountry
                },
                EconomicActivity = new EconomicActivityDTO
                {
                    Id = companyParamTaxRate.EconomicActivity.Id
                },
                Branch = new BranchDTO
                {
                    Id = companyParamTaxRate.Branch.Id
                },
                TaxPeriodRate = new TaxPeriodRateDTO
                {
                    Id = companyParamTaxRate.TaxPeriodRate.Id,
                    CurrentFrom = companyParamTaxRate.TaxPeriodRate.CurrentFrom,
                    Rate = companyParamTaxRate.TaxPeriodRate.Rate,
                    AdditionalRate = companyParamTaxRate.TaxPeriodRate.AdditionalRate,
                    BaseTaxAdditional = companyParamTaxRate.TaxPeriodRate.BaseTaxAdditional,
                    MinBaseAMT = companyParamTaxRate.TaxPeriodRate.MinBaseAMT,
                    MinAdditionalBaseAMT = companyParamTaxRate.TaxPeriodRate.MinAdditionalBaseAMT,
                    MinTaxAMT = companyParamTaxRate.TaxPeriodRate.MinTaxAMT,
                    MinAdditionalTaxAMT = companyParamTaxRate.TaxPeriodRate.MinAdditionalTaxAMT
                },
                Coverage = new CoverageDTO
                {
                    Id = companyParamTaxRate.Coverage.Id
                }
                
            };
            return taxRateDTO;
        }

        public static List<TaxRateDTO> MappTaxRatesCompanytoDTOs(List<CompanyParamTaxRate> companyParamTaxRates)
        {
            List<TaxRateDTO> taxRatesDTOs = new List<TaxRateDTO>();
            foreach (CompanyParamTaxRate item in companyParamTaxRates)
            {
                taxRatesDTOs.Add(MappTaxRateCompanytoDTO(item));
            }
            return taxRatesDTOs;
        }
        #endregion

        #region TaxCategory Methods
        public static TaxCategoryDTO MappTaxCategoryCompanytoDTO(CompanyParamTaxCategory companyParamTaxCategory)
        {
            TaxCategoryDTO taxCategoryDTO = new TaxCategoryDTO
            {
                Id = companyParamTaxCategory.Id,
                IdTax = companyParamTaxCategory.IdTax,
                Description = companyParamTaxCategory.Description
            };
            return taxCategoryDTO;
        }

        public static List<TaxCategoryDTO> MappTaxCategoriesCompanytoDTOs(List<CompanyParamTaxCategory> companyParamTaxCategories)
        {
            List<TaxCategoryDTO> taxCategoriesDTO = new List<TaxCategoryDTO>();
            foreach (CompanyParamTaxCategory item in companyParamTaxCategories)
            {
                taxCategoriesDTO.Add(MappTaxCategoryCompanytoDTO(item));
            }
            return taxCategoriesDTO;
        }
        #endregion

        #region TaxCondition Methods
        public static TaxConditionDTO MappTaxConditionCompanytoDTO(CompanyParamTaxCondition companyParamTaxCondition)
        {
            TaxConditionDTO taxConditionDTO = new TaxConditionDTO
            {
                Id = companyParamTaxCondition.Id,
                IdTax = companyParamTaxCondition.IdTax,
                Description = companyParamTaxCondition.Description,
                HasNationalRate = companyParamTaxCondition.HasNationalRate,
                IsIndependent = companyParamTaxCondition.IsIndependent,
                IsDefault = companyParamTaxCondition.IsDefault
            };
            return taxConditionDTO;
        }

        public static List<TaxConditionDTO> MappTaxConditionsCompanytoDTOs(List<CompanyParamTaxCondition> companyParamTaxConditions)
        {
            List<TaxConditionDTO> taxConditionsDTO = new List<TaxConditionDTO>();
            foreach (CompanyParamTaxCondition item in companyParamTaxConditions)
            {
                taxConditionsDTO.Add(MappTaxConditionCompanytoDTO(item));
            }
            return taxConditionsDTO;
        }
        #endregion

        #endregion
    }
}
