using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.Vehicles.VehicleServices
{
    [ServiceContract]
    public interface IVehicleServiceCore : IVehicles
    {
        /// <summary>
        /// Obtener Usos
        /// </summary>
        /// <returns>Lista de Usos</returns>
        [OperationContract]
        List<Use> GetUses();

        /// <summary>
        /// Obtener Usos pór tipo de carroceria
        /// </summary>
        /// <param name="bodyId"></param>
        /// <returns></returns> 
        [OperationContract]
        List<Models.Use> GetUsesByBodyId(int bodyId);

        /// <summary>
        /// Obtener Accesorios
        /// </summary>
        /// <returns>Lista de Accesorios</returns>
        [OperationContract]
        List<Accessory> GetAccessories();


        /// <summary>
        /// Obtiene listado de las polizas asociados al individualId, y obtiene las caracteristicas del vehiculo de cada poliza
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetVehiclesCoreByIndividualId")]
        List<Models.Vehicle> GetVehiclesByIndividualId(int individualId);

        /// <summary>
        /// Metodo para devolver riesgos
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Vehicle> GetRiskVehiclesByEndorsementId(int endorsementId);

        /// <summary>
        /// Metodo para devolver riesgos
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Vehicle> GetRiskVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType);

        /// <summary>
        /// Obtener Vehículo por Placa
        /// </summary>
        /// <param name="licencePlate">Placa</param>
        /// <returns>Vehículo</returns>
        [OperationContract]
        Vehicle GetRiskVehicleByLicensePlate(string licencePlate);

        [OperationContract]
        List<Vehicle> GetRisksVehicleByInsuredId(int insuredId);

        [OperationContract]
        List<Vehicle> GetRisksVehicleByLicensePlate(string licensePlate);

        [OperationContract]
        Vehicle GetRiskVehicleByRiskId(int riskId);

        /// <summary>
        /// Obtener Tipo de Riesgo por placa
        /// </summary>
        /// <param name="description">
        /// <returns> Lista por placa </returns>
        [OperationContract]
        List<Vehicle> GetSelectRisksVehicleByLicensePlate(string licencePlate);
    }
}
