using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Configuration;

namespace Sistran.Core.Framework.UIF.Web.Areas.AccountingClosing
{
    public class AccountingClosingAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// AreaName
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "AccountingClosing";
            }
        }

        /// <summary>
        /// RegisterArea
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AccountingClosing_default",
                "AccountingClosing/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            var compileDebug = (CompilationSection)System.Configuration.ConfigurationManager.GetSection("system.web/compilation");

            if (compileDebug.Debug)
            {
                // PROCESOS MENSUALES
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/AccountingClosing").
                Include("~/Areas/AccountingClosing/Scripts/AccountingClosing/AccountingClosing.js"));

                // CIERRE DE EJERCICIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/Closing").
                Include("~/Areas/AccountingClosing/Scripts/AccountingClosing/Closing.js"));

                // DIFERENCIA CAMBIARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/ExchangeDifference").
                Include("~/Areas/AccountingClosing/Scripts/AccountingClosing/ExchangeDifference.js"));

                //REPORTE/EMISION/DETALLE DE PRODUCCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/ProductionDetailList").
                Include("~/Areas/AccountingClosing/Scripts/MassiveReports/ProductionDetailList.js"));

                //REPORTE/EMISION/REGISTRO DE CANCELACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/CancellationRecordIssuance").
                Include("~/Areas/AccountingClosing/Scripts/MassiveReports/CancellationRecordIssuance.js"));
            }
            else
            {
                // PROCESOS MENSUALES
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/AccountingClosing").
                Include("~/Areas/AccountingClosing/Scripts/AccountingClosing/AccountingClosing.es5.js"));

                // CIERRE DE EJERCICIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/Closing").
                Include("~/Areas/AccountingClosing/Scripts/AccountingClosing/Closing.es5.js"));

                // DIFERENCIA CAMBIARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/ExchangeDifference").
                Include("~/Areas/AccountingClosing/Scripts/AccountingClosing/ExchangeDifference.es5.js"));

                //REPORTE/EMISION/DETALLE DE PRODUCCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/ProductionDetailList").
                Include("~/Areas/AccountingClosing/Scripts/MassiveReports/ProductionDetailList.es5.js"));

                //REPORTE/EMISION/REGISTRO DE CANCELACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accountingClosing/bundles/CancellationRecordIssuance").
                Include("~/Areas/AccountingClosing/Scripts/MassiveReports/CancellationRecordIssuance.es5.js"));
            }
        }
    }
}