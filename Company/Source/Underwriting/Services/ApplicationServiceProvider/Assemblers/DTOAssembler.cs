using Sistran.Company.Application.UnderwritingApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ApplicationServiceProvider.Assemblers
{
    internal static class DTOAssembler
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
                IsElectronicPolicy = companyVehicleType.IsElectronicPolicy,
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
                SmallDescription = companyVehicleBody.SmallDescription,
                State = Convert.ToInt32(companyVehicleBody.State),
                VehicleUses = CreateVehicleUses(companyVehicleBody.VehicleUses)
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

        #region VehicleUse

        public static VehicleUseDTO CreateVehicleUse(CompanyVehicleUse companyVehicleUse)
        {
            return new VehicleUseDTO
            {
                Id = companyVehicleUse.Id,
                SmallDescription = companyVehicleUse.SmallDescription,
                PrefixId = companyVehicleUse.PrefixId
            };
        }

        public static List<VehicleUseDTO> CreateVehicleUses(List<CompanyVehicleUse> companyVehicleUses)
        {
            List<VehicleUseDTO> vehicleUses = new List<VehicleUseDTO>();

            foreach (CompanyVehicleUse item in companyVehicleUses)
            {
                vehicleUses.Add(CreateVehicleUse(item));
            }

            return vehicleUses;
        }

        #endregion

        #region CiaRatingZone
        public static CiaRatingZoneBranchDTO CreateCiaRatingZoneBranchDTO(CiaRatingZoneBranch companyCiaRatingZoneBranch)
        {
            return new CiaRatingZoneBranchDTO
            {
                BranchCode = companyCiaRatingZoneBranch.Branch.Id,
                RatingZone = companyCiaRatingZoneBranch.RatingZone.Id
            };
        }

        public static List<CiaRatingZoneBranchDTO> CreateCiaRatingZoneBranchDTOS(List<CiaRatingZoneBranch> companyCiaRatingZoneBranch)
        {
            List<CiaRatingZoneBranchDTO> ciaRatingZoneBranchDTO = new List<CiaRatingZoneBranchDTO>();

            foreach (CiaRatingZoneBranch companyCiaRatingZones in companyCiaRatingZoneBranch)
            {
                ciaRatingZoneBranchDTO.Add(CreateCiaRatingZoneBranchDTO(companyCiaRatingZones));
            }

            return ciaRatingZoneBranchDTO;
        }


        public static CompanyRatingZoneDTO CreateCompanyRatingZone(CompanyRatingZone ratingZone)
        {
            return new CompanyRatingZoneDTO
            {
                Id = ratingZone.Id,
                Description = ratingZone.Description,
                SmallDescription = ratingZone.SmallDescription,
                PrefixCode = ratingZone.Prefix.Id,
            };
        }

        public static List<CompanyRatingZoneDTO> CreateCompanyRatingZones(List<CompanyRatingZone> ratingZones)
        {
            List<CompanyRatingZoneDTO> companyRatingZones = new List<CompanyRatingZoneDTO>();
            foreach (CompanyRatingZone item in ratingZones)
            {
                companyRatingZones.Add(CreateCompanyRatingZone(item));
            }

            return companyRatingZones;
        }
        #endregion
    }
}
