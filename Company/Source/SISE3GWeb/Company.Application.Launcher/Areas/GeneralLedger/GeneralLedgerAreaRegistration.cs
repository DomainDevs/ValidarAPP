using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Configuration;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger
{
    public class GeneralLedgerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GeneralLedger";
            }
        }

        /// <summary>
        /// RegisterArea
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GeneralLedger_default",
                "GeneralLedger/{controller}/{action}/{id}",
                new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            var compileDebug = (CompilationSection)System.Configuration.ConfigurationManager.GetSection("system.web/compilation");

            if (compileDebug.Debug)
            {

                // COMPAÑÍA
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AccountingCompany").
                Include("~/Areas/GeneralLedger/Scripts/AccountingCompany/AccountingCompany.js"));

                // PARAMETRIZACÓN/CENTRO DE COSTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/CostCenter").
                Include("~/Areas/GeneralLedger/Scripts/CostCenter/CostCenter.js"));

                // PARAMETRIZACÓN/TIPO DE CENTRO DE COSTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/CostCenterType").
                Include("~/Areas/GeneralLedger/Scripts/CostCenterType/CostCenterType.js"));

                // PARAMETRIZACÓN/DESTINO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Destination").
                Include("~/Areas/GeneralLedger/Scripts/Destination/Destination.js"));

                // PARAMETRIZACÓN/CÓDIGOS AUXILIARES
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AnalysisCode").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisCode/AnalysisCode.js"));

                // PARAMETRIZACÓN/TRATAMIENTO DE ANÁLISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AnalysisTreatment").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisTreatment/AnalysisTreatment.js"));

                // PARAMETRIZACÓN/CONCEPTO DE ANÁLISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AnalysisConcept").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisConcept/AnalysisConcept.js"));

                // PARAMETRIZACÓN/CONCEPTO DE ANÁLISIS POR CÓDIGO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AnalysisConceptByCode").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisConceptByCode/AnalysisConceptByCode.js"));

                // PARAMETRIZACÓN/CLAVE CONCEPTO DE ANÁLISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainAnalysisConceptKey").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisConceptKey/MainAnalysisConceptKey.js"));

                // PARAMETRIZACÓN/PLAN DE CUENTAS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AccountingAccount").
                Include("~/Areas/GeneralLedger/Scripts/AccountingAccount/AccountingAccount.js"));

                // PARAMETRIZACÓN/ASIENTO TIPO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/EntryType").
                Include("~/Areas/GeneralLedger/Scripts/EntryType/EntryType.js"));

                // PARAMETRIZACIÓN DINÁMICA/PARAMETROS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/EntryParameters").
                Include("~/Areas/GeneralLedger/Scripts/EntryParameter/EntryParameters.js"));

                // PARAMETRIZACIÓN DINÁMICA/PAQUETE DE REGLAS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AccountingRulePackage").
                Include("~/Areas/GeneralLedger/Scripts/EntryParameter/AccountingRulePackage.js"));

                // PARAMETRIZACIÓN DINÁMICA/CONDICION
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Condition").
                Include("~/Areas/GeneralLedger/Scripts/EntryParameter/Condition.js"));

                // PARAMETRIZACIÓN CUENTAS CONTABLES
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainAccountingConcept").
                Include("~/Areas/GeneralLedger/Scripts/AccountingConcept/MainAccountingConcept.js"));

                // PARAMETRIZACIÓN ORIGEN DE CONCEPTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainConceptSource").
                Include("~/Areas/GeneralLedger/Scripts/ConceptSource/MainConceptSource.js"));

                // PARAMETRIZACIÓN TIPO DE MOVIMIENTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainMovementType").
                Include("~/Areas/GeneralLedger/Scripts/MovementType/MovementType.js"));

                // PARAMETRIZACIÓN MainBranchAccountingConcept
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainBranchAccountingConcept").
                Include("~/Areas/GeneralLedger/Scripts/BranchAccountingConcept/BranchAccountingConcept.js"));

                // PARAMETRIZACIÓN/DUPLICAR CONCEPTOS DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/DuplicatePaymentConcept").
                Include("~/Areas/GeneralLedger/Scripts/AccountingConcept/DuplicatePaymentConcept.js"));

                // ASIENTO DIARIO/INGRESOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/JournalEntry").
                Include("~/Areas/GeneralLedger/Scripts/JournalEntry/JournalEntry.js"));

                // ASIENTO DIARIO/CONSULTA Y REVERSIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/JournalEntrySearch").
                Include("~/Areas/GeneralLedger/Scripts/JournalEntry/JournalEntrySearch.js"));

                // ASIENTOS DE MAYOR/INGRESOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Entry").
                Include("~/Areas/GeneralLedger/Scripts/Entry/Entry.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainLedgerEntry").
                Include("~/Areas/GeneralLedger/Scripts/LedgerEntry/MainLedgerEntry.js"));

                // ASIENTOS DE MAYOR/CONSULTA Y REVERSIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/EntryConsultation").
                Include("~/Areas/GeneralLedger/Scripts/Entry/EntryConsultation.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainLedgerEntrySearch").
                Include("~/Areas/GeneralLedger/Scripts/LedgerEntry/MainLedgerEntrySearch.js"));

                // ASIENTOS DE MAYOR/CARGA MASIVA
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/EntryMassiveLoad").
                Include("~/Areas/GeneralLedger/Scripts/Entry/EntryMassiveLoad.js"));
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MassiveLedgerEntry").
                Include("~/Areas/GeneralLedger/Scripts/LedgerEntry/MassiveLedgerEntry.js"));

                // REPORTES/DIARIO AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AuxiliaryDailyEntry").
                Include("~/Areas/GeneralLedger/Scripts/Reports/AuxiliaryDailyEntry.js"));

                // REPORTES/MAYOR AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AuxiliaryEntry").
                Include("~/Areas/GeneralLedger/Scripts/Reports/AuxiliaryEntry.js"));

                // REPORTES/MAYOR AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Balance").
                Include("~/Areas/GeneralLedger/Scripts/Reports/Balance.js"));

                // REPORTES/RESUMEN MAYOR AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AuxiliaryEntrySummary").
                Include("~/Areas/GeneralLedger/Scripts/Reports/AuxiliaryEntrySummary.js"));

                // MAYORIZACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Posting").
                Include("~/Areas/GeneralLedger/Scripts/Process/Posting.js"));

                // RECLASIFICACIÓN CUENTAS CONTABLES
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainAccountReclassification").
                Include("~/Areas/GeneralLedger/Scripts/AccountReclassification/MainAccountReclassification.js"));

                // GENERACIÓN RECLASIFICACIÓN DE CUENTAS CONTABLES
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainGenerationReclassification").
                Include("~/Areas/GeneralLedger/Scripts/AccountReclassification/MainGenerationReclassification.js"));

                //PROPAGAR BLOQUEO DE CUENTAS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AccountBlockadeSpreading").
                Include("~/Areas/GeneralLedger/Scripts/AccountingAccount/AccountBlockadeSpreading.js"));

                // PARAMETRIZACION TASA DE CAMBIO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/ExchangeRate").
                Include("~/Areas/GeneralLedger/Scripts/ExchangeRate/ExchangeRate.js"));

            }
            else
            {
                // COMPAÑÍA
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AccountingCompany").
                Include("~/Areas/GeneralLedger/Scripts/AccountingCompany/AccountingCompany.es5.js"));

                // PARAMETRIZACÓN/CENTRO DE COSTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/CostCenter").
                Include("~/Areas/GeneralLedger/Scripts/CostCenter/CostCenter.es5.js"));

                // PARAMETRIZACÓN/TIPO DE CENTRO DE COSTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/CostCenterType").
                Include("~/Areas/GeneralLedger/Scripts/CostCenterType/CostCenterType.es5.js"));

                // PARAMETRIZACÓN/DESTINO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Destination").
                Include("~/Areas/GeneralLedger/Scripts/Destination/Destination.es5.js"));

                // PARAMETRIZACÓN/CÓDIGOS AUXILIARES
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AnalysisCode").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisCode/AnalysisCode.es5.js"));

                // PARAMETRIZACÓN/TRATAMIENTO DE ANÁLISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AnalysisTreatment").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisTreatment/AnalysisTreatment.es5.js"));

                // PARAMETRIZACÓN/CONCEPTO DE ANÁLISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AnalysisConcept").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisConcept/AnalysisConcept.es5.js"));

                // PARAMETRIZACÓN/CONCEPTO DE ANÁLISIS POR CÓDIGO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AnalysisConceptByCode").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisConceptByCode/AnalysisConceptByCode.es5.js"));

                // PARAMETRIZACÓN/CLAVE CONCEPTO DE ANÁLISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainAnalysisConceptKey").
                Include("~/Areas/GeneralLedger/Scripts/AnalysisConceptKey/MainAnalysisConceptKey.es5.js"));

                // PARAMETRIZACÓN/PLAN DE CUENTAS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AccountingAccount").
                Include("~/Areas/GeneralLedger/Scripts/AccountingAccount/AccountingAccount.es5.js"));

                // PARAMETRIZACÓN/ASIENTO TIPO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/EntryType").
                Include("~/Areas/GeneralLedger/Scripts/EntryType/EntryType.es5.js"));

                // PARAMETRIZACIÓN DINÁMICA/PARAMETROS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/EntryParameters").
                Include("~/Areas/GeneralLedger/Scripts/EntryParameter/EntryParameters.es5.js"));

                // PARAMETRIZACIÓN DINÁMICA/PAQUETE DE REGLAS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AccountingRulePackage").
                Include("~/Areas/GeneralLedger/Scripts/EntryParameter/AccountingRulePackage.es5.js"));

                // PARAMETRIZACIÓN DINÁMICA/CONDICION
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Condition").
                Include("~/Areas/GeneralLedger/Scripts/EntryParameter/Condition.es5.js"));

                // PARAMETRIZACIÓN CUENTAS CONTABLES
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainAccountingConcept").
                Include("~/Areas/GeneralLedger/Scripts/AccountingConcept/MainAccountingConcept.es5.js"));

                // PARAMETRIZACIÓN ORIGEN DE CONCEPTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainConceptSource").
                Include("~/Areas/GeneralLedger/Scripts/ConceptSource/MainConceptSource.es5.js"));

                // PARAMETRIZACIÓN TIPO DE MOVIMIENTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainMovementType").
                Include("~/Areas/GeneralLedger/Scripts/MovementType/MovementType.es5.js"));

                // PARAMETRIZACIÓN MainBranchAccountingConcept
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainBranchAccountingConcept").
                Include("~/Areas/GeneralLedger/Scripts/BranchAccountingConcept/BranchAccountingConcept.es5.js"));

                // PARAMETRIZACIÓN/DUPLICAR CONCEPTOS DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/DuplicatePaymentConcept").
                Include("~/Areas/GeneralLedger/Scripts/AccountingConcept/DuplicatePaymentConcept.es5.js"));

                // ASIENTO DIARIO/INGRESOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/JournalEntry").
                Include("~/Areas/GeneralLedger/Scripts/JournalEntry/JournalEntry.es5.js"));

                // ASIENTO DIARIO/CONSULTA Y REVERSIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/JournalEntrySearch").
                Include("~/Areas/GeneralLedger/Scripts/JournalEntry/JournalEntrySearch.es5.js"));

                // ASIENTOS DE MAYOR/INGRESOS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Entry").
                Include("~/Areas/GeneralLedger/Scripts/Entry/Entry.es5.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainLedgerEntry").
                Include("~/Areas/GeneralLedger/Scripts/LedgerEntry/MainLedgerEntry.es5.js"));

                // ASIENTOS DE MAYOR/CONSULTA Y REVERSIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/EntryConsultation").
                Include("~/Areas/GeneralLedger/Scripts/Entry/EntryConsultation.es5.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainLedgerEntrySearch").
                Include("~/Areas/GeneralLedger/Scripts/LedgerEntry/MainLedgerEntrySearch.es5.js"));

                // ASIENTOS DE MAYOR/CARGA MASIVA
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/EntryMassiveLoad").
                Include("~/Areas/GeneralLedger/Scripts/Entry/EntryMassiveLoad.es5.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MassiveLedgerEntry").
                Include("~/Areas/GeneralLedger/Scripts/LedgerEntry/MassiveLedgerEntry.es5.js"));

                // REPORTES/DIARIO AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AuxiliaryDailyEntry").
                Include("~/Areas/GeneralLedger/Scripts/Reports/AuxiliaryDailyEntry.es5.js"));

                // REPORTES/MAYOR AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AuxiliaryEntry").
                Include("~/Areas/GeneralLedger/Scripts/Reports/AuxiliaryEntry.es5.js"));

                // REPORTES/MAYOR AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Balance").
                Include("~/Areas/GeneralLedger/Scripts/Reports/Balance.es5.js"));

                // REPORTES/RESUMEN MAYOR AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AuxiliaryEntrySummary").
                Include("~/Areas/GeneralLedger/Scripts/Reports/AuxiliaryEntrySummary.es5.js"));

                // MAYORIZACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/Posting").
                Include("~/Areas/GeneralLedger/Scripts/Process/Posting.es5.js"));

                // RECLASIFICACIÓN CUENTAS CONTABLES
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainAccountReclassification").
                Include("~/Areas/GeneralLedger/Scripts/AccountReclassification/MainAccountReclassification.es5.js"));

                // GENERACIÓN RECLASIFICACIÓN DE CUENTAS CONTABLES
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/MainGenerationReclassification").
                Include("~/Areas/GeneralLedger/Scripts/AccountReclassification/MainGenerationReclassification.es5.js"));

                //PROPAGAR BLOQUEO DE CUENTAS
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/AccountBlockadeSpreading").
                Include("~/Areas/GeneralLedger/Scripts/AccountingAccount/AccountBlockadeSpreading.es5.js"));

                // PARAMETRIZACION TASA DE CAMBIO
                BundleTable.Bundles.Add(new ScriptBundle("~/generalLedger/bundles/ExchangeRate").
                Include("~/Areas/GeneralLedger/Scripts/ExchangeRate/ExchangeRate.es5.js"));

            }

        }
    }
}