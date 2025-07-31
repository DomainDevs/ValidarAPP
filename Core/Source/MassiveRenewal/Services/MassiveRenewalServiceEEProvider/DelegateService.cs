using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.MassiveServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider
{
    class DelegateService
    {
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        internal static IMassiveServiceCore massiveServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IMassiveServiceCore>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
    }
}
