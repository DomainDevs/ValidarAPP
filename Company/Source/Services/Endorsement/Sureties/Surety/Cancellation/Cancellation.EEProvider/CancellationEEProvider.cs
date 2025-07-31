using Sistran.Company.Application.CancellationEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyCancellationService.EEProvider.Business;
using Sistran.Company.Application.SuretyCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.SuretyCancellationService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.SuretyCancellationService.EEProvider
{
    public class SuretyCancellationServiceEEProvider : CiaCancellationEndorsementEEProvider, ISuretyCancellationServiceCia
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
                SuretyCancellationBusinessCia suretyCancellationBusinessCia = new SuretyCancellationBusinessCia();
                return suretyCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy ExecuteThread(List<SEM.CompanyContract> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                SuretyCancellationBusinessCia suretyCancellationBusinessCia = new SuretyCancellationBusinessCia();
                return suretyCancellationBusinessCia.ExecuteThread(risksThread, policy,risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationSurety);
            }
        }
    }
}
