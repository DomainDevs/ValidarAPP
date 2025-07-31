using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyClauseService.EEProvider.Business;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.SuretyClauseService.EEProvider
{
    public class SuretyClauseServiceEEProvider : ISuretyClauseServiceCia
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
                SuretyEndorsementBusinessCia endorsementBusinessCia = new SuretyEndorsementBusinessCia();
                return endorsementBusinessCia.CreatePolicyTemporal(companyEndorsement, isMassive, isSaveTemp);
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
        public CompanyPolicyResult CreateClauses(CompanyPolicy companyEndorsement, CompanyModification companyModification)
        {
            try
            {
                ClauseBusinessCia clauseBusinessCia = new ClauseBusinessCia();
                return clauseBusinessCia.CreateEndorsementClause(companyEndorsement, companyModification);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
