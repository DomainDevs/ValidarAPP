using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.AdjustmentEndorsement.EEProvider
{
   public class DelegateService
    {
        internal static IBaseEndorsementService endorsementBaseService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();

    }
}
