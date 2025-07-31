using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.GeneralLedgerServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.GeneralLedgerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider
{
   public static class DelegateService
    {
        internal static IAccountingApplicationService generalLedgerService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingApplicationService>();
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IAccountingIntegrationService generalLedgerIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingIntegrationService>();
        internal static IEntryParameterApplicationService entryParameterApplicationService = ServiceProvider.Instance.getServiceManager().GetService<IEntryParameterApplicationService>();
    }
}
