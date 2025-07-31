using System.ServiceModel;
using Sistran.Core.Application.Vehicles.MassiveUnderwritingTPLServices;

namespace Sistran.Core.Application.Vehicles.MassiveTPLServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveTPLServiceEEProvider : IMassiveTPLServiceCore
    {   
    }
}