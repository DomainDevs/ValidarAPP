
using Sistran.Core.Application.CommonService;
//using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.SAF;
namespace Sistran.Core.Application.UniquePersonService.V1
{
    public class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        // internal static IUniqueUserServiceCore uniqueUserServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
       
    }
}