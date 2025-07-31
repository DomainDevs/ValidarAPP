using Sistran.Company.Application.ChangeCoInsuranceEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Company.Application.SuretyCancellationService;
using Sistran.Company.Application.SuretyModificationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SuretyChangeCoinsuranceService.EEProvider
{
    public static class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ISuretyModificationServiceCia suretyModificationService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyModificationServiceCia>();
        internal static ICiaChangeCoinsuranceEndorsement ciaChangeCoinsuranceEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeCoinsuranceEndorsement>();
        internal static ISuretyCancellationServiceCia endorsementSuretyCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyCancellationServiceCia>();
    }
}
