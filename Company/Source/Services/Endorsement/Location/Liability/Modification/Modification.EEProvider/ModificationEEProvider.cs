using Sistran.Company.Application.LiabilityModificationService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Core.Application.LiabilityEndorsementModificationService.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.LiabilityModificationService;

namespace Sistran.Company.Application.LiabilityModificationService.EEProvider
{
    public class LiabilityModificationServiceEEProviderCia : LiabilityModificationServiceEEProvider, ILiabilityModificationServiceCia
    {

        /// <summary>
        /// Creacion Temporal
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// 

        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool isMassive)
        {

            try
            {
                return CreateTemporal(companyEndorsement, isMassive, false);
            }
            
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Creacion Temporal
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool isMassive, bool isSaveTemp)
        {
            try
            {

                LiabilityEndorsementBusinessCia endorsementBusinessCia = new LiabilityEndorsementBusinessCia();
                return endorsementBusinessCia.CreatePolicyTemporal(companyEndorsement, isMassive, isSaveTemp);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Gets the data modification.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyLiabilityRisk GetDataModification(CompanyLiabilityRisk risk, CoverageStatusType coverageStatusType)
        {
            try
            {
                LiabilityEndorsementBusinessCia endorsementBusinessCia = new LiabilityEndorsementBusinessCia();
                return endorsementBusinessCia.GetDataModification(risk, coverageStatusType);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }
    }
}
