using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.TransportRenewalService.EEProvider.Business;

namespace Sistran.Company.Application.TransportEndorsementRenewalService.EEProvider
{
    public class TransportRenewalServiceEEProvider : ICiaTransportRenewalService
    {
        /// <summary>
        /// Creacion Renewal
        /// </summary>
        /// <param name="companyPolicy">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy)
        {
            try
            {
                RenewalBusinessCia RenewalBusinessCia = new RenewalBusinessCia();
                return RenewalBusinessCia.CreateTemporal(companyPolicy);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
