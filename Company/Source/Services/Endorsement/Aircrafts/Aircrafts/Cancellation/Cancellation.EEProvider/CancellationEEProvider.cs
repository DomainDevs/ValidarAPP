using Sistran.Company.Application.CancellationEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.AircraftCancellationService.EEProvider.Business;
using Sistran.Company.Application.AircraftCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.AircraftCancellationService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;

namespace Sistran.Company.Application.AircraftCancellationService.EEProvider
{
    public class AircraftCancellationServiceEEProvider : CiaCancellationEndorsementEEProvider, ICiaAircraftCancellationService
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
                AircraftCancellationBusinessCia aircraftCancellationBusinessCia = new AircraftCancellationBusinessCia();
                return aircraftCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalCancellationAircraft);
            }
        }

        public CompanyPolicy ExecuteThread(List<CompanyAircraft> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                AircraftCancellationBusinessCia aircraftCancellationBusinessCia = new AircraftCancellationBusinessCia();
                return aircraftCancellationBusinessCia.ExecuteThread(risksThread, policy,risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationAircraft);
            }
        }
    }
}
