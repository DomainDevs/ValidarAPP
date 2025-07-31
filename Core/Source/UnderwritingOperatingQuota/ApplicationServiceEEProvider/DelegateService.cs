using Sistran.Core.Application.OperationQuotaServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.OperationQuotaServices;

namespace Sistran.Core.Application.UnderwritingOperatingQuotaServices.EEProvider
{
    public static class DelegateService
    {
        public static readonly IOperationQuotaService operationQuotaService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaService>();
        public static readonly IOperationQuotaIntegrationService operationQuotaIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaIntegrationService>();
    }
}