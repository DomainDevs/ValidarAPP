using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Vehicles.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Vehicles.EEProvider.DAOs
{
    /// <summary>
    /// Tipo transmisión
    /// </summary>
    public class TransmissionTypeDAO
    {
        public List<Models.TransmissionType> GetVehicleTransmissionType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TransmissionType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Vehicles.EEProvider.DAOs.GetFuels");
            return ModelAssembler.CreateTransmissionTypes(businessCollection);
        }
    }
}
