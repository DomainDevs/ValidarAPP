using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.TransportTextService.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.TransportTextService.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.TransportTextService.ITransportTextService" />
    public class TransportTextServiceEEProvider : ITransportTextService
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
                TransportTextBusinessCia transportTextBusinessCia = new TransportTextBusinessCia();
                return transportTextBusinessCia.CreateEndorsementText(companyEndorsement);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
