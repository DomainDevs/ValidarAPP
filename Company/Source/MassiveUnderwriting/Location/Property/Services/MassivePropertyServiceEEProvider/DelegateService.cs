
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.MassiveUnderwritingServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.MassiveRenewalServices;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Company.Application.ExternalProxyServices;
using Sistran.Company.Application.ExternalProxyMirrorService;
using Sistran.Company.Application.SyBaseEntityService;

namespace Sistran.Company.Application.Location.MassivePropertyServices.EEProvider
{
    using Core.Application.AuthorizationPoliciesServices;

    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveUnderwritingService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IMassiveRenewalService massiveRenewalService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveRenewalService>();
        internal static IExternalProxyService externalProxyService = ServiceProvider.Instance.getServiceManager().GetService<IExternalProxyService>();
        internal static IExternalProxyMirrorService externalProxyMirrorService = ServiceProvider.Instance.getServiceManager().GetService<IExternalProxyMirrorService>();
        internal static IPendingOperationEntityService pendingOperationEntityService = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static ISyBaseEntityService sybaseEntityService = ServiceProvider.Instance.getServiceManager().GetService<ISyBaseEntityService>();
    }
}