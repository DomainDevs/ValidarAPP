using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.SuretyCancellationService;
using Sistran.Company.Application.SuretyModificationService;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.SuretyChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ISuretyModificationServiceCia suretyModificationService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyModificationServiceCia>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static ISuretyCancellationServiceCia endorsementSuretyCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyCancellationServiceCia>();
    }
}
