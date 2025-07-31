using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Core.Application.CancellationEndorsement.EEProvider;
using Sistran.Core.Application.FidelityEndorsementCancellationService;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.FidelityEndorsementCancellationService3GProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;

namespace Sistran.Core.Application.FidelityEndorsementCancellationService.EEProvider
{
    public class FidelityCancellationServiceEEProvider : CancellationEndorsementEEProvider, IFidelityCancellationService
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
                throw new BusinessException(ExceptionManager.GetMessage(ex, FidelityEndorsementCancellationService3GProvider.Resources.Errors.ErrorCreateTemporalCancellationFidelity), ex);
            }
        }
    }
}
