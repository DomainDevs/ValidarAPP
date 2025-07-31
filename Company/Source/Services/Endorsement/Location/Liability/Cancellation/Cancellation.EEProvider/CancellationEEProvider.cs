using Sistran.Company.Application.LiabilityCancellationService.EEProvider.Business;
using Sistran.Company.Application.LiabilityCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;


namespace Sistran.Company.Application.LiabilityCancellationService.EEProvider
{
    public class LiabilityCancellationServiceEEProvider : ILiabilityCancellationServiceCia
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        public CompanyPolicy CreateTemporalEndorsementCancellation(CompanyEndorsement companyEndorsement)
        {
            try
            {
                LiabilityCancellationBusinessCia liabilityCancellationBusinessCia = new LiabilityCancellationBusinessCia();
                return liabilityCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalCancellationLibiality);
            }
        }

        public CompanyPolicy ExecuteThread(List<LEM.CompanyLiabilityRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                LiabilityCancellationBusinessCia liabilityCancellationBusinessCia = new LiabilityCancellationBusinessCia();
                return liabilityCancellationBusinessCia.ExecuteThread(risksThread, policy, risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationLibiality);
            }
        }
    }
}
