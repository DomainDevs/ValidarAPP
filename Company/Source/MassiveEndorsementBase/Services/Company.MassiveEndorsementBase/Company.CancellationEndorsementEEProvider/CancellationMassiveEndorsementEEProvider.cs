using Sistran.Company.Application.CancellationMsvEndorsementServices;
using Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider.DAOs;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.BAF;
using System.Collections.Generic;
using System.ServiceModel;

using Sistran.Company.Application.CancellationMsvEndorsementServices.EEProvider.Resources;

using Sistran.Company.Application.CancellationMsvEndorsementServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider
{
    /// <summary>
    /// Provider Cancelacion Masiva base
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.CancellationMsvEndorsementServices.ICancellationMassiveEndorsementServices" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CancellationMassiveEndorsementEEProvider : ICancellationMassiveEndorsementServices
    {
        public CancellationMassiveEndorsementEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateCancellation(CompanyPolicy policy, int cancellationFactor)
        {
            try
            {
                CancellationMassiveDAO cancellationDAO = new CancellationMassiveDAO();
                return cancellationDAO.QuotateCancellation(policy, cancellationFactor);

            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Cancelacion masiva por numero de cargue
        /// </summary>
        /// <param name="massiveLoadId">Identificador del cargue</param>
        /// <returns></returns>
        public MassiveLoad CancellationMassiveByMassiveLoadId(MassiveLoad massiveLoad)
         {
            try
            {
                CancellationMassiveDAO cancellationMassiveDAO = new CancellationMassiveDAO();
                return cancellationMassiveDAO.CancellationMassiveByMassiveLoadId(massiveLoad);

            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Creacion Carge masivo
        /// </summary>
        /// <param name="massiveLoad">The massive load.</param>
        /// <returns></returns>
        public MassiveLoad CreateMassiveLoad(MassiveLoad massiveLoad)
        {
            try
            {
                CancellationMassiveDAO cancellationDAO = new CancellationMassiveDAO();
                return cancellationDAO.CreateMassiveLoad(massiveLoad);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Emision Cancelacion Masiva
        /// </summary>
        /// <param name="massiveLoadId">identificador del Cargue masiva</param>
        /// <returns></returns>
        public MassiveLoad CreateIssuePolicy(MassiveLoad massiveLoad)
        {
            try
            {
                CancellationMassiveDAO cancellationDAO = new CancellationMassiveDAO();
                return cancellationDAO.CreateIssuePolicy(massiveLoad);

            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
        /// <summary>
        /// Generar Reporte de Cancelacion Masiva por Identificador y estado del Cargue
        /// </summary>
        /// <param name="MassiveLoadId">The massive load identifier.</param>
        /// <param name="massiveLoadStatus">The massive load status.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public string GenerateReportByMassiveLoadIdByMassiveLoadStatus(int MassiveLoadId, MassiveLoadStatus massiveLoadStatus)
        {
            try
            {
                CancellationMassiveDAO cancellationDAO = new CancellationMassiveDAO();
                return cancellationDAO.GenerateReportByMassiveLoadIdByMassiveLoadStatus(MassiveLoadId, massiveLoadStatus);

            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public string PrintCancellationMassive(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail)
        {
            try
            {
                CancellationMassiveDAO cancellationDAO = new CancellationMassiveDAO();
                return null;// cancellationDAO.PrintCancellationMassive(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);

            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
