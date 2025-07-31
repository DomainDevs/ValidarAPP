
using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.AircraftModificationService;

namespace Sistran.Company.Application.AircraftCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICompanyAircraftBusinessService aircraftService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyAircraftBusinessService>();
        internal static ICiaCancellationEndorsement cancellationEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaCancellationEndorsement>();
        internal static IAircraftModificationServiceCia aircraftModificationService = ServiceProvider.Instance.getServiceManager().GetService<IAircraftModificationServiceCia>();

    }
}
