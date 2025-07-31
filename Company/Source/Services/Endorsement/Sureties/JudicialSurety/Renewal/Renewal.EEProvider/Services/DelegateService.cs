using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Sureties.JudicialSuretyServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.JudicialSuretyRenewalService.EEProvider.Services
{
    class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IJudicialSuretyService JudicialsuretyService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();



    }
}
