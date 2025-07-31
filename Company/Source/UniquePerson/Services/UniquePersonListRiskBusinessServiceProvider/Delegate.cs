using Sistran.Core.Application.UniquePersonListRiskBusinessService;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider
{
    public class Delegate
    {
        internal static IUtilitiesServiceCore utilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonListRiskBusinessService uniquePersonListRiskBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskBusinessService>();
    }
}
