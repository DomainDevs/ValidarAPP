
using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;
namespace Sistran.Core.Application.UniquePersonService
{
    public class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
    }
}