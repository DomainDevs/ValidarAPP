using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Vehicles.VehicleServices.DTOs;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class VehicleServiceEEProviderCore : Vehicles.EEProvider.VehiclesEEProvider, IVehicleServiceCore
    {

        /// <summary>
        /// Obtener Usos
        /// </summary>
        /// <returns>Lista de Usos</returns>
        public List<Models.Use> GetUses()
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetUses();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener Usos pór tipo de carroceria 
        /// </summary>
        /// <param name="bodyId"></param>
        /// <returns></returns>
        public List<Models.Use> GetUsesByBodyId(int bodyId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetUsesByBodyId(bodyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Accesorios
        /// </summary>
        /// <returns>Lista de Accesorios</returns>
        public List<Models.Accessory> GetAccessories()
        {
            try
            {
                AccessoryDAO accessoryDAO = new AccessoryDAO();
                return accessoryDAO.GetAccessories();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

              /// <summary>
        /// Obtiene listado de las polizas asociados al individualId, y obtiene las caracteristicas del vehiculo de cada poliza
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<Models.Vehicle> GetVehiclesByIndividualId(int individualId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehiclesByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region Claims

        public List<Vehicle> GetRiskVehiclesByEndorsementId(int endorsementId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetRiskVehiclesByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<Vehicle> GetRiskVehiclesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();

                List<Vehicle> companyVehicles = vehicleDAO.GetRiskVehiclesByEndorsementIdModuleType(endorsementId, moduleType);

                return companyVehicles;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public Vehicle GetRiskVehicleByLicensePlate(string licencePlate)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetVehicleByLicensePlate(licencePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<Vehicle> GetRisksVehicleByInsuredId(int insuredId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetRisksVehicleByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Vehicle> GetRisksVehicleByLicensePlate(string licensePlate)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetRisksVehicleByLicensePlate(licensePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Vehicle GetRiskVehicleByRiskId(int riskId)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetRiskVehicleByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Vehicle> GetSelectRisksVehicleByLicensePlate(string licencePlate)
        {
            try
            {
                VehicleDAO vehicleDAO = new VehicleDAO();
                return vehicleDAO.GetSelectRisksVehicleByLicensePlate(licencePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion
    }
}
