using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Core.Integration.OperationQuotaServices;
using Sistran.Core.Integration.UnderwritingReinsuranceWorkerServices;

namespace Sistran.Company.Application.SuretyReversionService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static IOperationQuotaIntegrationService OperationQuotaIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaIntegrationService>();
        internal static IUnderwritingReinsuranceWorkerIntegrationServices underwritingReinsuranceWorkerIntegration = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingReinsuranceWorkerIntegrationServices>();

    }
}
