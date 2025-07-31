
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Core.Framework.UIF2.Services;

namespace Sistran.Core.Framework.UIF.Web.Services
{
    public class DelegateService
    {
        internal static IUniqueUserService uniqueUserService = ServiceManager.Instance.GetService<IUniqueUserService>();
    }
}
