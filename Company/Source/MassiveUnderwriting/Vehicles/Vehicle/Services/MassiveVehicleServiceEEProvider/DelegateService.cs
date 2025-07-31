
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.MassiveUnderwritingServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Company.Application.MassiveRenewalServices;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Company.Application.CollectiveServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.AuthorizationPoliciesServices;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.UnderwritingParamService;
using Sistran.Core.Application.UnderwritingServices;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveUnderwritingService>();
        internal static IUniqueUserService uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserService>();
        internal static IMassiveRenewalService massiveRenewal = ServiceProvider.Instance.getServiceManager().GetService<IMassiveRenewalService>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IPendingOperationEntityService pendingOperationEntityService = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();
        internal static ICollectiveService collectiveService = ServiceProvider.Instance.getServiceManager().GetService<ICollectiveService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IAuthorizationPoliciesService authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesService>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUnderwritingParamServiceWeb UnderwritingParamServiceWeb = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingParamServiceWeb>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();

    }
}