using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class ReportsController : Controller
    {
        #region Instance Variables       
        readonly BaseController _baseController = new BaseController();

        #endregion Instance Variables

        #region EntryReportsViews

        /// <summary>
        /// Balance
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Balance()
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

        /// <summary>
        /// AuxiliaryDailyEntry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AuxiliaryDailyEntry()
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

        /// <summary>
        /// AuxiliaryEntry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AuxiliaryEntry()
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

        /// <summary>
        /// AuxiliaryEntrySummary
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AuxiliaryEntrySummary()
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

        #endregion EntryReportsViews

        #region EntrySummary

        /// <summary>
        /// ShowSummaryEntryReport
        /// </summary>
        /// <returns></returns>
        public void ShowSummaryEntryReport()
        {
            try
            {
                var reportSource = TempData["SummaryEntryReportSource"];
                var reportName = TempData["SummaryEntryReportName"];

                ReportDocument reportDocument = new ReportDocument();

                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                reportDocument.Load(reportPath);
                if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                {
                    //Llena Reporte Principal
                    reportDocument.SetDataSource(reportSource);
                    reportDocument.DataDefinition.FormulaFields["startDate"].Text = "'" + TempData["StartDate"] + "'";
                    reportDocument.DataDefinition.FormulaFields["endDate"].Text = "'" + TempData["EndDate"] + "'";
                }

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "SummaryEntryReportName");

                // Clear all sessions value
                TempData["StartDate"] = null;
                TempData["EndDate"] = null;
                TempData["EntryReportSource"] = null;
                TempData["EntrySubReportSource"] = null;
                TempData["EntryReportName"] = null;
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LoadSummaryEntryReport
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public void LoadSummaryEntryReport(int branchId, DateTime dateFrom, DateTime dateTo)
        {
            List<EntryReportModel> entryReportModels = new List<EntryReportModel>();
            dateTo = dateTo.Add(new TimeSpan(23, 59, 59));

            List<EntryDTO> entries = DelegateService.glAccountingApplicationService.GetEntriesByDate(dateFrom, dateTo, branchId);

            foreach (EntryDTO entry in entries)
            {
                EntryReportModel entryReportModel = new EntryReportModel();

                entryReportModel.Branch = entry.BranchDescription;
                entryReportModel.AccountingAccountId = Convert.ToInt32(entry.AccountingAccountId);
                entryReportModel.AccountingAccountDescription = Convert.ToString(entry.AccountingAccountDescription);
                entryReportModel.AccountingNumber = Convert.ToDecimal(entry.AccountingNumber);
                entryReportModel.BankDescription = entry.BankReconciliationDescription;
                entryReportModel.Description = Convert.ToString(entry.Description);
                entryReportModel.EntryNumber = Convert.ToInt32(entry.EntryNumber);
                entryReportModel.EntryId = Convert.ToInt32(entry.EntryId);
                entryReportModel.ReceiptNumber = Convert.ToInt32(entry.BankReconciliationId);
                entryReportModel.EntryId = Convert.ToInt32(entry.EntryId);
                entryReportModel.AccountingDate = entry.Date.ToString("dd/MM/yyyy");
                entryReportModel.AccountingAccountFullInformation = entry.AccountingNumber + " - " + entry.AccountingAccountDescription;

                if ((entry.ImputationTypeDescription == null) || (Convert.ToString(entry.ImputationTypeDescription) == ""))
                {
                    entryReportModel.ImputationDescription = Convert.ToString("0");
                }
                else
                {
                    entryReportModel.ImputationDescription = Convert.ToString(entry.ImputationTypeDescription);
                }

                if (entry.AccountingNature == 2)
                {
                    entryReportModel.Debit = (entry.AmountLocalValue == 0) ? (Convert.ToDecimal(entry.AmountValue) * Convert.ToDecimal(entry.ExchangeRate)) : Convert.ToDecimal(entry.AmountLocalValue);
                }
                else if (entry.AccountingNature == 1)
                {
                    entryReportModel.Credit = (entry.AmountLocalValue == 0) ? Convert.ToDecimal(entry.AmountValue) * Convert.ToDecimal(entry.ExchangeRate) : Convert.ToDecimal(entry.AmountLocalValue);
                }

                entryReportModel.Balance = entryReportModel.Debit - entryReportModel.Credit;

                if (branchId != 0 && branchId == Convert.ToInt32(entry.BranchCd))
                {
                    entryReportModels.Add(entryReportModel);
                }
                else if (branchId == 0)
                {
                    entryReportModels.Add(entryReportModel);
                }
            }

            MemoryStream memoryStream = new MemoryStream();

            if (entryReportModels.Count > 0)
            {
                memoryStream = GetCurrentSummaryEntryExcelBuilder(entryReportModels);
            }

            TempData["StartDate"] = dateFrom.ToString("dd/MM/yyyy");
            TempData["EndDate"] = dateTo.ToString("dd/MM/yyyy");
            TempData["SummaryEntryReportSource"] = entryReportModels;
            TempData["SummaryEntryReportName"] = "Areas//GeneralLedger//Reports//SummaryEntryReport.rpt";
            TempData["SummaryEntryExcel"] = memoryStream;
        }

        /// <summary>
        /// CreateExcelSummaryEntry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CreateExcelSummaryEntry()
        {
            MemoryStream memoryStream = (MemoryStream)TempData["SummaryEntryExcel"];
            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "SummaryEntryExcel.xls");
        }

        /// <summary>
        /// GetCurrentSummaryEntryExcelBuilder
        /// </summary>
        /// <param name="entryReportModels"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GetCurrentSummaryEntryExcelBuilder(List<EntryReportModel> entryReportModels)
        {
            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet();
            var header = sheet.CreateRow(11);
            var company = sheet.CreateRow(9);

            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;
            styleHeader.Alignment = HorizontalAlignment.Center;

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
            
            ICellStyle styleline = workbook.CreateCellStyle();
            styleline.SetFont(fontdetail);
            styleline.BottomBorderColor = HSSFColor.Black.Index;
            styleline.BorderBottom = BorderStyle.Thin;

            ICellStyle styledoubleline = workbook.CreateCellStyle();
            styledoubleline.SetFont(fontdetail);
            styledoubleline.BottomBorderColor = HSSFColor.Black.Index;
            styledoubleline.BorderBottom = BorderStyle.Double;

            ICellStyle styleletter = workbook.CreateCellStyle();
            styleletter.SetFont(fontdetail);
            company.CreateCell(4).SetCellValue(Global.Company.ToUpper() + ": " + Global.CompanyName.ToUpper());
            header.CreateCell(4).SetCellValue(Global.AuxiliaryLedgerSummary.ToUpper());

            var reportDate = sheet.CreateRow(18);
            reportDate.CreateCell(2).SetCellValue(@Global.Date.ToUpper() + ": ");
            reportDate.GetCell(2).CellStyle = styleletter;

            reportDate.CreateCell(3).SetCellValue(DateTime.Now.ToString("dd/MM/yyyy"));
            reportDate.GetCell(3).CellStyle = styleletter;

            var headerRow = sheet.CreateRow(19);

            headerRow.CreateCell(2).SetCellValue(@Global.AccountingDate.ToUpper());
            headerRow.CreateCell(3).SetCellValue(@Global.Branch.ToUpper());
            headerRow.CreateCell(4).SetCellValue(@Global.AccountingAccountNumber.ToUpper());
            headerRow.CreateCell(5).SetCellValue(@Global.AccountingAccount.ToUpper());
            headerRow.CreateCell(6).SetCellValue(@Global.Bank.ToUpper());
            headerRow.CreateCell(7).SetCellValue(@Global.Description.ToUpper());
            headerRow.CreateCell(8).SetCellValue(@Global.JournalEntryNumber.ToUpper());
            headerRow.CreateCell(9).SetCellValue(@Global.Debit.ToUpper());
            headerRow.CreateCell(10).SetCellValue(@Global.Credit.ToUpper());
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 30 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 20 * 256);
            sheet.SetColumnWidth(9, 20 * 256);
            sheet.SetColumnWidth(10, 20 * 256);
            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;
            headerRow.GetCell(6).CellStyle = styleHeader;
            headerRow.GetCell(7).CellStyle = styleHeader;
            headerRow.GetCell(8).CellStyle = styleHeader;
            headerRow.GetCell(9).CellStyle = styleHeader;
            headerRow.GetCell(10).CellStyle = styleHeader;

            int rowNumber = 20;

            foreach (var item in entryReportModels)
            {
                var row = sheet.CreateRow(rowNumber++);
                row.CreateCell(2).SetCellValue(item.AccountingDate);
                row.GetCell(2).CellStyle = styledetalle;
                row.CreateCell(3).SetCellValue(item.Branch);
                row.GetCell(3).CellStyle = styledetalle;
                row.CreateCell(4).SetCellValue(item.AccountingNumber.ToString());
                row.GetCell(4).CellStyle = styledetalle;
                row.CreateCell(5).SetCellValue(item.AccountingAccountDescription);
                row.GetCell(5).CellStyle = styledetalle;
                row.CreateCell(6).SetCellValue(item.BankDescription);
                row.GetCell(6).CellStyle = styledetalle;
                row.CreateCell(7).SetCellValue(item.Description);
                row.GetCell(7).CellStyle = styledetalle;
                row.CreateCell(8).SetCellValue(item.EntryNumber);
                row.GetCell(8).CellStyle = styledetalle;
                row.CreateCell(9).SetCellValue((double)item.Debit);
                row.GetCell(9).CellStyle = styledetalle;
                row.CreateCell(10).SetCellValue((double)item.Credit);
                row.GetCell(10).CellStyle = styledetalle;
            }

            String url = System.Web.HttpContext.Current.Server.MapPath("~/") + "\\Images\\Logo.jpg";

            XSSFCreationHelper helper = workbook.GetCreationHelper() as XSSFCreationHelper;
            XSSFDrawing drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            XSSFClientAnchor anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
            anchor.Col1 = 2;
            anchor.Row1 = 2;
            anchor.AnchorType = 5;
            XSSFPicture pict = drawing.CreatePicture(anchor, LoadImage(url, workbook)) as XSSFPicture;
            pict.Resize();
            pict.LineStyle = NPOI.SS.UserModel.LineStyle.None;

            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return output;
        }

        #endregion EntrySummary

        #region Entry

        /// <summary>
        /// ShowEntryReport
        /// </summary>
        public void ShowEntryReport()
        {
            try
            {
                var reportSource = TempData["EntryReportSource"];
                var reportName = TempData["EntryReportName"];

                ReportDocument reportDocument = new ReportDocument();

                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                reportDocument.Load(reportPath);
                if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                {
                    //Lena Reporte Principal
                    reportDocument.SetDataSource(reportSource);
                    reportDocument.DataDefinition.FormulaFields["startDate"].Text = "'" + TempData["StartDate"] + "'";
                    reportDocument.DataDefinition.FormulaFields["endDate"].Text = "'" + TempData["EndDate"] + "'";
                }

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "EntryReportName");
                // Clear all sessions value
                TempData["StartDate"] = null;
                TempData["EndDate"] = null;
                TempData["EntryReportSource"] = null;
                TempData["EntrySubReportSource"] = null;
                TempData["EntryReportName"] = null;
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LoadEntryReport
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult LoadEntryReport(int branchId, DateTime dateFrom, DateTime dateTo)
        {

            try
            {
                List<EntryReportModel> entryReportModels = new List<EntryReportModel>();
                dateTo = dateTo.Add(new TimeSpan(23, 59, 59));

                List<EntryDTO> entries = DelegateService.glAccountingApplicationService.GetEntriesByDate(dateFrom, dateTo, branchId);

                foreach (EntryDTO entry in entries)
                {
                    EntryReportModel entryReportModel = new EntryReportModel();

                    entryReportModel.Branch = entry.BranchDescription;
                    entryReportModel.AccountingAccountId = Convert.ToInt32(entry.AccountingAccountId);
                    entryReportModel.AccountingAccountDescription = Convert.ToString(entry.AccountingAccountDescription);
                    entryReportModel.AccountingNumber = Convert.ToDecimal(entry.AccountingNumber);
                    entryReportModel.BankDescription = Convert.ToString(entry.BankReconciliationDescription);
                    entryReportModel.Description = Convert.ToString(entry.Description);
                    entryReportModel.EntryNumber = Convert.ToInt32(entry.EntryNumber);
                    entryReportModel.EntryId = Convert.ToInt32(entry.EntryId);
                    //entryReportModel.ReceiptNumber = Convert.ToInt32(entry.BankReconciliationId);
                    entryReportModel.ReceiptNumber = Convert.ToInt32(entry.ReceiptNumber);
                    entryReportModel.CurrencyDescription = entry.CurrencyDescription;

                    if ((Convert.ToString(entry.ImputationTypeDescription) == "") || (entry.ImputationTypeDescription == null))
                    {
                        entryReportModel.ImputationDescription = Convert.ToString("Asiento Diario");
                    }
                    else
                    {
                        entryReportModel.ImputationDescription = Convert.ToString(entry.ImputationTypeDescription);
                    }


                    //débitos - créditos
                    if (entry.AccountingNature == 2)
                    {
                        entryReportModel.Debit = Convert.ToDecimal(entry.AmountLocalValue);
                        if (entry.AmountLocalValue == 0)
                        {
                            entryReportModel.Debit = Convert.ToDecimal(entry.AmountValue) * Convert.ToDecimal(entry.ExchangeRate);
                        }
                    }
                    if (entry.AccountingNature == 1)
                    {
                        entryReportModel.Credit = Convert.ToDecimal(entry.AmountLocalValue);
                        if (entry.AmountLocalValue == 0)
                        {
                            entryReportModel.Credit = Convert.ToDecimal(entry.AmountValue) * Convert.ToDecimal(entry.ExchangeRate);
                        }
                    }

                    entryReportModel.Balance = entryReportModel.Debit - entryReportModel.Credit;

                    if (branchId != 0 && branchId == Convert.ToInt32(entry.BranchCd))
                    {
                        entryReportModels.Add(entryReportModel);
                    }
                    else if (branchId == 0)
                    {
                        entryReportModels.Add(entryReportModel);
                    }
                }

                MemoryStream memoryStream = new MemoryStream();

                if (entryReportModels.Count > 0)
                {
                    memoryStream = GetCurrentAuxiliaryEntryBuilder(entryReportModels);
                }

                TempData["StartDate"] = dateFrom.ToString("dd/MM/yyyy");
                TempData["EndDate"] = dateTo.ToString("dd/MM/yyyy");
                TempData["EntryReportSource"] = entryReportModels;
                TempData["EntryReportName"] = "Areas//GeneralLedger//Reports//EntryReport.rpt";
                TempData["EntryReportExcel"] = memoryStream;


                return Json(new { success = true, result = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, result = ex.Message }, JsonRequestBehavior.AllowGet);
            }

         
        }

        /// <summary>
        /// CreateExcelEntry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CreateExcelEntry()
        {
            MemoryStream memoryStream = (MemoryStream)TempData["EntryReportExcel"];
            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "EntryReportExcel.xls");
        }

        /// <summary>
        /// GetCurrentAuxiliaryEntryBuilder
        /// </summary>
        /// <param name="entryReportModels"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GetCurrentAuxiliaryEntryBuilder(List<EntryReportModel> entryReportModels)
        {
            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet();
            var header = sheet.CreateRow(11);
            var company = sheet.CreateRow(9);

            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
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

            ICellStyle styleline = workbook.CreateCellStyle();
            styleline.SetFont(fontdetail);
            styleline.BottomBorderColor = HSSFColor.Black.Index;
            styleline.BorderBottom = BorderStyle.Thin;

            ICellStyle styledoubleline = workbook.CreateCellStyle();
            styledoubleline.SetFont(fontdetail);
            styledoubleline.BottomBorderColor = HSSFColor.Black.Index;
            styledoubleline.BorderBottom = BorderStyle.Double;

            ICellStyle styleletter = workbook.CreateCellStyle();
            styleletter.SetFont(fontdetail);
            company.CreateCell(4).SetCellValue("EMPRESA : SISTRAN ANDINA");
            header.CreateCell(4).SetCellValue("MAYOR AUXILIAR ");

            var reportDate = sheet.CreateRow(18);
            reportDate.CreateCell(2).SetCellValue("FECHA:");
            reportDate.GetCell(2).CellStyle = styleletter;

            reportDate.CreateCell(3).SetCellValue("");
            reportDate.GetCell(3).CellStyle = styleletter;

            var headerRow = sheet.CreateRow(19);

            headerRow.CreateCell(2).SetCellValue("Sucursal");
            headerRow.CreateCell(3).SetCellValue("# de Cuenta contable");
            headerRow.CreateCell(4).SetCellValue("Cuenta contable");
            headerRow.CreateCell(5).SetCellValue("Descripcion");
            headerRow.CreateCell(6).SetCellValue("Numero de Asiento");
            headerRow.CreateCell(7).SetCellValue("Moneda");
            headerRow.CreateCell(8).SetCellValue("Debito");
            headerRow.CreateCell(9).SetCellValue("Credito");
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 30 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 20 * 256);
            sheet.SetColumnWidth(9, 20 * 256);
            sheet.SetColumnWidth(10, 20 * 256);
            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;
            headerRow.GetCell(6).CellStyle = styleHeader;
            headerRow.GetCell(7).CellStyle = styleHeader;
            headerRow.GetCell(8).CellStyle = styleHeader;
            headerRow.GetCell(9).CellStyle = styleHeader;

            int rowNumber = 20;

            foreach (var item in entryReportModels)
            {
                var row = sheet.CreateRow(rowNumber++);
                row.CreateCell(2).SetCellValue(item.Branch);
                row.GetCell(2).CellStyle = styledetalle;
                row.CreateCell(3).SetCellValue(item.AccountingNumber.ToString());
                row.GetCell(3).CellStyle = styledetalle;
                row.CreateCell(4).SetCellValue(item.AccountingAccountDescription);
                row.GetCell(4).CellStyle = styledetalle;
                row.CreateCell(5).SetCellValue(item.Description);
                row.GetCell(5).CellStyle = styledetalle;
                row.CreateCell(6).SetCellValue(item.EntryNumber);
                row.GetCell(6).CellStyle = styledetalle;
                row.CreateCell(7).SetCellValue(item.CurrencyDescription);
                row.GetCell(7).CellStyle = styledetalle;
                row.CreateCell(8).SetCellValue((double)item.Debit);
                row.GetCell(8).CellStyle = styledetalle;
                row.CreateCell(9).SetCellValue((double)item.Credit);
                row.GetCell(9).CellStyle = styledetalle;
            }

            
            String url = System.Web.HttpContext.Current.Server.MapPath("~/") + "\\Images\\Logo.jpg";

            XSSFCreationHelper helper = workbook.GetCreationHelper() as XSSFCreationHelper;
            XSSFDrawing drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            XSSFClientAnchor anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
            anchor.Col1 = 2;
            anchor.Row1 = 2;
            anchor.AnchorType = 5;
            XSSFPicture pict = drawing.CreatePicture(anchor, LoadImage(url, workbook)) as XSSFPicture;
            pict.Resize();
            pict.LineStyle = NPOI.SS.UserModel.LineStyle.None;


            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return output;
        }

        #endregion Entry

        #region DailyEntry
        
        /// <summary>
        /// LoadDailyEntryReport
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public void LoadDailyEntryReport(int branchId, DateTime dateFrom, DateTime dateTo)
        {
            List<DailyEntryReportModel> dailyEntryReportModels = new List<DailyEntryReportModel>();
            dateTo = dateTo.Add(new TimeSpan(23, 59, 59));

            List<EntryDTO> entries = DelegateService.glAccountingApplicationService.GetDailyEntriesByRangeDateBranchId(dateFrom, dateTo, branchId);
          
            foreach (EntryDTO entry in entries)
            {
                DailyEntryReportModel entryReportModel = new DailyEntryReportModel();
                entryReportModel.Branch = entry.BranchDescription;// Mod-DanC: 2018-05-22
                entryReportModel.AccountingAccountId = Convert.ToInt32(entry.AccountingAccountId);
                entryReportModel.AccountingAccountDescription = Convert.ToString(entry.AccountingAccountDescription);
                entryReportModel.AccountingNumber = Convert.ToDecimal(entry.AccountingNumber);
                entryReportModel.BankDescription = Convert.ToString(entry.BranchDescription);
                entryReportModel.Description = Convert.ToString(entry.Description);
                entryReportModel.EntryNumber = Convert.ToInt32(entry.EntryNumber);
                entryReportModel.DailyEntryId = Convert.ToInt32(entry.DailyEntryId);
                entryReportModel.Date = Convert.ToDateTime(entry.ReceiptDate);
                entryReportModel.DailyEntryHeaderId = Convert.ToInt32(entry.DailyEntryHeaderId);
                entryReportModel.TransactionNumber = Convert.ToInt32(entry.TransactionNumber);
                entryReportModel.ImputationDescription = Convert.ToString(entry.ImputationTypeDescription);
                entryReportModel.CurrencyDescription = entry.CurrencyDescription;

                if (Convert.ToString(entry.ImputationTypeDescription) != "" && entry.ImputationTypeDescription != null)
                {
                    entryReportModel.ImputationDescription = Convert.ToString(entry.ImputationTypeDescription);
                }
                else
                {
                    entryReportModel.ImputationDescription = Convert.ToString("-");
                }

                //Créditos y débitos
                if (entry.AccountingNature == 2)
                {
                    entryReportModel.Debit = Convert.ToDecimal(entry.AmountLocalValue);
                    if (entry.AmountLocalValue == 0)
                    {
                        entryReportModel.Debit = Convert.ToDecimal(entry.AmountValue) * Convert.ToDecimal(entry.ExchangeRate);
                    }
                }
                if (entry.AccountingNature == 1)
                {
                    entryReportModel.Credit = Convert.ToDecimal(entry.AmountLocalValue);
                    if (entry.AmountLocalValue == 0)
                    {
                        entryReportModel.Credit = Convert.ToDecimal(entry.AmountValue) * Convert.ToDecimal(entry.ExchangeRate);
                    }
                }
                
                if (branchId != 0 && branchId == Convert.ToInt32(entry.BranchCd))
                {
                    dailyEntryReportModels.Add(entryReportModel);
                }
                else if (branchId == 0)
                {
                    dailyEntryReportModels.Add(entryReportModel);
                }
            }

            MemoryStream memoryStream = new MemoryStream();

            if (dailyEntryReportModels.Count > 0)
            {
                memoryStream = GetCurrentAuxiliaryDailyEntryBuilder(dailyEntryReportModels);    
            }

            TempData["DailyEntryReport"] = dailyEntryReportModels;
            TempData["DailyEntryReportName"] = "Areas//GeneralLedger//Reports//DailyEntryReport.rpt";
            TempData["DailyEntryReportExcel"] = memoryStream;
            TempData["dateFrom"] = dateFrom.ToString("dd/MM/yyyy");
            TempData["dateTo"] = dateTo.ToString("dd/MM/yyyy");
        }
        
        /// <summary>
        /// ShowDailyEntryReport
        /// </summary>
        public void ShowDailyEntryReport()
        {
            try
            {
                var reportSource = TempData["DailyEntryReport"];
                var reportName = TempData["DailyEntryReportName"];

                ReportDocument reportDocument = new ReportDocument();

                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                reportDocument.Load(reportPath);
                if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                {
                    //Lena Reporte Principal
                    reportDocument.SetDataSource(reportSource);
                    reportDocument.DataDefinition.FormulaFields["dateFrom"].Text = "'" + TempData["dateFrom"] + "'";
                    reportDocument.DataDefinition.FormulaFields["dateTo"].Text = "'" + TempData["dateTo"] + "'";
                }

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat,
                                                    System.Web.HttpContext.Current.Response, false, "DailyEntryReport");

                //// Clear all sessions value
                TempData["EntryReportSource"] = null;
                TempData["EntrySubReportSource"] = null;
                TempData["EntryReportName"] = null;
                TempData["dateFrom"] = null;
                TempData["dateTo"] = null;
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// GetCurrentAuxiliaryDailyEntryBuilder
        /// </summary>
        /// <param name="dailyEntryReportModels"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GetCurrentAuxiliaryDailyEntryBuilder(List<DailyEntryReportModel> dailyEntryReportModels)
        {
            var workbook = new XSSFWorkbook();

            var sheet = workbook.CreateSheet();
            var header = sheet.CreateRow(11);
            var company = sheet.CreateRow(9);

            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
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

            ICellStyle styleline = workbook.CreateCellStyle();
            styleline.SetFont(fontdetail);
            styleline.BottomBorderColor = HSSFColor.Black.Index;
            styleline.BorderBottom = BorderStyle.Thin;

            ICellStyle styledoubleline = workbook.CreateCellStyle();
            styledoubleline.SetFont(fontdetail);
            styledoubleline.BottomBorderColor = HSSFColor.Black.Index;
            styledoubleline.BorderBottom = BorderStyle.Double;

            ICellStyle styleletter = workbook.CreateCellStyle();
            styleletter.SetFont(fontdetail);
            company.CreateCell(4).SetCellValue("EMPRESA : SISTRAN ANDINA");
            header.CreateCell(4).SetCellValue("DIARIO AUXILIAR ");

            var ReportDate = sheet.CreateRow(18);
            ReportDate.CreateCell(2).SetCellValue("FECHA:");
            ReportDate.GetCell(2).CellStyle = styleletter;

            ReportDate.CreateCell(3).SetCellValue("");
            ReportDate.GetCell(3).CellStyle = styleletter;

            var headerRow = sheet.CreateRow(19);

            headerRow.CreateCell(2).SetCellValue("Sucursal");
            headerRow.CreateCell(3).SetCellValue("# de Cuenta contable");
            headerRow.CreateCell(4).SetCellValue("Cuenta contable");
            headerRow.CreateCell(5).SetCellValue("Descripcion");
            headerRow.CreateCell(6).SetCellValue("Numero de Asiento");
            headerRow.CreateCell(7).SetCellValue("Fecha");
            headerRow.CreateCell(8).SetCellValue("Numero de movimiento Contable");
            headerRow.CreateCell(9).SetCellValue("Moneda");
            headerRow.CreateCell(10).SetCellValue("Debito");
            headerRow.CreateCell(11).SetCellValue("Credito");
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 30 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 20 * 256);
            sheet.SetColumnWidth(9, 20 * 256);
            sheet.SetColumnWidth(10, 20 * 256);
            sheet.SetColumnWidth(11, 20 * 256);
            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;
            headerRow.GetCell(6).CellStyle = styleHeader;
            headerRow.GetCell(7).CellStyle = styleHeader;
            headerRow.GetCell(8).CellStyle = styleHeader;
            headerRow.GetCell(9).CellStyle = styleHeader;
            headerRow.GetCell(10).CellStyle = styleHeader;
            headerRow.GetCell(11).CellStyle = styleHeader;

            long  rowNumber = 20;

            foreach (var item in dailyEntryReportModels)
            {
                var row = sheet.CreateRow(Convert.ToInt32(rowNumber++));
                row.CreateCell(2).SetCellValue(item.Branch);
                row.GetCell(2).CellStyle = styledetalle;
                row.CreateCell(3).SetCellValue(item.AccountingNumber.ToString());
                row.GetCell(3).CellStyle = styledetalle;
                row.CreateCell(4).SetCellValue(item.AccountingAccountDescription);
                row.GetCell(4).CellStyle = styledetalle;
                row.CreateCell(5).SetCellValue(item.Description);
                row.GetCell(5).CellStyle = styledetalle;
                row.CreateCell(6).SetCellValue(item.DailyEntryHeaderId);
                row.GetCell(6).CellStyle = styledetalle;
                row.CreateCell(7).SetCellValue(Convert.ToString(item.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)));
                row.GetCell(7).CellStyle = styledetalle;
                row.CreateCell(8).SetCellValue(item.TransactionNumber);
                row.GetCell(8).CellStyle = styledetalle;
                row.CreateCell(9).SetCellValue(item.CurrencyDescription);//ImputationDescription
                row.GetCell(9).CellStyle = styledetalle;
                row.CreateCell(10).SetCellValue((double)item.Debit);
                row.GetCell(10).CellStyle = styledetalle;
                row.CreateCell(11).SetCellValue((double)item.Credit);
                row.GetCell(11).CellStyle = styledetalle;
            }
            
            String url = System.Web.HttpContext.Current.Server.MapPath("~/") + "\\Images\\Logo.jpg";

            XSSFCreationHelper helper = workbook.GetCreationHelper() as XSSFCreationHelper;
            XSSFDrawing drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            XSSFClientAnchor anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
            anchor.Col1 = 2;
            anchor.Row1 = 2;
            anchor.AnchorType = 5;
            XSSFPicture pict = drawing.CreatePicture(anchor, LoadImage(url, workbook)) as XSSFPicture;
            pict.Resize();
            pict.LineStyle = NPOI.SS.UserModel.LineStyle.None;
            
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return output;
        }

        /// <summary>
        /// CreateExcelDailyEntry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CreateExcelDailyEntry()
        {
            MemoryStream memoryStream = (MemoryStream)TempData["DailyEntryReportExcel"];
            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "DailyEntryReportExcel.xls");
        }

        #endregion DailyEntry

        #region Methods

        /// <summary>
        /// LoadImage
        /// </summary>
        /// <param name="path"></param>
        /// <param name="workbook"></param>
        /// <returns>int</returns>
        public static int LoadImage(string path, XSSFWorkbook workbook)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);

                return workbook.AddPicture(buffer, PictureType.JPEG);
            }
        }

        /// <summary>
        /// GetCurrencyDescriptionById
        /// Obtiene la descripción de la moneda dado su id
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns>string</returns>
        private string GetCurrencyDescriptionById(int currencyId)
        {
            var currencies = DelegateService.commonService.GetCurrencies();
            var currencyNames = currencies.Where(sl => sl.Id == currencyId).ToList();

            return currencyNames[0].Description;
        }

        #endregion Methods
        
        #region DailyEntryConsultationReport

        /// <summary>
        /// ShowDailyEntryConsultationReport
        /// </summary>
        /// <param name="entryNumber"></param>
        public void ShowDailyEntryConsultationReport(int entryNumber)
        {
            try
            {
                var entries = DelegateService.glAccountingApplicationService.SearchDailyEntryMovements(entryNumber, Convert.ToDateTime("01/01/1900"), 
                                              Convert.ToDateTime("01/01/1900"), 0, 0, 0);

                var entryReports = (from EntryConsultationDTO entry in entries
                                       select new DailyEntryReportModel()
                                       {
                                           Branch = _baseController.GetBranchDescriptionById(entry.BranchId, User.Identity.Name.ToUpper()),
                                           AccountingAccountId = entry.AccountingAccountId,
                                           AccountingAccountDescription = entry.AccountingAccountName,
                                           AccountingNumber = Convert.ToDecimal(entry.AccountingAccountNumber),
                                           Description = entry.EntryDescription,
                                           DailyEntryHeaderDescription = entry.EntryHeaderDescription,
                                           EntryNumber = entry.DailyEntryHeaderId,
                                           DailyEntryId = entry.EntryId,
                                           Date = Convert.ToDateTime(entry.Date),
                                           DailyEntryHeaderId = entry.DailyEntryHeaderId,
                                           CurrencyDescription = GetCurrencyDescriptionById(entry.CurrencyId),
                                           Debit = entry.AccountingNature == (int)AccountingNature.Debit
                                           ? entry.DebitsAmountValue : 0,
                                           Credit = entry.AccountingNature == (int)AccountingNature.Credit
                                           ? entry.CreditsAmountValue : 0
                                       }).ToList();

                var reportDocument = new ReportDocument();
                var reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Areas//GeneralLedger//Reports//DailyEntryConsultationReport.rpt";

                reportDocument.Load(reportPath);

                //Llena Reporte Principal
                reportDocument.SetDataSource(entryReports);
                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "DailyEntryReport");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        #endregion DailyEntryConsultationReport

        #region EntryConsultationReport

        /// <summary>
        /// ShowEntryConsultationReport
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="entryNumber"></param>
        /// <param name="destinationId"></param>
        /// <param name="accountingMovementTypeId"></param>
        public void ShowEntryConsultationReport(int branchId, int year, int month, int entryNumber, int destinationId, int accountingMovementTypeId)
        {
            try
            {
                // Se arma las fechas para consulta
                string dateFrom = "01" + "/" + month.ToString() + "/" + year.ToString();
                int numberOfDays = DateTime.DaysInMonth(year, month);
                string dateTo = numberOfDays.ToString() + "/" + month.ToString() + "/" + year.ToString();
                dateTo = dateTo + " 23:59:59";

                var entries = DelegateService.glAccountingApplicationService.SearchEntryMovements(entryNumber, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), branchId, destinationId, accountingMovementTypeId);

                var entryReports = (from EntryConsultationDTO entry in entries
                                       select new DailyEntryReportModel()
                                       {
                                           Branch = _baseController.GetBranchDescriptionById(entry.BranchId, User.Identity.Name.ToUpper()),
                                           AccountingAccountId = entry.AccountingAccountId,
                                           AccountingAccountDescription = entry.AccountingAccountName,
                                           AccountingNumber = Convert.ToDecimal(entry.AccountingAccountNumber),
                                           Description = entry.EntryDescription,
                                           DailyEntryHeaderDescription = " - ",
                                           EntryNumber = entry.EntryNumber,
                                           DailyEntryId = entry.EntryId,
                                           Date = Convert.ToDateTime(entries[0].Date.ToString().Substring(0, 10)).Date,
                                           DailyEntryHeaderId = entry.DailyEntryHeaderId,
                                           CurrencyDescription = GetCurrencyDescriptionById(entry.CurrencyId),
                                           Debit = entry.AccountingNature == (int)AccountingNature.Debit
                                           ? entry.DebitsAmountValue : 0,
                                           Credit = entry.AccountingNature == (int)AccountingNature.Credit
                                           ? entry.CreditsAmountValue : 0
                                       }).ToList();

                                
                var reportDocument = new ReportDocument();
                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Areas//GeneralLedger//Reports//EntryConsultationReport.rpt";

                reportDocument.Load(reportPath);

                //Llena Reporte Principal
                reportDocument.SetDataSource(entryReports);
                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "EntryReport");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// SearchNewEntry
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="date"></param>
        /// <param name="entryNumber"></param>
        /// <param name="destinationId"></param>
        /// <param name="accMovTypeId"></param>
        public void SearchNewEntry(int branchId, string date, int entryNumber, int destinationId, int accMovTypeId)
        {
            ShowEntryConsultationReport(branchId, Convert.ToDateTime(date).Year, Convert.ToDateTime(date).Month, entryNumber, destinationId, accMovTypeId);
        }

        #endregion EntryConsultationReport
        
        #region BalanceChecking

        /// <summary>
        /// LoadBalanceChekingReport
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public void LoadBalanceChekingReport(string dateFrom, string dateTo)
        {
            List<BalanceChekingModel> balanceCheckingModels = new List<BalanceChekingModel>();
            List<SumaryReportChekingModel> summaryReportCheckingModels = new List<SumaryReportChekingModel>();

            DateTime dateTimeTo = Convert.ToDateTime(dateTo);
            dateTimeTo = dateTimeTo.Add(new TimeSpan(23, 59, 59));

            DateTime dateTimeFrom = Convert.ToDateTime(dateFrom);

            var balances = DelegateService.glAccountingApplicationService.GetBalanceCheckingDTO(dateTimeFrom, dateTimeTo);

            decimal totalSumCredit = 0;
            decimal totalSumDebit = 0;
            decimal totalCreditbalance = 0;
            decimal totalDebitbalance = 0;
            decimal totalAssets = 0;
            decimal totalLiabilities = 0;
            decimal equity = 0;
            decimal totalIncome = 0;
            decimal totalCost = 0;
            decimal totalSpending = 0;
            decimal totalEquity = 0;
            decimal capital = 0;
            decimal grossIncome = 0;
            decimal totalUtilities = 0;


            foreach (BalanceCheckingDTO balanceCheckingDTO in balances)
            {
                BalanceChekingModel balanceChekingModel = new BalanceChekingModel();

                balanceChekingModel.AccountingAccount = Convert.ToString(balanceCheckingDTO.AccountingAccount);
                balanceChekingModel.DescriptionAccount = Convert.ToString(balanceCheckingDTO.DescriptionAccount);
                balanceChekingModel.SumCredit = Convert.ToDecimal(balanceCheckingDTO.SumCredit);
                balanceChekingModel.SumDebit = Convert.ToDecimal(balanceCheckingDTO.SumDebit);

                if (Convert.ToDecimal(balanceCheckingDTO.Debitbalance) > 0)
                {
                    balanceChekingModel.Creditbalance = Convert.ToDecimal(balanceCheckingDTO.Debitbalance);
                    balanceChekingModel.Debitbalance = Convert.ToDecimal(0);
                }
                else
                {
                    balanceChekingModel.Creditbalance = Convert.ToDecimal(0);
                    balanceChekingModel.Debitbalance = Convert.ToDecimal(balanceCheckingDTO.Creditbalance);
                }

                totalSumCredit = totalSumCredit + balanceChekingModel.SumCredit;
                totalSumDebit = totalSumDebit + balanceChekingModel.SumDebit;
                totalCreditbalance = totalCreditbalance + balanceChekingModel.Creditbalance;
                totalDebitbalance = totalDebitbalance + balanceChekingModel.Debitbalance;
                balanceCheckingModels.Add(balanceChekingModel);

                //calculos del resumen del balance
                //suma de activos
                if (balanceChekingModel.AccountingAccount.Substring(0, 1) == "1")
                {
                    totalAssets = totalAssets + balanceChekingModel.SumDebit - balanceChekingModel.SumCredit;
                }
                //suma pasivos
                if (balanceChekingModel.AccountingAccount.Substring(0, 1) == "2")
                {
                    totalLiabilities = totalLiabilities + balanceChekingModel.SumDebit - balanceChekingModel.SumCredit;
                }
                //suma patrimonio
                if (balanceChekingModel.AccountingAccount.Substring(0, 1) == "3")
                {
                    equity = equity + balanceChekingModel.SumDebit - balanceChekingModel.SumCredit;
                }
                // suma ingresos
                if (balanceChekingModel.AccountingAccount.Substring(0, 1) == "4")
                {
                    totalIncome = totalIncome + balanceChekingModel.SumDebit - balanceChekingModel.SumCredit;
                }
                // suma costos
                if (balanceChekingModel.AccountingAccount.Substring(0, 1) == "5")
                {
                    totalCost = totalCost + balanceChekingModel.SumDebit - balanceChekingModel.SumCredit;
                }
                //suma gastos
                if (balanceChekingModel.AccountingAccount.Substring(0, 1) == "6")
                {
                    totalSpending = totalSpending + balanceChekingModel.SumDebit - balanceChekingModel.SumCredit;
                }
            }

            //ingreso bruto
            grossIncome = totalIncome - totalCost;
            //total utilidades
            totalUtilities = grossIncome - totalSpending;
            //total patrimonio
            totalEquity = equity + totalUtilities;
            // capital Contable
            capital = totalLiabilities + totalEquity;

            SumaryReportChekingModel summaryReportChekingModel = new SumaryReportChekingModel();
            summaryReportChekingModel.TotalAssets = System.Math.Round(totalAssets, 2);
            summaryReportChekingModel.TotalLiabilities = System.Math.Round(totalLiabilities, 2);
            summaryReportChekingModel.Equity = System.Math.Round(equity, 2);
            summaryReportChekingModel.TotalIncome = System.Math.Round(totalIncome, 2);
            summaryReportChekingModel.TotalCost = System.Math.Round(totalCost, 2);
            summaryReportChekingModel.TotalSpending = System.Math.Round(totalSpending, 2);
            summaryReportChekingModel.GrossIncome = System.Math.Round(grossIncome, 2);
            summaryReportChekingModel.TotalUtilities = System.Math.Round(totalUtilities, 2);
            summaryReportChekingModel.TotalEquity = System.Math.Round(totalEquity, 2);
            summaryReportChekingModel.Capital = System.Math.Round(capital, 2);

            BalanceChekingModel balanceChekingModelTotal = new BalanceChekingModel();
            balanceChekingModelTotal.AccountingAccount = Convert.ToString("");
            balanceChekingModelTotal.DescriptionAccount = Convert.ToString("TOTAL : ");
            balanceChekingModelTotal.SumCredit = Convert.ToDecimal(totalSumCredit);
            balanceChekingModelTotal.SumDebit = Convert.ToDecimal(totalSumDebit);
            balanceChekingModelTotal.Creditbalance = Convert.ToDecimal(totalCreditbalance);
            balanceChekingModelTotal.Debitbalance = Convert.ToDecimal(totalDebitbalance);

            balanceCheckingModels.Add(balanceChekingModelTotal);
            summaryReportCheckingModels.Add(summaryReportChekingModel);
            TempData["BalanceCheking"] = balanceCheckingModels;
            TempData["SumaryBalanceCheking"] = summaryReportCheckingModels;
            TempData["BalanceChekingName"] = "Areas//GeneralLedger//Reports//BalanceCheking.rpt";
        }

        /// <summary>
        /// ShowReport
        /// </summary>
        public void ShowReport()
        {
            try
            {
                var reportSource = TempData["BalanceCheking"];
                var reportSourceSummary = TempData["SumaryBalanceCheking"];
                var reportName = TempData["BalanceChekingName"];
                ReportDocument reportDocument = new ReportDocument();

                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
              
                reportDocument.Load(reportPath);

                if (reportSource != null && reportSource.GetType().ToString() != "System.String")
                {
                    //Llena Reporte Principal
                    reportDocument.SetDataSource(reportSource);
                    // llena el subreporte de los resultados
                    reportDocument.Subreports[0].SetDataSource(reportSourceSummary);
                }

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "BalanceCheking");

                //// Clear all sessions value
                TempData["BalanceCheking"] = null;
                TempData["BalanceChekingName"] = null;
                TempData["SumaryBalanceCheking"] = null;
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// LoadBalanceChekingReportExcel
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public void LoadBalanceChekingReportExcel(DateTime dateFrom, DateTime dateTo)
        {
            DateTime dateTimeTo = dateTo.Add(new TimeSpan(23, 59, 59));
            List<BalanceCheckingDTO>  balanceCheckingReports = DelegateService.glAccountingApplicationService.GetBalanceCheckingDTO(dateFrom, dateTimeTo);
            MemoryStream memoryStream = GetCurrentAccountStringBuilder(balanceCheckingReports, dateFrom, dateTimeTo);
            TempData["BalanceChekingReportExcel"] = memoryStream;
        }

        /// <summary>
        /// GetCurrentAccountStringBuilder
        /// </summary>
        /// <param name="balanceCheckingReports"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        private MemoryStream GetCurrentAccountStringBuilder(List<BalanceCheckingDTO> balanceCheckingReports, DateTime dateFrom, DateTime dateTo)
        {
            decimal totalSumCredit = 0;
            decimal totalSumDebit = 0;
            decimal totalCreditbalance = 0;
            decimal totalDebitbalance = 0;
            decimal totalAssets = 0;
            decimal totalLiabilities = 0;
            decimal equity = 0;
            decimal totalIncome = 0;
            decimal totalCost = 0;
            decimal totalSpending = 0;
            decimal totalEquity = 0;
            decimal capital = 0;
            decimal grossIncome = 0;
            decimal totalUtilities = 0;

            var workbook = new XSSFWorkbook();


            var sheet = workbook.CreateSheet();
            var header = sheet.CreateRow(11);
            var company = sheet.CreateRow(9);

            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
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
            
            ICellStyle styleline = workbook.CreateCellStyle();
            styleline.SetFont(fontdetail);
            styleline.BottomBorderColor = HSSFColor.Black.Index;
            styleline.BorderBottom = BorderStyle.Thin;
            styleline.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            
            ICellStyle styledoubleline = workbook.CreateCellStyle();
            styledoubleline.SetFont(fontdetail);
            styledoubleline.BottomBorderColor = HSSFColor.Black.Index;
            styledoubleline.BorderBottom = BorderStyle.Double;
            styledoubleline.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");

            ICellStyle styleletter = workbook.CreateCellStyle();
            styleletter.SetFont(fontdetail);
            styleletter.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            company.CreateCell(4).SetCellValue("EMPRESA : SISTRAN ANDINA");
            header.CreateCell(4).SetCellValue("BALANCE DE COMPROBACION");

            var ReportDate = sheet.CreateRow(18);
            ReportDate.CreateCell(2).SetCellValue("FECHA:");
            ReportDate.GetCell(2).CellStyle = styleletter;
            
            ReportDate.CreateCell(3).SetCellValue(dateFrom.ToShortDateString() + " al " + dateTo.ToShortDateString());
            ReportDate.GetCell(3).CellStyle = styleletter;
            
            var headerRow = sheet.CreateRow(19);

            headerRow.CreateCell(2).SetCellValue("CTA.CBLE");
            headerRow.CreateCell(3).SetCellValue("DESCRIPCION");
            headerRow.CreateCell(4).SetCellValue("SUMA DEBITOS");
            headerRow.CreateCell(5).SetCellValue("SUMA CREDITOS");
            headerRow.CreateCell(6).SetCellValue("SALDO DEUDOR");
            headerRow.CreateCell(7).SetCellValue("SALDO ACREEDOR");
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
            headerRow.GetCell(6).CellStyle = styleHeader;
            headerRow.GetCell(7).CellStyle = styleHeader;
            
            int rowNumber = 20;
            foreach (BalanceCheckingDTO item in balanceCheckingReports)
            {
                var row = sheet.CreateRow(rowNumber++);

                row.CreateCell(2).SetCellValue(item.AccountingAccount);
                row.GetCell(2).CellStyle = styledetalle;
                row.CreateCell(3).SetCellValue(item.DescriptionAccount);
                row.GetCell(3).CellStyle = styledetalle;
                row.CreateCell(4).SetCellValue((double)item.SumDebit);
                row.GetCell(4).CellStyle = styledetalle;
                row.CreateCell(5).SetCellValue((double)item.SumCredit);
                row.GetCell(5).CellStyle = styledetalle;

                if ((double)item.Debitbalance > 0)
                {
                    row.CreateCell(6).SetCellValue((double)item.Debitbalance);
                    row.CreateCell(7).SetCellValue((double)0);
                    totalDebitbalance = totalDebitbalance + item.Debitbalance;
                }
                else
                {
                    row.CreateCell(6).SetCellValue((double)0);
                    row.CreateCell(7).SetCellValue((double)item.Creditbalance);
                    totalCreditbalance = totalCreditbalance + item.Creditbalance;
                }

                row.GetCell(6).CellStyle = styledetalle;
                row.GetCell(7).CellStyle = styledetalle;

                totalSumCredit = totalSumCredit + item.SumCredit;
                totalSumDebit = totalSumDebit + item.SumDebit;

                //calculos del resumen del balance
                //suma de activos
                if (item.AccountingAccount.Substring(0, 1) == "1")
                {
                    totalAssets = totalAssets + item.SumDebit - item.SumCredit;
                }
                //suma pasivos
                if (item.AccountingAccount.Substring(0, 1) == "2")
                {
                    totalLiabilities = totalLiabilities + item.SumDebit - item.SumCredit;
                }
                //suma patrimonio
                if (item.AccountingAccount.Substring(0, 1) == "3")
                {
                    equity = equity + item.SumDebit - item.SumCredit;
                }
                // suma ingresos
                if (item.AccountingAccount.Substring(0, 1) == "4")
                {
                    totalIncome = totalIncome + item.SumDebit - item.SumCredit;
                }
                // suma costos
                if (item.AccountingAccount.Substring(0, 1) == "5")
                {
                    totalCost = totalCost + item.SumDebit - item.SumCredit;
                }
                //suma gastos
                if (item.AccountingAccount.Substring(0, 1) == "6")
                {
                    totalSpending = totalSpending + item.SumDebit - item.SumCredit;
                }
            }

            //ingreso bruto
            grossIncome = totalIncome - totalCost;
            //total utilidades
            totalUtilities = grossIncome - totalSpending;
            //total patrimonio
            totalEquity = equity + totalUtilities;
            // capital Contable
            capital = totalLiabilities + totalEquity;

            //resumen del balance
            SumaryReportChekingModel summaryReportChekingModel = new SumaryReportChekingModel();
            summaryReportChekingModel.TotalAssets = System.Math.Round(totalAssets, 2);
            summaryReportChekingModel.TotalLiabilities = System.Math.Round(totalLiabilities, 2);
            summaryReportChekingModel.Equity = System.Math.Round(equity, 2);
            summaryReportChekingModel.TotalIncome = System.Math.Round(totalIncome, 2);
            summaryReportChekingModel.TotalCost = System.Math.Round(totalCost, 2);
            summaryReportChekingModel.TotalSpending = System.Math.Round(totalSpending, 2);
            summaryReportChekingModel.GrossIncome = System.Math.Round(grossIncome, 2);
            summaryReportChekingModel.TotalUtilities = System.Math.Round(totalUtilities, 2);
            summaryReportChekingModel.TotalEquity = System.Math.Round(totalEquity, 2);
            summaryReportChekingModel.Capital = System.Math.Round(capital, 2);

            var totalrow = sheet.CreateRow(rowNumber++);

            totalrow.CreateCell(2).SetCellValue("");
            totalrow.GetCell(2).CellStyle = styledetalle;
            totalrow.CreateCell(3).SetCellValue("TOTAL");
            totalrow.GetCell(3).CellStyle = styledetalle;
            totalrow.CreateCell(4).SetCellValue((double)totalSumDebit);
            totalrow.GetCell(4).CellStyle = styledetalle;
            totalrow.CreateCell(5).SetCellValue((double)totalSumCredit);
            totalrow.GetCell(5).CellStyle = styledetalle;
            totalrow.CreateCell(6).SetCellValue((double)totalDebitbalance);
            totalrow.GetCell(6).CellStyle = styledetalle;
            totalrow.CreateCell(7).SetCellValue((double)totalCreditbalance);
            totalrow.GetCell(7).CellStyle = styledetalle;

            rowNumber = rowNumber + 2;

            var sumarybalance = sheet.CreateRow(rowNumber++);
            sumarybalance.CreateCell(3).SetCellValue("Balance General");

            rowNumber = rowNumber + 1;

            var sumaryasset = sheet.CreateRow(rowNumber++);
            sumaryasset.CreateCell(3).SetCellValue("Total Activos (a)");
            sumaryasset.GetCell(3).CellStyle = styleletter;
            sumaryasset.CreateCell(4).SetCellValue((double)summaryReportChekingModel.TotalAssets);
            sumaryasset.GetCell(4).CellStyle = styleletter;

            var sumaryliabilities = sheet.CreateRow(rowNumber++);
            sumaryliabilities.CreateCell(3).SetCellValue("Total Pasivos (b)");
            sumaryliabilities.GetCell(3).CellStyle = styleletter;
            sumaryliabilities.CreateCell(4).SetCellValue((double)summaryReportChekingModel.TotalLiabilities);
            sumaryliabilities.GetCell(4).CellStyle = styleletter;

            var sumaryequity = sheet.CreateRow(rowNumber++);
            sumaryequity.CreateCell(3).SetCellValue("Patrimonio (c)");
            sumaryequity.GetCell(3).CellStyle = styleletter;
            sumaryequity.CreateCell(4).SetCellValue((double)summaryReportChekingModel.Equity);
            sumaryequity.GetCell(4).CellStyle = styleletter;

            var sumaryProfit = sheet.CreateRow(rowNumber++);
            sumaryProfit.CreateCell(3).SetCellValue("Utilidades Retenidas (i)");
            sumaryProfit.GetCell(3).CellStyle = styleline;
            sumaryProfit.CreateCell(4).SetCellValue((double)summaryReportChekingModel.Utilities);
            sumaryProfit.GetCell(4).CellStyle = styleline;

            var sumarytotalequity = sheet.CreateRow(rowNumber++);
            sumarytotalequity.CreateCell(3).SetCellValue("Total Patrimionio (j)");
            sumarytotalequity.GetCell(3).CellStyle = styledoubleline;
            sumarytotalequity.CreateCell(4).SetCellValue((double)summaryReportChekingModel.TotalEquity);
            sumarytotalequity.GetCell(4).CellStyle = styledoubleline;

            var sumarycapital = sheet.CreateRow(rowNumber++);
            sumarycapital.CreateCell(3).SetCellValue("Capital Contable ");
            sumarycapital.GetCell(3).CellStyle = styledoubleline;
            sumarycapital.CreateCell(4).SetCellValue((double)summaryReportChekingModel.Capital);
            sumarycapital.GetCell(4).CellStyle = styledoubleline;
            
            rowNumber = rowNumber + 2;

            var sumarybalanceEarning = sheet.CreateRow(rowNumber++);
            sumarybalanceEarning.CreateCell(4).SetCellValue("Estado de Resultados");

            rowNumber = rowNumber + 1;

            var sumaryincome = sheet.CreateRow(rowNumber++);
            sumaryincome.CreateCell(3).SetCellValue("Total Ingresos / Vtas (d)");
            sumaryincome.GetCell(3).CellStyle = styleletter;
            sumaryincome.CreateCell(4).SetCellValue((double)summaryReportChekingModel.TotalIncome);
            sumaryincome.GetCell(4).CellStyle = styleletter;

            var sumarycosts = sheet.CreateRow(rowNumber++);
            sumarycosts.CreateCell(3).SetCellValue("Total Costos / Vtas (e)");
            sumarycosts.GetCell(3).CellStyle = styleline;
            sumarycosts.CreateCell(4).SetCellValue((double)summaryReportChekingModel.TotalCost);
            sumarycosts.GetCell(4).CellStyle = styleline;

            var sumarygrossincome = sheet.CreateRow(rowNumber++);
            sumarygrossincome.CreateCell(3).SetCellValue("INGRESO BRUTO (f)");
            sumarygrossincome.GetCell(3).CellStyle = styleline;
            sumarygrossincome.CreateCell(4).SetCellValue((double)summaryReportChekingModel.GrossIncome);
            sumarygrossincome.GetCell(4).CellStyle = styleline;

            var sumarytotalcosts = sheet.CreateRow(rowNumber++);
            sumarytotalcosts.CreateCell(3).SetCellValue("Total Gastos / Admin (g)");
            sumarytotalcosts.GetCell(3).CellStyle = styledoubleline;
            sumarytotalcosts.CreateCell(4).SetCellValue((double)summaryReportChekingModel.TotalSpending);
            sumarytotalcosts.GetCell(4).CellStyle = styledoubleline;

            var sumarytotalgains = sheet.CreateRow(rowNumber++);
            sumarytotalgains.CreateCell(3).SetCellValue("Total Utilidades / Pérdidas (h)");
            sumarytotalgains.GetCell(3).CellStyle = styleletter;
            sumarytotalgains.CreateCell(4).SetCellValue((double)summaryReportChekingModel.TotalUtilities);
            sumarytotalgains.GetCell(4).CellStyle = styleletter;

            rowNumber = rowNumber + 6;
            
            var sign = sheet.CreateRow(rowNumber++);

            sign.CreateCell(3).SetCellValue("ELABORADO POR:");
            sign.GetCell(3).CellStyle = styleletter;
            sign.CreateCell(5).SetCellValue("AUTORIZADO POR:");
            sign.GetCell(5).CellStyle = styleletter;
            rowNumber = rowNumber + 2;

            var lineasign = sheet.CreateRow(rowNumber);

            lineasign.CreateCell(3).CellStyle = styleline;
            lineasign.CreateCell(5).CellStyle = styleline;

            String url = System.Web.HttpContext.Current.Server.MapPath("~/") + "\\Images\\Logo.jpg";

            XSSFCreationHelper helper = workbook.GetCreationHelper() as XSSFCreationHelper;
            XSSFDrawing drawing = sheet.CreateDrawingPatriarch() as XSSFDrawing;
            XSSFClientAnchor anchor = helper.CreateClientAnchor() as XSSFClientAnchor;
            anchor.Col1 = 2;
            anchor.Row1 = 2;
            anchor.AnchorType = 5;
            XSSFPicture pict = drawing.CreatePicture(anchor, LoadImage(url, workbook)) as XSSFPicture;
            pict.Resize();
            pict.LineStyle = NPOI.SS.UserModel.LineStyle.None;

            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;
        }

        /// <summary>
        /// ReportExcel
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportExcel()
        {
            MemoryStream memoryStream = (MemoryStream)TempData["BalanceChekingReportExcel"];
            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "ReportCheking.xls");
        }
        
        #endregion BalanceChecking

    }
}