using Sistran.Company.Application.ChangeAgentEndorsement;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.PropertyEndorsementModificationService;

using Sistran.Core.Framework.SAF;

using Sistran.Company.Application.PropertyModificationService;
using Sistran.Company.Application.PropertyCancellationService;

namespace Sistran.Company.Application.PropertyChangeAgentService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICiaChangeAgentEndorsement changeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeAgentEndorsement>();
        internal static IPropertyCancellationServiceCia endorsementPropertyCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyCancellationServiceCia>();
        //internal static IPropertyModificationService propertyModificationService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyModificationService>();
        internal static IPropertyModificationServiceCia propertyModificationService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyModificationServiceCia>();

    }
}
