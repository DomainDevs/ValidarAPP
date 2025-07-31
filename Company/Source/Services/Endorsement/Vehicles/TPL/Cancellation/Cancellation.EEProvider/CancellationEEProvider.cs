using Sistran.Company.Application.ThirdPartyLiabilityCancellationService.EEProvider.Business;
using Sistran.Company.Application.ThirdPartyLiabilityCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using TPLEM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;

namespace Sistran.Company.Application.ThirdPartyLiabilityCancellationService.EEProvider
{
    public class ThirdPartyLiabilityeCancellationServiceEEProvider : IThirdPartyLiabilityCancellationServiceCia
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
                ThirdPartyLiabilityCancellationBusinessCia tplCancellationBusinessCia = new ThirdPartyLiabilityCancellationBusinessCia();
                return tplCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalCancellationThirdPartyLiability);
            }
        }

        public CompanyPolicy ExecuteThread(List<TPLEM.CompanyTplRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                ThirdPartyLiabilityCancellationBusinessCia tplCancellationBusinessCia = new ThirdPartyLiabilityCancellationBusinessCia();
                return tplCancellationBusinessCia.ExecuteThread(risksThread, policy, risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationThirdPartyLiability);
            }
        }
    }
}
