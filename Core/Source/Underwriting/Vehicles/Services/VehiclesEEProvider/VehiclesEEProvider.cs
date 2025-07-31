using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.Vehicles.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Vehicles.Models;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;


namespace Sistran.Core.Application.Vehicles.EEProvider
{
    /// <summary>
    /// Proveedo autos
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.Vehicles.IVehicles" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class VehiclesEEProvider : IVehicles
    {

        public List<string> GetAllLicencePlates()
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetAllLicencePlates();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener lista de marcas
        /// </summary>
        /// <returns></returns>
        public List<Models.Make> GetMakes()
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetMakes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos
        /// </summary>
        /// <returns></returns>
        public List<Models.Type> GetTypes()
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de colores
        /// </summary>
        /// <returns></returns>
        public List<Models.Color> GetColors()
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetColors();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de carrocerias
        /// </summary>
        /// <returns></returns>
        public List<Models.Body> GetBodies()
        {
            try
            {
                BodyDAO bodyDAO = new BodyDAO();
                return bodyDAO.GetBodies();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de carrocerias por tipo de vehiculo
        /// </summary>
        /// <param name="vehicleTypeId"></param>
        /// <returns></returns> 
        public List<Models.Body> GetBodiesByVehicleTypeId(int vehicleTypeId)
        {
            try
            {
                BodyDAO bodyDAO = new BodyDAO();
                return bodyDAO.GetBodiesByVehicleTypeId(vehicleTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de combustible
        /// </summary>
        /// <returns></returns>
        public List<Models.Fuel> GetFuels()
        {
            try
            {
                FuelDAO vehicleFuelDAO = new FuelDAO();
                return vehicleFuelDAO.GetFuels();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener lista de Tips de servicio por productoId
        /// </summary>
        /// <param name="productId">Identificador de producto para filtro</param>
        /// <returns></returns>
        public List<Models.ServiceType> GetServiceTypesByProductId(int productId)
        {
            try
            {
                return ServiceTypeDAO.GetServiceTypesByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener el listado de modelos por marca
        /// </summary>
        /// <returns>Lista de modelos</returns>
        public List<Models.Model> GetModelsByMakeId(int makeId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetModelsByMakeId(makeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de modelos por marca y descripción
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <returns>Lista de modelos</returns>
        public Models.Model GetModelByMakeIdModelId(int makeId, int modelId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetModelByMakeIdModelId(makeId, modelId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener versiones por marca y modelo
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <returns>Lista de versiones</returns>
        public List<Models.Version> GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVersionsByMakeIdModelId(makeId, modelId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener años del vehiculo por versión, marca y modelo
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <param name="versionId">Id version</param>
        /// <returns></returns>
        public List<Vehicles.Models.Year> GetYearsByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene marca por IdMarke
        /// </summary>
        /// <returns></returns>
        public Models.Make GetMakeByVehicleMakeCd(int vehicleMakeCd)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetMakeByVehicleMakeCd(vehicleMakeCd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener tipo por IdType
        /// </summary>
        /// <returns></returns>
        public Models.Type GetTypesByVehicleTypeCd(int vehicleTypeCd)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetTypesByVehicleTypeCd(vehicleTypeCd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener version por marca, modelo y version
        /// </summary>
        /// <param name="makeId">Id marca</param>
        /// <param name="modelId">Id modelo</param>
        /// <param name="versionId">Id version</param>
        /// <returns>Version</returns>
        public Models.Version GetVersionByVersionIdModelIdMakeId(int versionId, int modelId, int makeId)
        {
            try
            {
                VehicleDAO vehicle = new VehicleDAO();
                return vehicle.GetVersionByVersionIdModelIdMakeId(versionId, modelId, makeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        // <summary>
        /// Obtener color por identificador 
        /// </summary>
        /// <returns>Lista de colores</returns>
        public Models.Color GetColorById(int id)
        {
            try
            {
                VehicleDAO vehicle = new VehicleDAO();
                return vehicle.GetColorById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<ServiceType> GetServiceTypes()
        {
            try
            {
                ServiceTypeDAO serviceTypeDAO = new ServiceTypeDAO();
                return serviceTypeDAO.GetServiceTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Consulta si la placa, motor o chasis ya existe en una póliza
        /// </summary>
        /// <param name="licensePlate">Placa</param>
        /// <param name="engineNumber">Motor</param>
        /// <param name="chassisNumber">Chasis</param>
        /// <param name="productId">Id Producto</param>
        /// <returns>Mensaje</returns>
        public string ExistsRiskByLicensePlateEngineNumberChassisNumberProductId(string licensePlate, string engineNumber, string chassisNumber, int productId, int endorsementId, DateTime currentFrom)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.ExistsRiskByLicensePlateEngineNumberChassisNumberProductId(licensePlate, engineNumber, chassisNumber, productId, endorsementId, currentFrom);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta los datos basicos de las polizas que esten asociadas a la placa 
        /// </summary>
        /// <param name="licensePlate"></param>
        /// <returns>poliza con datos basicos</returns>
        public List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByLicensePlate(string licensePlate)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetPolicyRiskDTOsByLicensePlate(licensePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Obtener lista de tipos de transmición
        /// </summary>
        /// <returns></returns>
        public List<TransmissionType> GetVehicleTransmissionType()
        {
            try
            {
                TransmissionTypeDAO transmissionTypeDAO = new TransmissionTypeDAO();
                return transmissionTypeDAO.GetVehicleTransmissionType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Lista de Fabricantes de Vehiculos por Descripcion
        /// </summary>
        /// <param name="makeDescription"></param>
        /// <returns></returns>
        public List<Models.Make> GetVehicleMakesByDescription(string description)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleMakesByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener Lista de Modelos de Vehiculos por Descripcion
        /// </summary>
        /// <param name="makeDescription"></param>
        /// <returns></returns>
        public List<Models.Model> GetVehicleModelsByDescription(string description)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleModelsByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crear VehicleVersion
        /// </summary>
        /// <param name="vehicleVersion"></param>
        /// <returns></returns>
        public Models.Version CreateVehicleVersion(Models.Version vehicleVersion)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.CreateVersion(vehicleVersion);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza vehicleVersion
        /// </summary>
        /// <param name="vehicleVersion"></param>
        /// <returns></returns>
        public Models.Version UpdateVehicleVersion(Models.Version vehicleVersion)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.UpdateVersion(vehicleVersion);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene una lista de vehicleVersion filtrada por descripción
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<Models.Version> GetVehicleVersionsByDescription(string description)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleVersionByDescription(description);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene una lista de vehicleVersion segun los filtros proporcionados
        /// </summary>
        /// <param name="makeCode"></param>
        /// <param name="modelCode"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<Models.Version> GetVehicleVersionsByMakeModelVersion(int? makeCode, int? modelCode, string description)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleVersionByMakeIdModelId(makeCode, modelCode, description);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un vehicleVersion 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        public void DeleteVehicleVersion(int id, int makeId, int modelId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                vehicleDAO.DeleteVehicleVersion(id, makeId, modelId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Obtener Precio del Vehículo
        /// </summary>
        /// <param name="makeId">Id Marca</param>
        /// <param name="modelId">Id Modelo</param>
        /// <param name="versionId">Id Version</param>
        /// <param name="year">Año</param>
        /// <returns>Precio Vehículo</returns>
        public decimal GetPriceByMakeIdModelIdVersionId(int makeId, int modelId, int versionId, int year)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetPriceByMakeIdModelIdVersionId(makeId, modelId, versionId, year);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}