


using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.UniquePersonService;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider
{
    public class DelegateService
    {
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static IUnderwritingServiceCore underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();

    }
}
