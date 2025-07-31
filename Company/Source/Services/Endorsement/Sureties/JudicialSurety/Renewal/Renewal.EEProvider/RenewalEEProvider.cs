using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.JudicialSuretyRenewalService.EEProvider.Business;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

namespace Sistran.Company.Application.JudicialSuretyRenewalService.EEProvider
{
    public class JudicialSuretyRenewalServiceEEProvider : IJudicialSuretyRenewalService
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
                JudicialSuretyBusinessRenewal RenewalBusinessCia = new JudicialSuretyBusinessRenewal();
                return RenewalBusinessCia.CreateTemporal(companyPolicy);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
