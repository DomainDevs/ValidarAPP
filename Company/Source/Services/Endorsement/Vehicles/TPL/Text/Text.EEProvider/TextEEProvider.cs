using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.ThirdPartyLiabilityTextService.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.ThirdPartyLiabilityTextService.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.VehicleTextService.IVehicleTextService" />
    public class ThirdPartyLiabilityTextServiceEEProvider: IThirdPartyLiabilityTextServiceCia
    {
        /// <summary>
        /// Creates the texts.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicyResult CreateTexts(CompanyEndorsement companyEndorsement)
        {
            try
            {
                ThirdPartyLiabilityTextBusinessCia ThirdPartyLiabilityTextBusinessCia = new ThirdPartyLiabilityTextBusinessCia();
                return ThirdPartyLiabilityTextBusinessCia.CreateEndorsementText(companyEndorsement);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
