using Sistran.Core.Application.ProductServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.ProductServices.EEProvider
{
    class DelegateService
    {
        internal static IProductServiceCore productServicecore = ServiceProvider.Instance.getServiceManager().GetService<IProductServiceCore>();
    }
}
