using Sistran.Core.Application.CancellationEndorsement.EEProvider;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.ThirdPartyLiabilityEndorsementCancellationService3GProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Core.Application.Utilities.Managers;

namespace Sistran.Core.Application.ThirdPartyLiabilityEndorsementCancellationService.EEProvider
{
    public class ThirdPartyLiabilityCancellationServiceEEProvider : CancellationEndorsementEEProvider, IThirdPartyLiabilityCancellationService
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
                throw new BusinessException(ExceptionManager.GetMessage(ex, ThirdPartyLiabilityEndorsementCancellationService3GProvider.Resources.Errors.ErrorCreateTemporalCancellationThirdPartyLiability), ex);
            }
        }
    }
}
