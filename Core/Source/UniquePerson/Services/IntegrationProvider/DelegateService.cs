//using Sistran.Core.Framework.SAF;

using Sistran.Core.Application.UniquePersonService;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Application.UniquePerson.IntegrationService.Provider
{
    public class DelegateService
    {
        internal static IUniquePersonServiceCore uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
    }
}