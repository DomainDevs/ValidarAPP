
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections;
using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Data;
using System.Linq;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public static class EndorsementRiskDAO
    {
        /// <summary>
        /// Finds the specified risk number.
        /// </summary>
        /// <param name="riskNum">The risk number.</param>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="policyId">The policy identifier.</param>
        /// <returns></returns>
        public static EndorsementRisk GetEndorsementRiskByRiskNumEndorsementIdPolicyId(int riskNum, int endorsementId, int policyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EndorsementRisk.Properties.RiskNum, typeof(EndorsementRisk).Name)
            .Equal()
            .Constant(riskNum)
            .And()
             .Property(EndorsementRisk.Properties.EndorsementId, typeof(EndorsementRisk).Name)
            .Equal()
            .Constant(endorsementId)
             .And()
             .Property(EndorsementRisk.Properties.PolicyId, typeof(EndorsementRisk).Name)
            .Equal()
            .Constant(policyId);
            return DataFacadeManager.Instance.GetDataFacade().List(typeof(EndorsementRisk), filter.GetPredicate()).Cast<EndorsementRisk>().FirstOrDefault();
            //PrimaryKey key = EndorsementRisk.CreatePrimaryKey(riskNum, endorsementId, policyId);
            //return (EndorsementRisk)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Lists the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public static IList GetEndorsementRisksByFilterSort(Predicate filter, string[] sort)
        {
            return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(EndorsementRisk), filter, sort);
        }

    }
}
