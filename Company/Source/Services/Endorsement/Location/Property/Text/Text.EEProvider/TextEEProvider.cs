using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.PropertyTextService.EEProvider.Business;
namespace Sistran.Company.Application.PropertyTextService.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.PropertyTextService.IPropertyTextService" />
    public class PropertyTextServiceEEProvider :  IPropertyTextServiceCia
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
                PropertyTextBusinessCia propertyTextBusinessCia = new PropertyTextBusinessCia();
                return propertyTextBusinessCia.CreateEndorsementText(companyEndorsement);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
