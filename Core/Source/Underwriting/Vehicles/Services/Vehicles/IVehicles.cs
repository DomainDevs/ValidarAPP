using Sistran.Core.Application.Vehicles.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;

namespace Sistran.Core.Application.Vehicles
{
    [ServiceContract]
    public interface IVehicles
    {
        /// <summary>
        /// Gets all licence plates.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<string> GetAllLicencePlates();

        /// <summary>
        /// Obtener lista de marcas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Make> GetMakes();
        
        /// <summary>
        /// Obtener el listado de modelos por marca
        /// </summary>
        /// <returns>Lista de modelos</returns>
        [OperationContract]
        List<Model> GetModelsByMakeId(int makeId);
       
        /// <summary>
        /// Obtener lista de modelos por marca y descripción
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <returns>Lista de modelos</returns>
        [OperationContract]
        Models.Model GetModelByMakeIdModelId(int makeId, int modelId);

        /// <summary>
        /// Obtener versiones por marca y modelo
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <returns>Lista de versiones</returns>
        [OperationContract]
        List<Models.Version> GetVersionsByMakeIdModelId(int makeId, int modelId);       

        /// <summary>
        /// Obtener lista de tipos de vehiculo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Type> GetTypes();
                
        /// <summary>
        /// Obtener lista de colores de vehiculo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Color> GetColors();
                
        /// <summary>
        /// Obtener lista de carrocerias
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Body> GetBodies();

        /// <summary>
        /// Obtener lista de carrocerias por tipo de vehiculo
        /// </summary>
        /// <param name="vehicleTypeId"></param>
        /// <returns></returns>
        [OperationContract] 
        List<Body> GetBodiesByVehicleTypeId(int vehicleTypeId);

         /// <summary>
         /// Obtener lista de combustible
         /// </summary>
         /// <returns></returns>
        [OperationContract]
        List<Fuel> GetFuels();

        /// <summary>
        /// Obtener lista de tipos de transmisión
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<TransmissionType> GetVehicleTransmissionType();

        /// <summary>
        /// Obtener lista de Tips de servicio por productoId
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ServiceType> GetServiceTypesByProductId(int productId);

        /// <summary>
        /// Obtener años del vehiculo por versione
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <param name="versionId">Id version</param>
        /// <returns></returns>
        [OperationContract]
        List<Year> GetYearsByMakeIdModelIdVersionId(int makeId, int modelId, int versionId);

        /// <summary>
        /// Obtiene marca por IdMarke
        /// </summary>
        /// <param name="vehicleMakeCd">Id marca</param>
        /// <returns>Marca</returns>
        [OperationContract]
        Models.Make GetMakeByVehicleMakeCd(int vehicleMakeCd);

        /// <summary>
        /// Obtener tipo por IdType
        /// </summary>
        /// <returns>Tipo de acuerdo al id</returns>
        [OperationContract]
        Models.Type GetTypesByVehicleTypeCd(int vehicleTypeCd);

        /// <summary>
        /// Obtener version por marca, modelo y version
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <param name="versionId">Id version</param>
        /// <returns>Version</returns>
        [OperationContract]
        Models.Version GetVersionByVersionIdModelIdMakeId(int versionId, int modelId, int makeId);

        // <summary>
        /// Obtener color por identificador 
        /// </summary>
        /// <returns>Lista de colores</returns>
        [OperationContract]
        Models.Color GetColorById(int id);

        /// <summary>
        /// Obtener Tipos de servicios
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.ServiceType> GetServiceTypes();

        /// <summary>
        /// Consulta si la placa, motor o chasis ya existe en una póliza
        /// </summary>
        /// <param name="licensePlate">Placa</param>
        /// <param name="engineNumber">Motor</param>
        /// <param name="chassisNumber">Chasis</param>
        /// <param name="productId">Id Producto</param>
        /// <returns>Mensaje</returns>
        [OperationContract]
        string ExistsRiskByLicensePlateEngineNumberChassisNumberProductId(string licensePlate, string engineNumber, string chassisNumber, int productId, int endorsementId,DateTime currentFrom);

        /// <summary>
        /// Consulta los datos basicos de las polizas que esten asociadas a la placa 
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <returns>poliza con datos basicos</returns>
        [OperationContract]
        List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByLicensePlate(string licensePlate);

        /// <summary>
        /// Crear un vehicleVersion
        /// </summary>
        /// <param name="vehicleVersion"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Version CreateVehicleVersion(Models.Version vehicleVersion);

        /// <summary>
        /// Actualiza vehicleVersion
        /// </summary>
        /// <param name="vehicleVersion"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Version UpdateVehicleVersion(Models.Version vehicleVersion);


        /// <summary>
        /// retorna una lista de vehicleVersions filtrada por descripción
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Version> GetVehicleVersionsByDescription(string description);

        /// <summary>
        /// Retorna una lista de vehicleVersions de acuerdo a los filtros proporcionados
        /// </summary>
        /// <param name="makeCode"></param>
        /// <param name="modelCode"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Version> GetVehicleVersionsByMakeModelVersion(int? makeCode, int? modelCode, string description);

        /// <summary>
        /// Elimina una vehicleVersion 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        [OperationContract]
        void DeleteVehicleVersion(int id, int makeId, int modelId);

        /// <summary>
        /// Obtener Precio del Vehículo
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Version</param>
        /// <param name="year">Año</param>
        /// <returns>Precio Vehículo</returns>
        [OperationContract]
        decimal GetPriceByMakeIdModelIdVersionId(int makeId, int modelId, int versionId, int year);    
		
		/// <summary>
        /// Obtener Lista de Fabricantes de Vehiculos por Descripcion
        /// </summary>
        /// <param name="makeDescription"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Make> GetVehicleMakesByDescription(string description);
		
        /// <summary>
        /// Obtiene Lista de Modelos de Vehiculos por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Model> GetVehicleModelsByDescription(string description);
    }
}
