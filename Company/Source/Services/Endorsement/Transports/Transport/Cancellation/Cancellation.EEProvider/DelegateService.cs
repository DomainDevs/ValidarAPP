
using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.TransportModificationService;

namespace Sistran.Company.Application.TransportCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICompanyTransportBusinessService transportService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportBusinessService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static ITransportModificationServiceCia transportModificationService = ServiceProvider.Instance.getServiceManager().GetService<ITransportModificationServiceCia>();

    }
}
