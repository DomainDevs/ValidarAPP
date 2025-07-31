using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Core.Application.PrintingServicesEEProvider
{
    public class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUnderwritingServiceCore underwritningServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
    }
}