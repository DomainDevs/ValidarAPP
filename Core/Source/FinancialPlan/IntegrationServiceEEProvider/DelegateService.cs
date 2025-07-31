using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.AccountingServices;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider
{
    public static class DelegateService
    {
        internal static IAccountingIntegrationService accountingIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingIntegrationService>();
    }
}
