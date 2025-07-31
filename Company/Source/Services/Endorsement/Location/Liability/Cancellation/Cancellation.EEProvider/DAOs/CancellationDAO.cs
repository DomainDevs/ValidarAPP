using Sistran.Core.Application.UnderwritingServices.Models;
using System;

namespace Sistran.Core.Application.LiabilityEndorsementCancellationService3GProvider.DAOs
{
    public class CancellationDAO
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        public int CreateTemporalEndorsementCancellation(Policy policy, int cancellationFactor, string userName)
        {
            var result = 0;
            return Convert.ToInt32(result);
        }
    }
}
