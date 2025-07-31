using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
//using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Company.Application.MassiveServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IUniqueUserService uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserService>();
        //internal static IPendingOperationEntityService pendingOperationEntityService = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IScriptsService scriptsService = ServiceProvider.Instance.getServiceManager().GetService<IScriptsService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
    }
}