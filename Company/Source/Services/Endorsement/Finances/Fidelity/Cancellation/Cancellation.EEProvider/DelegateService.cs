using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.FidelityModificationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Finances.FidelityServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.FidelityCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static IFidelityModificationServiceCia fidelityModificationService = ServiceProvider.Instance.getServiceManager().GetService<IFidelityModificationServiceCia>();
        internal static IFidelityService FidelityService = ServiceProvider.Instance.getServiceManager().GetService<IFidelityService>();
    }
}
