using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService;
using Sistran.Company.Application.UnderwritingServices;

using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.AircraftExtensionService.EEProvider
{
    class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ICompanyAircraftBusinessService aircraftService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyAircraftBusinessService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
    }
}