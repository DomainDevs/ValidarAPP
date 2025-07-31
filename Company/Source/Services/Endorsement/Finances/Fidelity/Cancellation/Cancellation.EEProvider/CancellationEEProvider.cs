using Sistran.Company.Application.FidelityCancellationService.EEProvider.Business;
using Sistran.Company.Application.FidelityCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using LEM = Sistran.Company.Application.Finances.FidelityServices.Models;


namespace Sistran.Company.Application.FidelityCancellationService.EEProvider
{
    public class FidelityCancellationServiceEEProvider : IFidelityCancellationServiceCia
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
                FidelityCancellationBusinessCia fidelityCancellationBusinessCia = new FidelityCancellationBusinessCia();
                return fidelityCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalCancellationFidelity);
            }
        }

        public CompanyPolicy ExecuteThread(List<LEM.CompanyFidelityRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                FidelityCancellationBusinessCia fidelityCancellationBusinessCia = new FidelityCancellationBusinessCia();
                return fidelityCancellationBusinessCia.ExecuteThread(risksThread, policy, risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationFidelity);
            }
        }
    }
}
