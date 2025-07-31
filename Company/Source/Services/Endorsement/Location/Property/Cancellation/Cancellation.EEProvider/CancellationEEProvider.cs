using Sistran.Company.Application.PropertyCancellationService.EEProvider.Business;
using Sistran.Company.Application.PropertyCancellationService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.PropertyEndorsementCancellationService.EEProvider;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;

namespace Sistran.Company.Application.PropertyCancellationService.EEProvider
{
    public class CiaPropertyCancellationServiceEEProvider : PropertyCancellationServiceEEProvider, IPropertyCancellationServiceCia
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
                PropertyCancellationBusinessCia propertyCancellationBusinessCia = new PropertyCancellationBusinessCia();
                return propertyCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalCancellationVehicle);
            }
        }

        public CompanyPolicy ExecuteThread(List<PEM.CompanyPropertyRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                PropertyCancellationBusinessCia propertyCancellationBusinessCia = new PropertyCancellationBusinessCia();
                return propertyCancellationBusinessCia.ExecuteThread(risksThread, policy,risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationVehicle);
            }
        }
    }
}
