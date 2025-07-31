using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Transports.TransportApplicationService;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.ProductServices;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICompanyTransportApplicationService transportsService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportApplicationService>();
        internal static ICompanyTransportAdjustmentBusinessService adjustmentService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportAdjustmentBusinessService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();

    }
}
