using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.MassiveServices;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.MassiveUnderwritingServices.EEProvider
{
    public class DelegateService
    {
        internal static IMassiveServiceCore massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveServiceCore>();
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IUniqueUserServiceCore uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
    }
}