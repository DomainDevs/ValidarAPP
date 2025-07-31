using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.ThirdPartyLiabilityModificationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.ThirdPartyLiabilityCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static IThirdPartyLiabilityModificationServiceCia tplModificationService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityModificationServiceCia>();
        internal static IThirdPartyLiabilityService tplService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
    }
}
