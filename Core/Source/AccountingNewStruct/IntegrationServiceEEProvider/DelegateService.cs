using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Integration.AccountingServices.EEProvider
{
    public static class DelegateService
    {
        internal static IAccountingIntegrationService accountingIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingIntegrationService>();
    }
}
