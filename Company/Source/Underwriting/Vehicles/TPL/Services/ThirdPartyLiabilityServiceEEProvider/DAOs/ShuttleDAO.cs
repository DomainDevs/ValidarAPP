using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.DAOs
{
    public class ShuttleDAO
    {
        /// <summary>
        /// Obtener trayectos por codigo
        /// </summary>
        /// <param name="shuttleCode">codigo de trayecto</param>
        /// <returns></returns>
        public Shuttle GetShuttleByShuttleCode(int shuttleCode)
        {
            PrimaryKey key = Shuttle.CreatePrimaryKey(shuttleCode);
            return (Shuttle)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Obtener trayectos disponibles
        /// </summary>
        /// <returns>Lista de trayectos</returns>
        public static List<Models.Shuttle> GetShuttlesEnabled()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Shuttle.Properties.Enabled);
            filter.Equal();
            filter.Constant(true);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.DAOs.GetShuttlesEnabled");
            return Assemblers.ModelAssembler.CreateShuttles((IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(Shuttle), filter.GetPredicate()));
        }
    }
}
