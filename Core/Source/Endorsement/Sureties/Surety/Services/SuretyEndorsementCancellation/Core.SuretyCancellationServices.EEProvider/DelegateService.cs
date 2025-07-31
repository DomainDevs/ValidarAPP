
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Application.BaseEndorsementService;


namespace Sistran.Core.Application.SuretyEndorsementCancellationService.EEProvider
{
    public class DelegateService
    {
        internal static IBaseEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
        internal static IUnderwritingServiceCore underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingServiceCore>();
        
    }
}