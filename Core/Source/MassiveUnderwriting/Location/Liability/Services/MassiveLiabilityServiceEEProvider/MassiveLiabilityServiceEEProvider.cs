using System.ServiceModel;
namespace Sistran.Core.Application.Location.MassiveLiabilityServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveLiabilityServiceEEProvider : IMassiveLiabilityServiceCore
    {

        
    }
}
