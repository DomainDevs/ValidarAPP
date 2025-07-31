using System.ServiceModel;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityCollectiveService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ThirdPartyLiabilityCollectiveServiceEEProviderCore : IThirdPartyLiabilityCollectiveServiceCore
    {
    }
}