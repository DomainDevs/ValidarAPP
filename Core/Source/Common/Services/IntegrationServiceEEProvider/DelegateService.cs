using Sistran.Core.Application.CommonService;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.CommonServices.EEProvider
{
    public class DelegateService
    {
        public static readonly ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();

    }
}
