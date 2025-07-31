using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.Location.PropertyServices;
//using Sistran.Company.Application.MassiveLiabilityCancellationService;
//using Sistran.Company.Application.MassivePropertyCancellationService;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.MassiveTPLCancellationServices;
using Sistran.Company.Application.MassiveUnderwritingServices;
using Sistran.Company.Application.MassiveVehicleCancellationService;
using Sistran.Company.Application.PrintingServices;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.MassiveServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Services.UtilitiesServices;
using System;
using System.Reflection;

namespace Sistran.Company.Application.CancellationMsvEndorsementServicesServices.EEProvider
{
    public class DelegateService
    {

        internal static IUnderwritingService underwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static ICommonServiceCore commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static ICommonService commonServiceCompany = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static IRulesService rulesService = ServiceProvider.Instance.getServiceManager().GetService<IRulesService>();
        internal static IMassiveService massiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveUnderwritingService>();
        internal static IMassiveVehicleCancellationService massiveVehicleCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveVehicleCancellationService>();
        internal static IBaseEndorsementService endorsementBaseService = ServiceProvider.Instance.getServiceManager().GetService<IBaseEndorsementService>();
        //internal static IMassivePropertyCancellationService massivePropertyCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IMassivePropertyCancellationService>();
        //internal static IMassiveLiabilityCancellationService massiveLiabilityCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveLiabilityCancellationService>();
        internal static IVehicleService vehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static IPropertyService propertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static ILiabilityService liabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static IThirdPartyLiabilityService tplService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
        internal static IPrintingService printingService = ServiceProvider.Instance.getServiceManager().GetService<IPrintingService>();
        internal static IMassiveTPLCancellationServices massiveTplCancellationService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveTPLCancellationServices>();
        internal static IAuthorizationPoliciesServiceCore AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IUtilitiesServiceCore UtilitiesService = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
    }
}