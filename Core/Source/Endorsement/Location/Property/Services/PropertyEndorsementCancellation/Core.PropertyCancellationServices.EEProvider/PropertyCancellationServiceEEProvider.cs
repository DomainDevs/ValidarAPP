using Sistran.Core.Application.CancellationEndorsement.EEProvider;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.PropertyEndorsementCancellationService3GProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Core.Application.Utilities.Managers;

namespace Sistran.Core.Application.PropertyEndorsementCancellationService.EEProvider
{
    public class PropertyCancellationServiceEEProvider : CancellationEndorsementEEProvider, IPropertyCancellationService
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
                throw new BusinessException(ExceptionManager.GetMessage(ex, PropertyEndorsementCancellationService3GProvider.Resources.Errors.ErrorCreateTemporalCancellationProperty), ex);
            }
        }
    }
}
