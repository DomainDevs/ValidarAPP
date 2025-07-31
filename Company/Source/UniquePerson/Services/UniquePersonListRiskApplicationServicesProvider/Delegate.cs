using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices;
using Sistran.Company.Application.UniquePersonListRiskBusinessService;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServicesProvider
{
   public  class Delegate
    {
        internal static IUniquePersonListRiskBusinessService uniquePersonListRiskBusinessService  = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskBusinessService>();

        internal static IUniquePersonListRiskApplicationServices coreUniquePersonListRiskApplicationServices = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskApplicationServices>();

        
    }
}
