using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.Sureties.EEProvider
{
    public static class DelegateService
    {
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
    }
}
