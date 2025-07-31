using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.ServiceModel;
using Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.DAOs;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Company.Application.Location.MassivePropertyServices.EEProvider.Resources;
using System.Text;
using Sistran.Core.Framework.Logging;
using Sistran.Company.Application.Utilities.Helpers;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Company.Application.Location.MassivePropertyServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassivePropertyServiceEEProvider : Sistran.Core.Application.Location.MassivePropertyServices.EEProvider.MassivePropertyServiceEEProvider, IMassivePropertyService
    {
        public MassivePropertyServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Crear Cargue
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission)
        {
            try
            {
                MassiveLoadPropertyDAO massiveLoadPropertyDAO = new MassiveLoadPropertyDAO();
                return massiveLoadPropertyDAO.CreateMassiveEmission(massiveEmission);
            }
            catch (Exception ex)
            {
                massiveEmission.HasError = true;
                massiveEmission.ErrorDescription = ex.Message;
                return massiveEmission;
            }
        }

        /// <summary>
        /// Tarifar Cargue
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad QuotateMassiveLoad(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadProcessPropertyDAO massiveProcessPropertyDao = new MassiveLoadProcessPropertyDAO();
                return massiveProcessPropertyDao.QuotateMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateMassiveLoad), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="massiveRenewal"></param>
        /// <returns></returns>
        public MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            try
            {
                DAOs.MassiveRenewalPropertyDAO massiveRenewalPropertyDAO = new DAOs.MassiveRenewalPropertyDAO();
                return massiveRenewalPropertyDAO.CreateMassiveLoad(massiveRenewal);

            }
            catch (Exception ex)
            {
                massiveRenewal.HasError = true;
                massiveRenewal.ErrorDescription = ex.Message;
                return massiveRenewal;
            }
        }

        /// <summary>
        /// Genera el archivo de reporte del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoad">massiveLoad</param>
        /// <returns>string</returns>
        public string GenerateReportToMassiveLoad(MassiveEmission massiveEmission)
        {
            try
            {
                DAOs.MassiveLoadPropertyDAO massiveProcessPropertyDao = new DAOs.MassiveLoadPropertyDAO();
                return massiveProcessPropertyDao.GenerateReportToMassiveLoad(massiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGenerateReportToMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Ejecuta la tarifación de un cargue masivo de renovación del ramo multiriesgo (Hogar)
        /// </summary>
        /// <param name="massiveRenewalId"></param>
        /// <returns></returns>
        public MassiveLoad QuotateMassiveRenewal(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveRenewalRowPropertyDAO renewalRowPropertyDao = new MassiveRenewalRowPropertyDAO();
                return renewalRowPropertyDao.QuotateMassiveRenewal(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorQuotateMassiveRenewal), ex);
            }
        }

        public string GenerateReportToMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            try
            {
                var massiveRenewalPropertyDao = new DAOs.MassiveRenewalPropertyDAO();
                return massiveRenewalPropertyDao.GenerateReportToMassiveRenewal(massiveRenewal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGenerateReportToMassiveRenewal), ex);
            }
        }

        public MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                var massivePropertyRowDao = new DAOs.MassiveLoadPropertyDAO();
                return massivePropertyRowDao.IssuanceMassiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuing), ex);
            }
        }

        public MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveRenewalRowPropertyDAO massiveRenewalRowPropertyDAO = new MassiveRenewalRowPropertyDAO();
                return massiveRenewalRowPropertyDAO.IssuanceRenewalMassiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuing), ex);
            }
        }

        public int FindExternalServicesLoad(int massiveLoadId)
        {
            try
            {
                MassiveLoad massiveLoad = DelegateService.massiveService.GetMassiveLoadByMassiveLoadId(massiveLoadId);
                massiveLoad.Status = Core.Application.MassiveServices.Enums.MassiveLoadStatus.Validating;
                DelegateService.massiveService.UpdateMassiveLoad(massiveLoad);
                return DelegateService.externalProxyService.FindExternalServicesLoad(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetExternalServices), ex);
            }
        }

        public void ProcessResponseFromScoreService(string response)
        {
            try
            {
                MassiveLoadPropertyDAO massiveLoadPropertyDAO = new MassiveLoadPropertyDAO();
                massiveLoadPropertyDAO.ProcessResponseFromScoreService(response);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorResponseScoreService), ex);
            }
        }

    }
}
