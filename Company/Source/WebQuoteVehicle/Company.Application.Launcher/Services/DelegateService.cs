using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.PrintingServices;
using Sistran.Company.Application.QuotationServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonAplicationServices;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.AuthenticationServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Services.UtilitiesServices;
using AUTHEPROVIDER = Sistran.Core.Application.AuthenticationServices;

namespace Sistran.Core.Framework.UIF.Web.Services
{
    public class DelegateService
    {
        internal static IAuthenticationProviders authenticationService = ServiceManager.Instance.GetService<IAuthenticationProviders>();
        internal static IAuthorizationProvider authorizationService = ServiceManager.Instance.GetService<IAuthorizationProvider>();
        internal static ICommonService commonService = ServiceManager.Instance.GetService<ICommonService>();
        internal static IUniquePersonService uniquePersonService = ServiceManager.Instance.GetService<IUniquePersonService>();
        internal static IUniqueUserService uniqueUserService = ServiceManager.Instance.GetService<IUniqueUserService>();
        internal static IRulesService rulesService = ServiceManager.Instance.GetService<IRulesService>();
        internal static IUnderwritingService underwritingService = ServiceManager.Instance.GetService<IUnderwritingService>();
        internal static IQuotationService quotationService = ServiceManager.Instance.GetService<IQuotationService>();
        internal static IVehicleService vehicleService = ServiceManager.Instance.GetService<IVehicleService>();
        internal static IPrintingService printingService = ServiceManager.Instance.GetService<IPrintingService>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceManager.Instance.GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonAplicationService uniquePersonAplicationService = ServiceManager.Instance.GetService<IUniquePersonAplicationService>();
        internal static AUTHEPROVIDER.IAuthenticationProviders AuthenticationProviders = ServiceManager.Instance.GetService<AUTHEPROVIDER.IAuthenticationProviders>();
    }
}