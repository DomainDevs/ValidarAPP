using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.TransportCancellationService;
using Sistran.Company.Application.TransportModificationService;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.TransportChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ICompanyTransportBusinessService transportService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportBusinessService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ITransportModificationServiceCia transportModificationService = ServiceProvider.Instance.getServiceManager().GetService<ITransportModificationServiceCia>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static ICiaTransportCancellationService endorsementTransportCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ICiaTransportCancellationService>();
    }
}
