using Sistran.Core.Application.CancellationEndorsement.EEProvider;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.VehicleEndorsementCancellationService3GProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;

namespace Sistran.Core.Application.TransportEndorsementCancellationService.EEProvider
{
    public class TransportCancellationServiceEEProvider : CancellationEndorsementEEProvider, ITransportCancellationService
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
                throw new BusinessException(ExceptionManager.GetMessage(ex, VehicleEndorsementCancellationService3GProvider.Resources.Errors.ErrorCreateTemporalCancellationVehicle), ex);
            }
        }
    }
}
