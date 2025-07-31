using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.ThirdPartyLiabilityModificationService.EEProvider.Services
{
    /// <summary>
    /// Servicios
    /// </summary>
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IThirdPartyLiabilityService ThirdPartyLiabilityService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
    }
}
