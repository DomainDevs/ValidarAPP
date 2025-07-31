using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Marines.MarineBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.BaseEndorsementService;

namespace Sistran.Company.Application.Marines.MarineApplicationService.EEProvider
{
    public class DelegateService
    {
        internal static ICompanyMarineBusinessService marineBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyMarineBusinessService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IBaseEndorsementService endorsementBaseService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
    }
}