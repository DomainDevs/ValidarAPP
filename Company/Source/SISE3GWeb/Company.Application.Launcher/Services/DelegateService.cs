using Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService;
using Sistran.Company.Application.AuthorizationPoliciesServices;
using Sistran.Company.Application.CancellationMsvEndorsementServices;
using Sistran.Company.Application.CollectionFormBusinessService;
using Sistran.Company.Application.CollectiveServices;
using Sistran.Company.Application.CommonParamService;
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.ExternalProxyServices;
//using Sistran.Company.Application.Location.CollectivePropertyRenewalService;
//using Sistran.Company.Application.Location.LiabilityCollectiveService;
using Sistran.Company.Application.Location.LiabilityServices;
//using Sistran.Company.Application.Location.MassiveLiabilityServices;
//using Sistran.Company.Application.Location.MassivePropertyServices;
//using Sistran.Company.Application.Location.PropertyCollectiveService;
//using Sistran.Company.Application.Location.PropertyModificationService;
using Sistran.Company.Application.Location.PropertyServices;
using Sistran.Company.Application.MassiveRenewalServices;
using Sistran.Company.Application.MassiveServices;
using Sistran.Company.Application.MassiveTPLCancellationServices;
using Sistran.Company.Application.MassiveUnderwritingServices;
using Sistran.Company.Application.PrintingServices;
using Sistran.Company.Application.ProductParamService;
using Sistran.Company.Application.ProductServices;
using Sistran.Company.Application.QuotationServices;
using Sistran.Company.Application.ReversionEndorsement;
using Sistran.Company.Application.SarlaftApplicationServices;
using Sistran.Company.Application.SarlaftBusinessServices;
using Sistran.Company.Application.Sureties.JudicialSuretyServices;
using Sistran.Company.Application.Sureties.SuretyServices;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices;
using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices;
using Sistran.Company.Application.Transports.TransportApplicationService;
//using Sistran.Company.Application.QuotationBusinessServices;
/* --------------------- */
using Sistran.Company.Application.UnderwritingApplicationService;
using Sistran.Company.Application.UnderwritingParamApplicationService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UniquePersonAplicationServices;
using Sistran.Company.Application.UniquePersonParamService;
using Sistran.Company.Application.UniqueUserApplicationServices;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Company.Application.Vehicle.ModificationService;
using Sistran.Company.Application.VehicleClauseService;
using Sistran.Company.Application.VehicleEndorsementRenewalService;
using Sistran.Company.Application.VehicleModificationService;
using Sistran.Company.Application.Vehicles.CollectiveTPLModificationService;
using Sistran.Company.Application.Vehicles.CollectiveTPLRenewalService;
using Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalService;
using Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices;
//using Sistran.Company.Application.Vehicles.MassiveThirdPartyLiabilityServices;
using Sistran.Company.Application.Vehicles.MassiveVehicleServices;
using Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService;
using Sistran.Company.Application.Vehicles.TPLCollectiveServices;
using Sistran.Company.Application.Vehicles.VehicleApplicationService;
using Sistran.Company.Application.Vehicles.VehicleCollectiveServices;
using Sistran.Company.Application.Vehicles.VehicleServices;
using Sistran.Company.Application.VehicletExtensionService;
using Sistran.Company.Application.VehicleTextService;
using Sistran.Core.Application.AuditServices;
using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Application.Cache.CacheBusinessService;
using Sistran.Core.Application.EventsServices;
using Sistran.Core.Application.MassiveUnderwritingServices;
using Sistran.Core.Application.RulesScriptsServices;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Framework.UIF2.Services;
using AUTHEPROVIDER = Sistran.Core.Application.AuthenticationServices;
using COAUTHOSERVICE = Sistran.Company.Application.AuthorizationPoliciesParamService;
using COJSCAN = Sistran.Company.Application.JudicialSuretyCancellationService;
using COJSCHA = Sistran.Company.Application.JudicialSuretyChangeAgentService;
using COJSCOI = Sistran.Company.Application.JudicialSuretyChangeCoinsuranceService;
using COJSCLAU = Sistran.Company.Application.JudicialSuretyClauseService;
using COJSEXT = Sistran.Company.Application.JudicialSuretytExtensionService;
using COJSMODF = Sistran.Company.Application.JudicialSuretyModificationService;
using COJSREV = Sistran.Company.Application.JudicialSuretyReversionService;
using COJSTEXT = Sistran.Company.Application.JudicialSuretyTextService;
using COMSURREN = Sistran.Company.Application.JudicialSuretyRenewalService;
using COJSPH = Sistran.Company.Application.JudicialSuretyChangePolicyHolderService;
using COMLIACANC = Sistran.Company.Application.LiabilityCancellationService;
using COMLIACHANGE = Sistran.Company.Application.LiabilityChangeAgentService;
using COMLIACHANGECOI = Sistran.Company.Application.LiabilityChangeCoinsuranceService;
using COMLIAPH = Sistran.Company.Application.LiabilityChangePolicyHolderService;
using COMLIACLAU = Sistran.Company.Application.LiabilityClauseService;
using COMLIAEXT = Sistran.Company.Application.LiabilitytExtensionService;
using COMLIAMODF = Sistran.Company.Application.LiabilityModificationService;
using COMLIAREN = Sistran.Company.Application.LiabilityRenewalService;
using COMLIAREVER = Sistran.Company.Application.LiabilityReversionService;
using COMLIATEX = Sistran.Company.Application.LiabilityTextService;
using COMLIACT = Sistran.Company.Application.LiabilityChangeTermService;
using COMPROCANC = Sistran.Company.Application.PropertyCancellationService;
using COMPROCHA = Sistran.Company.Application.PropertyChangeAgentService;
using COMPROCLA = Sistran.Company.Application.PropertyClauseService;
using COMPROEXT = Sistran.Company.Application.PropertytExtensionService;
using COMPROMOD = Sistran.Company.Application.PropertyModificationService;
using COMPROREN = Sistran.Company.Application.PropertyEndorsementRenewalService;
using COMPROREV = Sistran.Company.Application.PropertyReversionService;
using COMPROTEX = Sistran.Company.Application.PropertyTextService;
using COMSURCANC = Sistran.Company.Application.SuretyCancellationService;
using COMSURCHA = Sistran.Company.Application.SuretyChangeAgentService;
using COMSURCOI = Sistran.Company.Application.SuretyChangeCoinsuranceService;
using COMSURCLA = Sistran.Company.Application.SuretyClauseService;
using COMSURPH = Sistran.Company.Application.SuretyChangePolicyHolderService;
using COMSUCONC = Sistran.Company.Application.SuretyChangeConsolidationService;
using COMSUREXT = Sistran.Company.Application.SuretytExtensionService;
using COMSURMOD = Sistran.Company.Application.SuretyModificationService;
using COMSURREV = Sistran.Company.Application.SuretyReversionService;
using COMSURTEM = Sistran.Company.Application.SuretyChangeTermService;
using COMSURTEX = Sistran.Company.Application.SuretyTextService;
using COMVEHICANC = Sistran.Company.Application.VehicleCancellationService;
using COMVEHICHANGE = Sistran.Company.Application.VehicleChangeAgentService;
using COMVEHIREVER = Sistran.Company.Application.VehicleReversionService;
using COTPLCAN = Sistran.Company.Application.ThirdPartyLiabilityCancellationService;
using COTPLCHA = Sistran.Company.Application.ThirdPartyLiabilityChangeAgentService;
using COTPLCLAU = Sistran.Company.Application.ThirdPartyLiabilityClauseService;
using COTPLEXT = Sistran.Company.Application.ThirdPartyLiabilitytExtensionService;
using COTPLMODF = Sistran.Company.Application.ThirdPartyLiabilityModificationService;
using COTPLREN = Sistran.Company.Application.ThirdPartyLiabilityEndorsementRenewalService;
using COTPLREV = Sistran.Company.Application.ThirdPartyLiabilityReversionService;
using COTPLTEXT = Sistran.Company.Application.ThirdPartyLiabilityTextService;
using UniqueUserParamCompany = Sistran.Company.Application.UniqueUserParamService;
using UNPARAM = Sistran.Core.Application.UnderwritingParamService;
using VEHPARAM = Sistran.Core.Application.VehicleParamService;
using ENSERVICE = Sistran.Core.Application.EntityServices;
using PRDSERV = Sistran.Core.Application.ProductServices;


using TRANSCAN = Sistran.Company.Application.TransportCancellationService;
using PARCOMPANY = Sistran.Company.Application.ParametrizationAplicationServices;

using V1 = Sistran.Company.Application.UniquePersonServices.V1;
using Sistran.Company.Application.Event.ApplicationService;
using Sistran.Company.Application.TransportExtensionService;
using Sistran.Company.Application.TransportCancellationService;
using Sistran.Company.Application.TransportReversionService;
using Sistran.Company.Application.UniquePersonListRiskApplicationServices;
using AIRCAN = Sistran.Company.Application.AircraftCancellationService;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService;
using AIRMOD = Sistran.Company.Application.AircraftModificationService;
using Sistran.Company.Application.AircraftChangeAgentService;
using Sistran.Company.Application.AircraftClauseService;
using Sistran.Company.Application.AircraftReversionService;
using Sistran.Company.Application.TransportEndorsementRenewalService;
using Sistran.Company.Application.TransportModificationService;
using AIRSEX = Sistran.Company.Application.AircraftExtensionService;
using AIRREW = Sistran.Company.Application.AircraftEndorsementRenewalService;
using AIRTEXT = Sistran.Company.Application.AircraftTextService;
using Sistran.Company.Application.TransportTextService;
using Sistran.Company.Application.TransportClauseService;
using Sistran.Company.Application.TransportChangeAgentService;
using Sistran.Company.Application.DeclarationApplicationService;
using Sistran.Company.Application.Marines.MarineBusinessService;
using Sistran.Company.Application.AdjustmentApplicationService;
using Sistran.Company.Application.Adjustment;
using Sistran.Company.Application.Declaration;
using Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationService;
using Sistran.Company.Application.Transports.TransportBusinessService;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService;
using Sistran.Company.Application.Marines.MarineApplicationService;
using MARSEX = Sistran.Company.Application.MarineExtensionService;
using MARREW = Sistran.Company.Application.MarineEndorsementRenewalService;
using MARTEXT = Sistran.Company.Application.MarineTextService;
using MARCAN = Sistran.Company.Application.MarineCancellationService;
using Sistran.Company.Application.MarineReversionService;
using Sistran.Company.Application.MarineChangeAgentService;
using MARMOD = Sistran.Company.Application.MarineModificationService;
using Sistran.Company.Application.MarineClauseService;
using Sistran.Company.Application.Finances.FidelityServices;
using Sistran.Company.Application.FidelityTextService;
using Sistran.Company.Application.Aircrafts.AircraftApplicationService;
using Sistran.Company.Application.FidelityModificationService;
using Sistran.Company.Application.FidelityClauseService;
using COMFIACHANGE = Sistran.Company.Application.FidelityChangeAgentService;
using COMFIACANC = Sistran.Company.Application.FidelityCancellationService;
using GL = Sistran.Core.Application.GeneralLedgerServices;
using Sistran.Core.Application.ClaimServices;
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.TempCommonServices;
using Sistran.Core.Application.AutomaticDebitServices;
using Sistran.Core.Application.ReportingServices;
using Sistran.Core.Application.ReconciliationServices;
using Sistran.Core.Application.ReinsuranceServices;
using Sistran.Core.Application.AccountingClosingServices;
using Sistran.Core.Integration.TempCommonService;
using Sistran.Core.Integration.UniqueUserServices;
using Sistran.Core.Integration.CommonServices;
using Sistran.Core.Integration.AccountingServices;
using Sistran.Company.Application.SuretyEndorsementRenewalService;
using Sistran.Core.Application.OperationQuotaServices;
using Sistran.Core.Application.UnderwritingServices;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Services.UtilitiesServices;
using Sistran.Core.Application.FinancialPlanServices;
using Sistran.Company.Application.OperationQuotaCompanyServices;

namespace Sistran.Core.Framework.UIF.Web.Services
{
    public class DelegateService
    {
        internal static ICommonService commonService = ServiceManager.Instance.GetService<ICommonService>();
        internal static ICommonIntegrationServiceCore CommonIntegrationServiceCore = ServiceManager.Instance.GetService<ICommonIntegrationServiceCore>();
        internal static ICollectiveService collectiveService = ServiceManager.Instance.GetService<ICollectiveService>();
        internal static IConceptsService conceptsService = ServiceManager.Instance.GetService<IConceptsService>();
        internal static IBaseEndorsementService endorsementBaseService = ServiceManager.Instance.GetService<IBaseEndorsementService>();
        internal static ICiaReversionEndorsement ReversionEndorsementService = ServiceManager.Instance.GetService<ICiaReversionEndorsement>();
        internal static IEventServiceCore eventsService = ServiceManager.Instance.GetService<IEventServiceCore>();
        internal static ILiabilityService liabilityService = ServiceManager.Instance.GetService<ILiabilityService>();
        internal static IMassiveService massiveService = ServiceManager.Instance.GetService<IMassiveService>();
        internal static IMassiveRenewalService massiveRenewalService = ServiceManager.Instance.GetService<IMassiveRenewalService>();
        internal static IMassiveUnderwritingService massiveUnderwritingService = ServiceManager.Instance.GetService<IMassiveUnderwritingService>();
        internal static IMassiveVehicleService massiveVehicleService = ServiceManager.Instance.GetService<IMassiveVehicleService>();
        //internal static IMassiveLiabilityService massiveliabilityService = ServiceManager.Instance.GetService<IMassiveLiabilityService>();
        //internal static IMassivePropertyService massivepropertyService = ServiceManager.Instance.GetService<IMassivePropertyService>();
        internal static IPrintingService printingService = ServiceManager.Instance.GetService<IPrintingService>();
        internal static IPropertyService propertyService = ServiceManager.Instance.GetService<IPropertyService>();
        internal static IQuotationService quotationService = ServiceManager.Instance.GetService<IQuotationService>();
        internal static IRulesService rulesEditorServices = ServiceManager.Instance.GetService<IRulesService>();
        internal static IScriptsService scriptsService = ServiceManager.Instance.GetService<IScriptsService>();
        internal static ISuretyService suretyService = ServiceManager.Instance.GetService<ISuretyService>();
        internal static ITaxService taxService = ServiceManager.Instance.GetService<ITaxService>();
        internal static IThirdPartyLiabilityService thirdPartyLiabilityService = ServiceManager.Instance.GetService<IThirdPartyLiabilityService>();
        internal static Integration.UndewritingIntegrationServices.IUndewritingIntegrationServices underwritingIntegrationService = ServiceManager.Instance.GetService<Integration.UndewritingIntegrationServices.IUndewritingIntegrationServices>();
        internal static IUnderwritingService underwritingService = ServiceManager.Instance.GetService<IUnderwritingService>();
        internal static IUnderwritingServiceCore underwritingServiceCore = ServiceManager.Instance.GetService<IUnderwritingService>();
        internal static V1.IUniquePersonService uniquePersonService = ServiceManager.Instance.GetService<V1.IUniquePersonService>();
        internal static V1.IUniquePersonService uniquePersonServiceV1 = ServiceManager.Instance.GetService<V1.IUniquePersonService>();
        internal static IUniquePersonServiceCore uniquePersonServiceCore = ServiceManager.Instance.GetService<IUniquePersonServiceCore>();

        internal static IUniquePersonAplicationService uniquePersonAplicationService = ServiceManager.Instance.GetService<IUniquePersonAplicationService>();
        internal static IUniqueUserService uniqueUserService = ServiceManager.Instance.GetService<IUniqueUserService>();
        internal static IUniqueUserApplicationService UniqueUserApplicationServices = ServiceManager.Instance.GetService<IUniqueUserApplicationService>();
        internal static IUniqueUserIntegrationService UniqueUserIntegrationService = ServiceManager.Instance.GetService<IUniqueUserIntegrationService>();
        internal static IVehicleService vehicleService = ServiceManager.Instance.GetService<IVehicleService>();
        internal static IMassiveThirdPartyLiabilityService massiveThirdPartyLiabilityService = ServiceManager.Instance.GetService<IMassiveThirdPartyLiabilityService>();
        internal static IJudicialSuretyService judicialSuretyService = ServiceManager.Instance.GetService<IJudicialSuretyService>();

        internal static ICancellationMassiveEndorsementServices CancellationMassiveEndorsementServices = ServiceManager.Instance.GetService<ICancellationMassiveEndorsementServices>();
        internal static IAuthorizationPoliciesService AuthorizationPoliciesService = ServiceManager.Instance.GetService<IAuthorizationPoliciesService>();
        internal static AUTHEPROVIDER.IAuthenticationProviders AuthenticationProviders = ServiceManager.Instance.GetService<AUTHEPROVIDER.IAuthenticationProviders>();
        internal static IMassiveUnderwritingServiceCore MassiveServiceCore = ServiceManager.Instance.GetService<IMassiveUnderwritingServiceCore>();
        internal static IUtilitiesServiceCore utilitiesServiceCore = ServiceManager.Instance.GetService<IUtilitiesServiceCore>();
        internal static IProductService productService = ServiceManager.Instance.GetService<IProductService>();
        internal static IUniquePersonParamServiceWeb companyUniquePersonParamService = ServiceManager.Instance.GetService<IUniquePersonParamServiceWeb>();
        internal static ICommonParamServiceWeb companyCommonParamService = ServiceManager.Instance.GetService<ICommonParamServiceWeb>();
        internal static COAUTHOSERVICE.IAuthorizationPoliciesParamServiceWeb AuthorizationPoliciesParamService = ServiceManager.Instance.GetService<COAUTHOSERVICE.IAuthorizationPoliciesParamServiceWeb>();
        internal static UNPARAM.IUnderwritingParamServiceWeb UnderwritingParamServiceWeb = ServiceManager.Instance.GetService<UNPARAM.IUnderwritingParamServiceWeb>();
        internal static UniqueUserParamCompany.IUniqueUserParamServiceWeb companyUniqueUserParamService = ServiceManager.Instance.GetService<UniqueUserParamCompany.IUniqueUserParamServiceWeb>();
        internal static ENSERVICE.IEntityService EntityServices = ServiceManager.Instance.GetService<ENSERVICE.IEntityService>();
        internal static VEHPARAM.IVehicleParamServiceWeb VehicleParamServices = ServiceManager.Instance.GetService<VEHPARAM.IVehicleParamServiceWeb>();
        internal static ICompanyVehicleApplicationService vehicleApplicationService = ServiceManager.Instance.GetService<ICompanyVehicleApplicationService>();
        internal static ICompanyUnderwritingParamApplicationService CompanyUnderwritingParamApplicationService = ServiceManager.Instance.GetService<ICompanyUnderwritingParamApplicationService>();
        //internal static IPrintingParamServiceWeb PrintingParamServiceWeb = ServiceManager.Instance.GetService<IPrintingParamServiceWeb>();
        internal static ICiaAdjustmentEndorsement adjustmentEndorsement = ServiceManager.Instance.GetService<ICiaAdjustmentEndorsement>();
        internal static ICiaDeclarationEndorsement declarationEndorsement = ServiceManager.Instance.GetService<ICiaDeclarationEndorsement>();
        internal static IBaseCreditNoteApplicationService creditNoteApplicationService = ServiceManager.Instance.GetService<IBaseCreditNoteApplicationService>();

        internal static PRDSERV.IProductServiceCore coreProductService = ServiceManager.Instance.GetService<PRDSERV.IProductServiceCore>();
        //internal static PRDPARMSERV.IProductParamServiceCore coreProductParamService = ServiceManager.Instance.GetService<PRDPARMSERV.IProductParamServiceCore>();

        #region Colectivas
        internal static IVehicleCollectiveService vehicleCollectiveService = ServiceManager.Instance.GetService<IVehicleCollectiveService>();
        //internal static ILiabilityCollectiveService liabilityCollectiveService = ServiceManager.Instance.GetService<ILiabilityCollectiveService>();
        //internal static IPropertyCollectiveService propertyCollectiveService = ServiceManager.Instance.GetService<IPropertyCollectiveService>();
        internal static IThirdPartyLiabilityCollectiveService thirdPartyLiabilityCollectiveService = ServiceManager.Instance.GetService<IThirdPartyLiabilityCollectiveService>();
        internal static ICollectiveThirdPartyLiabilityRenewalService collectiveThirdPartyLiabilityRenewalService = ServiceManager.Instance.GetService<ICollectiveThirdPartyLiabilityRenewalService>();
        //internal static ICollectivePropertyRenewalService collectivePropertyRenewalService = ServiceManager.Instance.GetService<ICollectivePropertyRenewalService>();
        //internal static ICollectivePropertyModificationlService collectivePropertyModificationService = ServiceManager.Instance.GetService<ICollectivePropertyModificationlService>();
        internal static ICollectiveVehicleRenewalService collectiveVehicleRenewalService = ServiceManager.Instance.GetService<ICollectiveVehicleRenewalService>();
        internal static IVehicleModificationService collectiveVehicleModificationService = ServiceManager.Instance.GetService<IVehicleModificationService>();
        internal static IThirdPartyLiabilityModificationService thirdPartyLiabilityModificationService = ServiceManager.Instance.GetService<IThirdPartyLiabilityModificationService>();
        #endregion
        #region Auditoria
        internal static IAuditService AuditServiceCore = ServiceManager.Instance.GetService<IAuditService>();
        #endregion Auditoria
        #region endosos company
        internal static IVehicleModificationServiceCia vehicleModificationServiceCia = ServiceManager.Instance.GetService<IVehicleModificationServiceCia>();
        internal static IVehicleTextService vehicleTextServiceCia = ServiceManager.Instance.GetService<IVehicleTextService>();
        internal static IVehicleClauseService vehicleClauseService = ServiceManager.Instance.GetService<IVehicleClauseService>();
        internal static ICiaVehicleExtensionService vehicleExtensionService = ServiceManager.Instance.GetService<ICiaVehicleExtensionService>();
        internal static IVehicleRenewalService vehicleRenewalService = ServiceManager.Instance.GetService<IVehicleRenewalService>();


        internal static COMVEHIREVER.IVehicleReversionService vehicleReversionServiceCia = ServiceManager.Instance.GetService<COMVEHIREVER.IVehicleReversionService>();
        internal static COMVEHICANC.ICiaVehicleCancellationService vehicleCancellationServiceCia = ServiceManager.Instance.GetService<COMVEHICANC.ICiaVehicleCancellationService>();
        internal static COMVEHICHANGE.ICiaVehicleChangeAgentService vehicleChangeAgentServiceCia = ServiceManager.Instance.GetService<COMVEHICHANGE.ICiaVehicleChangeAgentService>();

        #region Endosos RC Pasajeros
        internal static COTPLMODF.IThirdPartyLiabilityModificationServiceCia thirdPartyLiabilityModificationServiceCia = ServiceManager.Instance.GetService<COTPLMODF.IThirdPartyLiabilityModificationServiceCia>();
        internal static COTPLTEXT.IThirdPartyLiabilityTextServiceCia thirdPartyLiabilityTextServiceCia = ServiceManager.Instance.GetService<COTPLTEXT.IThirdPartyLiabilityTextServiceCia>();
        internal static COTPLCLAU.IThirdPartyLiabilityClauseServiceCia thirdPartyLiabilityClauseServiceCia = ServiceManager.Instance.GetService<COTPLCLAU.IThirdPartyLiabilityClauseServiceCia>();
        internal static COTPLEXT.IThirdPartyLiabilityExtensionServiceCia thirdPartyLiabilityExtensionServiceCia = ServiceManager.Instance.GetService<COTPLEXT.IThirdPartyLiabilityExtensionServiceCia>();
        internal static COTPLCHA.IThirdPartyLiabilityChangeAgentServiceCia thirdPartyLiabilityChangeAgentServiceCia = ServiceManager.Instance.GetService<COTPLCHA.IThirdPartyLiabilityChangeAgentServiceCia>();
        internal static COTPLREN.IThirdPartyLiabilityRenewalServiceCia thirdPartyLiabilityRenewalServiceCia = ServiceManager.Instance.GetService<COTPLREN.IThirdPartyLiabilityRenewalServiceCia>();
        internal static COTPLCAN.IThirdPartyLiabilityCancellationServiceCia thirdPartyLiabilityCancellationServiceCia = ServiceManager.Instance.GetService<COTPLCAN.IThirdPartyLiabilityCancellationServiceCia>();
        internal static COTPLREV.IThirdPartyLiabilityReversionServiceCia thirdPartyLiabilityReversionServiceCia = ServiceManager.Instance.GetService<COTPLREV.IThirdPartyLiabilityReversionServiceCia>();

        internal static IMassiveTPLCancellationServices massiveTPLCancellationServices = ServiceManager.Instance.GetService<IMassiveTPLCancellationServices>();
        #endregion Endosos RC Pasajeros.
        #region endosos property
        internal static COMPROCANC.IPropertyCancellationServiceCia propertyCancellationServiceCia = ServiceManager.Instance.GetService<COMPROCANC.IPropertyCancellationServiceCia>();
        internal static COMPROTEX.IPropertyTextServiceCia propertyTextServiceCia = ServiceManager.Instance.GetService<COMPROTEX.IPropertyTextServiceCia>();
        internal static COMPROMOD.IPropertyModificationServiceCia propertyModificationServiceCia = ServiceManager.Instance.GetService<COMPROMOD.IPropertyModificationServiceCia>();
        internal static COMPROCHA.IPropertyChangeAgentServiceCia propertyChangeAgentServiceCia = ServiceManager.Instance.GetService<COMPROCHA.IPropertyChangeAgentServiceCia>();
        internal static COMPROCLA.IPropertyClauseServiceCia propertyClauseServiceCia = ServiceManager.Instance.GetService<COMPROCLA.IPropertyClauseServiceCia>();
        internal static COMPROEXT.IPropertyExtensionServiceCia propertyExtensionServiceCia = ServiceManager.Instance.GetService<COMPROEXT.IPropertyExtensionServiceCia>();
        internal static COMPROREN.IPropertyRenewalServiceCia propertyRenewalServiceCia = ServiceManager.Instance.GetService<COMPROREN.IPropertyRenewalServiceCia>();
        internal static COMPROREV.IPropertyReversionServiceCia propertyReversionEndorsementCia = ServiceManager.Instance.GetService<COMPROREV.IPropertyReversionServiceCia>();

        #endregion endosos property


        #region Liability

        internal static COMLIAMODF.ILiabilityModificationServiceCia liabilityModificationServiceCia = ServiceManager.Instance.GetService<COMLIAMODF.ILiabilityModificationServiceCia>();
        internal static COMLIATEX.ILiabilityTextServiceCia liabilityTextServiceCia = ServiceManager.Instance.GetService<COMLIATEX.ILiabilityTextServiceCia>();
        internal static COMLIACLAU.ILiabilityClauseServiceCia liabilityClauseService = ServiceManager.Instance.GetService<COMLIACLAU.ILiabilityClauseServiceCia>();
        internal static COMLIAEXT.ILiabilityExtensionServiceCia liabilityExtensionService = ServiceManager.Instance.GetService<COMLIAEXT.ILiabilityExtensionServiceCia>();
        internal static COMLIAREN.ILiabilityRenewalServiceCia liabilityRenewalService = ServiceManager.Instance.GetService<COMLIAREN.ILiabilityRenewalServiceCia>();

        internal static COMLIAREVER.ILiabilityReversionServiceCia liabilityReversionServiceCia = ServiceManager.Instance.GetService<COMLIAREVER.ILiabilityReversionServiceCia>();
        internal static COMLIACANC.ILiabilityCancellationServiceCia liabilityCancellationServiceCia = ServiceManager.Instance.GetService<COMLIACANC.ILiabilityCancellationServiceCia>();
        internal static COMLIACHANGE.ILiabilityChangeAgentServiceCia liabilityChangeAgentServiceCia = ServiceManager.Instance.GetService<COMLIACHANGE.ILiabilityChangeAgentServiceCia>();
        internal static COMLIACHANGECOI.ILiabilityChangeCoinsuranceServiceCia liabilityChangeCoinsuranceServiceCia = ServiceManager.Instance.GetService<COMLIACHANGECOI.ILiabilityChangeCoinsuranceServiceCia>();
        internal static COMLIAPH.ICiaLiabilityChangePolicyHolderService ciaLiabilityChangePolicyHolderService = ServiceManager.Instance.GetService<COMLIAPH.ICiaLiabilityChangePolicyHolderService>();
        internal static COMLIACT.ILiabilityChangeTermServiceCompany LiabilityChangeTermServiceCompany = ServiceManager.Instance.GetService<COMLIACT.ILiabilityChangeTermServiceCompany>();

        #endregion Liability
        #region endosos surety
        internal static COMSURCANC.ISuretyCancellationServiceCia suretyCancellationServiceCia = ServiceManager.Instance.GetService<COMSURCANC.ISuretyCancellationServiceCia>();
        internal static COMSURTEX.ISuretyTextServiceCia suretyTextServiceCia = ServiceManager.Instance.GetService<COMSURTEX.ISuretyTextServiceCia>();
        internal static COMSURMOD.ISuretyModificationServiceCia suretyModificationServiceCia = ServiceManager.Instance.GetService<COMSURMOD.ISuretyModificationServiceCia>();
        internal static COMSURCHA.ICiaSuretyChangeAgentService suretyChangeAgentServiceCia = ServiceManager.Instance.GetService<COMSURCHA.ICiaSuretyChangeAgentService>();
        internal static COMSURCOI.ICiaSuretyChangeCoinsuranceService ciaSuretyChangeCoinsuranceService = ServiceManager.Instance.GetService<COMSURCOI.ICiaSuretyChangeCoinsuranceService>();

        internal static COMSURCLA.ISuretyClauseServiceCia suretyClauseServiceCia = ServiceManager.Instance.GetService<COMSURCLA.ISuretyClauseServiceCia>();
        internal static COMSURPH.ICiaSuretyChangePolicyHolderService ciaSuretyChangePolicyHolderService = ServiceManager.Instance.GetService<COMSURPH.ICiaSuretyChangePolicyHolderService>();
        internal static COMSUCONC.ICiaSuretyChangeConsolidationService ciaSuretyChangeConsolidationService = ServiceManager.Instance.GetService<COMSUCONC.ICiaSuretyChangeConsolidationService>();
        internal static COMSUREXT.ISuretyExtensionServiceCia suretyExtensionServiceCia = ServiceManager.Instance.GetService<COMSUREXT.ISuretyExtensionServiceCia>();
        internal static ISuretyRenewalServiceCompany suretyRenewalServiceCia = ServiceManager.Instance.GetService<ISuretyRenewalServiceCompany>();
        internal static COMSURREV.ISuretyReversionServiceCia suretyReversionEndorsement = ServiceManager.Instance.GetService<COMSURREV.ISuretyReversionServiceCia>();
        internal static COMSURTEM.ISuretyChangeTermServiceCompany suretyChangeTermServiceCompany = ServiceManager.Instance.GetService<COMSURTEM.ISuretyChangeTermServiceCompany>();
        #endregion endosos surety


        #region JudicialSurery
        internal static COJSCAN.IJudicialSuretyCancellationServiceCompany JudicialSuretyCancellationService = ServiceManager.Instance.GetService<COJSCAN.IJudicialSuretyCancellationServiceCompany>();
        internal static COJSCHA.IJudicialSuretyChangeAgentServiceCompany JudicialSuretyChangeAgentService = ServiceManager.Instance.GetService<COJSCHA.IJudicialSuretyChangeAgentServiceCompany>();
        internal static COJSCOI.IJudicialSuretyChangeCoinsuranceServiceCompany judicialSuretyChangeCoinsuranceServiceCompany = ServiceManager.Instance.GetService<COJSCOI.IJudicialSuretyChangeCoinsuranceServiceCompany>();
        internal static COJSEXT.IJudicialSuretyExtensionServiceCompany JudicialSuretyExtensionService = ServiceManager.Instance.GetService<COJSEXT.IJudicialSuretyExtensionServiceCompany>();
        internal static COJSMODF.IJudicialSuretyModificationServiceCompany JudicialSuretyModificationService = ServiceManager.Instance.GetService<COJSMODF.IJudicialSuretyModificationServiceCompany>();
        internal static COJSTEXT.IJudicialSuretyTextServiceCompany JudicialSuretyTextService = ServiceManager.Instance.GetService<COJSTEXT.IJudicialSuretyTextServiceCompany>();
        internal static COJSCLAU.IJudicialSuretyClauseServiceCompany JudicialSuretyClauseService = ServiceManager.Instance.GetService<COJSCLAU.IJudicialSuretyClauseServiceCompany>();
        internal static COJSREV.IJudicialSuretyReversionServiceCompany JudicialSuretyReversionService = ServiceManager.Instance.GetService<COJSREV.IJudicialSuretyReversionServiceCompany>();
        internal static COMSURREN.IJudicialSuretyRenewalService JudicialSuretyRenewalService = ServiceManager.Instance.GetService<COMSURREN.IJudicialSuretyRenewalService>();
        internal static COJSPH.ICiaJudicialSuretyChangePolicyHolderService CiaJudicialSuretyChangePolicyHolderService = ServiceManager.Instance.GetService<COJSPH.ICiaJudicialSuretyChangePolicyHolderService>();
        #endregion JudicialSurery

        #endregion endosos company
        #region param producto company
        internal static IProductParamService productParamService = ServiceManager.Instance.GetService<IProductParamService>();
        #endregion

        #region Reports
        //internal static IReportsService reportsService = ServiceManager.Instance.GetService<IReportsService>();
        internal static ICompanyCollectionFormBusinessService collectionFormBusinessService = ServiceManager.Instance.GetService<ICompanyCollectionFormBusinessService>();
        #endregion Reports

        #region Transports

        internal static ICompanyTransportApplicationService transportApplicationService = ServiceManager.Instance.GetService<ICompanyTransportApplicationService>();
        internal static ICiaTransportsExtensionService transportExtensionService = ServiceManager.Instance.GetService<ICiaTransportsExtensionService>();
        internal static ITransportReversionService ITransportReversionService = ServiceManager.Instance.GetService<ITransportReversionService>();
        internal static ICiaTransportRenewalService TransportRenewalService = ServiceManager.Instance.GetService<ICiaTransportRenewalService>();
        internal static ITransportModificationServiceCia TransportModificationService = ServiceManager.Instance.GetService<ITransportModificationServiceCia>();
        internal static ITransportTextService TransporTextService = ServiceManager.Instance.GetService<ITransportTextService>();
        internal static ITransportClauseService transportClauseService = ServiceManager.Instance.GetService<ITransportClauseService>();
        internal static ICiaTransportChangeAgentService TransportChangeAgentService = ServiceManager.Instance.GetService<ICiaTransportChangeAgentService>();
        internal static ICiaTransportCancellationService transportCancellationServiceCia = ServiceManager.Instance.GetService<TRANSCAN.ICiaTransportCancellationService>();
        internal static ITransportCreditNoteApplicationService transportCreditNoteApplicationService = ServiceManager.Instance.GetService<ITransportCreditNoteApplicationService>();
        internal static ITransportDeclarationApplicationService transportDeclarationServiceCia = ServiceManager.Instance.GetService<ITransportDeclarationApplicationService>();
        internal static ITransportAdjustmentApplicationService transportAdjustmentApplicationService = ServiceManager.Instance.GetService<ITransportAdjustmentApplicationService>();
        internal static ICompanyTransportBusinessService TransportBusinessService = ServiceManager.Instance.GetService<ICompanyTransportBusinessService>();
        #endregion

        #region PropertyEndorsemnet
        internal static IDeclarationApplicationService propertyDeclarationApplicationService = ServiceManager.Instance.GetService<IDeclarationApplicationService>();
        internal static IPropertyAdjustmentApplicationService propertyAdjustmentApplicationService = ServiceManager.Instance.GetService<IPropertyAdjustmentApplicationService>();
        #endregion

        #region UnderwritingApplicationService

        internal static IUnderwritingApplicationService UnderwritingApplicationService = ServiceManager.Instance.GetService<IUnderwritingApplicationService>(); /*ServiceManager.Instance.GetService<IUnderwritingApplicationService>();*/
        internal static IUnderwritingApplicationService UnderwritingApplicationServiceV2 = ServiceManager.Instance.GetService<Company.Application.UnderwritingApplicationService.IUnderwritingApplicationService>();
        #endregion


        #region Claims

        internal static IClaimApplicationService claimApplicationService = ServiceManager.Instance.GetService<IClaimApplicationService>();
        internal static IPaymentRequestApplicationService paymentRequestApplicationService = ServiceManager.Instance.GetService<IPaymentRequestApplicationService>();
        internal static ISalvageApplicationService salvageApplicationService = ServiceManager.Instance.GetService<ISalvageApplicationService>();
        internal static IRecoveryApplicationService recoveryApplicationService = ServiceManager.Instance.GetService<IRecoveryApplicationService>();

        #endregion


        #region parametrization

        internal static PARCOMPANY.IParametrizationAplicationService parametrizationAplicationService = ServiceManager.Instance.GetService<PARCOMPANY.IParametrizationAplicationService>();
        #endregion

        internal static IExternalProxyService ExternalServiceWeb = ServiceManager.Instance.GetService<IExternalProxyService>();
        internal static IAuthorizationPoliciesApplicationService authorizationPoliciesApplicationService = ServiceManager.Instance.GetService<IAuthorizationPoliciesApplicationService>();

        #region Sarlaft
        internal static ISarlaftApplicationServices SarlaftApplicationServices = ServiceManager.Instance.GetService<ISarlaftApplicationServices>();
        internal static ISarlaftBusinessServices SarlaftBusinessServices = ServiceManager.Instance.GetService<ISarlaftBusinessServices>();
        #endregion


        #region Aricraft
        internal static ICompanyAircraftApplicationService aircraftApplicationService = ServiceManager.Instance.GetService<ICompanyAircraftApplicationService>();
        internal static ICompanyAircraftBusinessService aircraftBusinessService = ServiceManager.Instance.GetService<ICompanyAircraftBusinessService>();
        internal static AIRSEX.ICiaAircraftsExtensionService aircraftExtensionService = ServiceManager.Instance.GetService<AIRSEX.ICiaAircraftsExtensionService>();
        internal static AIRCAN.ICiaAircraftCancellationService aircraftCancellationServiceCia = ServiceManager.Instance.GetService<AIRCAN.ICiaAircraftCancellationService>();
        internal static AIRMOD.IAircraftModificationServiceCia aircraftModificationService = ServiceManager.Instance.GetService<AIRMOD.IAircraftModificationServiceCia>();
        internal static ICiaAircraftChangeAgentService ciaAircraftChangeAgentService = ServiceManager.Instance.GetService<ICiaAircraftChangeAgentService>();
        internal static AIRREW.ICiaAircraftRenewalService aircraftRenewalService = ServiceManager.Instance.GetService<AIRREW.ICiaAircraftRenewalService>();
        internal static IAircraftClauseService aircraftClauseService = ServiceManager.Instance.GetService<IAircraftClauseService>();
        internal static AIRTEXT.IAircraftTextService aircraftTextService = ServiceManager.Instance.GetService<AIRTEXT.IAircraftTextService>();
        internal static IAircraftReversionService aircraftReversionService = ServiceManager.Instance.GetService<IAircraftReversionService>();
        #endregion

        #region Marines

        internal static ICompanyMarineApplicationService marineApplicationService = ServiceManager.Instance.GetService<ICompanyMarineApplicationService>();
        internal static ICompanyMarineBusinessService marineBusinessService = ServiceManager.Instance.GetService<ICompanyMarineBusinessService>();
        internal static MARSEX.ICiaMarinesExtensionService marineExtensionService = ServiceManager.Instance.GetService<MARSEX.ICiaMarinesExtensionService>();
        internal static MARCAN.ICiaMarineCancellationService marineCancellationServiceCia = ServiceManager.Instance.GetService<MARCAN.ICiaMarineCancellationService>();
        internal static MARMOD.IMarineModificationServiceCia marineModificationService = ServiceManager.Instance.GetService<MARMOD.IMarineModificationServiceCia>();
        internal static ICiaMarineChangeAgentService ciaMarineChangeAgentService = ServiceManager.Instance.GetService<ICiaMarineChangeAgentService>();
        internal static MARREW.ICiaMarineRenewalService marineRenewalService = ServiceManager.Instance.GetService<MARREW.ICiaMarineRenewalService>();
        internal static IMarineClauseService marineClauseService = ServiceManager.Instance.GetService<IMarineClauseService>();
        internal static MARTEXT.IMarineTextService marineTextService = ServiceManager.Instance.GetService<MARTEXT.IMarineTextService>();
        internal static IMarineReversionService marineReversionService = ServiceManager.Instance.GetService<IMarineReversionService>();
        #endregion

        #region Fidelities
        internal static IFidelityService fidelityService = ServiceManager.Instance.GetService<IFidelityService>();
        //internal static ICompanyFidelityApplicationService fidelityApplicationService = ServiceManager.Instance.GetService<ICompanyFidelityApplicationService>();
        //internal static IFidelityService fidelityBusinessService = ServiceManager.Instance.GetService<IFidelityService>();
        internal static Company.Application.FidelitytExtensionService.IFidelityExtensionServiceCia fidelityExtensionService = ServiceManager.Instance.GetService<Company.Application.FidelitytExtensionService.IFidelityExtensionServiceCia>();
        internal static IFidelityModificationServiceCia fidelityModificationService = ServiceManager.Instance.GetService<IFidelityModificationServiceCia>();
        internal static Company.Application.FidelityRenewalService.IFidelityRenewalServiceCia fidelityRenewalService = ServiceManager.Instance.GetService<Company.Application.FidelityRenewalService.IFidelityRenewalServiceCia>();
        internal static IFidelityClauseServiceCia fidelityClauseService = ServiceManager.Instance.GetService<IFidelityClauseServiceCia>();
        internal static IFidelityTextServiceCia fidelityTextService = ServiceManager.Instance.GetService<IFidelityTextServiceCia>();
        internal static Company.Application.FidelityReversionService.IFidelityReversionServiceCia fidelityReversionService = ServiceManager.Instance.GetService<Company.Application.FidelityReversionService.IFidelityReversionServiceCia>();
        internal static COMFIACHANGE.IFidelityChangeAgentServiceCia fidelityChangeAgentServiceCia = ServiceManager.Instance.GetService<COMFIACHANGE.IFidelityChangeAgentServiceCia>();
        internal static COMFIACANC.IFidelityCancellationServiceCia fidelityCancellationServiceCia = ServiceManager.Instance.GetService<COMFIACANC.IFidelityCancellationServiceCia>();
        #endregion

        #region ListRiskPerson
        internal static IUniquePersonListRiskApplicationServices uniquePersonListRiskApplication = ServiceManager.Instance.GetService<IUniquePersonListRiskApplicationServices>();
        #endregion

        #region FASECOLDA
        internal static ICompanyMassiveVehicleParamApplicationService companyMassiveVehicleParamApplicationService = ServiceManager.Instance.GetService<ICompanyMassiveVehicleParamApplicationService>();
        #endregion

        //internal static IFullServicesSUP FullServices = ServiceManager.Instance.GetService<IFullServicesSUP>();

        #region cache
        internal static ICacheBusinessService cacheBusinessService = ServiceManager.Instance.GetService<ICacheBusinessService>();
        #endregion

        //internal static IFullServicesSUP FullServices = ServiceManager.Instance.GetService<IFullServicesSUP>();
        #region authorizationProviders - menu R2
        internal static AUTHEPROVIDER.IAuthorizationProvider authorizationProviders = ServiceManager.Instance.GetService<AUTHEPROVIDER.IAuthorizationProvider>();
        #endregion
        #region EVENTS
        internal static IEventApplicationService EventApplicationService = ServiceManager.Instance.GetService<IEventApplicationService>();
        #endregion

        //internal static IAccountingService GeneralLedgerService = ServiceManager.Instance.GetService<IAccountingService>();
        #region Accounting 
        internal static IAccountingAccountService accountingAccountService = ServiceManager.Instance.GetService<IAccountingAccountService>();

        internal static IAccountingAccountsPayableService accountingAccountsPayableService = ServiceManager.Instance.GetService<IAccountingAccountsPayableService>();
        internal static IAccountingAmortizationService accountingAmortizationService = ServiceManager.Instance.GetService<IAccountingAmortizationService>();
        internal static IAccountingCollectControlService accountingCollectControlService = ServiceManager.Instance.GetService<IAccountingCollectControlService>();
        internal static IAccountingCollectService accountingCollectService = ServiceManager.Instance.GetService<IAccountingCollectService>();
        internal static IAccountingComponentDistributionService accountingComponentDistributionService = ServiceManager.Instance.GetService<IAccountingComponentDistributionService>();
        internal static IAccountingCreditNoteService accountingCreditNoteService = ServiceManager.Instance.GetService<IAccountingCreditNoteService>();
        //internal static IAccountingImputationService imputationApplicationService = ServiceManager.Instance.GetService<IAccountingImputationService>();
        internal static IAccountingApplicationService accountingApplicationService = ServiceManager.Instance.GetService<IAccountingApplicationService>();
        /*internal static Application.AccountingServices.IAccountingApplicationService accountingApplicationService = 
            ServiceManager.Instance.GetService<Application.AccountingServices.IAccountingApplicationService>();*/
        internal static IAccountingParameterService accountingParameterService = ServiceManager.Instance.GetService<IAccountingParameterService>();
        internal static IAccountingPaymentBallotService accountingPaymentBallotService = ServiceManager.Instance.GetService<IAccountingPaymentBallotService>();
        internal static IAccountingPaymentService accountingPaymentService = ServiceManager.Instance.GetService<IAccountingPaymentService>();
        internal static IAccountingPaymentTicketService accountingPaymentTicketService = ServiceManager.Instance.GetService<IAccountingPaymentTicketService>();
        internal static IAccountingRetentionService accountingRetentionService = ServiceManager.Instance.GetService<IAccountingRetentionService>();
        internal static IAccountingIntegrationService accountingIntegrationService = ServiceManager.Instance.GetService<IAccountingIntegrationService>();
        internal static ITempCommonService tempCommonService = ServiceManager.Instance.GetService<ITempCommonService>();
        internal static ITempCommonIntegrationService tempCommonIntegrationService = ServiceManager.Instance.GetService<ITempCommonIntegrationService>();
        #endregion
        #region AccountingClosing
        internal static IAccountingClosingApplicationService AccountingClosingService = ServiceManager.Instance.GetService<IAccountingClosingApplicationService>();
        internal static IAccountingClosingApplicationService accountingClosingApplicationService = ServiceManager.Instance.GetService<IAccountingClosingApplicationService>();
        #endregion

        #region GeneralLedger
        internal static GL.IAccountingApplicationService glAccountingApplicationService = ServiceManager.Instance.GetService<GL.IAccountingApplicationService>();
        internal static GL.IEntryParameterApplicationService entryParameterApplicationService = ServiceManager.Instance.GetService<GL.IEntryParameterApplicationService>();
        #endregion

        #region AutomaticDebitServices
        internal static IAutomaticDebitService automaticDebitService = ServiceManager.Instance.GetService<IAutomaticDebitService>();
        #endregion

        #region Reporting
        internal static IReportingService reportingService = ServiceManager.Instance.GetService<IReportingService>();
        #endregion

        #region Reconciliation
        internal static IReconciliationService reconciliationService = ServiceManager.Instance.GetService<IReconciliationService>();

        #endregion

        #region Reinsurance
        internal static IReinsuranceApplicationService reinsuranceService = ServiceManager.Instance.GetService<IReinsuranceApplicationService>();
        internal static IReinsuranceReportsService reinsuranceReportService = ServiceManager.Instance.GetService<IReinsuranceReportsService>();

        #endregion

        #region OperationQuota
        internal static IOperationQuotaService operationQuotaService = ServiceManager.Instance.GetService<IOperationQuotaService>();
        internal static IConsortiumService consortiumService = ServiceManager.Instance.GetService<IConsortiumService>();
        internal static IEconomicGroupService EconomicGroupService = ServiceManager.Instance.GetService<IEconomicGroupService>();
        #region CompanyOperationQuota
        internal static IOperationQuotaCompanyService operationQuotaCompanyService = ServiceManager.Instance.GetService<IOperationQuotaCompanyService>();
        #endregion
        #endregion

        #region recuotificacion
        internal static IFinancialPlanService financialPlanService = ServiceManager.Instance.GetService<IFinancialPlanService>();
        #endregion
        #region Reversion de Primas
        internal static IAccountingReversionService accountingReversionService = ServiceManager.Instance.GetService<IAccountingReversionService>();
        #endregion
    }
}
