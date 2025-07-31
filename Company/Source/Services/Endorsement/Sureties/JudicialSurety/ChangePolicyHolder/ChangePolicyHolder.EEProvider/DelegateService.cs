using Sistran.Company.Application.BaseEndorsementService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Sureties.JudicialSuretyServices;
using Sistran.Company.Application.JudicialSuretyCancellationService;
using Sistran.Company.Application.ProductServices;
using Sistran.Core.Framework.SAF;
using Sistran.Company.Application.ChangePolicyHolderEndorsement;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Company.Application.JudicialSuretyModificationService;
using Sistran.Core.Application.UniquePersonService.V1;

namespace Sistran.Company.Application.JudicialSuretyChangePolicyHolderService.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICiaChangePolicyHolderEndorsement ciaChangePolicyHolderEndorsement = ServiceProvider.Instance.getServiceManager().GetService<ICiaChangePolicyHolderEndorsement>();
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IJudicialSuretyModificationServiceCompany suretyModificationService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyModificationServiceCompany>();
        internal static IJudicialSuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static IBaseCiaEndorsementService baseEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<IBaseCiaEndorsementService>();
        internal static IJudicialSuretyCancellationServiceCompany endorsementSuretyCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyCancellationServiceCompany>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
    }
}
