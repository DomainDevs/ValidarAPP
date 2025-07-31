using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.LiabilityTextService.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.LiabilityTextService.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.LibialityTextService.ILibialityTextService" />
    public class LiabilityTextServiceEEProvider : ILiabilityTextServiceCia
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
                LiabilityTextBusinessCia LiabilityTextBusinessCia = new LiabilityTextBusinessCia();
                return LiabilityTextBusinessCia.CreateEndorsementText(companyEndorsement, companyModification);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
