using Sistran.Company.Application.CollectiveServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.MassiveUnderwritingServices;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.VehicleCancellationService;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.ProductServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Company.Application.UniquePersonServices.V1;

namespace Sistran.Company.Application.MassiveVehicleCancellationService.EEProvider
{
    /// <summary>
    /// delegados
    /// </summary>
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveUnderwritingService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IProductServiceCore productService = ServiceProvider.Instance.getServiceManager().GetService<IProductServiceCore>();
        internal static IPendingOperationEntityService pendingOperationEntity = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();
        internal static ICollectiveService collectiveService = ServiceProvider.Instance.getServiceManager().GetService<ICollectiveService>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static ICiaVehicleCancellationService ciaVehicleCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ICiaVehicleCancellationService>();
        internal static IUniqueUserService uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserService>();
    }
}