using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.UniquePerson.IntegrationService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.OperationQuotaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider
{
    public static class DelegateService
    {
        internal static IOperationQuotaIntegrationService operationQuotaIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaIntegrationService>();
        internal static IEconomicGroupService economicGroupService = ServiceProvider.Instance.getServiceManager().GetService<IEconomicGroupService>();
        internal static IOperationQuotaService operationQuotaService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaService>();
        internal static IConsortiumService consortiumService = ServiceProvider.Instance.getServiceManager().GetService<IConsortiumService>();
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUniquePersonIntegrationService uniquePersonIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonIntegrationService>();
    }
}
