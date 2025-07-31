using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.AccountingServices;
using Sistran.Core.Integration.UndewritingIntegrationServices;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider
{
    public static class DelegateService
    {
        internal static IUndewritingIntegrationServices integrationUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUndewritingIntegrationServices>();
        internal static IAccountingIntegrationService accountingIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingIntegrationService>();
        internal static ICommonServiceCore commonServices = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
    }
}

