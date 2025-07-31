using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Company.Application.PrintingServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Company.Application.ProductServices;

namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveUnderwritingService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static IUniqueUserService uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserService>();
        internal static IPrintingService printingService = ServiceProvider.Instance.getServiceManager().GetService<IPrintingService>();
        internal static IAuthorizationPoliciesServiceCore IAuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
    }
}