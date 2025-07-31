using Sistran.Company.Application.ChangeCoInsuranceEndorsement;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.LiabilityCancellationService;
using Sistran.Company.Application.LiabilityModificationService;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.LiabilityChangeCoinsuranceService.EEProvider
{
    public static class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ILiabilityService LibialityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ILiabilityModificationServiceCia libialityModificationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityModificationServiceCia>();
        internal static ICiaChangeCoinsuranceEndorsement ciaChangeCoinsuranceEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangeCoinsuranceEndorsement>();
        internal static ILiabilityCancellationServiceCia endorsementlibialityCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityCancellationServiceCia>();
    }
}
