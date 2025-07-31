using Sistran.Company.Application.MassiveUnderwritingServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.MassiveRenewalServices;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Company.Application.ExternalProxyServices;

namespace Sistran.Company.Application.Location.MassiveLiabilityServices.EEProvider
{
    public class DelegateService
    {
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ILiabilityService liabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IMassiveRenewalService massiveRenewalService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveRenewalService>();
        internal static IPendingOperationEntityService pendingOperationEntityService = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();
        internal static IExternalProxyService externalProxyService = ServiceProvider.Instance.getServiceManager().GetService<IExternalProxyService>();
    }
}