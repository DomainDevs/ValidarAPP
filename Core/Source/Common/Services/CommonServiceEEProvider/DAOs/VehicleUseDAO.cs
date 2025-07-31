using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public static class VehicleUseDAO
    {
        /// <summary>
        /// Finds the specified vehicle use code.
        /// </summary>
        /// <param name="vehicleUseCode">The vehicle use code.</param>
        /// <returns></returns>
        public static COMMEN.VehicleUse Find(int vehicleUseCode)
        {
            PrimaryKey key = COMMEN.VehicleUse.CreatePrimaryKey(vehicleUseCode);
            return (COMMEN.VehicleUse)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Lists the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public static IList List(Predicate filter, string[] sort)
        {
            return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(COMMEN.VehicleType), filter, sort);
        }
    }
}
