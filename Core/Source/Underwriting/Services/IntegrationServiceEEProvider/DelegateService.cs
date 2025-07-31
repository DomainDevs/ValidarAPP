using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.AccountingServices;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider
{
    public static class DelegateService
    {
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IAccountingIntegrationService accountingIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingIntegrationService>();
        internal static IUniquePersonServiceCore personService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
    }
}
