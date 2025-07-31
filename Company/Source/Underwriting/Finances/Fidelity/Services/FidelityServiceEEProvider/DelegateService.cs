using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider
{
    public class DelegateService
    {
        internal static IRulesService rulesService = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();        
        internal static UniquePersonServices.V1.IUniquePersonService uniquePersonServiceV1 = ServiceProvider.Instance.getServiceManager().GetService<UniquePersonServices.V1.IUniquePersonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
    }
}
