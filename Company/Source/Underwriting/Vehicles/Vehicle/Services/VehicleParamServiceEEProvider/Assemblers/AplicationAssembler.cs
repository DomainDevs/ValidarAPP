using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Vehicles.VehicleApplicationService.DTOs;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ModelServices.Models.VehicleParam;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;

namespace Sistran.Company.Application.Vehicles.VehicleApplicationService.EEProvider.Assemblers
{
    public class AplicationAssembler
    {
        public static List<SelectDTO> CreateMakeDTOs(List<Make> listMakeModel)
        {
            List<SelectDTO> ListMakesDTOs = new List<SelectDTO>();
            foreach (Make item in listMakeModel)
            {
                ListMakesDTOs.Add(CreateMakeDTO(item));
            }
            return ListMakesDTOs;
        }

        private static SelectDTO CreateMakeDTO(Make item)
        {
            var result = new SelectDTO();
            result.Id = item.Id;
            result.Description = item.Description;
            return result;
        }

        internal static List<SelectDTO> CreateModelDTOs(List<Model> listModelModel)
        {
            List<SelectDTO> ListModelsDTOs = new List<SelectDTO>();
            foreach (Model item in listModelModel)
            {
                ListModelsDTOs.Add(CreateModelDTO(item));
            }
            return ListModelsDTOs;
        }

        private static SelectDTO CreateModelDTO(Model item)
        {
            var result = new SelectDTO();
            result.Id = item.Id;
            result.Description = item.Description;
            return result;
        }

        public static List<SelectDTO> CreateCauseDTOs(List<CompanyNotInsurableCause> listCauseModel)
        {
            List<SelectDTO> ListCausesDTOs = new List<SelectDTO>();
            foreach (CompanyNotInsurableCause item in listCauseModel)
            {
                ListCausesDTOs.Add(CreateCauseDTO(item));
            }
            return ListCausesDTOs;
        }

        private static SelectDTO CreateCauseDTO(CompanyNotInsurableCause item)
        {
            var result = new SelectDTO();
            result.Id = item.Id;
            result.Description = item.Description;
            return result;
        }

        public static FasecoldaDTO CreateFasecoldaDTO(CompanyVehicle item)
        {
            var result = new FasecoldaDTO();
            result.FasecoldaCode = item.Fasecolda.Description;
            result.MakeId = item.Make.Id.ToString();
            result.ModelId = item.Model.Id.ToString();
            result.VersionId = item.Version.Id.ToString();
            return result;
        }

        public static List<ValidationPlateDTO> CreateValidationPlatesDTO(List<ValidationPlateServiceModel> listValidationPlate)
        {
            List<ValidationPlateDTO> ListlistValidationPlateDTOs = new List<ValidationPlateDTO>();
            foreach (ValidationPlateServiceModel item in listValidationPlate)
            {
                ListlistValidationPlateDTOs.Add(CreateValidationPlateDTO(item));
            }
            return ListlistValidationPlateDTOs;
        }
        public static ValidationPlateDTO CreateValidationPlateDTO(ValidationPlateServiceModel item)
        {
            var result = new ValidationPlateDTO();
            result.Id = item.Id;
            result.Chassis = item.Chassis;
            result.CodCause = item.CodCause;
            result.CodFasecolda = item.CodFasecolda;
            //result.CodMake = item.CodMake;
            //result.CodModel = item.CodModel;
            //result.CodVersion = item.CodVersion;
            result.IsEnabled = item.IsEnabled;
            result.Plate = item.Plate;
            result.Motor = item.Motor;
            result.Status = Enums.StatusTypeService.Original;            
            return result;
        }        

        public static List<SelectDTO> CreateVersionsDTOs(VersionsServiceModel versions)
        {
            List<SelectDTO> VersionDto = new List<SelectDTO>();
            foreach (VersionServiceModel mVersionServiceModel in versions.ListVersionServiceModel)
            {
                SelectDTO vmVersionViewModel = new SelectDTO();
                vmVersionViewModel.Description = mVersionServiceModel.Description;
                vmVersionViewModel.Id = mVersionServiceModel.Id;

                VersionDto.Add(vmVersionViewModel);
            }
            return VersionDto;
        }
        public static SelectDTO CreateVersionDTOs(Core.Application.Vehicles.Models.Version versions)
        {            
                var vmVersionViewModel = new SelectDTO();
                vmVersionViewModel.Description = versions.Description;
                vmVersionViewModel.Id = versions.Id;
                return vmVersionViewModel;
        }

        internal static List<SelectDTO> CreateFuelDTOs(List<Fuel> listFuelModel)
        {
            List<SelectDTO> ListFuelDTOs = new List<SelectDTO>();
            foreach (Fuel item in listFuelModel)
            {
                ListFuelDTOs.Add(CreateFuelDTO(item));
            }
            return ListFuelDTOs;
        }      

        private static SelectDTO CreateFuelDTO(Fuel item)
        {
            var result = new SelectDTO();
            result.Id = item.Id;
            result.Description = item.Description;
            return result;
        }
       

        internal static List<SelectDTO> CreateBodiesDTOs(List<Body> listBodiesModel)
        {
            List<SelectDTO> ListBodiesDTOs = new List<SelectDTO>();
            foreach (Body item in listBodiesModel)
            {
                ListBodiesDTOs.Add(CreateBodyDTO(item));
            }
            return ListBodiesDTOs;
        }
       

        private static SelectDTO CreateBodyDTO(Body item)
        {
            var result = new SelectDTO();
            result.Id = item.Id;
            result.Description = item.Description;
            return result;
        }

        internal static List<SelectDTO> CreateTypeDTOs(List<Core.Application.Vehicles.Models.Type> listTypesModel)
        {
            List<SelectDTO> ListTypeDTOs = new List<SelectDTO>();
            foreach (Core.Application.Vehicles.Models.Type item in listTypesModel)
            {
                ListTypeDTOs.Add(CreateTypeDTO(item));
            }
            return ListTypeDTOs;
        }
      

        private static SelectDTO CreateTypeDTO(Core.Application.Vehicles.Models.Type item)
        {
            var result = new SelectDTO();
            result.Id = item.Id;
            result.Description = item.Description;
            return result;
        }

        internal static List<SelectDTO> CreateCurrenciesDTOs(List<Currency> listCurrenciesModel)
        {
            List<SelectDTO> ListCurrenciesDTOs = new List<SelectDTO>();
            foreach (Currency item in listCurrenciesModel)
            {
                ListCurrenciesDTOs.Add(CreateCurrencyDTO(item));
            }
            return ListCurrenciesDTOs;
        }

        private static SelectDTO CreateCurrencyDTO(Currency item)
        {
            var result = new SelectDTO();
            result.Id = item.Id;
            result.Description = item.Description;
            return result;
        }

        internal static List<SelectDTO> CreateTransmissionTypeDTOs(List<Core.Application.Vehicles.Models.TransmissionType> listTransmissionTypeModel)
        {
            List<SelectDTO> ListTransmissionTypeDTOs = new List<SelectDTO>();
            foreach (Core.Application.Vehicles.Models.TransmissionType item in listTransmissionTypeModel)
            {
                ListTransmissionTypeDTOs.Add(CreateTransmissionTypeDTO(item));
            }
            return ListTransmissionTypeDTOs;
        }

        private static SelectDTO CreateTransmissionTypeDTO(Core.Application.Vehicles.Models.TransmissionType item)
        {
            var result = new SelectDTO();
            result.Id = item.Id;
            result.Description = item.Description;
            return result;
        }

        internal static VehicleVersionDTO CreateVehicleVersionDTO(Models.CompanyVersion vehicleVersionModel)
        {
            return new VehicleVersionDTO
            {
                Id = vehicleVersionModel.Id,
                VehicleMakeServiceQueryModel = vehicleVersionModel.Make.Id,
                VehicleModelServiceQueryModel = vehicleVersionModel.Model.Id,
                Description = vehicleVersionModel.Description,
                EngineQuantity = vehicleVersionModel.Engine.EngineCc ?? 0,
                HorsePower = vehicleVersionModel.Engine.Horsepower ?? 0,
                Weight = vehicleVersionModel.Weight ?? 0,
                TonsQuantity = vehicleVersionModel.TonsQuantity ?? 0,
                PassengerQuantity = vehicleVersionModel.PassengerQuantity,
                VehicleFuelServiceQueryModel = vehicleVersionModel.Fuel.Id,
                VehicleBodyServiceQueryModel = vehicleVersionModel.Body.Id,
                VehicleTypeServiceQueryModel = vehicleVersionModel.Type.Id,
                VehicleTransmissionTypeServiceQueryModel = vehicleVersionModel.TransmissionType.Id,
                MaxSpeedQuantity = vehicleVersionModel.Engine.TopSpeed ?? 0,
                DoorQuantity = vehicleVersionModel.DoorQuantity ?? 0,
                Price = vehicleVersionModel.NewVehiclePrice ?? 0,
                IsImported = vehicleVersionModel.IsImported,
                LastModel = vehicleVersionModel.LastModel,
                Currency = vehicleVersionModel.Currency.Id,
                StatusTypeService = Enums.StatusTypeService.Original,
                IsElectronicPolicy = vehicleVersionModel.IsElectronicPolicy

            };
        }

        internal static List<VehicleVersionDTO> CreateVehicleVersionDTOs(List<Models.CompanyVersion> vehicleVersionsModel)
        {
            List<VehicleVersionDTO> listVehicleVersionDTOs = new List<VehicleVersionDTO>();
            foreach (var item in vehicleVersionsModel)
            {
                listVehicleVersionDTOs.Add(CreateVehicleVersionDTO(item));
            }
            return listVehicleVersionDTOs;
        }
    }
}
