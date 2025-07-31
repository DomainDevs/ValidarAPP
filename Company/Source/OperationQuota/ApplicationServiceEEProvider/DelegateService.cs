using Sistran.Company.Integration.OperationQuotaCompanyServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.OperationQuotaServices;
using V1 = Sistran.Company.Application.UniquePersonServices.V1;

namespace Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider
{
    public static class DelegateService
    {
        internal static IOperationQuotaCompanyIntegrationService operationQuotaCompanyIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaCompanyIntegrationService>();
        internal static IOperationQuotaCompanyService operationQuotaCompanyService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaCompanyService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IOperationQuotaService operationQuotaService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaService>();
        internal static V1.IUniquePersonService uniquePersonServiceV1 = ServiceProvider.Instance.getServiceManager().GetService<V1.IUniquePersonService>();
    }
}
