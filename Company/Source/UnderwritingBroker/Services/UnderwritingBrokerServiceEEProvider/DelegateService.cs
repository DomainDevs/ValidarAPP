using Sistran.Company.Application.CollectiveServices;
using Sistran.Company.Application.MassiveRenewalServices;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.MassiveUnderwritingServices;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Company.Application.Vehicles.TPLCollectiveServices;
using Sistran.Company.Application.Vehicles.VehicleCollectiveServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Company.Application.PendingOperationEntityServiceEEProvider
{
    public class DelegateService
    {
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveUnderwritingService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IMassiveRenewalService massiveRenewalService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveRenewalService>();
        internal static ICollectiveService collectiveService = ServiceProvider.Instance.getServiceManager().GetService<ICollectiveService>();
        internal static IVehicleCollectiveService vehicleCollectiveService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleCollectiveService>();
        //internal static IMassiveVehicleService massiveVehicleService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveVehicleService>();
        //internal static IMassivePropertyService massivePropertyService = ServiceProvider.Instance.getServiceManager().GetService<IMassivePropertyService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        //internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        //internal static ISyBaseEntityService syBaseEntityService = ServiceProvider.Instance.getServiceManager().GetService<ISyBaseEntityService>();
        internal static IPendingOperationEntityService pendingOperationEntityService = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IThirdPartyLiabilityService thirdPartyLiabilityService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
        internal static IThirdPartyLiabilityCollectiveService thirdPartyLiabilityCollectiveService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityCollectiveService>();

    }
}