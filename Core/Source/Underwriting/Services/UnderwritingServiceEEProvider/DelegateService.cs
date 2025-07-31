using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.ProductServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider
{
    /// <summary>
    /// Delegados
    /// </summary>
    class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IUniqueUserServiceCore uniqueUserServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
        internal static IRulesService ruleServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static IScriptsService scriptServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IScriptsService>();
        internal static IProductServiceCore productServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IProductServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonServiceCoreV1 = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static ITaxService taxServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ITaxService>();
    }
}
