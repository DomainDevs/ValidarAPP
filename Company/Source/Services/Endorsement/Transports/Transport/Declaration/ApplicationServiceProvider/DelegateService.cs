//using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.Transports.Endorsement.Declaration.BusinessServices;
using Sistran.Company.Application.Transports.TransportApplicationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICompanyTransportApplicationService transportApplicationService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportApplicationService>();
        internal static ICompanyTransportDeclarationBusinessService DeclarationService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportDeclarationBusinessService>();

    }
}
