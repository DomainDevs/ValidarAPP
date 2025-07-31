using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public static class VehicleVersionDAO
    {
        /// <summary>
        /// Finds the specified vehicle version code.
        /// </summary>
        /// <param name="vehicleVersionCode">The vehicle version code.</param>
        /// <param name="vehicleModelCode">The vehicle model code.</param>
        /// <param name="vehicleMakeCode">The vehicle make code.</param>
        /// <returns></returns>
        public static COMMEN.VehicleVersion Find(int vehicleVersionCode, int vehicleModelCode, int vehicleMakeCode)
        {
            PrimaryKey key = COMMEN.VehicleVersion.CreatePrimaryKey(vehicleVersionCode, vehicleModelCode, vehicleMakeCode);
            return (COMMEN.VehicleVersion)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Lists the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public static IList List(Predicate filter, string[] sort)
        {
            return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(COMMEN.VehicleVersion), filter, sort);
        }
    }
}
