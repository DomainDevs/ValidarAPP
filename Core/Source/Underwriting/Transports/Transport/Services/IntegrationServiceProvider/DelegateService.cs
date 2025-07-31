using Sistran.Core.Application.Transports.TransportBusinessService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Integration.TransportServices.EEProvider
{
    public class DelegateService
    {
        internal static ITransportBusinessService transportBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ITransportBusinessService>();

    }
}
