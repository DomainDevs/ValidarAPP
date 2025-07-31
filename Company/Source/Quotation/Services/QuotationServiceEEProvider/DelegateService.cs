using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.SarlaftApplicationServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.QuotationServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Company.Application.QuotationServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IUniqueUserService uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IQuotationServiceCore quotationService = ServiceProvider.Instance.getServiceManager().GetService<IQuotationServiceCore>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static ISarlaftApplicationServices SarlaftApplicationServices = ServiceProvider.Instance.getServiceManager().GetService<ISarlaftApplicationServices>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IRulesService rulesService = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
    }
}