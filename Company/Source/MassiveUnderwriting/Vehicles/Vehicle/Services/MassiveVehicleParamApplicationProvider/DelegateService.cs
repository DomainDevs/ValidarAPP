using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.MassiveVehicleParamBusinessService;

namespace Sistran.Company.Application.MassiveVehicleParamApplicationServiceProvider
{
    public class DelegateService
    {
        internal static ICompanyMassiveVehicleParamBusinessService vehicleParamBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyMassiveVehicleParamBusinessService>();
    }
}
