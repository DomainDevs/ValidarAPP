using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Framework.SAF;
namespace Sistran.Company.Application.VehicleReversionService.EEProvider
{
    using BaseEndorsementService;
    using Core.Application.AuthorizationPoliciesServices;

    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
    }
}
