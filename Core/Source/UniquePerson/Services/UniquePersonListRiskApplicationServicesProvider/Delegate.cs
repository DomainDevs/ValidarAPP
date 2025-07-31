using Sistran.Core.Application.UniquePersonListRiskBusinessService;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonListRiskApplicationServicesProvider
{
   public  class Delegate
    {
        internal static IUniquePersonListRiskBusinessService uniquePersonListRiskBusinessService  = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskBusinessService>();
    }
}
