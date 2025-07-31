using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Integration.VehicleServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEL=Sistran.Core.Application.Vehicles.Models;

namespace Sistran.Core.Integration.VehicleService.EEProvider.Assemblers
{
   public static class DTOAssembler
    {
        internal static MakeDTO CreateVehicleMake(Make vehicleMake)
        {
            return new MakeDTO
            {
                Id = vehicleMake.Id,
                Description = vehicleMake.Description
            };
        }

        internal static List<MakeDTO> CreateVehicleMakes(List<Make> vehicleMakes)
        {
            List<MakeDTO> vehicleMakesDTO = new List<MakeDTO>();

            foreach (Make vehicleMake in vehicleMakes)
            {
                vehicleMakesDTO.Add(CreateVehicleMake(vehicleMake));
            }

            return vehicleMakesDTO;
        }

        internal static ModelDTO CreateVehicleModel(Model vehicleModel)
        {
            return new ModelDTO
            {
                Id = vehicleModel.Id,
                Description = vehicleModel.Description
            };
        }

        internal static List<ModelDTO> CreateVehicleModels(List<Model> vehicleModels)
        {
            List<ModelDTO> vehicleModelDTO = new List<ModelDTO>();

            foreach (Model vehicleModel in vehicleModels)
            {
                vehicleModelDTO.Add(CreateVehicleModel(vehicleModel));
            }

            return vehicleModelDTO;
        }

        internal static ColorDTO CreateVehicleColor(Color vehicleColor)
        {
            return new ColorDTO
            {
                Id = vehicleColor.Id,
                Description = vehicleColor.Description
            };
        }

        internal static List<ColorDTO> CreateVehicleColors(List<Color> vehicleColors)
        {
            List<ColorDTO> vehicleColorDTO = new List<ColorDTO>();

            foreach (Color vehicleColor in vehicleColors)
            {
                vehicleColorDTO.Add(CreateVehicleColor(vehicleColor));
            }

            return vehicleColorDTO;
        }

        internal static List<YearDTO> CreateVehicleYears(List<Year> years)
        {
            List<YearDTO> selectDTO = new List<YearDTO>();

            foreach (Year year in years)
            {
                selectDTO.Add(CreateVehicleYear(year));
            }

            return selectDTO.OrderByDescending(x => x.Description).ToList();
        }
        internal static YearDTO CreateVehicleYear(Year year)
        {
            return new YearDTO
            {
                Id = year.Description,
                Description = year.Description.ToString()
            };
        }

        internal static VersionDTO CreateVersion(VEL.Version version)
        {
            return new VersionDTO
            {
                Id = version.Id,
                Description = version.Description
            };
        }

        internal static List<VersionDTO> CreateVersions(List<VEL.Version> versions)
        {
            List<VersionDTO> selectDTO = new List<VersionDTO>();

            foreach (VEL.Version version in versions)
            {
                selectDTO.Add(CreateVersion(version));
            }
            return selectDTO;
        }

        internal static List<VehicleDTO> CreateVehicles(List<Vehicle> Vehicles)
        {
            List<VehicleDTO> vehiclesDTO = new List<VehicleDTO>();

            foreach (Vehicle vehicle in Vehicles)
            {
                vehiclesDTO.Add(CreateVehicle(vehicle));
            }

            return vehiclesDTO;
        }

        internal static VehicleDTO CreateVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return null;
            }

            return new VehicleDTO
            {
                RiskId = vehicle.Risk.RiskId,
                RiskNumber = vehicle.Risk.Number,
                Plate = vehicle.LicensePlate,
                Year = vehicle.Year,
                Make = vehicle.Make.Description,
                MakeId = vehicle.Make.Id,
                Model = vehicle.Model.Description,
                ModelId = vehicle.Model.Id,
                Color = vehicle.Color.Description,
                ColorId = vehicle.Color.Id,
                InsuredAmount = vehicle.Risk.AmountInsured,
                Chasis = vehicle.ChassisSerial,
                Motor = vehicle.EngineSerial,
                NumberBeneficiarie = Convert.ToInt32(vehicle.Risk.Beneficiaries?.FirstOrDefault()?.IdentificationDocument?.Number),
                NameBeneficiarie = vehicle.Risk.Beneficiaries?.FirstOrDefault()?.Name,
                ParticipationBeneficiarie = Convert.ToInt32(vehicle.Risk.Beneficiaries?.FirstOrDefault()?.Participation),
                InsuredDocumentNum = vehicle.Risk.MainInsured?.IdentificationDocument?.Number,
                InsuredName = vehicle.Risk.MainInsured?.Name,
                VersionId = vehicle.Version.Id,
                DocumentNumber = Convert.ToInt32(vehicle.Risk.Policy?.DocumentNumber),
                EndorsementId = Convert.ToInt32(vehicle.Risk.Policy?.Endorsement.Id),
                CoveredRiskType = Convert.ToInt32(vehicle.Risk.CoveredRiskType),
                IndividualId = Convert.ToInt32(vehicle.Risk?.MainInsured?.IndividualId),
            };
        }
    }
}
