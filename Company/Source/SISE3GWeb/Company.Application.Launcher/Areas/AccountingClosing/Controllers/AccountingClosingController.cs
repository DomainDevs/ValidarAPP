//System
//Terceros
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
//Sistran Company
//Sistran Core
using Sistran.Core.Application.AccountingClosingServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.AccountingClosing.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Resources;
//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
//using AccountingRulesModels = Sistran.Core.Application.GeneralLedgerServices.Models.AccountingRules;
//using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Framework.UIF.Web.Areas.AccountingClosing.Controllers
{
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class AccountingClosingController : Controller
    {
        #region  WorkerFactory

        /// <summary>
        /// WorkerFactory
        /// </summary>
        public sealed class WorkerFactory
        {
            private static volatile WorkerFactory _instance;
            private static object syncRoot = new Object();

            public static WorkerFactory Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (syncRoot)
                        {
                            if (_instance == null)
                                _instance = new WorkerFactory();
                        }
                    }
                    return _instance;
                }
            }

            /// <summary>
            /// CreateWorker
            /// </summary>
            /// <param name="module"></param>
            /// <param name="year"></param>
            /// <param name="month"></param>
            /// <param name="day"></param>
            public void CreateWorker(int module, int year, int month, int day)
            {
                if (module == Convert.ToInt32(ConfigurationManager.AppSettings["TechnicalReserveModule"]))
                {
                    Thread thread = new Thread(() => DelegateService.accountingClosingApplicationService.ClaimReserveClosureGeneration(year, month, module));
                    thread.Start();
                }
            }

        }

        #endregion

        #region Interfaz


        readonly BaseController _baseController = new BaseController();

        #endregion

        #region View

        /// <summary>
        /// AccountingClosing
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AccountingClosing()
        {
            try
            {

                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                ViewBag.IncomeAndExpensesModule = Convert.ToInt32(ConfigurationManager.AppSettings["IncomeAndExpensesModule"]);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// Closing
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Closing()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }    
        }

        public ActionResult ExchangeDifference()
        {
            return View();
        }

        #endregion

        #region Actions

        /// <summary>
        /// GetStatus
        /// </summary>
        /// <param name="module"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetStatus(int module)
        {           
            return Json(new { aaData = DelegateService.accountingClosingApplicationService.GetStatus(module) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// MonthlyClosure
        /// Realiza el proceso de precierre
        /// </summary>
        /// <param name="module"></param>
        /// <returns>Task<JsonResult/></returns>
        public async Task<JsonResult> MonthlyClosure(int module)
        {
            var result = 0;
            int processId = 0; //dato para actualización de proceso en caso de error.

            try
            {
                result = DelegateService.accountingClosingApplicationService.MonthlyClosureAsync(module);
                return Json(new { success = true, result = result.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                processId = DelegateService.AccountingClosingService.GetLogProcessId(module);

                //Si existen errores se actualiza el log de proceso
                DelegateService.AccountingClosingService.UpdateLogProcess(processId);

                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                processId = DelegateService.AccountingClosingService.GetLogProcessId(module);

                //Si existen errores se actualiza el log de proceso
                DelegateService.AccountingClosingService.UpdateLogProcess(processId);

                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GenerateMonthlyClosing
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public JsonResult GenerateMonthlyClosing(int module)
        {
            try
            {
                var accountingDate = DelegateService.AccountingClosingService.GetClosingDate(module);
                                
                int day = 0;
                
                if (day == 0) // TODO: dato quemado por el momento, no existe el campo LastClosingDd en el modelo moduleDate del Common
                {
                    day = 0;
                }
                else
                {
                    day = accountingDate.Day;
                }

                WorkerFactory.Instance.CreateWorker(module, accountingDate.Year, accountingDate.Month, day);

                return Json(new { success = true, result = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// AccountClosure
        /// Realiza el proceso de Contabilización
        /// </summary>
        /// <param name="module"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AccountClosure(int module)
        {            
            try
            {
                List<string> entryNumbers = new List<string>();
                int day = 0; // dato quemado por el momento, no existe el campo LastClosingDd en el modelo moduleDate del Common
                entryNumbers = DelegateService.accountingClosingApplicationService.AccountClosure(module, SessionHelper.GetUserId(), day);

                return Json(new { success = true, result = entryNumbers }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException unhandledException)
            {
                return Json(new { success = false, result = unhandledException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// LoadMonthlyProcessReport
        /// Carga información para los reportes
        /// </summary>
        /// <param name="module"></param>
        /// <returns>int</returns>
        public int LoadMonthlyProcessReport(int module)
        {
            
            var reports = DelegateService.accountingClosingApplicationService.LoadMonthlyProcessReport(module, SessionHelper.GetUserId(), SessionHelper.GetUserName());
            TempData["MonthlyProcessReports"] = reports;
            TempData["MonthlyProcessReportsSummary"] = DelegateService.accountingClosingApplicationService.LoadMonthlyProcessReportSummaries(module, SessionHelper.GetUserId()); 
            TempData["MonthlyProcessReportsName"] = "Areas//AccountingClosing//Reports//MonthlyProcessReports.rpt";

            return reports.Count;
        }

        /// <summary>
        /// LoadPrintEntryReport
        /// Carga datos para el reporte
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public void LoadPrintEntryReport(int entry, int module)
        {
            TempData["MonthlyProcessReports"] = DelegateService.accountingClosingApplicationService.LoadPrintEntryReport(entry, module, SessionHelper.GetUserId(), SessionHelper.GetUserName());
            TempData["MonthlyProcessReportsSummary"] = DelegateService.accountingClosingApplicationService.LoadSummaryEntryReport(entry, module, SessionHelper.GetUserId());
            TempData["MonthlyProcessReportsName"] = "Areas//AccountingClosing//Reports//MonthlyProcessReports.rpt";
        }

        /// <summary>
        /// ShowMonthlyProcessReport
        /// Carga el reporte a pdf
        /// </summary>
        /// <returns></returns>
        public void ShowMonthlyProcessReport()
        {
            var summary = TempData["MonthlyProcessReportsSummary"];
            var reportSource = TempData["MonthlyProcessReports"];
            var reportName = TempData["MonthlyProcessReportsName"];

            ReportDocument reportDocument = new ReportDocument();

            string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/" + reportName);

            if (reportName != null)
            {
                reportDocument.Load(reportPath);

                if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                {
                    reportDocument.SetDataSource(reportSource);

                    reportDocument.Subreports[0].SetDataSource(summary);
                }

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response,
                    false, "Cierre Mensual de Emisión");
            }

            TempData["MonthlyProcessReport"] = null;
            TempData["MonthlyProcessReportsSummary"] = null;
            TempData["MonthlyProcessReportName"] = null;
        }

        /// <summary>
        /// GetClosureMonth
        /// Obtiene el mes y año que se va a cerrar
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>Closing Date</returns>
        public JsonResult GetClosureMonth(int moduleId)
        {
            var closingDate = DelegateService.AccountingClosingService.GetClosingDate(moduleId);
            return Json(new { year = closingDate.Date.Year, month = closingDate.Date.Month, day = closingDate.Date.Day }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReportExcel
        /// </summary>
        /// <param name="module"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ReportExcel(int module)
        {
            List<AccountingClosingReportDTO> closures = new List<AccountingClosingReportDTO>();
            List<AccountingClosingReportDTO> excelReports = new List<AccountingClosingReportDTO>();


            excelReports = DelegateService.accountingClosingApplicationService.ReportExcel(module, SessionHelper.GetUserId());
            if (excelReports.Count == 0)
            {
                return File(new byte[0], "application/vnd.ms-excel", "ReportExcel.xls");
            }

            MemoryStream array = GetExcelStringBuilder(excelReports, module);
            return File(array.ToArray(), "application/vnd.ms-excel", "ReportExcel.xls");
        }

        /// <summary>
        /// GenerateTaxYearEntryReport
        /// </summary>
        /// <param name="closingTypeId"></param>
        /// <param name="entryNumber"></param>
        /// <param name="year"></param>
        public void GenerateTaxYearEntryReport(int closingTypeId, int entryNumber, int year)
        {
            DateTime dateMonthlyClosing = DateTime.Now; //pendiente definir el módulo en tmodulo

            var monthlyProcess = new List<MonthlyProcessModel>();
            var monthlyProcessSummary = new List<MonthlyProcessSummaryModel>();

            List<AccountingClosingReportDTO> accountingClosingReports = new List<AccountingClosingReportDTO>();
            decimal totalDebit = 0;
            decimal totalCredit = 0;

            foreach (AccountingClosingReportDTO accountingClosingReport in accountingClosingReports)
            {
                monthlyProcess.Add(new MonthlyProcessModel()
                {
                    BranchCode = accountingClosingReport.BrachCd,
                    BranchDescription = accountingClosingReport.BranchDescription,
                    CurrencyCode = accountingClosingReport.CurrencyCd,
                    CurrencyDescription = accountingClosingReport.CurrencyDescription,
                    AccountNatureCode = (accountingClosingReport.AccountNatureCd.Equals("C"))
                    ? Global.Credit.ToUpper() : Global.Debit.ToUpper(),
                    AccountingAccountCode = accountingClosingReport.AccountingAccountCd,
                    AccountingAccountDescription = accountingClosingReport.AccountingAccountDescription,
                    Debit = (accountingClosingReport.AccountNatureCd.Equals("C")) ? 0 : accountingClosingReport.LocalAmountValue,
                    Credit = (accountingClosingReport.AccountNatureCd.Equals("C")) ? accountingClosingReport.LocalAmountValue : 0,
                    Title = "COMPROBANTE DE DIARIO No " + entryNumber,
                    AccountDate = dateMonthlyClosing.ToShortDateString(),
                    Description = accountingClosingReport.Description
                });

                totalCredit += (accountingClosingReport.AccountNatureCd.Equals("C")) ? accountingClosingReport.LocalAmountValue : 0;
                totalDebit += (accountingClosingReport.AccountNatureCd.Equals("D")) ? accountingClosingReport.LocalAmountValue : 0;
            }

            var summary = new MonthlyProcessSummaryModel();

            summary.TotalCredit = totalCredit;
            summary.TotalDebit = totalDebit;

            monthlyProcessSummary.Add(summary);

            TempData["MonthlyProcessReports"] = monthlyProcess;
            TempData["MonthlyProcessReportsSummary"] = monthlyProcessSummary;
            TempData["MonthlyProcessReportsName"] = "Areas//AccountingClosing//Reports//MonthlyProcessReports.rpt";
        }

        /// <summary>
        /// GetUserIdByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>User Code</returns>
        public int GetUserIdByName(string name)
        {
            return DelegateService.uniqueUserService.GetUserByLogin(name).UserId;
        }

        #endregion

        #region IssuanceDailyEntry

        /// <summary>
        /// GenerateIssuanceEntry
        /// Genera los asientos para el cierre de emisión.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public LedgerEntryDTO GenerateIssuanceEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId)
        {
            
            return DelegateService.accountingClosingApplicationService.GenerateIssuanceEntry(accountingClosings, moduleId, SessionHelper.GetUserId());
        }

        #endregion IssuanceDailyEntry

        #region ReinsuranceDailyEntry

        /// <summary>
        /// GenerateReinsuranceEntry
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="temporalEntryNumber"></param>
        /// <param name="processId"></param>
        /// <param name="moduleId"></param>
        public void GenerateReinsuranceEntry(List<AccountingClosingReportDTO> accountingClosings, int temporalEntryNumber, int processId, int moduleId)
        {
            #region Parameters


            #endregion Parameters

            try
            {
                DelegateService.accountingClosingApplicationService.GenerateReinsuranceEntry(accountingClosings, temporalEntryNumber, processId, moduleId, SessionHelper.GetUserId());
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SaveTemporalEntryRecord
        /// Método para grabar el movimiento en la tabla temporal
        /// </summary>
        /// <param name="ledgerEntry"></param>
        /// <param name="temporalEntryNumber"></param>
        /// <param name="processId"></param>
        /// <param name="module"></param>
        public void SaveTemporalEntryRecord(LedgerEntryDTO ledgerEntry, int temporalEntryNumber, int processId, int module)
        {            
            try
            {
                DelegateService.accountingClosingApplicationService.SaveTemporalEntryRecord(ledgerEntry, temporalEntryNumber, processId, module,SessionHelper.GetUserId());
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// ReinsuranceClosureGeneration
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public int ReinsuranceClosureGeneration(int year, int month, int day, int module)
        {           
            return DelegateService.accountingClosingApplicationService.ReinsuranceGenerationClosure(year, month, day, module, SessionHelper.GetUserId()); //records
        }

        #endregion ReinsuranceDailyEntry

        #region ClaimReserveDailyEntry

        /// <summary>
        /// GenerateClaimReserveEntry
        /// Genera los asientos para el cierre de reservas de riesgos.
        /// </summary>
        /// <param name="AccountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateClaimReserveEntry(List<AccountingClosingReportDTO> AccountingClosings, int moduleId)
        {            
            return DelegateService.accountingClosingApplicationService.GenerateClaimReserveEntry(AccountingClosings, moduleId, SessionHelper.GetUserId()); 
        }

        #endregion ClaimReserveDailyEntry

        #region RiskReserveDailyEntry

        /// <summary>
        /// GenerateRiskReserveEntry
        /// Genera los asientos para el cierre de reservas de riesgos.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateRiskReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId)
        {            
            return DelegateService.accountingClosingApplicationService.GenerateRiskReserveEntry(accountingClosings, moduleId, SessionHelper.GetUserId()); 
        }

        #endregion RiskReserveDailyEntry

        #region Closing

        /// <summary>
        /// GetClosingList
        /// Obtiene lista de tipos de cierres
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetClosings()
        {
            List<ClosingModel> closingModels = new List<ClosingModel>();

            closingModels.Add(new ClosingModel() { ClosingTypeId = 1, Description = Global.ClosingIncomeExpenses });
            closingModels.Add(new ClosingModel() { ClosingTypeId = 2, Description = Global.MonthlyUtilityClosing });
            closingModels.Add(new ClosingModel() { ClosingTypeId = 3, Description = Global.OpeningSeatAssetsPassive });
            closingModels.Add(new ClosingModel() { ClosingTypeId = 4, Description = Global.ReverseAnnualOpeningSeat });
            closingModels.Add(new ClosingModel() { ClosingTypeId = 5, Description = Global.ReverseAnnualClosureIncomeExpenses });

            return new UifSelectResult(closingModels);
        }

        /// <summary>
        /// CheckClosedModules
        /// Metodo para saber si todos los módulos han sido cerrados para el mes de diciembre.
        /// </summary>
        /// <param name="year"></param>
        /// <returns>UifJsonResult</returns>
        public ActionResult CheckClosedModules(int year)
        {

            try
            {
                return new UifJsonResult(true, DelegateService.accountingClosingApplicationService.CheckClosedModules(year));
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// ExecuteClosing
        /// Realiza cierre de Procesos
        /// </summary>
        /// <param name="closingTypeId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>JsonResult</returns>
        public ActionResult ExecuteClosing(int closingTypeId, int year, int month)
        {            
            try
            {                
                return new UifJsonResult(true, DelegateService.accountingClosingApplicationService.ExecuteClosing(closingTypeId, year, month, SessionHelper.GetUserId()));
            }
            catch
            {
                return new UifJsonResult(false, 0);
            }
        }

        #endregion Closing

        #region IBNRReserveDailyEntry

        /// <summary>
        /// GenerateIBNRReserveEntry
        /// Genera los asientos para el cierre de reservas de IBNR.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateIBNRReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId)
        {
            
            return DelegateService.accountingClosingApplicationService.GenerateIBNRReserveEntry(accountingClosings, moduleId, SessionHelper.GetUserId()); 
        }

        #endregion IBNRReserveDailyEntry

        #region PrevisionReserveDailyEntry

        /// <summary>
        /// GeneratePrevisionReserveEntry
        /// Genera los asientos para el cierre de reservas de previsión.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GeneratePrevisionReserveEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId)
        {
            return DelegateService.accountingClosingApplicationService.GeneratePrevisionReserveEntry(accountingClosings, moduleId, SessionHelper.GetUserId());
        }

        #endregion PrevisionReserveDailyEntry

        #region CatastrophicRiskDailyEntry

        /// <summary>
        /// GenerateCatastrophicRiskEntry
        /// Genera los asientos para el cierre de reservas riesgos catastróficos.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateCatastrophicRiskEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId)
        {
            return DelegateService.accountingClosingApplicationService.GenerateCatastrophicRiskEntry(accountingClosings, moduleId,SessionHelper.GetUserId());
        }

        #endregion CatastrophicRiskDailyEntry

        #region ExpiredPremiumsDailyEntry

        /// <summary>
        /// GenerateExpiredPremiumsEntry
        /// Genera los asientos para el cierre de primas vencidas.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateExpiredPremiumsEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId)
        {
           return DelegateService.accountingClosingApplicationService.GenerateExpiredPremiumsEntry(accountingClosings, moduleId, SessionHelper.GetUserId());
        }

        /// <summary>
        /// GeneratePrevisionExpiredPremiumsEntry
        /// Genera los asientos de previsión para el cierre de primas vencidas.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GeneratePrevisionExpiredPremiumsEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId)
        {
            return DelegateService.accountingClosingApplicationService.GeneratePrevisionExpiredPremiumsEntry(accountingClosings, moduleId, SessionHelper.GetUserId()); 
        }

        #endregion ExpiredPremiumsDailyEntry

        #region IncomeAndExpensesDailyEntry

        /// <summary>
        /// GenerateIncomeAndExpensesEntry
        /// Genera los asientos para el cierre de ingresos y egresos.
        /// </summary>
        /// <param name="accountingClosings"></param>
        /// <param name="moduleId"></param>
        /// <returns>Entry</returns>
        public LedgerEntryDTO GenerateIncomeAndExpensesEntry(List<AccountingClosingReportDTO> accountingClosings, int moduleId)
        {
            return DelegateService.accountingClosingApplicationService.GenerateIncomeAndExpensesEntry(accountingClosings, moduleId, SessionHelper.GetUserId());
        }

        #endregion IncomeAndExpensesDailyEntry

        #region ExchangeDifference

        /// <summary>
        /// GetExchangeDifference
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public JsonResult GetExchangeDifferenceDate(string startDate, string endDate)
        {
            return Json(DelegateService.accountingClosingApplicationService.GetExchangeDifferenceDate(startDate,endDate), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GenerateExchangeDifferenceReport
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateExchangeDifferenceReport()
        {
            try
            {
                TempData["ExchangeDifferenceRecords"] = DelegateService.accountingClosingApplicationService.GenerateExchangeDifferenceReport();
                TempData["ExchangeDifferenceReportName"] = "Areas//AccountingClosing//Reports//ExchangeDifferenceReport.rpt";

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }


        }

        /// <summary>
        /// ShowExchangeDifferenceReport
        /// </summary>
        public void ShowExchangeDifferenceReport()
        {
            var reportSource = TempData["ExchangeDifferenceRecords"];
            var reportName = TempData["ExchangeDifferenceReportName"];

            ReportDocument reportDocument = new ReportDocument();

            string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/" + reportName);

            if (reportName != null)
            {
                reportDocument.Load(reportPath);

                if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                {
                    reportDocument.SetDataSource(reportSource);
                }

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Cierre Mensual de Emisión");
            }

            TempData["ExchangeDifferenceRecords"] = null;
            TempData["ExchangeDifferenceReportName"] = null;
        }

        /// <summary>
        /// AccountExchangeDifferenceRecords
        /// </summary>
        public JsonResult AccountExchangeDifferenceRecords()
        {
            return Json(DelegateService.accountingClosingApplicationService.AccountExchangeDifferenceRecords(SessionHelper.GetUserId()), JsonRequestBehavior.AllowGet);
        }

        #endregion ExchangeDifference

        #region PrivateMethods

        /// <summary>
        /// GetExcelStringBuilder
        /// </summary>
        /// <param name="accountingClosingReports"></param>
        /// <param name="module"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GetExcelStringBuilder(List<AccountingClosingReportDTO> accountingClosingReports, int module)
        {
            var workbook = new HSSFWorkbook();            
            var sheet = workbook.CreateSheet();
            var header = sheet.CreateRow(10);
            var company = sheet.CreateRow(8);

            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = HSSFColor.Indigo.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            var fontdetail = workbook.CreateFont();
            fontdetail.FontName = "Tahoma";
            fontdetail.FontHeightInPoints = 8;
            fontdetail.Boldweight = 3;

            ICellStyle styledetalle = workbook.CreateCellStyle();
            styledetalle.SetFont(fontdetail);
            styledetalle.BottomBorderColor = HSSFColor.Black.Index;
            styledetalle.LeftBorderColor = HSSFColor.Black.Index;
            styledetalle.RightBorderColor = HSSFColor.Black.Index;
            styledetalle.TopBorderColor = HSSFColor.Black.Index;
            styledetalle.BorderBottom = BorderStyle.Thin;
            styledetalle.BorderLeft = BorderStyle.Thin;
            styledetalle.BorderRight = BorderStyle.Thin;
            styledetalle.BorderTop = BorderStyle.Thin;
            styledetalle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            styledetalle.Alignment = HorizontalAlignment.Left;

            ICellStyle styleline = workbook.CreateCellStyle();
            styleline.SetFont(fontdetail);
            styleline.BottomBorderColor = HSSFColor.Black.Index;
            styleline.BorderBottom = BorderStyle.Thin;
            styleline.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");

            ICellStyle styleletter = workbook.CreateCellStyle();
            styleletter.SetFont(fontdetail);
            styleletter.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            company.CreateCell(4).SetCellValue("EMPRESA SISTRAN ANDINA");

            DateTime dateMonthlyClosing = DelegateService.AccountingClosingService.GetClosingDate(module);
            var moduleDecription = _baseController.GetModuleDateDescriptionByModuleId(module);

            var title = "PARTIDA DE PRODUCCION CORRESPONDIENTE AL MES DE " + CultureInfo.CurrentCulture.
                                DateTimeFormat.GetMonthName(dateMonthlyClosing.Month).ToUpper() + " DEL AÑO "
                                + dateMonthlyClosing.Year.ToString();

            header.CreateCell(4).SetCellValue(title);

            var date = sheet.CreateRow(18);
            date.CreateCell(2).SetCellValue("FECHA CONTABLE:" + " " + dateMonthlyClosing.ToShortDateString());
            date.CreateCell(4).SetCellValue("FECHA DE INGRESO:" + " " + DateTime.Now.ToShortDateString());
            date.GetCell(2).CellStyle = styleletter;
            date.GetCell(4).CellStyle = styleletter;

            var details = sheet.CreateRow(19);
            details.CreateCell(2).SetCellValue("DESCRIPCIÓN:" + " " + moduleDecription.ToString());
            details.CreateCell(4).SetCellValue("MONEDA:" + " " + accountingClosingReports.FirstOrDefault().CurrencyDescription);
            details.GetCell(2).CellStyle = styleletter;
            details.GetCell(4).CellStyle = styleletter;

            var user = sheet.CreateRow(20);
            user.CreateCell(2).SetCellValue("USUARIO: " + SessionHelper.GetUserName());
            user.GetCell(2).CellStyle = styleletter;

            var headerRow = sheet.CreateRow(22);
            headerRow.CreateCell(2).SetCellValue("CUENTA");
            headerRow.CreateCell(3).SetCellValue("DESCRIPCION");
            headerRow.CreateCell(4).SetCellValue("DEBE");
            headerRow.CreateCell(5).SetCellValue("HABER");
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 30 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;

            var rowNumber = 23;
            decimal totalDebitbalance = 0;
            decimal totalCreditbalance = 0;
            decimal debit = 0;
            decimal credit = 0;

            foreach (AccountingClosingReportDTO accountingClosingReport in accountingClosingReports)
            {
                var row = sheet.CreateRow(rowNumber++);

                row.CreateCell(2).SetCellValue(accountingClosingReport.AccountingAccountCd);
                row.GetCell(2).CellStyle = styledetalle;
                row.CreateCell(3).SetCellValue(accountingClosingReport.AccountingAccountDescription);
                row.GetCell(3).CellStyle = styledetalle;
                debit = (accountingClosingReport.AccountNatureCd.Equals(Convert.ToString(Convert.ToInt32(AccountingNatures.Credit)))) ? 0 : accountingClosingReport.LocalAmountValue;
                credit = (accountingClosingReport.AccountNatureCd.Equals(Convert.ToString(Convert.ToInt32(AccountingNatures.Credit)))) ? accountingClosingReport.LocalAmountValue : 0;
                // Reaseguros
                if (module == Convert.ToInt32(ConfigurationManager.AppSettings["ReinsuranceModule"]))
                {
                    debit = credit;
                }
                row.CreateCell(4).SetCellValue((double)debit);
                row.GetCell(4).CellStyle = styledetalle;
                row.CreateCell(5).SetCellValue((double)credit);
                row.GetCell(5).CellStyle = styledetalle;

                totalCreditbalance += (accountingClosingReport.AccountNatureCd.Equals(Convert.ToString(Convert.ToInt32(AccountingNatures.Credit)))) ? accountingClosingReport.LocalAmountValue : 0;
                totalDebitbalance += (accountingClosingReport.AccountNatureCd.Equals(Convert.ToString(Convert.ToInt32(AccountingNatures.Debit)))) ? accountingClosingReport.LocalAmountValue : 0;
                // Reaseguros
                if (module == Convert.ToInt32(ConfigurationManager.AppSettings["ReinsuranceModule"]))
                {
                    totalDebitbalance = totalCreditbalance;
                }
            }

            rowNumber++;
            var totalrow = sheet.CreateRow(rowNumber++);

            totalrow.CreateCell(3).SetCellValue("BALANCE GENERAL");
            totalrow.CreateCell(4).SetCellValue((double)totalDebitbalance);
            totalrow.CreateCell(5).SetCellValue((double)totalCreditbalance);
            totalrow.GetCell(3).CellStyle = styledetalle;
            totalrow.GetCell(4).CellStyle = styledetalle;
            totalrow.GetCell(5).CellStyle = styledetalle;

            rowNumber = rowNumber + 2;
            var sign = sheet.CreateRow(rowNumber++);
            sign.CreateCell(3).SetCellValue("ELABORADO POR:");
            sign.CreateCell(5).SetCellValue("AUTORIZADO POR:");
            sign.GetCell(3).CellStyle = styleletter;
            sign.GetCell(5).CellStyle = styleletter;

            var lineasign = sheet.CreateRow(rowNumber);
            lineasign.CreateCell(3).CellStyle = styleline;
            lineasign.CreateCell(5).CellStyle = styleline;

            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            //create the anchor
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 3, 4, 5);
            anchor.AnchorType = 5;

            String url = System.Web.HttpContext.Current.Server.MapPath("~/") + "Images/Logo.jpg";
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage(url, workbook));
            picture.Resize();
            picture.LineStyle = NPOI.SS.UserModel.LineStyle.None;

            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            return output;
        }

        /// <summary>
        /// LoadImage
        /// Carga imagen en el reporte
        /// </summary>
        /// <param name="path"></param>
        /// <param name="workbook"></param>
        /// <returns>int</returns>
        private int LoadImage(string path, HSSFWorkbook workbook)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);

                return workbook.AddPicture(buffer, PictureType.JPEG);
            }
        }

        /// <summary>
        /// GetExchangeDifferenceReportRecords
        /// </summary>
        /// <returns></returns>
        private List<ExchangeDifferenceReportDTO> GetExchangeDifferenceReportRecords()
        {
            ModuleDate module = new ModuleDate();
            module.Id = 9;
            module = DelegateService.tempCommonService.GetModuleDate(module);
            int accountingYear = module.LastClosingYyyy;

            List<ExchangeDifferenceReportDTO> exchangeDifferenceReportDTOs = DelegateService.AccountingClosingService.GetExchangeDifferenceRecords();
            exchangeDifferenceReportDTOs = (from ExchangeDifferenceReportDTO item in exchangeDifferenceReportDTOs where item.AccountingYear == accountingYear && !item.Posted select item).ToList();

            return exchangeDifferenceReportDTOs;
        }
        
        #endregion PrivateMethods
    }
}