using Sistran.Core.Application.Marines.MarineBusinessService;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.MarineServices.EEProvider
{
    public class DelegateService
    {
        internal static IMarineBusinessService marineBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IMarineBusinessService>();

    }
}
