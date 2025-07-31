using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.Marines.MarineBusinessService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingServiceCore underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();

    }
}
