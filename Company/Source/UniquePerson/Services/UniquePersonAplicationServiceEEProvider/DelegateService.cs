

using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.SarlaftApplicationServices;
using Sistran.Company.Application.UniquePersonAplicationServices;
using Sistran.Company.Application.UniquePersonParamService;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.OperationQuotaServices;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.OperationQuotaServices;
using MOCOV1 = Sistran.Company.Application.UniquePersonServices.V1;

namespace Sistran.Company.Application.UniquePersonServices.EEProvider
{
    public class DelegateService
    {
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUniquePersonAplicationService uniquePersonAplicationService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonAplicationService>();
        internal static IUniquePersonParamServiceWeb uniquePersonParamServiceWeb = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonParamServiceWeb>();
        internal static ISarlaftApplicationServices SarlaftApplicationServices = ServiceProvider.Instance.getServiceManager().GetService<ISarlaftApplicationServices>();
        internal static IOperationQuotaService operationQuotaService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaService>();
        internal static IUniquePersonListRiskApplicationServices coreUniqueListRiskPersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskApplicationServices>();
        internal static IConsortiumIntegrationService consortiumIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IConsortiumIntegrationService>();
        internal static MOCOV1.IUniquePersonService uniquePersonServicev1 = ServiceProvider.Instance.getServiceManager().GetService<MOCOV1.IUniquePersonService>();
    }
}
