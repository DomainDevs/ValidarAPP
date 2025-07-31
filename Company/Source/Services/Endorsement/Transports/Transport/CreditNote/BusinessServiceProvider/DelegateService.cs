using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider
{
    public class DelegateService
    {
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ICompanyTransportBusinessService transportService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportBusinessService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
    }
}
