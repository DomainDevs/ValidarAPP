using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.LiabilityModificationService;
using Sistran.Company.Application.ProductServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.ChangePolicyHolderEndorsement;
using Sistran.Company.Application.LiabilityCancellationService;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.UniquePersonService.V1;

namespace Sistran.Company.Application.LiabilityChangePolicyHolderService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICiaChangePolicyHolderEndorsement ciaChangePolicyHolderEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangePolicyHolderEndorsement>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ILiabilityModificationServiceCia LiabilityModificationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityModificationServiceCia>();
        internal static ILiabilityService LiabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ILiabilityCancellationServiceCia endorsementLiabilityCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityCancellationServiceCia>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
    }
}
