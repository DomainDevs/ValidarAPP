using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public static class EndorsementRiskCoverageDAO
    {
        /// <summary>
        /// Lists the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public static IList GetEndorsementRiskCoveragesByFolterSort(Predicate filter, string[] sort)
        {
            return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(EndorsementRiskCoverage), filter, sort);
        }

        /// <summary>
        /// obtiene un EndorsementRiskCoverage a partir del riskNum, endorsementId, y policyId
        /// </summary>
        /// <param name="riskNum">nuemro del riesgo</param>
        /// <param name="endorsementId">id del endoso</param>
        /// <param name="policyId">id de la poliza</param>
        /// <returns></returns>
        public static EndorsementRiskCoverage GetEndorsementRiskCoverageByRiskNumEndorsementIdPolicyId(int riskNum, int endorsementId, int policyId)
        {
            PrimaryKey key = EndorsementRisk.CreatePrimaryKey(riskNum, endorsementId, policyId);
            return (EndorsementRiskCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }
    }
}
