using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.ChangeTermEndorsement.EEProvider
{
    public class DelegateService
    {
        internal static IBaseEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
        internal static IChangeTermEndorsement changeTermEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IChangeTermEndorsement>();     
    }
}