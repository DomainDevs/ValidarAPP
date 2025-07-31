using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.SuretyEndorsementExtensionService.EEProvider.Services
{
    class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
    }
}
