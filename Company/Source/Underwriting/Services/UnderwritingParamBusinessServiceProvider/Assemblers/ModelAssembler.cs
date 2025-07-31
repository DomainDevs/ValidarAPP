// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author></author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Assemblers
{
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Company.Application.UnderwritingParamApplicationService.DTOs;
    using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
    using Sistran.Company.Application.Common.Entities;
    using CoreCommonEntities = Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Parameters.Entities;
    using CoreProductEntities = Sistran.Core.Application.Product.Entities;
    using CoreQuotationtEntities = Sistran.Core.Application.Quotation.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Core.Application.Product.Entities;

    /// <summary>
    /// ModelAssembler. Proveedor del servicio de aplicación.
    /// </summary>
    public class ModelAssembler
    {

        #region VehicleType_Previsora

        public static VehicleType CreateVehicleType(CompanyVehicleType companyVehicleType)
        {
            return new VehicleType
            {
                Id = companyVehicleType.Id,
                Description = companyVehicleType.Description,
                SmallDescription = companyVehicleType.SmallDescription,
                IsTruck = companyVehicleType.IsTruck,
                IsActive = companyVehicleType.IsActive,
                State = companyVehicleType.State,
                VehicleBodies = CreateVehicleBodies(companyVehicleType.VehicleBodies),
                ExtendedProperties = companyVehicleType.ExtendedProperties
            };

        }

        public static List<VehicleType> CreateVehicleTypes(List<CompanyVehicleType> companyVehicleTypes)
        {
            List<VehicleType> vehicleTypes = new List<VehicleType>();

            foreach (CompanyVehicleType item in companyVehicleTypes)
            {
                vehicleTypes.Add(CreateVehicleType(item));
            }

            return vehicleTypes;
        }

        public static CompanyVehicleType CreateCompanyVehicleType(VehicleType vehicleType)
        {
            return new CompanyVehicleType
            {
                Id = vehicleType.Id,
                Description = vehicleType.Description,
                SmallDescription = vehicleType.SmallDescription,
                IsTruck = vehicleType.IsTruck,
                IsActive = vehicleType.IsActive,
                State = vehicleType.State,
                VehicleBodies = vehicleType.VehicleBodies != null ? CreateCompanyVehicleBodies(vehicleType.VehicleBodies) : new List<CompanyVehicleBody>(),
                ExtendedProperties = vehicleType.ExtendedProperties

            };
        }

        public static List<CompanyVehicleType> CreateCompanyVehicleTypes(List<VehicleType> vehicleTypes)
        {
            List<CompanyVehicleType> companyVehicleTypes = new List<CompanyVehicleType>();

            foreach (VehicleType item in vehicleTypes)
            {
                companyVehicleTypes.Add(CreateCompanyVehicleType(item));
            }

            return companyVehicleTypes;
        }


        #endregion

        #region VehicleBody_Previsora

        public static VehicleBody CreateVehicleBody(CompanyVehicleBody companyVehicleBody)
        {
            return new VehicleBody
            {
                Id = companyVehicleBody.Id,
                SmallDescription = companyVehicleBody.SmallDescription,
                State = companyVehicleBody.State,
                //VehicleUses = companyVehicleBody.VehicleUses != null ? CreateVehicleUses(companyVehicleBody.VehicleUses) : new List<VehicleUse>()
            };
        }

        public static List<VehicleBody> CreateVehicleBodies(List<CompanyVehicleBody> companyVehicleBodies)
        {
            List<VehicleBody> vehicleBodies = new List<VehicleBody>();

            foreach (CompanyVehicleBody item in companyVehicleBodies)
            {
                vehicleBodies.Add(CreateVehicleBody(item));
            }

            return vehicleBodies;
        }

        public static CompanyVehicleBody CreateCompanyVehicleBody(VehicleBody vehicleBody)
        {
            return new CompanyVehicleBody
            {
                Id = vehicleBody.Id,
                SmallDescription = vehicleBody.SmallDescription,
                State = vehicleBody.State,
                //VehicleUses = vehicleBody.VehicleUses != null ? CreateCompanyVehicleUses(vehicleBody.VehicleUses) : new List<CompanyVehicleUse>()
            };
        }

        public static List<CompanyVehicleBody> CreateCompanyVehicleBodies(List<VehicleBody> vehicleBodies)
        {
            List<CompanyVehicleBody> companyVehicleBodies = new List<CompanyVehicleBody>();

            foreach (VehicleBody item in vehicleBodies)
            {
                companyVehicleBodies.Add(CreateCompanyVehicleBody(item));
            }

            return companyVehicleBodies;
        }
        #endregion
        #region AllyCoverage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ally_coverages"></param>
        /// <returns></returns>
        public static List<AllyCoverageDTO> MappParamAllyCoverages()
        {
            List<AllyCoverageDTO> coverages = new List<AllyCoverageDTO>();

            return coverages;
        }
        /// <summary>
        /// 
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

        #endregion

        public static CompanyParamMinPremiunRelation CreateCompanyParam(MinPremiumRelation entity)
        {

            var response = new CompanyParamMinPremiunRelation()
            {
                Id = entity.MinPremiumRelId,
                Branch = new CompanyParamBranch() { Id = entity.BranchCode.Value },
                Currency = new CompanyParamCurrency() { Id = entity.CurrencyCode },
                EndorsementType = new CompanyParamEndorsementType() { Id = entity.EndoTypeCode },
                Product = new CompanyParamProduct() { Id = int.Parse(entity.Key1.ToString()) },
                GroupCoverage = new CompanyParamGroupCoverage() { Id = entity.Key2 == null ? 0 : (int.Parse(entity.Key2.ToString())) },
                MinPremiunRange = new CompanyParamMinPremiunRange() { Id = entity.Key2 == null ? 0 : (int.Parse(entity.Key2.ToString()))},
                Prefix = new CompanyParamPrefix() { Id = entity.PrefixCode },
                RiskMinPremiun = entity.RiskMinPremium,
                SubMinPremiun = entity.SubsMinPremium
            };
            return response;
        }

        public static CompanyParamMinPremiunRelation CreateCompanyParam(MinPremiumRelation entity,
            CoreCommonEntities.Branch branch,
            CoreCommonEntities.Currency currency,
            EndorsementType EndorsementType,
            CoreProductEntities.Product product,
            CoreProductEntities.GroupCoverage groupCoverage,
            MinPremiumRange minPremiunRange,
            CoreCommonEntities.Prefix prefix)
        {
            var result = CreateCompanyParam(entity);

            if (branch != null)
            {
                result.Branch.Description = branch.Description;
            }

            if (currency != null)
            {
                result.Currency.Description = currency.Description;
            }

            if (EndorsementType != null)
            {
                result.EndorsementType.Description = EndorsementType.Description;
            }

            if (product != null)
            {
                result.Product.Description = product.Description;
            }
           
            if (prefix != null)
            {
                result.Prefix.Description = prefix.Description;
            }
            return result;
        }

        public static CompanyParamCoverage CreateCompanyParam(CoreQuotationtEntities.Coverage entity)
        {
            var response = new CompanyParamCoverage()
            {
                Id = entity.CoverageId,
                Description = entity.PrintDescription
            };
            return response;
        }
        public static ExcelFileDTO MappExcelFile(CompanyExcel companyExcel)
        {
            ExcelFileDTO excel = new ExcelFileDTO
            {
                File = companyExcel.FileData,
            };

            return excel;
        }

        #region MinPremiumRelation
        public static CompanyParamCoverage CreateGroupcoverageToCompany(ProductGroupCover productGroupCover)
        {
            return new CompanyParamCoverage
            {
                Description = productGroupCover.SmallDescription,               
                Id = productGroupCover.CoverGroupId
            };
        }
        public static CompanyParamCoverage CreateMinRangeToCompany(MinPremiumRange minPremiumRange)
        {
            return new CompanyParamCoverage
            {
                Description = minPremiumRange.Description,
                Id = minPremiumRange.MinPremiumRangeId
            };
        }
        #endregion
    }
}
