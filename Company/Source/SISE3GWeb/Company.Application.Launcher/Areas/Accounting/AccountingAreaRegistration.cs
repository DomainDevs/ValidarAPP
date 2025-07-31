using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Configuration;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting
{
    public class AccountingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Accounting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Accounting_default",
                "Accounting/{controller}/{action}/{id}",
                new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            var compileDebug = (CompilationSection)System.Configuration.ConfigurationManager.GetSection("system.web/compilation");

            if (compileDebug.Debug)
            {
                // BOLETAS/REGULARIZACIÓN DE TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ApplicationRegularization").
                Include("~/Areas/Accounting/Scripts/Regularization/MainCardVoucherRegularization.js"
                    , "~/Areas/Accounting/Scripts/Regularization/MainCheckRegularization.js"));

                /*Separacion 18/08/2017*/
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/mainBilling").
                Include(
                 "~/Areas/Accounting/Scripts/Billing/MainBilling.js"
                , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.js"
                , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.js"
                , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRedux.js"
                , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRedux.js"
                , "~/Areas/Accounting/Scripts/Billing/MainBillingRedux.js"
                , "~/Areas/Accounting/Scripts/Billing/MainBillingRequest.js"
                ));

                // CAJA/CIERRE DE CAJA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyCashClosing").
                Include("~/Areas/Accounting/Scripts/Billing/DailyCashClosingRequest.js"
                , "~/Areas/Accounting/Scripts/Billing/DailyCashClosing.js"));

                // CAJA/CANCELACION INGRESOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CancelAppliationReceipt").
                Include("~/Areas/Accounting/Scripts/Billing/CancelAppliationReceipt.js"));

                // CAJA/BÚSQUEDA DE RECIBOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBillSearch").
                Include("~/Areas/Accounting/Scripts/BillSearch/MainBillSearch.js"));

                // CAJA/CONSULTA DE MOVIMIENTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BalanceInquiries").
                Include("~/Areas/Accounting/Scripts/Transaction/BalanceInquiries.js"));

                // CAJA/CONSULTA DE PAGOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPaymentStatus").
                Include("~/Areas/Accounting/Scripts/PaymentStatus/MainPaymentStatus.js"));

                // CAJA/GENERAR CARGA MASIVA EXCEL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainMassiveDataGenerate").
                Include("~/Areas/Accounting/Scripts/MassiveDataLoad/MainMassiveDataGenerate.js"));

                // CAJA/CARGA MASIVA  
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainMassiveDataLoad").
                Include("~/Areas/Accounting/Scripts/MassiveDataLoad/MainMassiveDataLoad.js"));

                // CAJA/BOTÓN DE PAGOS ASEGURADO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPaymentService").
                Include("~/Areas/Accounting/Scripts/PaymentService/MainPaymentService.js"));

                // CAJA/RECUOTIFICACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/FinancialPlan").
                Include("~/Areas/Accounting/Scripts/FinancialPlan/FinancialPlan.js",
                "~/Areas/Accounting/Scripts/FinancialPlan/FinancialPlanRequest.js"));


                // BOLETAS/BOLETA INTERNA CHEQUES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckInternalDepositSlip").
                Include("~/Areas/Accounting/Scripts/CheckInternalDepositSlip/MainCheckInternalDepositSlipRequest.js",
                 "~/Areas/Accounting/Scripts/CheckInternalDepositSlip/MainCheckInternalDepositSlip.js"));

                // BOLETAS/BOLETA DE DEPÓSITO CHEQUES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckDepositSlip").
                Include("~/Areas/Accounting/Scripts/Transaction/MainCheckDepositSlipRequest.js",
                "~/Areas/Accounting/Scripts/Transaction/MainCheckDepositSlip.js"));

                // BOLETAS/BÚSQUEDA DE BOLETA DE DEPÓSITO 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DepositSlipSearch").
                Include("~/Areas/Accounting/Scripts/DepositSlipSearch/DepositSlipSearch.js"));

                // BOLETAS/BÚSQUEDA DE CHEQUES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckSearch").
                Include("~/Areas/Accounting/Scripts/Transaction/MainCheckSearch.js"));

                // BOLETAS/CHEQUES PENDIENTES DE DEPÓSITO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckPendingDeposit").
                Include("~/Areas/Accounting/Scripts/Transaction/MainCheckPendingDeposit.js"));

                // BOLETA INTERNA DE TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardInternalDepositSlip").
                Include("~/Areas/Accounting/Scripts/CardInternalDepositSlip/MainCardInternalDepositSlip.js"));

                // BOLETAS/BOLETA DE DEPÓSITO TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardDepositSlip").
                Include("~/Areas/Accounting/Scripts/Transaction/MainCardDepositSlip.js"));

                // BOLETAS/BÚSQUEDA BOLETA INTERNA DE TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardDepositSlipSearch").
                Include("~/Areas/Accounting/Scripts/CardDepositSlipSearch/MainCardDepositSlipSearch.js"));

                // BOLETAS/CONSULTA DE TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardVoucherSearch").
                Include("~/Areas/Accounting/Scripts/CardVoucherSearch/MainCardVoucherSearch.js"));

                // BOLETAS/TARJETAS PENDIENTES DEPÓSITO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardPendingDeposit").
                Include("~/Areas/Accounting/Scripts/CardVoucherSearch/MainCardPendingDeposit.js"));

                // CAJA EGRESOS/BÚSQUEDA ÓRDENES DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrderSearch").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/PaymentOrders/PaymentOrderSearch.js"));

                // CAJA EGRESOS/ ÓRDENES DE PAGO / AUTORIZACIÓN ÓRDENES DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrderAuthorization").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/PaymentOrders/PaymentOrderAuthorization.js"));

                // CAJA EGRESOS/SOLICITUD DE PAGOS VARIOS           
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPaymentRequest").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/PaymentRequest/MainPaymentRequest.js"));

                // PRELIQUIDACIÓN/BÚSQUEDA DE PRELIQUIDACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPreLiquidationsSearch").
                Include("~/Areas/Accounting/Scripts/PreLiquidations/MainPreLiquidationsSearch.js"));

                // CAJA EGRESOS/PARAMETRO/CUENTA BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankAccount").
                Include("~/Areas/Accounting/Scripts/Parameters/BankAccount/MainBankAccount.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCompanyBankAccount").
                Include("~/Areas/Accounting/Scripts/Parameters/CompanyBankAccount/MainCompanyBankAccount.js"));

                // CAJA EGRESOS/PARAMETRO/CONTROL DE CHEQUERAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckBookControl").
                Include("~/Areas/Accounting/Scripts/Parameters/CheckBookControl/MainCheckBookControl.js",
                        "~/Areas/Accounting/Scripts/Parameters/CheckBookControl/MainCheckBookControlRequest.js"));

                // CAJA EGRESOS/PARAMETRO/ASOCIAR BANCO A SUCURSAL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainAssociateBankBranch").
                Include("~/Areas/Accounting/Scripts/Parameters/AssociateBankBranch/MainAssociateBankBranch.js"));

                // ASIENTO DE DIARIO/NOTAS DE CRÉDITO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCreditNotes").
                Include("~/Areas/Accounting/Scripts/JournalEntry/MainCreditNotes.js"));

                // ASIENTO DE DIARIO/BÚSQUEDA ASIENTO DE DIARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainJournalEntryBillSearch").
                Include("~/Areas/Accounting/Scripts/JournalEntrySearch/MainJournalEntryBillSearch.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ASIGNACIÓN AUTOMATICA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckAutomaticAssignment").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckAutomaticAssignment.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ASIGNACIÓN MANUAL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckManualAssignment").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckManualAssignment.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/IMPRESIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckPrinting").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckPrinting.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/REIMPRESIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckReprinting").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckReprinting.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ENTREGA DE CHEQUES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckDelivery").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckDelivery.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ANULACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckNullification").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckNullification.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/GENERACIÓN/TRANSFERENCIA BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainTransfer").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/Transfer/MainTransfer.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ANULACIÓN/TRANSFERENCIA BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainTransferNullification").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/Transfer/MainTransferNullification.js"));

                // CUENTA CORRIENTE/BALANCE DE COMISIONES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCommissionBalance").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Agent/MainCommissionBalance.js"));

                // CUENTA CORRIENTE/ÓRDENES DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCommissionPaymentOrder").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Agent/MainCommissionPaymentOrder.js"));

                // CUENTA CORRIENTE/CIERRE PARCIAL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPartialClosure").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Agent/MainPartialClosure.js"));

                // CUENTA CORRIENTE/GENERACIÓN DEL CIERRE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainClosureGeneration").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Coinsurance/MainClosureGeneration.js"));

                // CUENTA CORRIENTE/REPORTE CIERRES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainClosureReport").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Coinsurance/MainClosureReport.js"));

                // DÉBITO AUTOMÁTICO/ASIGNACIÓN FORMATO A RED
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainAutomaticDebitFormat").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainAutomaticDebitFormat.js"));

                // DÉBITO AUTOMÁTICO/GENERACIÓN DE CUPONES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainGeneratingCoupons").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainGeneratingCoupons.js"));

                // DÉBITO AUTOMÁTICO/ADMINISTRACIÓN DE ENVIOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainSendingAdministration").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainSendingAdministration.js"));

                // DÉBITO AUTOMÁTICO/CARGAR RESPUESTA DEL BANCO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainLoadBankResponse").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainLoadBankResponse.js"));

                // DÉBITO AUTOMÁTICO/ESTADO DE CUPONES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainReportCouponsStatus").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainReportCouponsStatus.js"));

                // DÉBITO AUTOMÁTICO/CUPONES RECAUDADOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainReportCollectedCoupons").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainReportCollectedCoupons.js"));

                // CANCELACIÓN AUTOMÁTICA DE PÓLIZAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPolicyAutomaticCancellation").
                Include("~/Areas/Accounting/Scripts/PolicyCancellation/PolicyAutomaticCancellation.js"));


                // PARÁMETROS/CAJA/CONCEPTO DE INGRESO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainIncomeConcept").
                Include("~/Areas/Accounting/Scripts/Parameters/IncomeConcept/MainIncomeConcept.js"));

                // PARÁMETROS/CAJA/COMPAÑÍA CONTABLE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountingCompany").
                Include("~/Areas/Accounting/Scripts/Parameters/AccountingCompany/AccountingCompany.js"));

                // PARÁMETROS/CAJA/CUENTAS BANCARIAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BankAccountsSettings").
                Include("~/Areas/Accounting/Scripts/Parameters/BankAccountsSettings.js"));

                // PARÁMETROS/CAJA/DIFERENCIA DE MONEDA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CurrencyDifference").
                Include("~/Areas/Accounting/Scripts/Parameters/CurrencyDifference.js"));

                // PARÁMETROS/CAJA/TIPO DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPaymentType").
                Include("~/Areas/Accounting/Scripts/Parameters/PaymentType/MainPaymentType.js"));

                // PARÁMETROS/RETENCIONES PERCIBIDAS/CONCEPTO DE RETENCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainRetentionConcept").
                Include("~/Areas/Accounting/Scripts/Parameters/Retention/MainRetentionConcept.js"));

                // PARÁMETROS/RETENCIONES PERCIBIDAS/BASE DE RETENCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/RetentionBase").
                Include("~/Areas/Accounting/Scripts/Parameters/Retention/RetentionBase.js"));

                // PARÁMETROS/FORMATOS DE SALIDA/AGRUPADOR DE FORMATOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TemplateGroup").
                Include("~/Areas/Accounting/Scripts/Parameters/OutputTemplate/TemplateGroup.js"));

                // PARÁMETROS/FORMATOS DE SALIDA/FORMATO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TemplateFormat").
                Include("~/Areas/Accounting/Scripts/Parameters/OutputTemplate/TemplateFormat.js"));

                // PARÁMETROS/FORMATOS DE SALIDA/DISEÑO DE FORMATO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TemplateParametrization").
                Include("~/Areas/Accounting/Scripts/Parameters/OutputTemplate/TemplateParametrization.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO/REDES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainNetwork").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainNetwork.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO /CÓDIGOS DE ESTADO DÉBITO           
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainDebitStatusCodes").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainDebitStatusCodes.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO/CÓDIGOS DE RESPUESTA BANCO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankResponseCodes").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainBankResponseCodes.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO/RELACIÓN REDES POR CONDUCTO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankNetworkRelationship").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainBankNetworkRelationship.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO/RELACIÓN CONDUCTO TIPO DE CUENTA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainRelationshipAccountType").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainRelationshipAccountType.js"));

                // PARÁMETROS/REPORTE DE CARTERA//RANGOS DE DEUDAS PENDIENTES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/RangeParametrizationDetail").
                Include("~/Areas/Accounting/Scripts/Parameters/Range/RangeParametrizationDetail.js"));

                // TEMPORALES/BÚSQUEDA DE TEMPORALES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainTemporarySearch").
                Include("~/Areas/Accounting/Scripts/TemporarySearch/MainTemporarySearch.js"));

                // REPORTS/CUENTAS/CONSULTA DE CUENTAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountingAccount").
                Include("~/Areas/Accounting/Scripts/Reports/AccountingAccount.js"));

                // REPORTS/CONTABILIDAD/REPORTE DE CUENTAS CONTABLES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountingAccountReport").
                Include("~/Areas/Accounting/Scripts/Reports/AccountingAccountReport.js"));

                // REPORTS/CONTABILIDAD/INVENTARIO DE CUENTAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountInventory").
                Include("~/Areas/Accounting/Scripts/Reports/AccountInventory.js"));

                // REPORTS/CUENTAS/CONSULTA DE CUENTAS EN ASIENTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountOnEntry").
                Include("~/Areas/Accounting/Scripts/Reports/AccountOnEntry.js"));

                // REPORTS/CUENTAS/CONSULTA DE ANALISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Analysis").
                Include("~/Areas/Accounting/Scripts/Reports/Analysis.js"));

                // REPORTS/CUENTAS/CONSULTA DE ASIENTOS AUTOMÁTICOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AutomaticEntry").
                Include("~/Areas/Accounting/Scripts/Reports/AutomaticEntry.js"));

                // REPORTS/CONTABILIDAD/LIBRO AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AuxiliaryLedger").
                Include("~/Areas/Accounting/Scripts/Reports/AuxiliaryLedger.js"));

                // REPORTS/CONTABILIDAD/BALANCE GENERAL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BalanceSheet").
                Include("~/Areas/Accounting/Scripts/Reports/BalanceSheet.js"));

                // REPORTS/FINANCIERA/LIBRO DE BANCOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BankLedger").
                Include("~/Areas/Accounting/Scripts/Reports/BankLedger.js"));

                // REPORTS/CONTABILIDAD/MAYOR AUXILIAR DE CAJA Y BANCOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BillingBankAuxiliary").
                Include("~/Areas/Accounting/Scripts/Reports/BillingBankAuxiliary.js"));

                // REPORTS/CONTABILIDAD/RESUMEN MAYOR AUXILIAR DE CAJA Y BANCOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BillingBankAuxiliarySummary").
                Include("~/Areas/Accounting/Scripts/Reports/BillingBankAuxiliarySummary.js"));

                // REPORTS/CONTABILIDAD/DIARIO AUXILIAR DE CAJA Y BANCOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BillingBankDailyAuxiliary").
                Include("~/Areas/Accounting/Scripts/Reports/BillingBankDailyAuxiliary.js"));

                // REPORTS/CAJA INGRESOS/LISTADOS DE MOVIMIENTOS EN CAJA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BillingMovementList").
                Include("~/Areas/Accounting/Scripts/Reports/BillingMovementList.js"));

                // REPORTS/REASEGUROS/BORDER AUX
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Borderaux").
                Include("~/Areas/Accounting/Scripts/Reports/Borderaux.js"));

                // REPORTS/OPERACIONES/CARTERA VENCIDA POR CORREDOR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BrokerExpiredPortfolioList").
                Include("~/Areas/Accounting/Scripts/Reports/BrokerExpiredPortfolioList.js"));

                // REPORTS/OPERACIONES/CARTERA POR CORREDOR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PortfolioByBrokerList").
                Include("~/Areas/Accounting/Scripts/Reports/PortfolioByBrokerList.js"));

                // REPORTS/EMISION/REGISTRO DE CANCELACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CancellationRecordIssuance").
                Include("~/Areas/Accounting/Scripts/Reports/CancellationRecordIssuance.js"));

                // REPORTS/CONTABLES/FLUJO DE EFECTIVO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CashFlow").
                Include("~/Areas/Accounting/Scripts/Reports/CashFlow.js"));

                // REPORTS/RESERVA DE RIESGOS CATASTROFICOS/RESERVA CATASTRÓFICA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CatastrophicReserve").
                Include("~/Areas/Accounting/Scripts/Reports/CatastrophicReserve.js"));

                // REPORTS/RIESGOS CATASTROFICOS/ASIENTO DE RESERVA CATASTRÓFICA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CatastrophicReserveEntry").
                Include("~/Areas/Accounting/Scripts/Reports/CatastrophicReserveEntry.js"));

                // REPORTS/REASEGUROS/CUENTA CORRIENTE POR TRATADO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckingAccountByTreaty").
                Include("~/Areas/Accounting/Scripts/Reports/CheckingAccountByTreaty.js"));

                // REPORTS/REASEGUROS/CUENTA CORRIENTE PRIMA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckingAccountPrime").
                Include("~/Areas/Accounting/Scripts/Reports/CheckingAccountPrime.js"));

                // REPORTS/REASEGUROS/CUENTA CORRIENTE PRIMA VS DETALLE PRODUCCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckingAccountPrimeProductionDetail").
                Include("~/Areas/Accounting/Scripts/Reports/CheckingAccountPrimeProductionDetail.js"));

                // REPORTS//RECLAMOS/REGISTRO DE RESERVAS SINIESTRO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ClaimsReserveRecord").
                Include("~/Areas/Accounting/Scripts/Reports/ClaimsReserveRecord.js"));

                // REPORTS/REPORTES DE/IMPUESTOS POR CLIENTE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ClientTax").
                Include("~/Areas/Accounting/Scripts/Reports/ClientTax.js"));

                // REPORTS/REPORTES/CONTROL DE OPERACIONES DE CIERRE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ClosingOperationsControl").
                Include("~/Areas/Accounting/Scripts/Reports/ClosingOperationsControl.js"));

                // REPORTS/REPORTES/REVISIÓN OPERACIONES DE CIERRE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ClosingOperationsReview").
                Include("~/Areas/Accounting/Scripts/Reports/ClosingOperationsReview.js"));

                // REPORTS/COBRANZAS/REGISTRO DE COBRANZAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CollectingRecord").
                Include("~/Areas/Accounting/Scripts/Reports/CollectingRecord.js"));

                // REPORTS/CONTABLES/COMPARATIVO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ComparativeBalanceSheet").
                Include("~/Areas/Accounting/Scripts/Reports/ComparativeBalanceSheet.js"));

                // REPORTS/CUENTA CONTABLE/BALANCE CONDENSADO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CondensedBalanceSheet").
                Include("~/Areas/Accounting/Scripts/Reports/CondensedBalanceSheet.js"));

                // REPORTS/RESERVAS DE PREVISIÓN/DETALLE RESERVA DE PREVISIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ContingencyReserve").
                Include("~/Areas/Accounting/Scripts/Reports/ContingencyReserve.js"));

                // REPORTS/CUENTAS/CENTRO DE COSTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CostCenter").
                Include("~/Areas/Accounting/Scripts/Reports/CostCenter.js"));

                // REPORTS/CUENTAS/CUADRO DE RESULTADOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ResultBoard").
                Include("~/Areas/Accounting/Scripts/Reports/ResultBoard.js"));

                // REPORTS/SUMAS ASEGURADAS VIGENTES/ASIENTO PRELIMINAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CurrentInsuredAmountPreliminaryEntry").
                Include("~/Areas/Accounting/Scripts/Reports/CurrentInsuredAmountPreliminaryEntry.js"));

                // REPORTS/SUMAS ASEGURADAS VIGENTES/CUADRO TECNICO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CurrentInsuredAmountsTechnicalChart").
                Include("~/Areas/Accounting/Scripts/Reports/CurrentInsuredAmountsTechnicalChart.js"));

                // REPORTS/OPERACIONES/PÓLIZAS VIGENTES 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CurrentPoliciesList").
                Include("~/Areas/Accounting/Scripts/Reports/CurrentPoliciesList.js"));

                // REPORTS/CAJA INGRESOS/CIERRE DE CAJA DIARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyCashClosingList").
                Include("~/Areas/Accounting/Scripts/Reports/DailyCashClosingList.js"));

                // REPORTS/CAJA EGRESOS/LISTADO DE CIERRES DE CAJA 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyClosingList").
                Include("~/Areas/Accounting/Scripts/Reports/DailyClosingList.js"));

                // REPORTS/CAJA INGRESOS/DETALLE DIARIO DE INGRESOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyIncomeDetail").
                Include("~/Areas/Accounting/Scripts/Reports/DailyIncomeDetail.js"));

                // REPORTS/CAJA EGRESOS/DETALLE DIARIO DE EGRESOS  
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyOutcomeDetails").
                Include("~/Areas/Accounting/Scripts/Reports/DailyOutcomeDetails.js"));

                // REPORTS/CAJA EGRESOS/RESUMEN DIARIO DE EGRESOS  
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyOutcomeSummary").
                Include("~/Areas/Accounting/Scripts/Reports/DailyOutcomeSummary.js"));

                // REPORTS/CAJA INGRESOS/RESUMEN DIARIO DE INGRESOS 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailySummaryIncome").
                Include("~/Areas/Accounting/Scripts/Reports/DailySummaryIncome.js"));

                // REPORTS/REPORTES DE/COMISIONES DE EVALUACION NO DEVENGADAS   
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/EvaluationCommissionsUnearned").
                Include("~/Areas/Accounting/Scripts/Reports/EvaluationCommissionsUnearned.js"));

                // REPORTS/PRIMAS VENCIDAS/ASIENTO PRELIMINAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ExpiredPrimesPreliminaryEntry").
                Include("~/Areas/Accounting/Scripts/Reports/ExpiredPrimesPreliminaryEntry.js"));

                // REPORTS/PRIMAS VENCIDAS/CUADRO TÉCNICO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ExpiredPrimeTechnicalChart").
                Include("~/Areas/Accounting/Scripts/Reports/ExpiredPrimeTechnicalChart.js"));

                // REPORTS/CONTABLES/COMPARACIÓN DE SALDOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/FinalBalance").
                Include("~/Areas/Accounting/Scripts/Reports/FinalBalance.js"));

                // REPORTS/REASEGUROS/ESTADO DE CUENTA-FLUCTUACIONES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/FluctuationAccountStatement").
                Include("~/Areas/Accounting/Scripts/Reports/FluctuationAccountStatement.js"));


                // REPORTS/RECLAMOS/REGISTRO DE FLUCTUACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/FluctuationRecord").
                Include("~/Areas/Accounting/Scripts/Reports/FluctuationRecord.js"));

                // REPORTS/CONTABLES/LIBRO DIARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/GeneralJournal").
                Include("~/Areas/Accounting/Scripts/Reports/GeneralJournal.js"));

                // REPORTS/CONTABLES/LIBRO MAYOR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/GeneralLedger").
                Include("~/Areas/Accounting/Scripts/Reports/GeneralLedger.js"));

                // REPORTS/RESERVAS DE IBNR/DETALLE DE RESERVA DE IBNR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IBNRReserve").
                Include("~/Areas/Accounting/Scripts/Reports/IBNRReserve.js"));

                // REPORTS/IBNR/ASIENTO RESERVA IBNR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IBNRReserveEntry").
                Include("~/Areas/Accounting/Scripts/Reports/IBNRReserveEntry.js"));

                // REPORTS/CIERRES/BALANCE DE SALDOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IncomeStatement").
                Include("~/Areas/Accounting/Scripts/Reports/IncomeStatement.js"));

                // REPORTS/REPORTES DE/IMPUESTOS POR INTERMEDIARIO 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IntermediaryTax").
                Include("~/Areas/Accounting/Scripts/Reports/IntermediaryTax.js"));

                // REPORTS/CAJA EGRESOS/CHEQUES EMITIDOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IssuedChecks").
                Include("~/Areas/Accounting/Scripts/Reports/IssuedChecks.js"));

                // REPORTS/REASEGUROS/PARTIDA DE PÉRDIDAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/LossItem").
                Include("~/Areas/Accounting/Scripts/Reports/LossItem.js"));

                // REPORTS/REPORTES/CONSULTA REPORTES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainViewReport").
                Include("~/Areas/Accounting/Scripts/Reports/MainViewReport.js"));

                // REPORTS/REASEGUROS/DISTRIBUCIÓN DEL MES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MonthDistribution").
                Include("~/Areas/Accounting/Scripts/Reports/MonthDistribution.js"));

                // REPORTS/REPORTES DE CARTERA/BALANCE DE ANTIGUEDAD
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/OldBalanceList").
                Include("~/Areas/Accounting/Scripts/Reports/OldBalanceList.js"));

                // REPORTS/OPERACIONES/EMITIDAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/OperationsIssuedList").
                Include("~/Areas/Accounting/Scripts/Reports/OperationsIssuedList.js"));

                // REPORTS/ÓRDENES DE PAGO/LIQUIDADAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrdersPaying").
                Include("~/Areas/Accounting/Scripts/Reports/PaymentOrdersPaying.js"));

                // REPORTS/ÓRDENES DE PAGO/PENDIENTES            
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrders").
                Include("~/Areas/Accounting/Scripts/Reports/PaymentOrders.js"));

                // REPORTS/ÓRDENES DE PAGO/ELIMINADAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrdersRemoved").
                Include("~/Areas/Accounting/Scripts/Reports/PaymentOrdersRemoved.js"));

                // REPORTS/RECIBOS/PENDIENTES DE APLICACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PendingApplicationList").
                Include("~/Areas/Accounting/Scripts/Reports/PendingApplicationList.js"));

                // REPORTS/REPORTES/RETENCIONES PERCIBIDAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PerceivedRetentions").
                Include("~/Areas/Accounting/Scripts/Reports/PerceivedRetentions.js"));

                // REPORTS/OPERACIONES/CARTERA POR CORREDOR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PortfolioByBrokerList").
                Include("~/Areas/Accounting/Scripts/Reports/PortfolioByBrokerList.js"));

                // REPORTS/REPORTES DE CARTERA/CARTERA RECAUDADA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PortfolioCollectedList").
                Include("~/Areas/Accounting/Scripts/Reports/PortfolioCollectedList.js"));

                // REPORTS/PRIMAS VENCIDAS/ASIENTO PRELIMINAR PROVISIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PreliminaryProvisionForPrimesEntry").
                Include("~/Areas/Accounting/Scripts/Reports/PreliminaryProvisionForPrimesEntry.js"));

                // REPORTS/RESERVAS TECNICAS/PRIMAS POR COBRAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PremiumReceivable").
                Include("~/Areas/Accounting/Scripts/Reports/PremiumReceivable.js"));

                // REPORTS/REPORTES DE CARTERA/ANTIGUEDAD DE PRIMAS POR COBRAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PremiumReceivableAntiquityList").
                Include("~/Areas/Accounting/Scripts/Reports/PremiumReceivableAntiquityList.js"));

                // REPORTS/REPORTES DE CARTERA/DEUDORES POR PRIMAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PrimeDebtorList").
                Include("~/Areas/Accounting/Scripts/Reports/PrimeDebtorList.js"));

                // REPORTS/EMISION/DETALLE DE PRODUCCIÓN    
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ProductionDetailList").
                Include("~/Areas/Accounting/Scripts/Reports/ProductionDetailList.js"));

                // REPORTS/CONTABLES/PÉRDIDAS Y GANANCIAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ProfitsAndLosses").
                Include("~/Areas/Accounting/Scripts/Reports/ProfitsAndLosses.js"));

                // REPORTS/REPORTES DE/IMPUESTOS POR PROVEEDORES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ProviderTax").
                Include("~/Areas/Accounting/Scripts/Reports/ProviderTax.js"));

                // REPORTS/RECIBOS/APLICADOS EN EL DIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ReceiptsAppliedList").
                Include("~/Areas/Accounting/Scripts/Reports/ReceiptsAppliedList.js"));

                // REPORTS/REASEGUROS/CUENTA CORRIENTE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ReinsuranceCheckingAccount").
                Include("~/Areas/Accounting/Scripts/Reports/ReinsuranceCheckingAccount.js"));

                // REPORTS/CAJA INGRESOS/SOLICITUDES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Requests").
                Include("~/Areas/Accounting/Scripts/Reports/Requests.js"));

                // REPORTS/RESERVAS DE PREVISIÓN/ASIENTO RESERVA DE PREVISIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ReserveEstimateEntry").
                Include("~/Areas/Accounting/Scripts/Reports/ReserveEstimateEntry.js"));

                // REPORTS/CUENTAS/CUADRO DE RESULTADOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ResultBoard").
                Include("~/Areas/Accounting/Scripts/Reports/ResultBoard.js"));

                // REPORTS/REPORTES/RETENCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Retention").
                Include("~/Areas/Accounting/Scripts/Reports/Retention.js"));

                // REPORTS/RESERVAS TÉCNICAS/DETALLE RESERVAS DE RIESGOS EN CURSO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/RiskReserve").
                Include("~/Areas/Accounting/Scripts/Reports/RiskReserve.js"));

                // REPORTS/RIESGOS EN CURSO/ASIENTOS DE RIESGOS EN CURSO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/RiskReserveEntry").
                Include("~/Areas/Accounting/Scripts/Reports/RiskReserveEntry.js"));

                // REPORTS/CONTABLES/BALANCE DE COMPROBACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TrialBalance").
                Include("~/Areas/Accounting/Scripts/Reports/TrialBalance.js"));

                // REPORTS/CHEQUES ESCRITOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/WrittenCheck").
                Include("~/Areas/Accounting/Scripts/Reports/WrittenCheck.js"));

                // PROCESOS ESPECIALES/CRUCE AUTOMÁTICO DE NOTAS DE CRÉDITO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainAutomaticCreditNotes").
                Include("~/Areas/Accounting/Scripts/AutomaticCreditNotes/MainAutomaticCreditNotes.js",
                "~/Areas/Accounting/Scripts/AutomaticCreditNotes/AutomaticCreditNotesRequest.js"));

                // PROCESOS ESPECIALES//AMORTIZACIÓN AUTOMÁTICA (no implementado en EE)
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainAutomaticAmortization").
                Include("~/Areas/Accounting/Scripts/AutomaticAmortization/MainAutomaticAmortization.js"));

                // CONCILIACIÓN BANCARIA/FORMATO DE EXTRACTO BANCARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankFileFormat").
                Include("~/Areas/Accounting/Scripts/Parameters/BankReconciliation/MainBankFileFormat.js"));

                // CONCILIACIÓN BANCARIA/EXTRACTO BANCARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankStatement").
                Include("~/Areas/Accounting/Scripts/BankStatement/MainBankStatement.js"));

                // CONCILIACIÓN BANCARIA/ASIGNACIÓN FORMATO CONCILIACIÓN A CUENTA BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainReconciliationFormat").
                Include("~/Areas/Accounting/Scripts/BankStatement/MainReconciliationFormat.js"));

                // CONCILIACIÓN BANCARIA/CONCILIACIÓN BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankReconciliation").
                Include("~/Areas/Accounting/Scripts/BankReconciliation/MainBankReconciliation.js"));

                // CONCILIACIÓN BANCARIA/REVERSO CONCILIACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainReverseReconciliation").
                Include("~/Areas/Accounting/Scripts/BankReconciliation/MainReverseReconciliation.js"));

                // CONCILIACIÓN BANCARIA/PARÁMETROS/MOVIMIENTOS BANCARIOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankMovement").
                Include("~/Areas/Accounting/Scripts/BankMovement/MainBankMovement.js"));

                // CONCILIACIÓN BANCARIA/PARÁMETROS/EQUIVALENCIAS ENTRE CÓDIGOS BANCARIOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankCodeEquivalence").
                Include("~/Areas/Accounting/Scripts/BankMovement/BankCodeEquivalence.js"));

                // PARAMETROS/DÍAS POR RAMO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainDayPrefix").
                Include("~/Areas/Accounting/Scripts/Parameters/CancellationPolicy/MainDayPrefix.js"));

                // PARAMETROS/EXCLUSION DE ASEGURADO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainExclusion").
                Include("~/Areas/Accounting/Scripts/Parameters/CancellationPolicy/MainExclusion.js"));

                // REPORTS/CONTABILIDAD/ANALYSIS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Analysis").
                Include("~/Areas/Accounting/Scripts/Reports/Analysis.js"));

                // REPORTS/CUENTAS/CONSULTA DE ANALISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TaxRetentionList").
                Include("~/Areas/Accounting/Scripts/Reports/TaxRetentionList.js"));

                var jsappBundle = new ScriptBundle("~/accounting/bundles/ApplicationReceipt");
                BundleTable.Bundles.Add(jsappBundle.Include("~/Areas/Accounting/Scripts/Application/ReceiptApplication/MainApplicationReceipt.js"  //APLICACIÓN DE RECIBOS

                       , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovements.js"                                          //AGENTE
                                                                                                                                                    //, "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPolicies.js"                                        //PRIMAS POR COBRAR
                       , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRedux.js"                                        //PRIMAS POR COBRAR
                       , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovements.js"                               //COASEGUROS
                       , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovements.js"                               //REASEGUROS
                                                                                                                                                    //, "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRedux.js"                               //REASEGUROS
                                                                                                                                                    //, "~/Areas/Accounting/Scripts/Application/Accounting/Accounting.js"                                                          //CONTABILIDAD
                       , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRedux.js"                                                          //CONTABILIDAD

                       , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovements.js"                                  //SOLICITUD PAGO VARIOS
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovements.js"                                    //SOLICITUD PAGO DE SINIESTROS
                       , "~/Areas/Accounting/Scripts/Application/InsuredLoans/InsuredLoans.js"                                                      //PRÉSTAMOS ASEGURADO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovementsRequest.js"
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovementsRequest.js"
                       , "~/Areas/Accounting/Scripts/Application/ReceiptApplication/MainApplicationReceiptRequest.js"
                       , "~/Areas/Accounting/Scripts/Application/ReceiptApplication/MainApplicationReceiptRedux.js"  //APLICACIÓN DE RECIBOS
                       ));



                var jspaoBundle = new ScriptBundle("~/accounting/bundles/ApplicationPaymentOrder");
                BundleTable.Bundles.Add(jspaoBundle.Include("~/Areas/Accounting/Scripts/AccountsPayable/PaymentOrders/MainPaymentOrdersGeneration.js"   //GENERACIÓN ÓRDENES DE PAGO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovements.js"                                             //AGENTE
                        //, "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPolicies.js"                                           //PRIMAS POR COBRAR
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovements.js"                                  //COASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovements.js"                                  //REASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/Accounting/Accounting.js"                                                             //CONTABILIDAD
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovements.js"                                     //SOLICITUD PAGO VARIOS
                        , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovements.js"                                       //SOLICITUD PAGO DE SINIESTROS
                        , "~/Areas/Accounting/Scripts/Application/InsuredLoans/InsuredLoans.js"                                                         //PRÉSTAMOS ASEGURADO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovementsRequest.js"
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovementsRequest.js"
                        ));


                var jspreBundle = new ScriptBundle("~/accounting/bundles/ApplicationPreliquidation");
                BundleTable.Bundles.Add(jspreBundle.Include("~/Areas/Accounting/Scripts/PreLiquidations/MainPreLiquidations.js"     //GENERACIÓN DE PRELIQUIDACIÓN
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovements.js"                         //AGENTE
                       // , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPolicies.js"                       //PRIMAS POR COBRAR
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovements.js"              //COASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovements.js"              //REASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/Accounting/Accounting.js"                                         //CONTABILIDAD
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovements.js"                 //SOLICITUD PAGO VARIOS
                        , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovements.js"                   //SOLICITUD PAGO DE SINIESTROS
                        , "~/Areas/Accounting/Scripts/Application/InsuredLoans/InsuredLoans.js"                                     //PRÉSTAMOS ASEGURADO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovementsRequest.js"
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovementsRequest.js"
                        ));


                var jsjouBundle = new ScriptBundle("~/accounting/bundles/ApplicationJournalEntry");
                BundleTable.Bundles.Add(jsjouBundle.Include("~/Areas/Accounting/Scripts/JournalEntry/MainJournalEntry.js"   //GENERACIÓN ASIENTO DIARIO       
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRedux.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRedux.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovements.js"                 //AGENTE
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovements.js"      //COASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovements.js"      //REASEGUROS
                                                                                                                            //, "~/Areas/Accounting/Scripts/Application/Accounting/Accounting.js"                                 //CONTABILIDAD
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovements.js"         //SOLICITUD PAGO VARIOS
                        , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovements.js"           //SOLICITUD PAGO DE SINIESTROS
                        , "~/Areas/Accounting/Scripts/Application/InsuredLoans/InsuredLoans.js"                             //PRÉSTAMOS ASEGURADO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovementsRequest.js"
                        , "~/Areas/Accounting/Scripts/JournalEntry/MainJournalEntryRedux.js"
                        , "~/Areas/Accounting/Scripts/JournalEntry/MainJournalEntryRequest.js"
                        ));

                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ReverseApplicationPremium").
                Include("~/Areas/Accounting/Scripts/Application/ReverseApplicationPremium/ReverseApplicationPremium.js",
                "~/Areas/Accounting/Scripts/Application/ReverseApplicationPremium/ReverseApplicationPremiumRequest.js",
                        "~/Areas/Accounting/Scripts/Application/ReverseApplicationPremium/reducers/ReversionPremiumRedux.js"
                      ));

            }
            else
            {
                // BOLETAS/REGULARIZACIÓN DE TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ApplicationRegularization").
                Include("~/Areas/Accounting/Scripts/Regularization/MainCardVoucherRegularization.es5.js"
                    , "~/Areas/Accounting/Scripts/Regularization/MainCheckRegularization.es5.js"));

                /*Separacion 18/08/2017*/
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/mainBilling").
                Include("~/Areas/Accounting/Scripts/Billing/MainBilling.es5.js"
                , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.es5.js"
                , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.es5.js"
                , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRedux.es5.js"
                , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRedux.es5.js"
                , "~/Areas/Accounting/Scripts/Billing/MainBillingRedux.es5.js"
                , "~/Areas/Accounting/Scripts/Billing/MainBillingRequest.es5.js"
                ));

                // CAJA/CIERRE DE CAJA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyCashClosing").
                Include("~/Areas/Accounting/Scripts/Billing/DailyCashClosingRequest.es5.js"
                , "~/Areas/Accounting/Scripts/Billing/DailyCashClosing.es5.js"));

                // CAJA/CANCELACION INGRESOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CancelAppliationReceipt").
                Include("~/Areas/Accounting/Scripts/Billing/CancelAppliationReceipt.es5.js"));

                // CAJA/BÚSQUEDA DE RECIBOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBillSearch").
                Include("~/Areas/Accounting/Scripts/BillSearch/MainBillSearch.es5.js"));

                // CAJA/CONSULTA DE MOVIMIENTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BalanceInquiries").
                Include("~/Areas/Accounting/Scripts/Transaction/BalanceInquiries.es5.js"));

                // CAJA/CONSULTA DE PAGOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPaymentStatus").
                Include("~/Areas/Accounting/Scripts/PaymentStatus/MainPaymentStatus.es5.js"));

                // CAJA/GENERAR CARGA MASIVA EXCEL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainMassiveDataGenerate").
                Include("~/Areas/Accounting/Scripts/MassiveDataLoad/MainMassiveDataGenerate.es5.js"));

                // CAJA/CARGA MASIVA  
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainMassiveDataLoad").
                Include("~/Areas/Accounting/Scripts/MassiveDataLoad/MainMassiveDataLoad.es5.js"));

                // CAJA/BOTÓN DE PAGOS ASEGURADO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPaymentService").
                Include("~/Areas/Accounting/Scripts/PaymentService/MainPaymentService.es5.js"));

                // CAJA/RECUOTIFICACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/FinancialPlan").
                Include("~/Areas/Accounting/Scripts/FinancialPlan/FinancialPlanRequest.es5.js",
                 "~/Areas/Accounting/Scripts/FinancialPlan/FinancialPlan.es5.js"));

                // BOLETAS/BOLETA INTERNA CHEQUES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckInternalDepositSlip").
                Include("~/Areas/Accounting/Scripts/CheckInternalDepositSlip/MainCheckInternalDepositSlipRequest.es5.js",
                 "~/Areas/Accounting/Scripts/CheckInternalDepositSlip/MainCheckInternalDepositSlip.es5.js"));

                // BOLETAS/BOLETA DE DEPÓSITO CHEQUES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckDepositSlip").
                Include("~/Areas/Accounting/Scripts/Transaction/MainCheckDepositSlipRequest.es5.js",
                "~/Areas/Accounting/Scripts/Transaction/MainCheckDepositSlip.es5.js"));

                // BOLETAS/BÚSQUEDA DE BOLETA DE DEPÓSITO 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DepositSlipSearch").
                Include("~/Areas/Accounting/Scripts/DepositSlipSearch/DepositSlipSearch.es5.js"));

                // BOLETAS/BÚSQUEDA DE CHEQUES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckSearch").
                Include("~/Areas/Accounting/Scripts/Transaction/MainCheckSearch.es5.js"));

                // BOLETAS/CHEQUES PENDIENTES DE DEPÓSITO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckPendingDeposit").
                Include("~/Areas/Accounting/Scripts/Transaction/MainCheckPendingDeposit.es5.js"));

                // BOLETA INTERNA DE TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardInternalDepositSlip").
                Include("~/Areas/Accounting/Scripts/CardInternalDepositSlip/MainCardInternalDepositSlip.es5.js"));

                // BOLETAS/BOLETA DE DEPÓSITO TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardDepositSlip").
                Include("~/Areas/Accounting/Scripts/Transaction/MainCardDepositSlip.es5.js"));

                // BOLETAS/BÚSQUEDA BOLETA INTERNA DE TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardDepositSlipSearch").
                Include("~/Areas/Accounting/Scripts/CardDepositSlipSearch/MainCardDepositSlipSearch.es5.js"));

                // BOLETAS/CONSULTA DE TARJETAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardVoucherSearch").
                Include("~/Areas/Accounting/Scripts/CardVoucherSearch/MainCardVoucherSearch.es5.js"));

                // BOLETAS/TARJETAS PENDIENTES DEPÓSITO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCardPendingDeposit").
                Include("~/Areas/Accounting/Scripts/CardVoucherSearch/MainCardPendingDeposit.es5.js"));

                // CAJA EGRESOS/BÚSQUEDA ÓRDENES DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrderSearch").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/PaymentOrders/PaymentOrderSearch.es5.js"));

                // CAJA EGRESOS/SOLICITUD DE PAGOS VARIOS           
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPaymentRequest").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/PaymentRequest/MainPaymentRequest.es5.js"));

                // PRELIQUIDACIÓN/BÚSQUEDA DE PRELIQUIDACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPreLiquidationsSearch").
                Include("~/Areas/Accounting/Scripts/PreLiquidations/MainPreLiquidationsSearch.es5.js"));

                // CAJA EGRESOS/PARAMETRO/CUENTA BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankAccount").
                Include("~/Areas/Accounting/Scripts/Parameters/BankAccount/MainBankAccount.es5.js"));

                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCompanyBankAccount").
                Include("~/Areas/Accounting/Scripts/Parameters/CompanyBankAccount/MainCompanyBankAccount.es5.js"));

                // CAJA EGRESOS/PARAMETRO/CONTROL DE CHEQUERAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCheckBookControl").
                Include("~/Areas/Accounting/Scripts/Parameters/CheckBookControl/MainCheckBookControl.es5.js",
                        "~/Areas/Accounting/Scripts/Parameters/CheckBookControl/MainCheckBookControlRequest.es5.js"));

                // CAJA EGRESOS/PARAMETRO/ASOCIAR BANCO A SUCURSAL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainAssociateBankBranch").
                Include("~/Areas/Accounting/Scripts/Parameters/AssociateBankBranch/MainAssociateBankBranch.es5.js"));

                // ASIENTO DE DIARIO/NOTAS DE CRÉDITO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCreditNotes").
                Include("~/Areas/Accounting/Scripts/JournalEntry/MainCreditNotes.es5.js"));

                // ASIENTO DE DIARIO/BÚSQUEDA ASIENTO DE DIARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainJournalEntryBillSearch").
                Include("~/Areas/Accounting/Scripts/JournalEntrySearch/MainJournalEntryBillSearch.es5.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ASIGNACIÓN AUTOMATICA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckAutomaticAssignment").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckAutomaticAssignment.es5.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ASIGNACIÓN MANUAL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckManualAssignment").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckManualAssignment.es5.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/IMPRESIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckPrinting").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckPrinting.es5.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/REIMPRESIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckReprinting").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckReprinting.es5.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ENTREGA DE CHEQUES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckDelivery").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckDelivery.es5.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ANULACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckNullification").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/CheckControl/CheckNullification.es5.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/GENERACIÓN/TRANSFERENCIA BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainTransfer").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/Transfer/MainTransfer.es5.js"));

                // CAJA EGRESOS/IMPRESIÓN DE CHEQUES/ANULACIÓN/TRANSFERENCIA BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainTransferNullification").
                Include("~/Areas/Accounting/Scripts/AccountsPayable/Transfer/MainTransferNullification.es5.js"));

                // CUENTA CORRIENTE/BALANCE DE COMISIONES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCommissionBalance").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Agent/MainCommissionBalance.es5.js"));

                // CUENTA CORRIENTE/ÓRDENES DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainCommissionPaymentOrder").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Agent/MainCommissionPaymentOrder.es5.js"));

                // CUENTA CORRIENTE/CIERRE PARCIAL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPartialClosure").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Agent/MainPartialClosure.es5.js"));

                // CUENTA CORRIENTE/GENERACIÓN DEL CIERRE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainClosureGeneration").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Coinsurance/MainClosureGeneration.es5.js"));

                // CUENTA CORRIENTE/REPORTE CIERRES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainClosureReport").
                Include("~/Areas/Accounting/Scripts/CurrentAccount/Coinsurance/MainClosureReport.es5.js"));

                // DÉBITO AUTOMÁTICO/ASIGNACIÓN FORMATO A RED
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainAutomaticDebitFormat").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainAutomaticDebitFormat.es5.js"));

                // DÉBITO AUTOMÁTICO/GENERACIÓN DE CUPONES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainGeneratingCoupons").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainGeneratingCoupons.es5.js"));

                // DÉBITO AUTOMÁTICO/ADMINISTRACIÓN DE ENVIOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainSendingAdministration").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainSendingAdministration.es5.js"));

                // DÉBITO AUTOMÁTICO/CARGAR RESPUESTA DEL BANCO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainLoadBankResponse").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainLoadBankResponse.es5.js"));

                // DÉBITO AUTOMÁTICO/ESTADO DE CUPONES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainReportCouponsStatus").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainReportCouponsStatus.es5.js"));

                // DÉBITO AUTOMÁTICO/CUPONES RECAUDADOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainReportCollectedCoupons").
                Include("~/Areas/Accounting/Scripts/AutomaticDebit/MainReportCollectedCoupons.es5.js"));

                // CANCELACIÓN AUTOMÁTICA DE PÓLIZAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPolicyAutomaticCancellation").
                Include("~/Areas/Accounting/Scripts/PolicyCancellation/PolicyAutomaticCancellation.es5.js"));

                // PARÁMETROS/CAJA/CONCEPTO DE INGRESO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainIncomeConcept").
                Include("~/Areas/Accounting/Scripts/Parameters/IncomeConcept/MainIncomeConcept.es5.js"));

                // PARÁMETROS/CAJA/COMPAÑÍA CONTABLE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountingCompany").
                Include("~/Areas/Accounting/Scripts/Parameters/AccountingCompany/AccountingCompany.es5.js"));

                // PARÁMETROS/CAJA/CUENTAS BANCARIAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BankAccountsSettings").
                Include("~/Areas/Accounting/Scripts/Parameters/BankAccountsSettings.es5.js"));

                // PARÁMETROS/CAJA/DIFERENCIA DE MONEDA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CurrencyDifference").
                Include("~/Areas/Accounting/Scripts/Parameters/CurrencyDifference.es5.js"));

                // PARÁMETROS/CAJA/TIPO DE PAGO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainPaymentType").
                Include("~/Areas/Accounting/Scripts/Parameters/PaymentType/MainPaymentType.es5.js"));

                // PARÁMETROS/RETENCIONES PERCIBIDAS/CONCEPTO DE RETENCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainRetentionConcept").
                Include("~/Areas/Accounting/Scripts/Parameters/Retention/MainRetentionConcept.es5.js"));

                // PARÁMETROS/RETENCIONES PERCIBIDAS/BASE DE RETENCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/RetentionBase").
                Include("~/Areas/Accounting/Scripts/Parameters/Retention/RetentionBase.es5.js"));

                // PARÁMETROS/FORMATOS DE SALIDA/AGRUPADOR DE FORMATOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TemplateGroup").
                Include("~/Areas/Accounting/Scripts/Parameters/OutputTemplate/TemplateGroup.es5.js"));

                // PARÁMETROS/FORMATOS DE SALIDA/FORMATO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TemplateFormat").
                Include("~/Areas/Accounting/Scripts/Parameters/OutputTemplate/TemplateFormat.es5.js"));

                // PARÁMETROS/FORMATOS DE SALIDA/DISEÑO DE FORMATO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TemplateParametrization").
                Include("~/Areas/Accounting/Scripts/Parameters/OutputTemplate/TemplateParametrization.es5.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO/REDES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainNetwork").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainNetwork.es5.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO /CÓDIGOS DE ESTADO DÉBITO           
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainDebitStatusCodes").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainDebitStatusCodes.es5.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO/CÓDIGOS DE RESPUESTA BANCO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankResponseCodes").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainBankResponseCodes.es5.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO/RELACIÓN REDES POR CONDUCTO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankNetworkRelationship").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainBankNetworkRelationship.es5.js"));

                // PARÁMETROS/DEBITO AUTOMÁTICO/RELACIÓN CONDUCTO TIPO DE CUENTA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainRelationshipAccountType").
                Include("~/Areas/Accounting/Scripts/Parameters/AutomaticDebit/MainRelationshipAccountType.es5.js"));

                // PARÁMETROS/REPORTE DE CARTERA//RANGOS DE DEUDAS PENDIENTES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/RangeParametrizationDetail").
                Include("~/Areas/Accounting/Scripts/Parameters/Range/RangeParametrizationDetail.es5.js"));

                // TEMPORALES/BÚSQUEDA DE TEMPORALES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainTemporarySearch").
                Include("~/Areas/Accounting/Scripts/TemporarySearch/MainTemporarySearch.es5.js"));

                // REPORTS/CUENTAS/CONSULTA DE CUENTAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountingAccount").
                Include("~/Areas/Accounting/Scripts/Reports/AccountingAccount.es5.js"));

                // REPORTS/CONTABILIDAD/REPORTE DE CUENTAS CONTABLES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountingAccountReport").
                Include("~/Areas/Accounting/Scripts/Reports/AccountingAccountReport.es5.js"));

                // REPORTS/CONTABILIDAD/INVENTARIO DE CUENTAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountInventory").
                Include("~/Areas/Accounting/Scripts/Reports/AccountInventory.es5.js"));

                // REPORTS/CUENTAS/CONSULTA DE CUENTAS EN ASIENTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AccountOnEntry").
                Include("~/Areas/Accounting/Scripts/Reports/AccountOnEntry.es5.js"));

                // REPORTS/CUENTAS/CONSULTA DE ANALISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Analysis").
                Include("~/Areas/Accounting/Scripts/Reports/Analysis.es5.js"));

                // REPORTS/CUENTAS/CONSULTA DE ASIENTOS AUTOMÁTICOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AutomaticEntry").
                Include("~/Areas/Accounting/Scripts/Reports/AutomaticEntry.es5.js"));

                // REPORTS/CONTABILIDAD/LIBRO AUXILIAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/AuxiliaryLedger").
                Include("~/Areas/Accounting/Scripts/Reports/AuxiliaryLedger.es5.js"));

                // REPORTS/CONTABILIDAD/BALANCE GENERAL
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BalanceSheet").
                Include("~/Areas/Accounting/Scripts/Reports/BalanceSheet.es5.js"));

                // REPORTS/FINANCIERA/LIBRO DE BANCOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BankLedger").
                Include("~/Areas/Accounting/Scripts/Reports/BankLedger.es5.js"));

                // REPORTS/CONTABILIDAD/MAYOR AUXILIAR DE CAJA Y BANCOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BillingBankAuxiliary").
                Include("~/Areas/Accounting/Scripts/Reports/BillingBankAuxiliary.es5.js"));

                // REPORTS/CONTABILIDAD/RESUMEN MAYOR AUXILIAR DE CAJA Y BANCOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BillingBankAuxiliarySummary").
                Include("~/Areas/Accounting/Scripts/Reports/BillingBankAuxiliarySummary.es5.js"));

                // REPORTS/CONTABILIDAD/DIARIO AUXILIAR DE CAJA Y BANCOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BillingBankDailyAuxiliary").
                Include("~/Areas/Accounting/Scripts/Reports/BillingBankDailyAuxiliary.es5.js"));

                // REPORTS/CAJA INGRESOS/LISTADOS DE MOVIMIENTOS EN CAJA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BillingMovementList").
                Include("~/Areas/Accounting/Scripts/Reports/BillingMovementList.es5.js"));

                // REPORTS/REASEGUROS/BORDER AUX
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Borderaux").
                Include("~/Areas/Accounting/Scripts/Reports/Borderaux.es5.js"));

                // REPORTS/OPERACIONES/CARTERA VENCIDA POR CORREDOR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/BrokerExpiredPortfolioList").
                Include("~/Areas/Accounting/Scripts/Reports/BrokerExpiredPortfolioList.es5.js"));

                // REPORTS/OPERACIONES/CARTERA POR CORREDOR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PortfolioByBrokerList").
                Include("~/Areas/Accounting/Scripts/Reports/PortfolioByBrokerList.es5.js"));

                // REPORTS/EMISION/REGISTRO DE CANCELACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CancellationRecordIssuance").
                Include("~/Areas/Accounting/Scripts/Reports/CancellationRecordIssuance.es5.js"));

                // REPORTS/CONTABLES/FLUJO DE EFECTIVO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CashFlow").
                Include("~/Areas/Accounting/Scripts/Reports/CashFlow.es5.js"));

                // REPORTS/RESERVA DE RIESGOS CATASTROFICOS/RESERVA CATASTRÓFICA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CatastrophicReserve").
                Include("~/Areas/Accounting/Scripts/Reports/CatastrophicReserve.es5.js"));

                // REPORTS/RIESGOS CATASTROFICOS/ASIENTO DE RESERVA CATASTRÓFICA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CatastrophicReserveEntry").
                Include("~/Areas/Accounting/Scripts/Reports/CatastrophicReserveEntry.es5.js"));

                // REPORTS/REASEGUROS/CUENTA CORRIENTE POR TRATADO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckingAccountByTreaty").
                Include("~/Areas/Accounting/Scripts/Reports/CheckingAccountByTreaty.es5.js"));

                // REPORTS/REASEGUROS/CUENTA CORRIENTE PRIMA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckingAccountPrime").
                Include("~/Areas/Accounting/Scripts/Reports/CheckingAccountPrime.es5.js"));

                // REPORTS/REASEGUROS/CUENTA CORRIENTE PRIMA VS DETALLE PRODUCCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CheckingAccountPrimeProductionDetail").
                Include("~/Areas/Accounting/Scripts/Reports/CheckingAccountPrimeProductionDetail.es5.js"));

                // REPORTS//RECLAMOS/REGISTRO DE RESERVAS SINIESTRO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ClaimsReserveRecord").
                Include("~/Areas/Accounting/Scripts/Reports/ClaimsReserveRecord.es5.js"));

                // REPORTS/REPORTES DE/IMPUESTOS POR CLIENTE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ClientTax").
                Include("~/Areas/Accounting/Scripts/Reports/ClientTax.es5.js"));

                // REPORTS/REPORTES/CONTROL DE OPERACIONES DE CIERRE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ClosingOperationsControl").
                Include("~/Areas/Accounting/Scripts/Reports/ClosingOperationsControl.es5.js"));

                // REPORTS/REPORTES/REVISIÓN OPERACIONES DE CIERRE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ClosingOperationsReview").
                Include("~/Areas/Accounting/Scripts/Reports/ClosingOperationsReview.es5.js"));

                // REPORTS/COBRANZAS/REGISTRO DE COBRANZAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CollectingRecord").
                Include("~/Areas/Accounting/Scripts/Reports/CollectingRecord.es5.js"));

                // REPORTS/CONTABLES/COMPARATIVO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ComparativeBalanceSheet").
                Include("~/Areas/Accounting/Scripts/Reports/ComparativeBalanceSheet.es5.js"));

                // REPORTS/CUENTA CONTABLE/BALANCE CONDENSADO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CondensedBalanceSheet").
                Include("~/Areas/Accounting/Scripts/Reports/CondensedBalanceSheet.es5.js"));

                // REPORTS/RESERVAS DE PREVISIÓN/DETALLE RESERVA DE PREVISIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ContingencyReserve").
                Include("~/Areas/Accounting/Scripts/Reports/ContingencyReserve.es5.js"));

                // REPORTS/CUENTAS/CENTRO DE COSTOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CostCenter").
                Include("~/Areas/Accounting/Scripts/Reports/CostCenter.es5.js"));

                // REPORTS/CUENTAS/CUADRO DE RESULTADOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ResultBoard").
                Include("~/Areas/Accounting/Scripts/Reports/ResultBoard.es5.js"));

                // REPORTS/SUMAS ASEGURADAS VIGENTES/ASIENTO PRELIMINAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CurrentInsuredAmountPreliminaryEntry").
                Include("~/Areas/Accounting/Scripts/Reports/CurrentInsuredAmountPreliminaryEntry.es5.js"));

                // REPORTS/SUMAS ASEGURADAS VIGENTES/CUADRO TECNICO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CurrentInsuredAmountsTechnicalChart").
                Include("~/Areas/Accounting/Scripts/Reports/CurrentInsuredAmountsTechnicalChart.es5.js"));

                // REPORTS/OPERACIONES/PÓLIZAS VIGENTES 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/CurrentPoliciesList").
                Include("~/Areas/Accounting/Scripts/Reports/CurrentPoliciesList.es5.js"));

                // REPORTS/CAJA INGRESOS/CIERRE DE CAJA DIARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyCashClosingList").
                Include("~/Areas/Accounting/Scripts/Reports/DailyCashClosingList.es5.js"));

                // REPORTS/CAJA EGRESOS/LISTADO DE CIERRES DE CAJA 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyClosingList").
                Include("~/Areas/Accounting/Scripts/Reports/DailyClosingList.es5.js"));

                // REPORTS/CAJA INGRESOS/DETALLE DIARIO DE INGRESOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyIncomeDetail").
                Include("~/Areas/Accounting/Scripts/Reports/DailyIncomeDetail.es5.js"));

                // REPORTS/CAJA EGRESOS/DETALLE DIARIO DE EGRESOS  
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyOutcomeDetails").
                Include("~/Areas/Accounting/Scripts/Reports/DailyOutcomeDetails.es5.js"));

                // REPORTS/CAJA EGRESOS/RESUMEN DIARIO DE EGRESOS  
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailyOutcomeSummary").
                Include("~/Areas/Accounting/Scripts/Reports/DailyOutcomeSummary.es5.js"));

                // REPORTS/CAJA INGRESOS/RESUMEN DIARIO DE INGRESOS 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/DailySummaryIncome").
                Include("~/Areas/Accounting/Scripts/Reports/DailySummaryIncome.es5.js"));

                // REPORTS/REPORTES DE/COMISIONES DE EVALUACION NO DEVENGADAS   
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/EvaluationCommissionsUnearned").
                Include("~/Areas/Accounting/Scripts/Reports/EvaluationCommissionsUnearned.es5.js"));

                // REPORTS/PRIMAS VENCIDAS/ASIENTO PRELIMINAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ExpiredPrimesPreliminaryEntry").
                Include("~/Areas/Accounting/Scripts/Reports/ExpiredPrimesPreliminaryEntry.es5.js"));

                // REPORTS/PRIMAS VENCIDAS/CUADRO TÉCNICO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ExpiredPrimeTechnicalChart").
                Include("~/Areas/Accounting/Scripts/Reports/ExpiredPrimeTechnicalChart.es5.js"));

                // REPORTS/CONTABLES/COMPARACIÓN DE SALDOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/FinalBalance").
                Include("~/Areas/Accounting/Scripts/Reports/FinalBalance.es5.js"));

                // REPORTS/REASEGUROS/ESTADO DE CUENTA-FLUCTUACIONES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/FluctuationAccountStatement").
                Include("~/Areas/Accounting/Scripts/Reports/FluctuationAccountStatement.es5.js"));

                // REPORTS/RECLAMOS/REGISTRO DE FLUCTUACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/FluctuationRecord").
                Include("~/Areas/Accounting/Scripts/Reports/FluctuationRecord.es5.js"));

                // REPORTS/CONTABLES/LIBRO DIARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/GeneralJournal").
                Include("~/Areas/Accounting/Scripts/Reports/GeneralJournal.es5.js"));

                // REPORTS/CONTABLES/LIBRO MAYOR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/GeneralLedger").
                Include("~/Areas/Accounting/Scripts/Reports/GeneralLedger.es5.js"));

                // REPORTS/RESERVAS DE IBNR/DETALLE DE RESERVA DE IBNR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IBNRReserve").
                Include("~/Areas/Accounting/Scripts/Reports/IBNRReserve.es5.js"));

                // REPORTS/IBNR/ASIENTO RESERVA IBNR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IBNRReserveEntry").
                Include("~/Areas/Accounting/Scripts/Reports/IBNRReserveEntry.es5.js"));

                // REPORTS/CIERRES/BALANCE DE SALDOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IncomeStatement").
                Include("~/Areas/Accounting/Scripts/Reports/IncomeStatement.es5.js"));

                // REPORTS/REPORTES DE/IMPUESTOS POR INTERMEDIARIO 
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IntermediaryTax").
                Include("~/Areas/Accounting/Scripts/Reports/IntermediaryTax.es5.js"));

                // REPORTS/CAJA EGRESOS/CHEQUES EMITIDOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/IssuedChecks").
                Include("~/Areas/Accounting/Scripts/Reports/IssuedChecks.es5.js"));

                // REPORTS/REASEGUROS/PARTIDA DE PÉRDIDAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/LossItem").
                Include("~/Areas/Accounting/Scripts/Reports/LossItem.es5.js"));

                // REPORTS/REPORTES/CONSULTA REPORTES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainViewReport").
                Include("~/Areas/Accounting/Scripts/Reports/MainViewReport.es5.js"));

                // REPORTS/REASEGUROS/DISTRIBUCIÓN DEL MES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MonthDistribution").
                Include("~/Areas/Accounting/Scripts/Reports/MonthDistribution.es5.js"));

                // REPORTS/REPORTES DE CARTERA/BALANCE DE ANTIGUEDAD
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/OldBalanceList").
                Include("~/Areas/Accounting/Scripts/Reports/OldBalanceList.es5.js"));

                // REPORTS/OPERACIONES/EMITIDAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/OperationsIssuedList").
                Include("~/Areas/Accounting/Scripts/Reports/OperationsIssuedList.es5.js"));

                // REPORTS/ÓRDENES DE PAGO/LIQUIDADAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrdersPaying").
                Include("~/Areas/Accounting/Scripts/Reports/PaymentOrdersPaying.es5.js"));

                // REPORTS/ÓRDENES DE PAGO/PENDIENTES            
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrders").
                Include("~/Areas/Accounting/Scripts/Reports/PaymentOrders.es5.js"));

                // REPORTS/ÓRDENES DE PAGO/ELIMINADAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PaymentOrdersRemoved").
                Include("~/Areas/Accounting/Scripts/Reports/PaymentOrdersRemoved.es5.js"));

                // REPORTS/RECIBOS/PENDIENTES DE APLICACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PendingApplicationList").
                Include("~/Areas/Accounting/Scripts/Reports/PendingApplicationList.es5.js"));

                // REPORTS/REPORTES/RETENCIONES PERCIBIDAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PerceivedRetentions").
                Include("~/Areas/Accounting/Scripts/Reports/PerceivedRetentions.es5.js"));

                // REPORTS/OPERACIONES/CARTERA POR CORREDOR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PortfolioByBrokerList").
                Include("~/Areas/Accounting/Scripts/Reports/PortfolioByBrokerList.es5.js"));

                // REPORTS/REPORTES DE CARTERA/CARTERA RECAUDADA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PortfolioCollectedList").
                Include("~/Areas/Accounting/Scripts/Reports/PortfolioCollectedList.es5.js"));

                // REPORTS/PRIMAS VENCIDAS/ASIENTO PRELIMINAR PROVISIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PreliminaryProvisionForPrimesEntry").
                Include("~/Areas/Accounting/Scripts/Reports/PreliminaryProvisionForPrimesEntry.es5.js"));

                // REPORTS/RESERVAS TECNICAS/PRIMAS POR COBRAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PremiumReceivable").
                Include("~/Areas/Accounting/Scripts/Reports/PremiumReceivable.es5.js"));

                // REPORTS/REPORTES DE CARTERA/ANTIGUEDAD DE PRIMAS POR COBRAR
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PremiumReceivableAntiquityList").
                Include("~/Areas/Accounting/Scripts/Reports/PremiumReceivableAntiquityList.es5.js"));

                // REPORTS/REPORTES DE CARTERA/DEUDORES POR PRIMAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/PrimeDebtorList").
                Include("~/Areas/Accounting/Scripts/Reports/PrimeDebtorList.es5.js"));

                // REPORTS/EMISION/DETALLE DE PRODUCCIÓN    
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ProductionDetailList").
                Include("~/Areas/Accounting/Scripts/Reports/ProductionDetailList.es5.js"));

                // REPORTS/CONTABLES/PÉRDIDAS Y GANANCIAS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ProfitsAndLosses").
                Include("~/Areas/Accounting/Scripts/Reports/ProfitsAndLosses.es5.js"));

                // REPORTS/REPORTES DE/IMPUESTOS POR PROVEEDORES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ProviderTax").
                Include("~/Areas/Accounting/Scripts/Reports/ProviderTax.es5.js"));

                // REPORTS/RECIBOS/APLICADOS EN EL DIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ReceiptsAppliedList").
                Include("~/Areas/Accounting/Scripts/Reports/ReceiptsAppliedList.es5.js"));

                // REPORTS/REASEGUROS/CUENTA CORRIENTE
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ReinsuranceCheckingAccount").
                Include("~/Areas/Accounting/Scripts/Reports/ReinsuranceCheckingAccount.es5.js"));

                // REPORTS/CAJA INGRESOS/SOLICITUDES
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Requests").
                Include("~/Areas/Accounting/Scripts/Reports/Requests.es5.js"));

                // REPORTS/RESERVAS DE PREVISIÓN/ASIENTO RESERVA DE PREVISIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ReserveEstimateEntry").
                Include("~/Areas/Accounting/Scripts/Reports/ReserveEstimateEntry.es5.js"));

                // REPORTS/CUENTAS/CUADRO DE RESULTADOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ResultBoard").
                Include("~/Areas/Accounting/Scripts/Reports/ResultBoard.es5.js"));

                // REPORTS/REPORTES/RETENCIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Retention").
                Include("~/Areas/Accounting/Scripts/Reports/Retention.es5.js"));

                // REPORTS/RESERVAS TÉCNICAS/DETALLE RESERVAS DE RIESGOS EN CURSO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/RiskReserve").
                Include("~/Areas/Accounting/Scripts/Reports/RiskReserve.es5.js"));

                // REPORTS/RIESGOS EN CURSO/ASIENTOS DE RIESGOS EN CURSO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/RiskReserveEntry").
                Include("~/Areas/Accounting/Scripts/Reports/RiskReserveEntry.es5.js"));

                // REPORTS/CONTABLES/BALANCE DE COMPROBACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TrialBalance").
                Include("~/Areas/Accounting/Scripts/Reports/TrialBalance.es5.js"));

                // REPORTS/CHEQUES ESCRITOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/WrittenCheck").
                Include("~/Areas/Accounting/Scripts/Reports/WrittenCheck.es5.js"));

                // PROCESOS ESPECIALES/CRUCE AUTOMÁTICO DE NOTAS DE CRÉDITO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainAutomaticCreditNotes").
                Include("~/Areas/Accounting/Scripts/AutomaticCreditNotes/MainAutomaticCreditNotes.es5.js",
                "~/Areas/Accounting/Scripts/AutomaticCreditNotes/AutomaticCreditNotesRequest.es5.js"));

                // PROCESOS ESPECIALES//AMORTIZACIÓN AUTOMÁTICA (no implementado en EE)
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainAutomaticAmortization").
                Include("~/Areas/Accounting/Scripts/AutomaticAmortization/MainAutomaticAmortization.es5.js"));

                // CONCILIACIÓN BANCARIA/FORMATO DE EXTRACTO BANCARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankFileFormat").
                Include("~/Areas/Accounting/Scripts/Parameters/BankReconciliation/MainBankFileFormat.es5.js"));

                // CONCILIACIÓN BANCARIA/EXTRACTO BANCARIO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankStatement").
                Include("~/Areas/Accounting/Scripts/BankStatement/MainBankStatement.es5.js"));

                // CONCILIACIÓN BANCARIA/ASIGNACIÓN FORMATO CONCILIACIÓN A CUENTA BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainReconciliationFormat").
                Include("~/Areas/Accounting/Scripts/BankStatement/MainReconciliationFormat.es5.js"));

                // CONCILIACIÓN BANCARIA/CONCILIACIÓN BANCARIA
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankReconciliation").
                Include("~/Areas/Accounting/Scripts/BankReconciliation/MainBankReconciliation.es5.js"));

                // CONCILIACIÓN BANCARIA/REVERSO CONCILIACIÓN
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainReverseReconciliation").
                Include("~/Areas/Accounting/Scripts/BankReconciliation/MainReverseReconciliation.es5.js"));

                // CONCILIACIÓN BANCARIA/PARÁMETROS/MOVIMIENTOS BANCARIOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankMovement").
                Include("~/Areas/Accounting/Scripts/BankMovement/MainBankMovement.es5.js"));

                // CONCILIACIÓN BANCARIA/PARÁMETROS/EQUIVALENCIAS ENTRE CÓDIGOS BANCARIOS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainBankCodeEquivalence").
                Include("~/Areas/Accounting/Scripts/BankMovement/BankCodeEquivalence.es5.js"));

                // PARAMETROS/DÍAS POR RAMO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainDayPrefix").
                Include("~/Areas/Accounting/Scripts/Parameters/CancellationPolicy/MainDayPrefix.es5.js"));

                // PARAMETROS/EXCLUSION DE ASEGURADO
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/MainExclusion").
                Include("~/Areas/Accounting/Scripts/Parameters/CancellationPolicy/MainExclusion.es5.js"));

                // REPORTS/CONTABILIDAD/ANALYSIS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/Analysis").
                Include("~/Areas/Accounting/Scripts/Reports/Analysis.es5.js"));

                // REPORTS/CUENTAS/CONSULTA DE ANALISIS
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/TaxRetentionList").
                Include("~/Areas/Accounting/Scripts/Reports/TaxRetentionList.es5.js"));


                var jsappBundle = new ScriptBundle("~/accounting/bundles/ApplicationReceipt");
                BundleTable.Bundles.Add(jsappBundle.Include("~/Areas/Accounting/Scripts/Application/ReceiptApplication/MainApplicationReceipt.es5.js"  //APLICACIÓN DE RECIBOS

                       , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovements.es5.js"                                          //AGENTE
                                                                                                                                                        //, "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPolicies.js"                                        //PRIMAS POR COBRAR                                        //PRIMAS POR COBRAR
                       , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRedux.es5.js"                                        //PRIMAS POR COBRAR
                       , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovements.es5.js"                               //COASEGUROS
                       , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovements.es5.js"                               //REASEGUROS
                                                                                                                                                        //, "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRedux.js"                               //REASEGUROS
                                                                                                                                                        //, "~/Areas/Accounting/Scripts/Application/Accounting/Accounting.js"                                                          //CONTABILIDAD
                       , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRedux.es5.js"                                                          //CONTABILIDAD

                       , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovements.es5.js"                                  //SOLICITUD PAGO VARIOS
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovements.es5.js"                                    //SOLICITUD PAGO DE SINIESTROS
                       , "~/Areas/Accounting/Scripts/Application/InsuredLoans/InsuredLoans.es5.js"                                                      //PRÉSTAMOS ASEGURADO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovementsRequest.es5.js"
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovementsRequest.es5.js"
                       , "~/Areas/Accounting/Scripts/Application/ReceiptApplication/MainApplicationReceiptRequest.es5.js"
                       , "~/Areas/Accounting/Scripts/Application/ReceiptApplication/MainApplicationReceiptRedux.es5.js"  //APLICACIÓN DE RECIBOS
                       ));

                var jspaoBundle = new ScriptBundle("~/accounting/bundles/ApplicationPaymentOrder");
                BundleTable.Bundles.Add(jspaoBundle.Include("~/Areas/Accounting/Scripts/AccountsPayable/PaymentOrders/MainPaymentOrdersGeneration.es5.js"   //GENERACIÓN ÓRDENES DE PAGO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovements.es5.js"                                             //AGENTE
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPolicies.es5.js"                                           //PRIMAS POR COBRAR
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovements.es5.js"                                  //COASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovements.es5.js"                                  //REASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/Accounting/Accounting.es5.js"                                                             //CONTABILIDAD
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovements.es5.js"                                     //SOLICITUD PAGO VARIOS
                        , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovements.es5.js"                                       //SOLICITUD PAGO DE SINIESTROS
                        , "~/Areas/Accounting/Scripts/Application/InsuredLoans/InsuredLoans.es5.js"                                                         //PRÉSTAMOS ASEGURADO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovementsRequest.es5.js"
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovementsRequest.es5.js"
                        ));


                var jspreBundle = new ScriptBundle("~/accounting/bundles/ApplicationPreliquidation");
                BundleTable.Bundles.Add(jspreBundle.Include("~/Areas/Accounting/Scripts/PreLiquidations/MainPreLiquidations.es5.js"     //GENERACIÓN DE PRELIQUIDACIÓN
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovements.es5.js"                         //AGENTE
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPolicies.es5.js"                       //PRIMAS POR COBRAR
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovements.es5.js"              //COASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovements.es5.js"              //REASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/Accounting/Accounting.es5.js"                                         //CONTABILIDAD
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovements.es5.js"                 //SOLICITUD PAGO VARIOS
                        , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovements.es5.js"                   //SOLICITUD PAGO DE SINIESTROS
                        , "~/Areas/Accounting/Scripts/Application/InsuredLoans/InsuredLoans.es5.js"                                     //PRÉSTAMOS ASEGURADO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovementsRequest.es5.js"
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovementsRequest.es5.js"
                        ));

                var jsjouBundle = new ScriptBundle("~/accounting/bundles/ApplicationJournalEntry");
                BundleTable.Bundles.Add(jsjouBundle.Include(
                        "~/Areas/Accounting/Scripts/JournalEntry/MainJournalEntryRequest.es5.js",
                        "~/Areas/Accounting/Scripts/JournalEntry/MainJournalEntry.es5.js"

                        //GENERACIÓN ASIENTO DIARIO
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PremiumsReceivable/DialogSearchPoliciesRedux.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRedux.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovements.es5.js"                 //AGENTE
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovements.es5.js"      //COASEGUROS
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovements.es5.js"      //REASEGUROS
                                                                                                                                //, "~/Areas/Accounting/Scripts/Application/Accounting/Accounting.es5.js"                                 //CONTABILIDAD
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovements.es5.js"         //SOLICITUD PAGO VARIOS
                        , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovements.es5.js"           //SOLICITUD PAGO DE SINIESTROS
                        , "~/Areas/Accounting/Scripts/Application/InsuredLoans/InsuredLoans.es5.js"                             //PRÉSTAMOS ASEGURADO
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountBrokers/AgentMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/Accounting/AccountingRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountReinsurances/ReinsuranceMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/CheckingAccountCoinsurances/CoinsuranceMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/Application/PaymentVarious/PaymentRequestVariousMovementsRequest.es5.js"
                       , "~/Areas/Accounting/Scripts/Application/PaymentClaims/PaymentRequestClaimsMovementsRequest.es5.js"
                        , "~/Areas/Accounting/Scripts/JournalEntry/MainJournalEntryRedux.es5.js"
                        , "~/Areas/Accounting/Scripts/JournalEntry/MainJournalEntryRequest.es5.js"

                        ));
                BundleTable.Bundles.Add(new ScriptBundle("~/accounting/bundles/ReverseApplicationPremium").
                Include("~/Areas/Accounting/Scripts/Application/ReverseApplicationPremium/ReverseApplicationPremium.es5.js"
                , "~/Areas/Accounting/Scripts/Application/ReverseApplicationPremium/ReverseApplicationPremiumRequest.es5.js",
                "~/Areas/Accounting/Scripts/Application/ReverseApplicationPremium/reducers/ReversionPremiumRedux.es5.js"
                ));
            }
        }
    }
}
