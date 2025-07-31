using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.UnderwritingServices;

namespace Sistran.Company.Application.ApplicationServiceProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
    }
}
