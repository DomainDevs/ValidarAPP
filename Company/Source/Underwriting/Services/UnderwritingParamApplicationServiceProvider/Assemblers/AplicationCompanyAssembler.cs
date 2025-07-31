// -----------------------------------------------------------------------
// <copyright file="AplicationCompanyAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>William Martin</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamApplicationServiceProvider.Assemblers
{
    
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
    using Sistran.Core.Application.UnderwritingServices.Models.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// AplicationCompanyAssembler. Interfaz De servicio de aplicación
    /// </summary>
    public class AplicationCompanyAssembler
    {
        #region VehicleType

        public static VehicleTypeDTO CreateVehicleType(CompanyVehicleType companyVehicleType)
        {
            return new VehicleTypeDTO
            {
                Id = companyVehicleType.Id,
                Description = companyVehicleType.Description,
                SmallDescription = companyVehicleType.SmallDescription,
                IsTruck = companyVehicleType.IsTruck,
                IsActive = companyVehicleType.IsActive,
                State = Convert.ToInt32(companyVehicleType.State),
                VehicleBodies = CreateVehicleBodies(companyVehicleType.VehicleBodies)
            };
        }

        public static List<VehicleTypeDTO> CreateVehicleTypes(List<CompanyVehicleType> companyVehicleTypes)
        {
            List<VehicleTypeDTO> vehicleTypes = new List<VehicleTypeDTO>();

            foreach (CompanyVehicleType item in companyVehicleTypes)
            {
                vehicleTypes.Add(CreateVehicleType(item));
            }

            return vehicleTypes;
        }

        #endregion

        #region VehicleBody

        public static VehicleBodyDTO CreateVehicleBody(CompanyVehicleBody companyVehicleBody)
        {
            return new VehicleBodyDTO
            {
                Id = companyVehicleBody.Id,
                Description = companyVehicleBody.SmallDescription,
                State = Convert.ToInt32(companyVehicleBody.State)
            };
        }

        public static List<VehicleBodyDTO> CreateVehicleBodies(List<CompanyVehicleBody> companyVehicleBodies)
        {
            List<VehicleBodyDTO> vehicleBodies = new List<VehicleBodyDTO>();

            foreach (CompanyVehicleBody item in companyVehicleBodies)
            {
                vehicleBodies.Add(CreateVehicleBody(item));
            }

            return vehicleBodies;
        }

        #endregion
        #region AllyCoverage

        /// <summary>
        /// Mapping List from DTO list-Objet to CompanyParam list-object
        /// </summary>
        /// <param name="ally_coverages"></param>
        /// <returns></returns>
        public static List<CompanyParamAllyCoverage> MappParamAllyCoverages(List<AllyCoverageDTO> ally_coverages)
        {
            List<CompanyParamAllyCoverage> coverages = new List<CompanyParamAllyCoverage>();
            foreach (var item in ally_coverages)
            {
                coverages.Add(MappParamAllyCoverage(item));
            }
            return coverages;
        }
        /// <summary>
        /// Mapping from DTO Objet to CompanyParam object
        /// </summary>
        /// <param name="ally_coverages"></param>
        /// <returns></returns>
        public static CompanyParamAllyCoverage MappParamAllyCoverage(AllyCoverageDTO item)
        {
            CompanyParamAllyCoverage allyCoverageDTO = new CompanyParamAllyCoverage
            {
                AllyCoverageId = item.AllyCoverageId,
                CoverageId = item.CoverageId,
                CoveragePct = item.CoveragePct
            };

            return allyCoverageDTO;
        }

        public static List<CompanyParamQueryAllyCoverage> MappParamQueryAllyCoverage(List<QueryAllyCoverageDTO> query_list)
        {
            var result = new List<CompanyParamQueryAllyCoverage>();
            query_list.ForEach((x) =>
            {
                result.Add(new CompanyParamQueryAllyCoverage
                {
                    AllyCoverage = MappingQueryCoverageObject(x.AllyCoverageId),
                    Coverage = MappingQueryCoverageObject(x.CoverageId),
                    CoveragePct = x.CoveragePct

                });
            });
            return result;
        }

        private static CompanyParamQueryCoverage MappingQueryCoverageObject(QueryCoverageDTO allyCoverageId)
        {
            return new CompanyParamQueryCoverage
            {
                Id = allyCoverageId.Id,
                PerilId = allyCoverageId.PerilId,
                SubLineBusinessId = allyCoverageId.SubLineBusinessId,
                LineBusinessId = allyCoverageId.LineBusinessId,
                InsuredObjectId = allyCoverageId.InsuredObjectId,
                PrintDescription = allyCoverageId.PrintDescription,
                IsPrimary = allyCoverageId.IsPrimary,
                ExpirationDate = allyCoverageId.ExpirationDate,
                CompositionTypeId = allyCoverageId.CompositionTypeId,
                RuleSetId = allyCoverageId.RuleSetId
            };
        }
        #endregion

        #region MinPremiumRelation
        public static CompanyParamMinPremiunRelation Mapper (MinPremiunRelationDTO dto)
        {
            var response = new CompanyParamMinPremiunRelation()
            {
                Id = dto.Id,
                Branch = (dto.Branch != null ? new CompanyParamBranch() { Id = dto.Branch.Id, Description = dto.Branch.Description } : null),
                Currency = (dto.Currency != null ? new CompanyParamCurrency() { Id = dto.Currency.Id, Description = dto.Currency.Description } : null),
                EndorsementType = (dto.EndorsementType != null ? new CompanyParamEndorsementType() { Id = dto.EndorsementType.Id, Description = dto.EndorsementType.Description } : null),
                GroupCoverage = (dto.GroupCoverage != null ? new CompanyParamGroupCoverage() { Id = dto.GroupCoverage.Id, Description = dto.GroupCoverage.Description } : null),
                MinPremiunRange = (dto.MinPremiunRange != null ? new CompanyParamMinPremiunRange() { Id = dto.MinPremiunRange.Id, Description = dto.MinPremiunRange.Description } : null),
                Prefix = (dto.Prefix != null ? new CompanyParamPrefix { Id = dto.Prefix.Id, Description = dto.Prefix.Description } : null),
                Product = (dto.Product != null ? new CompanyParamProduct { Id = dto.Product.Id, Description = dto.Product.Description } : null),
                RiskMinPremiun = dto.RiskMinPremiun,
                SubMinPremiun = dto.SubMinPremiun
            };
            return response;
        }
        #endregion

        #region MapeoCoCoverageValue
        public static CompanyParamCoCoverageValue MappCompanyParamCoCoverageValue(CoCoverageValueDTO coCoverageValueDTO)
        {
            CompanyParamCoCoverageValue companyParamCoCoverageValue = new CompanyParamCoCoverageValue();
            companyParamCoCoverageValue.Percentage = coCoverageValueDTO.Porcentage;
            companyParamCoCoverageValue.Coverage= new CompanyParamCoverage { Id=coCoverageValueDTO.Coverage.Id, Description=coCoverageValueDTO.Coverage.Description};
            companyParamCoCoverageValue.Prefix= new CompanyParamPrefix { Id= coCoverageValueDTO.Prefix.Id, Description=coCoverageValueDTO.Prefix.Description};

            return companyParamCoCoverageValue;
        }
        #endregion
        
        #region ConditionText
        /// <summary>
        /// MappCompanyConditionalText. Modelo DTO condition text a modelo Company.
        /// </summary>
        /// <param name="ConditionTextDto">Modelo DTO ConditionText</param>
        /// <returns>CompanyParamConditionText. Modelo Company</returns>
        public static CompanyParamConditionText MappCompanyConditionalText(ConditionTextDTO ConditionTextDto)
        {
            CompanyParamConditionText CompanyParamConditionText =new CompanyParamConditionText();
            CompanyParamConditionText.Id = ConditionTextDto.Id;
            CompanyParamConditionText.Title = ConditionTextDto.Title;
            CompanyParamConditionText.Body = ConditionTextDto.Body;
            CompanyParamConditionText.ConditionTextLevel = new BaseConditionTextLevel { Id = ConditionTextDto.ConditionTextLevel.Id, Description = ConditionTextDto.ConditionTextLevel.Description ?? "" };
            CompanyParamConditionText.ConditionTextLevelType = new BaseConditionTextLevelType { Id = ConditionTextDto.ConditionTextLevelType.Id, Description = ConditionTextDto.ConditionTextLevelType.Description ?? "" };

            return CompanyParamConditionText;
        }
        #endregion

        #region Tax

        #region TaxMethods
        public static CompanyParamTax MappCompanyDTOtoParamTax(TaxDTO taxDTO)
        {
            CompanyParamTax companyParamTax = new CompanyParamTax()
            {
                Id = taxDTO.Id,
                Description = taxDTO.Description,
                TinyDescription = taxDTO.TinyDescription,
                CurrentFrom = taxDTO.CurrentFrom,
                IsSurPlus = taxDTO.IsSurPlus,
                IsAdditionalSurPlus = taxDTO.IsAdditionalSurPlus,
                Enabled = taxDTO.Enabled,
                IsEarned = taxDTO.IsEarned,
                IsRetention = taxDTO.IsRetention,
                RateType = new CompanyTaxRate
                {
                    Id = taxDTO.RateType.Id,
                    Description = taxDTO.RateType.Description
                },
                TaxRoles = taxDTO.TaxRoles.Select(t => new CompanyTaxRole { Id = t.Id, Description = t.Description }).ToList(),
                TaxAttributes = taxDTO.TaxAttributes.Select(t => new CompanyTaxAttribute { Id = t.Id, Description = t.Description }).ToList(),
                RetentionTax = new CompanyTax
                {
                    Id = taxDTO.RetentionTax.Id,
                    Description = taxDTO.RetentionTax.Description
                },
                BaseConditionTax = new CompanyTax
                {
                    Id = taxDTO.BaseConditionTax.Id,
                    Description = taxDTO.BaseConditionTax.Description
                },
                AdditionalRateType = new CompanyTaxRate
                {
                    Id = taxDTO.AdditionalRateType.Id,
                    Description = taxDTO.AdditionalRateType.Description
                }
            };
            return companyParamTax;
        }

        #endregion

        #region TaxRate Methods

        public static CompanyParamTaxRate MappCompanyDTOtoParamTaxRate(TaxRateDTO taxRateDTO)
        {
            CompanyParamTaxRate companyParamTaxRate = new CompanyParamTaxRate()
            {
                Id = taxRateDTO.Id,
                IdTax = taxRateDTO.IdTax,
                TaxCondition = new CompanyParamTaxCondition
                {
                    Id = taxRateDTO.TaxCondition.Id
                },
                TaxCategory = new CompanyParamTaxCategory
                {
                    Id = taxRateDTO.TaxCategory.Id
                },
                LineBusiness = new CompanyParamLineBusiness
                {
                    Id = taxRateDTO.LineBusiness.Id
                },
                TaxState = new CompanyParamTaxState
                {
                    IdState = taxRateDTO.TaxState.IdState,
                    IdCountry = taxRateDTO.TaxState.IdCountry,
                    IdCity = taxRateDTO.TaxState.IdCity
                },
                EconomicActivity = new CompanyParamEconomicActivity
                {
                    Id = taxRateDTO.EconomicActivity.Id
                },
                Branch = new CompanyParamBranch
                {
                    Id = taxRateDTO.Branch.Id
                },
                TaxPeriodRate = new CompanyParamTaxPeriodRate
                {
                    CurrentFrom = taxRateDTO.TaxPeriodRate.CurrentFrom,
                    Rate = taxRateDTO.TaxPeriodRate.Rate,
                    AdditionalRate = taxRateDTO.TaxPeriodRate.AdditionalRate,
                    BaseTaxAdditional = taxRateDTO.TaxPeriodRate.BaseTaxAdditional,
                    MinBaseAMT = taxRateDTO.TaxPeriodRate.MinBaseAMT,
                    MinAdditionalBaseAMT = taxRateDTO.TaxPeriodRate.MinAdditionalBaseAMT,
                    MinTaxAMT = taxRateDTO.TaxPeriodRate.MinTaxAMT,
                    MinAdditionalTaxAMT = taxRateDTO.TaxPeriodRate.MinAdditionalTaxAMT
                },
                Coverage = new CompanyParamCoverage
                {
                   Id = taxRateDTO.Coverage.Id 
                }
            };
            return companyParamTaxRate;
        }
        #endregion

        #region TaxCategory Methods

        public static CompanyParamTaxCategory MappCompanyDTOtoParamTaxCategory(TaxCategoryDTO taxCategoryDTO)
        {
            CompanyParamTaxCategory companyParamTaxCategory = new CompanyParamTaxCategory()
            {
                Id = taxCategoryDTO.Id,
                IdTax = taxCategoryDTO.IdTax,
                Description = taxCategoryDTO.Description
            };
            return companyParamTaxCategory;
        }
        #endregion

        #region TaxCondition Methods

        public static CompanyParamTaxCondition MappCompanyDTOtoParamTaxCondition(TaxConditionDTO taxConditionDTO)
        {
            CompanyParamTaxCondition companyParamTaxCondition = new CompanyParamTaxCondition()
            {
                Id = taxConditionDTO.Id,
                IdTax = taxConditionDTO.IdTax,
                Description = taxConditionDTO.Description,
                HasNationalRate = taxConditionDTO.HasNationalRate,
                IsIndependent = taxConditionDTO.IsIndependent,
                IsDefault = taxConditionDTO.IsDefault
            };
            return companyParamTaxCondition;
        }
        #endregion

        #endregion
    }
}
