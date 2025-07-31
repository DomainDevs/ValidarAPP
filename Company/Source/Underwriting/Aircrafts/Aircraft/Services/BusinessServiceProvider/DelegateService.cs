using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.RulesScriptsServices;
using PRDSERV = Sistran.Core.Application.ProductServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;


namespace Sistran.Company.Application.Aircrafts.AircraftBusinessService.EEProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IRulesService rulesService = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static PRDSERV.IProductServiceCore productServiceCore = ServiceProvider.Instance.getServiceManager().GetService<PRDSERV.IProductServiceCore>();
        
    }
}