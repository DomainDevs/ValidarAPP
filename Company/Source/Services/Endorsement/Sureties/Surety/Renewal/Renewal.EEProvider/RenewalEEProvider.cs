using Sistran.Company.Application.SuretyRenewalService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.SuretyEndorsementRenewalService.EEProvider
{
    public class SuretyRenewalServiceEEProvider :  ISuretyRenewalServiceCompany
    {
        /// <summary>
        /// Creacion Renewal
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
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
