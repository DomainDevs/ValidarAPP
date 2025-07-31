using Sistran.Core.Application.CommonService;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Application.AccountingGeneralLedgerServices;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Integration.UndewritingIntegrationServices;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public static class DelegateService
    {
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();


        internal static IUnderwritingServiceCore underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IUndewritingIntegrationServices integrationUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUndewritingIntegrationServices>();
        internal static IUniquePersonServiceCore personService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static IAccountingAccountsPayableService accountingAccountsPayableService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingAccountsPayableService>();
        internal static IAccountingCollectService accountingCollectService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingCollectService>();
        //internal static IAccountingImputationService accountingImputationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingImputationService>();
        internal static IAccountingParameterService accountingParameterService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingParameterService>();
        internal static IAccountingPaymentService accountingPaymentService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingPaymentService>();
        internal static IAccountingRetentionService accountingRetentionService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingRetentionService>();
        internal static IAccountingApplicationService accountingApplicationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingApplicationService>();
        internal static ITechnicalTransactionIntegrationService technicalTransactionIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<ITechnicalTransactionIntegrationService>();
        internal static IAccountingGeneralLedgerApplicationService accountingGeneralLedgerApplicationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingGeneralLedgerApplicationService>();
        internal static IAccountingPaymentTicketService accountingPaymentTicketService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingPaymentTicketService>();
        internal static IAccountingAccountService accountingAccountService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingAccountService>();
        internal static IUniqueUserServiceCore uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
        internal static IAccountingPaymentBallotService accountingPaymentBallotService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingPaymentBallotService>();
        internal static IUndewritingIntegrationServices underwritingIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IUndewritingIntegrationServices>();
    }
}

