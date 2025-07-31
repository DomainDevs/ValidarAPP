using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.ReinsuranceIntegrationServices;

namespace Sistran.Core.Application.UnderwritingReinsuranceWorkerServices.EEProvider
{
    internal class DelegateService
    {
        internal static readonly IReinsuranceIntegrationServices reinsuranceIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IReinsuranceIntegrationServices>();
    }
}
