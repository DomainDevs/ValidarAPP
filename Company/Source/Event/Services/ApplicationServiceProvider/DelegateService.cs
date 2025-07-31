using Sistran.Company.Application.Event.BusinessService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.Event.ApplicationServices.EEProvider
{
    public class DelegateService
    {
        internal static ICompanyEventBusinessService _EventBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyEventBusinessService>();
    }
}
