using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.VehicleModificationService;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Company.Application.CommonServices;

namespace Sistran.Company.Application.VehicleCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static IVehicleModificationServiceCia vehicleModificationService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleModificationServiceCia>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
    }
}
