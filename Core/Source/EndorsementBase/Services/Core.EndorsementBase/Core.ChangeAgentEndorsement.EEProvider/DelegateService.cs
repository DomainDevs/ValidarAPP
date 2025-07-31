
using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.ChangeAgentEndorsement.EEProvider
{
    public class DelegateService
    {
        internal static IBaseEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
        internal static IUnderwritingServiceCore underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
    }
}