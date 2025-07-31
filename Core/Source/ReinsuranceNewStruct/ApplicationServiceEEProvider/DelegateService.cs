using Sistran.Core.Application.ReportingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Application.UniquePerson.IntegrationService;
using Sistran.Core.Integration.UniqueUserServices;
using Sistran.Core.Integration.CommonServices;
using Sistran.Core.Application.GeneralLedgerServices;
using Sistran.Core.Integration.TempCommonService;
using Sistran.Core.Integration.AccountingServices;
using Sistran.Core.Integration.ReinsuranceOperatingQuotaServices;
using Sistran.Core.Integration.UndewritingIntegrationServices;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider
{
    internal class DelegateService
    {
        public static readonly IUniquePersonIntegrationService uniquePersonIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonIntegrationService>();
        public static readonly ITempCommonIntegrationService tempCommonIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<ITempCommonIntegrationService>();
        public static readonly IUniqueUserIntegrationService uniqueUserIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserIntegrationService>();
        public static readonly IUndewritingIntegrationServices underwritingIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IUndewritingIntegrationServices>();
        public static readonly ICommonIntegrationServiceCore commonIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<ICommonIntegrationServiceCore>();
        public static readonly IAccountingIntegrationService accountingIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingIntegrationService>();
        public static readonly IReinsuranceOperatingQuotaIntegrationServices reinsuranceOperatingQuotaIntegrationServices = ServiceProvider.Instance.getServiceManager().GetService<IReinsuranceOperatingQuotaIntegrationServices>();
        #region Reporting
        internal static IReportingService reportingService = ServiceProvider.Instance.getServiceManager().GetService<IReportingService>();
        internal static IAccountingApplicationService glAccountingApplicationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingApplicationService>();
        #endregion

    }
}
