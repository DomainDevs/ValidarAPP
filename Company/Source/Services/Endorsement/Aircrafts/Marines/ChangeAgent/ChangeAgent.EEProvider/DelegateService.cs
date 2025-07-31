using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.MarineCancellationService;
using Sistran.Company.Application.MarineModificationService;
using Sistran.Company.Application.Marines.MarineBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.MarineChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ICompanyMarineBusinessService marineService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyMarineBusinessService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IMarineModificationServiceCia marineModificationService = ServiceProvider.Instance.getServiceManager().GetService<IMarineModificationServiceCia>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static ICiaMarineCancellationService endorsementMarineCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ICiaMarineCancellationService>();
    }
}
