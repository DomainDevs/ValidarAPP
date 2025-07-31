using Sistran.Core.Application.Finances.FidelityServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Integration.FidelityService.EEProvider
{
    public static class DelegateService
    {
        internal static IFidelityServiceCore fidelityServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IFidelityServiceCore>();
    }
}
