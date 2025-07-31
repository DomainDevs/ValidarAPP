using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Company.Application.SuretyModificationService;
using Sistran.Company.Application.ProductServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.ChangePolicyHolderEndorsement;
using Sistran.Company.Application.SuretyCancellationService;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.UniquePersonService.V1;

namespace Sistran.Company.Application.SuretyChangePolicyHolderService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICiaChangePolicyHolderEndorsement ciaChangePolicyHolderEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangePolicyHolderEndorsement>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static ISuretyModificationServiceCia suretyModificationService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyModificationServiceCia>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static ISuretyCancellationServiceCia endorsementSuretyCancellationService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyCancellationServiceCia>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
    }
}
