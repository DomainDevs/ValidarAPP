using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider
{
    public class DelegateService
    {
        internal static ICompanyAircraftBusinessService AircraftBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyAircraftBusinessService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IBaseEndorsementService endorsementBaseService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();

    }
}