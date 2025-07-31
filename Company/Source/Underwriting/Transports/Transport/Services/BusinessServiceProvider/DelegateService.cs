using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.SarlaftBusinessServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;


namespace Sistran.Company.Application.Transports.TransportBusinessService.EEProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IRulesService rulesService = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static ISarlaftBusinessServices sarlaftBusinessServices = ServiceProvider.Instance.getServiceManager().GetService<ISarlaftBusinessServices>();
        internal static ICompanyTransportBusinessService transportBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportBusinessService>();

    }
}