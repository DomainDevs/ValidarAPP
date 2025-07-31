using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public static class AllyCoverageDAO
    {
        /// <summary>
        /// Finds the specified ally coverage identifier.
        /// </summary>
        /// <param name="allyCoverageId">The ally coverage identifier.</param>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <returns></returns>
        public static AllyCoverage GetAllyCoverageByAllyCoverageIdCoverageId(int allyCoverageId, int coverageId)
        {
            PrimaryKey key = AllyCoverage.CreatePrimaryKey(allyCoverageId, coverageId);
            return (AllyCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Lists the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public static IList GetAllyCoveragesByFilterSort(Predicate filter, string[] sort)
        {
            return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(AllyCoverage), filter, sort);
        }
    }
}
