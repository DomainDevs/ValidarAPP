using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Company.Application.PrintingServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonAplicationServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Company.Application.CollectiveServices.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IUniquePersonAplicationService uniquePersonAplicationService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonAplicationService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IBaseEndorsementService endorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
        internal static IPrintingService printingService = ServiceProvider.Instance.getServiceManager().GetService<IPrintingService>();
        internal static ICollectiveService collectiveService = ServiceProvider.Instance.getServiceManager().GetService<ICollectiveService>();
        internal static IPendingOperationEntityService pendingOperationEntityService = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
    }
}
