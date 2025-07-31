using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.QuotationServices;
using Sistran.Company.Application.Sureties.JudicialSuretyServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.AuditServices;
using Sistran.Core.Application.Cache.CacheBusinessService;
using Sistran.Core.Application.UniquePersonListRiskBusinessService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.WrapperServices.EEProvider
{
    public class DelegateService
    {
        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static IQuotationService quotationService = ServiceProvider.Instance.getServiceManager().GetService<IQuotationService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static ILiabilityService liabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static ISuretyService suretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static IThirdPartyLiabilityService thirdPartyLiabilityService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
        internal static IJudicialSuretyService judicialSuretyService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyService>();
        internal static IMassiveVehicleService massiveVehicleService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveVehicleService>();
        internal static ICacheBusinessService cacheBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICacheBusinessService>();
        internal static IAuditService auditService = ServiceProvider.Instance.getServiceManager().GetService<IAuditService>();
        internal static IUniquePersonListRiskBusinessService listRiskBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskBusinessService>();
    }
}