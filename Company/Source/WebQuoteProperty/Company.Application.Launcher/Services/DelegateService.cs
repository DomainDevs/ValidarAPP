using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.PrintingServices;
using Sistran.Company.Application.QuotationServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Core.Application.AuthenticationServices;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Company.Application.Location.PropertyServices;

namespace Sistran.Core.Framework.UIF.Web.Services
{
    public class DelegateService
    {
        internal static IAuthenticationProviders authenticationService = ServiceManager.Instance.GetService<IAuthenticationProviders>();
        internal static ICommonService commonService = ServiceManager.Instance.GetService<ICommonService>();
        internal static IUniquePersonService uniquePersonService = ServiceManager.Instance.GetService<IUniquePersonService>();
        internal static IUniqueUserService uniqueUserService = ServiceManager.Instance.GetService<IUniqueUserService>();
        internal static IUnderwritingService underwritingService = ServiceManager.Instance.GetService<IUnderwritingService>();
        internal static IQuotationService quotationService = ServiceManager.Instance.GetService<IQuotationService>();
        internal static IPropertyService propertyService = ServiceManager.Instance.GetService<IPropertyService>();
        internal static IPrintingService printingService = ServiceManager.Instance.GetService<IPrintingService>();
    }
}