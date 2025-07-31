using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.VehicleCancellationService;
using Sistran.Company.Application.VehicleModificationService;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.VehicleChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IVehicleModificationServiceCia vehicleModificationService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleModificationServiceCia>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static ICiaVehicleCancellationService endorsementVehicleCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ICiaVehicleCancellationService>();
    }
}
