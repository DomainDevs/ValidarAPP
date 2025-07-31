using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;

          
namespace Sistran.Company.Application.PropertyEndorsementRenewalService.EEProvider
{
    public class PropertyRenewalServiceEEProvider :  IPropertyRenewalServiceCia
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
                Business.RenewalBusinessCia RenewalBusinessCia = new Business.RenewalBusinessCia();
                return RenewalBusinessCia.CreateTemporal(companyPolicy);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
