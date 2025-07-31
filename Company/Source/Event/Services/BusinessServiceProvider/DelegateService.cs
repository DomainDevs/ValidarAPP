using Sistran.Core.Application.AuthorizationPoliciesParamService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.Event.BusinessService.EEProvider
{
    public class DelegateService
    {
        internal static IAuthorizationPoliciesParamServiceWebCore _AuthorizationPolicies = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesParamServiceWebCore>();
    }
}
