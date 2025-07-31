using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.Sureties.JudicialSuretyServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.ProductServices;
using Sistran.Core.Integration.OperationQuotaServices;
using Sistran.Core.Integration.UnderwritingReinsuranceWorkerServices;
using Sistran.Core.Integration.UniqueUserServices;
using Sistran.Company.Application.ExternalProxyServices;

namespace JudicialSuretyServicesEEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUnderwritingService underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IJudicialSuretyCore judicialSuretyServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyCore>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static IProductServiceCore productServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IProductServiceCore>();
        internal static IOperationQuotaIntegrationService OperationQuotaIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaIntegrationService>();
        internal static IUniqueUserIntegrationService uniqueUserIntegration = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserIntegrationService>();
        internal static IUnderwritingReinsuranceWorkerIntegrationServices underwritingReinsuranceWorkerIntegration = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingReinsuranceWorkerIntegrationServices>();
        internal static IExternalProxyService ExternalServiceWeb = ServiceProvider.Instance.getServiceManager().GetService<IExternalProxyService>();
    }
}
