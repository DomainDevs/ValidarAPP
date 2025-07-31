using Sistran.Core.Application.Location.PropertyServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Integration.PropertyServices.EEProvider
{
    public static class DelegateService
    {
        internal static IPropertyServiceCore propertyServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IPropertyServiceCore>();
    }
}
