using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.JudicialSuretyClauseService.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.JudicialSuretyClauseService.EEProvider
{
    public class JudicialSuretyClauseServiceEEProvider : IJudicialSuretyClauseServiceCompany
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
        /// Creacion Clausulas
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateClauses(CompanyPolicy companyEndorsement)
        {
            try
            {
                ClauseBusinessCompany clauseBusinessCompany = new ClauseBusinessCompany();
                return clauseBusinessCompany.CreateEndorsementClause(companyEndorsement);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
