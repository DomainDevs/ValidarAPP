using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.LiabilityCancellationService;
using Sistran.Company.Application.LiabilityModificationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.LiabilityChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ILiabilityService LibialityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ILiabilityModificationServiceCia libialityModificationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityModificationServiceCia>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static ILiabilityCancellationServiceCia endorsementlibialityCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityCancellationServiceCia>();

    }
}

