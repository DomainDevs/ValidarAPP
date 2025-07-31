using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
//Crystal
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.AccountReclassification;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
using ReclassificationModels = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountReclassification;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Framework.UIF.Web.Helpers;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    public class AccountReclassificationController : Controller
    {
        #region Constants

        public const string PathTemp = "~/Temp";

        #endregion

        #region Instance Variables
        readonly BaseController _baseController = new BaseController();

        #endregion

        #region View

        /// <summary>
        /// MainAccountReclassification
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainAccountReclassification()
        {

            try
            {

                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                ViewBag.Year = DateTime.Now.Year;
                ViewBag.Month = DateTime.Now.Month;

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainGenerationReclassification
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainGenerationReclassification()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                ViewBag.Year = DateTime.Now.Year;
                ViewBag.Month = DateTime.Now.Month;

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }      
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetOpeningSelect
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetOpeningSelect()
        {
            var openings = new List<object>();

            openings.Add(new
            {
                Id = 0,
                Description = @Global.No
            });
            openings.Add(new
            {
                Id = 1,
                Description = @Global.Yes
            });

            return new UifSelectResult(openings);
        }

        /// <summary>
        /// GetMonths
        /// Carga los meses en DropDownlist
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetMonths()
        {
            var yearMonths = new List<object>();
            int monthNumber = 1;

            yearMonths.Add(new { Id = monthNumber, Description = @Global.January });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.February });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.March });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.April });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.May });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.June });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.July });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.August });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.September });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.October });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.November });
            yearMonths.Add(new { Id = monthNumber++, Description = @Global.December });

            return new UifSelectResult(yearMonths);

        }

        /// <summary>
        /// GetAccountingReclassificationByMonthAndYear
        /// Obtiene la parametrización reclasificación contable por mes y año
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAccountingReclassificationByMonthAndYear(int month, int year)
        {
            try
            {
                List<ReclassificationModels.AccountReclassificationDTO> accountReclassifications = new List<ReclassificationModels.AccountReclassificationDTO>();

                accountReclassifications = DelegateService.glAccountingApplicationService.GetAccountReclassification(month, year);

                var accountingReclassification = from items in accountReclassifications
                                                 select new
                                                 {
                                                     Year = items.Year,
                                                     Month = items.Month,
                                                     AccountingAccountOriginId = items.SourceAccountingAccount.AccountingAccountId,
                                                     AccountingAccountOrigin = items.SourceAccountingAccount.Number,
                                                     AccountingAccountOriginName = items.SourceAccountingAccount.Description,
                                                     AccountingAccountDestinationId = items.DestinationAccountingAccount.AccountingAccountId,
                                                     AccountingAccountDestination = items.DestinationAccountingAccount.Number,
                                                     AccountingAccountDestinationName = items.DestinationAccountingAccount.Description,
                                                     PrefixOpening = items.OpeningPrefix ? "Si" : "No",
                                                     PrefixOpeningId = items.OpeningPrefix ? 1 : 0,
                                                     BranchOpening = items.OpeningBranch ? "Si" : "No",
                                                     BranchOpeningId = items.OpeningBranch ? 1 : 0
                                                 };

                return new UifTableResult(accountingReclassification);
            }
            catch (Exception exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetAccountingAccountByNumber
        /// Obtiene los datos de la cuenta contable a partir del número
        /// </summary>
        /// <param name="query"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingAccountByNumber(string query)
        {
            AccountingAccountDTO accountingAccount = new AccountingAccountDTO()
            {
                Description = "",
                Number = query
            };

            var accountingAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccountsByNumberDescription(accountingAccount);
            var jsonData = (from result in accountingAccounts
                            select new
                            {
                                result.AccountingAccountId,
                                AccountingNumber = result.Number.Trim(),
                                result.Description
                            }).Take(10);

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveAccountingReclassification
        /// Graba / actualiza la parametrización de reclasificación de cuentas
        /// </summary>
        /// <param name="accountingReclassificationModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveAccountingReclassification(AccountingReclassificationModel accountingReclassificationModel, string operationType)
        {
            try
            {
                List<object> accountingReclassifications = new List<object>();
                ReclassificationModels.AccountReclassificationDTO accountReclassification = new ReclassificationModels.AccountReclassificationDTO()
                {
                    DestinationAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = accountingReclassificationModel.AccountingAccountDestinationId,
                        Description = "",
                        Number = accountingReclassificationModel.AccountingAccountDestination
                    },
                    Id = 0,
                    Month = accountingReclassificationModel.Month,
                    OpeningBranch = accountingReclassificationModel.BranchOpening == "1",
                    OpeningPrefix = accountingReclassificationModel.PrefixOpening == "1",
                    SourceAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = accountingReclassificationModel.AccountingAccountOriginId,
                        Description = "",
                        Number = accountingReclassificationModel.AccountingAccountOrigin,
                    },
                    Year = accountingReclassificationModel.Year
                };

                if (operationType.Equals("I"))
                {
                    accountReclassification = DelegateService.glAccountingApplicationService.SaveAccountReclassification(accountReclassification);
                }
                if (operationType.Equals("U"))
                {
                    accountReclassification = DelegateService.glAccountingApplicationService.UpdateAccountReclassification(accountReclassification);
                }

                accountingReclassifications.Add(new
                {
                    AccountingReclassificationCode = accountReclassification.Id,
                    MessageError = accountReclassification.SourceAccountingAccount.Description
                });

                return Json(new { success = true, result = accountingReclassifications }, JsonRequestBehavior.AllowGet);
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
        /// DeleteAccountingReclassification
        /// Borra una parametrización de reclasificación de cuentas
        /// </summary>
        /// <param name="accountingReclassificationModel"></param>
        /// <param name="operationType"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteAccountingReclassification(AccountingReclassificationModel accountingReclassificationModel, string operationType)
        {
            try
            {
                List<object> accountingReclassifications = new List<object>();

                ReclassificationModels.AccountReclassificationDTO accountReclassification = new ReclassificationModels.AccountReclassificationDTO()
                {
                    DestinationAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = accountingReclassificationModel.AccountingAccountDestinationId,
                        Description = "",
                        Number = accountingReclassificationModel.AccountingAccountDestination
                    },
                    Id = 0,
                    Month = accountingReclassificationModel.Month,
                    OpeningBranch = accountingReclassificationModel.BranchOpening == "1",
                    OpeningPrefix = accountingReclassificationModel.PrefixOpening == "1",
                    SourceAccountingAccount = new AccountingAccountDTO()
                    {
                        AccountingAccountId = accountingReclassificationModel.AccountingAccountOriginId,
                        Description = "",
                        Number = accountingReclassificationModel.AccountingAccountDestination,
                    },
                    Year = accountingReclassificationModel.Year
                };

                if (operationType.Equals("D"))
                {
                    DelegateService.glAccountingApplicationService.DeleteAccountReclassification(accountReclassification);
                }

                accountingReclassifications.Add(new
                {
                    AccountingReclassificationCode = 0,
                    MessageError = Global.YouCanNotDeleteTheRecord //"No se puede eliminar el registro, ya que tiene dependencias"
                });

                return Json(new { success = true, result = accountingReclassifications }, JsonRequestBehavior.AllowGet);
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
        /// GenerateAccountingReclassification
        /// Genera el listado de registro del proceso reclasificación contable por mes y año
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>Task<JsonResult></returns>
        public async Task<JsonResult> GenerateAccountingReclassification(int month, int year)
        {
            var isGenerated = 1;
            try
            {
                isGenerated = await Task.FromResult(DelegateService.glAccountingApplicationService.GenerateEntryReclassification(month, year));
                return Json(new { success = true, result = isGenerated.ToString() }, JsonRequestBehavior.AllowGet);
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
        /// GetReclassificationStatus
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetReclassificationStatus(int month, int year)
        {
            var reclassifications = new List<object>();
            double hourElapsed = 0;
            double minuteElapsed = 0;
            double dayElapsed = 0;

            DateTime startDate;
            DateTime endDate;

            ProcessLogDTO processLog = new ProcessLogDTO()
            {
                Month = month,
                UserId = SessionHelper.GetUserId(),
                Year = year,
            };

            processLog = DelegateService.glAccountingApplicationService.GetProcessLog(processLog);

            if (processLog.Id > 0)
            {
                startDate = Convert.ToDateTime(processLog.StartDate);
                endDate = Convert.ToDateTime(processLog.EndDate);

                TimeSpan timeSpan = new TimeSpan();

                if (processLog.ProcessLogStatus == (int)ProcessLogStatus.Started)
                {
                    timeSpan = DateTime.Now - startDate;

                    if (timeSpan.TotalDays >= 1)
                    {
                        dayElapsed = timeSpan.Days;
                        hourElapsed = timeSpan.Hours;
                        minuteElapsed = timeSpan.Minutes;
                    }
                    else
                    {
                        dayElapsed = System.Math.Round((DateTime.Now.TimeOfDay.TotalDays - startDate.TimeOfDay.TotalDays), 2);
                        hourElapsed = timeSpan.TotalHours;
                        minuteElapsed = timeSpan.Minutes;
                    }
                }
                else
                {
                    timeSpan = endDate - startDate;

                    if (timeSpan.TotalDays >= 1)
                    {
                        dayElapsed = timeSpan.TotalDays;
                        hourElapsed = System.Math.Round((endDate.TimeOfDay.TotalHours - startDate.TimeOfDay.TotalHours), 2);
                        minuteElapsed = hourElapsed - System.Math.Truncate(hourElapsed);
                        minuteElapsed = System.Math.Ceiling(minuteElapsed * 60);
                    }
                    else
                    {
                        dayElapsed = System.Math.Round((endDate.TimeOfDay.TotalHours - startDate.TimeOfDay.TotalHours), 2);
                        hourElapsed = System.Math.Round((endDate.TimeOfDay.TotalHours - startDate.TimeOfDay.TotalHours), 2);
                        minuteElapsed = hourElapsed - System.Math.Truncate(hourElapsed);
                        minuteElapsed = System.Math.Ceiling(minuteElapsed * 60);
                    }
                }

                string processLogStatus = "";
                if(processLog.ProcessLogStatus == (int)ProcessLogStatus.Started)
                {
                    processLogStatus = Global.Started;
                }
                else if (processLog.ProcessLogStatus == (int)ProcessLogStatus.Finished)
                {
                    processLogStatus = Global.Finalized;
                }
                else
                {
                    processLogStatus = Global.Accounted;
                }


                reclassifications.Add(new
                {
                    Year = processLog.Year,
                    Month = GetMonthNameById(processLog.Month), 
                    StartDate = processLog.StartDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    EndDate = processLog.EndDate == Convert.ToDateTime("01/01/0001 0:00:00") ? "" : processLog.EndDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    Delay = System.Math.Truncate(dayElapsed) + " d " + System.Math.Truncate(hourElapsed) + " h " + System.Math.Truncate(minuteElapsed) + " m",
                    InProgress = processLogStatus
                });

                return new UifTableResult(reclassifications);
            }
            else
            {
                return new UifTableResult(new object());
            }
        }

        /// <summary>
        /// GenerateReclassificationReport
        /// Llena el reporte con los datos del registro del proceso de reclasificación
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>int</returns>
        public int GenerateReclassificationReport(int month, int year)
        {
            List<ReclassificationModel> reclassifications = new List<ReclassificationModel>();

            var accountingReclasifications = DelegateService.glAccountingApplicationService.GetAccountReclassificationResults(month, year/*, user*/);

            foreach (ReclassificationModels.AccountReclassificationResultDTO reclassificationResult in accountingReclasifications)
            {
                ReclassificationModel reclassification = new ReclassificationModel();

                reclassification.Amount = reclassificationResult.LocalAmount.Value;
                reclassification.AnalysisDescription = reclassificationResult.Analysis.Description;
                reclassification.AnalysisId = reclassificationResult.Analysis.AnalysisId;
                reclassification.AnalysisConceptDescription = reclassificationResult.Analysis.AnalysisConcept.Description;
                reclassification.AnalysisConceptId = reclassificationResult.Analysis.AnalysisConcept.AnalysisConceptId;
                reclassification.BranchDescription = _baseController.GetBranchDescriptionById(Convert.ToInt32(reclassificationResult.Branch.Id), SessionHelper.GetUserName().ToUpper());
                reclassification.BranchId = reclassificationResult.Branch.Id;
                reclassification.CostCenterDescription = reclassificationResult.CostCenter.Description;
                reclassification.CostCenterId = reclassificationResult.CostCenter.CostCenterId;
                reclassification.CurrencyDescription = _baseController.GetCurrencyDescriptionById(reclassificationResult.Amount.Currency.Id);
                reclassification.CurrencyId = reclassificationResult.Amount.Currency.Id;
                reclassification.DestinationAccountingAccountDescription = reclassificationResult.DestinationAccountingAccount.Description;
                reclassification.DestinationAccountingAccountNumber = reclassificationResult.DestinationAccountingAccount.Number;
                reclassification.ExchangeRate = reclassificationResult.ExchangeRate.SellAmount;
                reclassification.IncomeAmount = reclassificationResult.Amount.Value;
                reclassification.Month = reclassificationResult.Month;
                reclassification.Nature = reclassificationResult.AccountingNature == (int)AccountingNatures.Credit ? "C" : "D";
                reclassification.SourceAccountingAccountDescription = reclassificationResult.SourceAccountingAccount.Description;
                reclassification.SourceAccountingAccountNumber = reclassificationResult.SourceAccountingAccount.Number;
                reclassification.Year = reclassificationResult.Year;

                reclassifications.Add(reclassification);
            }

            TempData["ReclassificationSource"] = reclassifications;
            TempData["ReclassificationReportName"] = "Areas//GeneralLedger//Reports//ReclassificationProcess.rpt";

            return 1;
        }

        /// <summary>
        /// GenerateReclassificationToExcel
        /// </summary>
        /// <param name="month">Mes de proceso</param>
        /// <param name="year">Año de proceso</param>
        /// <param name="monthName">Nombre mes de proceso</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GenerateReclassificationToExcel(int month, int year, string monthName)
        {
            try
            {
                // Se obtiene la data
                GenerateReclassificationReport(month, year);
                ExportOptions exportOptions = new ExportOptions();
                DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();

                var fileName = "ListadoReclasificacion_MesAño" + month.ToString() + year.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
                string fullPath = Path.Combine(Server.MapPath(PathTemp), fileName);

                bool isValid = true;

                var reportSource = TempData["ReclassificationSource"];
                var reportName = TempData["ReclassificationReportName"];
                var title = "REGISTRO DE RECLASIFICACIÓN DEL MES DE " + monthName.ToUpper() + " DE " + year.ToString();

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);

                    reportDocument.SetDataSource(reportSource);
                    reportDocument.DataDefinition.FormulaFields["Month"].Text = "'" + title + "'";

                    exportOptions.ExportFormatType = ExportFormatType.Excel;
                    diskFileDestinationOptions.DiskFileName = fullPath;
                    exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    exportOptions.ExportDestinationOptions = diskFileDestinationOptions;

                    reportDocument.Export(exportOptions);

                    TempData["ReclassificationSource"] = null;
                    TempData["ReclassificationReportName"] = null;
                }

                // Return the Excel file name
                return Json(new { fileName = fileName, errorMessage = "" });
            }
            catch (Exception ex)
            {
                return Json(new { fileName = "", errorMessage = ex.Message });
            }
        }

        /// <summary>
        /// ShowReclassificationReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="monthName"></param>
        /// <returns></returns>
        public void ShowReclassificationReport(int month, int year, string monthName)
        {
            try
            {
                List<ReclassificationModel> reclassifications = new List<ReclassificationModel>();
                var accountingReclasifications = DelegateService.glAccountingApplicationService.GetAccountReclassificationResults(month, year/*, user*/);
                foreach (ReclassificationModels.AccountReclassificationResultDTO reclassificationResult in accountingReclasifications)
                {
                    ReclassificationModel reclassification = new ReclassificationModel();

                    reclassification.Amount = reclassificationResult.LocalAmount.Value;
                    reclassification.AnalysisDescription = reclassificationResult.Analysis.Description;
                    reclassification.AnalysisId = reclassificationResult.Analysis.AnalysisId;
                    reclassification.AnalysisConceptDescription = reclassificationResult.Analysis.AnalysisConcept.Description;
                    reclassification.AnalysisConceptId = reclassificationResult.Analysis.AnalysisConcept.AnalysisConceptId;
                    reclassification.BranchDescription = _baseController.GetBranchDescriptionById(Convert.ToInt32(reclassificationResult.Branch.Id), SessionHelper.GetUserName().ToUpper());
                    reclassification.BranchId = reclassificationResult.Branch.Id;
                    reclassification.CostCenterDescription = reclassificationResult.CostCenter.Description;
                    reclassification.CostCenterId = reclassificationResult.CostCenter.CostCenterId;
                    reclassification.CurrencyDescription = _baseController.GetCurrencyDescriptionById(reclassificationResult.Amount.Currency.Id);
                    reclassification.CurrencyId = reclassificationResult.Amount.Currency.Id;
                    reclassification.DestinationAccountingAccountDescription = reclassificationResult.DestinationAccountingAccount.Description;
                    reclassification.DestinationAccountingAccountNumber = reclassificationResult.DestinationAccountingAccount.Number;
                    reclassification.ExchangeRate = reclassificationResult.ExchangeRate.SellAmount;
                    reclassification.IncomeAmount = reclassificationResult.Amount.Value;
                    reclassification.Month = reclassificationResult.Month;
                    reclassification.Nature = reclassificationResult.AccountingNature == (int)AccountingNatures.Credit ? "C" : "D";
                    reclassification.SourceAccountingAccountDescription = reclassificationResult.SourceAccountingAccount.Description;
                    reclassification.SourceAccountingAccountNumber = reclassificationResult.SourceAccountingAccount.Number;
                    reclassification.Year = reclassificationResult.Year;

                    reclassifications.Add(reclassification);
                }

                var reportDocument = new ReportDocument();
                var reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Areas//GeneralLedger//Reports//ReclassificationProcess.rpt";

                reportDocument.Load(reportPath);

                var title = "REGISTRO DE RECLASIFICACIÓN DEL MES DE " + monthName.ToUpper() + " DE " + year.ToString();
                //Llena Reporte Principal
                reportDocument.SetDataSource(reclassifications);
                reportDocument.DataDefinition.FormulaFields["Month"].Text = "'" + title + "'";
                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ProcessLogListReport");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// Download
        /// Permite descargar el archivo
        /// </summary>
        /// <param name="file">Nombre del Archivo</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [DeleteFileAttribute] //Action Filter, it will auto delete the file after download, 
        public ActionResult Download(string file)
        {
            // Get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath(PathTemp), file);

            // Return the file for download, this is an Excel 
            // so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", file);
        }


        /// <summary>
        /// GenerateReclassificationEntryReport
        /// Llena el reporte con los datos del asiento del proceso de reclasificación
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>int</returns>
        public int GenerateReclassificationEntryReport(int month, int year)
        {
            List<ReclassificationEntryModel> reclassificationEntries = new List<ReclassificationEntryModel>();
            ReclassificationEntryModel reclassificationEntry = new ReclassificationEntryModel();

            var accountingReclasifications = DelegateService.glAccountingApplicationService.GetAccountReclassificationResults(month, year);

            if (accountingReclasifications.Count > 0)
            {
                foreach (ReclassificationModels.AccountReclassificationResultDTO reclassificationResult in accountingReclasifications)
                {
                    reclassificationEntry = new ReclassificationEntryModel();

                    reclassificationEntry.Amount = reclassificationResult.LocalAmount.Value;
                    reclassificationEntry.BranchDescription = _baseController.GetBranchDescriptionById(Convert.ToInt32(reclassificationResult.Branch.Id), SessionHelper.GetUserName().ToUpper());
                    reclassificationEntry.BranchId = reclassificationResult.Branch.Id;
                    reclassificationEntry.CurrencyDescription = _baseController.GetCurrencyDescriptionById(reclassificationResult.Amount.Currency.Id);
                    reclassificationEntry.CurrencyId = reclassificationResult.Amount.Currency.Id;
                    reclassificationEntry.DestinationAccountingAccountDescription = reclassificationResult.DestinationAccountingAccount.Description;
                    reclassificationEntry.DestinationAccountingAccountNumber = reclassificationResult.DestinationAccountingAccount.Number;
                    reclassificationEntry.ExchangeRate = reclassificationResult.ExchangeRate.SellAmount;
                    reclassificationEntry.IncomeAmount = reclassificationResult.Amount.Value;
                    reclassificationEntry.Month = reclassificationResult.Month;
                    reclassificationEntry.Nature = reclassificationResult.AccountingNature == (int)AccountingNatures.Credit ? "C" : "D";
                    reclassificationEntry.SourceAccountingAccountDescription = reclassificationResult.SourceAccountingAccount.Description;
                    reclassificationEntry.SourceAccountingAccountNumber = reclassificationResult.SourceAccountingAccount.Number;
                    reclassificationEntry.Year = reclassificationResult.Year;

                    reclassificationEntries.Add(reclassificationEntry);
                }
            }
            else
            {
                reclassificationEntry.Amount = 0;
                reclassificationEntry.BranchDescription = "";
                reclassificationEntry.BranchId = 0;
                reclassificationEntry.CurrencyDescription = "";
                reclassificationEntry.CurrencyId = 0;
                reclassificationEntry.DestinationAccountingAccountDescription = "";
                reclassificationEntry.DestinationAccountingAccountNumber = "";
                reclassificationEntry.ExchangeRate = 0;
                reclassificationEntry.IncomeAmount = 0;
                reclassificationEntry.Month = 0;
                reclassificationEntry.Nature = "";
                reclassificationEntry.SourceAccountingAccountDescription = "";
                reclassificationEntry.SourceAccountingAccountNumber = "";
                reclassificationEntry.Year = 0;

                reclassificationEntries.Add(reclassificationEntry);
             }

            TempData["ReclassificationEntrySource"] = reclassificationEntries;
            TempData["ReclassificationEntryReportName"] = "Areas//GeneralLedger//Reports//ReclassificationEntry.rpt"; 
            
            return 1;
        }

        /// <summary>
        /// GenerateReclassificationEntryToExcel
        /// </summary>
        /// <param name="month">Mes de proceso</param>
        /// <param name="year">Año de proceso</param>
        /// <param name="monthName">Nombre mes de proceso</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GenerateReclassificationEntryToExcel(int month, int year, string monthName)
        {
            try
            {
                // Se obtiene la data
                int isRecords = GenerateReclassificationEntryReport(month, year);
                ExportOptions exportOptions = new ExportOptions();
                DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();

                var fileName = "ListadoAsientoReclasificacion_MesAño" + month.ToString() + year.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
                string fullPath = Path.Combine(Server.MapPath(PathTemp), fileName);

                bool isValid = true;

                var reportSource = TempData["ReclassificationEntrySource"];
                var reportName = TempData["ReclassificationEntryReportName"];
                var title = "ASIENTO DE RECLASIFICACIÓN DEL MES DE " + monthName.ToUpper() + " DE " + year.ToString();

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);

                    reportDocument.SetDataSource(reportSource);
                    reportDocument.DataDefinition.FormulaFields["Month"].Text = "'" + title + "'";

                    exportOptions.ExportFormatType = ExportFormatType.Excel;
                    diskFileDestinationOptions.DiskFileName = fullPath;
                    exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    exportOptions.ExportDestinationOptions = diskFileDestinationOptions;

                 	if (isRecords > 0)
                    {
                        reportDocument.Export(exportOptions);
                    }

                    TempData["ReclassificationEntrySource"] = null;
                    TempData["ReclassificationEntryReportName"] = null;
                }

                // Return the Excel file name
                return Json(new { fileName = fileName, errorMessage = "" });
            }
            catch (Exception ex)
            {
                return Json(new { fileName = "", errorMessage = ex.Message });
            }
        }

        /// <summary>
        /// ShowReclassificationEntryReport
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="monthName"></param>
        /// <returns></returns>
        public void ShowReclassificationEntryReport(int month, int year, string monthName)
        {
            try
            {
                List<ReclassificationEntryModel> reclassificationEntries = new List<ReclassificationEntryModel>();
                var accountingReclasifications = DelegateService.glAccountingApplicationService.GetAccountReclassificationResults(month, year);

                foreach (ReclassificationModels.AccountReclassificationResultDTO reclassificationResult in accountingReclasifications)
                {
                    ReclassificationEntryModel reclassificationEntry = new ReclassificationEntryModel();

                    reclassificationEntry.Amount = reclassificationResult.LocalAmount.Value;
                    reclassificationEntry.BranchDescription = _baseController.GetBranchDescriptionById(Convert.ToInt32(reclassificationResult.Branch.Id), SessionHelper.GetUserName().ToUpper());
                    reclassificationEntry.BranchId = reclassificationResult.Branch.Id;
                    reclassificationEntry.CurrencyDescription = _baseController.GetCurrencyDescriptionById(reclassificationResult.Amount.Currency.Id);
                    reclassificationEntry.CurrencyId = reclassificationResult.Amount.Currency.Id;
                    reclassificationEntry.DestinationAccountingAccountDescription = reclassificationResult.DestinationAccountingAccount.Description;
                    reclassificationEntry.DestinationAccountingAccountNumber = reclassificationResult.DestinationAccountingAccount.Number;
                    reclassificationEntry.ExchangeRate = reclassificationResult.ExchangeRate.SellAmount;
                    reclassificationEntry.IncomeAmount = reclassificationResult.Amount.Value;
                    reclassificationEntry.Month = reclassificationResult.Month;
                    reclassificationEntry.Nature = reclassificationResult.AccountingNature == (int)AccountingNatures.Credit ? "C" : "D";
                    reclassificationEntry.SourceAccountingAccountDescription = reclassificationResult.SourceAccountingAccount.Description;
                    reclassificationEntry.SourceAccountingAccountNumber = reclassificationResult.SourceAccountingAccount.Number;
                    reclassificationEntry.Year = reclassificationResult.Year;

                    reclassificationEntries.Add(reclassificationEntry);
                }

                var reportDocument = new ReportDocument();
                var reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Areas//GeneralLedger//Reports//ReclassificationEntry.rpt";

                reportDocument.Load(reportPath);

                //Llena Reporte Principal
                var title = "ASIENTO DE RECLASIFICACIÓN DEL MES DE " + monthName.ToUpper() + " DE " + year.ToString();
                reportDocument.SetDataSource(reclassificationEntries);
                reportDocument.DataDefinition.FormulaFields["Month"].Text = "'" + title + "'";

                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "ProcessLogListReport");
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// AccountingReclassificationClosure
        /// Realiza el proceso de contabilización de la reclasificación de cuentas
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AccountingReclassificationClosure(int month, int year)
        {
            List<string> entryNumbers = new List<string>();
            List<LedgerEntryDTO> ledgerEntries = new List<LedgerEntryDTO>();
            LedgerEntryDTO newLedgerEntry = new LedgerEntryDTO();
            List<ReclassificationModels.AccountReclassificationResultDTO> accountingReclasifications = new List<ReclassificationModels.AccountReclassificationResultDTO>();
            ProcessLogDTO processLog = new ProcessLogDTO();
            var entryNumber = 0;
            int userId = SessionHelper.GetUserId();
            int module = Convert.ToInt32(ConfigurationManager.AppSettings["ReclassificationModule"]);

            try
            {
                processLog.Id = -1;
                processLog.Month = month;
                processLog.ProcessLogStatus = (int)ProcessLogStatus.Canceled;
                processLog.UserId = SessionHelper.GetUserId();
                processLog.Year = year;

                // Se obtiene los parámetros para generar el asiento
                accountingReclasifications = DelegateService.glAccountingApplicationService.GetAccountReclassificationResults(month, year);

                if (accountingReclasifications.Count > 0)
                {
                    // Se agrupa por sucursal y moneda
                    List<ReclassificationModels.AccountReclassificationResultDTO> reclassifications = accountingReclasifications.GroupBy(p => new { filter = p.Branch.Id, p.Amount.Currency.Id }).Select(g => g.First()).ToList();

                    foreach (var reclassification in reclassifications)
                    {
                        // Se filtra por sucursal y moneda
                        List<ReclassificationModels.AccountReclassificationResultDTO> accountingList = (from ReclassificationModels.AccountReclassificationResultDTO item in accountingReclasifications
                                          where item.Branch.Id == reclassification.Branch.Id &&
                                                item.Amount.Currency.Id == reclassification.Amount.Currency.Id
                                          select item).ToList();

                        LedgerEntryDTO ledgerEntry = GenerateEntryReclassification(accountingList);

                        ledgerEntries.Add(ledgerEntry);
                    }

                    newLedgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

                    foreach (LedgerEntryDTO ledgerEntry in ledgerEntries)
                    {
                        newLedgerEntry.AccountingCompany = ledgerEntry.AccountingCompany;
                        newLedgerEntry.AccountingDate = ledgerEntry.AccountingDate;
                        newLedgerEntry.AccountingMovementType = ledgerEntry.AccountingMovementType;
                        newLedgerEntry.Branch = ledgerEntry.Branch;
                        newLedgerEntry.Description = ledgerEntry.Description;
                        newLedgerEntry.EntryDestination = ledgerEntry.EntryDestination;
                        newLedgerEntry.EntryNumber = ledgerEntry.EntryNumber;
                        newLedgerEntry.Id = 0;

                        foreach (LedgerEntryItemDTO ledgerEntryItem in ledgerEntry.LedgerEntryItems)
                        {
                            newLedgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
                        }

                        newLedgerEntry.ModuleDateId = ledgerEntry.ModuleDateId;
                        newLedgerEntry.RegisterDate = ledgerEntry.RegisterDate;
                        newLedgerEntry.SalePoint = ledgerEntry.SalePoint;
                        newLedgerEntry.Status = ledgerEntry.Status;
                        newLedgerEntry.UserId = ledgerEntry.UserId;
                    }

                    // Se valida débitos y créditos
                    decimal debits = 0;
                    decimal credits = 0;

                    foreach (LedgerEntryItemDTO ledgerEntryItem in newLedgerEntry.LedgerEntryItems)
                    {
                        if (ledgerEntryItem.AccountingNature == (int)AccountingNatures.Debit)
                        {
                            debits = debits + ledgerEntryItem.LocalAmount.Value;
                        }
                        else
                        {
                            credits = credits + ledgerEntryItem.LocalAmount.Value;
                        }
                    }

                    if (debits == credits)
                    {
                        if (newLedgerEntry.LedgerEntryItems.Count > 10)
                        {
                            // Se borra los datos de la tabla temporal de trabajo
                            DelegateService.glAccountingApplicationService.ClearTempAccountEntry();
                            DelegateService.glAccountingApplicationService.SaveTempEntryItem(newLedgerEntry, module, false, userId);
                            entryNumber = DelegateService.glAccountingApplicationService.SaveTempEntry(module, 0, "", userId); // isDailyEntry va en verdadero porque es un asiento de diario, isEntryRevertion va en falso porque no es una reversión
                        }
                        else
                        {
                            entryNumber = DelegateService.glAccountingApplicationService.SaveLedgerEntry(newLedgerEntry); 
                        }

                        if (entryNumber > 0)
                        {
                            entryNumbers.Add(" " + entryNumber);
                            DelegateService.glAccountingApplicationService.UpdateProcessLog(processLog);
                        }
                        else
                        {
                            entryNumbers.Add(Global.EntryRecordingError);
                        }
                    }
                }

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

        #endregion

        #region Private Methods


        /// <summary>
        /// AccountinReclassificationCount
        /// Obtener la cantidad de registros que hay en el proceso de reclasificacion de cuenta
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AccountinReclassificationCount(int month, int year)
        {
            var accountingReclasifications = DelegateService.glAccountingApplicationService.GetAccountReclassificationResults(month, year);
            return Json(accountingReclasifications.Count, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetMonthNameById
        /// Obtiene el nombre del mes
        /// </summary>
        /// <param name="id"></param>
        /// <returns>string</returns>
        private string GetMonthNameById(int id)
        {
            string month = "";

            switch (id)
            {
                case 1:
                    month = Global.January;
                    break;
                case 2:
                    month = Global.February;
                    break;
                case 3:
                    month = Global.March;
                    break;
                case 4:
                    month = Global.April;
                    break;
                case 5:
                    month = Global.May;
                    break;
                case 6:
                    month = Global.June;
                    break;
                case 7:
                    month = Global.July;
                    break;
                case 8:
                    month = Global.August;
                    break;
                case 9:
                    month = Global.September;
                    break;
                case 10:
                    month = Global.October;
                    break;
                case 11:
                    month = Global.November;
                    break;
                case 12:
                    month = Global.December;
                    break;
            }

            return month;
        }

        /// <summary>
        /// GenerateEntryReclassification
        /// Genera los asientos para la reclasificación de cuentas contables.
        /// </summary>
        /// <param name="accountReclassifications"></param>
        /// <returns>LedgerEntry</returns>
        private LedgerEntryDTO GenerateEntryReclassification(List<ReclassificationModels.AccountReclassificationResultDTO> accountReclassifications)
        {
            int moduleId = Convert.ToInt32(ConfigurationManager.AppSettings["ReclassificationModule"]);
            LedgerEntryDTO ledgerEntry = new LedgerEntryDTO();
            List<CostCenterDTO> costCenters = new List<CostCenterDTO>();
            List<AnalysisDTO> analysis = new List<AnalysisDTO>();

            ledgerEntry.LedgerEntryItems = new List<LedgerEntryItemDTO>();

            // Se arma los movimientos para los tipos de componentes
            foreach (ReclassificationModels.AccountReclassificationResultDTO item in accountReclassifications)
            {
                int accountingCompanyId = 0;

                accountingCompanyId = (from accountingCompanyItem in DelegateService.glAccountingApplicationService.GetAccountingCompanies() where accountingCompanyItem.Default == true select accountingCompanyItem).ToList()[0].AccountingCompanyId;

                // Cabecera
                ledgerEntry.AccountingCompany = new AccountingCompanyDTO { AccountingCompanyId = accountingCompanyId };
                ledgerEntry.AccountingDate = _baseController.GetAccountingDateByModule(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]));
                ledgerEntry.AccountingMovementType = new AccountingMovementTypeDTO()
                {
                    AccountingMovementTypeId = Convert.ToInt32(ConfigurationManager.AppSettings["AutomaticEntries"])
                };
                ledgerEntry.Branch = new BranchDTO() { Id = item.Branch.Id };
                ledgerEntry.Description = Global.AccountingReclassification;
                ledgerEntry.EntryDestination = new EntryDestinationDTO();
                ledgerEntry.EntryDestination.DestinationId = Convert.ToInt32(ConfigurationManager.AppSettings["DestinationLocal"]); //todo: Revisar que destino tiene la reclasificación.
                ledgerEntry.Id = 0;
                ledgerEntry.ModuleDateId = moduleId;
                ledgerEntry.RegisterDate = DateTime.Now;
                ledgerEntry.SalePoint = new SalePointDTO() { Id = 0 }; //no existe este dato en ingreso de caja
                ledgerEntry.Status = 1;       //activo
                ledgerEntry.UserId = SessionHelper.GetUserId();


                // Los movimientos no se calculan por reglas, estos vienen directo de la consulta.
                AccountingAccountDTO accountingAccount = new AccountingAccountDTO();
                accountingAccount.AccountingAccountId = item.DestinationAccountingAccount.AccountingAccountId;

                LedgerEntryItemDTO ledgerEntryItem = new LedgerEntryItemDTO();
                ledgerEntryItem.AccountingAccount = accountingAccount;
                ledgerEntryItem.AccountingNature = item.AccountingNature;
                ledgerEntryItem.Amount = new AmountDTO() { Value = item.Amount.Value };
                ledgerEntryItem.Amount.Currency = new CurrencyDTO();
                ledgerEntryItem.Amount.Currency.Id = item.Amount.Currency.Id;
                ledgerEntryItem.ExchangeRate = new ExchangeRateDTO() { SellAmount = item.ExchangeRate.SellAmount };
                ledgerEntryItem.LocalAmount = new AmountDTO() { Value = item.LocalAmount.Value };

                ledgerEntryItem.Analysis = new List<AnalysisDTO>();
                if (item.Analysis.AnalysisId > 0)
                {
                    analysis.Add(new AnalysisDTO()
                    {
                        AnalysisId = item.Analysis.AnalysisId,
                        AnalysisConcept = new AnalysisConceptDTO()
                        {
                            AnalysisConceptId = item.Analysis.AnalysisConcept.AnalysisConceptId,
                            AnalysisCode = new AnalysisCodeDTO()
                            {
                                AnalysisCodeId = item.Analysis.AnalysisConcept.AnalysisConceptId
                            }
                        },
                        Description = item.Analysis.Description,
                        ConceptKey = item.Analysis.ConceptKey
                    });
                    ledgerEntryItem.Analysis = analysis;
                }

                ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementTypeDTO();

                ledgerEntryItem.CostCenters = new List<CostCenterDTO>();
                if (item.CostCenter.CostCenterId > 0)
                {
                    costCenters.Add(new CostCenterDTO()
                    {
                        CostCenterId = item.CostCenter.CostCenterId,
                        PercentageAmount = 100
                    });
                    ledgerEntryItem.CostCenters = costCenters;
                }

                ledgerEntryItem.Currency = new CurrencyDTO() { Id = item.Amount.Currency.Id };
                ledgerEntryItem.Description = Global.AccountingReclassification;

                ledgerEntryItem.EntryType = new EntryTypeDTO() { EntryTypeId = 0 };
                ledgerEntryItem.Id = 0;
                ledgerEntryItem.Individual = new IndividualDTO() { IndividualId = 0 };
                ledgerEntryItem.PostDated = new List<PostDatedDTO>();
                ledgerEntryItem.Receipt = new ReceiptDTO();

                ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
            }

            return ledgerEntry;
        }

        #endregion

    }

    #region Class

    /// <summary>
    /// DeleteFileAttribute
    /// </summary>
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();

            // Convert the current filter context to file and get the file path
            string filePath = (filterContext.Result as FilePathResult).FileName;

            // Delete the file after download
            File.Delete(filePath);
        }
    }

    #endregion

}