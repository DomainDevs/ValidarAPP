using Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.DAOs;
using Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider.Resources;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Vehicles.TPLCollectiveServices.EEProvider;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Vehicles.TPLCollectiveServices.EEProvider
{
    /// <summary>
    /// Servicio Colectivas Autos
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.Vehicles.TPLCollectiveServices.EEProvider.VehicleCollectiveServiceEEProviderCore" />
    /// <seealso cref="Sistran.Company.Application.Vehicles.TPLCollectiveServices.IVehicleCollectiveService" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ThirdPartyLiabilityCollectiveServiceEEProvider : ThirdPartyLiabilityCollectiveServiceEEProviderCore, IThirdPartyLiabilityCollectiveService
    {
        public ThirdPartyLiabilityCollectiveServiceEEProvider()
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
                CollectiveLoadThirdPartyLiabilityDAO collectiveLoadVehicleDAO = new CollectiveLoadThirdPartyLiabilityDAO();
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
                return new CollectiveLoadProcessThirdPartyLiabilityDAO().GetCollectiveEmissionRowById(id);

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
                CollectiveLoadProcessThirdPartyLiabilityDAO collectiveProcessDAO = new CollectiveLoadProcessThirdPartyLiabilityDAO();
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
                CollectiveLoadProcessThirdPartyLiabilityDAO collectiveProcessDAO = new CollectiveLoadProcessThirdPartyLiabilityDAO();
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
                CollectiveLoadThirdPartyLiabilityDAO collectiveLoadVehicleDAO = new CollectiveLoadThirdPartyLiabilityDAO();
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
                return new CollectiveLoadProcessThirdPartyLiabilityDAO().IssuanceCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingVehiclePolicies), ex);
            }
        }
        public void SaveTemporalTpl(string[] objectsToSave)
        {
            try
            {
                new CollectiveLoadThirdPartyLiabilityDAO().SaveTemporalTpl(objectsToSave);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingVehiclePolicies), ex);
            }
        }
    }
}