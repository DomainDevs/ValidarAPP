using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.PrintingServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.PrintingServicesNase
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IPrintingServiceCore printingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IPrintingServiceCore>();

    }
}
