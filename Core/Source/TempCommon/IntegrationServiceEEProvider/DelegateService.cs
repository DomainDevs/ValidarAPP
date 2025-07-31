using Sistran.Core.Application.TempCommonServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.TempCommon.EEProvider
{
    public class DelegateService
    {
        public static readonly ITempCommonService tempCommonService = ServiceProvider.Instance.getServiceManager().GetService<ITempCommonService>();
    }
}
