using Sistran.Company.Application.ThirdPartyLiabilityRenewalService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.ThirdPartyLiabilityEndorsementRenewalService.EEProvider
{
    public class ThirdPartyLiabilityRenewalServiceEEProvider : IThirdPartyLiabilityRenewalServiceCia
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
