using Sistran.Company.Application.AuthorizationPoliciesServices;
using Sistran.Company.Application.CollectiveServices;
using Sistran.Company.Application.LiabilityReversionService;
using Sistran.Company.Application.Location.LiabilityServices;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.PropertyReversionService;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Company.Application.SuretyReversionService;
using Sistran.Company.Application.ThirdPartyLiabilityReversionService;
using Sistran.Company.Application.TransportReversionService;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Company.Application.VehicleReversionService;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Core.Application.ClaimServices;
using Sistran.Core.Framework.SAF;

namespace Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider
{
    using LiabilityChangeAgentService;
    using LiabilityChangeCoinsuranceService;
    using LiabilityChangePolicyHolderService;
    using SarlaftApplicationServices;
    using Sistran.Company.Application.CommonServices;
    using Sistran.Company.Application.JudicialSuretyChangeAgentService;
    using Sistran.Company.Application.JudicialSuretyChangeCoinsuranceService;
    using Sistran.Company.Application.JudicialSuretyChangePolicyHolderService;
    using Sistran.Company.Application.JudicialSuretyReversionService;
    using Sistran.Company.Application.LiabilityChangeTermService;
    using Sistran.Company.Application.OperationQuotaCompanyServices;
    using Sistran.Company.Application.UniquePersonServices.V1;
    using Sureties.JudicialSuretyServices;
    using SuretyChangeAgentService;
    using SuretyChangeCoinsuranceService;
    using SuretyChangeConsolidationService;
    using SuretyChangePolicyHolderService;
    using SuretyChangeTermService;
    using UniquePersonAplicationServices;

    public static class DelegateService
    {
        internal static readonly ICommonService commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonService>();
        internal static readonly IMassiveService MassiveService = ServiceProvider.Instance.getServiceManager().GetService<IMassiveService>();
        internal static readonly ICollectiveService CollectiveService = ServiceProvider.Instance.getServiceManager().GetService<ICollectiveService>();
        internal static readonly IUnderwritingService UnderwritingService = ServiceProvider.Instance.getServiceManager().GetService<IUnderwritingService>();
        internal static readonly IAuthorizationPoliciesService AuthorizationPoliciesService = ServiceProvider.Instance.getServiceManager().GetService<IAuthorizationPoliciesService>();
        internal static readonly IUniqueUserService UniqueUserService = ServiceProvider.Instance.getServiceManager().GetService<IUniqueUserService>();
        internal static readonly IVehicleService VehicleService = ServiceProvider.Instance.getServiceManager().GetService<IVehicleService>();
        internal static readonly IPropertyService PropertyService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyService>();
        internal static readonly ISuretyService SuretyService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyService>();
        internal static readonly ILiabilityService LiabilityService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityService>();
        internal static readonly IThirdPartyLiabilityService ThirdPartyLiabilityService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityService>();
        internal static readonly ICompanyTransportBusinessService TransportBusinessService = ServiceProvider.Instance.getServiceManager().GetService<ICompanyTransportBusinessService>();
        internal static readonly ISuretyReversionServiceCia SuretyReversionEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyReversionServiceCia>();
        internal static readonly ISuretyChangeTermServiceCompany SuretyChangeTermEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ISuretyChangeTermServiceCompany>();
        internal static readonly ICiaSuretyChangeAgentService SuretyChangeAgentEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaSuretyChangeAgentService>();
        internal static readonly ICiaSuretyChangeConsolidationService SuretyChangeConsolidationEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaSuretyChangeConsolidationService>();
        internal static readonly ICiaSuretyChangePolicyHolderService SuretyChangePolicyHolderEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaSuretyChangePolicyHolderService>();
        internal static readonly ICiaSuretyChangeCoinsuranceService SuretyChangeCoinsuranceEndorsementService = ServiceProvider.Instance.getServiceManager().GetService<ICiaSuretyChangeCoinsuranceService>();
        internal static readonly IVehicleReversionService VehicleReversionServiceCia = ServiceProvider.Instance.getServiceManager().GetService<IVehicleReversionService>();
        internal static readonly IClaimApplicationService claimService = ServiceProvider.Instance.getServiceManager().GetService<IClaimApplicationService>();
        internal static readonly IPaymentRequestApplicationService paymentRequestService = ServiceProvider.Instance.getServiceManager().GetService<IPaymentRequestApplicationService>();
        internal static readonly IPropertyReversionServiceCia PropertyReversionService = ServiceProvider.Instance.getServiceManager().GetService<IPropertyReversionServiceCia>();
        internal static readonly ILiabilityReversionServiceCia LiabilityReversionService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityReversionServiceCia>();
        internal static readonly ILiabilityChangeAgentServiceCia LiabilityChangeAgentService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityChangeAgentServiceCia>();
        internal static readonly ILiabilityChangeCoinsuranceServiceCia LiabilityChangeCoinsuranceService = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityChangeCoinsuranceServiceCia>();
        internal static readonly ICiaLiabilityChangePolicyHolderService LiabilityChangePolicyHolderService = ServiceProvider.Instance.getServiceManager().GetService<ICiaLiabilityChangePolicyHolderService>();
        internal static readonly ILiabilityChangeTermServiceCompany liabilityChangeTermServiceCompany = ServiceProvider.Instance.getServiceManager().GetService<ILiabilityChangeTermServiceCompany>();
        internal static readonly IThirdPartyLiabilityReversionServiceCia ThirdPartyLiabilityReversionService = ServiceProvider.Instance.getServiceManager().GetService<IThirdPartyLiabilityReversionServiceCia>();
        internal static readonly ITransportReversionService TransportReversionService = ServiceProvider.Instance.getServiceManager().GetService<ITransportReversionService>();
        internal static readonly IUniquePersonAplicationService PersonAplicationService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonAplicationService>();
        internal static readonly IUniquePersonService PersonAplicationServiceV1 = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonService>();
        internal static readonly ISarlaftApplicationServices SarlaftApplicationService = ServiceProvider.Instance.getServiceManager().GetService<ISarlaftApplicationServices>();
        internal static readonly IJudicialSuretyService JudicialSuretyService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyService>();
        internal static readonly IJudicialSuretyReversionServiceCompany JudicialSuretyReversionService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyReversionServiceCompany>();
        internal static readonly ICiaJudicialSuretyChangePolicyHolderService judicialSuretyChangePolicyHolderService = ServiceProvider.Instance.getServiceManager().GetService<ICiaJudicialSuretyChangePolicyHolderService>();
        internal static readonly IJudicialSuretyChangeAgentServiceCompany judicialSuretyChangeAgentService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyChangeAgentServiceCompany>();
        internal static readonly IJudicialSuretyChangeCoinsuranceServiceCompany judicialSuretyChangeCoinsuranceService = ServiceProvider.Instance.getServiceManager().GetService<IJudicialSuretyChangeCoinsuranceServiceCompany>();
        internal static readonly IOperationQuotaCompanyService operationQuotaCompanyService = ServiceProvider.Instance.getServiceManager().GetService<IOperationQuotaCompanyService>();
    }
}
