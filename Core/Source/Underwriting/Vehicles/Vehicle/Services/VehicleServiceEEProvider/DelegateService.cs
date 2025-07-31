using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static IUnderwritingServiceCore underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();

    }
}
