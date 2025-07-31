using Sistran.Core.Application.CancellationEndorsement.EEProvider;
using Sistran.Core.Application.SuretyEndorsementCancellationService3GProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Core.Application.Utilities.Managers;

namespace Sistran.Core.Application.SuretyEndorsementCancellationService.EEProvider
{
    public class SuretyCancellationServiceEEProvider : CancellationEndorsementEEProvider, ISuretyCancellationService
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        public int CreateTemporalEndorsementCancellation(Policy policy, int cancellationFactor, string userName)
        {
            try
            {
                CancellationDAO cancellationDAO = new CancellationDAO();
                return cancellationDAO.CreateTemporalEndorsementCancellation(policy, cancellationFactor, userName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, SuretyEndorsementCancellationService3GProvider.Resources.Errors.ErrorCreateTemporalCancellationSurety), ex);
            }
        }

        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        public List<Risk> QuotateCancellationSurety(Policy policy, int cancellationFactor)
        {
            try
            {
                CancellationDAO cancellationDAO = new CancellationDAO();
                return cancellationDAO.QuotateCancellationSurety(policy, cancellationFactor);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, SuretyEndorsementCancellationService3GProvider.Resources.Errors.ErrorQuotateCancellationSurety), ex);
            }   
        }
    }
}
