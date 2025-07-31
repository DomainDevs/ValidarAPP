using Sistran.Company.Application.AdjustmentBusinessService;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AdjustmentApplicationServiceEEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IAdjustmentBusinessService adjustmentBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IAdjustmentBusinessService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
    }
}
