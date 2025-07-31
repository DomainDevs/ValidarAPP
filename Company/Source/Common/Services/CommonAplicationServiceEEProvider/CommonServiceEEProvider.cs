using Sistran.Company.Application.CommonAplicationServices;
using System.ServiceModel;

namespace Sistran.Company.Application.CommonServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CommonAplicationServiceEEProvider : Core.Application.CommonServices.EEProvider.CommonServiceEEProvider, ICommonAplicationService
    {
        
    }
}
