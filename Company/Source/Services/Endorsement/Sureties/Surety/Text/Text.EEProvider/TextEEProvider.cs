using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyTextService.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.SuretyTextService.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.SuretyTextService.ISuretyTextService" />
    public class SuretyTextServiceEEProvider : ISuretyTextServiceCia
    {
        /// <summary>
        /// Creates the texts.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicyResult CreateTexts(CompanyEndorsement companyEndorsement, CompanyModification companyModification)
        {
            try
            {
                SuretyTextBusinessCia suretyTextBusinessCia = new SuretyTextBusinessCia();
                return suretyTextBusinessCia.CreateEndorsementText(companyEndorsement, companyModification);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
