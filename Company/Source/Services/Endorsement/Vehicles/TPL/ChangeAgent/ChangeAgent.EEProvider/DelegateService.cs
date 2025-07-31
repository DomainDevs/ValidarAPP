using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.ThirdPartyLiabilityCancellationService;
using Sistran.Company.Application.ThirdPartyLiabilityModificationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.ThirdPartyLiabilityChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IThirdPartyLiabilityService tplService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IThirdPartyLiabilityModificationServiceCia tplModificationService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityModificationServiceCia>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static IThirdPartyLiabilityCancellationServiceCia endorsementtplCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityCancellationServiceCia>();

    }
}

