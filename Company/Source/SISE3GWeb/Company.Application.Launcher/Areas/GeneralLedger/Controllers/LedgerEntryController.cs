////System
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Globalization;

////Terceros
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using Excel;

////Sistran FWK
//using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Reports;
//using Sistran.Core.Framework.UIF.Web.Resources;
//using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
//using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Assemblers;
//using Sistran.Core.Framework.UIF.Web.Models;
//using Sistran.Core.Framework.UIF2.Controls.UifTable;
//using Sistran.Core.Framework.UIF2.Services;
//using Sistran.Core.Framework.BAF;
//using Sistran.Core.Framework.Exceptions;

////Sistran Core
////using Sistran.Core.Application.AccountingServices;
//using Sistran.Core.Application.CommonService.Models;
//using Sistran.Core.Application.GeneralLedgerServices;
//using Sistran.Core.Application.GeneralLedgerServices.DTOs;
//using Sistran.Core.Application.GeneralLedgerServices.Enums;
//using Sistran.Core.Application.GeneralLedgerServices.Models;
//using Sistran.Core.Application.TempCommonServices;
//using Sistran.Core.Application.UniquePersonService.Models;

////Sistran Company
//using Sistran.Company.Application.CommonServices;
//using Sistran.Company.Application.UniqueUserServices;

//namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
//{
//    public class LedgerEntryController : Controller
//    {
//        #region Class

//        public class MassiveLog
//        {
//            /// <summary>
//            /// Identificador único de clase
//            /// </summary>
//            public int Id { get; set; }

//            /// <summary>
//            /// Descripción del error
//            /// </summary>
//            public string Description { get; set; }

//            /// <summary>
//            /// Fecha de proceso
//            /// </summary>
//            public DateTime ProcessDate { get; set; }

//            /// <summary>
//            /// Fecha contable de operación
//            /// </summary>
//            public DateTime OperationDate { get; set; }

//            /// <summary>
//            /// Número de fila del archivo excel
//            /// </summary>
//            public int RowNumber { get; set; }

//            /// <summary>
//            /// Estado el registro: false -> error, true --> exitoso
//            /// </summary>
//            public bool Status { get; set; }
//        }

//        #endregion

//        #region Instance Variables

//        readonly ICommonService DelegateService.commonService = ServiceManager.Instance.GetService<ICommonService>();
//        readonly IAccountingService DelegateService.glAccountingApplicationService = ServiceManager.Instance.GetService<IAccountingService>();
//        readonly IImputationService _imputationService = ServiceManager.Instance.GetService<IImputationService>();
//        readonly IUniqueUserService DelegateService.uniqueUserService = ServiceManager.Instance.GetService<IUniqueUserService>();
//        readonly ITempCommonService DelegateService.tempCommonService = ServiceManager.Instance.GetService<ITempCommonService>();

//        readonly BaseController _baseController = new BaseController();

//        #endregion

//        #region View

//        /// <summary>
//        /// MainLedgerEntry
//        /// </summary>
//        /// <returns>ActionResult</returns>
//        public ActionResult MainLedgerEntry()
//        {
//            try
//            {   


//                ViewBag.BranchDefault = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 0);
//                ViewBag.BranchDisabled = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 1);
//                ViewBag.SalePointBranchUserDefault = _imputationService.GetSalePointDefaultByUserIdAndBranchId(_baseController.SessionHelper.GetUserId(), ViewBag.BranchDefault);

//                //Obtiene si es el uso del tercero en  1 TRUE 0 FALSE
//                ViewBag.ThirdAccountingUsed = DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["ThirdAccountingUsed"])).NumberParameter;

//                //Obtiene si es multicompañía 1 TRUE 0 FALSE
//                ViewBag.ParameterMulticompany = _baseController.GetParameterMulticompany();
//                //Se obtiene la compañía contable por defecto
//                ViewBag.DefaultAccountingCompany = _baseController.GetDefaultAccountingCompany();

//                return View();

//            }
//            catch (UnhandledException)
//            {
//                return View("~/Views/Error/NotFound.cshtml");
//            }
//        }

//        /// <summary>
//        /// LedgerEntrySearch
//        /// </summary>
//        /// <returns>ActionResult</returns>
//        public ActionResult LedgerEntrySearch()
//        {
//            try
//            {  

//                ViewBag.Active = Convert.ToInt32(AccountingEntryStatus.Active);
//                ViewBag.Reverted = Convert.ToInt32(AccountingEntryStatus.Reverted);
//                ViewBag.BranchDefault = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 0);
//                ViewBag.BranchDisabled = _baseController.GetBranchDefaultByUserId(_baseController.SessionHelper.GetUserId(), 1);

//                return View();

//            }
//            catch (UnhandledException)
//            {
//                return View("~/Views/Error/NotFound.cshtml");
//            }
//        }

//        /// <summary>
//        /// MassiveLedgerEntry
//        /// </summary>
//        /// <returns>ActionResult</returns>
//        public ActionResult MassiveLedgerEntry()
//        {
//            try
//            {
//                //valida que el servicio este arriba
//                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

//                return View();
//            }
//            catch (UnhandledException)
//            {
//                return View("~/Views/Error/NotFound.cshtml");
//            }         
//        }

//        #endregion

//        #region Public Methods

//        /// <summary>
//        /// SaveLedgerEntry
//        /// </summary>
//        /// <param name="accountingEntryModels"></param>
//        /// <returns>ActionResult</returns>
//        [HttpPost]
//        public ActionResult SaveLedgerEntry(LedgerEntryModel ledgerEntryModel)
//        {
//            int saved = 0;

//            try
//            {
//                int userId = SessionHelper.GetUserId());
//                int moduleId = Convert.ToInt32(ConfigurationManager.AppSettings["EntryModule"]);

//                LedgerEntry ledgerEntry = new LedgerEntry();
//                // Cabecera
//                ledgerEntry.AccountingCompany = new AccountingCompany()
//                {
//                    AccountingCompanyId = ledgerEntryModel.AccountingCompanyId
//                };
//                ledgerEntry.AccountingDate = _baseController.GetAccountingDateByModule(moduleId); //se envía la fecha contable, la fecha de registro se enviará a nivel de servicio.
//                ledgerEntry.AccountingMovementType = new AccountingMovementType()
//                {
//                    AccountingMovementTypeId = ledgerEntryModel.AccountingMovementTypeId
//                };
//                ledgerEntry.Branch = new Branch() { Id = ledgerEntryModel.BranchId };
//                ledgerEntry.Description = ledgerEntryModel.Description;
//                ledgerEntry.EntryDestination = new EntryDestination() { DestinationId = ledgerEntryModel.EntryDestinationId };
//                ledgerEntry.EntryNumber = 0;
//                ledgerEntry.Id = 0;
//                ledgerEntry.LedgerEntryItems = new List<LedgerEntryItem>();

//                foreach (LedgerEntryItemModel ledgerEntryItemModel in ledgerEntryModel.LedgerEntryItems)
//                {
//                    LedgerEntryItem ledgerEntryItem = new LedgerEntryItem();

//                    ledgerEntryItem.AccountingAccount = new AccountingAccount()
//                    {
//                        Number = ledgerEntryItemModel.AccountingAccountNumber,
//                        AccountingAccountId = ledgerEntryItemModel.AccountingAccountId
//                    };
//                    ledgerEntryItem.AccountingNature = (AccountingNatures)ledgerEntryItemModel.AccountingNatureId;
//                    ledgerEntryItem.Amount = new Amount()
//                    {
//                        Currency = new Currency() { Id = ledgerEntryItemModel.CurrencyId },
//                        Value = ledgerEntryItemModel.Amount
//                    };
//                    ledgerEntryItem.ExchangeRate = new ExchangeRate()
//                    {
//                        SellAmount = ledgerEntryItemModel.ExchangeRate
//                    };
//                    ledgerEntryItem.LocalAmount = new Amount() { Value = ledgerEntryItemModel.Amount * ledgerEntryItemModel.ExchangeRate };

//                    ledgerEntryItem.Analysis = new List<Analysis>();
//                    // Se arma la lista de análisis
//                    if (ledgerEntryItemModel.Analysis != null)
//                    {
//                        foreach (AnalysisModel analysisModel in ledgerEntryItemModel.Analysis)
//                        {
//                            ledgerEntryItem.Analysis.Add(ModelAssembler.GetAnalysis(analysisModel));
//                        }
//                    }
//                    ledgerEntryItem.ReconciliationMovementType = new ReconciliationMovementType()
//                    {
//                        Id = ledgerEntryItemModel.BankReconciliationId
//                    };

//                    ledgerEntryItem.CostCenters = new List<CostCenter>();
//                    // Se arma la lista de centros de costos
//                    if (ledgerEntryItemModel.CostCenters != null)
//                    {
//                        foreach (CostCenterEntryModel costCenterEntryModel in ledgerEntryItemModel.CostCenters)
//                        {
//                            ledgerEntryItem.CostCenters.Add(ModelAssembler.GetCostCenterEntry(costCenterEntryModel));
//                        }
//                    }
//                    ledgerEntryItem.Currency = new Currency() {  Id = ledgerEntryItemModel.CurrencyId };
//                    ledgerEntryItem.Description = ledgerEntryItemModel.Description;
//                    ledgerEntryItem.EntryType = new EntryType() { EntryTypeId = ledgerEntryItemModel.EntryTypeId };
//                    ledgerEntryItem.Id = 0;
//                    ledgerEntryItem.Individual = new Individual() { IndividualId = ledgerEntryItemModel.IndividualId };

//                    ledgerEntryItem.PostDated = new List<PostDated>();
//                    // Se arma la lista de postfechados
//                    if (ledgerEntryItemModel.Postdated != null)
//                    {
//                        foreach (PostDatedModel postDatedModel in ledgerEntryItemModel.Postdated)
//                        {
//                            ledgerEntryItem.PostDated.Add(ModelAssembler.GetPostDated(postDatedModel));
//                        }
//                    }
//                    ledgerEntryItem.Receipt = new Receipt()
//                    {
//                        Date = Convert.ToDateTime(ledgerEntryItemModel.ReceiptDate),
//                        Number = ledgerEntryItemModel.ReceiptNumber
//                    };

//                    ledgerEntry.LedgerEntryItems.Add(ledgerEntryItem);
//                }
//                ledgerEntry.ModuleDateId = moduleId;
//                ledgerEntry.RegisterDate = DateTime.Now;
//                ledgerEntry.SalePoint = new SalePoint() { Id = ledgerEntryModel.SalePointId };
//                ledgerEntry.Status = 1; // activo
//                ledgerEntry.UserId = userId;

//                saved = DelegateService.glAccountingApplicationService.SaveLedgerEntry(ledgerEntry);
//            }
//            catch (BusinessException businessException)
//            {
//                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
//            }
//            catch (UnhandledException)
//            {
//                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
//            }

//            return new UifJsonResult(true, saved);
//        }

//        /// <summary>
//        /// GetLedgerEntryByCriteria
//        /// </summary>
//        /// <param name="branchId"></param>
//        /// <param name="date"></param>
//        /// <param name="entryNumber"></param>
//        /// <param name="destinationId"></param>
//        /// <param name="accountingMovementTypeId"></param>
//        public void GetLedgerEntryByCriteria(int branchId, string date, int entryNumber, int destinationId, int accountingMovementTypeId)
//        {
//            try
//            {
//                // Se arma las fechas para consulta
//                int year = Convert.ToDateTime(date).Year;
//                int month = Convert.ToDateTime(date).Month;
//                string dateFrom = "01" + "/" + month.ToString() + "/" + year.ToString();
//                int numberOfDays = DateTime.DaysInMonth(year, month);
//                string dateTo = numberOfDays.ToString() + "/" + month.ToString() + "/" + year.ToString();
//                dateTo = dateTo + " 23:59:59";

//                var ledgerEntries = DelegateService.glAccountingApplicationService.GetLedgerEntries(entryNumber, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), branchId, destinationId, accountingMovementTypeId);
//                List<DailyEntryReportModel> legderEntryModels = new List<DailyEntryReportModel>();

//                foreach(LedgerEntry ledgerEntry in ledgerEntries)
//                {
//                    foreach (LedgerEntryItem ledgerEntryItem in ledgerEntry.LedgerEntryItems)
//                    {
//                        DailyEntryReportModel ledgerEntryModel = new DailyEntryReportModel();

//                        ledgerEntryModel.AccountingCompany = ledgerEntry.AccountingCompany.AccountingCompanyId;
//                        ledgerEntryModel.Branch = _baseController.GetBranchDescriptionById(ledgerEntry.Branch.Id, User.Identity.Name.ToUpper());
//                        ledgerEntryModel.CompanyDescription = ledgerEntry.AccountingCompany.Description;
//                        ledgerEntryModel.DailyEntryHeaderDescription = ledgerEntry.Description;
//                        ledgerEntryModel.DailyEntryHeaderId = ledgerEntry.Id;
//                        ledgerEntryModel.Date = Convert.ToDateTime(ledgerEntry.AccountingDate.ToString().Substring(0, 10)).Date;
//                        ledgerEntryModel.SalePoint = ledgerEntry.SalePoint.Id;

//                        ledgerEntryModel.AccountingAccountDescription = ledgerEntryItem.AccountingAccount.Description;
//                        ledgerEntryModel.AccountingAccountId = ledgerEntryItem.AccountingAccount.AccountingAccountId;
//                        ledgerEntryModel.AccountingNature = ledgerEntryItem.AccountingNature == AccountingNatures.Credit ? 1 : 2;
//                        ledgerEntryModel.AccountingNumber = Convert.ToDecimal(ledgerEntryItem.AccountingAccount.Number);
//                        ledgerEntryModel.AmountValue = ledgerEntryItem.Amount.Value;
//                        ledgerEntryModel.Balance = 0;
//                        ledgerEntryModel.BankDescription = "";
//                        ledgerEntryModel.Credit = ledgerEntryItem.AccountingNature == AccountingNatures.Credit ? ledgerEntryItem.Amount.Value : 0;
//                        ledgerEntryModel.Currency = ledgerEntryItem.Currency.Id;
//                        ledgerEntryModel.CurrencyDescription = _baseController.GetCurrencyDescriptionById(ledgerEntryItem.Currency.Id);
//                        ledgerEntryModel.DailyEntryId = ledgerEntryItem.Id;
//                        ledgerEntryModel.Debit = ledgerEntryItem.AccountingNature == AccountingNatures.Debit ? ledgerEntryItem.Amount.Value : 0;
//                        ledgerEntryModel.Description = ledgerEntryItem.Description;
//                        ledgerEntryModel.EntryNumber = ledgerEntry.EntryNumber;
//                        ledgerEntryModel.ImputationCode = 0;
//                        ledgerEntryModel.ImputationDescription = "";
//                        ledgerEntryModel.ReceiptDate = Convert.ToDateTime(ledgerEntryItem.Receipt.Date);
//                        ledgerEntryModel.ReceiptNumber = Convert.ToInt32(ledgerEntryItem.Receipt.Number);
//                        ledgerEntryModel.SubTotalBalance = 0;
//                        ledgerEntryModel.SubTotalCredit = 0;
//                        ledgerEntryModel.SubTotalDebit = 0;
//                        ledgerEntryModel.TechnicalTransaction = 0;
//                        ledgerEntryModel.TransactionNumber = 0;

//                        legderEntryModels.Add(ledgerEntryModel);
//                    }
//                }

//                var reportDocument = new ReportDocument();
//                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Areas//GeneralLedger//Reports//EntryConsultationReport.rpt";

//                reportDocument.Load(reportPath);

//                //Se llena el reporte principal
//                reportDocument.SetDataSource(legderEntryModels);
//                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "EntryReport");
//            }
//            catch (Exception ex)
//            {
//                Response.Write(ex.ToString());
//            }
//        }

//        /// <summary>
//        /// GenerateLedgerEntryReport
//        /// </summary>
//        /// <param name="branchId"></param>
//        /// <param name="year"></param>
//        /// <param name="month"></param>
//        /// <param name="entryNumber"></param>
//        /// <param name="destinationId"></param>
//        /// <param name="accountingMovementTypeId"></param>
//        public void GenerateLedgerEntryReport(int branchId, int year, int month, int entryNumber, int destinationId, int accountingMovementTypeId)
//        {
//            try
//            {
//                // Se arma las fechas para consulta
//                string dateFrom = "01" + "/" + month.ToString() + "/" + year.ToString();
//                int numberOfDays = DateTime.DaysInMonth(year, month);
//                string dateTo = numberOfDays.ToString() + "/" + month.ToString() + "/" + year.ToString();
//                dateTo = dateTo + " 23:59:59";

//                var ledgerEntries = DelegateService.glAccountingApplicationService.GetLedgerEntries(entryNumber, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), branchId, destinationId, accountingMovementTypeId);
//                List<DailyEntryReportModel> legderEntryModels = new List<DailyEntryReportModel>();

//                foreach (LedgerEntry ledgerEntry in ledgerEntries)
//                {
//                    foreach (LedgerEntryItem ledgerEntryItem in ledgerEntry.LedgerEntryItems)
//                    {
//                        DailyEntryReportModel ledgerEntryModel = new DailyEntryReportModel();

//                        ledgerEntryModel.AccountingCompany = ledgerEntry.AccountingCompany.AccountingCompanyId;
//                        ledgerEntryModel.Branch = _baseController.GetBranchDescriptionById(ledgerEntry.Branch.Id, User.Identity.Name.ToUpper());
//                        ledgerEntryModel.CompanyDescription = ledgerEntry.AccountingCompany.Description;
//                        ledgerEntryModel.DailyEntryHeaderDescription = ledgerEntry.Description;
//                        ledgerEntryModel.DailyEntryHeaderId = ledgerEntry.Id;
//                        ledgerEntryModel.Date = Convert.ToDateTime(ledgerEntry.AccountingDate.ToString().Substring(0, 10)).Date;
//                        ledgerEntryModel.SalePoint = ledgerEntry.SalePoint.Id;

//                        ledgerEntryModel.AccountingAccountDescription = ledgerEntryItem.AccountingAccount.Description;
//                        ledgerEntryModel.AccountingAccountId = ledgerEntryItem.AccountingAccount.AccountingAccountId;
//                        ledgerEntryModel.AccountingNature = ledgerEntryItem.AccountingNature == AccountingNatures.Credit ? 1 : 2;
//                        ledgerEntryModel.AccountingNumber = Convert.ToDecimal(ledgerEntryItem.AccountingAccount.Number);
//                        ledgerEntryModel.AmountValue = ledgerEntryItem.Amount.Value;
//                        ledgerEntryModel.Balance = 0;
//                        ledgerEntryModel.BankDescription = "";
//                        ledgerEntryModel.Credit = ledgerEntryItem.AccountingNature == AccountingNatures.Credit ? ledgerEntryItem.Amount.Value : 0;
//                        ledgerEntryModel.Currency = ledgerEntryItem.Currency.Id;
//                        ledgerEntryModel.CurrencyDescription = _baseController.GetCurrencyDescriptionById(ledgerEntryItem.Currency.Id);
//                        ledgerEntryModel.DailyEntryId = ledgerEntryItem.Id;
//                        ledgerEntryModel.Debit = ledgerEntryItem.AccountingNature == AccountingNatures.Debit ? ledgerEntryItem.Amount.Value : 0;
//                        ledgerEntryModel.Description = ledgerEntryItem.Description;
//                        ledgerEntryModel.EntryNumber = ledgerEntry.EntryNumber;
//                        ledgerEntryModel.ImputationCode = 0;
//                        ledgerEntryModel.ImputationDescription = "";
//                        ledgerEntryModel.ReceiptDate = Convert.ToDateTime(ledgerEntryItem.Receipt.Date);
//                        ledgerEntryModel.ReceiptNumber = Convert.ToInt32(ledgerEntryItem.Receipt.Number);
//                        ledgerEntryModel.SubTotalBalance = 0;
//                        ledgerEntryModel.SubTotalCredit = 0;
//                        ledgerEntryModel.SubTotalDebit = 0;
//                        ledgerEntryModel.TechnicalTransaction = 0;
//                        ledgerEntryModel.TransactionNumber = 0;

//                        legderEntryModels.Add(ledgerEntryModel);
//                    }
//                }

//                var reportDocument = new ReportDocument();
//                string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Areas//GeneralLedger//Reports//EntryConsultationReport.rpt";

//                reportDocument.Load(reportPath);

//                //Se llena el reporte principal
//                reportDocument.SetDataSource(legderEntryModels);
//                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "EntryReport");
//            }
//            catch (Exception ex)
//            {
//                Response.Write(ex.ToString());
//            }
//        }

//        /// <summary>
//        /// GetLedgerEntries
//        /// </summary>
//        /// <param name="branchId"></param>
//        /// <param name="year"></param>
//        /// <param name="month"></param>
//        /// <param name="entryNumber"></param>
//        /// <param name="destinationId"></param>
//        /// <param name="accountingMovementTypeId"></param>
//        /// <param name="isFiltered"></param>
//        /// <returns>ActionResult</returns>
//        public ActionResult GetLedgerEntries(int? branchId, int year, int month, int entryNumber,
//                                             int? destinationId, int? accountingMovementTypeId, bool isFiltered)
//        {
//            // Se arma las fechas para consulta
//            string dateFrom = "01" + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
//            int numberOfDays = DateTime.DaysInMonth(year, month);
//            string dateTo = Convert.ToString(numberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
//            dateTo = dateTo + " 23:59:59";

//            List<LedgerEntry> ledgerEntries = DelegateService.glAccountingApplicationService.GetLedgerEntries(entryNumber, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo),
//                                   Convert.ToInt32(branchId), Convert.ToInt32(destinationId), Convert.ToInt32(accountingMovementTypeId));

//            if (ledgerEntries.Count > 0)
//            {
//                foreach (var ledgerEntry in ledgerEntries)
//                {
//                    foreach(LedgerEntryItem ledgerEntryItem in ledgerEntry.LedgerEntryItems)
//                    {
//                        ledgerEntryItem.Currency.Description = _baseController.GetCurrencyDescriptionById(ledgerEntryItem.Currency.Id);
//                    }
//                }
//            }

//            if (isFiltered)
//            {
//                var entries = ledgerEntries.GroupBy(x => new
//                {
//                    x.AccountingDate,
//                    EntryDestinationId = x.EntryDestination.DestinationId,
//                    EntryDestinationDescription = x.EntryDestination.Description,
//                    BranchId = x.Branch.Id,
//                    x.AccountingMovementType.AccountingMovementTypeId,
//                    AccountingMovementTypeDescription = x.AccountingMovementType.Description
//                }).ToList();

//                List<EntryConsultationDTO> entryConsultations = new List<EntryConsultationDTO>();

//                foreach (var item in entries)
//                {
//                    entryConsultations.Add(new EntryConsultationDTO()
//                    {
//                        Date = item.Key.AccountingDate.ToString("dd/MM/yyyy"),
//                        EntryNumber = entryNumber, //Convert.ToInt32(item.Key.EntryNumber),
//                        EntryDestinationId = Convert.ToInt32(item.Key.EntryDestinationId),
//                        EntryDestinationDescription = item.Key.EntryDestinationDescription,
//                        AccountingMovementTypeId = Convert.ToInt32(item.Key.AccountingMovementTypeId),
//                        AccountingMovementTypeDescription = item.Key.AccountingMovementTypeDescription,
//                        BranchId = Convert.ToInt32(item.Key.BranchId),
//                        BranchDescription = _baseController.GetBranchDescriptionById(Convert.ToInt32(item.Key.BranchId), User.Identity.Name.ToUpper())
//                    });
//                }

//                return new UifTableResult(entryConsultations);
//            }
//            else
//            {
//                List<object> entries = new List<object>();
//                foreach (LedgerEntry ledgerEntry in ledgerEntries)
//                {

//                    foreach (LedgerEntryItem ledgerEntryItem in ledgerEntry.LedgerEntryItems)
//                    {
//                        entries.Add(new
//                        {
//                            ledgerEntryItem.AccountingAccount.AccountingAccountId,
//                            AccountingAccountName = ledgerEntryItem.AccountingAccount.Description,
//                            AccountingAccountNumber = ledgerEntryItem.AccountingAccount.Number,
//                            AccountingMovementTypeDescription = ledgerEntry.AccountingMovementType.Description,
//                            ledgerEntry.AccountingMovementType.AccountingMovementTypeId,
//                            AccountingNature = Convert.ToInt32(ledgerEntryItem.AccountingNature),
//                            AccountingNatureDescription = Convert.ToInt32(ledgerEntryItem.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) ? Global.Credits : Global.Debits,
//                            BranchDescription = _baseController.GetBranchDescriptionById(ledgerEntry.Branch.Id, User.Identity.Name),
//                            BranchId = ledgerEntry.Branch.Id,
//                            CreditsAmountLocalValue = Convert.ToInt32(ledgerEntryItem.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) 
//                            ? string.Format(new CultureInfo("en-US"), "{0:C}", ledgerEntryItem.LocalAmount.Value) : "0",
//                            CreditsAmountValue = Convert.ToInt32(ledgerEntryItem.AccountingNature) == Convert.ToInt32(AccountingNatures.Credit) ? ledgerEntryItem.Amount.Value : 0,
//                            CurrencyDescription = _baseController.GetCurrencyDescriptionById(ledgerEntryItem.Currency.Id),
//                            CurrencyId = ledgerEntryItem.Currency.Id,
//                            DailyEntryHeaderId = ledgerEntry.Id,
//                            Date = ledgerEntry.AccountingDate.ToString("dd/MM/yyyy"),
//                            DebitsAmountLocalValue = Convert.ToInt32(ledgerEntryItem.AccountingNature) == Convert.ToInt32(AccountingNatures.Debit)
//                            ? string.Format(new CultureInfo("en-US"), "{0:C}", ledgerEntryItem.LocalAmount.Value) : "0",
//                            DebitsAmountValue = Convert.ToInt32(ledgerEntryItem.AccountingNature) == Convert.ToInt32(AccountingNatures.Debit) ? ledgerEntryItem.Amount.Value : 0,
//                            EntryDescription = ledgerEntryItem.Description,
//                            EntryDestinationDescription = ledgerEntry.EntryDestination.Description,
//                            EntryDestinationId = ledgerEntry.EntryDestination.DestinationId,
//                            EntryHeaderDescription = ledgerEntry.Description,
//                            EntryId = ledgerEntryItem.Id,
//                            ledgerEntry.EntryNumber,
//                            Status = ledgerEntry.Status == 1 ? AccountingEntryStatus.Active : AccountingEntryStatus.Reverted,
//                            StatusDescription = ledgerEntry.Status == 1 ? Global.Active : Global.Reverted,
//                            UserId = ledgerEntry.UserId,
//                            UserName = DelegateService.uniqueUserService.GetUserById(ledgerEntry.UserId).AccountName
//                        });
//                    }
//                }

//                return new UifTableResult(entries);
//            }
//        }

//        /// <summary>
//        /// LedgerEntryRevertion
//        /// </summary>
//        /// <param name="branchId"></param>
//        /// <param name="year"></param>
//        /// <param name="month"></param>
//        /// <param name="entryNumber"></param>
//        /// <param name="destinationId"></param>
//        /// <param name="accountingMovementTypeId"></param>
//        /// <param name="accountingDate"></param>
//        /// <param name="ledgerEntryId"></param>
//        /// <returns>ActionResult</returns>
//        [HttpPost]
//        public ActionResult LedgerEntryRevertion(int branchId, int year, int month, int entryNumber, int destinationId, int accountingMovementTypeId, string accountingDate, int ledgerEntryId)
//        {
//            try
//            {
//                int reverted = 0;

//                // Se arma las fechas para consulta
//                string dateFrom = "01" + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
//                int numberOfDays = DateTime.DaysInMonth(year, month);
//                string dateTo = Convert.ToString(numberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year);
//                dateTo = dateTo + " 23:59:59";

//                int userId = SessionHelper.GetUserId());
//                int transactionNumber = Convert.ToInt32(DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["JournalEntryTransactionNumber"])).NumberParameter); //obtiene parámetro de la BDD
//                DateTime accountingDateRevertion = _baseController.GetAccountingDateByModule(Convert.ToInt32(ConfigurationManager.AppSettings["EntryModule"]));

//                LedgerEntry ledgerEntry = new LedgerEntry()
//                {
//                    AccountingCompany = new AccountingCompany(),
//                    AccountingDate = Convert.ToDateTime(accountingDate),
//                    AccountingMovementType = new AccountingMovementType() { AccountingMovementTypeId = accountingMovementTypeId },
//                    Branch = new Branch() { Id = branchId },
//                    Description = "",
//                    EntryDestination = new EntryDestination() { DestinationId = destinationId },
//                    EntryNumber = entryNumber,
//                    Id = ledgerEntryId,
//                    LedgerEntryItems = new List<LedgerEntryItem>(),
//                    ModuleDateId = 0,
//                    RegisterDate = accountingDateRevertion,
//                    SalePoint = new SalePoint() { Id = 0 },
//                    Status = Convert.ToInt32(AccountingEntryStatus.Reverted), // reversar
//                    UserId = userId
//                };

//                reverted = DelegateService.glAccountingApplicationService.ReverseLedgerEntry(ledgerEntry);

//                return new UifJsonResult(true, reverted);
//            }
//            catch (BusinessException businessException)
//            {
//                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
//            }
//            catch (UnhandledException)
//            {
//                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        /// <summary>
//        /// ReadFileInMemory
//        /// </summary>
//        /// <param name="uploadedFile"></param>
//        /// <param name="fileType"></param>
//        /// <returns>JsonResult</returns>
//        public JsonResult ReadFileInMemory(HttpPostedFileBase uploadedFile, int fileType)
//        {
//            string fileLocationName = "";
//            fileLocationName = uploadedFile.FileName;
//            string[] data = fileLocationName.Split(new char[] { '.' });
//            if (fileType == 0)
//            {
//                if (data[1] == "xls" || data[1] == "xlsx")
//                {
//                    return ExcelToStream(uploadedFile);
//                }
//            }

//            if (fileType != 1)
//            {
//                fileLocationName = "BadFileExtension";
//            }

//            object[] jsonData = new object[2];

//            jsonData[0] = fileLocationName;
//            jsonData[1] = false;

//            return Json(jsonData, JsonRequestBehavior.AllowGet);
//        }

//        /// <summary>
//        /// GetEntryMassiveLoadRecords
//        /// </summary>
//        /// <returns>JsonResult</returns>
//        public JsonResult GetEntryMassiveLoadRecords()
//        {
//            List<MassiveLog> massiveLogs = (List<MassiveLog>)TempData["MassiveLogs"];
//            List<object> records = new List<object>();
//            int successfulRecords = 0;
//            int failedRecords = 0;

//            if (massiveLogs.Count > 0)
//            {
//                var successQuery = from MassiveLog massiveLog in massiveLogs where massiveLog.Status == Convert.ToBoolean(1) select massiveLog;
//                if (successQuery.Count() > 0)
//                {
//                    successfulRecords = Convert.ToInt32(successQuery.Count());
//                }

//                var failedQuery = from MassiveLog massiveLog in massiveLogs where massiveLog.Status == Convert.ToBoolean(0) select massiveLog;

//                if (failedQuery.Count() > 0)
//                {
//                    failedRecords = Convert.ToInt32(failedQuery.Count());
//                }
//            }

//            records.Add(new
//            {
//                FailedRecords = failedRecords,
//                TotalRecords = massiveLogs.Count,
//                SuccessfulRecords = successfulRecords
//            });

//            return Json(records, JsonRequestBehavior.AllowGet);
//        }

//        /// <summary>
//        /// GetMassiveEntryFailedRecords
//        /// </summary>
//        /// <returns>ActionResult</returns>
//        public ActionResult GetMassiveEntryFailedRecords()
//        {
//            List<MassiveLog> failedMassiveLogs = (List<MassiveLog>)Session["MassiveLogs"];
//            List<object> records = new List<object>();
//            var massiveLogs = from MassiveLog massiveLog in failedMassiveLogs where massiveLog.Status == Convert.ToBoolean(0) select massiveLog;

//            foreach (MassiveLog massiveLog in massiveLogs)
//            {
//                records.Add(new
//                {
//                    Description = massiveLog.Description,
//                    Enabled = true,
//                    Id = massiveLog.Id,
//                    OperationDate = massiveLog.OperationDate.ToString("dd/MM/yyyy"),
//                    ProcessDate = massiveLog.ProcessDate.ToString("dd/MM/yyyy"),
//                    RowNumber = massiveLog.RowNumber,
//                    Success = massiveLog.Status,
//                });
//            }

//            return new UifTableResult(records);
//        }

//        /// <summary>
//        /// GenerateEntry
//        /// </summary>
//        /// <returns>JsonResult</returns>
//        public JsonResult GenerateEntry()
//        {
//            try
//            {
//                int entry = 0;
//                int userId = 0;

//                userId = SessionHelper.GetUserId());
//                entry = DelegateService.glAccountingApplicationService.GenerateEntry(userId);

//                return Json(entry, JsonRequestBehavior.AllowGet);
//            }
//            catch (BusinessException businessException)
//            {
//                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
//            }
//            catch (UnhandledException)
//            {
//                return Json(new { success = false, result = Global.UnhandledExceptionMsj }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        #endregion

//        #region Private Methods

//        /// <summary>
//        /// ExcelToStream
//        /// </summary>
//        /// <param name="uploadedFile"></param>
//        /// <returns>JsonResult</returns>
//        private JsonResult ExcelToStream(HttpPostedFileBase uploadedFile)
//        {
//            bool successful = false;
//            string message = "";
//            string fileLocationName = "";
//            Byte[] arrayContent;
//            bool validateHeader = false;
//            int processedRows = 0;

//            fileLocationName = uploadedFile.FileName;
//            string[] data = fileLocationName.Split(new char[] { '.' });

//            //Convertir a Bytes
//            var buffer = new byte[uploadedFile.InputStream.Length];
//            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

//            //Lee el archivo y guarda en arreglo de typo byte y este a su vez a arrayContent
//            arrayContent = buffer;

//            Stream stream = new MemoryStream(arrayContent);

//            IExcelDataReader excelReader;

//            int rowNumber = 1;
//            List<MassiveLog> massiveLogs = new List<MassiveLog>();

//            try
//            {
//                if (data[1] == "xls")
//                {
//                    //1. Lee desde binary Excel  ('97-2003 format; *.xls)
//                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
//                }
//                else
//                {
//                    //2. Lee desde binary OpenXml Excel file (2007 format; *.xlsx)
//                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
//                }
//                //3. DataSet - El resultado sera creado en result.Tables
//                DataSet dataSet = excelReader.AsDataSet();

//                // Se borra la tabla de trabajo.
//                DelegateService.glAccountingApplicationService.ClearEntryMassiveLoad();

//                // Se deshabilita los registros de la tabla de log

//                if (dataSet.Tables[0].Rows.Count > 0)
//                {
//                    int branchId = 0;
//                    int salePointId = 0;
//                    int accountingCompanyId = 0;
//                    int entryDestinationId = 0;
//                    int accountingMovementTypeId = 0;
//                    DateTime operationDate = DateTime.Now;
//                    decimal debits = 0;
//                    decimal credits = 0;
//                    bool isBalanced = false;

//                    // Se lee la cabecera                    
//                    if (Convert.ToString(dataSet.Tables[0].Rows[1][0]) != "")
//                    {
//                        branchId = Convert.ToInt32(dataSet.Tables[0].Rows[1][0]);
//                        validateHeader = true;
//                    }
//                    else
//                    {
//                        validateHeader = false;
//                        message = "NoBranchId";
//                    }
//                    if (validateHeader)
//                    {
//                        if (Convert.ToString(dataSet.Tables[0].Rows[1][2]) != "")
//                        {
//                            salePointId = Convert.ToInt32(dataSet.Tables[0].Rows[1][2]);
//                            validateHeader = true;
//                        }
//                        else
//                        {
//                            validateHeader = false;
//                            message = "NoSalePointId";
//                        }
//                    }
//                    if (validateHeader)
//                    {
//                        if (Convert.ToString(dataSet.Tables[0].Rows[1][4]) != "")
//                        {
//                            accountingCompanyId = Convert.ToInt32(dataSet.Tables[0].Rows[1][4]);
//                            validateHeader = true;
//                        }
//                        else
//                        {
//                            validateHeader = false;
//                            message = "NoAccountingCompanyId";
//                        }
//                    }
//                    if (validateHeader)
//                    {
//                        if (Convert.ToString(dataSet.Tables[0].Rows[1][6]) != "")
//                        {
//                            entryDestinationId = Convert.ToInt32(dataSet.Tables[0].Rows[1][6]);
//                            validateHeader = true;
//                        }
//                        else
//                        {
//                            validateHeader = false;
//                            message = "NoEntryDestinationId";
//                        }
//                    }
//                    if (validateHeader)
//                    {
//                        if (Convert.ToString(dataSet.Tables[0].Rows[1][8]) != "")
//                        {
//                            accountingMovementTypeId = Convert.ToInt32(dataSet.Tables[0].Rows[1][8]);
//                            validateHeader = true;
//                        }
//                        else
//                        {
//                            validateHeader = false;
//                            message = "NoAccountingMovementTypeId";
//                        }
//                    }
//                    if (validateHeader)
//                    {
//                        if (Convert.ToString(dataSet.Tables[0].Rows[1][10]) != "")
//                        {
//                            operationDate = Convert.ToDateTime(dataSet.Tables[0].Rows[1][10]);
//                            validateHeader = true;
//                        }
//                        else
//                        {
//                            validateHeader = false;
//                            message = "NoOperationDate";
//                        }
//                    }

//                    if (validateHeader)
//                    {
//                        // Se lee el detalle
//                        var rows = dataSet.Tables[0].Rows;

//                        for (int index = 4; index < rows.Count; index++)
//                        {
//                            // Se valida débitos y créditos.
//                            if (Convert.ToString(rows[index][0]) == "")
//                            {
//                                break; //indica que no existe nro de registro en el archivo, se lo usa para fin de lectura de movimientos
//                            }
//                            else
//                            {
//                                if (Convert.ToString(rows[index][2]) == "")
//                                {
//                                    successful = false;
//                                    message = "NoAccountingNature";
//                                    break;
//                                }
//                                else
//                                {
//                                    if (Convert.ToInt32(rows[index][2]) == (int)AccountingNatures.Credit)
//                                    {
//                                        credits = credits + Convert.ToDecimal(rows[index][6]);
//                                    }
//                                    if (Convert.ToInt32(rows[index][2]) == (int)AccountingNatures.Debit)
//                                    {
//                                        debits = debits + Convert.ToDecimal(rows[index][6]);
//                                    }
//                                }
//                            }
//                        }
//                        if (debits == credits)
//                        {
//                            isBalanced = true;
//                        }
//                        else
//                        {
//                            message = "Unbalanced";
//                            successful = false;
//                        }
//                        if (isBalanced)
//                        {
//                            for (int index = 4; index < rows.Count; index++)
//                            {
//                                if (Convert.ToString(rows[index][0]) == "")
//                                {
//                                    break; //indica que no existe nro de registro en el archivo, se lo usa para fin de lectura de movimientos
//                                }
//                                else
//                                {
//                                    processedRows = processedRows + 1;
//                                    // Se arma el dto para grabarlo en la base
//                                    MassiveEntryDTO massiveEntryDTO = new MassiveEntryDTO();
//                                    int error = 0;
//                                    string errorDescription = "";

//                                    // Cabecera
//                                    massiveEntryDTO.Id = 0; //este dato es autonumérico
//                                    massiveEntryDTO.BranchId = branchId;
//                                    massiveEntryDTO.SalePointId = salePointId;
//                                    massiveEntryDTO.AccoutingCompanyId = accountingCompanyId;
//                                    massiveEntryDTO.EntryDestinationId = entryDestinationId;
//                                    massiveEntryDTO.AccountingMovementTypeId = accountingMovementTypeId;
//                                    massiveEntryDTO.OperationDate = operationDate;

//                                    // Movimientos
//                                    massiveEntryDTO.RowNumber = Convert.ToInt32(rows[index][0]);
//                                    if (Convert.ToString(rows[index][1]) == "")
//                                    {
//                                        error = error + 1;
//                                        errorDescription = Global.NoEnteredAccountingAccount;
//                                    }
//                                    else
//                                    {
//                                        AccountingAccount accountingAccount = new AccountingAccount() { Number = Convert.ToString(rows[index][1]) };

//                                        List<AccountingAccount> accountingAccounts = DelegateService.glAccountingApplicationService.GetAccountingAccountsByNumberDescription(accountingAccount);

//                                        if (accountingAccounts.Count > 0)
//                                        {
//                                            massiveEntryDTO.AccountingAccountId = accountingAccounts[0].AccountingAccountId;
//                                        }
//                                        else
//                                        {
//                                            error = error + 1;
//                                            errorDescription = errorDescription == "" ? Global.NoAccountingAccountValidation : errorDescription + ", " + Global.NoAccountingAccountValidation;
//                                        }
//                                    }

//                                    massiveEntryDTO.AccountingNature = Convert.ToInt32(rows[index][2]);
//                                    if (Convert.ToString(rows[index][4]) == "")
//                                    {
//                                        error = error + 1;
//                                        errorDescription = errorDescription == "" ? Global.NoCurrencyValidation : errorDescription + ", " + Global.NoCurrencyValidation;
//                                        massiveEntryDTO.CurrencyId = -1;
//                                    }
//                                    else
//                                    {
//                                        massiveEntryDTO.CurrencyId = Convert.ToInt32(rows[index][4]);
//                                        // Calculo la tasa de cambio
//                                        ExchangeRate exchangeRate = _baseController.GetExchangeRateByCurrencyId(massiveEntryDTO.CurrencyId);

//                                        massiveEntryDTO.ExchangeRate = exchangeRate.SellAmount;
//                                    }

//                                    if (Convert.ToString(rows[index][6]) == "")
//                                    {
//                                        error = error + 1;
//                                        errorDescription = errorDescription == "" ? Global.NoAmountValidation : errorDescription + ", " + Global.NoAmountValidation;
//                                        massiveEntryDTO.Amount = 0;
//                                    }
//                                    else
//                                    {
//                                        massiveEntryDTO.Amount = Convert.ToDecimal(rows[index][6]);
//                                    }
//                                    if (Convert.ToString(rows[index][7]) == "")
//                                    {
//                                        error = error + 1;
//                                        errorDescription = errorDescription == "" ? Global.NoPersonDocumentNumberValidation : errorDescription + ", " + Global.NoPersonDocumentNumberValidation;
//                                        massiveEntryDTO.IndividualId = 0;
//                                    }
//                                    else
//                                    {
//                                        var persons = DelegateService.tempCommonService.GetPersonsByDocumentNumber(Convert.ToString(rows[index][7]));
//                                        if (persons.Count > 0)
//                                        {
//                                            massiveEntryDTO.IndividualId = persons[0].IndividualId;
//                                        }
//                                        else
//                                        {
//                                            error = error + 1;
//                                            errorDescription = errorDescription == "" ? Global.PersonNotFound : errorDescription + ", " + Global.PersonNotFound;
//                                            massiveEntryDTO.IndividualId = 0;
//                                        }
//                                    }
//                                    if (Convert.ToString(rows[index][8]) == "")
//                                    {
//                                        error = error + 1;
//                                        errorDescription = errorDescription == "" ? Global.NoMovementDescriptionValidation : errorDescription + ", " + Global.NoMovementDescriptionValidation;
//                                        massiveEntryDTO.Description = "";
//                                    }
//                                    else
//                                    {
//                                        massiveEntryDTO.Description = Convert.ToString(rows[index][8]);
//                                    }

//                                    #region BankReconciliation

//                                    bool isBankReconciliation = false;

//                                    massiveEntryDTO.BankReconciliationId = rows[index][9] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][9]);

//                                    // Se ingresó conciliación bancaria
//                                    if (massiveEntryDTO.BankReconciliationId > 0)
//                                    {
//                                        // Se comprueba que es una conciliación bancaria válida.
//                                        List<ReconciliationMovementType> bankReconciliations = DelegateService.glAccountingApplicationService.GetReconciliationMovementTypes();
//                                        isBankReconciliation = bankReconciliations.Any(item => item.Id == massiveEntryDTO.BankReconciliationId);
//                                        if (!isBankReconciliation)
//                                        {
//                                            error = error + 1;
//                                            errorDescription = errorDescription == "" ? Global.BankConciliationIdValidation : errorDescription + ", " + Global.BankConciliationIdValidation;
//                                            massiveEntryDTO.BankReconciliationId = 0;
//                                        }
//                                        else
//                                        {
//                                            if (rows[index][11] == DBNull.Value)
//                                            {
//                                                error = error + 1;
//                                                errorDescription = errorDescription == "" ? Global.NoBankConciliationNumberValidation : errorDescription + ", " + Global.NoBankConciliationNumberValidation;
//                                                massiveEntryDTO.ReceiptNumber = 0;
//                                            }
//                                            else
//                                            {
//                                                massiveEntryDTO.ReceiptNumber = Convert.ToInt32(rows[index][11]);
//                                            }
//                                            if (rows[index][12] == DBNull.Value)
//                                            {
//                                                error = error + 1;
//                                                errorDescription = errorDescription == "" ? Global.NoBankConciliationDateValidation : errorDescription + ", " + Global.NoBankConciliationDateValidation;
//                                                massiveEntryDTO.ReceiptDate = null;
//                                            }
//                                            else
//                                            {
//                                                massiveEntryDTO.ReceiptDate = Convert.ToDateTime(rows[index][12]);
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        massiveEntryDTO.ReceiptNumber = rows[index][11] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][11]);
//                                        massiveEntryDTO.ReceiptDate = rows[index][12] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rows[index][12]);
//                                    }

//                                    #endregion BankReconciliation

//                                    #region CostCenter

//                                    bool isCostCenter = false;

//                                    // Centro de costos
//                                    massiveEntryDTO.CostCenterId = rows[index][13] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][13]);
//                                    if (massiveEntryDTO.CostCenterId > 0)
//                                    {
//                                        // Se comprueba que es un centro de costos válido
//                                        List<CostCenter> costCenters = DelegateService.glAccountingApplicationService.GetCostCenters();
//                                        isCostCenter = costCenters.Any(item => item.CostCenterId == massiveEntryDTO.CostCenterId);
//                                        if (!isCostCenter)
//                                        {
//                                            error = error + 1;
//                                            errorDescription = errorDescription == "" ? Global.CostCenterIdValidation : errorDescription + ", " + Global.CostCenterIdValidation;
//                                            massiveEntryDTO.CostCenterId = 0;
//                                        }
//                                        else
//                                        {
//                                            if (rows[index][15] == DBNull.Value)
//                                            {
//                                                error = error + 1;
//                                                errorDescription = errorDescription == "" ? Global.NoCostCenterPercentageValidation : errorDescription + ", " + Global.NoCostCenterPercentageValidation;
//                                                massiveEntryDTO.Percentage = 0;
//                                            }
//                                            else
//                                            {
//                                                if (Convert.ToDecimal(rows[index][15]) != 100)
//                                                {
//                                                    error = error + 1;
//                                                    errorDescription = errorDescription == "" ? Global.CostCenterPercentageValidation : errorDescription + ", " + Global.CostCenterPercentageValidation;
//                                                    massiveEntryDTO.Percentage = 0;
//                                                }
//                                                else
//                                                {
//                                                    massiveEntryDTO.Percentage = Convert.ToDecimal(rows[index][15]);
//                                                }
//                                            }
//                                        }
//                                    }

//                                    massiveEntryDTO.Percentage = rows[index][15] == DBNull.Value ? 0 : Convert.ToDecimal(rows[index][15]);

//                                    #endregion CostCenter

//                                    #region Analysis

//                                    bool isAnalysis = false;
//                                    // Análisis
//                                    massiveEntryDTO.AnalysisId = rows[index][16] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][16]);

//                                    if (massiveEntryDTO.AnalysisId > 0)
//                                    {
//                                        // Se comprueba que el código de análisis sea válido.
//                                        List<AnalysisCode> analysisCodes = DelegateService.glAccountingApplicationService.GetAnalysisCodes();
//                                        isAnalysis = analysisCodes.Any(item => item.AnalysisCodeId == massiveEntryDTO.AnalysisId);
//                                        if (!isAnalysis)
//                                        {
//                                            error = error + 1;
//                                            errorDescription = errorDescription == "" ? Global.AnalysisCodeIdValidation : errorDescription + ", " + Global.AnalysisCodeIdValidation;
//                                            massiveEntryDTO.AnalysisId = 0;
//                                        }
//                                        else
//                                        {
//                                            // Se valida el código de concepto
//                                            bool isConceptCode = false;

//                                            massiveEntryDTO.ConceptId = rows[index][18] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][18]);

//                                            if (massiveEntryDTO.ConceptId > 0)
//                                            {
//                                                List<AnalysisConceptAnalysisDTO> analysisConceptAnalyses = DelegateService.glAccountingApplicationService.GetPaymentConceptsByAnalysisCode(Convert.ToInt32(massiveEntryDTO.AnalysisId));
//                                                isConceptCode = analysisConceptAnalyses.Any(item => item.AnalysisConceptId == massiveEntryDTO.ConceptId);
//                                                if (!isConceptCode)
//                                                {
//                                                    error = error + 1;
//                                                    errorDescription = errorDescription == "" ? Global.ConceptCodeNotRelatedValidation : errorDescription + ", " + Global.ConceptCodeNotRelatedValidation;
//                                                    massiveEntryDTO.ConceptId = 0;
//                                                }
//                                                else
//                                                {
//                                                    massiveEntryDTO.ConceptId = Convert.ToInt32(rows[index][18]);
//                                                }

//                                                // Se valida clave de análisis
//                                                if (rows[index][20] == DBNull.Value)
//                                                {
//                                                    error = error + 1;
//                                                    errorDescription = errorDescription == "" ? Global.NoConceptKeyValidation : errorDescription + ", " + Global.NoConceptKeyValidation;
//                                                    massiveEntryDTO.ConceptKey = null;
//                                                }
//                                                else
//                                                {
//                                                    massiveEntryDTO.ConceptKey = Convert.ToString(rows[index][20]);
//                                                }

//                                                // Se valido descripción
//                                                if (rows[index][21] == DBNull.Value)
//                                                {
//                                                    error = error + 1;
//                                                    errorDescription = errorDescription == "" ? Global.NoAnalysisDescriptionValidation : errorDescription + ", " + Global.NoAnalysisDescriptionValidation;
//                                                    massiveEntryDTO.AnalysisDescription = null;
//                                                }
//                                                else
//                                                {
//                                                    massiveEntryDTO.AnalysisDescription = Convert.ToString(rows[index][21]);
//                                                }
//                                            }
//                                            else
//                                            {
//                                                error = error + 1;
//                                                errorDescription = errorDescription == "" ? Global.NoConceptCodeValidation : errorDescription + ", " + Global.NoConceptCodeValidation;
//                                                massiveEntryDTO.ConceptId = 0;
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        massiveEntryDTO.ConceptId = rows[index][18] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][18]);
//                                        massiveEntryDTO.ConceptKey = rows[index][20] == DBNull.Value ? null : Convert.ToString(rows[index][20]);
//                                        massiveEntryDTO.AnalysisDescription = rows[index][21] == DBNull.Value ? null : Convert.ToString(rows[index][21]);
//                                    }

//                                    #endregion Analysis

//                                    #region Postdated

//                                    bool isPostdated = false;
//                                    ExchangeRate postdatedExchangeRate = new ExchangeRate();
//                                    // Postfechados
//                                    massiveEntryDTO.PostdatedId = rows[index][22] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][22]);

//                                    if (massiveEntryDTO.PostdatedId > 0)
//                                    {
//                                        // Se comprueba que es un tipo de postfechado válido
//                                        List<PostDatedTypeModel> postDatedTypeModels = ServiceModelAssembler.PostDatedTypes(Enum.GetNames(typeof(PostDateTypes)));
//                                        isPostdated = postDatedTypeModels.Any(item => item.Id == massiveEntryDTO.PostdatedId);
//                                        if (!isPostdated)
//                                        {
//                                            error = error + 1;
//                                            errorDescription = errorDescription == "" ? Global.PostdatedIdValidation : errorDescription + ", " + Global.PostdatedIdValidation;
//                                            massiveEntryDTO.PostdatedId = 0;
//                                        }
//                                        else
//                                        {
//                                            // Se valida moneda
//                                            if (rows[index][24] == DBNull.Value)
//                                            {
//                                                error = error + 1;
//                                                errorDescription = errorDescription == "" ? Global.NoPostdatedCurrencyValidation : errorDescription + ", " + Global.NoPostdatedCurrencyValidation;
//                                                massiveEntryDTO.PostdatedCurrencyId = 0;
//                                            }
//                                            else
//                                            {
//                                                massiveEntryDTO.PostdatedCurrencyId = Convert.ToInt32(rows[index][24]);
//                                                // Se valida tasa de cambio
//                                                postdatedExchangeRate = _baseController.GetExchangeRateByCurrencyId(Convert.ToInt32(massiveEntryDTO.PostdatedCurrencyId));

//                                                massiveEntryDTO.PostdatedExchangeRate = postdatedExchangeRate.SellAmount;
//                                            }
//                                            // Se valida número de documento
//                                            if (rows[index][26] == DBNull.Value)
//                                            {
//                                                error = error + 1;
//                                                errorDescription = errorDescription == "" ? Global.NoPostdatedDocumentValidation : errorDescription + ", " + Global.NoPostdatedDocumentValidation;
//                                                massiveEntryDTO.PosdatedDocumentNumber = null;
//                                            }
//                                            else
//                                            {
//                                                massiveEntryDTO.PosdatedDocumentNumber = Convert.ToString(rows[index][26]);
//                                            }
//                                            // Se valida importe
//                                            if (rows[index][27] == DBNull.Value)
//                                            {
//                                                error = error + 1;
//                                                errorDescription = errorDescription == "" ? Global.NoPostdatedAmountValidation : errorDescription + ", " + Global.NoPostdatedAmountValidation;
//                                                massiveEntryDTO.PostdatedAmount = 0;
//                                            }
//                                            else
//                                            {
//                                                massiveEntryDTO.PostdatedAmount = Convert.ToDecimal(rows[index][27]);
//                                            }
//                                        }
//                                    }

//                                    massiveEntryDTO.PostdatedCurrencyId = rows[index][24] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][24]);
//                                    int postdatedCurrencyId = 0;
//                                    postdatedCurrencyId = rows[index][24] == DBNull.Value ? 0 : Convert.ToInt32(rows[index][24]);
//                                    postdatedExchangeRate = _baseController.GetExchangeRateByCurrencyId(postdatedCurrencyId);
//                                    massiveEntryDTO.PostdatedExchangeRate = postdatedExchangeRate.SellAmount;
//                                    massiveEntryDTO.PosdatedDocumentNumber = rows[index][26] == DBNull.Value ? null : Convert.ToString(rows[index][26]);
//                                    massiveEntryDTO.PostdatedAmount = rows[index][27] == DBNull.Value ? 0 : Convert.ToDecimal(rows[index][27]);

//                                    #endregion Postdated

//                                    // Método para grabar el error
//                                    MassiveEntryLogDTO massiveEntryLogDTO = new MassiveEntryLogDTO();
//                                    massiveEntryLogDTO.Id = 0; //autonumérico
//                                    massiveEntryLogDTO.ProcessDate = DateTime.Now;
//                                    massiveEntryLogDTO.OperationDate = massiveEntryDTO.OperationDate;
//                                    massiveEntryLogDTO.RowNumber = massiveEntryDTO.RowNumber;
//                                    massiveEntryLogDTO.ErrorDescription = errorDescription;
//                                    massiveEntryLogDTO.Enabled = true;
//                                    massiveEntryLogDTO.Success = true;

//                                    if (error > 0)
//                                    {
//                                        massiveEntryLogDTO.Success = false;
//                                    }

//                                    MassiveLog massiveLog = new MassiveLog();
//                                    massiveLog.Description = errorDescription;
//                                    massiveLog.Id = rowNumber;
//                                    massiveLog.OperationDate = massiveEntryDTO.OperationDate;
//                                    massiveLog.ProcessDate = DateTime.Now;
//                                    massiveLog.RowNumber = massiveEntryDTO.RowNumber;
//                                    massiveLog.Status = true;

//                                    if (error > 0)
//                                    {
//                                        massiveLog.Status = false;
//                                    }
//                                    massiveLogs.Add(massiveLog);


//                                    // Método para grabar el movimiento en la tabla de trabajo.
//                                    DelegateService.glAccountingApplicationService.SaveEntryMassiveLoadRequest(massiveEntryDTO);

//                                    // Graba el log
//                                    //DelegateService.glAccountingApplicationService.SaveEntryMassiveLoadLogRequest(massiveEntryLogDTO);
//                                    error = 0;
//                                    rowNumber++;
//                                }
//                            }
//                        }
//                        successful = true;
//                        TempData["MassiveLogs"] = massiveLogs;
//                        Session["MassiveLogs"] = massiveLogs;
//                    }
//                    else
//                    {
//                        message = "IncompleteHeader";
//                        successful = false;
//                    }
//                }
//            }
//            catch (FormatException)
//            {
//                message = "Exception";
//                successful = false;
//            }
//            catch (OverflowException)
//            {
//                message = "Exception";
//                successful = false;
//            }
//            catch (IndexOutOfRangeException)
//            {
//                message = "Exception";
//                successful = false;
//            }
//            catch (InvalidCastException)
//            {
//                message = "Exception";
//                successful = false;
//            }
//            catch (Exception)
//            {
//                message = "Exception";
//                successful = false;
//            }

//            stream.Close();

//            object[] jsonData = new object[2];

//            jsonData[0] = message;
//            jsonData[1] = successful;

//            return Json(jsonData, JsonRequestBehavior.AllowGet);
//        }

//        #endregion

//    }
//}