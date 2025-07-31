using System.ServiceModel;
using Sistran.Core.Application.Vehicles.MassiveUnderwritingVehicleServices;

namespace Sistran.Core.Application.Vehicles.MassiveVehicleServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveVehicleServiceEEProvider : IMassiveVehicleServiceCore
    {   
    }
}