using Sistran.Company.Application.ThirdPartyLiabilityEndorsementExtensionService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.ThirdPartyLiabilitytExtensionService.EEProvider
{
    public class ThirdPartyLiabilityExtensionServiceEEProvider: IThirdPartyLiabilityExtensionServiceCia
    {
        /// <summary>
        /// Creacion prorroga
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement)
        {
            try
            {
                ExtensionBusinessCia ExtensionBusinessCia = new ExtensionBusinessCia();
                return ExtensionBusinessCia.CreateEndorsementExtension(companyEndorsement);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
