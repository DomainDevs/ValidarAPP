using Sistran.Company.Application.UnderwritingServices.Models;
using System;

namespace Sistran.Company.Application.AircraftCancellationService.EEProvider.DAOs
{
    public class CiaCancellationDAO
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        public int CreateTemporalEndorsementCancellation(CompanyPolicy policy, int cancellationFactor, string userName)
        {
            var result = 0;
            return Convert.ToInt32(result);
        }
    }
}
