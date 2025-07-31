
using Sistran.Company.Application.Vehicles.VehicleApplicationService.DTOs;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;

namespace Sistran.Company.Application.Vehicles.VehicleApplicationService
{
    [ServiceContract]
    public interface ICompanyVehicleApplicationService
    {
        #region VehicleVersion

        [OperationContract]
        List<SelectDTO> GetVehiclesMake();
        [OperationContract]
        List<SelectDTO> GetModelsByMake(int makeID);
        [OperationContract]
        List<SelectDTO> GetFuelsType();
        [OperationContract]
        List<SelectDTO> GetBodies();
        [OperationContract]
        List<SelectDTO> GetVehicleNotInsuredCauses();
        [OperationContract]
        List<ValidationPlateDTO> GetValidationPlate();
        /// <summary>
        /// Obtener Vehículo por Marca, Modelo y Versión
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Versión</param>
        /// <returns>Vehículo</returns>
        [OperationContract]
        FasecoldaDTO GetFasecoldaCodeByMakeIdModelIdVersionId(int makeId, int modelId, int versionId);
        [OperationContract]
        FasecoldaDTO GetVehicleByFasecoldaCode(string code, int year);
        [OperationContract]        
        List<SelectDTO> GetTypesVehicle();
        [OperationContract]
        List<SelectDTO> GetTransmissionsType();
        [OperationContract]
        List<SelectDTO> GetCurrencies();
        [OperationContract]
        List<SelectDTO> GetVersionByMakeIdModelId(int makeId, int modelId);
        [OperationContract]
        VehicleVersionDTO CreateVehicleVersion(VehicleVersionDTO vehicleVersionDTO);
        [OperationContract]
        VehicleVersionDTO UpdateVehicleVersion(VehicleVersionDTO vehicleVersionDTO);

        [OperationContract]
        List<VehicleVersionDTO> GetVehicleVersionsByDescription(string description);

        [OperationContract]
        List<VehicleVersionDTO> GetAdvanzedSearchVehiclesVersion(int? makeCode, int? modelCode, string description);

        [OperationContract]
        void DeleteVehicleVersion(int id, int makeId, int modelId);
        #endregion
    }
}
