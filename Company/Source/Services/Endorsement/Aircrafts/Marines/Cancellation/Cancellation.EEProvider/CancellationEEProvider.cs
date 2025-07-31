using Sistran.Company.Application.CancellationEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.MarineCancellationService.EEProvider.Business;
using Sistran.Company.Application.MarineCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.MarineCancellationService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;

namespace Sistran.Company.Application.MarineCancellationService.EEProvider
{
    public class MarineCancellationServiceEEProvider : CiaCancellationEndorsementEEProvider, ICiaMarineCancellationService
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
                MarineCancellationBusinessCia marineCancellationBusinessCia = new MarineCancellationBusinessCia();
                return marineCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalCancellationMarine);
            }
        }

        public CompanyPolicy ExecuteThread(List<CompanyMarine> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                MarineCancellationBusinessCia marineCancellationBusinessCia = new MarineCancellationBusinessCia();
                return marineCancellationBusinessCia.ExecuteThread(risksThread, policy,risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationMarine);
            }
        }
    }
}
