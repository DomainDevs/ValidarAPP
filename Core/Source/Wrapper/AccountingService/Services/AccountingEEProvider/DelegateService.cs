using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices;
using Sistran.Core.Integration.UndewritingIntegrationServices;

namespace Sistran.Core.Application.WrapperAccountingServiceEEProvide
{

    public static class DelegateService
    {
        internal static readonly IAccountingAccountService accountingAccount = ServiceProvider.Instance.getServiceManager().GetService<IAccountingAccountService>();
        internal static readonly ITechnicalTransactionIntegrationService technicalTransactionIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<ITechnicalTransactionIntegrationService>();
        internal static readonly IAccountingApplicationService accountingApplicationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingApplicationService>();
        internal static readonly IUndewritingIntegrationServices undewritingIntegrationServices = ServiceProvider.Instance.getServiceManager().GetService<IUndewritingIntegrationServices>();
        internal static readonly ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static readonly IAccountingCollectControlService accountingCollectControlService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingCollectControlService>();
        internal static readonly IAccountingPaymentService accountingPaymentService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingPaymentService>();
    }
}
