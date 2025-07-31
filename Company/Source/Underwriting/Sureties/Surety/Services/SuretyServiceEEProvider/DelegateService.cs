
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.ProductServices;
using Sistran.Core.Integration.OperationQuotaServices;
using Sistran.Core.Integration.UnderwritingReinsuranceWorkerServices;
using Sistran.Core.Integration.UniqueUserServices;
using Sistran.Core.Application.ClaimServices;
using Sistran.Core.Application.OperationQuotaServices;
using Sistran.Company.Application.ExternalProxyServices;

namespace Sistran.Company.Application.Sureties.SuretyServices.EEProvider
{
    /// <summary>
    /// Delegados
    /// </summary>
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IProductServiceCore productServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IProductServiceCore>();
        internal static IOperationQuotaIntegrationService OperationQuotaIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaIntegrationService>();
        internal static IConsortiumIntegrationService consortiumIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IConsortiumIntegrationService>();
        internal static IEconomicGroupIntegrationService economicGroupIntegration = ServiceProvider.Instance.getServiceManager().GetService<IEconomicGroupIntegrationService>();
        internal static IUnderwritingReinsuranceWorkerIntegrationServices underwritingReinsuranceWorkerIntegration = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingReinsuranceWorkerIntegrationServices>();
        internal static IUniqueUserIntegrationService uniqueUserIntegration = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserIntegrationService>();
        internal static IClaimApplicationService claimApplicationService = ServiceProvider.Instance.getServiceManager().GetService<IClaimApplicationService>();
        internal static IEconomicGroupService EconomicGroupService = ServiceProvider.Instance.getServiceManager().GetService<IEconomicGroupService>();
        internal static IOperationQuotaService operationQuotaService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaService>();
        internal static IConsortiumService consortiumService = ServiceProvider.Instance.getServiceManager().GetService<IConsortiumService>();
        internal static IExternalProxyService ExternalServiceWeb = ServiceProvider.Instance.getServiceManager().GetService<IExternalProxyService>();

    }
}
