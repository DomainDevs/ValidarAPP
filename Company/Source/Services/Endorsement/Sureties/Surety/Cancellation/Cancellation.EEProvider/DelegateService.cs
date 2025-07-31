using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.SuretyModificationService;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.CommonServices;

namespace Sistran.Company.Application.SuretyCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static ISuretyModificationServiceCia suretyModificationService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyModificationServiceCia>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();

    }
}
