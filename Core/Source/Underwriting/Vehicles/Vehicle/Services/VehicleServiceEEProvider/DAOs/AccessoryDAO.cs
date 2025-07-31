using System.Collections.Generic;
using Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using System.Diagnostics;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs
{
    public class AccessoryDAO
    {
        /// <summary>
        /// Obtener Accesorios
        /// </summary>
        /// <returns>Lista de Accesorios</returns>
        public List<Models.Accessory> GetAccessories()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Detail)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.DAOs.GetAccessories");
            return ModelAssembler.CreateAccessories(businessCollection);
        }
    }
}
