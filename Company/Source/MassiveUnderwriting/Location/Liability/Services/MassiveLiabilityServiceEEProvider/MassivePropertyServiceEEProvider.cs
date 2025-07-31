using System;
using System.ServiceModel;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.DAOs;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveLiabilityServiceEEProvider : Sistran.Core.Application.Location.MassiveLiabilityServices.EEProvider.MassiveLiabilityServiceEEProvider, IMassiveLiabilityService
    {
        public MassiveLiabilityServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Crear Cargue Masivo
        /// </summary>
        /// <param name="MassiveLoad">Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        public MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission)
        {
            try
            {
                MassiveLoadLiabilityDAO massiveLoadLiabilityDAO = new MassiveLoadLiabilityDAO();
                return massiveLoadLiabilityDAO.CreateMassiveEmission(massiveEmission);
            }
            catch (Exception ex)
            {
                massiveEmission.HasError = true;
                massiveEmission.ErrorDescription = ex.Message;
                return massiveEmission;
            }
        }

        /// <summary>
        /// Tarifar Cargue Masivo
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        public MassiveLoad QuotateMassiveLoad(int massiveLoadId)
        {
            try
            {
                MassiveLoadProcessLiabilityDAO massiveLoadProcessLiabilityDAO = new MassiveLoadProcessLiabilityDAO();
                return massiveLoadProcessLiabilityDAO.QuotateMassiveLoad(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorQuotateMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Crear cargue masivo de renovación
        /// </summary>
        /// <param name="massiveRenewal">Cargue Masivo</param>
        /// <returns>Cargue Masivo</returns>
        public MassiveRenewal CreateMassiveRenewal(MassiveRenewal massiveRenewal)
        {
            try
            {
                MassiveRenewalLiabilityDAO massiveRenewalLiabilityDAO = new MassiveRenewalLiabilityDAO();
                return massiveRenewalLiabilityDAO.CreateMassiveLoad(massiveRenewal);
            }
            catch (Exception ex)
            {
                massiveRenewal.HasError = true;
                massiveRenewal.ErrorDescription = ex.Message;
                return massiveRenewal;
            }
        }

        /// <summary>
        /// Tarifar Cargue Masivo de renovación
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue Masivo de renovación</param>
        /// <returns>Cargue Masivo</returns>
        public MassiveLoad QuotateMassiveRenewal(int massiveLoadId)
        {
            try
            {
                MassiveRenewalRowLiabilityDAO massiveRenewalRowLiabilityDAO = new MassiveRenewalRowLiabilityDAO();
                return massiveRenewalRowLiabilityDAO.QuotateMassiveRenewal(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorQuotateMassiveLoad), ex);
            }
        }

        public MassiveLoad IssuanceMassiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadLiabilityDAO massiveLiabilityRowDao = new MassiveLoadLiabilityDAO();
                return massiveLiabilityRowDao.IssuanceMassiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorIssuing + "(" + ex.Message + ")");
            }
        }

        public MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveRenewalLiabilityDAO massiveLiabilityRowDao = new MassiveRenewalLiabilityDAO();
                return massiveLiabilityRowDao.IssuanceRenewalMassiveEmission(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorIssuing + "(" + ex.Message + ")");
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
                MassiveLoadLiabilityDAO massiveLoadLiabilityDAO = new MassiveLoadLiabilityDAO();
                return massiveLoadLiabilityDAO.GenerateReportToMassiveLoad(massiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGenerateReportToMassiveLoad), ex);
            }
        }
    }
}