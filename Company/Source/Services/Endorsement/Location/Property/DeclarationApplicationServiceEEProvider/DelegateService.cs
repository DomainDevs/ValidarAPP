using Sistran.Company.Application.DeclarationBusinessService;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.DeclarationApplicationServiceEEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static IDeclarationBusinessService declarationBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IDeclarationBusinessService>();
    }
}
