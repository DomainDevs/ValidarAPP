using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Core.CancellationEndorsement3GProvider
{
    public class DelegateService
    {
        internal static IBaseEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
        internal static IUnderwritingServiceCore underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IRulesService rulesService = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
    }
}