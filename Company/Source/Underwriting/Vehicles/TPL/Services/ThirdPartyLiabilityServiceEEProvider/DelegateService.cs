using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.Transports.TransportBusinessService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;


namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.EEProvider
{
    public class DelegateService
    {        
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        //internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static ITransportBusinessService TransportServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ITransportBusinessService>();
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
    }
}