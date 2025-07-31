using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.JudicialSuretyModificationService.EEProvider.Business;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.SuretyEndorsementModificationService.EEProvider;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.JudicialSuretyModificationService.EEProvider
{
    public class JudicialSuretyModificationServiceEEProviderCompany : SuretyModificationServiceEEProvider, IJudicialSuretyModificationServiceCompany
    {

        /// <summary>
        /// Creacion Temporal
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
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
                JudicialSuretyEndorsementBusinessCompany endorsementBusinessCompany = new JudicialSuretyEndorsementBusinessCompany();
                return endorsementBusinessCompany.CreatePolicyTemporal(companyEndorsement, isMassive, isSaveTemp);
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
        public CompanyJudgement GetDataModification(CompanyJudgement risk, CoverageStatusType coverageStatusType)
        {
            try
            {
                JudicialSuretyEndorsementBusinessCompany endorsementBusinessCompany = new JudicialSuretyEndorsementBusinessCompany();
                return endorsementBusinessCompany.GetDataModification(risk, coverageStatusType);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }
    }
}
