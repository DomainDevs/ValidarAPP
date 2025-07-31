using Sistran.Core.Application.AuthorizationPoliciesServices;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Application.UniqueUserServices;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Integration.AircraftServices;
using Sistran.Core.Integration.FidelityServices;
using Sistran.Core.Integration.SuretyServices;
using Sistran.Core.Integration.TransportServices;
using Sistran.Core.Integration.VehicleServices;
using Sistran.Core.Integration.MarineServices;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Integration.PropertyServices;
using Sistran.Core.Application.GeneralLedgerServices;
using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices;
using Sistran.Core.Application.OperationQuotaServices;
using Sistran.Core.Integration.UndewritingIntegrationServices;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices;
using Sistran.Core.Integration.ClaimsReinsuranceWorkerServices;

namespace Sistran.Core.Application.ClaimServices.EEProvider
{
    public static class DelegateService
    {
        internal static IAuthorizationPoliciesServiceCore authorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesServiceCore>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUtilitiesServiceCore>();
        internal static ICommonServiceCore commonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        internal static IUndewritingIntegrationServices underwritingIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IUndewritingIntegrationServices>();
        internal static IAccountingApplicationService generalLedgerService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingApplicationService>();
        internal static IUniqueUserServiceCore uniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserServiceCore>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();
        internal static IVehicleIntegrationService vehicleIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleIntegrationService>();
        internal static ITransportIntegrationService transportIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<ITransportIntegrationService>();
        internal static ISuretyIntegrationService suretyIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyIntegrationService>();
        internal static ITaxService taxServices = ServiceProvider.Instance.getServiceManager().GetService<ITaxService>();
        internal static IFidelityIntegrationService fidelityIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IFidelityIntegrationService>();
        internal static IAircraftIntegrationService aircraftIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IAircraftIntegrationService>();
        internal static IMarineIntegrationService marineIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IMarineIntegrationService>();
        internal static IPropertyIntegrationService propertyIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyIntegrationService>();
        internal static IClaimsGeneralLedgerWorkerIntegrationService claimsWorkerIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<IClaimsGeneralLedgerWorkerIntegrationService>();
        internal static IConsortiumService consortiumService = ServiceProvider.Instance.getServiceManager().GetService<IConsortiumService>();
        internal static ITechnicalTransactionIntegrationService technicalTransactionIntegrationService = ServiceProvider.Instance.getServiceManager().GetService<ITechnicalTransactionIntegrationService>();
        internal static IUniquePersonListRiskApplicationServices coreUniqueListRiskPersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonListRiskApplicationServices>();
        internal static IClaimsReinsuranceWorkerIntegrationServices claimsReinsuranceWorkerIntegrationServices = ServiceProvider.Instance.getServiceManager().GetService<IClaimsReinsuranceWorkerIntegrationServices>();
        internal static IClaimApplicationService claimApplicationService = ServiceProvider.Instance.getServiceManager().GetService<IClaimApplicationService>();
    }
}
