using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Company.Application.PrintingServices;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.CommonService;
using Sistran.Company.Application.ProductServices;
using Sistran.Core.Application.UnderwritingServices;

namespace Sistran.Company.Application.MassiveRenewalServices.EEProvider
{
    class DelegateService
    {
        internal static ILiabilityService liabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        //internal static IThirdPartyLiabilityService thirdPartyLiabilityService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IMassiveRenewalService massiveRenewalService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveRenewalService>();
        internal static IBaseEndorsementService endorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
        internal static IPrintingService printingService = ServiceProvider.Instance.getServiceManager().GetService<IPrintingService>();
        internal static IPendingOperationEntityService pendingOperationEntityService = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
    }
}
