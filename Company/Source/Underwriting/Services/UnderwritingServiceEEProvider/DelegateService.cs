using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ParametrizationParamBusinessService;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.SarlaftApplicationServices;
using Sistran.Company.Application.UniquePersonAplicationServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static UniquePersonServices.V1.IUniquePersonService uniquePersonServiceV1 = ServiceProvider.Instance.getServiceManager().GetService<UniquePersonServices.V1.IUniquePersonService>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IParametrizationParamBusinessService ParametrizationParamBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IParametrizationParamBusinessService>();
        internal static IUniqueUserServiceCore uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IUniquePersonListRiskApplicationServices coreUniqueListRiskPersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskApplicationServices>();
        internal static ISarlaftApplicationServices SarlaftApplicationServices = ServiceProvider.Instance.getServiceManager().GetService<ISarlaftApplicationServices>();
        internal static ITaxService taxServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ITaxService>();
        internal static IUniquePersonAplicationService uniquePersonAplicationService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonAplicationService>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
    }
}