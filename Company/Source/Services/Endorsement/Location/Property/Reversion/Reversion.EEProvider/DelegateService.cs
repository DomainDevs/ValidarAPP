using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Location.PropertyServices;

using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Company.Application.BaseEndorsementService;

namespace Sistran.Company.Application.PropertyReversionService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
    }
}
