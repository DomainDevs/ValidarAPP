using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Services
{
    public class DelegateService
    {
        
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        
    }
}
