using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.Sureties.JudicialSuretyServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.MassiveServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Company.Application.CollectionFormBusinessService;
using Sistran.Company.Application.UniquePersonServices;
using Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Core.Application.PrintingServices;

namespace Sistran.Company.Application.PrintingServicesEEProvider
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        //internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static IMassiveServiceCore massiveServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IMassiveServiceCore>();
        internal static IJudicialSuretyService juditialSuretyService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyService>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        //internal static IThirdPartyLiabilityService thirdPartyLiabilityService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
        internal static ILiabilityService liabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IProductService productService = ServiceProvider.Instance.getServiceManager().GetService<IProductService>();
        internal static ICompanyCollectionFormBusinessService collectionFormBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyCollectionFormBusinessService>();
        internal static IUniquePersonService uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static IPrintingServiceCore printingServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IPrintingServiceCore>();

    }
}