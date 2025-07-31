
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Framework.SAF;
using VEHPARAM = Sistran.Core.Application.VehicleParamService;

namespace Sistran.Company.Application.Vehicles.VehicleApplicationService.EEProvider
{
    public class DelegateService
    {
        internal static VehicleServices.IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();

        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();

        internal static VEHPARAM.IVehicleParamServiceWeb VehicleParamServices = ServiceProvider.Instance.getServiceManager().GetService<VEHPARAM.IVehicleParamServiceWeb>();
    }
}
