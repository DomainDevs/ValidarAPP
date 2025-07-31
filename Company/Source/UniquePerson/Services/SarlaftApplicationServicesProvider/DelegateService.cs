using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.SarlaftBusinessServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Company.Application.UniquePersonServices.V1;

namespace Sistran.Company.Application.SarlaftApplicationServicesProvider
{
    using SarlaftApplicationServices;
    using Sistran.Core.Application.UniquePersonListRiskApplicationServices;

    /// <summary>
    ///  DelegateService. Conector de Servicios
    /// </summary>
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static ISarlaftBusinessServices sarlaftBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ISarlaftBusinessServices>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static ISarlaftApplicationServices SarlaftApplicationServices = ServiceProvider.Instance.getServiceManager().GetService<ISarlaftApplicationServices>();
        internal static IUniquePersonListRiskApplicationServices coreUniqueListRiskPersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskApplicationServices>();
    }
}
