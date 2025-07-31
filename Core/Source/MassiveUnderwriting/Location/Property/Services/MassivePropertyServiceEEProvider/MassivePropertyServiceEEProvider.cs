using System.ServiceModel;
namespace Sistran.Core.Application.Location.MassivePropertyServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassivePropertyServiceEEProvider : IMassivePropertyServiceCore
    {

        
    }
}
