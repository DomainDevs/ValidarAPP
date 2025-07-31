using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections;
using QUOEN = Sistran.Core.Application.Quotation.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public static class ComponentDAO
    {
        /// <summary>
        /// Finds the specified component code.
        /// </summary>
        /// <param name="componentCode">The component code.</param>
        /// <returns></returns>
        public static QUOEN.Component Find(int componentCode)
        {
            PrimaryKey key = QUOEN.Component.CreatePrimaryKey(componentCode);
            return (QUOEN.Component)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Lists the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public static IList List(Predicate filter, string[] sort)
        {
            return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(QUOEN.Component), filter, sort);
        }
    }
}
