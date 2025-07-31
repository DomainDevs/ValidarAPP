using System.Configuration;
using System.Web.Configuration;
using System.Web.Optimization;

namespace Sistran.Core.Framework.UIF.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundle/app-style").Include(
                "~/Content/app-styles.css"
            ));

            #region
            bundles.Add(new ScriptBundle("~/bundle/UifLayout").Include(
                "~/Lib/framework/build/vendors/js/vendors-{version}.js",
                "~/Lib/framework/build/uif-touch-{version}-alpha.es.js",
                "~/Scripts/app/start.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.validate.unobtrusive*"
                ));
            bundles.Add(new ScriptBundle("~/uifr2/theme").Include(
                "~/Lib/framework/theme/scripts/layout.js"
                ));
            bundles.Add(new StyleBundle("~/Lib/UifCss").Include(
                "~/Lib/framework/build/vendors/css/vendors-{version}.css",
                "~/Lib/framework/build/uif-touch-4.3.0.min.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Scripts/vendor/jquery.ui.widget.js"
                ));
            //LOGIN
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
               "~/Content/login/css/buttons.css",
                "~/Content/login/css/style.css"
               ));
            //HUBS
            bundles.Add(new ScriptBundle("~/bundle/js/SignalR").Include(
                "~/Scripts/jquery.signalR-2.2.2.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/CKEditor").Include(
                    "~/Scripts/CKEditor/ckeditor.js"
                ));
            //Valores Globales
            bundles.Add(new ScriptBundle("~/bundle/js/Global").Include(
                  "~/Areas/Endorsement/Scripts/Search/GlobalValue.js",
                  "~/Areas/Endorsement/Scripts/SharedEndorsementBase.js"
               ));
            //Authorization Policies
            bundles.Add(new ScriptBundle("~/bundle/js/AuthorizationPolicies").Include(
                "~/Areas/AuthorizationPolicies/Scripts/Policies/Policies.js",
                "~/Areas/AuthorizationPolicies/Scripts/Authorize/Authorize.js",
                "~/Areas/AuthorizationPolicies/Scripts/Policies/PoliciesRules.js",
                "~/Areas/AuthorizationPolicies/Scripts/Policies/AdvancedSearchPolicy.js",
                "~/Areas/AuthorizationPolicies/Scripts/WorkFlowPolicies/WorkFlowPolicies.js",
                "~/Areas/AuthorizationPolicies/Scripts/WorkFlowPolicies/WorkFlowPoliciesRequest.js",
                "~/Areas/AuthorizationPolicies/Scripts/AuthorizationPersonRiskList/AuthorizationPersonRiskList.js",
                "~/Areas/AuthorizationPolicies/Scripts/AuthorizationPersonRiskList/AuthorizationPersonRiskListRequest.js",
                "~/Areas/AuthorizationPolicies/Scripts/AuthorizationSarlaftOperation/AuthorizationSarlaftOperation.js",
                "~/Areas/AuthorizationPolicies/Scripts/AuthorizationSarlaftOperation/AuthorizationSarlaftOperationRequest.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/LaunchPolicies").Include(
                "~/Areas/AuthorizationPolicies/Scripts/LaunchPolicies.js",
                "~/Areas/AuthorizationPolicies/Scripts/Summary/Summary.js"
                ));
            //Reglas
            bundles.Add(new ScriptBundle("~/bundle/js/Rules").Include(
                "~/Areas/RulesAndScripts/Scripts/RuleSet/RuleSet.js",
                "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalRuleSet.js",
                "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalRule.js",
                "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalConditionRule.js",
                "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalActionRule.js"
                ));
            //Cache
            bundles.Add(new ScriptBundle("~/bundle/js/RuleSetCache").Include(
                "~/Areas/RulesAndScripts/Scripts/RuleSetCache/RuleSetCache.js"
                ));
            //Tablas de decision
            bundles.Add(new ScriptBundle("~/bundle/js/DecisionTable").Include(
                "~/Areas/RulesAndScripts/Scripts/DecisionTable/DecisionTable.js",
                "~/Areas/RulesAndScripts/Scripts/DecisionTable/Header.js",
                "~/Areas/RulesAndScripts/Scripts/DecisionTable/Body.js",
                "~/Areas/RulesAndScripts/Scripts/DecisionTable/LoadFromFile.js",
                "~/Areas/RulesAndScripts/Scripts/DecisionTable/AdvancedSearchDt.js"
                ));
            //Productos

            //Enums
            bundles.Add(new ScriptBundle("~/bundle/js/Enums").Include(
                "~/Scripts/Validator.js",
                "~/Scripts/jquery.redirect.js",
                "~/Scripts/EnumsCommon.js",
                "~/Scripts/EnumsPerson.js",
                "~/Scripts/EnumsUnderwriting.js",
                "~/Scripts/EnumsMassive.js",
                "~/Scripts/EnumsProduct.js",
                "~/Scripts/Helpers.js",
                "~/Scripts/JSLINQ.js",
                "~/Scripts/Uif2Co.js",
                "~/Scripts/EnumsPoliciesAuthorization.js",
                "~/Scripts/EnumsEvents.js",
                "~/Scripts/EnumsParametrization.js",
                "~/Scripts/EnumsSurcharge.js",
                "~/Scripts/EnumsDiscount.js",
                "~/Scripts/EnumsClaims.js"
                ));
            //AutomaticQuota
            bundles.Add(new ScriptBundle("~/bundle/js/AutomaticQuota").Include(
                "~/Areas/AutomaticQuota/Scripts/AutomaticQuota.js",
                "~/Areas/AutomaticQuota/Scripts/AutomaticQuotaRequest.js",
                "~/Areas/AutomaticQuota/Scripts/AdvancedSearchAutomaticQuota.js"
                ));
            //Surety
            bundles.Add(new ScriptBundle("~/bundle/js/Surety").Include(
                "~/Areas/Underwriting/Scripts/RiskSurety/Surety.js",
                "~/Areas/Underwriting/Scripts/RiskSurety/SuretyRequest.js",
                "~/Areas/Underwriting/Scripts/RiskSurety/ContractObject.js",
                "~/Areas/Underwriting/Scripts/RiskSurety/ContractObjectRequest.js",
                "~/Areas/Underwriting/Scripts/RiskSurety/CrossGuarantees.js",
                "~/Areas/Underwriting/Scripts/RiskSurety/CrossGuaranteesRequest.js",
                "~/Areas/Underwriting/Scripts/RiskSurety/OperationQuotaCumulus.js",
                "~/Areas/Underwriting/Scripts/RiskSurety/OperationQuotaCumulusRequest.js"
                ));
            //CoverageSurety
            bundles.Add(new ScriptBundle("~/bundle/js/CoverageSurety").Include(
                "~/Areas/Underwriting/Scripts/RiskSurety/RiskSuretyCoverage.js",
                "~/Areas/Underwriting/Scripts/RiskSurety/CoverageRequest.js"
                ));
            //ContractObject
            bundles.Add(new ScriptBundle("~/bundle/js/ContractObject").Include(
                "~/Areas/Underwriting/Scripts/ContractObject/ContractObject.js",
                "~/Areas/Underwriting/Scripts/ContractObject/ContractObjectRequest.js"
                ));
            //Property
            bundles.Add(new ScriptBundle("~/bundle/js/Property").Include(
                "~/Areas/Underwriting/Scripts/RiskProperty/PropertyRequest.js",
                "~/Areas/Underwriting/Scripts/RiskProperty/Property.js",
                "~/Areas/Underwriting/Scripts/RiskProperty/AdditionalDataRequest.js",
                "~/Areas/Underwriting/Scripts/RiskProperty/AdditionalData.js",
                "~/Areas/Underwriting/Scripts/RiskProperty/InsuredObjectRequest.js",
                "~/Areas/Underwriting/Scripts/RiskProperty/InsuredObject.js"
                ));
            //Liability
            bundles.Add(new ScriptBundle("~/bundle/js/Liability").Include(
                "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiability.js",
                "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiabilityRequest.js",
                "~/Areas/Underwriting/Scripts/RiskLiability/Coverage.js",
                "~/Areas/Underwriting/Scripts/RiskLiability/CoverageRequest.js"

                ));
            //Transport
            bundles.Add(new ScriptBundle("~/bundle/js/Transport").Include(
                "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransport.js",
                "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportRequest.js",
                "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportCoverage/RiskTransportCoverage.js",
                "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportCoverage/RiskTransportCoverageRequest.js"
                ));

            //TransportAjustment
            bundles.Add(new ScriptBundle("~/bundle/js/TransportAdjustment").Include(
                    "~/Areas/Endorsement/Scripts/TransportAdjustment/TransportAdjustment.js",
                "~/Areas/Endorsement/Scripts/TransportAdjustment/TransportAdjustmentRequest.js"
                ));

            //TransportCreditNote
            bundles.Add(new ScriptBundle("~/bundle/js/TransportCreditNote").Include(
                "~/Areas/Endorsement/Scripts/TransportCreditNote/TransportCreditNote.js",
                "~/Areas/Endorsement/Scripts/TransportCreditNote/TransportCreditNoteRequest.js"
                ));

            //TransportDeclaration
            bundles.Add(new ScriptBundle("~/bundle/js/TransportDeclaration").Include(
                "~/Areas/Endorsement/Scripts/TransportDeclaration/TransportDeclaration.js",
                "~/Areas/Endorsement/Scripts/TransportDeclaration/TransportDeclarationRequest.js"
                ));

            //ThirdPartyLiability
            bundles.Add(new ScriptBundle("~/bundle/js/ThirdPartyLiability").Include(
                "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/ThirdPartyLiability.js",
                "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/ThirdPartyLiabilityRequest.js",
                "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/Coverage.js",
                "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/CoverageRequest.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/fileupload").Include(
                "~/Scripts/vendor/jquery.ui.widget.js",
                "~/Scripts/jquery.fileupload.js"
                ));
            //Concepts
            bundles.Add(new ScriptBundle("~/bundle/js/RiskConcepts").Include(
                "~/Areas/Underwriting/Scripts/Coverages/Concepts.js",
                "~/Areas/Underwriting/Scripts/Risks/Concepts.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/AuthorizeUser").Include(
                "~/Areas/Collective/Scripts/Collective/AuthorizeUserTemporal.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/Collective").Include(
                "~/Areas/Collective/Scripts/Collective/Collective.js",
                "~/Areas/Collective/Scripts/Collective/AddMassive.js",
                "~/Areas/Collective/Scripts/Collective/MassiveLoadFinished.js",
                "~/Areas/Collective/Scripts/Collective/MassiveLoadTariffed.js",
                "~/Areas/Collective/Scripts/Collective/MassiveLoadIssue.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementConsultation").Include(
                "~/Areas/Collective/Scripts/Collective/EndorsementConsultation.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementModificationCollective").Include(
                "~/Areas/Collective/Scripts/Collective/EndorsementModification.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/ImportMassive").Include(
                "~/Areas/Collective/Scripts/Collective/ImportMassive.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementCancellation").Include(
                "~/Areas/Endorsement/Scripts/CancellationRequest.js",
                "~/Areas/Endorsement/Scripts/Cancellation.js"

                ));

            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementExtension").Include(
                "~/Areas/Endorsement/Scripts/ExtensionRequest.js",
                "~/Areas/Endorsement/Scripts/Extension.js"

                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementClauses").Include(
                "~/Areas/Endorsement/Scripts/ClausesEndorsementRequest.js",
                "~/Areas/Endorsement/Scripts/Clauses.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementModification").Include(
                "~/Areas/Endorsement/Scripts/ModificationRequest.js",
                "~/Areas/Endorsement/Scripts/Modification.js",
                "~/Areas/Endorsement/Scripts/UnderwritingEndorsement.js"

                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementPaymentPlan").Include(
                "~/Areas/Endorsement/Scripts/PaymentPlanRequest.js",
                "~/Areas/Endorsement/Scripts/PaymentPlan.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementTexts").Include(
                "~/Areas/Endorsement/Scripts/TextsRequest.js",
                "~/Areas/Endorsement/Scripts/Texts.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementRenewal").Include(
                 "~/Areas/Endorsement/Scripts/RenewalRequest.js",
                "~/Areas/Endorsement/Scripts/Renewal.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementReversion").Include(
                "~/Areas/Endorsement/Scripts/ReversionRequest.js",
                "~/Areas/Endorsement/Scripts/Reversion.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/SearchEndorsement").Include(
                "~/Areas/Endorsement/Scripts/Search/SearchIndex.js",
                "~/Areas/Endorsement/Scripts/SearchRequest.js",
                "~/Areas/Endorsement/Scripts/Search.js",
                "~/Areas/Endorsement/Scripts/AdvancedSearch.js",
                "~/Areas/Collective/Scripts/Collective/Consultation/Texts.js",
                "~/Areas/Collective/Scripts/Collective/Consultation/ClausesRequest.js",
                "~/Areas/Collective/Scripts/Collective/Consultation/Clauses.js",
                "~/Areas/Collective/Scripts/Collective/Consultation/PaymentPlan.js",
                "~/Areas/Collective/Scripts/Collective/Consultation/Agents.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementChangePolicyHolder").Include(
                "~/Areas/Endorsement/Scripts/ChangePolicyHolder.js",
                "~/Areas/Endorsement/Scripts/ChangePolicyHolderRequest.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementChangeConsolidation").Include(
                "~/Areas/Endorsement/Scripts/ChangeConsolidation.js",
                "~/Areas/Endorsement/Scripts/ChangeConsolidationRequest.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementChangeCoinsurance").Include(
              "~/Areas/Endorsement/Scripts/ChangeCoinsurance.js",
              "~/Areas/Endorsement/Scripts/ChangeCoinsuranceRequest.js"
              ));

            bundles.Add(new ScriptBundle("~/bundle/js/Massive").Include(
                "~/Areas/Massive/Scripts/MassiveRequest.js",
                "~/Areas/Massive/Scripts/Massive/Massive.js",
                "~/Areas/Massive/Scripts/Massive/MassiveAdvancedSearch.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/MassiveProcess").Include(
                "~/Areas/Massive/Scripts/MassiveProcess/MassiveProcess.js",
                "~/Areas/Massive/Scripts/MassiveProcess/AdvancedSearchMassiveProcess.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/CancellationMassive").Include(
                "~/Areas/Massive/Scripts/CancellationMassive/CancellationMassive.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/StrongPassword").Include(
                "~/Scripts/StrongPassword/ChangePassword.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/js/EndorsementChangeTerm").Include(
                "~/Areas/Endorsement/Scripts/ChangeTerm.js",
                "~/Areas/Endorsement/Scripts/ChangeTermRequest.js"
                ));
            //ScoreTypeDoc
            bundles.Add(new ScriptBundle("~/bundle/js/ScoreTypeDoc").Include(
                "~/Areas/Person/Scripts/Person/ScoreTypeDoc.js",
                "~/Areas/Person/Scripts/Person/ScoreTypeDocAdvancedSearch.js"
                ));
            //BusinessBranch
            bundles.Add(new ScriptBundle("~/bundle/js/Prefix").Include(
                "~/Areas/Parametrization/Scripts/Prefix/Prefix.js",
                "~/Areas/Parametrization/Scripts/Prefix/AdvancedSearch.js",
                "~/Areas/Parametrization/Scripts/Prefix/TechnicalBranchBusiness.js",
                "~/Areas/Parametrization/Scripts/Prefix/AditionalInformation.js"
                ));
            //Fasecolda
            bundles.Add(new ScriptBundle("~/bundle/js/Fasecolda").Include(
                "~/Areas/ReportsFasecolda/Scripts/Fasecolda/Fasecolda.js",
                "~/Areas/ReportsFasecolda/Scripts/Fasecolda/LogFasecolda.js"
                ));
            //Cexper

            //ConsultPolice
            bundles.Add(new ScriptBundle("~/bundle/js/ConsultPolice").Include(
                   "~/Areas/Reports/ConsultPolicies/Scripts/ConsultPolice.js"
                   ));
            //Incentive
            bundles.Add(new ScriptBundle("~/bundle/js/Incentive").Include(
                   "~/Areas/Reports/Scripts/Incentive/Incentive.js"
                   ));
            //ReportQuotation
            bundles.Add(new ScriptBundle("~/bundle/js/ReportQuotation").Include(
                "~/Areas/Reports/Scripts/Quotation/VehicleIndicators.js"
                ));
            //ScoreCredit
            //bundles.Add(new ScriptBundle("~/bundle/js/ScoreCredit").Include(
            //    "~/Areas/Reports/Scripts/ScoreCredit/ScoreCredit.js"
            //    ));
            //Sup
            bundles.Add(new ScriptBundle("~/bundle/js/Sup").Include(
                "~/Areas/Sup/Scripts/Sup.js"
                ));

            //_UIFLayout-r2 
            bundles.Add(new ScriptBundle("~/bundle/js/common-validations").Include(
                "~/Scripts/common-validations.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/blockUI-2.7.0").Include(
                "~/Scripts/blockUI-2.7.0.js"
            ));

            bundles.Add(new StyleBundle("~/bundle/css/unidaddenegocio").Include(
               "~/Content/unidaddenegocio.css"
           ));

            bundles.Add(new StyleBundle("~/bundle/css/PRV-colorStyles").Include(
                "~/Content/PRV-colorStyles.css"
            ));

            bundles.Add(new StyleBundle("~/bundle/css/fontawesome").Include(
                "~/Content/fonts/font-awesome/web-fonts-with-css/css/fontawesome.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundle/css/uif-touch-4.6.9").Include(
                "~/Lib/framework/build/uif-touch-4.6.9.min.css"
            ));

            bundles.Add(new StyleBundle("~/bundle/css/vendors-4.6.9").Include(
                "~/Lib/framework/build/vendors/css/vendors-4.6.9.css"
            ));
            #endregion



            /*_UIFLayout-r2 */

            bundles.Add(new ScriptBundle("~/bundle/js/vendors469").Include(
                "~/Lib/framework/build/vendors/js/vendors-4.6.9.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/touch-4.6.9").Include(
                "~/Lib/framework/build/uif-touch-4.6.9.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/touch-4.0.0").Include(
                "~/Lib/framework/build/uif-touch-4.0.0-alpha.es.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/polyfills").Include(
                "~/Lib/framework/build/polyfills.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/Menu").Include(
                "~/Scripts/Menu.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/common-validations").Include(
                "~/Scripts/common-validations.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/jquery.center").Include(
                "~/Scripts/jquery.center.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/jquery.msg").Include(
                "~/Scripts/jquery.msg.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/blockUI-2.7.0").Include(
                "~/Scripts/blockUI-2.7.0.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/start").Include(
                "~/Scripts/app/start.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/JSLINQ").Include(
                "~/Scripts/JSLINQ.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/ValidateSession").Include(
                "~/Scripts/ValidateSession.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/js/es6-promise.auto").Include(
                "~/Scripts/es6-promise.auto.min.js"
            ));
            /*_UIFLayout-r2 */

            var compileDebug = (CompilationSection)ConfigurationManager.GetSection("system.web/compilation");

            if (compileDebug.Debug)
            {
                #region
                //Notification
                bundles.Add(new ScriptBundle("~/bundle/js/Notification").Include(
                    "~/Areas/Notification/Scripts/Notification.js"
                //"~/Scripts/Hubs/NotificationHub.js"
                ));


                bundles.Add(new ScriptBundle("~/bundle/js/VehicleVersionParametrization").Include(
                "~/Areas/Parametrization/Scripts/VehicleVersion/VehicleVersion.js",
                "~/Areas/Parametrization/Scripts/VehicleVersion/VehicleVersionSearch.js"
                ));


                //VehicleType
                bundles.Add(new ScriptBundle("~/bundle/js/VehicleType").Include(
                    "~/Areas/Parametrization/Scripts/VehicleType/VehicleBody.js",
                    "~/Areas/Parametrization/Scripts/VehicleType/VehicleType.js"
                    ));
                //Parametrization/SubLineBusiness
                bundles.Add(new ScriptBundle("~/bundle/js/SubLineBusiness").Include(
                    "~/Areas/Parametrization/Scripts/SubLineBusiness/SubLineBusinessAdvancedSearch.js",
                    "~/Areas/Parametrization/Scripts/SubLineBusiness/SubLineBusiness.js",
                    "~/Areas/Parametrization/Scripts/SubLineBusiness/SubLineBusinessRequest.js"
                    ));

                //Parametrization/Expense
                bundles.Add(new ScriptBundle("~/bundle/js/Expense").Include(
                    "~/Areas/Parametrization/Scripts/Expense/Expense.js"
                    ));

                //Parametrization/Coverage
                bundles.Add(new ScriptBundle("~/bundle/js/CoverageParametrization").Include(
                    "~/Areas/Parametrization/Scripts/Coverage/CoverageParametrization.js",
                    "~/Areas/Parametrization/Scripts/Coverage/ClausesParametrization.js",
                    "~/Areas/Parametrization/Scripts/Coverage/DeductiblesParametrization.js",
                    "~/Areas/Parametrization/Scripts/Coverage/DetailTypesParametrization.js",
                    "~/Areas/Parametrization/Scripts/Coverage/CoverageParametrizationAdvancedSearch.js",
                    "~/Areas/Parametrization/Scripts/Coverage/Coverage2GParametrization.js",
                    "~/Areas/Parametrization/Scripts/Coverage/PrintCoveragesParametrization.js"
                    ));


                //Parametrization/Infringement
                bundles.Add(new ScriptBundle("~/bundle/js/Infringement").Include(
                    "~/Areas/Parametrization/Scripts/Infringement/Infringement.js"
                    ));

                //Parametrization/InfringementGroup
                bundles.Add(new ScriptBundle("~/bundle/js/InfringementGroup").Include(
                    "~/Areas/Parametrization/Scripts/InfringementGroup/InfringementGroup.js"
                    ));

                //Parametrization/SalePoint
                bundles.Add(new ScriptBundle("~/bundle/js/SalePoint").Include(
                   "~/Areas/Parametrization/Scripts/SalePoint/SalePoint.js",
                   "~/Areas/Parametrization/Scripts/SalePoint/SalePointAdvancedSearch.js"

                   ));

                bundles.Add(new ScriptBundle("~/bundle/js/LegalRepresentativeSing").Include(
                            "~/Areas/Printing/Scripts/LegalRepresentativeSing/LegalRepresentativeSing.js",
                            "~/Areas/Printing/Scripts/LegalRepresentativeSing/LegalRepresentativeSingAdvancedSearch.js"
                            ));
                //Prospect// Estos archivos ya existen en Person 
                //bundles.Add(new ScriptBundle("~/bundle/js/Prospect").Include(
                //    "~/Areas/Prospect/Scripts/Prospect/ProspectusNatural.js",
                //    "~/Areas/Prospect/Scripts/Prospect/Shared.js",
                //    "~/Areas/Prospect/Scripts/Prospect/ProspectusLegal.js",
                //    "~/Areas/Prospect/Scripts/Prospect/ProspectRequest.js"
                //    ));
                //Person
                bundles.Add(new ScriptBundle("~/bundle/js/Person").Include(
                    "~/Areas/Person/Scripts/Person/Person.js",
                    "~/Areas/Person/Scripts/Person/PersonRequest.js",
                    "~/Areas/Person/Scripts/Person/Shared.js",
                    "~/Areas/Person/Scripts/Person/Sarlaft.js",
                    "~/Areas/Person/Scripts/Person/Address.js",
                    "~/Areas/Person/Scripts/Person/StaffLabour.js",
                    "~/Areas/Person/Scripts/Person/Phone.js",
                    "~/Areas/Person/Scripts/Person/Email.js",
                    "~/Areas/Person/Scripts/Person/EditBasicInfo.js",
                    "~/Areas/Person/Scripts/Person/MethodPayment.js",
                    "~/Areas/Person/Scripts/Person/OperatingQuota.js",
                    "~/Areas/Person/Scripts/Person/Insured.js",
                    "~/Areas/Person/Scripts/Person/AdvancedSearch.js",
                    "~/Areas/Person/Scripts/Person/RepresentLegal.js",
                    "~/Areas/Person/Scripts/Person/Partner.js",
                    "~/Areas/Person/Scripts/Person/ConsortiumMembers.js",
                    "~/Areas/Person/Scripts/Person/BusinessName.js",
                    "~/Areas/Person/Scripts/Person/Agent.js",
                    "~/Areas/Person/Scripts/Person/CoInsurer.js",
                    "~/Areas/Person/Scripts/Person/ReInsurer.js",
                    "~/Areas/Person/Scripts/Person/Agency.js",
                    "~/Areas/Person/Scripts/Person/Provider.js",
                    "~/Areas/Person/Scripts/Person/Tax.js",
                    "~/Areas/Person/Scripts/Person/AdditionalData.js",
                    "~/Areas/Person/Scripts/Person/PersonNatural.js",
                    "~/Areas/Person/Scripts/Person/PersonLegal.js",
                    "~/Areas/Person/Scripts/Person/ProspectusNatural.js",
                    "~/Areas/Person/Scripts/Person/ProspectusLegal.js",
                    "~/Areas/Person/Scripts/Person/AgentAlliance.js",
                     "~/Areas/Person/Scripts/Person/ComissionAgency.js",
                     "~/Areas/Person/Scripts/Person/BasicInformation.js",
                     "~/Areas/Person/Scripts/Person/BasicInformationAdvancedSearch.js",
                     "~/Areas/Person/Scripts/Person/Third.js",
                     "~/Areas/Person/Scripts/Employee/Employee.js",
                    "~/Areas/Person/Scripts/Employee/EmployeeRequest.js",
                    "~/Areas/Person/Scripts/EconomicGroup/EconomicGroup.js",
                    "~/Areas/Person/Scripts/EconomicGroup/EconomicGroupRequest.js",
                    "~/Areas/Person/Scripts/EconomicGroup/AdvancedSearchGroup.js",
                    "~/Areas/Person/Scripts/EconomicGroup/AdvancedSearchGroupRequest.js",
                     "~/Areas/Person/Scripts/Person/BankTransfers.js",
                    "~/Areas/Person/Scripts/Person/BankTransfersRequest.js",
                     "~/Areas/Person/Scripts/Person/ElectronicBilling.js",
                    "~/Areas/Person/Scripts/Person/ElectronicBillingRequest.js"

                    ));
                //Guarantee
                bundles.Add(new ScriptBundle("~/bundle/js/Guarantee").Include(
                    "~/Areas/Person/Scripts/Guarantee/GuaranteeRequest.js",
                    "~/Areas/Person/Scripts/Guarantee/Guarantee.js",
                    "~/Areas/Person/Scripts/Guarantee/DocumentationReceived.js",
                    "~/Areas/Person/Scripts/Guarantee/Binnacle.js",
                    "~/Areas/Person/Scripts/Guarantee/PrefixAssociated.js",
                    "~/Areas/Person/Scripts/Guarantee/BindPolicy.js",
                    "~/Areas/Person/Scripts/Guarantee/Guarantors.js",
                    "~/Areas/Person/Scripts/Guarantee/Mortage.js",
                    "~/Areas/Person/Scripts/Guarantee/Others.js",
                    "~/Areas/Person/Scripts/Guarantee/Pledge.js",
                    "~/Areas/Person/Scripts/Guarantee/Actions.js",
                    "~/Areas/Person/Scripts/Guarantee/PromissoryNote.js",
                    "~/Areas/Person/Scripts/Guarantee/FixedTermDeposit.js",
                    "~/Areas/Person/Scripts/Resources.Person.es.js"
                    ));

                //Guarantees
                bundles.Add(new ScriptBundle("~/bundle/js/Guarantees").Include(
                    "~/Areas/Guarantees/Scripts/Guarantees/SearchInsured.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/GuaranteeRequest.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Guarantee.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/DocumentationReceived.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Binnacle.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/PrefixAssociated.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/BindPolicy.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Guarantors.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Actions.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Mortage.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Others.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Pledge.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/PromissoryNote.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/FixedTermDeposit.js",
                    "~/Areas/Guarantees/Scripts/Resources.Person.es.js"
                    ));

                //Search Insured
                bundles.Add(new ScriptBundle("~/bundle/js/SearchInsured").Include(
                    "~/Areas/Guarantees/Scripts/Guarantees/SearchInsured.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/DocumentationReceived.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Binnacle.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/PrefixAssociated.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/BindPolicy.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Guarantors.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Mortage.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Others.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Pledge.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Actions.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/PromissoryNote.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/FixedTermDeposit.js"

                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/RenewalMassive").Include(
                    //"~/Areas/Massive/Scripts/Renewal/Renewal.js",
                    "~/Areas/Massive/Scripts/Renewal/AdvancedSearchRenewal.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/RenewalRequestGrouping").Include(
                    //"~/Areas/Massive/Scripts/RenewalRequestGrouping/RenewalRequestGrouping.js",
                    "~/Areas/Massive/Scripts/RenewalRequestGrouping/Agents.js",
                    "~/Areas/Massive/Scripts/RenewalRequestGrouping/CoInsurance.js",
                    "~/Areas/Massive/Scripts/RenewalRequestGrouping/AdvancedSearch.js"
                    ));

                //UniqueUser         
                bundles.Add(new ScriptBundle("~/bundle/js/UniqueUser").Include(
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UniqueUser.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/Hierarchy.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/Branch.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserAgent.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/PersonOnline.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserAdvancedSearch.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/AlliedSalePoints.js",
                    //"~/Areas/UniqueUser/Scripts/UniqueUser/UserProduct.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/PrefixUser.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserGroup.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserPermissions.js"
                    ));

                //
                bundles.Add(new ScriptBundle("~/bundle/js/Module").Include(
                    "~/Areas/UniqueUser/Scripts/Module/Module.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/SubModule").Include(
                    "~/Areas/UniqueUser/Scripts/SubModule/SubModule.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Profile").Include(
                    "~/Areas/UniqueUser/Scripts/Profile/Profile.js",
                    "~/Areas/UniqueUser/Scripts/Profile/ProfileAdvancedSearch.js",
                    "~/Areas/UniqueUser/Scripts/Profile/AccessProfile.js",
                    "~/Areas/UniqueUser/Scripts/Profile/CopyProfile.js",
                    "~/Areas/UniqueUser/Scripts/Profile/ProfileAdvancedSearch.js",
                    "~/Areas/UniqueUser/Scripts/Profile/ProfileContextPermissions.js",
                    "~/Areas/UniqueUser/Scripts/Profile/GuaranteeStatus.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Access").Include(
                    "~/Areas/UniqueUser/Scripts/Access/Access.js",
                    "~/Areas/UniqueUser/Scripts/Access/AccessAdvancedSearch.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Common").Include(
                    "~/Areas/Common/Scripts/Common/Main.js",
                     "~/Areas/Common/Scripts/Common/Agents.js",
                     "~/Areas/Common/Scripts/Common/Request.js"
                    ));
                //BusinessBranch
                bundles.Add(new ScriptBundle("~/bundle/js/Prefix").Include(
                    "~/Areas/Parametrization/Scripts/Prefix/Prefix.js",
                    "~/Areas/Parametrization/Scripts/Prefix/AdvancedSearch.js",
                    "~/Areas/Parametrization/Scripts/Prefix/TechnicalBranchBusiness.js",
                    "~/Areas/Parametrization/Scripts/Prefix/AditionalInformation.js"
                    ));
                //FalseColda
                bundles.Add(new ScriptBundle("~/bundle/js/Fasecolda").Include(
                "~/Areas/ReportsFasecolda/Scripts/Fasecolda/Fasecolda.js",
                "~/Areas/ReportsFasecolda/Scripts/Fasecolda/LogFasecolda.js"
                ));
                //Cexper
                //ReportQuotation
                bundles.Add(new ScriptBundle("~/bundle/js/ReportQuotation").Include(
                    "~/Areas/Reports/Scripts/Quotation/VehicleIndicators.js"
                    ));
                //ConsultPolice
                bundles.Add(new ScriptBundle("~/bundle/js/ConsultPolice").Include(
                "~/Areas/Reports/ConsultPolicies/Scripts/ConsultPolice.js"
                ));
                //Incentive
                bundles.Add(new ScriptBundle("~/bundle/js/Incentive").Include(
                "~/Areas/Reports/Scripts/Incentive/Incentive.js"
                ));



                // ParametrizationRequest
                bundles.Add(new ScriptBundle("~/bundle/js/ParametrizationRequest").Include(
                    "~/Areas/Parametrization/Scripts/Parametrization.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Protection").Include(
                    "~/Areas/Parametrization/Scripts/Parametrization.js",
                    "~/Areas/Parametrization/Scripts/Protection/Protection.js",
                    "~/Areas/Parametrization/Scripts/Protection/ProtectionAdvancedSearch.js"
                    ));


                //Technical Branch
                bundles.Add(new ScriptBundle("~/bundle/js/TechnicalBranch").Include(
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/TechnicalBranch.js",
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/InsuranceObjects.js",
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/AdvancedSearchTechnicalBranch.js",
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/RiskTypeTechnicalBranch.js",
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/Protections.js"
                    ));

                //LineBusiness
                bundles.Add(new ScriptBundle("~/bundle/js/LineBusinessParametrization").Include(
                    "~/Areas/Parametrization/Scripts/LineBusiness/AdvancedSearch.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/Clauses.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/InsuranceObjects.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/Protections.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/RiskType.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/LineBusiness.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/LineBusinessRequest.js"
                    ));

                //Parametrization/Alliance
                bundles.Add(new ScriptBundle("~/bundle/js/Alliance").Include(
                    "~/Areas/Parametrization/Scripts/Alliance/Alliance.js"
                    ));

                //Parametrization/BranchAlliance
                bundles.Add(new ScriptBundle("~/bundle/js/BranchAlliance").Include(
                    "~/Areas/Parametrization/Scripts/BranchAlliance/BranchAlliance.js"
                    ));

                //Parametrization/InsurancesObjects
                bundles.Add(new ScriptBundle("~/bundle/js/InsurancesObjects").Include(
                    "~/Areas/Parametrization/Scripts/InsuredObject/InsurancesObjects.js",
                      "~/Areas/Parametrization/Scripts/InsuredObject/InsuredObjectAdvancedSearch.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/Branch").Include(
                    "~/Areas/Parametrization/Scripts/Branch/Branch.js",
                    "~/Areas/Parametrization/Scripts/Branch/BranchAdvancedSearch.js"
                    ));


                //Parametrization/InfringementState
                bundles.Add(new ScriptBundle("~/bundle/js/InfringementState").Include(
                    "~/Areas/Parametrization/Scripts/InfringementState/InfringementState.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/Channel").Include(
                    "~/Areas/Parametrization/Scripts/Channel/Channel.js",
                    "~/Areas/Parametrization/Scripts/Channel/ChannelAdvancedSearch.js",
                    "~/Areas/Parametrization/Scripts/Channel/ValuesDefault.js"
                    ));

                //Parametrization/EconomicGroup
                bundles.Add(new ScriptBundle("~/bundle/js/EconomicGroup").Include(
                    "~/Areas/person/Scripts/EconomicGroup/EconomicGroup.js",
                    "~/Areas/person/Scripts/EconomicGroup/EconomicGroupRequest.js",
                    "~/Areas/person/Scripts/EconomicGroup/AdvancedSearchGroup.js",
                    "~/Areas/person/Scripts/EconomicGroup/AdvancedSearchGroupRequest.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/SearchEndorsementPP").Include(
                    "~/Areas/Endorsement/Scripts/AdvancedSearch.js"
                    ));
                //JudicialSurety
                bundles.Add(new ScriptBundle("~/bundle/js/JudicialSurety").Include(
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/RiskJudicialSurety.js",
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/CoverageJudicialSurety.js",
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/CrossGuarantees.js",
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/AdditionalData.js",
                     "~/Areas/Underwriting/Scripts/RiskJudicialSurety/RiskJudicialRequest.js"
                ));
                //Authorization Policies
                bundles.Add(new ScriptBundle("~/bundle/js/AuthorizationPolicies").Include(
                    "~/Areas/AuthorizationPolicies/Scripts/Policies/Policies.js",
                    "~/Areas/AuthorizationPolicies/Scripts/Authorize/Authorize.js",
                    "~/Areas/AuthorizationPolicies/Scripts/Policies/PoliciesRules.js",
                    "~/Areas/AuthorizationPolicies/Scripts/Policies/AdvancedSearchPolicy.js",
                    "~/Areas/AuthorizationPolicies/Scripts/WorkFlowPolicies/WorkFlowPolicies.js",
                    "~/Areas/AuthorizationPolicies/Scripts/WorkFlowPolicies/WorkFlowPoliciesRequest.js",
                    "~/Areas/AuthorizationPolicies/Scripts/AuthorizationPersonRiskList/AuthorizationPersonRiskList.js",
                    "~/Areas/AuthorizationPolicies/Scripts/AuthorizationPersonRiskList/AuthorizationPersonRiskListRequest.js",
                    "~/Areas/AuthorizationPolicies/Scripts/AuthorizationSarlaftOperation/AuthorizationSarlaftOperation.js",
                    "~/Areas/AuthorizationPolicies/Scripts/AuthorizationSarlaftOperation/AuthorizationSarlaftOperationRequest.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/LaunchPolicies").Include(
                    "~/Areas/AuthorizationPolicies/Scripts/LaunchPolicies.js",
                    "~/Areas/AuthorizationPolicies/Scripts/Summary/Summary.js"
                    ));

                //Risks
                bundles.Add(new ScriptBundle("~/bundle/js/Risk").Include(
                    "~/Areas/Underwriting/Scripts/Risks/Beneficiaries.js",
                    "~/Areas/Underwriting/Scripts/Risks/Texts.js",
                    "~/Areas/Underwriting/Scripts/Risks/Clauses.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/ExecuteScript.js",
                    "~/Areas/RulesScripts/Scripts/SearchCombo/SearchCombo.js"
                    ));
                //Coverages
                bundles.Add(new ScriptBundle("~/bundle/js/Coverage").Include(
                    "~/Areas/Underwriting/Scripts/Coverages/Texts.js",
                    "~/Areas/Underwriting/Scripts/Coverages/Clauses.js",
                    "~/Areas/Underwriting/Scripts/Coverages/Deductible.js",
                     "~/Areas/Underwriting/Scripts/Coverages/DeductibleRequest.js",
                    "~/Areas/Underwriting/Scripts/Coverages/CoverageRequest.js"
                    ));
                //Vehicle
                bundles.Add(new ScriptBundle("~/bundle/js/Vehicle").Include(
                    "~/Areas/Underwriting/Scripts/RiskVehicle/Vehicle.js",
                    "~/Areas/Underwriting/Scripts/RiskVehicle/Accessories.js",
                    "~/Areas/Underwriting/Scripts/RiskVehicle/AdditionalData.js",
                    "~/Areas/Underwriting/Scripts/RiskVehicle/Coverage.js",
                    "~/Areas/Underwriting/Scripts/RiskVehicle/RiskVehicleRequest.js"
                    ));
                //Marine
                bundles.Add(new ScriptBundle("~/bundle/js/Marine").Include(
                    "~/Areas/Underwriting/Scripts/RiskMarine/RiskMarine.js",
                    "~/Areas/Underwriting/Scripts/RiskMarine/RiskMarineCoverage.js",
                    "~/Areas/Underwriting/Scripts/RiskMarine/RiskMarineCoverageRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskMarine/RiskMarineRequest.js"
                    ));
                //Aircraft
                bundles.Add(new ScriptBundle("~/bundle/js/Aircraft").Include(
                    "~/Areas/Underwriting/Scripts/RiskAircraft/RiskAircraft.js",
                    "~/Areas/Underwriting/Scripts/RiskAircraft/RiskAircraftCoverage.js",
                    "~/Areas/Underwriting/Scripts/RiskAircraft/RiskAircraftCoverageRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskAircraft/RiskAircraftRequest.js"
                    ));
                //Underwriting
                bundles.Add(new ScriptBundle("~/bundle/js/Temporal").Include(
                    "~/Areas/Underwriting/Scripts/Underwriting/Temporal.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/TemporalRiskUnderwriting.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/TemporalAdvancedSearch.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Endorsement.js"

                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Quotation").Include(
                    "~/Areas/Underwriting/Scripts/Underwriting/Quotation.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/QuotationAdvancedSearch.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Underwriting").Include(
                    "~/Areas/Underwriting/Scripts/Underwriting/AdditionalData.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Alliance.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Agents.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Clauses.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Texts.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Underwriting.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/CoInsurance.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Beneficiaries.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/PaymentPlan.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/PersonOnline.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/UnderwritingRequest.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/IssueWithEvent.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/ModalExpenses.js",
                     //"~/Areas/Underwriting/Scripts/Underwriting/TableExpreses.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/Surcharges.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/Discounts.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/Taxes.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/Promissory.js"

                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementChangeAgent").Include(
                  "~/Areas/Endorsement/Scripts/ChangeAgent.js",
                  "~/Areas/Endorsement/Scripts/Request/PolicyRequest.js"
                ));
                bundles.Add(new ScriptBundle("~/bundle/js/RequestGrouping").Include(
               "~/Areas/Massive/Scripts/RequestGrouping/RequestGrouping.js",
               "~/Areas/Massive/Scripts/RequestGrouping/Agents.js",
               "~/Areas/Massive/Scripts/RequestGrouping/BillingGroup.js",
               "~/Areas/Massive/Scripts/RequestGrouping/RequestGroup.js",
               "~/Areas/Massive/Scripts/RequestGrouping/CoInsurance.js"
               ));
                //Parametrization/Deductible
                bundles.Add(new ScriptBundle("~/bundle/js/DeductibleParametrization").Include(
                    "~/Areas/Parametrization/Scripts/Deductible/AdvancedSearchParametrization.js",
                     "~/Areas/Parametrization/Scripts/Deductible/DeductibleParametrization.js",
                    "~/Areas/Parametrization/Scripts/Deductible/DeductibleParametrizationRequest.js"
                    ));

                //Parametrization/Clause
                bundles.Add(new ScriptBundle("~/bundle/js/ClauseParametrization").Include(
                    "~/Areas/Parametrization/Scripts/Clause/ClauseDetail.js",
                    "~/Areas/Parametrization/Scripts/Clause/ClauseParametrization.js",
                    "~/Areas/Parametrization/Scripts/Clause/ClauseParametrizationRequest.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/PaymentPlanParametrization").Include(
                 "~/Areas/Parametrization/Scripts/PaymentPlan/PaymentPlan.js",
                 "~/Areas/Parametrization/Scripts/PaymentPlan/DistributionQuota.js",
                 "~/Areas/Parametrization/Scripts/PaymentPlan/PaymentPlanAdvanced.js"
               ));
                //Insured Profile
                bundles.Add(new ScriptBundle("~/bundle/js/InsuredProfile").Include(
                       "~/Areas/Parametrization/Scripts/InsuredProfile/InsuredProfile.js",
                       "~/Areas/Parametrization/Scripts/InsuredProfile/InsuredProfileAdvancedSearch.js"
                       ));
                //Insured Segment
                bundles.Add(new ScriptBundle("~/bundle/js/InsuredSegment").Include(
                       "~/Areas/Parametrization/Scripts/InsuredSegment/InsuredSegment.js",
                       "~/Areas/Parametrization/Scripts/InsuredSegment/InsuredSegmentAdvancedSearch.js"
                       ));
                //Alliance Print Format
                bundles.Add(new ScriptBundle("~/bundle/js/AlliancePrintFormat").Include(
                       "~/Areas/Parametrization/Scripts/AlliancePrintFormat/AlliancePrintFormat.js",
                       "~/Areas/Parametrization/Scripts/AlliancePrintFormat/AlliancePrintFormatAdvancedSearch.js"
                       ));
                //Covered Risk Type
                bundles.Add(new ScriptBundle("~/bundle/js/CoveredRiskType").Include(
                       "~/Areas/Parametrization/Scripts/CoveredRiskType/CoveredRiskType.js"
                       ));
                //Parametrization FinancialPlan
                bundles.Add(new ScriptBundle("~/bundle/js/FinancialPlan").Include(
                    "~/Areas/Parametrization/Scripts/FinancialPlan/FinancialPlanParametrization.js",
                    "~/Areas/Parametrization/Scripts/FinancialPlan/FinancialPlanParametrizationRequest.js"
                    ));
                //CompanyAddressType
                bundles.Add(new ScriptBundle("~/bundle/js/CompanyAddressType").Include(
                    "~/Areas/Person/Scripts/Person/CompanyAddressType.js",
                    "~/Areas/Person/Scripts/Person/CompanyAddressTypeAdvancedSearch.js"
                    ));
                //CompanyPhoneType
                bundles.Add(new ScriptBundle("~/bundle/js/CompanyPhoneType").Include(
                    "~/Areas/Person/Scripts/Person/CompanyPhoneType.js",
                    "~/Areas/Person/Scripts/Person/CompanyPhoneTypeAdvancedSearch.js"
                    ));
                //ScoreTypeDoc
                bundles.Add(new ScriptBundle("~/bundle/js/ScoreTypeDoc").Include(
                    "~/Areas/Person/Scripts/Person/ScoreTypeDoc.js",
                    "~/Areas/Person/Scripts/Person/ScoreTypeDocAdvancedSearch.js"
                    ));

                //Productos
                bundles.Add(new ScriptBundle("~/bundle/js/Product").Include(
                    "~/Areas/Product/Scripts/Product/Product.js",
                    "~/Areas/Product/Scripts/Product/ProductRequestT.js",
                    "~/Areas/Product/Scripts/Product/Currency.js",
                    "~/Areas/Product/Scripts/Product/PolicyType.js",
                    "~/Areas/Product/Scripts/Product/RiskType.js",
                    "~/Areas/Product/Scripts/Product/Agent.js",
                    "~/Areas/Product/Scripts/Product/AgentRequestT.js",
                    "~/Areas/Product/Scripts/Product/PaymentPlan.js",
                    "~/Areas/Product/Scripts/Product/PaymentPlanRequestT.js",
                    "~/Areas/Product/Scripts/Product/InsuredObject.js",
                    "~/Areas/Product/Scripts/Product/InsuredObjectRequestT.js",
                    "~/Areas/Product/Scripts/Product/Coverage.js",
                    "~/Areas/Product/Scripts/Product/CoverageRequestT.js",
                    "~/Areas/Product/Scripts/Product/CoverageAllied.js",
                    "~/Areas/Product/Scripts/Product/RulesAndscript.js",
                    "~/Areas/Product/Scripts/Product/RulesAndscriptRequestT.js",
                    "~/Areas/Product/Scripts/Product/Script.js",
                    "~/Areas/Product/Scripts/Product/ScriptRequestT.js",
                    "~/Areas/Product/Scripts/Product/CopyProduct.js",
                    "~/Areas/Product/Scripts/Product/CopyProductRequest.js",
                    "~/Areas/Product/Scripts/Product/AdditionalData.js",
                    "~/Areas/Product/Scripts/Product/AdditionalDataRequestT.js",
                    "~/Areas/Product/Scripts/Product/CommisionAgent.js",
                    "~/Areas/Product/Scripts/Product/ProductResources.es.js",
                    "~/Areas/Product/Scripts/Product/AdvancedSearch.js",
                    "~/Areas/Product/Scripts/Product/AdvancedSearchRequest.js",
                    "~/Areas/Product/Scripts/Product/DeductiblesByCoverage.js",
                    "~/Areas/Product/Scripts/Product/DeductiblesByCoverageRequest.js",
                    "~/Areas/Product/Scripts/Product/IncentivesForAgents.js",
                     "~/Areas/Product/Scripts/Product/IncentivesForAgentsRequest.js",
                    "~/Areas/Product/Scripts/Product/UifTouch_Co.js",
                    "~/Areas/Product/Scripts/Product/TypeOfAssistance.js"
                    ));
                //Variable
                bundles.Add(new ScriptBundle("~/bundle/js/Variable").Include(
                    "~/Areas/Parametrization/Scripts/Variable/Variable.js"
                    ));
                ///Parametrization Vehicle
                bundles.Add(new ScriptBundle("~/bundle/js/Parametrization").Include(
                    "~/Areas/Parametrization/Scripts/Vehicle/Fasecolda.js",
                    "~/Areas/Parametrization/Scripts/Vehicle/AdvancedSearchFasecolda.js",
                    "~/Areas/Parametrization/Scripts/Vehicle/VehicleFasecolda.js",
                     "~/Areas/Parametrization/Scripts/Vehicle/Concessionaire.js"
                    ));

                ////Parametrization/ProtectionParametrization
                bundles.Add(new ScriptBundle("~/bundle/js/ProtectionParametrization").Include(
                    "~/Areas/Parametrization/Scripts/Protection/ProtectionParametrization.js",
                    "~/Areas/Parametrization/Scripts/Protection/ProtectionParametrizationRequest.js",
                    "~/Areas/Parametrization/Scripts/Protection/ProtectionAdvancedSearch.js"
                ));

                //Parametrization/Surcharge
                bundles.Add(new ScriptBundle("~/bundle/js/Surcharge").Include(
                       "~/Areas/Parametrization/Scripts/Surcharge/Surcharge.js"
                       ));

                //Parametrization/Discount
                bundles.Add(new ScriptBundle("~/bundle/js/Discount").Include(
                       "~/Areas/Parametrization/Scripts/Discount/Discount.js"
                       ));
                //Parametrizaci?n detalle
                bundles.Add(new ScriptBundle("~/bundle/js/DetailParametrization").Include(
                "~/Areas/Parametrization/Scripts/Detail/DetailParametrization.js"
              ));
                ////Parametrization/CoverageGroupParametrization
                bundles.Add(new ScriptBundle("~/bundle/js/CoverageGroupParametrization").Include(
                    "~/Areas/Parametrization/Scripts/CoverageGroup/CoverageGroupParametrization.js"
                ));
                ////Parametrization/PaymentMethod
                bundles.Add(new ScriptBundle("~/bundle/js/PaymentMethod").Include(
                "~/Areas/Parametrization/Scripts/PaymentMethod/PaymentMethod.js"
                ));

                //Type Assistance
                bundles.Add(new ScriptBundle("~/bundle/js/AssistanceType").Include(
                    "~/Areas/Parametrization/Scripts/AssistanceType/AssistanceType.js",
                    "~/Areas/Parametrization/Scripts/AssistanceType/AssistanceAdvancedSearch.js",
                    "~/Areas/Parametrization/Scripts/AssistanceType/AsistenceText.js"
                ));
                //Product 2G
                bundles.Add(new ScriptBundle("~/bundle/js/Product2G").Include(
                    "~/Areas/Parametrization/Scripts/Product2G/Product2G.js",
                    "~/Areas/Parametrization/Scripts/Product2G/Product2GAdvancedSearch.js"
                ));
                //BusinessConfiguration
                bundles.Add(new ScriptBundle("~/bundle/js/BusinessConfiguration").Include(
                    "~/Areas/Parametrization/Scripts/BusinessConfiguration/BusinessConfiguration.js",
                    "~/Areas/Parametrization/Scripts/BusinessConfiguration/BusinessConfigurationAdvancedSearch.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/QuotationNumerationParametrization").Include(
                    "~/Areas/Parametrization/Scripts/QuotationNumeration/QuotationNumerationParametrization.js",
                    "~/Areas/Parametrization/Scripts/QuotationNumeration/QuotationNumerationParametrizationRequest.js"
                    ));
                //WorkerType
                bundles.Add(new ScriptBundle("~/bundle/js/WorkerType").Include(
                    "~/Areas/Parametrization/Scripts/WorkerType/WorkerType.js"
                    ));
                // Parametrization/PolicyNumeration
                bundles.Add(new ScriptBundle("~/bundle/js/PolicyNumerationParametrization").Include(
                   "~/Areas/Parametrization/Scripts/PolicyNumeration/PolicyNumerationParametrization.js",
                   "~/Areas/Parametrization/Scripts/PolicyNumeration/PolicyNumerationParametrizationRequest.js"
                   ));

                //DelegationParametrization
                bundles.Add(new ScriptBundle("~/bundle/js/DelegationParametrization").Include(
                    "~/Areas/ParamAuthorizationPolicies/Scripts/Delegation/DelegationParametrization.js",
                    "~/Areas/ParamAuthorizationPolicies/Scripts/Delegation/DelegationParametrizationRequest.js"
                    ));
                //Auditoria
                bundles.Add(new ScriptBundle("~/bundle/js/Audit").Include(
                  "~/Areas/Audit/Scripts/Audit/Audit.js",
                  "~/Areas/Audit/Scripts/Audit/AuditRequest.js"
                  ));
                // RatingZone
                bundles.Add(new ScriptBundle("~/bundle/js/RatingZoneParametrization").Include(
                    "~/Areas/Parametrization/Scripts/RatingZone/RatingZone.js"
                ));
                //Concepts
                bundles.Add(new ScriptBundle("~/bundle/js/Concepts").Include(
                    "~/Areas/RulesScripts/Scripts/Concepts/ListEntity.js",
                    "~/Areas/RulesScripts/Scripts/Concepts/ListEntityAdvancedSearch.js",
                    "~/Areas/RulesScripts/Scripts/Concepts/RangeEntity.js",
                    "~/Areas/RulesScripts/Scripts/Concepts/RangeEntityAdvancedSearch.js"
                    ));

                //Guiones
                bundles.Add(new ScriptBundle("~/bundle/js/Scripts").Include(
                    "~/Areas/RulesScripts/Scripts/SearchCombo/SearchCombo.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/Enum.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/ScriptObj.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/Scripts.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/ExecuteScript.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/ViewScript.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/AdvancedSearchScript.js"
                ));
                //VehicleBody
                bundles.Add(new ScriptBundle("~/bundle/js/VehicleBody").Include(
                    "~/Areas/Parametrization/Scripts/VehicleBody/VehicleUse.js",
                    "~/Areas/Parametrization/Scripts/VehicleBody/VehicleBody.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/VehicleModel").Include(
                    "~/Areas/Parametrization/Scripts/VehicleModel/VehicleModel.js",
                    "~/Areas/Parametrization/Scripts/VehicleModel/VehicleModelSearch.js"

                    ));
                //Limit Rc
                bundles.Add(new ScriptBundle("~/bundle/js/LimitRc").Include(
                       "~/Areas/Parametrization/Scripts/LimitRc/LimitRc.js"
                       ));

                //VehicleVersionYear
                bundles.Add(new ScriptBundle("~/bundle/js/VehicleVersionYear").Include(
                    "~/Areas/Parametrization/Scripts/VehicleVersionYear/Index.js",
                    "~/Areas/Parametrization/Scripts/VehicleVersionYear/AdvancedSearch.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/TechnicalPlan").Include(
                 "~/Areas/Parametrization/Scripts/TechnicalPlan/TechnicalPlan.js",
                 "~/Areas/Parametrization/Scripts/TechnicalPlan/TechnicalPlanAllyCoverages.js",
                 "~/Areas/Parametrization/Scripts/TechnicalPlan/TechnicalPlanCoverages.js",
                 "~/Areas/Parametrization/Scripts/TechnicalPlan/TechnicalPlanSearch.js"
               ));

                //Reglas
                bundles.Add(new ScriptBundle("~/bundle/js/Rules").Include(
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/RuleSet.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalRuleSet.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalRule.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalConditionRule.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalActionRule.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/AdvancedSearch.js"
                    ));
                //Cache
                bundles.Add(new ScriptBundle("~/bundle/js/RuleSetCache").Include(
                    "~/Areas/RulesAndScripts/Scripts/RuleSetCache/RuleSetCache.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Massive").Include(
                "~/Areas/Massive/Scripts/MassiveRequest.js",
                "~/Areas/Massive/Scripts/Massive/Massive.js",
                "~/Areas/Massive/Scripts/Massive/MassiveAdvancedSearch.js"
                ));
                bundles.Add(new ScriptBundle("~/bundle/js/MassiveProcess").Include(
                "~/Areas/Massive/Scripts/MassiveProcess/MassiveProcess.js",
                "~/Areas/Massive/Scripts/MassiveProcess/AdvancedSearchMassiveProcess.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/StrongPassword").Include(
               "~/Scripts/StrongPassword/ChangePassword.js"
               ));

                bundles.Add(new ScriptBundle("~/bundle/js/ReportRenovation").Include(
                    "~/Areas/Reports/RenovationReport/Scripts/ReportRenovation.js"
                    ));
                // GroundsRejectionParametrization
                bundles.Add(new ScriptBundle("~/bundle/js/GroundsRejection").Include(
                 "~/Areas/ParamAuthorizationPolicies/Scripts/GoundsRejection/GroundsRejectionCauses.js",
                 "~/Areas/ParamAuthorizationPolicies/Scripts/GoundsRejection/AdvancedSearch.js"
               ));
                //Parametrization/Tax
                bundles.Add(new ScriptBundle("~/bundle/js/Tax").Include(
                   "~/Areas/Parametrization/Scripts/Tax/Tax.js",
                   "~/Areas/Parametrization/Scripts/Tax/TaxRequests.js",
                   "~/Areas/Parametrization/Scripts/Tax/CategoryTax.js",
                   "~/Areas/Parametrization/Scripts/Tax/CategoryTaxRequests.js",
                   "~/Areas/Parametrization/Scripts/Tax/ConditionTax.js",
                   "~/Areas/Parametrization/Scripts/Tax/ConditionTaxRequests.js",
                   "~/Areas/Parametrization/Scripts/Tax/RateTax.js",
                   "~/Areas/Parametrization/Scripts/Tax/RateTaxRequests.js",
                   "~/Areas/Parametrization/Scripts/Tax/RateTaxAdvancedSearch.js"
                ));
                ///Wallet
                bundles.Add(new ScriptBundle("~/bundle/js/Wallet").Include(
                    "~/Areas/Reports/Scripts/Wallet/Wallet.js"
                ));
                // Group Policies
                bundles.Add(new ScriptBundle("~/bundle/js/GroupPolicies").Include(
                 "~/Areas/AuthorizationPolicies/Scripts/GroupPolicies/GroupPolicies.js",
                  "~/Areas/AuthorizationPolicies/Scripts/GroupPolicies/AdvancedSearchGroupPolicies.js"
               ));
                // Ally Coverage
                bundles.Add(new ScriptBundle("~/bundle/js/AllyCoverage").Include(
                 "~/Areas/Parametrization/Scripts/AllyCoverage/AllyCoverage.js",
                  "~/Areas/Parametrization/Scripts/AllyCoverage/AllyCoverageAdvancedSearch.js"
               ));
                //Ciudades
                bundles.Add(new ScriptBundle("~/bundle/js/City").Include(
                 "~/Areas/Parametrization/Scripts/City/City.js",
                  "~/Areas/Parametrization/Scripts/City/SearchAdvCity.js"
              ));
                bundles.Add(new ScriptBundle("~/bundle/js/MinPremiunRelation").Include(
                "~/Areas/Parametrization/Scripts/MinPremiunRelation/MinPremiunRelation.js",
                "~/Areas/Parametrization/Scripts/MinPremiunRelation/MinPremiunRelationRequest.js"
                ));
                //Textos Precatalogados
                bundles.Add(new ScriptBundle("~/bundle/js/ConditionText").Include(
                 "~/Areas/Parametrization/Scripts/ConditionText/ConditionText.js"
              ));
                // Report Authorization Policies
                bundles.Add(new ScriptBundle("~/bundle/js/ReportAuthorizationPolicies").Include(
                 "~/Areas/AuthorizationPolicies/Scripts/ReportAuthorizationPolicies/ReportAuthorizationPolicies.js"
               ));

                // Report Authorization Policies
                bundles.Add(new ScriptBundle("~/bundle/js/ReassignPolicies").Include(
                 "~/Areas/AuthorizationPolicies/Scripts/ReassignPolicies/ReassignPolicies.js"
               ));
                //Parametrization/TextPrecatalogued
                bundles.Add(new ScriptBundle("~/bundle/js/TextPrecatalogued").Include(
                    "~/Areas/Parametrization/Scripts/TextPrecatalogued/TextPrecataloguedDetail.js",
                    "~/Areas/Parametrization/Scripts/TextPrecatalogued/TextPrecataloguedParametrization.js",
                    "~/Areas/Parametrization/Scripts/TextPrecatalogued/TextPrecataloguedParametrizationRequest.js"
                    ));
                //Parametrization/ValidationPlate
                bundles.Add(new ScriptBundle("~/bundle/js/ValidationPlate").Include(
                    "~/Areas/Parametrization/Scripts/ValidationPlate/ValidationPlateDetail.js",
                    "~/Areas/Parametrization/Scripts/ValidationPlate/ValidationPlateParametrization.js",
                    "~/Areas/Parametrization/Scripts/ValidationPlate/ValidationPlateParametrizationRequest.js"
                    ));
                //Parametrization/DocumentTypeRange
                bundles.Add(new ScriptBundle("~/bundle/js/DocumentTypeRange").Include(
                    "~/Areas/Parametrization/Scripts/DocumentTypeRange/DocumentTypeRange.js"
                    ));
                // bundles.Add(new ScriptBundle("~/bundle/js/ReportAuthorizationPolicies").Include(
                //  "~/Areas/AuthorizationPolicies/Scripts/ReportAuthorizationPolicies/ReportAuthorizationPolicies.js"
                //));
                #endregion

                //Eventos R1
                bundles.Add(new ScriptBundle("~/bundle/js/EventsMain").Include(
                        "~/Areas/Events/Scripts/Events/LaunchEvents.js"
                        ));

                //BILLING PERIOD
                bundles.Add(new ScriptBundle("~/bundle/js/BillingPeriod").Include(
                       "~/Areas/Parametrization/Scripts/BillingPeriod/BillingPeriod.js"
                       ));

                //Business Type
                bundles.Add(new ScriptBundle("~/bundle/js/BusinessTypes").Include(
                       "~/Areas/Parametrization/Scripts/BusinessType/BusinessTypes.js"
                       ));
                //Endosos
                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementClauses").Include(
                    "~/Areas/Endorsement/Scripts/ClausesEndorsementRequest.js",
                    "~/Areas/Endorsement/Scripts/Clauses.js"
                    ));
                // Parametrizacion coverage value
                bundles.Add(new ScriptBundle("~/bundle/js/CoverageValue").Include(
                   "~/Areas/Parametrization/Scripts/CoverageValue/CoverageValue.js",
                    "~/Areas/Parametrization/Scripts/CoverageValue/SearchAdvCoCoverageValue.js"
                 ));
                //Cexper
                bundles.Add(new ScriptBundle("~/bundle/js/Cexper").Include(
                   "~/Areas/Externals/Scripts/Cexper.js"
                 ));
                //Sisa
                bundles.Add(new ScriptBundle("~/bundle/js/Sisa").Include(
                   "~/Areas/Externals/Scripts/FasecoldaSISA.js"
                 ));
                //ProductionReport
                bundles.Add(new ScriptBundle("~/bundle/js/ProductionReport").Include(
                   "~/Areas/Underwriting/Scripts/Underwriting/ProductionReport.js",
                   "~/Areas/Underwriting/Scripts/Underwriting/ProductionReportRequest.js"
                 ));
                //Sarlaft
                bundles.Add(new ScriptBundle("~/bundle/js/Sarlaft").Include(
                    "~/Areas/Sarlaft/Scripts/InternationalOperations.js",
                    "~/Areas/Sarlaft/Scripts/LegalRepresentative.js",
                    "~/Areas/Sarlaft/Scripts/Links.js",
                    "~/Areas/Sarlaft/Scripts/Partners.js",
                    "~/Areas/Sarlaft/Scripts/Peps.js",
                    "~/Areas/Sarlaft/Scripts/Exempt.js",
                    "~/Areas/Sarlaft/Scripts/FinalBeneficiary.js",
                    "~/Areas/Sarlaft/Scripts/SarlaftParam.js",
                    "~/Areas/Sarlaft/Scripts/SarlaftRequest.js",
                    "~/Areas/Sarlaft/Scripts/SarlaftSimpleSearch.js"



                 ));

                bundles.Add(new ScriptBundle("~/bundle/js/Printer").Include(
                "~/Areas/Printing/Scripts/Printer/Printer.js",
                "~/Areas/Printing/Scripts/Printer/PrinterRequest.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/CollectFormat").Include(
                "~/Areas/Printing/Scripts/Printer/CollectFormat.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/CounterGuarantee").Include(
                "~/Areas/Printing/Scripts/Printer/CounterGuarantee.js",
                "~/Areas/Printing/Scripts/Printer/CounterGuaranteeRequest.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/MassivePrinter").Include(
                "~/Areas/Printing/Scripts/Printer/MassivePrinter.js"
                ));
                //AutomaticQuota
               // bundles.Add(new ScriptBundle("~/bundle/js/AutomaticQuota").Include(
               //"~/Areas/AutomaticQuota/Scripts/AutomaticQuota.js",
               //"~/Areas/AutomaticQuota/Scripts/AutomaticQuotaRequest.js"
               //));
                //Surety
                bundles.Add(new ScriptBundle("~/bundle/js/Surety").Include(
                    "~/Areas/Underwriting/Scripts/RiskSurety/Surety.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/SuretyRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/ContractObject.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/ContractObjectRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/CrossGuarantees.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/CrossGuaranteesRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/OperationQuotaCumulus.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/OperationQuotaCumulusRequest.js"
                    ));

                //CoverageSurety
                bundles.Add(new ScriptBundle("~/bundle/js/CoverageSurety").Include(
                    "~/Areas/Underwriting/Scripts/RiskSurety/RiskSuretyCoverage.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/CoverageRequest.js"
                    ));

                //ContractObject
                bundles.Add(new ScriptBundle("~/bundle/js/ContractObject").Include(
                    "~/Areas/Underwriting/Scripts/ContractObject/ContractObject.js",
                    "~/Areas/Underwriting/Scripts/ContractObject/ContractObjectRequest.js"
                    ));

                //SubscriptionSearch
                bundles.Add(new ScriptBundle("~/bundle/js/SubscriptionSearch").Include(
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/SubscriptionSearch.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/SubscriptionSearchRequest.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/QuotationSearch.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/QuotationSearchRequest.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/TemporalSearch.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/TemporalSearchRequest.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/PolicySearch.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/PolicySearchRequest.js"
                ));
                /* CLAIMS DEBUG */

                //EstimationTypePrefix
                bundles.Add(new ScriptBundle("~/bundle/js/EstimationTypePrefix").Include(
                "~/Areas/Parametrization/Scripts/EstimationTypePrefix/EstimationTypePrefix.js",
                "~/Areas/Parametrization/Scripts/EstimationTypePrefix/EstimationTypePrefixRequest.js"
                ));

                //CauseCoverage
                bundles.Add(new ScriptBundle("~/bundle/js/CauseCoverage").Include(
                "~/Areas/Parametrization/Scripts/CauseCoverage/CauseCoverage.js",
                "~/Areas/Parametrization/Scripts/CauseCoverage/CauseCoverageRequest.js"
                ));

                //ClaimsAssociationConceptsofPaymentCoverage
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsAssociation").Include(
                "~/Areas/Parametrization/Scripts/ClaimsAssociation/ClaimsAssociation.js",
                "~/Areas/Parametrization/Scripts/ClaimsAssociation/ClaimsAssociationRequest.js"
                ));

                //Subcause
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsSubCause").Include(
                "~/Areas/Parametrization/Scripts/ClaimsSubCause/ClaimsSubCause.js",
                "~/Areas/Parametrization/Scripts/ClaimsSubCause/ClaimsSubCauseRequest.js"
                ));

                //ClaimsDocument
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsDocument").Include(
                "~/Areas/Parametrization/Scripts/ClaimsDocument/ClaimsDocument.js",
                "~/Areas/Parametrization/Scripts/ClaimsDocument/ClaimsDocumentRequest.js"
                ));

                //RoleAndSubrole
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsPersonCompany").Include(
                "~/Areas/Parametrization/Scripts/ClaimsPersonCompany/ClaimsPersonCompany.js",
                "~/Areas/Parametrization/Scripts/ClaimsPersonCompany/ClaimsPersonCompanyRequest.js"
                ));

                //LackPeriod
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsLackPeriod").Include(
                "~/Areas/Parametrization/Scripts/ClaimsLackPeriod/ClaimsLackPeriod.js",
                "~/Areas/Parametrization/Scripts/ClaimsLackPeriod/ClaimsLackPeriodRequest.js"
                ));

                //EstimationTypeStatus
                bundles.Add(new ScriptBundle("~/bundle/js/EstimationTypeStatus").Include(
                "~/Areas/Parametrization/Scripts/EstimationTypeStatus/EstimationTypeStatus.js",
                "~/Areas/Parametrization/Scripts/EstimationTypeStatus/EstimationTypeStatusRequest.js"
                ));

                //GuaranteStatusRoute
                bundles.Add(new ScriptBundle("~/bundle/js/GuaranteeStatusRoute").Include(
                "~/Areas/Parametrization/Scripts/GuaranteeStatusRoute/GuaranteeStatusRoute.js",
                "~/Areas/Parametrization/Scripts/GuaranteeStatusRoute/GuaranteeStatusRouteRequest.js"
                ));

                //Parametrization/ConfigurationPanels/ConfigurationPanels
                bundles.Add(new ScriptBundle("~/bundle/js/ConfigurationPanels").Include(
                "~/Areas/Parametrization/Scripts/ConfigurationPanels/ConfigurationPanels.js",
                "~/Areas/Parametrization/Scripts/ConfigurationPanels/ConfigurationPanelsRequest.js"
                ));

                //Parametrization/ConfigurationPanels/ConfigurationPanels
                bundles.Add(new ScriptBundle("~/bundle/js/ConfigurationPanels").Include(
                "~/Areas/Parametrization/Scripts/ConfigurationPanels/ConfigurationPanels.js",
                "~/Areas/Parametrization/Scripts/ConfigurationPanels/ConfigurationPanelsRequest.js"
                ));

                //Claim/Notice
                bundles.Add(new ScriptBundle("~/bundle/js/Notice").Include(
                "~/Areas/Claims/Scripts/Notice/Notice.js",
                "~/Areas/Claims/Scripts/Notice/NoticeRequest.js"
                ));

                //Claim/NoticeLocation/Location
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeLocation").Include(
                "~/Areas/Claims/Scripts/NoticeLocation/NoticeLocation.js",
                "~/Areas/Claims/Scripts/NoticeLocation/NoticeLocationRequest.js"
                ));

                //Claim/NoticeVehicle/Vehicle
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeVehicle").Include(
                "~/Areas/Claims/Scripts/NoticeVehicle/NoticeVehicle.js",
                "~/Areas/Claims/Scripts/NoticeVehicle/NoticeVehicleRequest.js"
                ));

                //Claim/NoticeSurety/Surety
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeSurety").Include(
                "~/Areas/Claims/Scripts/NoticeSurety/NoticeSurety.js",
                "~/Areas/Claims/Scripts/NoticeSurety/NoticeSuretyRequest.js"
                ));

                //Claim/NoticeTransport/Transport
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeTransport").Include(
                "~/Areas/Claims/Scripts/NoticeTransport/NoticeTransport.js",
                "~/Areas/Claims/Scripts/NoticeTransport/NoticeTransportRequest.js"
                ));

                //Claim/NoticeAirCraft/AirCraft
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeAirCraft").Include(
                "~/Areas/Claims/Scripts/NoticeAirCraft/NoticeAirCraft.js",
                "~/Areas/Claims/Scripts/NoticeAirCraft/NoticeAirCraftRequest.js"
                ));

                //Claim/NoticeFidelity/Fidelity
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeFidelity").Include(
                "~/Areas/Claims/Scripts/NoticeFidelity/NoticeFidelity.js",
                "~/Areas/Claims/Scripts/NoticeFidelity/NoticeFidelityRequest.js"
                ));

                //Claim
                bundles.Add(new ScriptBundle("~/bundle/js/Claim").Include(
                "~/Areas/Claims/Scripts/Claim/Claim.js",
                "~/Areas/Claims/Scripts/Claim/ClaimRequest.js",
                "~/Areas/Claims/Scripts/Claim/ClaimEstimation.js",
                "~/Areas/Claims/Scripts/Claim/ClaimEstimationRequest.js"
                ));

                //Claim/Vehicle
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimVehicle").Include(
                "~/Areas/Claims/Scripts/ClaimVehicle/ClaimVehicle.js",
                "~/Areas/Claims/Scripts/ClaimVehicle/ClaimVehicleRequest.js"
                ));

                //Claim/Location
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimLocation").Include(
                "~/Areas/Claims/Scripts/ClaimLocation/ClaimLocation.js",
                "~/Areas/Claims/Scripts/ClaimLocation/ClaimLocationRequest.js"
                ));

                //Fidelity
                bundles.Add(new ScriptBundle("~/bundle/js/Fidelity").Include(
                "~/Areas/Underwriting/Scripts/RiskFidelity/RiskFidelity.js",
                "~/Areas/Underwriting/Scripts/RiskFidelity/RiskFidelityRequest.js",
                "~/Areas/Underwriting/Scripts/RiskFidelity/Coverage.js",
                "~/Areas/Underwriting/Scripts/RiskFidelity/CoverageRequest.js"
                ));

                //Claim/Surety
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimSurety").Include(
                "~/Areas/Claims/Scripts/ClaimSurety/ClaimSurety.js",
                "~/Areas/Claims/Scripts/ClaimSurety/ClaimSuretyRequest.js"
                ));

                //Claim/Salvage
                bundles.Add(new ScriptBundle("~/bundle/js/Salvage").Include(
                "~/Areas/Claims/Scripts/Salvage/Salvage.js",
                "~/Areas/Claims/Scripts/Salvage/SalvageRequest.js"
                ));

                //Claim/Recovery
                bundles.Add(new ScriptBundle("~/bundle/js/Recovery").Include(
                "~/Areas/Claims/Scripts/Recovery/Recovery.js",
                "~/Areas/Claims/Scripts/Recovery/RecoveryRequest.js"
                ));

                //Claim/PaymentRequest
                bundles.Add(new ScriptBundle("~/bundle/js/Payment").Include(
                "~/Areas/Claims/Scripts/PaymentRequest/Payment.js",
                "~/Areas/Claims/Scripts/PaymentRequest/ClaimsPaymentRequest.js"
                ));

                //Claim/ChargeRequest
                bundles.Add(new ScriptBundle("~/bundle/js/Charge").Include(
                "~/Areas/Claims/Scripts/Charge/Charge.js",
                "~/Areas/Claims/Scripts/Charge/ChargeRequest.js"
                ));

                //Claim/Search
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimSearch").Include(
                "~/Areas/Claims/Scripts/Search/ClaimSearch.js",
                "~/Areas/Claims/Scripts/Search/ClaimSearchRequest.js"
                ));

                //Claim/CancelRequest
                bundles.Add(new ScriptBundle("~/bundle/js/RequestCancellation").Include(
                "~/Areas/Claims/Scripts/RequestCancellation/RequestCancellation.js",
                "~/Areas/Claims/Scripts/RequestCancellation/RequestCancellationRequest.js"
                ));

                //Claims/SetClaimReserve
                bundles.Add(new ScriptBundle("~/bundle/js/SetClaimReserve").Include(
                "~/Areas/Claims/Scripts/SetClaimReserve/SetClaimReserve.js",
                "~/Areas/Claims/Scripts/SetClaimReserve/SetClaimReserveRequest.js"
                ));
                bundles.Add(new ScriptBundle("~/bundle/js/Billing").Include(
                "~/Areas/Endorsement/Scripts/BillingRequest.js",
                "~/Areas/Endorsement/Scripts/Billing.js"

                ));

                //Claims/ClaimTransport
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimTransport").Include(
                "~/Areas/Claims/Scripts/ClaimTransport/ClaimTransport.js",
                "~/Areas/Claims/Scripts/ClaimTransport/ClaimTransportRequest.js"
                ));

                //Claims/ClaimAirCraft
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimAirCraft").Include(
                "~/Areas/Claims/Scripts/ClaimAirCraft/ClaimAirCraft.js",
                "~/Areas/Claims/Scripts/ClaimAirCraft/ClaimAirCraftRequest.js"
                ));

                //Claims/ClaimFidelity
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimFidelity").Include(
                "~/Areas/Claims/Scripts/ClaimFidelity/ClaimFidelity.js",
                "~/Areas/Claims/Scripts/ClaimFidelity/ClaimFidelityRequest.js"
                ));

                // Claims/AutomaticSalaryUpdate
                bundles.Add(new ScriptBundle("~/bundle/js/AutomaticSalaryUpdate").Include(
                "~/Areas/Claims/Scripts/AutomaticSalaryUpdate/AutomaticSalaryUpdate.js",
                "~/Areas/Claims/Scripts/AutomaticSalaryUpdate/AutomaticSalaryUpdateRequest.js"
                ));
                /* CLAIMS DEBUG */



                //ListRiskPerson
                bundles.Add(new ScriptBundle("~/bundle/js/ListRisk").Include(
                    "~/Areas/ListRiskPerson/Scripts/ListRiskPerson.js",
                    "~/Areas/ListRiskPerson/Scripts/ListRiskPersonRequest.js",
                    "~/Areas/ListRiskPerson/Scripts/ListRiskFile.js",
                    "~/Areas/ListRiskPerson/Scripts/ListRiskFileRequest.js",
                    "~/Areas/ListRiskPerson/Scripts/MatchingProcess.js",
                    "~/Areas/ListRiskPerson/Scripts/MatchingProcessRequest.js"
                    ));
                //EventSarlaf
                bundles.Add(new ScriptBundle("~/bundle/js/EventSarlaf").Include(
                    "~/Areas/EventsSarlaft/Scripts/EventsSarlaft/EventsSarlaft.js",
                    "~/Areas/EventsSarlaft/Scripts/EventsSarlaft/EventsSarlaftRequest.js",
                    "~/Areas/EventsSarlaft/Scripts/EventsSarlaft/EventsSarlaftAdvanceSearch.js"
                    ));

                //QuotaOperation
                bundles.Add(new ScriptBundle("~/bundle/js/QuotaOperation").Include(
                    "~/Areas/QuotaOperational/Scripts/QuotaOperational/QuotaOperational.js",
                    "~/Areas/QuotaOperational/Scripts/QuotaOperational/QuotaOperationalRequest.js"
                    ));

                //VehicleFasecolda
                bundles.Add(new ScriptBundle("~/bundle/js/MassiveVehicleFasecolda").Include(
                    "~/Areas/MassiveVehicleFasecolda/Scripts/MassiveVehicleFasecolda/MassiveVehicleFasecolda.js",
                    "~/Areas/MassiveVehicleFasecolda/Scripts/MassiveVehicleFasecolda/MassiveVehicleFasecoldaRequest.js"
                ));

                //Property
                bundles.Add(new ScriptBundle("~/bundle/js/Property").Include(
                    "~/Areas/Underwriting/Scripts/RiskProperty/PropertyRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/Property.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/AdditionalDataRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/AdditionalData.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/InsuredObjectRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/InsuredObject.js"
                    ));
                //Liability
                bundles.Add(new ScriptBundle("~/bundle/js/Liability").Include(
                    "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiability.js",
                    "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiabilityRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiabilityCoverage.js",
                    "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiabilityCoverageRequest.js"

                    ));
                //Transport
                bundles.Add(new ScriptBundle("~/bundle/js/Transport").Include(
                    "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransport.js",
                    "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportCoverage.js",
                    "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportCoverageRequest.js"
                    ));

                //Declaration
                bundles.Add(new ScriptBundle("~/bundle/js/Declaration").Include(
                "~/Areas/Endorsement/Scripts/Declaration.js",
                "~/Areas/Endorsement/Scripts/DeclarationRequest.js"
                ));

                //Ajustment
                bundles.Add(new ScriptBundle("~/bundle/js/Adjustment").Include(
                        "~/Areas/Endorsement/Scripts/Adjustment.js",
                    "~/Areas/Endorsement/Scripts/AdjustmentRequest.js"
                    ));

                //CreditNote
                bundles.Add(new ScriptBundle("~/bundle/js/CreditNote").Include(
                    "~/Areas/Endorsement/Scripts/CreditNote.js",
                    "~/Areas/Endorsement/Scripts/CreditNoteRequest.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/TransportCreditNote").Include(
                   "~/Areas/Endorsement/Scripts/TransportCreditNote/TransportCreditNote.js",
                   "~/Areas/Endorsement/Scripts/TransportCreditNote/TransportCreditNoteRequest.js"
                   ));


                //ThirdPartyLiability
                bundles.Add(new ScriptBundle("~/bundle/js/ThirdPartyLiability").Include(
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/RiskThirdPartyLiability.js",
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/RiskThirdPartyLiabilityRequest.js",
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/RiskThirdPartyLiabilityCoverage.js",
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/RiskThirdPartyLiabilityCoverageRequest.js",
                     "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/AdditionalData.js"
                    ));
                //TaxConceptsExpenses
                bundles.Add(new ScriptBundle("~/bundle/js/TaxConceptsExpenses").Include(
                    "~/Areas/Parametrization/Scripts/Tax/TaxConceptsExpenses.js",
                    "~/Areas/Parametrization/Scripts/Tax/TaxConceptsExpensesRequests.js"
                    ));
                //ProductArticle
                bundles.Add(new ScriptBundle("~/bundle/js/ProductArticle").Include(
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ProductArticle.js",
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ProductArticleRequest.js",
                     "~/Areas/Parametrization/Scripts/JudicialSurety/ProductArticleSearch.js"
                    ));
                //ArticleLine
                bundles.Add(new ScriptBundle("~/bundle/js/ArticleLine").Include(
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ArticleLine.js",
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ArticleLineRequest.js",
                     "~/Areas/Parametrization/Scripts/JudicialSurety/ArticleLineSearch.js"
                    ));
                //ArticleLine
                bundles.Add(new ScriptBundle("~/bundle/js/CourtType").Include(
                    "~/Areas/Parametrization/Scripts/JudicialSurety/CourtType.js",
                    "~/Areas/Parametrization/Scripts/JudicialSurety/CourtTypeRequest.js",
                     "~/Areas/Parametrization/Scripts/JudicialSurety/CourtTypeSearch.js"
                    ));

            }
            else
            {
                bundles.Add(new ScriptBundle("~/bundle/js/Global").Include(
                    "~/Areas/Endorsement/Scripts/Search/GlobalValue.es5.js",
                        "~/Areas/Endorsement/Scripts/SharedEndorsementBase.es5.js"
                         ));
                //Eventos R1
                bundles.Add(new ScriptBundle("~/bundle/js/EventsMain").Include(
                        "~/Areas/Events/Scripts/Events/LaunchEvents.es5.js"
                        ));

                //Notification
                bundles.Add(new ScriptBundle("~/bundle/js/Notification").Include(
                           "~/Areas/Notification/Scripts/Notification.es5.js"
                    //"~/Scripts/Hubs/NotificationHub.es5.js"
                    ));
                //Prospect
                bundles.Add(new ScriptBundle("~/bundle/js/Prospect").Include(
                    "~/Areas/Prospect/Scripts/Prospect/ProspectusNatural.es5.js",
                    "~/Areas/Prospect/Scripts/Prospect/ProspectusLegal.es5.js",
                    "~/Areas/Prospect/Scripts/Prospect/ProspectRequest.es5.js"
                    ));
                //Person
                bundles.Add(new ScriptBundle("~/bundle/js/Person").Include(
                    "~/Areas/Person/Scripts/Person/Person.es5.js",
                    "~/Areas/Person/Scripts/Person/PersonRequest.es5.js",
                    "~/Areas/Person/Scripts/Person/Shared.es5.js",
                    "~/Areas/Person/Scripts/Person/Sarlaft.es5.js",
                    "~/Areas/Person/Scripts/Person/Address.es5.js",
                    "~/Areas/Person/Scripts/Person/StaffLabour.es5.js",
                    "~/Areas/Person/Scripts/Person/Phone.es5.js",
                    "~/Areas/Person/Scripts/Person/Email.es5.js",
                    "~/Areas/Person/Scripts/Person/EditBasicInfo.es5.js",
                    "~/Areas/Person/Scripts/Person/MethodPayment.es5.js",
                    "~/Areas/Person/Scripts/Person/OperatingQuota.es5.js",
                    "~/Areas/Person/Scripts/Person/Insured.es5.js",
                    "~/Areas/Person/Scripts/Person/AdvancedSearch.es5.js",
                    "~/Areas/Person/Scripts/Person/RepresentLegal.es5.js",
                    "~/Areas/Person/Scripts/Person/Partner.es5.js",
                    "~/Areas/Person/Scripts/Person/ConsortiumMembers.es5.js",
                    "~/Areas/Person/Scripts/Person/BusinessName.es5.js",
                    "~/Areas/Person/Scripts/Person/Agent.es5.js",
                    "~/Areas/Person/Scripts/Person/CoInsurer.es5.js",
                    "~/Areas/Person/Scripts/Person/ReInsurer.es5.js",
                    "~/Areas/Person/Scripts/Person/Agency.es5.js",
                    "~/Areas/Person/Scripts/Person/Provider.es5.js",
                    "~/Areas/Person/Scripts/Person/Tax.es5.js",
                    "~/Areas/Person/Scripts/Person/PersonNatural.es5.js",
                    "~/Areas/Person/Scripts/Person/PersonLegal.es5.js",
                    "~/Areas/Person/Scripts/Person/ProspectusNatural.es5.js",
                    "~/Areas/Person/Scripts/Person/ProspectusLegal.es5.js",
                    "~/Areas/Person/Scripts/Person/ComissionAgency.es5.js",
                    "~/Areas/Person/Scripts/Person/BasicInformation.es5.js",
                    "~/Areas/Person/Scripts/Person/BasicInformationAdvancedSearch.es5.js",
                    "~/Areas/Person/Scripts/Person/AdditionalData.es5.js",
                    "~/Areas/Person/Scripts/Person/Third.es5.js",
                    "~/Areas/Person/Scripts/Employee/Employee.es5.js",
                    "~/Areas/Person/Scripts/Employee/EmployeeRequest.es5.js",
                    "~/Areas/Person/Scripts/EconomicGroup/EconomicGroup.es5.js",
                    "~/Areas/Person/Scripts/EconomicGroup/EconomicGroupRequest.es5.js",
                    "~/Areas/Person/Scripts/EconomicGroup/AdvancedSearchGroup.es5.js",
                    "~/Areas/Person/Scripts/EconomicGroup/AdvancedSearchGroupRequest.es5.js",
                    "~/Areas/Person/Scripts/Person/BankTransfers.es5.js",
                    "~/Areas/Person/Scripts/Person/BankTransfersRequest.es5.js",
                    "~/Areas/Person/Scripts/Person/ElectronicBilling.es5.js",
                    "~/Areas/Person/Scripts/Person/ElectronicBillingRequest.es5.js"
                    ));
                //Guarantee
                bundles.Add(new ScriptBundle("~/bundle/js/Guarantee").Include(
                    "~/Areas/Person/Scripts/Guarantee/GuaranteeRequest.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/Guarantee.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/DocumentationReceived.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/Binnacle.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/PrefixAssociated.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/BindPolicy.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/Guarantors.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/Mortage.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/Others.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/Pledge.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/Actions.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/PromissoryNote.es5.js",
                    "~/Areas/Person/Scripts/Guarantee/FixedTermDeposit.es5.js"
                    ));
                //Guarantees
                bundles.Add(new ScriptBundle("~/bundle/js/Guarantees").Include(
                    "~/Areas/Guarantees/Scripts/Guarantees/GuaranteeRequest.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Guarantee.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/DocumentationReceived.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Binnacle.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/PrefixAssociated.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/BindPolicy.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Guarantors.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Mortage.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Others.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Actions.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/Pledge.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/PromissoryNote.es5.js",
                    "~/Areas/Guarantees/Scripts/Guarantees/FixedTermDeposit.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/RenewalMassive").Include(
                    //"~/Areas/Massive/Scripts/Renewal/Renewal.es5.js"
                    //"~/Areas/Massive/Scripts/Renewal/AdvancedSearchRenewal.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/RenewalRequestGrouping").Include(
                    //"~/Areas/Massive/Scripts/RenewalRequestGrouping/RenewalRequestGrouping.es5.js",
                    "~/Areas/Massive/Scripts/RenewalRequestGrouping/Agents.es5.js",
                    "~/Areas/Massive/Scripts/RenewalRequestGrouping/CoInsurance.es5.js",
                    "~/Areas/Massive/Scripts/RenewalRequestGrouping/AdvancedSearch.es5.js"
                    ));
                //UniqueUser
                bundles.Add(new ScriptBundle("~/bundle/js/UniqueUser").Include(
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UniqueUser.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/Hierarchy.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/Branch.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserAgent.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/PersonOnline.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserAdvancedSearch.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/AlliedSalePoints.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserProduct.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserGroup.es5.js",
                    "~/Areas/UniqueUser/Scripts/UniqueUser/UserPermissions.es5.js"
                    ));
                //
                bundles.Add(new ScriptBundle("~/bundle/js/Module").Include(
                    "~/Areas/UniqueUser/Scripts/Module/Module.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/SubModule").Include(
                    "~/Areas/UniqueUser/Scripts/SubModule/SubModule.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Profile").Include(
                    "~/Areas/UniqueUser/Scripts/Profile/Profile.es5.js",
                    "~/Areas/UniqueUser/Scripts/Profile/ProfileAdvancedSearch.es5.js",
                    "~/Areas/UniqueUser/Scripts/Profile/AccessProfile.es5.js",
                    "~/Areas/UniqueUser/Scripts/Profile/CopyProfile.es5.js",
                    "~/Areas/UniqueUser/Scripts/Profile/ProfileContextPermissions.es5.js",
                    "~/Areas/UniqueUser/Scripts/Profile/ProfileAdvancedSearch.es5.js",
                    "~/Areas/UniqueUser/Scripts/Profile/GuaranteeStatus.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Access").Include(
                    "~/Areas/UniqueUser/Scripts/Access/Access.es5.js",
                    "~/Areas/UniqueUser/Scripts/Access/AccessAdvancedSearch.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Common").Include(
                    "~/Areas/Common/Scripts/Common/Main.es5.js",
                     "~/Areas/Common/Scripts/Common/Agents.es5.js",
                     "~/Areas/Common/Scripts/Common/Request.es5.js"
                    ));
                //BusinessBranch
                bundles.Add(new ScriptBundle("~/bundle/js/Prefix").Include(
                    "~/Areas/Parametrization/Scripts/Prefix/Prefix.es5.js",
                    "~/Areas/Parametrization/Scripts/Prefix/AdvancedSearch.es5.js",
                    "~/Areas/Parametrization/Scripts/Prefix/TechnicalBranchBusiness.es5.js",
                    "~/Areas/Parametrization/Scripts/Prefix/AditionalInformation.es5.js"
                    ));
                //FalseColda
                bundles.Add(new ScriptBundle("~/bundle/js/Fasecolda").Include(
                    "~/Areas/ReportsFasecolda/Scripts/Fasecolda/Fasecolda.es5.js",
                    "~/Areas/ReportsFasecolda/Scripts/Fasecolda/LogFasecolda.es5.js"
                ));
                //ReportQuotation
                bundles.Add(new ScriptBundle("~/bundle/js/ReportQuotation").Include(
                    "~/Areas/Reports/Scripts/Quotation/VehicleIndicators.es5.js"
                ));
                bundles.Add(new ScriptBundle("~/bundle/js/VehicleType").Include(
                    "~/Areas/Parametrization/Scripts/VehicleType/VehicleBody.es5.js",
                    "~/Areas/Parametrization/Scripts/VehicleType/VehicleType.es5.js"

                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Protection").Include(
                   "~/Areas/Parametrization/Scripts/Parametrization.es5.js",
                   "~/Areas/Parametrization/Scripts/Protection/Protection.es5.js",
                   "~/Areas/Parametrization/Scripts/Protection/ProtectionAdvancedSearch.es5.js"
                   ));

                //LineBusinessParametrization
                bundles.Add(new ScriptBundle("~/bundle/js/LineBusinessParametrization").Include(
                    "~/Areas/Parametrization/Scripts/LineBusiness/LineBusiness.es5.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/LineBusinessRequest.es5.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/InsuranceObjects.es5.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/Clauses.es5.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/AdvancedSearch.es5.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/RiskType.es5.js",
                    "~/Areas/Parametrization/Scripts/LineBusiness/Protections.es5.js"
                    ));

                //Technical Branch
                bundles.Add(new ScriptBundle("~/bundle/js/TechnicalBranch").Include(
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/TechnicalBranch.es5.js",
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/InsuranceObjects.es5.js",
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/AdvancedSearchTechnicalBranch.es5.js",
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/RiskTypeTechnicalBranch.es5.js",
                    "~/Areas/Parametrization/Scripts/TechnicalBranch/Protections.es5.js"
                    ));

                //Fasecolda
                bundles.Add(new ScriptBundle("~/bundle/js/Fasecolda").Include(
                    "~/Areas/ReportsFasecolda/Scripts/Fasecolda/Fasecolda.es5.js",
                    "~/Areas/ReportsFasecolda/Scripts/Fasecolda/LogFasecolda.es5.js"
                    ));

                //ReportQuotation
                bundles.Add(new ScriptBundle("~/bundle/js/ReportQuotation").Include(
                    "~/Areas/Reports/Scripts/Quotation/VehicleIndicators.es5.js"
                    ));

                //Parametrization/SubLineBusiness
                bundles.Add(new ScriptBundle("~/bundle/js/SubLineBusiness").Include(
                    "~/Areas/Parametrization/Scripts/SubLineBusiness/SubLineBusiness.es5.js",
                        "~/Areas/Parametrization/Scripts/SubLineBusiness/SubLineBusinessAdvancedSearch.es5.js",
                    "~/Areas/Parametrization/Scripts/SubLineBusiness/SubLineBusinessRequest.es5.js"

                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/VehicleVersionParametrization").Include(
                "~/Areas/Parametrization/Scripts/VehicleVersion/VehicleVersion.es5.js",
                "~/Areas/Parametrization/Scripts/VehicleVersion/VehicleVersionSearch.es5.js"
                ));

                //Parametrization/SalePoint
                bundles.Add(new ScriptBundle("~/bundle/js/SalePoint").Include(
                   "~/Areas/Parametrization/Scripts/SalePoint/SalePoint.es5.js",
                   "~/Areas/Parametrization/Scripts/SalePoint/SalePointAdvancedSearch.es5.js"

                   ));
                bundles.Add(new ScriptBundle("~/bundle/js/Massive").Include(
                    "~/Areas/Massive/Scripts/MassiveRequest.es5.js",
                    "~/Areas/Massive/Scripts/Massive/Massive.es5.js",
                    "~/Areas/Massive/Scripts/Massive/MassiveAdvancedSearch.es5.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/MassiveProcess").Include(
            "~/Areas/Massive/Scripts/MassiveProcess/MassiveProcess.es5.js",
            "~/Areas/Massive/Scripts/MassiveProcess/AdvancedSearchMassiveProcess.es5.js"
            ));

                bundles.Add(new ScriptBundle("~/bundle/js/RequestGrouping").Include(
                    "~/Areas/Massive/Scripts/RequestGrouping/RequestGrouping.es5.js",
                    "~/Areas/Massive/Scripts/RequestGrouping/Agents.es5.js",
                    "~/Areas/Massive/Scripts/RequestGrouping/BillingGroup.es5.js",
                    "~/Areas/Massive/Scripts/RequestGrouping/RequestGroup.es5.js",
                    "~/Areas/Massive/Scripts/RequestGrouping/CoInsurance.es5.js"
                    ));

                //Parametrization/InfringementGroup
                bundles.Add(new ScriptBundle("~/bundle/js/InfringementGroup").Include(
                    "~/Areas/Parametrization/Scripts/InfringementGroup/InfringementGroup.es5.js"
                    ));
                //ScoreTypeDoc
                bundles.Add(new ScriptBundle("~/bundle/js/ScoreTypeDoc").Include(
                    "~/Areas/Person/Scripts/Person/ScoreTypeDoc.es5.js",
                    "~/Areas/Person/Scripts/Person/ScoreTypeDocAdvancedSearch.es5.js"
                    ));
                //Parametrization/Expense
                bundles.Add(new ScriptBundle("~/bundle/js/Expense").Include(
                    "~/Areas/Parametrization/Scripts/Expense/Expense.es5.js"
                    ));
                // ParametrizationRequest
                bundles.Add(new ScriptBundle("~/bundle/js/ParametrizationRequest").Include(
                    "~/Areas/Parametrization/Scripts/Parametrization.es5.js"
                    ));

                //Parametrization/Alliance
                bundles.Add(new ScriptBundle("~/bundle/js/Alliance").Include(
                    "~/Areas/Parametrization/Scripts/Alliance/Alliance.es5.js"
                    ));
                //Parametrization/BranchAlliance
                bundles.Add(new ScriptBundle("~/bundle/js/BranchAlliance").Include(
                    "~/Areas/Parametrization/Scripts/BranchAlliance/BranchAlliance.es5.js"
                    ));

                //Parametrization/InsurancesObjects
                bundles.Add(new ScriptBundle("~/bundle/js/InsurancesObjects").Include(
                    "~/Areas/Parametrization/Scripts/InsuredObject/InsurancesObjects.es5.js",
                      "~/Areas/Parametrization/Scripts/InsuredObject/InsuredObjectAdvancedSearch.es5.js"

                    ));

                //Parametrization/Infringement
                bundles.Add(new ScriptBundle("~/bundle/js/Infringement").Include(
                    "~/Areas/Parametrization/Scripts/Infringement/Infringement.es5.js"
                    ));

                //Parametrization/InfringementState
                bundles.Add(new ScriptBundle("~/bundle/js/InfringementState").Include(
                    "~/Areas/Parametrization/Scripts/InfringementState/InfringementState.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Branch").Include(
                    "~/Areas/Parametrization/Scripts/Branch/Branch.es5.js",
                    "~/Areas/Parametrization/Scripts/Branch/BranchAdvancedSearch.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Channel").Include(
                    "~/Areas/Parametrization/Scripts/Channel/Channel.es5.js",
                    "~/Areas/Parametrization/Scripts/Channel/ChannelAdvancedSearch.es5.js",
                    "~/Areas/Parametrization/Scripts/Channel/ValuesDefault.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/SearchEndorsementPP").Include(
                    "~/Areas/Endorsement/Scripts/AdvancedSearch.es5.js"
                    ));
                // Parametrization/Coverages
                bundles.Add(new ScriptBundle("~/bundle/js/CoverageParametrization").Include(
                    "~/Areas/Parametrization/Scripts/Coverage/CoverageParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/Coverage/ClausesParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/Coverage/DeductiblesParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/Coverage/DetailTypesParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/Coverage/CoverageParametrizationAdvancedSearch.es5.js",
                    "~/Areas/Parametrization/Scripts/Coverage/Coverage2GParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/Coverage/PrintCoveragesParametrization.es5.js"
                    ));
                ////Parametrization/EconomicGroup
                bundles.Add(new ScriptBundle("~/bundle/js/EconomicGroup").Include(
                    "~/Areas/person/Scripts/EconomicGroup/EconomicGroup.es5.js",
                    "~/Areas/person/Scripts/EconomicGroup/EconomicGroupRequest.es5.js",
                    "~/Areas/person/Scripts/EconomicGroup/AdvancedSearchGroup.es5.js",
                    "~/Areas/person/Scripts/EconomicGroup/AdvancedSearchGroupRequest.es5.js"
                    ));
                //JudicialSurety
                bundles.Add(new ScriptBundle("~/bundle/js/JudicialSurety").Include(
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/RiskJudicialSurety.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/CoverageJudicialSurety.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/CrossGuarantees.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/AdditionalData.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskJudicialSurety/RiskJudicialRequest.es5.js"
                ));
                //Authorization Policies
                bundles.Add(new ScriptBundle("~/bundle/js/AuthorizationPolicies").Include(
                    "~/Areas/AuthorizationPolicies/Scripts/Policies/Policies.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/Authorize/Authorize.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/Policies/PoliciesRules.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/Policies/AdvancedSearchPolicy.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/WorkFlowPolicies/WorkFlowPolicies.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/WorkFlowPolicies/WorkFlowPoliciesRequest.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/AuthorizationPersonRiskList/AuthorizationPersonRiskList.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/AuthorizationPersonRiskList/AuthorizationPersonRiskListRequest.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/AuthorizationSarlaftOperation/AuthorizationSarlaftOperation.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/AuthorizationSarlaftOperation/AuthorizationSarlaftOperationRequest.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/LaunchPolicies").Include(
                    "~/Areas/AuthorizationPolicies/Scripts/LaunchPolicies.es5.js",
                    "~/Areas/AuthorizationPolicies/Scripts/Summary/Summary.es5.js"
                    ));
                //Reglas
                bundles.Add(new ScriptBundle("~/bundle/js/Rules").Include(
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/RuleSet.es5.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalRuleSet.es5.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalRule.es5.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalConditionRule.es5.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/ModalActionRule.es5.js",
                    "~/Areas/RulesAndScripts/Scripts/RuleSet/AdvancedSearch.es5.js"
                    ));
                //Cache
                bundles.Add(new ScriptBundle("~/bundle/js/RuleSetCache").Include(
                    "~/Areas/RulesAndScripts/Scripts/RuleSetCache/RuleSetCache.es5.js"
                    ));
                //Tablas de decision
                bundles.Add(new ScriptBundle("~/bundle/js/DecisionTable").Include(
                    "~/Areas/RulesAndScripts/Scripts/DecisionTable/DecisionTable.es5.js",
                    "~/Areas/RulesAndScripts/Scripts/DecisionTable/Header.es5.js",
                    "~/Areas/RulesAndScripts/Scripts/DecisionTable/Body.es5.js",
                    "~/Areas/RulesAndScripts/Scripts/DecisionTable/LoadFromFile.es5.js",
                     "~/Areas/RulesAndScripts/Scripts/DecisionTable/AdvancedSearchDt.es5.js"
                    ));

                //Guiones
                bundles.Add(new ScriptBundle("~/bundle/js/Scripts").Include(
                    "~/Areas/RulesScripts/Scripts/SearchCombo/SearchCombo.es5.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/Enum.es5.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/ScriptObj.es5.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/Scripts.es5.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/ExecuteScript.es5.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/ViewScript.es5.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/AdvancedSearchScript.es5.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/LegalRepresentativeSing").Include(
                            "~/Areas/Printing/Scripts/LegalRepresentativeSing/LegalRepresentativeSing.es5.js",
                            "~/Areas/Printing/Scripts/LegalRepresentativeSing/LegalRepresentativeSingAdvancedSearch.es5.js"
                            ));

                bundles.Add(new ScriptBundle("~/bundle/js/StrongPassword").Include(
                    "~/Scripts/StrongPassword/ChangePassword.es5.js"
                    ));
                //Risks
                bundles.Add(new ScriptBundle("~/bundle/js/Risk").Include(
                    "~/Areas/Underwriting/Scripts/Risks/Beneficiaries.es5.js",
                    "~/Areas/Underwriting/Scripts/Risks/Texts.es5.js",
                    "~/Areas/Underwriting/Scripts/Risks/Clauses.es5.js",
                    "~/Areas/RulesScripts/Scripts/Scripts/ExecuteScript.es5.js",
                    "~/Areas/RulesScripts/Scripts/SearchCombo/SearchCombo.es5.js"
                    ));
                //Coverages
                bundles.Add(new ScriptBundle("~/bundle/js/Coverage").Include(
                    "~/Areas/Underwriting/Scripts/Coverages/Texts.es5.js",
                    "~/Areas/Underwriting/Scripts/Coverages/Clauses.es5.js",
                    "~/Areas/Underwriting/Scripts/Coverages/Deductible.es5.js",
                    "~/Areas/Underwriting/Scripts/Coverages/DeductibleRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/Coverages/CoverageRequest.es5.js"
                    ));
                //Vehicle
                bundles.Add(new ScriptBundle("~/bundle/js/Vehicle").Include(
                    "~/Areas/Underwriting/Scripts/RiskVehicle/Vehicle.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskVehicle/Accessories.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskVehicle/AdditionalData.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskVehicle/Coverage.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskVehicle/RiskVehicleRequest.es5.js"
                    ));
                //Marine
                bundles.Add(new ScriptBundle("~/bundle/js/Marine").Include(
                    "~/Areas/Underwriting/Scripts/RiskMarine/RiskMarine.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskMarine/RiskMarineCoverage.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskMarine/RiskMarineCoverageRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskMarine/RiskMarineRequest.es5.js"
                    ));
                //Aircraft
                bundles.Add(new ScriptBundle("~/bundle/js/Aircraft").Include(
                    "~/Areas/Underwriting/Scripts/RiskAircraft/RiskAircraft.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskAircraft/RiskAircraftCoverage.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskAircraft/RiskAircraftCoverageRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskAircraft/RiskAircraftRequest.es5.js"
                    ));
                //Underwriting
                bundles.Add(new ScriptBundle("~/bundle/js/Temporal").Include(
                    "~/Areas/Underwriting/Scripts/Underwriting/Temporal.es5.js",
                    "~/Areas/Printing/Scripts/Printer/PrinterRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/TemporalAdvancedSearch.es5.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/Endorsement.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Quotation").Include(
                    "~/Areas/Underwriting/Scripts/Underwriting/Quotation.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/QuotationAdvancedSearch.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Underwriting").Include(
                    "~/Areas/Underwriting/Scripts/Underwriting/AdditionalData.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Alliance.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Agents.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Clauses.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Texts.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Underwriting.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/CoInsurance.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/Beneficiaries.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/PaymentPlan.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/PersonOnline.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/UnderwritingRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/IssueWithEvent.es5.js",
                    "~/Areas/Underwriting/Scripts/Underwriting/ModalExpenses.es5.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/TableExpreses.es5.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/Surcharges.es5.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/Discounts.es5.js",
                     "~/Areas/Underwriting/Scripts/Underwriting/Promissory.es5.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementChangeAgent").Include(
                 "~/Areas/Endorsement/Scripts/ChangeAgent.es5.js",
                   "~/Areas/Endorsement/Scripts/Request/PolicyRequest.es5.js"

               ));
                bundles.Add(new ScriptBundle("~/bundle/js/InsuredProfile").Include(
                    "~/Areas/Parametrization/Scripts/InsuredProfile/InsuredProfile.es5.js",
                    "~/Areas/Parametrization/Scripts/InsuredProfile/InsuredProfileAdvancedSearch.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/InsuredSegment").Include(
                       "~/Areas/Parametrization/Scripts/InsuredSegment/InsuredSegment.es5.js",
                       "~/Areas/Parametrization/Scripts/InsuredSegment/InsuredSegmentAdvancedSearch.es5.js"
                       ));
                //Alliance Print Format
                bundles.Add(new ScriptBundle("~/bundle/js/AlliancePrintFormat").Include(
                       "~/Areas/Parametrization/Scripts/AlliancePrintFormat/AlliancePrintFormat.es5.js",
                       "~/Areas/Parametrization/Scripts/AlliancePrintFormat/AlliancePrintFormatAdvancedSearch.es5.js"
                       ));
                bundles.Add(new ScriptBundle("~/bundle/js/CoveredRiskType").Include(
                       "~/Areas/Parametrization/Scripts/CoveredRiskType/CoveredRiskType.es5.js"
                       ));
                bundles.Add(new ScriptBundle("~/bundle/js/PaymentPlanParametrization").Include(
                "~/Areas/Parametrization/Scripts/PaymentPlan/PaymentPlan.es5.js",
                "~/Areas/Parametrization/Scripts/PaymentPlan/DistributionQuota.es5.js",
                "~/Areas/Parametrization/Scripts/PaymentPlan/PaymentPlanAdvanced.es5.js"
                      ));
                //CompanyAddressType
                bundles.Add(new ScriptBundle("~/bundle/js/CompanyAddressType").Include(
                    "~/Areas/Person/Scripts/Person/CompanyAddressType.es5.js",
                    "~/Areas/Person/Scripts/Person/CompanyAddressTypeAdvancedSearch.es5.js"
                    ));
                //CompanyPhoneType
                bundles.Add(new ScriptBundle("~/bundle/js/CompanyPhoneType").Include(
                    "~/Areas/Person/Scripts/Person/CompanyPhoneType.es5.js",
                    "~/Areas/Person/Scripts/Person/CompanyPhoneTypeAdvancedSearch.es5.js"
                    ));
                //ScoreTypeDoc
                bundles.Add(new ScriptBundle("~/bundle/js/ScoreTypeDoc").Include(
                    "~/Areas/Person/Scripts/Person/ScoreTypeDoc.es5.js",
                    "~/Areas/Person/Scripts/Person/ScoreTypeDocAdvancedSearch.es5.js"
                    ));

                //Productos
                bundles.Add(new ScriptBundle("~/bundle/js/Product").Include(
                  "~/Areas/Product/Scripts/Product/Product.es5.js",
                  "~/Areas/Product/Scripts/Product/Currency.es5.js",
                  "~/Areas/Product/Scripts/Product/PolicyType.es5.js",
                  "~/Areas/Product/Scripts/Product/RiskType.es5.js",
                  "~/Areas/Product/Scripts/Product/Agent.es5.js",
                  "~/Areas/Product/Scripts/Product/AgentRequestT.es5.js",
                  "~/Areas/Product/Scripts/Product/PaymentPlan.es5.js",
                  "~/Areas/Product/Scripts/Product/PaymentPlanRequestT.es5.js",
                  "~/Areas/Product/Scripts/Product/InsuredObject.es5.js",
                  "~/Areas/Product/Scripts/Product/InsuredObjectRequestT.es5.js",
                  "~/Areas/Product/Scripts/Product/Coverage.es5.js",
                  "~/Areas/Product/Scripts/Product/CoverageRequestT.es5.js",
                  "~/Areas/Product/Scripts/Product/CoverageAllied.es5.js",
                  "~/Areas/Product/Scripts/Product/RulesAndscript.es5.js",
                  "~/Areas/Product/Scripts/Product/RulesAndscriptRequestT.es5.js",
                  "~/Areas/Product/Scripts/Product/Script.es5.js",
                  "~/Areas/Product/Scripts/Product/CopyProduct.es5.js",
                   "~/Areas/Product/Scripts/Product/CopyProductRequest.es5.js",
                  "~/Areas/Product/Scripts/Product/AdditionalData.es5.js",
                   "~/Areas/Product/Scripts/Product/AdditionalDataRequestT.es5.js",
                  "~/Areas/Product/Scripts/Product/CommisionAgent.es5.js",
                  "~/Areas/Product/Scripts/Product/ProductResources.es.es5.js",
                  "~/Areas/Product/Scripts/Product/AdvancedSearch.es5.js",
                  "~/Areas/Product/Scripts/Product/AdvancedSearchRequest.es5.js",
                  "~/Areas/Product/Scripts/Product/DeductiblesByCoverage.es5.js",
                   "~/Areas/Product/Scripts/Product/DeductiblesByCoverageRequest.es5.js",
                  "~/Areas/Product/Scripts/Product/TypeOfAssistance.es5.js",
                  "~/Areas/Product/Scripts/Product/IncentivesForAgents.es5.js",
                  "~/Areas/Product/Scripts/Product/IncentivesForAgentsRequest.es5.js",
                  "~/Areas/Product/Scripts/Product/UifTouch_Co.es5.js",
                  "~/Areas/Product/Scripts/Product/ProductRequestT.es5.js",
                  "~/Areas/Product/Scripts/Product/ScriptRequestT.es5.js"
                  ));
                //DelegationParametrization
                bundles.Add(new ScriptBundle("~/bundle/js/DelegationParametrization").Include(
                    "~/Areas/ParamAuthorizationPolicies/Scripts/Delegation/DelegationParametrization.es5.js",
                    "~/Areas/ParamAuthorizationPolicies/Scripts/Delegation/DelegationParametrizationRequest.es5.js"
                ));
                //Parametrization/Deductible
                bundles.Add(new ScriptBundle("~/bundle/js/DeductibleParametrization").Include(
                    "~/Areas/Parametrization/Scripts/Deductible/AdvancedSearchParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/Deductible/DeductibleParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/Deductible/DeductibleParametrizationRequest.es5.js"
                    ));
                ///Parametrization Vehicle
                bundles.Add(new ScriptBundle("~/bundle/js/Parametrization").Include(
                    "~/Areas/Parametrization/Scripts/Vehicle/Fasecolda.es5.js",
                    "~/Areas/Parametrization/Scripts/Vehicle/AdvancedSearchFasecolda.es5.js",
                    "~/Areas/Parametrization/Scripts/Vehicle/VehicleFasecolda.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/ProtectionParametrization").Include(
                    "~/Areas/Parametrization/Scripts/Protection/ProtectionParametrization.es5.js",
                  "~/Areas/Parametrization/Scripts/Protection/ProtectionParametrizationRequest.es5.js",
                  "~/Areas/Parametrization/Scripts/Protection/ProtectionAdvancedSearch.es5.js"
                ));
                // GroundsRejectionParametrization
                bundles.Add(new ScriptBundle("~/bundle/js/GroundsRejection").Include(
                 "~/Areas/ParamAuthorizationPolicies/Scripts/GoundsRejection/GroundsRejectionCauses.es5.js",
                  "~/Areas/ParamAuthorizationPolicies/Scripts/GoundsRejection/AdvancedSearch.es5.js"
               ));

                //Parametrization/Surcharge
                bundles.Add(new ScriptBundle("~/bundle/js/Surcharge").Include(
                       "~/Areas/Parametrization/Scripts/Surcharge/Surcharge.es5.js"
                       ));
                //Parametrization/Discount
                bundles.Add(new ScriptBundle("~/bundle/js/Discount").Include(
                       "~/Areas/Parametrization/Scripts/Discount/Discount.es5.js"
                       ));
                //Parametrizaci?n detalle
                bundles.Add(new ScriptBundle("~/bundle/js/DetailParametrization").Include(
                "~/Areas/Parametrization/Scripts/Detail/DetailParametrization.es5.js"
                ));
                ////Parametrization/CoverageGroupParametrization
                bundles.Add(new ScriptBundle("~/bundle/js/CoverageGroupParametrization").Include(
                    "~/Areas/Parametrization/Scripts/CoverageGroup/CoverageGroupParametrization.es5.js"

                ));
                //Variable
                bundles.Add(new ScriptBundle("~/bundle/js/Variable").Include(
                    "~/Areas/Parametrization/Scripts/Variable/Variable.es5.js"
                    ));
                ////Parametrization/PaymentMethod
                bundles.Add(new ScriptBundle("~/bundle/js/PaymentMethod").Include(
                    "~/Areas/Parametrization/Scripts/PaymentMethod/PaymentMethod.es5.js"
                    ));
                //Parametrization/Clause
                bundles.Add(new ScriptBundle("~/bundle/js/ClauseParametrization").Include(
                    "~/Areas/Parametrization/Scripts/Clause/ClauseDetail.es5.js",
                    "~/Areas/Parametrization/Scripts/Clause/ClauseParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/Clause/ClauseParametrizationRequest.es5.js"


                    ));

                //Type Assistance
                bundles.Add(new ScriptBundle("~/bundle/js/AssistanceType").Include(
                    "~/Areas/Parametrization/Scripts/AssistanceType/AssistanceType.es5.js",
                    "~/Areas/Parametrization/Scripts/AssistanceType/AssistanceAdvancedSearch.es5.js",
                    "~/Areas/Parametrization/Scripts/AssistanceType/AsistenceText.es5.js"
                ));
                //Product 2G
                bundles.Add(new ScriptBundle("~/bundle/js/Product2G").Include(
                    "~/Areas/Parametrization/Scripts/Product2G/Product2G.es5.js",
                    "~/Areas/Parametrization/Scripts/Product2G/Product2GAdvancedSearch.es5.js"
                ));
                //BusinessConfiguration
                bundles.Add(new ScriptBundle("~/bundle/js/BusinessConfiguration").Include(
                    "~/Areas/Parametrization/Scripts/BusinessConfiguration/BusinessConfiguration.es5.js",
                    "~/Areas/Parametrization/Scripts/BusinessConfiguration/BusinessConfigurationAdvancedSearch.es5.js"
                    ));

                //WorkerType
                bundles.Add(new ScriptBundle("~/bundle/js/WorkerType").Include(
                    "~/Areas/Parametrization/Scripts/WorkerType/WorkerType.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/QuotationNumerationParametrization").Include(
                    "~/Areas/Parametrization/Scripts/QuotationNumeration/QuotationNumerationParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/QuotationNumeration/QuotationNumerationParametrizationRequest.es5.js"
                    ));
                // Parametrization/PolicyNumeration
                bundles.Add(new ScriptBundle("~/bundle/js/PolicyNumerationParametrization").Include(
                 "~/Areas/Parametrization/Scripts/PolicyNumeration/PolicyNumerationParametrization.es5.js",
                 "~/Areas/Parametrization/Scripts/PolicyNumeration/PolicyNumerationParametrizationRequest.es5.js"
                 ));
                //Auditoria
                bundles.Add(new ScriptBundle("~/bundle/js/Audit").Include(
                  "~/Areas/Audit/Scripts/Audit/Audit.es5.js",
                  "~/Areas/Audit/Scripts/Audit/AuditRequest.es5.js"
                  ));

                // RatingZone	
                bundles.Add(new ScriptBundle("~/bundle/js/RatingZoneParametrization").Include(
                    "~/Areas/Parametrization/Scripts/RatingZone/RatingZone.es5.js"
                ));

                //Concepts
                bundles.Add(new ScriptBundle("~/bundle/js/Concepts").Include(
                    "~/Areas/RulesScripts/Scripts/Concepts/ListEntity.es5.js",
                    "~/Areas/RulesScripts/Scripts/Concepts/ListEntityAdvancedSearch.es5.js",
                    "~/Areas/RulesScripts/Scripts/Concepts/RangeEntity.es5.js",
                    "~/Areas/RulesScripts/Scripts/Concepts/RangeEntityAdvancedSearch.es5.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/ReportRenovation").Include(
                    "~/Areas/Reports/RenovationReport/Scripts/ReportRenovation.es5.js"
                     ));
                //VehicleBody
                bundles.Add(new ScriptBundle("~/bundle/js/VehicleBody").Include(
                    "~/Areas/Parametrization/Scripts/VehicleBody/VehicleUse.es5.js",
                    "~/Areas/Parametrization/Scripts/VehicleBody/VehicleBody.es5.js"

                    ));
                //Parametrization FinancialPlan
                bundles.Add(new ScriptBundle("~/bundle/js/FinancialPlan").Include(
                    "~/Areas/Parametrization/Scripts/FinancialPlan/FinancialPlanParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/FinancialPlan/FinancialPlanParametrizationRequest.es5.js"
                    ));
                //Limit Rc
                bundles.Add(new ScriptBundle("~/bundle/js/LimitRc").Include(
                       "~/Areas/Parametrization/Scripts/LimitRc/LimitRc.es5.js"
                       ));

                //VehicleVersionYear
                bundles.Add(new ScriptBundle("~/bundle/js/VehicleVersionYear").Include(
                    "~/Areas/Parametrization/Scripts/VehicleVersionYear/Index.es5.js",
                    "~/Areas/Parametrization/Scripts/VehicleVersionYear/AdvancedSearch.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/VehicleModel").Include(
                   "~/Areas/Parametrization/Scripts/VehicleModel/VehicleModel.es5.js",
                   "~/Areas/Parametrization/Scripts/VehicleModel/VehicleModelSearch.es5.js"

               ));

                bundles.Add(new ScriptBundle("~/bundle/js/TechnicalPlan").Include(
                    "~/Areas/Parametrization/Scripts/TechnicalPlan/TechnicalPlan.es5.js",
                    "~/Areas/Parametrization/Scripts/TechnicalPlan/TechnicalPlanAllyCoverages.es5.js",
                    "~/Areas/Parametrization/Scripts/TechnicalPlan/TechnicalPlanCoverages.es5.js",
                    "~/Areas/Parametrization/Scripts/TechnicalPlan/TechnicalPlanSearch.es5.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/Tax").Include(
                    "~/Areas/Parametrization/Scripts/Tax/Tax.es5.js",
                    "~/Areas/Parametrization/Scripts/Tax/TaxRequests.es5.js",
                    "~/Areas/Parametrization/Scripts/Tax/CategoryTax.es5.js",
                    "~/Areas/Parametrization/Scripts/Tax/CategoryTaxRequests.es5.js",
                   "~/Areas/Parametrization/Scripts/Tax/ConditionTax.es5.js",
                   "~/Areas/Parametrization/Scripts/Tax/ConditionTaxRequests.es5.js",
                   "~/Areas/Parametrization/Scripts/Tax/RateTax.es5.js",
                   "~/Areas/Parametrization/Scripts/Tax/RateTaxRequests.es5.js",
                   "~/Areas/Parametrization/Scripts/Tax/RateTaxAdvancedSearch.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/Wallet").Include(
                    "~/Areas/Reports/Scripts/Wallet/Wallet.es5.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementExtension").Include(
                   "~/Areas/Endorsement/Scripts/ExtensionRequest.es5.js",
                   "~/Areas/Endorsement/Scripts/Extension.es5.js"

               ));

                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementCancellation").Include(
                 "~/Areas/Endorsement/Scripts/CancellationRequest.es5.js",
               "~/Areas/Endorsement/Scripts/Cancellation.es5.js"

               ));

                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementPaymentPlan").Include(
                    "~/Areas/Endorsement/Scripts/PaymentPlanRequest.es5.js",
                    "~/Areas/Endorsement/Scripts/PaymentPlan.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementTexts").Include(
                    "~/Areas/Endorsement/Scripts/TextsRequest.es5.js",
                    "~/Areas/Endorsement/Scripts/Texts.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementRenewal").Include(
                    "~/Areas/Endorsement/Scripts/RenewalRequest.es5.js",
                    "~/Areas/Endorsement/Scripts/Renewal.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementReversion").Include(
                    "~/Areas/Endorsement/Scripts/ReversionRequest.es5.js",
                    "~/Areas/Endorsement/Scripts/Reversion.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/SearchEndorsement").Include(
                "~/Areas/Endorsement/Scripts/Search/SearchIndex.js",
               "~/Areas/Endorsement/Scripts/SearchRequest.es5.js",
               "~/Areas/Endorsement/Scripts/Search.es5.js",
               "~/Areas/Collective/Scripts/Collective/Consultation/Texts.es5.js",
               "~/Areas/Collective/Scripts/Collective/Consultation/ClausesRequest.es5.js",
               "~/Areas/Collective/Scripts/Collective/Consultation/Clauses.es5.js",
               "~/Areas/Collective/Scripts/Collective/Consultation/PaymentPlan.es5.js",
               "~/Areas/Collective/Scripts/Collective/Consultation/Agents.es5.js"
               ));
                bundles.Add(new ScriptBundle("~/bundle/js/Property").Include(
                    "~/Areas/Underwriting/Scripts/RiskProperty/PropertyRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/Property.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/AdditionalData.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/AdditionalDataRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/InsuredObjectRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskProperty/InsuredObject.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/ThirdPartyLiability").Include(
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/RiskThirdPartyLiability.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/RiskThirdPartyLiabilityRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/RiskThirdPartyLiabilityCoverage.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/RiskThirdPartyLiabilityCoverageRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskThirdPartyLiability/AdditionalData.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementModification").Include(
                    "~/Areas/Endorsement/Scripts/ModificationRequest.es5.js",
                    "~/Areas/Endorsement/Scripts/Modification.es5.js",
                     "~/Areas/Endorsement/Scripts/UnderwritingEndorsement.es5.js"
              ));

                bundles.Add(new ScriptBundle("~/bundle/js/Liability").Include(
                "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiability.es5.js",
                "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiabilityRequest.es5.js",
                "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiabilityCoverage.es5.js",
                "~/Areas/Underwriting/Scripts/RiskLiability/RiskLiabilityCoverageRequest.es5.js"

                ));
                //Fidelity
                bundles.Add(new ScriptBundle("~/bundle/js/Fidelity").Include(
                "~/Areas/Underwriting/Scripts/RiskFidelity/RiskFidelity.es5.js",
                "~/Areas/Underwriting/Scripts/RiskFidelity/RiskFidelityRequest.es5.js",
                "~/Areas/Underwriting/Scripts/RiskFidelity/Coverage.es5.js",
                "~/Areas/Underwriting/Scripts/RiskFidelity/CoverageRequest.es5.js"
                ));
                //Transport
                bundles.Add(new ScriptBundle("~/bundle/js/Transport").Include(
                    "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransport.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportCoverage.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskTransport/RiskTransportCoverageRequest.es5.js"

                    ));
                //Adjustment 
                bundles.Add(new ScriptBundle("~/bundle/js/Adjustment").Include(
                "~/Areas/Endorsement/Scripts/Adjustment.es5.js",
                "~/Areas/Endorsement/Scripts/AdjustmentRequest.es5.js"
                    ));

                //CreditNote
                bundles.Add(new ScriptBundle("~/bundle/js/CreditNote").Include(
                "~/Areas/Endorsement/Scripts/CreditNote.es5.js",
                "~/Areas/Endorsement/Scripts/CreditNoteRequest.es5.js"
                ));

                //Declaration
                bundles.Add(new ScriptBundle("~/bundle/js/Declaration").Include(
                "~/Areas/Endorsement/Scripts/Declaration.es5.js",
                "~/Areas/Endorsement/Scripts/DeclarationRequest.es5.js"
                ));

                //VehicleFasecolda
                bundles.Add(new ScriptBundle("~/bundle/js/MassiveVehicleFasecolda").Include(
                    "~/Areas/MassiveVehicleFasecolda/Scripts/MassiveVehicleFasecolda/MassiveVehicleFasecolda.es5.js",
                    "~/Areas/MassiveVehicleFasecolda/Scripts/MassiveVehicleFasecolda/MassiveVehicleFasecoldaRequest.es5.js"
                ));

                //ListRiskPerson
                bundles.Add(new ScriptBundle("~/bundle/js/ListRisk").Include(
                    "~/Areas/ListRiskPerson/Scripts/ListRiskPerson.es5.js",
                    "~/Areas/ListRiskPerson/Scripts/ListRiskPersonRequest.es5.js",
                    "~/Areas/ListRiskPerson/Scripts/ListRiskFile.es5.js",
                    "~/Areas/ListRiskPerson/Scripts/ListRiskFileRequest.es5.js",
                    "~/Areas/ListRiskPerson/Scripts/MatchingProcess.es5.js",
                    "~/Areas/ListRiskPerson/Scripts/MatchingProcessRequest.es5.js"
                ));
                bundles.Add(new ScriptBundle("~/bundle/js/EventSarlaf").Include(
               "~/Areas/EventsSarlaft/Scripts/EventsSarlaft/EventsSarlaft.es5.js",
               "~/Areas/EventsSarlaft/Scripts/EventsSarlaft/EventsSarlaftRequest.es5.js",
               "~/Areas/EventsSarlaft/Scripts/EventsSarlaft/EventsSarlaftAdvanceSearch.es5.js"
               ));

                //QuotaOperation
                bundles.Add(new ScriptBundle("~/bundle/js/QuotaOperation").Include(
                    "~/Areas/QuotaOperational/Scripts/QuotaOperational/QuotaOperational.es5.js",
                    "~/Areas/QuotaOperational/Scripts/QuotaOperational/QuotaOperationalRequest.es5.js"
                ));
                //AutomaticQuota
                bundles.Add(new ScriptBundle("~/bundle/js/AutomaticQuota").Include(
               "~/Areas/AutomaticQuota/Scripts/AutomaticQuota.es5.js",
               "~/Areas/AutomaticQuota/Scripts/AutomaticQuotaRequest.es5.js",
                "~/Areas/AutomaticQuota/Scripts/AdvancedSearchAutomaticQuota.es5.js"
               ));
                //Surety
                bundles.Add(new ScriptBundle("~/bundle/js/Surety").Include(
                    "~/Areas/Underwriting/Scripts/RiskSurety/Surety.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/SuretyRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/ContractObject.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/ContractObjectRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/CrossGuarantees.es5.js",
                     "~/Areas/Underwriting/Scripts/RiskSurety/CrossGuaranteesRequest.es5.js",
                       "~/Areas/Underwriting/Scripts/RiskSurety/OperationQuotaCumulus.es5.js",
                     "~/Areas/Underwriting/Scripts/RiskSurety/OperationQuotaCumulusRequest.es5.js"
                    ));
                //CoverageSurety
                bundles.Add(new ScriptBundle("~/bundle/js/CoverageSurety").Include(
                    "~/Areas/Underwriting/Scripts/RiskSurety/RiskSuretyCoverage.es5.js",
                    "~/Areas/Underwriting/Scripts/RiskSurety/CoverageRequest.es5.js"
                    ));

                //CoverageSurety
                bundles.Add(new ScriptBundle("~/bundle/js/ContractObject").Include(
                    "~/Areas/Underwriting/Scripts/ContractObject/ContractObject.es5.js",
                    "~/Areas/Underwriting/Scripts/ContractObject/ContractObjectRequest.es5.js"
                    ));

                bundles.Add(new ScriptBundle("~/bundle/js/RiskConcepts").Include(
                    "~/Areas/Underwriting/Scripts/Coverages/Concepts.es5.js",
                    "~/Areas/Underwriting/Scripts/Risks/Concepts.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/AuthorizeUser").Include(
                    "~/Areas/Collective/Scripts/Collective/AuthorizeUserTemporal.es5.js"
                    ));
                //ScoreCredit
                bundles.Add(new ScriptBundle("~/bundle/js/ScoreCredit").Include(
                    "~/Areas/Reports/Scripts/ScoreCredit/ScoreCredit.es5.js"
                    ));
                // Group Policies
                bundles.Add(new ScriptBundle("~/bundle/js/GroupPolicies").Include(
                 "~/Areas/AuthorizationPolicies/Scripts/GroupPolicies/GroupPolicies.es5.js",
                  "~/Areas/AuthorizationPolicies/Scripts/GroupPolicies/AdvancedSearchGroupPolicies.es5.js"
               ));
                // Ally Coverage
                bundles.Add(new ScriptBundle("~/bundle/js/AllyCoverage").Include(
                 "~/Areas/Parametrization/Scripts/AllyCoverage/AllyCoverage.es5.js",
                  "~/Areas/Parametrization/Scripts/AllyCoverage/AllyCoverageAdvancedSearch.es5.js"
               ));
                // Report Authorization Policies
                bundles.Add(new ScriptBundle("~/bundle/js/ReportAuthorizationPolicies").Include(
                 "~/Areas/AuthorizationPolicies/Scripts/ReportAuthorizationPolicies/ReportAuthorizationPolicies.es5.js"
               ));

                //Parametrization/TextPrecatalogued
                bundles.Add(new ScriptBundle("~/bundle/js/TextPrecatalogued").Include(
                    "~/Areas/Parametrization/Scripts/TextPrecatalogued/TextPrecataloguedDetail.es5.js",
                    "~/Areas/Parametrization/Scripts/TextPrecatalogued/TextPrecataloguedParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/TextPrecatalogued/TextPrecataloguedParametrizationRequest.es5.js"
                    ));
                //Parametrization/ValidationPlate
                bundles.Add(new ScriptBundle("~/bundle/js/ValidationPlate").Include(
                    "~/Areas/Parametrization/Scripts/ValidationPlate/ValidationPlateDetail.es5.js",
                    "~/Areas/Parametrization/Scripts/ValidationPlate/ValidationPlateParametrization.es5.js",
                    "~/Areas/Parametrization/Scripts/ValidationPlate/ValidationPlateParametrizationRequest.es5.js"
                    ));
                //Parametrization/DocumentTypeRange
                bundles.Add(new ScriptBundle("~/bundle/js/DocumentTypeRange").Include(
                    "~/Areas/Parametrization/Scripts/DocumentTypeRange/DocumentTypeRange.es5.js"

                    ));
                //BILLING PERIOD
                bundles.Add(new ScriptBundle("~/bundle/js/BillingPeriod").Include(
                       "~/Areas/Parametrization/Scripts/BillingPeriod/BillingPeriod.es5.js"
                       ));

                // Report Business Type
                bundles.Add(new ScriptBundle("~/bundle/js/BusinessTypes").Include(
                 "~/Areas/Parametrization/Scripts/BusinessType/BusinessTypes.es5.js"
               ));

                // Parametrizacion ciudades
                bundles.Add(new ScriptBundle("~/bundle/js/City").Include(
               "~/Areas/Parametrization/Scripts/City/City.es5.js",
               "~/Areas/Parametrization/Scripts/City/SearchAdvCity.es5.js"
             ));

                bundles.Add(new ScriptBundle("~/bundle/js/MinPremiunRelation").Include(
                    "~/Areas/Parametrization/Scripts/MinPremiunRelation/MinPremiunRelation.es5.js",
                    "~/Areas/Parametrization/Scripts/MinPremiunRelation/MinPremiunRelationRequest.es5.js"
                    ));

                //Endosos
                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementClauses").Include(
                   "~/Areas/Endorsement/Scripts/ClausesEndorsementRequest.es5.js",
                   "~/Areas/Endorsement/Scripts/Clauses.es5.js"
               ));
                // Parametrizacion coverage value
                bundles.Add(new ScriptBundle("~/bundle/js/CoverageValue").Include(
               "~/Areas/Parametrization/Scripts/CoverageValue/CoverageValue.es5.js",
               "~/Areas/Parametrization/Scripts/CoverageValue/SearchAdvCoCoverageValue.es5.js"
               ));

                //Cexper
                bundles.Add(new ScriptBundle("~/bundle/js/Cexper").Include(
                   "~/Areas/Externals/Scripts/Cexper.es5.js"
                 ));

                //Sisa
                bundles.Add(new ScriptBundle("~/bundle/js/Sisa").Include(
                   "~/Areas/Externals/Scripts/FasecoldaSISA.es5.js"
                 ));

                //ProductionReport
                bundles.Add(new ScriptBundle("~/bundle/js/ProductionReport").Include(
                  "~/Areas/Underwriting/Scripts/Underwriting/ProductionReport.es5.js",
                  "~/Areas/Underwriting/Scripts/Underwriting/ProductionReportRequest.es5.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/Sup").Include(
                              "~/Areas/Sup/Scripts/Sup.es5.js"
                ));

                //Sarlaft
                bundles.Add(new ScriptBundle("~/bundle/js/Sarlaft").Include(
                    "~/Areas/Sarlaft/Scripts/InternationalOperations.es5.js",
                    "~/Areas/Sarlaft/Scripts/LegalRepresentative.es5.js",
                    "~/Areas/Sarlaft/Scripts/Links.es5.js",
                    "~/Areas/Sarlaft/Scripts/Partners.es5.js",
                    "~/Areas/Sarlaft/Scripts/Peps.es5.js",
                    "~/Areas/Sarlaft/Scripts/Exempt.es5.js",
                    "~/Areas/Sarlaft/Scripts/FinalBeneficiary.es5.js",
                    "~/Areas/Sarlaft/Scripts/SarlaftParam.es5.js",
                    "~/Areas/Sarlaft/Scripts/SarlaftRequest.es5.js",
                    "~/Areas/Sarlaft/Scripts/SarlaftSimpleSearch.es5.js"

                 ));
                //Printing
                bundles.Add(new ScriptBundle("~/bundle/js/Printer").Include(
                   "~/Areas/Printing/Scripts/Printer/PrinterRequest.es5.js",
                   "~/Areas/Printing/Scripts/Printer/Printer.es5.js"
                 ));

                bundles.Add(new ScriptBundle("~/bundle/js/CollectFormat").Include(
                "~/Areas/Printing/Scripts/Printer/CollectFormat.es5.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/CounterGuarantee").Include(
               "~/Areas/Printing/Scripts/Printer/CounterGuarantee.es5.js",
                "~/Areas/Printing/Scripts/Printer/CounterGuaranteeRequest.es5.js"
               ));

                bundles.Add(new ScriptBundle("~/bundle/js/MassivePrinter").Include(
                "~/Areas/Printing/Scripts/Printer/MassivePrinter.es5.js"
                ));

                bundles.Add(new ScriptBundle("~/bundle/js/ConditionText").Include(
                 "~/Areas/Parametrization/Scripts/ConditionText/ConditionText.es5.js"
              ));

                //SubscriptionSearch
                bundles.Add(new ScriptBundle("~/bundle/js/SubscriptionSearch").Include(
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/SubscriptionSearch.es5.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/SubscriptionSearchRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/QuotationSearch.es5.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/QuotationSearchRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/TemporalSearch.es5.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/TemporalSearchRequest.es5.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/PolicySearch.es5.js",
                    "~/Areas/Underwriting/Scripts/SearchUnderwriting/PolicySearchRequest.es5.js"
                ));

                /* CLAIMS RELEASE */

                //EstimationTypePrefix
                bundles.Add(new ScriptBundle("~/bundle/js/EstimationTypePrefix").Include(
                "~/Areas/Parametrization/Scripts/EstimationTypePrefix/EstimationTypePrefix.es5.js",
                "~/Areas/Parametrization/Scripts/EstimationTypePrefix/EstimationTypePrefixRequest.es5.js"
                ));

                //CauseCoverage
                bundles.Add(new ScriptBundle("~/bundle/js/CauseCoverage").Include(
                "~/Areas/Parametrization/Scripts/CauseCoverage/CauseCoverage.es5.js",
                "~/Areas/Parametrization/Scripts/CauseCoverage/CauseCoverageRequest.es5.js"
                ));

                //Subcause
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsAssociation").Include(
                "~/Areas/Parametrization/Scripts/ClaimsAssociation/ClaimsAssociation.es5.js",
                "~/Areas/Parametrization/Scripts/ClaimsAssociation/ClaimsAssociationRequest.es5.js"
                ));

                //Subcause
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsSubCause").Include(
                "~/Areas/Parametrization/Scripts/ClaimsSubCause/ClaimsSubCause.es5.js",
                "~/Areas/Parametrization/Scripts/ClaimsSubCause/ClaimsSubCauseRequest.es5.js"
                ));

                //ClaimsDocument
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsDocument").Include(
                "~/Areas/Parametrization/Scripts/ClaimsDocument/ClaimsDocument.es5.js",
                "~/Areas/Parametrization/Scripts/ClaimsDocument/ClaimsDocumentRequest.es5.js"
                ));

                //RoleAndSubroles
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsPersonCompany").Include(
                "~/Areas/Parametrization/Scripts/ClaimsPersonCompany/ClaimsPersonCompany.es5.js",
                "~/Areas/Parametrization/Scripts/ClaimsPersonCompany/ClaimsPersonCompanyRequest.es5.js"
                ));

                //ClaimsLackPeriod
                //LackPeriod
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimsLackPeriod").Include(
                "~/Areas/Parametrization/Scripts/ClaimsLackPeriod/ClaimsLackPeriod.es5.js",
                "~/Areas/Parametrization/Scripts/ClaimsLackPeriod/ClaimsLackPeriodRequest.es5.js"
                ));

                //EstimationTypeStatus
                bundles.Add(new ScriptBundle("~/bundle/js/EstimationTypeStatus").Include(
                "~/Areas/Parametrization/Scripts/EstimationTypeStatus/EstimationTypeStatus.es5.js",
                "~/Areas/Parametrization/Scripts/EstimationTypeStatus/EstimationTypeStatusRequest.es5.js"
                ));

                //GuaranteStatusRoute
                bundles.Add(new ScriptBundle("~/bundle/js/GuaranteeStatusRoute").Include(
                "~/Areas/Parametrization/Scripts/GuaranteeStatusRoute/GuaranteeStatusRoute.es5.js",
                "~/Areas/Parametrization/Scripts/GuaranteeStatusRoute/GuaranteeStatusRouteRequest.es5.js"
                ));

                //Parametrization/ConfigurationPanels/ConfigurationPanels
                bundles.Add(new ScriptBundle("~/bundle/js/ConfigurationPanels").Include(
                "~/Areas/Parametrization/Scripts/ConfigurationPanels/ConfigurationPanels.es5.js",
                "~/Areas/Parametrization/Scripts/ConfigurationPanels/ConfigurationPanelsRequest.es5.js"
                ));


                //Claim/Notice
                bundles.Add(new ScriptBundle("~/bundle/js/Notice").Include(
                "~/Areas/Claims/Scripts/Notice/Notice.es5.js",
                "~/Areas/Claims/Scripts/Notice/NoticeRequest.es5.js"
                ));

                //Claim/Notice/Location
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeLocation").Include(
                "~/Areas/Claims/Scripts/NoticeLocation/NoticeLocation.es5.js",
                "~/Areas/Claims/Scripts/NoticeLocation/NoticeLocationRequest.es5.js"
                ));

                //Claim/Notice/Vehicle
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeVehicle").Include(
                "~/Areas/Claims/Scripts/NoticeVehicle/NoticeVehicle.es5.js",
                "~/Areas/Claims/Scripts/NoticeVehicle/NoticeVehicleRequest.es5.js"
                ));

                //Claim/Notice/Surety
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeSurety").Include(
                "~/Areas/Claims/Scripts/NoticeSurety/NoticeSurety.es5.js",
                "~/Areas/Claims/Scripts/NoticeSurety/NoticeSuretyRequest.es5.js"
                ));

                //Claim/Notice/Transport
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeTransport").Include(
                "~/Areas/Claims/Scripts/NoticeTransport/NoticeTransport.es5.js",
                "~/Areas/Claims/Scripts/NoticeTransport/NoticeTransportRequest.es5.js"
                ));

                //Claim/Notice/AirCraft
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeAirCraft").Include(
                "~/Areas/Claims/Scripts/NoticeAirCraft/NoticeAirCraft.es5.js",
                "~/Areas/Claims/Scripts/NoticeAirCraft/NoticeAirCraftRequest.es5.js"
                ));

                //Claim/Notice/Fidelity
                bundles.Add(new ScriptBundle("~/bundle/js/NoticeFidelity").Include(
                "~/Areas/Claims/Scripts/NoticeFidelity/NoticeFidelity.es5.js",
                "~/Areas/Claims/Scripts/NoticeFidelity/NoticeFidelityRequest.es5.js"
                ));

                //Claim
                bundles.Add(new ScriptBundle("~/bundle/js/Claim").Include(
                "~/Areas/Claims/Scripts/Claim/Claim.es5.js",
                "~/Areas/Claims/Scripts/Claim/ClaimRequest.es5.js",
                "~/Areas/Claims/Scripts/Claim/ClaimEstimation.es5.js",
                "~/Areas/Claims/Scripts/Claim/ClaimEstimationRequest.es5.js"
                ));

                //Claim/Vehicle
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimVehicle").Include(
                "~/Areas/Claims/Scripts/ClaimVehicle/ClaimVehicle.es5.js",
                "~/Areas/Claims/Scripts/ClaimVehicle/ClaimVehicleRequest.es5.js"
                ));

                //Claim/Location
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimLocation").Include(
                "~/Areas/Claims/Scripts/ClaimLocation/ClaimLocation.es5.js",
                "~/Areas/Claims/Scripts/ClaimLocation/ClaimLocationRequest.es5.js"
                ));

                //Claim/Surety
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimSurety").Include(
                "~/Areas/Claims/Scripts/ClaimSurety/ClaimSurety.es5.js",
                "~/Areas/Claims/Scripts/ClaimSurety/ClaimSuretyRequest.es5.js"
                ));

                //Claim/Salvage
                bundles.Add(new ScriptBundle("~/bundle/js/Salvage").Include(
                "~/Areas/Claims/Scripts/Salvage/Salvage.es5.js",
                "~/Areas/Claims/Scripts/Salvage/SalvageRequest.es5.js"
                ));

                //Claim/Recovery
                bundles.Add(new ScriptBundle("~/bundle/js/Recovery").Include(
                "~/Areas/Claims/Scripts/Recovery/Recovery.es5.js",
                "~/Areas/Claims/Scripts/Recovery/RecoveryRequest.es5.js"
                ));

                //Claim/PaymentRequest
                bundles.Add(new ScriptBundle("~/bundle/js/Payment").Include(
                "~/Areas/Claims/Scripts/PaymentRequest/Payment.es5.js",
                "~/Areas/Claims/Scripts/PaymentRequest/ClaimsPaymentRequest.es5.js"
                ));

                //Claim/ChargeRequest
                bundles.Add(new ScriptBundle("~/bundle/js/Charge").Include(
                "~/Areas/Claims/Scripts/Charge/Charge.es5.js",
                "~/Areas/Claims/Scripts/Charge/ChargeRequest.es5.js"
                ));

                //Claim/Search
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimSearch").Include(
                "~/Areas/Claims/Scripts/Search/ClaimSearch.es5.js",
                "~/Areas/Claims/Scripts/Search/ClaimSearchRequest.es5.js"
                ));

                //Claim/CancelRequest
                bundles.Add(new ScriptBundle("~/bundle/js/RequestCancellation").Include(
                "~/Areas/Claims/Scripts/RequestCancellation/RequestCancellation.es5.js",
                "~/Areas/Claims/Scripts/RequestCancellation/RequestCancellationRequest.es5.js"
                ));

                //Claims/SetClaimReserve
                bundles.Add(new ScriptBundle("~/bundle/js/SetClaimReserve").Include(
                "~/Areas/Claims/Scripts/SetClaimReserve/SetClaimReserve.es5.js",
                "~/Areas/Claims/Scripts/SetClaimReserve/SetClaimReserveRequest.es5.js"
                ));
                bundles.Add(new ScriptBundle("~/bundle/js/Billing").Include(
                "~/Areas/Endorsement/Scripts/BillingRequest.es5.js",
                "~/Areas/Endorsement/Scripts/Billing.es5.js"));

                //Claims/ClaimTransport
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimTransport").Include(
                "~/Areas/Claims/Scripts/ClaimTransport/ClaimTransport.es5.js",
                "~/Areas/Claims/Scripts/ClaimTransport/ClaimTransportRequest.es5.js"
                ));

                //Claims/ClaimAirCraft
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimAirCraft").Include(
                "~/Areas/Claims/Scripts/ClaimAirCraft/ClaimAirCraft.es5.js",
                "~/Areas/Claims/Scripts/ClaimAirCraft/ClaimAirCraftRequest.es5.js"
                ));

                //Claims/ClaimFidelity
                bundles.Add(new ScriptBundle("~/bundle/js/ClaimFidelity").Include(
                "~/Areas/Claims/Scripts/ClaimFidelity/ClaimFidelity.es5.js",
                "~/Areas/Claims/Scripts/ClaimFidelity/ClaimFidelityRequest.es5.js"
                ));

                // Claims/AutomaticSalaryUpdate
                bundles.Add(new ScriptBundle("~/bundle/js/AutomaticSalaryUpdate").Include(
                "~/Areas/Claims/Scripts/AutomaticSalaryUpdate/AutomaticSalaryUpdate.es5.js",
                "~/Areas/Claims/Scripts/AutomaticSalaryUpdate/AutomaticSalaryUpdateRequest.es5.js"
                ));
                /* CLAIMS RELEASE */

                bundles.Add(new ScriptBundle("~/bundle/js/EndorsementChangeTerm").Include(
               "~/Areas/Endorsement/Scripts/ChangeTerm.es5.js",
               "~/Areas/Endorsement/Scripts/ChangeTermRequest.es5.js"
               ));
                //TaxConceptsExpenses
                bundles.Add(new ScriptBundle("~/bundle/js/TaxConceptsExpenses").Include(
                    "~/Areas/Parametrization/Scripts/Tax/TaxConceptsExpenses.es5.js",
                    "~/Areas/Parametrization/Scripts/Tax/TaxConceptsExpensesRequests.es5.js"
                    ));

                // Reasign Policies
                bundles.Add(new ScriptBundle("~/bundle/js/ReassignPolicies").Include(
                 "~/Areas/AuthorizationPolicies/Scripts/ReassignPolicies/ReassignPolicies.es5.js"
               ));
                //Article
                bundles.Add(new ScriptBundle("~/bundle/js/ProductArticle").Include(
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ProductArticle.es5.js",
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ProductArticleRequest.es5.js",
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ProductArticleSearch.es5.js"
                    ));
                //ArticleLine
                bundles.Add(new ScriptBundle("~/bundle/js/ArticleLine").Include(
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ArticleLine.es5.js",
                    "~/Areas/Parametrization/Scripts/JudicialSurety/ArticleLineRequest.es5.js",
                     "~/Areas/Parametrization/Scripts/JudicialSurety/ArticleLineSearch.es5.js"
                    ));
                bundles.Add(new ScriptBundle("~/bundle/js/CourtType").Include(
                   "~/Areas/Parametrization/Scripts/JudicialSurety/CourtType.es5.js",
                   "~/Areas/Parametrization/Scripts/JudicialSurety/CourtTypeRequest.es5.js",
                    "~/Areas/Parametrization/Scripts/JudicialSurety/CourtTypeSearch.es5.js"
                   ));

            }
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}

