using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.TransportModificationService;

namespace Sistran.Company.Application.TransportClauseService.EEProvider.Services
{
    /// <summary>
    /// Servicios
    /// </summary>
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ICompanyTransportBusinessService transportService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportBusinessService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static ITransportModificationServiceCia transportModificationService = ServiceProvider.Instance.getServiceManager().GetService<ITransportModificationServiceCia>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();

    }
}
