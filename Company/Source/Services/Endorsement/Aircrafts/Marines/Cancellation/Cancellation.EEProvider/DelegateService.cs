
using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Marines.MarineBusinessService;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.MarineModificationService;

namespace Sistran.Company.Application.MarineCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICompanyMarineBusinessService marineService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyMarineBusinessService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static IMarineModificationServiceCia marineModificationService = ServiceProvider.Instance.getServiceManager().GetService<IMarineModificationServiceCia>();

    }
}
