using Sistran.Company.Application.CancellationEndorsement.EEProvider;
using Sistran.Company.Application.JudicialSuretyCancellationService.EEProvider.Business;
using Sistran.Company.Application.JudicialSuretyCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using JSEM = Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;

namespace Sistran.Company.Application.JudicialSuretyCancellationService.EEProvider
{
    public class JudicialSuretyCancellationServiceEEProvider : CiaCancellationEndorsementEEProvider, IJudicialSuretyCancellationServiceCompany
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
                JudicialSuretyCancellationBusinessCompany judicialsuretyCancellationBusinessCompany = new JudicialSuretyCancellationBusinessCompany();
                return judicialsuretyCancellationBusinessCompany.CreateTemporal(companyEndorsement);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalCancellationSurety);
            }
        }

        public CompanyPolicy ExecuteThread(List<JSEM.CompanyJudgement> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                JudicialSuretyCancellationBusinessCompany judicialsuretyCancellationBusinessCompany = new JudicialSuretyCancellationBusinessCompany();
                return judicialsuretyCancellationBusinessCompany.ExecuteThread(risksThread, policy,risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationSurety);
            }
        }
    }
}
