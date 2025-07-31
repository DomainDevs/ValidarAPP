using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Configuration;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance
{
    public class ReinsuranceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reinsurance";
            }
        }

        /// <summary>
        /// RegisterArea
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reinsurance_default",
                "Reinsurance/{controller}/{action}/{id}",
                new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            var compileDebug = (CompilationSection)System.Configuration.ConfigurationManager.GetSection("system.web/compilation");

            if (compileDebug.Debug)
            {
                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsurePolicies").
                Include("~/Areas/Reinsurance/Scripts/Process/ReinsurePolicies.js"));            //REASEGURAR POLIZAS

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/FindReinsurance").
                Include("~/Areas/Reinsurance/Scripts/Process/FindReinsurance.js"));             //CONSULTA DE PAGOS

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsurePoliciesRangeDate").
                  Include("~/Areas/Reinsurance/Scripts/Process/ReinsurePoliciesRangeDate.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/SearchReinsureProcessMassive").
                Include("~/Areas/Reinsurance/Scripts/Process/SearchReinsureProcessMassive.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsureReports").
                Include("~/Areas/Reinsurance/Scripts/Process/ReinsureReports.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsureClaims").
                Include("~/Areas/Reinsurance/Scripts/Process/ReinsureClaims.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsurePayments").
                Include("~/Areas/Reinsurance/Scripts/Process/ReinsurePayments.js"));

                var jsappReinsBundle = new ScriptBundle("~/reinsurance/bundles/MainContract");
                BundleTable.Bundles.Add(jsappReinsBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/MainContract.js"   //PARAMETRIZACIÓN CONTRATOS
                   , "~/Areas/Reinsurance/Scripts/Parameter/ContractDialog.js"
                   , "~/Areas/Reinsurance/Scripts/Parameter/ContractLineDialog.js"
                   , "~/Areas/Reinsurance/Scripts/Parameter/ContractLevelDialog.js"
                   , "~/Areas/Reinsurance/Scripts/Parameter/ContractLevelCompanyDialog.js"));

                var jsappBundle = new ScriptBundle("~/reinsurance/bundles/MainLine");
                BundleTable.Bundles.Add(jsappBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/MainLine.js"   //PARAMETRIZACIÓN LÍNEAS
                      , "~/Areas/Reinsurance/Scripts/Parameter/LineDialog.js"
                      , "~/Areas/Reinsurance/Scripts/Parameter/ContractDialog.js"
                      , "~/Areas/Reinsurance/Scripts/Parameter/ContractLineDialog.js"
                      , "~/Areas/Reinsurance/Scripts/Parameter/ContractLevelDialog.js"
                      , "~/Areas/Reinsurance/Scripts/Parameter/ContractLevelCompanyDialog.js"));

                var jspaoBundle = new ScriptBundle("~/reinsurance/bundles/AssociationLine");
                BundleTable.Bundles.Add(jspaoBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/AssociationLine.js"   //PARAMETRIZACIÓN ASOCIACIÓN DE LÍNEAS
                      , "~/Areas/Reinsurance/Scripts/Parameter/AssociationLineDialog.js"));

                var jsprBundle = new ScriptBundle("~/reinsurance/bundles/PrefixCumulus");
                BundleTable.Bundles.Add(jsprBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/MainPrefixReinsurance.js"   //PARAMETRIZACIÓN Ramos Reaseguros
                      , "~/Areas/Reinsurance/Scripts/Parameter/ModalPrefixReinsurance.js"));

                var jscumBundle = new ScriptBundle("~/reinsurance/bundles/QueryCumulus");
                BundleTable.Bundles.Add(jscumBundle.Include("~/Areas/Reinsurance/Scripts/Process/QueryCumulus.js"
                    , "~/Areas/Reinsurance/Scripts/Process/QueryCumulusRequest.js"));

                var jsPriRetBundle = new ScriptBundle("~/reinsurance/bundles/PriorityRetention");
                BundleTable.Bundles.Add(jsPriRetBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/PriorityRetention.js"            //PARAMETRIZACIÓN RETENCIÓN PRIORITARIA
                    , "~/Areas/Reinsurance/Scripts/Parameter/PriorityRetentionRequest.js"));
            }
            else
            {
                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsurePolicies").
                Include("~/Areas/Reinsurance/Scripts/Process/ReinsurePolicies.es5.js"));            //REASEGURAR POLIZAS

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/FindReinsurance").
                Include("~/Areas/Reinsurance/Scripts/Process/FindReinsurance.es5.js"));             //CONSULTA DE PAGOS

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsurePoliciesRangeDate").
                  Include("~/Areas/Reinsurance/Scripts/Process/ReinsurePoliciesRangeDate.es5.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/SearchReinsureProcessMassive").
                Include("~/Areas/Reinsurance/Scripts/Process/SearchReinsureProcessMassive.es5.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsureReports").
                Include("~/Areas/Reinsurance/Scripts/Process/ReinsureReports.es5.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsureClaims").
                Include("~/Areas/Reinsurance/Scripts/Process/ReinsureClaims.es5.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/reinsurance/bundles/ReinsurePayments").
                Include("~/Areas/Reinsurance/Scripts/Process/ReinsurePayments.es5.js"));

                var jsappReinsBundle = new ScriptBundle("~/reinsurance/bundles/MainContract");
                BundleTable.Bundles.Add(jsappReinsBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/MainContract.es5.js"   //PARAMETRIZACIÓN CONTRATOS
                   , "~/Areas/Reinsurance/Scripts/Parameter/ContractDialog.es5.js"
                   , "~/Areas/Reinsurance/Scripts/Parameter/ContractLineDialog.es5.js"
                   , "~/Areas/Reinsurance/Scripts/Parameter/ContractLevelDialog.es5.js"
                   , "~/Areas/Reinsurance/Scripts/Parameter/ContractLevelCompanyDialog.es5.js"));

                var jsappBundle = new ScriptBundle("~/reinsurance/bundles/MainLine");
                BundleTable.Bundles.Add(jsappBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/MainLine.es5.js"   //PARAMETRIZACIÓN LÍNEAS
                      , "~/Areas/Reinsurance/Scripts/Parameter/LineDialog.es5.js"
                      , "~/Areas/Reinsurance/Scripts/Parameter/ContractDialog.es5.js"
                      , "~/Areas/Reinsurance/Scripts/Parameter/ContractLineDialog.es5.js"
                      , "~/Areas/Reinsurance/Scripts/Parameter/ContractLevelDialog.es5.js"
                      , "~/Areas/Reinsurance/Scripts/Parameter/ContractLevelCompanyDialog.es5.js"));

                var jspaoBundle = new ScriptBundle("~/reinsurance/bundles/AssociationLine");
                BundleTable.Bundles.Add(jspaoBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/AssociationLine.es5.js"   //PARAMETRIZACIÓN ASOCIACIÓN DE LÍNEAS
                      , "~/Areas/Reinsurance/Scripts/Parameter/AssociationLineDialog.es5.js"));

                var jsprBundle = new ScriptBundle("~/reinsurance/bundles/PrefixCumulus");
                BundleTable.Bundles.Add(jsprBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/MainPrefixReinsurance.es5.js"   //PARAMETRIZACIÓN Ramos Reaseguros
                      , "~/Areas/Reinsurance/Scripts/Parameter/ModalPrefixReinsurance.es5.js"));

                var jscumBundle = new ScriptBundle("~/reinsurance/bundles/QueryCumulus");
                BundleTable.Bundles.Add(jscumBundle.Include("~/Areas/Reinsurance/Scripts/Process/QueryCumulus.es5.js"
                    , "~/Areas/Reinsurance/Scripts/Process/QueryCumulusRequest.es5.js"));

                var jsPriRetBundle = new ScriptBundle("~/reinsurance/bundles/PriorityRetention");
                BundleTable.Bundles.Add(jsPriRetBundle.Include("~/Areas/Reinsurance/Scripts/Parameter/PriorityRetention.es5.js"            //PARAMETRIZACIÓN RETENCIÓN PRIORITARIA
                    , "~/Areas/Reinsurance/Scripts/Parameter/PriorityRetentionRequest.es5.js"));
            }
        }
    }
}
