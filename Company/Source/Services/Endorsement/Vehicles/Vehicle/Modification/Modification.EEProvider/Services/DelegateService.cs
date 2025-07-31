using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.VehicleModificationService.EEProvider.Services
{
    /// <summary>
    /// Servicios
    /// </summary>
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
    }
}
