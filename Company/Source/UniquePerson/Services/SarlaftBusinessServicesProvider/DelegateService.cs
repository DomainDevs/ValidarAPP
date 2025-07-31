using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Application.AuthorizationPoliciesServices;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
    }
}
