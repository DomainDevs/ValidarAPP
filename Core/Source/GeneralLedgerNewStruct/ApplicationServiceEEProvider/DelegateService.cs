using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IEntryParameterApplicationService entryParameterApplicationService = ServiceProvider.Instance.getServiceManager().GetService<IEntryParameterApplicationService>();
    }
}
