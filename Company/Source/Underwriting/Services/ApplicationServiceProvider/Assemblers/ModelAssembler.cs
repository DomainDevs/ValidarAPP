using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ApplicationServiceProvider.Assemblers
{
    internal static class ModelAssembler
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
                IsElectronicPolicy = vehicleType.IsElectronicPolicy,
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
                SmallDescription = vehicleBody.SmallDescription,
                State = vehicleBody.State,
                VehicleUses = vehicleBody.VehicleUses != null ? CreateCompanyVehicleUses(vehicleBody.VehicleUses) : new List<CompanyVehicleUse>()
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

        #region VehicleUse
        
        public static CompanyVehicleUse CreateCompanyVehicleUse(VehicleUseDTO vehicleUse)
        {
            return new CompanyVehicleUse
            {
                Id = Convert.ToInt32(vehicleUse.Id),
                SmallDescription = vehicleUse.SmallDescription,
                PrefixId = vehicleUse.PrefixId
            };
        }

        public static List<CompanyVehicleUse> CreateCompanyVehicleUses(List<VehicleUseDTO> vehicleUses)
        {
            List<CompanyVehicleUse> companyVehicleUses = new List<CompanyVehicleUse>();

            foreach (VehicleUseDTO item in vehicleUses)
            {
                companyVehicleUses.Add(CreateCompanyVehicleUse(item));
            }

            return companyVehicleUses;
        }

        #endregion

        #region CiaRatingZone
        public static CiaRatingZoneBranch CreateCiaRatingZoneBranch(CiaRatingZoneBranchDTO ratingZoneDTO)
        {
            return new CiaRatingZoneBranch
            {
                Branch = new CompanyBranch { Id = ratingZoneDTO.BranchCode },
                RatingZone = new CompanyRatingZone { Id = ratingZoneDTO.RatingZone }

            };
        }

        //public static List<CiaRatingZoneBranch> CreateCiaRatingZoneBranches(List<CiaRatingZoneBranchDTO> ratingZonesDTO)
        //{
        //    List<CiaRatingZoneBranch> companyRatingZones = new List<CiaRatingZoneBranch>();
        //    foreach (CiaRatingZoneBranchDTO item in ratingZonesDTO)
        //    {
        //        companyRatingZones.Add(CreateCiaRatingZoneBranch(item));
        //    }

        //    return companyRatingZones;
        //}

        public static CompanyRatingZone CreateCompanyRatingZone(CompanyRatingZoneDTO companyRatingZoneDTO)
        {
            return new CompanyRatingZone
            {
                Id = companyRatingZoneDTO.Id,
                Description = companyRatingZoneDTO.Description,
                Prefix = new CompanyPrefix { Id = companyRatingZoneDTO.PrefixCode },
                SmallDescription = companyRatingZoneDTO.SmallDescription

            };
        }

        public static List<CompanyRatingZone> CreateCompanyRatingZones(List<CompanyRatingZoneDTO> companyRatingZonesDTO)
        {
            List<CompanyRatingZone> companyRatingZones = new List<CompanyRatingZone>();
            foreach (CompanyRatingZoneDTO item in companyRatingZonesDTO)
            {
                companyRatingZones.Add(CreateCompanyRatingZone(item));
            }

            return companyRatingZones;
        }
        #endregion
    }
}
