//System
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.Reflection;

//Sistran Core
using Sistran.Core.Application.TempCommonServices;
using Reporting = Sistran.Core.Application.ReportingServices.Models;

using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingClosingServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.AccountingClosing.Controllers
{
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class MassiveReportsController : Controller
    {
        #region View

        /// <summary>
        /// CancellationRecordIssuance
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CancellationRecordIssuance()
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
        /// ProductionDetailList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ProductionDetailList()
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

        #endregion

        #region General Methods

        /// <summary>
        /// GetReportProcess
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReportProcess(string reportName)
        {
            List<object> massiveReportResponse = new List<object>();

            try
            {
                List<MassiveReportDTO> massiveReports = DelegateService.accountingClosingApplicationService.GetMassiveReportsByUserIdReportName(SessionHelper.GetUserId(), reportName);
                List<MassiveReportDTO> order = (from massive in massiveReports
                                                orderby massive.EndDate descending
                                                select massive).ToList();

                if (massiveReports.Count > 0)
                {
                    double elapsed = 0;
                    double minElapsed = 0;
                    double progress = 0;
                    int index = 1;

                    foreach (MassiveReportDTO masiveReport in order)
                    {
                        if (Convert.ToDouble(masiveReport.RecordsProcessed) > 0)
                        {
                            progress = (Convert.ToDouble(masiveReport.RecordsProcessed)) / Convert.ToDouble(masiveReport.RecordsNumber);
                            if (progress < 1)
                            {
                                elapsed = System.Math.Round((DateTime.Now.TimeOfDay.TotalHours - masiveReport.StartDate.TimeOfDay.TotalHours), 2);
                                minElapsed = elapsed - System.Math.Truncate(elapsed);
                                minElapsed = minElapsed * 60;
                            }
                            else
                            {
                                elapsed = System.Math.Round((masiveReport.EndDate.TimeOfDay.TotalHours - masiveReport.StartDate.TimeOfDay.TotalHours), 2);
                                minElapsed = elapsed - System.Math.Truncate(elapsed);
                                minElapsed = minElapsed * 60;
                            }
                        }

                        massiveReportResponse.Add(new
                        {
                            Order = index,
                            ProcessId = masiveReport.Id,
                            Description = masiveReport.Description,
                            RecordsNumber = masiveReport.RecordsNumber,
                            RecordsProcessed = masiveReport.RecordsProcessed,
                            Progress = progress.ToString("P", CultureInfo.InvariantCulture),
                            Elapsed = System.Math.Truncate(elapsed) + " h " + System.Math.Truncate(minElapsed) + " m",
                            UrlFile = masiveReport.UrlFile,
                            Status = masiveReport.Success
                        });
                        index++;
                    }
                }

                return new UifTableResult(massiveReportResponse);
            }
            catch (Exception)
            {
                massiveReportResponse.Add(new
                {
                    ProcessId = -1
                });
                return new UifTableResult(massiveReportResponse);
            }
        }

        /// <summary>
        /// GenerateStructureReportMassive
        /// Crea el archivo y guarda en el servidor(metodo Asicrono no tiene pruebas Test)
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="reportTypeDescription"></param>
        /// <param name="exportFormatType"></param>
        /// <param name="recordsNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GenerateStructureReportMassive(int processId, string reportTypeDescription,
                                                         int exportFormatType, decimal recordsNumber)
        {
            List<object> reportResponse = new List<object>();

            try
            {
                reportResponse.Add(new
                {
                    ExportedFileName = DelegateService.accountingClosingApplicationService.GenerateStructureReportMassive(processId, reportTypeDescription, exportFormatType, recordsNumber, SessionHelper.GetUserId())
                });
            }
            catch (Exception ex)
            {
                reportResponse = new List<object>();
                List<string> errorMessage = new List<string>();
                errorMessage.Add(@Global.FailedToGeneratePDF + " --> " + ex.Message);

                if ((ex.InnerException != null) && (ex.InnerException.Message != null))
                {
                    errorMessage.Add(ex.InnerException.Message);
                }
                reportResponse.Add(new
                {
                    ExportedFileName = "",
                    ErrorInfo = errorMessage
                });
            }

            return Json(reportResponse, JsonRequestBehavior.AllowGet);
        }

        #region PRODUCCIÓN DE PRIMAS

        /// <summary>
        /// GetTotalRecordsProductionDetail
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsProductionDetail(string year, string month, string day)
        {
            int totRecords = 0;
            try
            {
                JsonResult report = ProductionDetailReports(year, month, day, -1);
                bool success = (bool)TypeModel.GetPropertyValue(report.Data, "success");

                if (success)
                {
                    object reportResult = report.Data;
                    totRecords = (int)TypeModel.GetPropertyValue(reportResult, "result");
                }

                var reportResponse = new
                {
                    records = totRecords
                };
                return new UifJsonResult(true, reportResponse);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// ProductionDetailReports
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ProductionDetailReports(string year, string month, string day, int? process)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.accountingClosingApplicationService.GetProductionDetailReports(year, month, day, SessionHelper.GetUserId(), process));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }
        #endregion

        #region CANCELLATION OF RECORDS

        /// <summary>
        /// GetTotalRecordsCancellationRecordIssuance
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsCancellationRecordIssuance(string year, string month, string day)
        {
            int totRecords = 0;
            try
            {
                JsonResult report = CancellationRecordIssuanceReports(year, month, day, -1);
                bool success = (bool)TypeModel.GetPropertyValue(report.Data, "success");

                if (success)
                {
                    object reportResult = report.Data;
                    totRecords = (int)TypeModel.GetPropertyValue(reportResult, "result");
                }

                var reportResponse = new
                {
                    records = totRecords
                };
                return new UifJsonResult(true, reportResponse);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// CancellationRecordIssuanceReports
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CancellationRecordIssuanceReports(string year, string month, string day, int? process)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.accountingClosingApplicationService.GetCancellationRecordIssuanceReports(year, month, day, SessionHelper.GetUserId(), process));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }
        #endregion

        #endregion

    }
}