using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.ChangeTermEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.LiabilityCancellationService;
using Sistran.Company.Application.LiabilityModificationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.Location.LiabilityServices;

namespace Sistran.Company.Application.LiabilityChangeTermService.EEProvider.Services
{
    /// <summary>
    /// Servicios
    /// </summary>
    public class DelegateService
    {
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ILiabilityModificationServiceCia LiabilityModificationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityModificationServiceCia>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ILiabilityService LiabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IChangeTermEndorsementCompany changeTermEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IChangeTermEndorsementCompany>();
        internal static ILiabilityCancellationServiceCia endorsementLiabilityCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityCancellationServiceCia>();

    }
}
