
using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ICompanyTransportBusinessService transportServices = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportBusinessService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ICompanyTransportDeclarationBusinessService transportService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportDeclarationBusinessService>();

    }
}