using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.BaseEndorsementService;

namespace Sistran.Company.Application.Transports.TransportApplicationService.EEProvider
{
    public class DelegateService
    {
        internal static ICompanyTransportBusinessService transportBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportBusinessService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IBaseEndorsementService endorsementBaseService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
            
    }
}