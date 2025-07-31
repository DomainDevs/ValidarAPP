using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.LiabilityModificationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.CommonServices;

namespace Sistran.Company.Application.LiabilityCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static ILiabilityModificationServiceCia libialityModificationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityModificationServiceCia>();
        internal static ILiabilityService LibialityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
    }
}
