using Sistran.Core.Application.CollectiveServices;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityCollectiveService.EEProvider
{
    public class DelegateService
    {
        internal static ICollectiveServiceCore collectiveService = ServiceProvider.Instance.getServiceManager().GetService<ICollectiveServiceCore>();
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
    }
}