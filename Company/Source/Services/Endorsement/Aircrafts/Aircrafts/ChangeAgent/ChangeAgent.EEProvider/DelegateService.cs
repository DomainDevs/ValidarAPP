using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.AircraftCancellationService;
using Sistran.Company.Application.AircraftModificationService;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.AircraftChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ICompanyAircraftBusinessService aircraftService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyAircraftBusinessService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IAircraftModificationServiceCia aircraftModificationService = ServiceProvider.Instance.getServiceManager().GetService<IAircraftModificationServiceCia>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static ICiaAircraftCancellationService endorsementAircraftCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ICiaAircraftCancellationService>();
    }
}
