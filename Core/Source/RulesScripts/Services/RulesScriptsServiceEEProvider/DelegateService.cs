using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
//using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
    }
}