using Sistran.Company.Application.CancellationEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.TransportCancellationService.EEProvider.Business;
using Sistran.Company.Application.TransportCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.TransportCancellationService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;

namespace Sistran.Company.Application.TransportCancellationService.EEProvider
{
    public class TransportCancellationServiceEEProvider : CiaCancellationEndorsementEEProvider, ICiaTransportCancellationService
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
                TransportCancellationBusinessCia transportCancellationBusinessCia = new TransportCancellationBusinessCia();
                return transportCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalCancellationTransport);
            }
        }

        public CompanyPolicy ExecuteThread(List<CompanyTransport> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                TransportCancellationBusinessCia transportCancellationBusinessCia = new TransportCancellationBusinessCia();
                return transportCancellationBusinessCia.ExecuteThread(risksThread, policy,risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationTransport);
            }
        }
    }
}
