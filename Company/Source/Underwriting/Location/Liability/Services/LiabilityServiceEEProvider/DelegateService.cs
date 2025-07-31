using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.ProductServices;
using Sistran.Core.Integration.OperationQuotaServices;
using Sistran.Core.Integration.UnderwritingReinsuranceWorkerServices;
using Sistran.Core.Integration.UniqueUserServices;
using Sistran.Company.Application.ExternalProxyServices;

namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider
{
    public class DelegateService
    {
        internal static IRulesService rulesService = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static UniquePersonServices.V1.IUniquePersonService uniquePersonServiceV1 = ServiceProvider.Instance.getServiceManager().GetService<UniquePersonServices.V1.IUniquePersonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IProductServiceCore productServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IProductServiceCore>();
        internal static IOperationQuotaIntegrationService OperationQuotaIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaIntegrationService>();
        internal static IUnderwritingReinsuranceWorkerIntegrationServices underwritingReinsuranceWorkerIntegration = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingReinsuranceWorkerIntegrationServices>();
        internal static IUniqueUserIntegrationService uniqueUserIntegration = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserIntegrationService>();
        internal static IExternalProxyService ExternalServiceWeb = ServiceProvider.Instance.getServiceManager().GetService<IExternalProxyService>();
    }
}
