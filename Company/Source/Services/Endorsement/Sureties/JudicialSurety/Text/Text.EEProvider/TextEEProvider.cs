using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.JudicialSuretyTextService.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.JudicialSuretyTextService.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.SuretyTextService.ISuretyTextService" />
    public class JudicialSuretyTextServiceEEProvider :  IJudicialSuretyTextServiceCompany
    {
        /// <summary>
        /// Creates the texts.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateTexts(CompanyEndorsement companyEndorsement)
        {
            try
            {
                JudicialSuretyTextBusinessCompany suretyTextBusinessCompany = new JudicialSuretyTextBusinessCompany();
                return suretyTextBusinessCompany.CreateEndorsementText(companyEndorsement);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
