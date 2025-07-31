using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.JudicialSuretyModificationService;
using Sistran.Company.Application.Sureties.JudicialSuretyServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Company.Application.JudicialSuretyCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IJudicialSuretyService judicialsuretyService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static IJudicialSuretyModificationServiceCompany judicialsuretyModificationService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyModificationServiceCompany>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
    }
}
