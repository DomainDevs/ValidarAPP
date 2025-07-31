using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.ProductServices.EEProvider
{
    class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static IRulesService ruleServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static IScriptsService scriptServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IScriptsService>();
        internal static IUniqueUserServiceCore uniqueUserServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
    }
}
