using System.Collections.Generic;
using Sistran.Core.Application.Vehicles.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using System.Diagnostics;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.Vehicles.EEProvider.DAOs
{
    /// <summary>
    /// Combustible
    /// </summary>
    public class FuelDAO
    {
        /// <summary>
        /// Obtener listado de combustible
        /// </summary>
        /// <returns></returns>
        public List<Models.Fuel> GetFuels()
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(VehicleFuel)));
            }
            
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetFuels");
            return ModelAssembler.CreateFuels(businessCollection);
        }
    }
}
