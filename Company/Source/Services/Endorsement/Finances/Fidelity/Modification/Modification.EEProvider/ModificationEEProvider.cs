using Sistran.Company.Application.FidelityModificationService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Finances.FidelityServices.Models;
using Sistran.Core.Application.FidelityEndorsementModificationService.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.FidelityModificationService;

namespace Sistran.Company.Application.FidelityModificationService.EEProvider
{
    public class FidelityModificationServiceEEProviderCia : FidelityModificationServiceEEProvider, IFidelityModificationServiceCia
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

                FidelityEndorsementBusinessCia endorsementBusinessCia = new FidelityEndorsementBusinessCia();
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
        public CompanyFidelityRisk GetDataModification(CompanyFidelityRisk risk, CoverageStatusType coverageStatusType)
        {
            try
            {
                FidelityEndorsementBusinessCia endorsementBusinessCia = new FidelityEndorsementBusinessCia();
                return endorsementBusinessCia.GetDataModification(risk, coverageStatusType);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }
    }
}
