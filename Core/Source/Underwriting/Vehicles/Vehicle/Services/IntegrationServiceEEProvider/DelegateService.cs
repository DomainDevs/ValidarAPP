using Sistran.Core.Application.Vehicles.VehicleServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.VehicleService.EEProvider
{
    public static class DelegateService
    {
        internal static IVehicleServiceCore vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleServiceCore>();
    }
}
