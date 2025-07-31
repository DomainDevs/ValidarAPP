using Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.DAOs;
using Sistran.Core.Application.Vehicles.VehicleCollectiveServices.EEProvider;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.Resources;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using CollectiveEmission = Sistran.Core.Application.CollectiveServices.Models.CollectiveEmission;
using Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.CollectiveServices.Models;

namespace Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider
{
    /// <summary>
    /// Servicio Colectivas Autos
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.Vehicles.VehicleCollectiveServices.EEProvider.VehicleCollectiveServiceEEProviderCore" />
    /// <seealso cref="Sistran.Company.Application.Vehicles.VehicleCollectiveServices.IVehicleCollectiveService" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class VehicleCollectiveServiceEEProvider : VehicleCollectiveServiceEEProviderCore, IVehicleCollectiveService
    {
        public VehicleCollectiveServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Crear Cargue Colectivas
        /// </summary>
        /// <param name="collectiveLoad">The collective load.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CollectiveEmission CreateCollectiveLoad(CollectiveEmission collectiveLoad)
        {
            try
            {
                CollectiveLoadVehicleDAO collectiveLoadVehicleDAO = new CollectiveLoadVehicleDAO();
                return collectiveLoadVehicleDAO.CreateCollectiveLoad(collectiveLoad);
            }
            catch (Exception ex)
            {
                collectiveLoad.HasError = true;
                collectiveLoad.ErrorDescription = ex.Message;
                return collectiveLoad;
            }
        }


        /// <summary>
        /// Consulta EmisionRow
        /// </summary>
        /// <param name="id">int id </param>
        public CollectiveEmissionRow GetCollectiveEmissionRowById(int id)
        {

            try
            {
                return new CollectiveLoadProcessVehicleDAO().GetCollectiveEmissionRowById(id);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingVehiclePolicies), ex);
            }
        }




        /// <summary>
        /// Quotates the collective load.
        /// </summary>
        /// <param name="collectiveLoadIds">The collective load ids.</param>
        /// <exception cref="BusinessException"></exception>
        public void QuotateCollectiveLoad(List<int> collectiveLoadIds)
        {
            try
            {
                CollectiveLoadProcessVehicleDAO collectiveProcessDAO = new CollectiveLoadProcessVehicleDAO();
                collectiveProcessDAO.QuotateCollectiveLoad(collectiveLoadIds);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorInQuotateCollectiveLoad), ex);
            }
        }

        /// <summary>
        /// Quotates the massive collective emission.
        /// </summary>
        /// <param name="collectiveLoadIds">The collective load ids.</param>
        /// <exception cref="BusinessException"></exception>
        public void QuotateMassiveCollectiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                CollectiveLoadProcessVehicleDAO collectiveProcessDAO = new CollectiveLoadProcessVehicleDAO();
                collectiveProcessDAO.QuotateMassiveCollectiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorInQuotateCollectiveLoad), ex);
            }
        }

        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="collectiveEmission">massiveLoad</param>
        /// <param name="endorsementType"></param>
        /// <returns>string</returns>
        public string GenerateReportToCollectiveLoad(CollectiveEmission collectiveEmission, int endorsementType)
        {
            try
            {
                CollectiveLoadVehicleDAO collectiveLoadVehicleDAO = new CollectiveLoadVehicleDAO();
                return collectiveLoadVehicleDAO.GenerateReportToCollectiveLoad(collectiveEmission, endorsementType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGeneratingReport), ex);
            }
        }
        
        public MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveEmission)
        {
            try
            {
                return new CollectiveLoadProcessVehicleDAO().IssuanceCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingVehiclePolicies), ex);
            }
        }

        public void SaveTemporalVehicle(string[] objectsToSave)

        {
            try
            {
                new CollectiveLoadVehicleDAO().SaveTemporalVehicle(objectsToSave);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingVehiclePolicies), ex);
            }
        }
    }
}