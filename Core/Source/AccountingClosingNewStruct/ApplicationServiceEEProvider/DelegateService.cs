using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.GeneralLedgerServices;
using Sistran.Core.Application.ReportingServices;
using Sistran.Core.Application.TempCommonServices;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider
{
    public static class DelegateService
    {
        internal static IDataFacadeManager DataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        internal static ITempCommonService tempCommonService = ServiceProvider.Instance.getServiceManager().GetService<ITempCommonService>();
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IAccountingApplicationService generalLedgerService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingApplicationService>();
        internal static IEntryParameterApplicationService entryParameterService = ServiceProvider.Instance.getServiceManager().GetService<IEntryParameterApplicationService>();
        internal static IUniqueUserServiceCore uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
        internal static IReportingService reportService = ServiceProvider.Instance.getServiceManager().GetService<IReportingService>();
    }
}
