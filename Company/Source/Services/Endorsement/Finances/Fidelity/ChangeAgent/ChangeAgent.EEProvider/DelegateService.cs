using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.FidelityCancellationService;
using Sistran.Company.Application.FidelityModificationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Finances.FidelityServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.FidelityChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IFidelityService FidelityService = ServiceProvider.Instance.getServiceManager().GetService<IFidelityService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IFidelityModificationServiceCia fidelityModificationService = ServiceProvider.Instance.getServiceManager().GetService<IFidelityModificationServiceCia>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static IFidelityCancellationServiceCia endorsementfidelityCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IFidelityCancellationServiceCia>();

    }
}

