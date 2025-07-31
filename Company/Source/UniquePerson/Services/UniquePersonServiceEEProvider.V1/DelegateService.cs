using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Company.Application.ExternalProxyServices;
using Sistran.Company.Application.UniquePersonAplicationServices;
using Sistran.Core.Application.UnderwritingServices;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider
{
    public static class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IUniquePersonServiceCore UniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static IExternalProxyService externalProxyService = ServiceProvider.Instance.getServiceManager().GetService<IExternalProxyService>();
        internal static IUniquePersonAplicationService uniquePersonAplicationService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonAplicationService>();
        internal static IUnderwritingServiceCore underwritingServices = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
    }
}
