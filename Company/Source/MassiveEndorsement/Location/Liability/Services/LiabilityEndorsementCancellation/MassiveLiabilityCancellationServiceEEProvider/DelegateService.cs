using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.MassiveUnderwritingServices;
using Sistran.Company.Application.PendingOperationEntityService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.MassiveLiabilityCancellationService.EEProvider
{
    /// <summary>
    /// delegados
    /// </summary>
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ILiabilityService liabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveUnderwritingService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IPendingOperationEntityService pendingOperationEntityService = ServiceProvider.Instance.getServiceManager().GetService<IPendingOperationEntityService>();

    }
}