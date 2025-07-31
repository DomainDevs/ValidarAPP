using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.Reflection;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.ReportingServices;
using Reporting = Sistran.Core.Application.ReportingServices.Models;
using FormatModels = Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class ReportsController : Controller
    {
        #region Class

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
            /// <param name="report"></param>
            public void CreateWorker(Reporting.Report report)
            {
                IReportingService reportService = ServiceManager.Instance.GetService<IReportingService>();

                Thread thread = new Thread(() => reportService.GenerateReport(report));
                thread.Start();
            }

            /// <summary>
            /// CreateWorker
            /// </summary>
            /// <param name="report"></param>
            /// <param name="isExcel"></param>
            public void CreateWorker(Reporting.Report report, bool isExcel)
            {
                IReportingService reportService = ServiceManager.Instance.GetService<IReportingService>();
                if (isExcel)
                {
                    Thread thread = new Thread(() => reportService.GenerateMassiveReport(report, null));
                    thread.Start();
                }

            }

            /// <summary>
            /// CreateWorkerStructure
            /// </summary>
            /// <param name="report"></param>
            /// <param name="isExcel"></param>
            public void CreateWorkerStructure(Reporting.Report report, bool isExcel)
            {
                IReportingService reportService = ServiceManager.Instance.GetService<IReportingService>();

                if (isExcel)
                {
                    Thread thread = new Thread(() => reportService.GenerateFileByReport(report));
                    thread.Start();
                }
            }

            /// <summary>
            /// CreateWorkerReportByStoreProcedure
            /// </summary>
            /// <param name="report"></param>
            /// <param name="massiveReport"></param>
            public void CreateWorkerReportByStoreProcedure(Reporting.Report report, Reporting.MassiveReport massiveReport)
            {
                IReportingService reportService = ServiceManager.Instance.GetService<IReportingService>();

                Thread thread = new Thread(() => reportService.GenerateMassiveReport(report, massiveReport));
                thread.Start();
            }
        }

        #endregion

        #endregion

        #region Instance variables
        readonly CommonController _commonController = new CommonController();

        #endregion

        #region Actions

        /// <summary>
        /// GetReportTypeList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetReportTypeList()
        {
            List<object> reportTypes = new List<object>();

            reportTypes.Add(new
            {
                Id = 1,
                Description = @Global.DuePorfolio
            });
            reportTypes.Add(new
            {
                Id = 2,
                Description = @Global.AccountState
            });
            reportTypes.Add(new
            {
                Id = 3,
                Description = @Global.Collection
            });
            reportTypes.Add(new
            {
                Id = 4,
                Description = @Global.PayerPaymentDetail
            });
            return new UifSelectResult(reportTypes);
        }

        #endregion

        #region View

        /// <summary>
        /// ReceiptsAppliedList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ReceiptsAppliedList()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// ProviderTax
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ProviderTax()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// EvaluationCommissionsUnearned
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult EvaluationCommissionsUnearned()
        {
            try
            {
                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// IncomeStatement
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult IncomeStatement()
        {
            try
            {
                // Setear el valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// CostCenter
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CostCenter()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// ResultBoard
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ResultBoard()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// AutomaticEntry
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AutomaticEntry()
        {
            try
            {
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// AccountingAccount
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AccountingAccount()
        {
            try
            {
                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// WrittenCheck
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult WrittenCheck()
        {
            try
            {

                // Setear el valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

            return View();
        }

        /// <summary>
        /// BankLedger
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BankLedger()
        {

            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

            return View();
        }
        /// <summary>
        /// TrialBalance
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult TrialBalance()
        {
            try
            {

                ViewBag.Assets = ConfigurationManager.AppSettings["Assets"];
                ViewBag.Liabilities = ConfigurationManager.AppSettings["Liabilities"];
                ViewBag.Patrimony = ConfigurationManager.AppSettings["Patrimony"];
                ViewBag.Income = ConfigurationManager.AppSettings["Income"];
                ViewBag.Expenses = ConfigurationManager.AppSettings["Expenses"];

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GeneralLedger
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GeneralLedger()
        {
            try
            {
                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// BalanceSheet
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BalanceSheet()
        {
            try
            {

                ViewBag.Assets = ConfigurationManager.AppSettings["Assets"];
                ViewBag.Liabilities = ConfigurationManager.AppSettings["Liabilities"];
                ViewBag.Patrimony = ConfigurationManager.AppSettings["Patrimony"];
                ViewBag.Income = ConfigurationManager.AppSettings["Income"];
                ViewBag.Expenses = ConfigurationManager.AppSettings["Expenses"];

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// CashFlow
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CashFlow()
        {
            try
            {

                ViewBag.Assets = ConfigurationManager.AppSettings["Assets"];
                ViewBag.Liabilities = ConfigurationManager.AppSettings["Liabilities"];
                ViewBag.Income = ConfigurationManager.AppSettings["Income"];
                ViewBag.Expenses = ConfigurationManager.AppSettings["Expenses"];

                //Setea el valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// ProfitsAndLosses
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ProfitsAndLosses()
        {

            try
            {

                ViewBag.Income = ConfigurationManager.AppSettings["Income"];
                ViewBag.Expenses = ConfigurationManager.AppSettings["Expenses"];

                //Setea el valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// AccountInventory
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AccountInventory()
        {
            try
            {

                ViewBag.Assets = ConfigurationManager.AppSettings["Assets"];
                ViewBag.Liabilities = ConfigurationManager.AppSettings["Liabilities"];
                ViewBag.Patrimony = ConfigurationManager.AppSettings["Patrimony"];
                ViewBag.Income = ConfigurationManager.AppSettings["Income"];
                ViewBag.Expenses = ConfigurationManager.AppSettings["Expenses"];

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// FinalBalance
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult FinalBalance()
        {
            try
            {

                ViewBag.Assets = ConfigurationManager.AppSettings["Assets"];
                ViewBag.Liabilities = ConfigurationManager.AppSettings["Liabilities"];
                ViewBag.Patrimony = ConfigurationManager.AppSettings["Patrimony"];
                ViewBag.Income = ConfigurationManager.AppSettings["Income"];
                ViewBag.Expenses = ConfigurationManager.AppSettings["Expenses"];

                //Setea elvalor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }
        /// <summary>
        /// ComparativeBalanceSheet
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ComparativeBalanceSheet()
        {
            try
            {
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GeneralJournal
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GeneralJournal()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// CondensedBalanceSheet
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CondensedBalanceSheet()
        {
            try
            {
                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// PremiumReceivableAntiquityList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PremiumReceivableAntiquityList()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// PortfolioCollectedList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PortfolioCollectedList()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// PendingApplicationList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PendingApplicationList()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// PaymentOrders
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PaymentOrders()
        {
            try
            {
                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// OperationsIssuedList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult OperationsIssuedList()
        {

            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainViewReport
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainViewReport()
        {
            try
            {
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// IssuedChecks
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult IssuedChecks()
        {

            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// IntermediaryTax
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult IntermediaryTax()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// ClientTax
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult ClientTax()
        {

            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// BillingMovementList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BillingMovementList()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// BrokerExpiredPortfolioList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BrokerExpiredPortfolioList()
        {
            try
            {
                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// DailyClosingList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult DailyClosingList()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// DailyIncomeDetail
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult DailyIncomeDetail()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// DailyOutcomeDetails
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult DailyOutcomeDetails()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// Analysis
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Analysis()
        {

            try
            {
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// PortfolioByBrokerList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PortfolioByBrokerList()
        {
            try
            {

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// PrimeDebtorList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PrimeDebtorList()
        {
            try
            {

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// CurrentPoliciesList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CurrentPoliciesList()
        {
            try
            {
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// TaxRetentionList
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult TaxRetentionList()
        {
            try
            {

                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.SupplierCode = Convert.ToInt32(ConfigurationManager.AppSettings["SupplierCode"]); // 10
                ViewBag.ProducerCode = Convert.ToInt32(ConfigurationManager.AppSettings["ProducerCode"]); //1
                ViewBag.EmployeeCode = Convert.ToInt32(ConfigurationManager.AppSettings["EmployeeCode"]);//11

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// PerceivedRetentions
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PerceivedRetentions()
        {
            try
            {

                // Presenta la Sucursal predefinida de cada usuario 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// AuxiliaryLedger
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AuxiliaryLedger()
        {
            try
            {
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
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
            Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();

            List<object> massiveReportsResponses = new List<object>();

            int userId = 0;
            if (User != null)

            {
                userId = _commonController.GetUserIdByName(User.Identity.Name);
            }

            else
            {
                userId = Convert.ToInt32(ConfigurationManager.AppSettings["UnitTestUserId"]);
            }

            try
            {
                massiveReport.UserId = userId;
                massiveReport.Description = reportName;
                massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                List<Reporting.MassiveReport> massiveReports = DelegateService.reportingService.GetMassiveReports(massiveReport);
                List<Reporting.MassiveReport> massiveReportsOrders = massiveReports.OrderByDescending(x => x.Id).ToList();


                if (massiveReports.Count > 0)
                {
                    double dayElapsed = 0;
                    double minuteElapsed = 0;
                    double progress = 0;
                    double hourElapsed = 0;
                    int order = 1;

                    TimeSpan timeSpan;

                    foreach (Reporting.MassiveReport masiveReport in massiveReportsOrders)
                    {
                        if (masiveReport.RecordsProcessed == 0 && masiveReport.RecordsNumber > 0)
                        {
                            masiveReport.RecordsProcessed = 1;
                        }

                        if (masiveReport.RecordsProcessed > 0)
                        {
                            progress = (Convert.ToDouble(masiveReport.RecordsProcessed)) / Convert.ToDouble(masiveReport.RecordsNumber);

                            if (progress < 1)
                            {
                                timeSpan = DateTime.Now - masiveReport.StartDate;
                            }
                            else
                            {
                                timeSpan = masiveReport.EndDate - masiveReport.StartDate;
                            }

                            dayElapsed = timeSpan.Days;
                            hourElapsed = timeSpan.Hours;
                            minuteElapsed = timeSpan.Minutes;
                        }

                        massiveReportsResponses.Add(new
                        {
                            Order = order,
                            ProcessId = masiveReport.Id,
                            Description = masiveReport.Description,
                            RecordsNumber = masiveReport.RecordsNumber,
                            RecordsProcessed = masiveReport.RecordsProcessed,
                            Progress = progress.ToString("P", CultureInfo.InvariantCulture),
                            Elapsed = System.Math.Truncate(dayElapsed) + " d " + System.Math.Truncate(hourElapsed) + " h " + System.Math.Truncate(minuteElapsed) + " m",
                            UrlFile = masiveReport.UrlFile,
                            Status = masiveReport.Success
                        });
                        order++;
                    }
                }

                return new UifTableResult(massiveReportsResponses);
            }
            catch (Exception)
            {
                massiveReportsResponses.Add(new
                {
                    ProcessId = -1
                });
                return new UifTableResult(massiveReportsResponses);
            }
        }

        /// <summary>
        /// GenerateStructureReportMassive
        /// Crea un archivo que se guarda en el servidor(metodo Asicrono no tiene pruebas Test)
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="reportTypeDescription"></param>
        /// <param name="exportFormatType"></param>
        /// <param name="recordsNumber"></param>
        /// <returns>Json</returns>
        public JsonResult GenerateStructureReportMassive(int processId, string reportTypeDescription,
                                                         int exportFormatType, decimal recordsNumber)
        {
            ParameterModel parameter = new ParameterModel();
            parameter.IsObject = false;
            Reporting.Report report = new Reporting.Report();
            List<object> reportsResults = new List<object>();

            int formatId = 0;
            string storedProcedureName = "";
            string exportedFileName = "";

            try
            {
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();
                List<Reporting.Parameter> parameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    report.Filter = "";

                    #region CashIncome

                    #region Movimientos de caja ingresos

                    // MOVIMIENTOS DE CAJA INGRESOS
                    if (reportTypeDescription == @Global.BillingMovementList.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatBillingMovements"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameBillingMovements"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetBillingMovements"];
                    }

                    #endregion

                    #region Detalle diario de ingresos

                    // DETALLE DIARIO DE INGRESOS
                    if (reportTypeDescription == @Global.DailyIncomeDetail.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatDayliIncomeDetail"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameDayliIncomeDetail"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetDayliIncomeDetail"];
                    }

                    #endregion

                    #endregion

                    #region CashAdvance

                    #region Cierre diario de caja egresos

                    // CIERRE DIARIO DE CAJA EGRESOS
                    if (reportTypeDescription == @Global.CashClosingList.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatDailyClosing"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameDailyClosing"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetDailyClosing"];
                    }

                    #endregion

                    #region Detalle diario de egresos

                    // DETALLE DIARIO DE EGRESOS
                    if (reportTypeDescription == @Global.DailyOutcomeDetails.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatDailyOutcomeDetails"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameDailyOutcomeDetails"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetDailyOutcomeDetails"];
                    }

                    #endregion

                    #region Cheques Emitidos

                    // CHEQUES EMITIDOS
                    if (reportTypeDescription == @Global.IssuedChecks.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatIssuedChecks"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameIssuedChecks"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetIssuedChecks"];
                    }

                    #endregion

                    #region Ordenes de Pago Emitidas

                    // ORDENES DE PAGO EMITIDAS
                    if (reportTypeDescription == @Global.PaymentOrders.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatPaymentOrders"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNamePaymentOrders"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetPaymentOrders"];
                    }

                    #endregion

                    #endregion

                    #region Accounting

                    #region Taxes_levies

                    #region ClientTax

                    // IMPUESTO POR CLIENTES
                    if (reportTypeDescription == @Global.ClientTax.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatClientTax"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameClientTax"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetClientTax"];
                    }

                    #endregion

                    #region AgentTax

                    // IMPUESTO POR AGENTE
                    if (reportTypeDescription == @Global.AgentTax.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatAgentTax"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameAgentTax"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetAgentTax"];
                    }

                    #endregion

                    #region ProviderTax

                    // IMPUESTO POR PROVEEDOR
                    if (reportTypeDescription == @Global.ProviderTax.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatProviderTax"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameProviderTax"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetProviderTax"];
                    }

                    #endregion

                    #endregion

                    #region closing

                    #region IncomeStatement

                    // BALANCE DE SALDOS
                    if (reportTypeDescription == @Global.IncomeStatement.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatIncomeStatement"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameIncomeStatement"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetIncomeStatement"];
                    }

                    #endregion

                    #endregion

                    #region Financial

                    #region WrittenCheck

                    // CHEQUES GIRADOS
                    if (reportTypeDescription == @Global.WrittenCheck.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatWrittenCheck"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameWrittenCheck"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetWrittenCheck"];
                    }

                    #endregion

                    #region BankLedger

                    // LIBRO DE BANCOS
                    if (reportTypeDescription == @Global.BankLedger.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatBankLedger"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameBankLedger"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetBankLedger"];
                    }

                    #endregion

                    #region Account

                    //ANALISIS DE TRANSACCIONES CONTABLES
                    if (reportTypeDescription == @Global.AnalysisAccountTransaction.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["FormatAnalysisDetailCd"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameAnalysis"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetEntryAnalysisAccount"];
                    }

                    #endregion

                    #endregion

                    #region Accounts

                    #region CostCenter

                    // CENTRO DE COSTOS
                    if (reportTypeDescription == @Global.CostCenter.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatCostCenter"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameCostCenter"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetCostCenter"];

                    }

                    #endregion

                    #region ResultBoard

                    // CUADRO DE RESULTADOS
                    if (reportTypeDescription == @Global.ResultBoard.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatResultBoard"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameResultBoard"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetResultBoard"];
                    }

                    #endregion

                    #region ProfitsAndLosses

                    // PÉRDIDAS Y GANANCIAS
                    if (reportTypeDescription == @Global.ProfitsAndLosses.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatProfitsAndLosses"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameProfitsAndLosses"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetProfitsAndLosses"];
                    }

                    #endregion

                    #region AccountInventory

                    // INVENTARIO DE CUENTAS
                    if (reportTypeDescription == @Global.AccountInventory.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatAccountInventory"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameAccountInventory"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetAccountInventory"];
                    }

                    #endregion

                    #region AccountingAccountEntry

                    // CONSULTA CUENTAS EN ASIENTO

                    if (reportTypeDescription == @Global.AccountingAccountConsult.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatAccountingAccountEntry"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameAccountingAccountEntry"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetAccountingAccountEntry"];
                    }

                    #region AutomaticEntry

                    // CUADRO DE RESULTADOS
                    if (reportTypeDescription == @Global.AutomaticEntry.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatAutomaticEntry"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameAutomaticEntry"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetAutomaticEntry"];
                    }

                    #endregion

                    #endregion

                    #endregion

                    #region TaxRetention

                    if (reportTypeDescription == (@Global.SearchTaxesRetention.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["FormatTaxRetention"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameTaxRetention"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetTaxRetention"];
                    }

                    #region Accountant

                    #region TrialBalance

                    // BALANCE DE COMPROBACIÓN
                    if (reportTypeDescription == @Global.TrialBalanceDetail.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatTBDetail"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameTBDetail"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetTrialBalance"];
                    }
                    if (reportTypeDescription == @Global.TrialBalanceTotal.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatTBTotal"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameTBTotales"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetTrialBalance"];
                    }

                    #endregion

                    #region BalanceSheet

                    if (reportTypeDescription == @Global.BalanceSheet.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatBalanceSheet"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameBalanceSheet"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetBalanceSheet"];
                    }
                    #endregion

                    #region CashFlow

                    if (reportTypeDescription == @Global.CashFlow.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatBalanceSheet"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameCashFlow"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetBalanceSheet"];
                    }

                    #endregion

                    #region FinalBalance

                    if (reportTypeDescription == @Global.FinalBalance.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatFinalBalance"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameFinalBalance"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetFinalBalance"];
                    }

                    #endregion

                    #region General Ledger

                    if (reportTypeDescription == @Global.GeneralLedgerReport.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatGLTotal"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameGL"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetGeneralLedger"];
                    }

                    #endregion

                    #region AuxiliaryLedger

                    if (reportTypeDescription == @Global.AuxiliaryLedger.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatAuxiliarLedger"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameAuxiliarLedger"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetAuxiliarLedger"];
                    }

                    #endregion

                    #region GeneralJournal

                    if (reportTypeDescription == @Global.GeneralJournal.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatGeneralJournal"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameGeneralJournal"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetGeneralJournal"];
                    }

                    #endregion

                    #region ComparativeBalanceSheet

                    if (reportTypeDescription == @Global.ComparativeBalanceSheet.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatComparativeBalance"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameComparativeBalance"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetComparativeBalance"];
                    }

                    #endregion

                    #region CondensedBalanceSheet

                    if (reportTypeDescription == @Global.CondensedBalanceSheet.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatCondensedBalanceSheet"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameCondensedBalanceSheet"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetCondensedBalanceSheet"];
                    }

                    #endregion

                    #endregion

                    #endregion

                    #endregion

                    #region Portfolio

                    #region Recibos Aplicados

                    //RECIBOS APLICADOS
                    if (reportTypeDescription == @Global.ReceiptsApplied.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatReceipts"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameReceiptsApplied"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetReceipts"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 5,
                            Description = "@IS_APPLIED",
                            IsFormula = false,
                            Value = -1
                        });
                    }

                    #endregion

                    #region Recibos Pendientes de AplicaciÃ³n

                    //RECIBOS NO APLICADOS
                    if (reportTypeDescription == @Global.PendingApplication.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatReceipts"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNamePendingApplicationList"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetReceipts"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 5,
                            Description = "@IS_APPLIED",
                            IsFormula = false,
                            Value = 0
                        });
                    }

                    #endregion

                    #region Operaciones Emitidas

                    //OPERACIONES EMITIDAS
                    if (reportTypeDescription == @Global.OperationsIssued.ToUpper())
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatOperationsIssued"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameOperationsIssued"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetOperationsIssued"];

                    }

                    #endregion

                    #region Cartera Recaudada

                    // CARTERA RECAUDADA
                    if (reportTypeDescription == (@Global.PortfolioCollected.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatPortfolioCollected"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNamePortfolioCollected"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetPortfolioCollected"];
                    }

                    #endregion

                    #region Cartera Vencida por Corredor

                    // CARTERA VENCIDA POR CORREDOR
                    if (reportTypeDescription == (@Global.BrokerExpiredPortfolio.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatBrokerExpiredPortfolio"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameBrokerExpiredPortfolio"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetBrokerExpiredPortfolio"];
                    }

                    #endregion

                    #region Antiguedad de Primas por Cobrar

                    // Antiguedad de Deuda
                    if (reportTypeDescription == (@Global.DuePorfolio.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatDuePorfolio"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameDuePorfolio"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetDuePorfolio"];
                    }

                    // Detalle de deuda por pagador
                    if (reportTypeDescription == (@Global.PayerPaymentDetail.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatPayerPaymentDetail"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNamePayerPaymentDetail"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetPayerPaymentDetail"];
                    }

                    // Estado de Cuenta
                    if (reportTypeDescription == (@Global.AccountState.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatAccountState"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameAccountState"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetAccountState"];
                    }

                    // Recaudos
                    if (reportTypeDescription == (@Global.Collection.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["FormatCollection"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameCollection"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetCollection"];
                    }

                    #endregion

                    #region Cartera por corredor

                    if (reportTypeDescription == (@Global.PortfolioByBroker.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["FormatPortfolioByBrokerCd"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNamePortfolioByBrokerList"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGePortfolioByBrokerList"];
                    }

                    #endregion

                    #region Deudores por prima

                    if (reportTypeDescription == (@Global.DebtorsByPrime.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["FormatDebtorsByPrimeCd"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameDebtorsByPrime"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetDebtorsByPrime"];
                    }

                    #endregion

                    #region Polizas Vigentes

                    if (reportTypeDescription == (@Global.CurrentPolicies.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["FormatCurrentPolicy"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameCurrentPolicy"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetCurrentPolicy"];
                    }

                    #endregion

                    #endregion

                    #region RetentionPolicy

                    if (reportTypeDescription == (@Global.PoliciesRetention.ToUpper()))
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["FormatRetentionPolicy"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameRetentionPolicy"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetRetentionPolicy"];
                    }
                    #endregion

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = processId
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@RECORD_COUNT",
                        IsFormula = false,
                        Value = recordsNumber
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@PAGE_SIZE",
                        IsFormula = false,
                        Value = Int32.Parse(ConfigurationManager.AppSettings["PageSizeReport"])
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@PAGE_NUMBER",
                        IsFormula = false,
                        Value = 1
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };
                }

                report.UserId = _commonController.GetUserIdByName(User.Identity.Name);

                report.ExportType = Reporting.ExportTypes.Excel;
                report.IsAsync = true;
                report.Description = @Global.GenerateDocument;

                report.Format = new FormatModels.Format()
                {
                    Id = formatId,
                    FileType = FormatModels.FileTypes.Excel
                };

                exportedFileName = "20";

                FormatModels.Format format = new FormatModels.Format();
                format.Id = formatId;
                format.FileType = FormatModels.FileTypes.Text;

                List<FormatModels.FormatDetail> formatDetails = DelegateService.reportingService.GetFormatDetailsByFormat(format);

                #region valida campo dinámico

                if (reportTypeDescription == (@Global.DuePorfolio.ToUpper()))
                {
                    parameters.Add(new Reporting.Parameter
                    {
                        Description = "@WORD_RESERVE",
                        Value = ConfigurationManager.AppSettings["WordReserve"]
                    });

                    report.Parameters = parameters;
                }

                #endregion

                if (formatDetails.Count > 0)
                {
                    WorkerFactory.Instance.CreateWorkerStructure(report, true);
                }
                else
                {
                    exportedFileName = "-1";
                }

                reportsResults.Add(new
                {
                    ExportedFileName = exportedFileName
                });
            }
            catch (Exception ex)
            {
                reportsResults = new List<object>();
                List<string> errorMessage = new List<string>();
                errorMessage.Add(@Global.FailedToGeneratePDF + " --> " + ex.Message);

                if ((ex.InnerException != null) && (ex.InnerException.Message != null))
                {
                    errorMessage.Add(ex.InnerException.Message);
                }

                reportsResults.Add(new
                {
                    ExportedFileName = "",
                    ErrorInfo = errorMessage
                });
            }

            return Json(reportsResults, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ValidateReportFile
        /// Valida Existencia de archivo generado en reportes 
        /// </summary>
        /// <param name="urlReport"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateReportFile(string urlReport)
        {

            var result = false;
            var message = "";
            try
            {
                var firtPos = urlReport.LastIndexOf("/") + 1;
                var endPos = urlReport.Length - firtPos;
                var cadena = urlReport.Substring(firtPos, endPos);
                string path = "";

                string folder = ConfigurationManager.AppSettings["SharedPortableDocumentFolder"];
                path = @folder + cadena;
                if (System.IO.File.Exists(path))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                result = false;
            }

            return Json(new
            {
                success = result,
                message
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CashIncome

        #region billingMovements

        /// <summary>
        /// GetTotalRecordsBillingMovements
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="cashierId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsBillingMovements(string dateFrom, string dateTo, string branchId,
                                                          string cashierId)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;

            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameBillingMovements"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@CASHIER_CD",
                        IsFormula = false,
                        Value = cashierId
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// BillingMovementsReports
        /// Reporte de Movimientos diarios de caja ingresos
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="cashierId"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult BillingMovementsReports(string dateFrom, string dateTo,
                                                  string branchId, string cashierId, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // MOVIMIENTOS DE CAJA
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatBillingMovements"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameBillingMovements"];
                        massiveReport.Description = @Global.BillingMovementList.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameBillingMovements"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@CASHIER_CD",
                            IsFormula = false,
                            Value = cashierId
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }


        #endregion

        #region DailyIncomeDetail

        /// <summary>
        /// GetTotalRecordsDailyIncome
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsDailyIncome(string dateFrom, string dateTo, string branchId)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameDayliIncomeDetail"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// DailyIncomeDetailReports
        /// Detalle de diario de ingresos
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DailyIncomeDetailReports(string dateFrom, string dateTo, string branchId,
                                                   int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // MOVIMIENTOS DE CAJA
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatDayliIncomeDetail"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameDayliIncomeDetail"];
                        massiveReport.Description = @Global.DailyIncomeDetail.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameDayliIncomeDetail"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }
        #endregion

        #endregion

        #region CashAdvance

        #region DailyClosingList

        /// <summary>
        /// GetTotalRecordsDailyClosing
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsDailyClosing(string dateFrom, string dateTo, string branchId)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameDailyClosing"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// DailyClosingReports
        /// Ejecuta el proceso de generacion de datos, guardando en una tabla temporal(metodo Asicrono  no tiene pruebas Test)
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DailyClosingReports(string dateFrom, string dateTo,
                                              string branchId, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // CIERRES DIARIOS DE CAJA EGRESOS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatDailyClosing"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameDailyClosing"];
                        massiveReport.Description = @Global.CashClosingList.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameDailyClosing"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = branchId
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #region DailyOutcomeDetails

        /// <summary>
        /// GetTotalRecordsDailyOutcomeDetails
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsDailyOutcomeDetails(string dateFrom, string dateTo, string branchId)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameDailyOutcomeDetails"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// DailyOutcomeDetailsReports
        /// Detalle de diario de egresos
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DailyOutcomeDetailsReports(string dateFrom, string dateTo,
                                                     string branchId, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // MOVIMIENTOS DE CAJA
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatDailyOutcomeDetails"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameDailyOutcomeDetails"];
                        massiveReport.Description = @Global.DailyOutcomeDetails.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameDailyOutcomeDetails"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }
        #endregion

        #region IssuedChecks

        /// <summary>
        /// GetTotalRecordsIssuedChecks
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>        
        /// <param name="numberAccountBank"></param>
        /// <param name="operation"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsIssuedChecks(string dateFrom, string dateTo,
                                                     string branchId, string bankId, string numberAccountBank, string operation)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameIssuedChecks"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@BANK_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(bankId)) ? 0 : Convert.ToInt32(bankId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@BANK_NUMBER",
                        IsFormula = false,
                        Value = numberAccountBank
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@OPERATION",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(operation)) ? 0 : Convert.ToInt32(operation)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 7,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 8,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// IssuedChecksReports
        /// Cheques Emitidos
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        /// <param name="numberAccountBank"></param>
        /// <param name="operation"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult IssuedChecksReports(string dateFrom, string dateTo, string branchId, string bankId,
                                              string numberAccountBank, string operation, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // CHEQUES EMITIDOS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatIssuedChecks"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameIssuedChecks"];
                        massiveReport.Description = @Global.IssuedChecks.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameIssuedChecks"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = Convert.ToInt32(branchId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@BANK_CD",
                            IsFormula = false,
                            Value = Convert.ToInt32(bankId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 5,
                            Description = "@BANK_NUMBER",
                            IsFormula = false,
                            Value = numberAccountBank
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 6,
                            Description = "@OPERATION",
                            IsFormula = false,
                            Value = Convert.ToInt32(operation)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }
        #endregion

        #region PaymentOrders

        /// <summary>
        /// GetTotalRecordsPaymentOrders
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="operation"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsPaymentOrders(string dateFrom, string dateTo,
                                                        string branchId, string operation)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNamePaymentOrders"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = Convert.ToInt32(branchId)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@OPERATION",
                        IsFormula = false,
                        Value = Convert.ToInt32(operation)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// PaymentOrdersReports
        /// Ordenes de Pago Emitidas
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="operation"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult PaymentOrdersReports(string dateFrom, string dateTo, string branchId,
                                              string operation, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // ORDENES DE PAGO EMITIDAS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatPaymentOrders"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNamePaymentOrders"];
                        massiveReport.Description = @Global.PaymentOrders.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNamePaymentOrders"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = Convert.ToInt32(branchId)
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@OPERATION",
                            IsFormula = false,
                            Value = Convert.ToInt32(operation)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }
        #endregion

        #endregion

        #region Accounting

        #region Taxes_levies

        #region ClientTax

        /// <summary>
        /// GetTotalRecordsClientTax
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsClientTax(string month, string year)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameClientTax"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MONTH",
                        IsFormula = false,
                        //SMT-1914 Inicio
                        Value = !string.IsNullOrEmpty(month) ? Convert.ToInt32(month) : 0
                        //SMT-1914 Fin
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@YEAR",
                        IsFormula = false,
                        //SMT-1914 Inicio
                        Value = !string.IsNullOrEmpty(year) ? Convert.ToInt32(year) : 0
                        //SMT-1914 Fin
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        //SMT-1914 Inicio
                        Value = 0
                        //SMT-1914 Fin
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        //SMT-1914 Inicio
                        Value = 0
                        //SMT-1914 Fin
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// ClientTaxReports
        /// Impuestos por Cliente
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        //SMT-1914 Inicio
        //public JsonResult ClientTaxReports(string month, string year, int reportType)
        //SMT-1914 Fin
        public JsonResult ClientTaxReports(int month, int year, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatClientTax"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameClientTax"];
                        massiveReport.Description = @Global.ClientTax.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameClientTax"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@MONTH",
                            IsFormula = false,
                            Value = month
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@YEAR",
                            IsFormula = false,
                            Value = year
                        });

                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #region AgentTax

        /// <summary>
        /// GetTotalRecordsAgentTax
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsAgentTax(string month, string year)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameAgentTax"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(month)) ? 0 : Convert.ToInt32(month)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(year)) ? 0 : Convert.ToInt32(year)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// AgentTaxReports
        /// Impuestos por Agente
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AgentTaxReports(string month, string year, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatAgentTax"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameAgentTax"];
                        massiveReport.Description = @Global.AgentTax.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameAgentTax"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@MONTH",
                            IsFormula = false,
                            Value = month
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@YEAR",
                            IsFormula = false,
                            Value = year
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #region ProviderTax

        /// <summary>
        /// GetTotalRecordsProviderTax
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsProviderTax(string month, string year)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameProviderTax"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(month)) ? 0 : Convert.ToInt32(month)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(year)) ? 0 : Convert.ToInt32(year)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// ProviderTaxReports
        /// Impuestos por Cliente
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ProviderTaxReports(string month, string year, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatProviderTax"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameProviderTax"];
                        massiveReport.Description = @Global.ProviderTax.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameProviderTax"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@MONTH",
                            IsFormula = false,
                            Value = month
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@YEAR",
                            IsFormula = false,
                            Value = year
                        });

                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #endregion

        #region Closing

        #region IncomeStatement

        /// <summary>
        /// GetTotalRecordsIncomeStatement
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsIncomeStatement(string month, string year)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameIncomeStatement"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = Convert.ToInt32(month)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = Convert.ToInt32(year)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// IncomeStatementReports
        /// Balance de Saldos
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult IncomeStatementReports(string month, string year, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatIncomeStatement"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameIncomeStatement"];
                        massiveReport.Description = @Global.IncomeStatement.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameIncomeStatement"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@MONTH",
                            IsFormula = false,
                            Value = Convert.ToInt32(month)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@YEAR",
                            IsFormula = false,
                            Value = Convert.ToInt32(year)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }


        #endregion

        #endregion

        #region Financial

        #region WrittenCheck

        /// <summary>
        /// GetTotalRecordsWrittenCheck
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsWrittenCheck(string dateFrom, string dateTo)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameWrittenCheck"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// WrittenCheckReports
        /// Ejecuta el proceso de generacion de datos, guardando en una tabla temporal(metodo Asicrono  no tiene pruebas Test)
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult WrittenCheckReports(string dateFrom, string dateTo, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // CIERRES DIARIOS DE CAJA EGRESOS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatWrittenCheck"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameWrittenCheck"];
                        massiveReport.Description = @Global.WrittenCheck.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameWrittenCheck"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #region BankLedger

        /// <summary>
        /// GetTotalRecordsBankLedger
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="accountNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsBankLedger(string branchId, string dateFrom,
                                                    string dateTo, string accountNumber)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameBankLedger"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@BRANCH_CD",
                        IsFormula = false,                        
                        Value = !string.IsNullOrEmpty(branchId) ? Convert.ToInt32(branchId) : 0                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@ACCOUNT_NUMBER",
                        IsFormula = false,
                        Value = accountNumber
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,                        
                        Value = 0                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@EXECUTE",
                        IsFormula = false,                        
                        Value = 0                        
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// BankLedgerReports
        /// Ejecuta el proceso de generacion de datos, guardando en una 
        /// tabla temporal(metodo Asicrono  no tiene pruebas Test)
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="accountNumber"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult BankLedgerReports(string branchId, string dateFrom,
                                            string dateTo, string accountNumber, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // CIERRES DIARIOS DE CAJA EGRESOS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatBankLedger"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameBankLedger"];
                        massiveReport.Description = @Global.BankLedger.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameBankLedger"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = Convert.ToInt16(branchId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@ACCOUNT_NUMBER",
                            IsFormula = false,
                            Value = accountNumber
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #endregion

        #region Accounts

        #region CostCenter

        /// <summary>
        /// GetTotalRecordsCostCenter
        /// </summary>
        /// <param name="costCenter"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="entries"></param>
        /// <param name="accountingNumberFrom"></param>        
        /// <param name="accountingNumberTo"></param>
        /// <param name="operation"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsCostCenter(string costCenter, string startDate, string endDate,
                                                     string entries, string accountingNumberFrom, string accountingNumberTo, string operation)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameCostCenter"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@COST_CENTER",
                        IsFormula = false,                        
                        Value = !string.IsNullOrEmpty(costCenter) ? Convert.ToInt32(costCenter) : 0                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = startDate
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = endDate
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@ENTRIES",
                        IsFormula = false,                        
                        Value = !string.IsNullOrEmpty(entries) ? Convert.ToInt32(entries) : 0
                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@OPERATION",
                        IsFormula = false,                        
                        Value = !string.IsNullOrEmpty(operation) ? Convert.ToInt32(operation) : 0
                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@ACCOUNTING_NUMBER_FROM",
                        IsFormula = false,
                        Value = accountingNumberFrom
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 7,
                        Description = "@ACCOUNTING_NUMBER_TO",
                        IsFormula = false,
                        Value = accountingNumberTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 8,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,                        
                        Value = 0
                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 9,
                        Description = "@EXECUTE",
                        IsFormula = false,                        
                        Value = 0                        
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// CostCenterReports
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="reportType"></param>
        /// <param name="costCenter"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="entries"></param>
        /// <param name="accountingNumberFrom"></param>
        /// <param name="accountingNumberTo"></param>
        /// <returns>JsonResult</returns> 
        public JsonResult CostCenterReports(string costCenter, string startDate, string endDate,
                                             string entries, string accountingNumberFrom, string accountingNumberTo, string operation, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // CENTRO DE COSTOS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatCostCenter"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameCostCenter"];
                        massiveReport.Description = @Global.CostCenter.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameCostCenter"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@COST_CENTER",
                            IsFormula = false,
                            Value = costCenter
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = startDate
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = endDate
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@ENTRIES",
                            IsFormula = false,
                            Value = entries
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 5,
                            Description = "@OPERATION",
                            IsFormula = false,
                            Value = operation
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 6,
                            Description = "@ACCOUNTING_NUMBER_FROM",
                            IsFormula = false,
                            Value = accountingNumberFrom
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 7,
                            Description = "@ACCOUNTING_NUMBER_TO",
                            IsFormula = false,
                            Value = accountingNumberTo
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }
        #endregion

        #region AccountingAccountEntry

        /// <summary>
        /// GetTotalRecordsAccountingAccount
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountingNumberFrom"></param>        
        /// <param name="accountingNumberTo"></param>
        /// <param name="operation"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsAccountingAccount(string startDate, string endDate,
                                                           string accountingNumberFrom, string accountingNumberTo, string operation)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;

            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameAccountingAccountEntry"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = startDate
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = endDate
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@ACCOUNTING_NUMBER_FROM",
                        IsFormula = false,
                        Value = accountingNumberFrom
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@ACCOUNTING_NUMBER_TO",
                        IsFormula = false,
                        Value = accountingNumberTo
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@OPERATION",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(operation)) ? 0 : Convert.ToInt32(operation)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 7,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// AccountingAccountReports
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="reportType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountingNumberFrom"></param>
        /// <param name="accountingNumberTo"></param>
        /// <returns>JsonResult</returns> 
        public JsonResult AccountingAccountReports(string startDate, string endDate,
                                                   string accountingNumberFrom, string accountingNumberTo, string operation, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // CONSULTA CUENTAS EN ASIENTOS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatAccountingAccountEntry"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameAccountingAccountEntry"];
                        massiveReport.Description = @Global.AccountingAccountConsult.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameAccountingAccountEntry"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = startDate
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = endDate
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@ACCOUNTING_NUMBER_FROM",
                            IsFormula = false,
                            Value = accountingNumberFrom
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@ACCOUNTING_NUMBER_TO",
                            IsFormula = false,
                            Value = accountingNumberTo
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 5,
                            Description = "@OPERATION",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(operation)) ? 0 : Convert.ToInt32(operation)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }
        #endregion

        #region Cuadro de Resultados

        /// <summary>
        /// GetTotalRecordsResultBoard
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsResultBoard(string branchId, string dateFrom, string dateTo)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = ResultBoardReports(branchId, dateFrom, dateTo, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// ResultBoardReports
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ResultBoardReports(string branchId, string dateFrom, string dateTo, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureResultBoard"];
                    massiveReport.Description = @Global.ResultBoard.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameResultBoard"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        //SMT-1914 Inicio
                        Value = !string.IsNullOrEmpty(branchId) ? Convert.ToInt32(branchId) : 0
                        //SMT-1914 Fin
                    });


                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    // Total de registros
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,                            
                            Value = 0                            
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,                            
                            Value = 0                            
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #region Contabilización de Asientos Automáticos

        /// <summary>
        /// GetTotalRecordsAutomaticEntry
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsAutomaticEntry(string year, string month)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = AutomaticEntrReports(year, month, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// AutomaticEntrReports
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AutomaticEntrReports(string year, string month, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureAutomaticEntry"];
                    massiveReport.Description = @Global.AutomaticEntry.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameAutomaticEntry"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(year)) ? 0 : Convert.ToInt32(year)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(month)) ? 0 : Convert.ToInt32(month)
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    // Total de registros
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }

                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #endregion

        #region Accountant

        #region TrialBalance

        /// <summary>
        /// GetTotalRecordsTrialBalance
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsTrialBalance(ParameterModel parameter)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = TrialBalanceReports(parameter, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// TrialBalanceReports
        /// Impuestos por Cliente
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult TrialBalanceReports(ParameterModel parameter, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameterModel = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameterModel.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureTrialBalance"];

                    massiveReport.Description = parameter.Operation == 0 ? @Global.TrialBalanceDetail.ToUpper() : @Global.TrialBalanceTotal.ToUpper();
                    massiveReport.UrlFile = parameter.Operation == 0 ? ConfigurationManager.AppSettings["TemplateNameTBDetail"] :
                                                                       ConfigurationManager.AppSettings["TemplateNameTBTotales"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = parameter.Year


                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH_FROM",
                        IsFormula = false,
                        Value = parameter.Month
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@MONTH_TO",
                        IsFormula = false,
                        Value = parameter.MonthTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = parameter.Branch
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@OPERATION",
                        IsFormula = false,
                        Value = parameter.Operation
                    });

                    #region ACCOUNT_PARENT_XML

                    XDocument accountDocument = default(XDocument);
                    XmlDocument documentXmlDocument = new XmlDocument();

                    if (parameter.All == -1)
                    {
                        accountDocument = new XDocument(
                            new XElement("ACCOUNT", new XElement("PARENT", new XElement("PARENT_ID", -1))));
                        documentXmlDocument.LoadXml(accountDocument.ToString());
                    }
                    else
                    {
                        accountDocument = new XDocument(
                            new XElement("ACCOUNT", new XElement("PARENT", new XElement("PARENT_ID", parameter.Assets.ToString())),
                                                    new XElement("PARENT", new XElement("PARENT_ID", parameter.Liabilities.ToString())),
                                                    new XElement("PARENT", new XElement("PARENT_ID", parameter.Patrimony.ToString())),
                                                    new XElement("PARENT", new XElement("PARENT_ID", parameter.Income.ToString())),
                                                    new XElement("PARENT", new XElement("PARENT_ID", parameter.Expenses.ToString()))));
                        documentXmlDocument.LoadXml(accountDocument.ToString());
                    }

                    #endregion

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@ACCOUNT_PARENT_XML",
                        IsFormula = false,
                        Value = documentXmlDocument.DocumentElement.OuterXml
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    // Total de registros
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #region GeneralJournal

        /// <summary>
        /// GetTotalRecordsGeneralJournal
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsGeneralJournal(int branch, string year, string month, string accountingAccountId)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = GeneralJournalReport(branch, year, month, accountingAccountId, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// GeneralJournalReport
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GeneralJournalReport(int branch, string year, string month, string accountingAccountId, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameterModel = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameterModel.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureGeneralJournal"];

                    massiveReport.Description = @Global.GeneralJournal.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameGeneralJournal"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@BRANCH_CD",
                        IsFormula = false,                        
                        Value =  branch                        
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = year
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH_FROM",
                        IsFormula = false,
                        Value = month
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@ACCOUNTING_ACCOUNT_ID",
                        IsFormula = false,                        
                        Value = !string.IsNullOrEmpty(accountingAccountId) ? Convert.ToInt32(accountingAccountId) : 0
                        
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    // Total de registros
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,                            
                            Value = 0                            
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,                            
                            Value = 0                            
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }
        #endregion

        #region GeneralLedger

        /// <summary>
        /// GetTotalRecordsGeneralLedger
        /// </summary>
        /// <param name="branchCd"></param>
        /// <param name="year"></param>
        /// <param name="monthFrom"></param>
        /// <param name="accountingAccountId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsGeneralLedger(int branchCd, string year, string monthFrom, int accountingAccountId)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = GeneralLedgerReports(branchCd, year, monthFrom, accountingAccountId, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// GeneralLedgerReports
        /// </summary>
        /// <param name="branchCd"></param>
        /// <param name="year"></param>
        /// <param name="monthFrom"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GeneralLedgerReports(int branchCd, string year, string monthFrom, int accountingAccountId, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameterModel = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameterModel.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureGeneralLedger"];

                    massiveReport.Description = @Global.GeneralLedgerReport.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameGL"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = branchCd
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = year
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@MONTH_FROM",
                        IsFormula = false,
                        Value = monthFrom
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@ACCOUNTING_ACCOUNT_ID",
                        IsFormula = false,
                        Value = accountingAccountId
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #region BalanceSheet

        /// <summary>
        /// GetTotalRecordsBalanceSheet
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsBalanceSheet(ParameterModel parameter)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = BalanceSheetReports(parameter, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// BalanceSheetReports
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult BalanceSheetReports(ParameterModel parameter, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameterModel = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameterModel.IsObject)
                {
                    if (parameter.Name == @Global.CashFlow)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureCashFlow"];
                        massiveReport.Description = @Global.CashFlow.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameCashFlow"];
                    }
                    else if (parameter.Name == @Global.FinalBalance)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureFinalBalance"];
                        massiveReport.Description = @Global.FinalBalance.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameFinalBalance"];
                    }
                    else
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureBalanceSheet"];
                        massiveReport.Description = @Global.BalanceSheet.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameBalanceSheet"];
                    }

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MONTH_FROM",
                        IsFormula = false,
                        Value = parameter.Month.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH_TO",
                        IsFormula = false,
                        Value = parameter.MonthTo.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = parameter.Year.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = parameter.Branch
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@ACCUMULATED",
                        IsFormula = false,
                        Value = parameter.Accumulated
                    });

                    #region ACCOUNT_PARENT_XML

                    XDocument accountDocument = default(XDocument);
                    XmlDocument documentXmlDocument = new XmlDocument();

                    accountDocument = new XDocument(
                             new XElement("ACCOUNT", new XElement("PARENT", new XElement("PARENT_ID", parameter.Assets.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Liabilities.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Patrimony.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Income.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Expenses.ToString()))));
                    documentXmlDocument.LoadXml(accountDocument.ToString());

                    #endregion

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@ACCOUNT_PARENT_XML",
                        IsFormula = false,
                        Value = documentXmlDocument.DocumentElement.OuterXml
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }
        #endregion

        #region CondensedBalanceSheet

        /// <summary>
        /// GetTotalRecordsCondensedBalanceSheet
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="brancCd"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsCondensedBalanceSheet(string month, string year, string brancCd)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = CondensedBalanceSheetReports(month, year, brancCd, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }
                else
                {
                    return new UifJsonResult(false, 0);
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// CondensedBalanceSheetReports
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="brancCd"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CondensedBalanceSheetReports(string month, string year, string brancCd, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameterModel = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameterModel.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureCondensedBalanceSheet"];
                    massiveReport.Description = @Global.CondensedBalanceSheet.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameCondensedBalanceSheet"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MONTH",
                        IsFormula = false,
                        Value = month
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = year
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(brancCd)) ? 0 : Convert.ToInt32(brancCd)
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }

                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }
        #endregion

        #region ProfitsAndLosses

        /// <summary>
        /// GetTotalRecordsProfitsAndLosses
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsProfitsAndLosses(ParameterModel parameter)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = ProfitsAndLossesReports(parameter, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// ProfitsAndLosses
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public JsonResult ProfitsAndLossesReports(ParameterModel parameter, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";

            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameterModel = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameterModel.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureProfitsAndLosses"];

                    massiveReport.Description = @Global.ProfitsAndLosses.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameProfitsAndLosses"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MONTH_FROM",
                        IsFormula = false,
                        Value = parameter.Month.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH_TO",
                        IsFormula = false,
                        Value = parameter.MonthTo.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = parameter.Year.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = parameter.Branch
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@ACCUMULATED",
                        IsFormula = false,
                        Value = parameter.Accumulated
                    });

                    #region ACCOUNT_PARENT_XML

                    XDocument accountDocument = default(XDocument);
                    XmlDocument documentXmlDocument = new XmlDocument();

                    accountDocument = new XDocument(
                             new XElement("ACCOUNT", new XElement("PARENT", new XElement("PARENT_ID", parameter.Income.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Expenses.ToString()))));
                    documentXmlDocument.LoadXml(accountDocument.ToString());

                    #endregion

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@ACCOUNT_PARENT_XML",
                        IsFormula = false,
                        Value = documentXmlDocument.DocumentElement.OuterXml
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #region AccountInventory

        /// <summary>
        /// GetTotalRecordsBalanceSheet
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsAccountInventory(ParameterModel parameter)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = AccountInventoryReports(parameter, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// AccountInventoryReports
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AccountInventoryReports(ParameterModel parameter, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";

            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameterModel = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameterModel.IsObject)
                {
                    if (parameter.Name == @Global.AccountInventory)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureAccountInventory"];
                        massiveReport.Description = @Global.AccountInventory.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameAccountInventory"];
                    }

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@MONTH_FROM",
                        IsFormula = false,
                        Value = parameter.Month.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH_TO",
                        IsFormula = false,
                        Value = parameter.MonthTo.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = parameter.Year.ToString()
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = parameter.Branch
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@ACCUMULATED",
                        IsFormula = false,
                        Value = parameter.Accumulated
                    });

                    #region ACCOUNT_PARENT_XML

                    XDocument accountDocument = default(XDocument);
                    XmlDocument documentXmlDocument = new XmlDocument();

                    accountDocument = new XDocument(
                             new XElement("ACCOUNT", new XElement("PARENT", new XElement("PARENT_ID", parameter.Assets.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Liabilities.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Patrimony.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Income.ToString())),
                                                     new XElement("PARENT", new XElement("PARENT_ID", parameter.Expenses.ToString()))));
                    documentXmlDocument.LoadXml(accountDocument.ToString());

                    #endregion

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@ACCOUNT_PARENT_XML",
                        IsFormula = false,
                        Value = documentXmlDocument.DocumentElement.OuterXml
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #region AuxiliaryLedger

        /// <summary>
        /// GetTotalAuxiliaryLedgerReport
        /// </summary>
        /// <param name="brandCd"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="accountNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalAuxiliaryLedgerReport(int brandCd, string dateFrom, string dateTo, string accountNumber)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = AuxiliaryLedgerReport(brandCd, dateFrom, dateTo, accountNumber, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// AuxiliaryLedgerReport
        /// </summary>
        /// <param name="brandCd"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="accountNumber"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AuxiliaryLedgerReport(int brandCd, string dateFrom, string dateTo, string accountNumber, int? process)
        {
            string storedProcedureName = "";
            int totalRecords = 0;
            Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
            ParameterModel parameter = new ParameterModel();
            Reporting.Report report = new Reporting.Report();
            List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

            try
            {
                if (!parameter.IsObject)
                {
                    massiveReport.Description = @Global.AuxiliaryLedger.ToUpper();
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureAuxiliarLedger"];
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameAuxiliarLedger"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = Convert.ToInt32(brandCd)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@ACCOUNT_NUMBER",
                        IsFormula = false,
                        Value = accountNumber
                    });
                }

                report.StoredProcedure = new Reporting.StoredProcedure()
                {
                    ProcedureName = storedProcedureName,
                    ProcedureParameters = procedureParameters
                };

                report.Parameters = null;
                report.ExportType = Reporting.ExportTypes.Excel;

                /*Total de registros*/
                if (process == -1)
                {
                    report.IsAsync = false;

                    report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 7,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                else
                {
                    /*Consulta de Datos*/
                    report.IsAsync = true;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #region ComparativeBalanceSheet

        /// <summary>
        /// GetTotalRecordsComparativeBalanceSheet
        /// </summary>
        /// <param name="year"></param>
        /// <param name="monthFrom"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="comparativeType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsComparativeBalanceSheet(string year, string monthFrom, int accountingAccountId, int comparativeType)
        {
            int totalRecords = 0;

            try
            {
                JsonResult jsonData = ComparativeBalanceSheetReports(year, monthFrom, accountingAccountId, comparativeType, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// ComparativeBalanceSheetReports
        /// </summary>
        /// <param name="year"></param>
        /// <param name="monthFrom"></param>
        /// <param name="accountingAccountId"></param>
        /// <param name="comparativeType"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ComparativeBalanceSheetReports(string year, string monthFrom, int accountingAccountId, int comparativeType, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";

            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameterModel = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameterModel.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureComparativeBalance"];

                    massiveReport.Description = @Global.ComparativeBalanceSheet.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameComparativeBalance"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@YEAR",
                        IsFormula = false,
                        Value = year
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@MONTH_FROM",
                        IsFormula = false,
                        Value = monthFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@ACCOUNTING_ACCOUNT_ID",
                        IsFormula = false,
                        Value = accountingAccountId
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@COMPARATIVE_TYPE",
                        IsFormula = false,
                        Value = comparativeType
                    });
                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }
        #endregion

        #endregion

        #region Analysis Transaction

        /// <summary>
        /// TotalAnalysisTransactionReport
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="analysisTypeCd"></param>
        /// <param name="analysisConceptCd"></param>
        /// <param name="currencyCd"></param>
        /// <param name="accountNumber"></param>
        /// <returns>UifJsonResult</returns>
        public JsonResult TotalAnalysisTransactionReport(string dateFrom, string dateTo, int analysisTypeCd,
                                                        int analysisConceptCd, int currencyCd, string accountNumber)
        {
            int totalRecords = 0;

            try
            {
                JsonResult jsonData = AnalysisTransactionReport(dateFrom, dateTo, analysisTypeCd, analysisConceptCd, currencyCd, accountNumber, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// AnalysisTransactionReport
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="analysisTypeCd"></param>
        /// <param name="analysisConceptCd"></param>
        /// <param name="currencyCd"></param>
        /// <param name="accountNumber"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult AnalysisTransactionReport(string dateFrom, string dateTo, int analysisTypeCd, int analysisConceptCd, int currencyCd,
                                                    string accountNumber, int? process)
        {
            string storedProcedureName = "";
            int totalRecords = 0;
            Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
            ParameterModel parameter = new ParameterModel();
            Reporting.Report report = new Reporting.Report();
            List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

            try
            {
                if (!parameter.IsObject)
                {
                    massiveReport.Description = @Global.AnalysisAccountTransaction.ToUpper();
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureEntryAnalysisAccount"];
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameAnalysis"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@ANALYSIS_TYPE_CD",
                        IsFormula = false,                        
                        Value = analysisTypeCd
                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@ANALYSIS_CONCEPT_CD",
                        IsFormula = false,                        
                        Value = analysisConceptCd
                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@CURRENCY_CD",
                        IsFormula = false,                        
                        Value = currencyCd                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@ACCOUNT_NUMBER",
                        IsFormula = false,
                        Value = accountNumber == "" ? "0" : accountNumber
                    });
                }

                report.StoredProcedure = new Reporting.StoredProcedure()
                {
                    ProcedureName = storedProcedureName,
                    ProcedureParameters = procedureParameters
                };

                report.Parameters = null;
                report.ExportType = Reporting.ExportTypes.Excel;

                /*Total de registros*/
                if (process == -1)
                {
                    report.IsAsync = false;

                    report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                        
                    });
                    report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 7,
                        Description = "@EXECUTE",
                        IsFormula = false,                        
                        Value = 0
                    });

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                else
                {
                    /*Consulta de Datos*/
                    report.IsAsync = true;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
            return new UifJsonResult(true, totalRecords);
        }
        #endregion

        #region TaxRetention

        /// <summary>
        /// GetTotalRecordsTaxRetention
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="currencyId"></param>
        /// <param name="paymentOrder"></param>
        /// <param name="beneficiaryTypeId"></param>
        /// <param name="individualId"></param>
        /// <param name="taxId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsTaxRetention(string branchId, string currencyId, string paymentOrder,
                                                        string beneficiaryTypeId, string individualId, string taxId)
        {
            int totalRecords = 0;

            try
            {
                JsonResult jsonData = TaxRetentionReports(branchId, currencyId, paymentOrder, beneficiaryTypeId, individualId, taxId, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// TaxRetentionReports
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="currencyId"></param>
        /// <param name="paymentOrder"></param>
        /// <param name="beneficiaryTypeId"></param>
        /// <param name="individualId"></param>
        /// <param name="taxId"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult TaxRetentionReports(string branchId, string currencyId, string paymentOrder,
                                              string beneficiaryTypeId, string individualId, string taxId, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";

            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureTaxRetention"];
                    massiveReport.Description = @Global.SearchTaxesRetention.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameTaxRetention"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = branchId == "" ? 0 : Int32.Parse(branchId)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "CURRENCY_CD",
                        IsFormula = false,
                        Value = currencyId == "" ? 0 : Int32.Parse(currencyId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@PAYMENT_ORDER",
                        IsFormula = false,
                        Value = paymentOrder == "" ? 0 : Int32.Parse(paymentOrder)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@INDIVIDUAL_ID",
                        IsFormula = false,
                        Value = individualId == "" ? "0" : individualId
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@TAX_ID",
                        IsFormula = false,
                        Value = taxId == "" ? 0 : Int32.Parse(taxId)
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #region PerceivedRetentions

        /// <summary>
        /// GetPerceivedRetentionsOperation
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPerceivedRetentionsOperation()
        {
            List<object> perceivedRetentionsOperation = new List<object>();

            perceivedRetentionsOperation.Add(new
            {
                Id = 1,
                Description = @Global.PoliciesRetention
            });
            perceivedRetentionsOperation.Add(new
            {
                Id = 2,
                Description = @Global.BillWithoutRetention
            });
            perceivedRetentionsOperation.Add(new
            {
                Id = 3,
                Description = @Global.AnulledRetention
            });
            perceivedRetentionsOperation.Add(new
            {
                Id = 4,
                Description = @Global.IncorrectRetention
            });

            return new UifSelectResult(perceivedRetentionsOperation);
        }

        /// <summary>
        /// GetTotalRecordsRetentionPolicy
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsRetentionPolicy(string dateFrom, string DateTo, string branchId)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = RetentionPolicyReports(dateFrom, DateTo, branchId, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// RetentionPolicyReports
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult RetentionPolicyReports(string dateFrom, string dateTo, string branchId, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureRetentionPolicy"];
                    massiveReport.Description = @Global.PoliciesRetention.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameRetentionPolicy"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = branchId == "" ? 0 : Int32.Parse(branchId)
                    });

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@USER_ID",
                        IsFormula = false,
                        Value = userId
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = userId;
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #endregion

        #region PortFolio

        #region ReceiptAppliedList

        /// <summary>
        /// GetTotalRecordsReceiptAppliedList
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="isApplied"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsReceiptsAppliedList(string dateFrom, string dateTo, string branchId, string isApplied)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;

            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameReceipts"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@IS_APPLIED",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(isApplied)) ? 0 : Convert.ToInt32(isApplied)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// ReceiptsAppliedListReports
        /// Pendientes de AplciaciÃ³n
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="isApplied"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReceiptsAppliedListReports(string dateFrom, string dateTo, string branchId, string isApplied, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // RECIBOS APLICADOS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatReceipts"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameReceipts"];
                        massiveReport.Description = @Global.ReceiptsApplied.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameReceiptsApplied"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@IS_APPLIED",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(isApplied)) ? 0 : Convert.ToInt32(isApplied)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }
        #endregion

        #region PendingApplicationListReports

        /// <summary>
        /// GetTotalRecordsPendingApplicationList
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="isApplied"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsPendingApplicationList(string dateFrom, string dateTo,
                                                                string branchId, string isApplied)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;

            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameReceipts"];
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@IS_APPLIED",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(isApplied)) ? 0 : Convert.ToInt32(isApplied)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// PendingApplicationListReports
        /// Recibos Aplicados
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="reportType"></param>
        /// <param name="isApplied"></param>
        /// <returns>JsonResult</returns>
        public JsonResult PendingApplicationListReports(string dateFrom, string dateTo, string branchId, string isApplied, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // RECIBOS APLICADOS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatReceipts"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameReceipts"];
                        massiveReport.Description = @Global.PendingApplication.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNamePendingApplicationList"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@IS_APPLIED",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(isApplied)) ? 0 : Convert.ToInt32(isApplied)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #region Operation

        /// <summary>
        /// GetTotalRecordsReceiptOperationsIssued
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="operation"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsOperationsIssued(string dateFrom, string dateTo,
                                                           string branchId, string operation)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameOperationsIssued"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@OPERATION",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(operation)) ? 0 : Convert.ToInt32(operation)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// OperationsIssuedReports
        /// Operaciones Emitidas
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="branchId"></param>
        /// <param name="operation"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult OperationsIssuedReports(string dateFrom, string dateTo, string branchId,
                                                  string operation, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // OPERACIONES EMITIDAS
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatOperationsIssued"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameOperationsIssued"];
                        massiveReport.Description = @Global.OperationsIssued.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameOperationsIssued"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@OPERATION",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(operation)) ? 0 : Convert.ToInt32(operation)
                        });
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #region PortfolioCollected

        /// <summary>
        /// GetTotalRecordsPortfolioCollected
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="listBy"></param>
        /// <param name="selectAll"></param>
        /// <param name="insuredId"></param>
        /// <param name="agentId"></param>
        /// <param name="userId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsPortfolioCollected(string branchId, string prefixId, string dateFrom,
                                                                string dateTo, string listBy, string selectAll,
                                                                string insuredId, string agentId, string userId)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNamePortfolioCollected"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@PREFIX_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(prefixId)) ? 0 : Convert.ToInt32(prefixId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@LIST_BY",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(listBy)) ? 0 : Convert.ToInt32(listBy)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 6,
                        Description = "@ALL",
                        IsFormula = false,
                        Value = selectAll
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 7,
                        Description = "@INSURED_INDIVIDUAL_ID",
                        IsFormula = false
                    });
                    if (insuredId == null || insuredId == "")
                    {
                        procedureParameters.Last().Value = null;
                        procedureParameters.Last().DbType = typeof(Int32);
                    }
                    else
                    {
                        procedureParameters.Last().Value = Int32.Parse(insuredId);
                    }

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 8,
                        Description = "@AGENT_INDIVIDUAL_ID",
                        IsFormula = false
                    });
                    if (agentId == null || agentId == "")
                    {
                        procedureParameters.Last().Value = null;
                        procedureParameters.Last().DbType = typeof(Int32);
                    }
                    else
                    {
                        procedureParameters.Last().Value = Int32.Parse(agentId);
                    }

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 9,
                        Description = "@USER_ID",
                        IsFormula = false
                    });
                    if (userId == null || userId == "")
                    {
                        procedureParameters.Last().Value = null;
                        procedureParameters.Last().DbType = typeof(Int32);
                    }
                    else
                    {
                        procedureParameters.Last().Value = Int32.Parse(userId);
                    }
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 10,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 11,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// PortfolioCollectedReports
        /// Cartera Recaudada
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="listBy"></param>
        /// <param name="selectAll"></param>
        /// <param name="insuredId"></param>
        /// <param name="agentId"></param>
        /// <param name="userId"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult PortfolioCollectedReports(string branchId, string prefixId, string dateFrom,
                                                        string dateTo, string listBy, string selectAll,
                                                        string insuredId, string agentId, string userId, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // CARTERA RECAUDADA
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatPortfolioCollected"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNamePortfolioCollected"];
                        massiveReport.Description = @Global.PortfolioCollected.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNamePortfolioCollected"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@PREFIX_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(prefixId)) ? 0 : Convert.ToInt32(prefixId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 5,
                            Description = "@LIST_BY",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(listBy)) ? 0 : Convert.ToInt32(listBy)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 6,
                            Description = "@ALL",
                            IsFormula = false,
                            Value = selectAll
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 7,
                            Description = "@INSURED_INDIVIDUAL_ID",
                            IsFormula = false
                        });
                        if (insuredId == null || insuredId == "")
                        {
                            procedureParameters.Last().Value = null;
                            procedureParameters.Last().DbType = typeof(Int32);
                        }
                        else
                        {
                            procedureParameters.Last().Value = Int32.Parse(insuredId);
                        }

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 8,
                            Description = "@AGENT_INDIVIDUAL_ID",
                            IsFormula = false
                        });
                        if (agentId == null || agentId == "")
                        {
                            procedureParameters.Last().Value = null;
                            procedureParameters.Last().DbType = typeof(Int32);
                        }
                        else
                        {
                            procedureParameters.Last().Value = Int32.Parse(agentId);
                        }

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 9,
                            Description = "@USER_ID",
                            IsFormula = false
                        });
                        if (userId == null || userId == "")
                        {
                            procedureParameters.Last().Value = null;
                            procedureParameters.Last().DbType = typeof(Int32);
                        }
                        else
                        {
                            procedureParameters.Last().Value = Int32.Parse(userId);
                        }
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #region BrokerExpiredPortfolio

        /// <summary>
        /// GetTotalRecordsBrokerExpiredPortfolio
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="listBy"></param>
        /// <param name="agentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsBrokerExpiredPortfolio(string dateTo, string listBy, string agentId)
        {
            string storedProcedureName = "";
            decimal totalRecords = 0;
            try
            {
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameBrokerExpiredPortfolio"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@LIST_BY",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(listBy)) ? 0 : Convert.ToInt32(listBy)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@AGENT_INDIVIDUAL_ID",
                        IsFormula = false
                    });
                    if (agentId == null || agentId.ToString() == "")
                    {
                        procedureParameters.Last().Value = null;
                        procedureParameters.Last().DbType = typeof(Int32);
                    }
                    else
                    {
                        procedureParameters.Last().Value = Int32.Parse(agentId);
                    }

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totalRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// BrokerExpiredPortfolioReports
        /// Cartera Recaudada
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="listBy"></param>
        /// <param name="agentId"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult BrokerExpiredPortfolioReports(string dateTo, string listBy, string agentId, int reportType)
        {
            try
            {
                string storedProcedureName = "";
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    // CARTERA VENCIDA POR CORREDOR
                    if (reportType == Convert.ToInt16(ConfigurationManager.AppSettings["IdFormatBrokerExpiredPortfolio"]))
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameBrokerExpiredPortfolio"];
                        massiveReport.Description = @Global.BrokerExpiredPortfolio.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameBrokerExpiredPortfolio"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@LIST_BY",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(listBy)) ? 0 : Convert.ToInt32(listBy)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@AGENT_INDIVIDUAL_ID",
                            IsFormula = false
                        });
                        if (agentId == null || agentId.ToString() == "null")
                        {
                            procedureParameters.Last().Value = null;
                            procedureParameters.Last().DbType = typeof(Int32);
                        }
                        else
                        {
                            procedureParameters.Last().Value = Int32.Parse(agentId);
                        }
                    }

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        #endregion

        #region PremiumReceivableAntiquity

        /// <summary>
        /// GetTotalRecordsPremiumReceivable
        /// </summary>
        /// <param name="businessTypeId"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="agentId"></param>
        /// <param name="insuredId"></param>
        /// <param name="cutoffDate"></param>
        /// <param name="rangeId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="dueDate"></param>
        /// <param name="reportType"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsPremiumReceivable(string businessTypeId, string branchId, string lineBusinessId, string agentId, string insuredId,
                                                           string cutoffDate, string rangeId, string dateFrom, string dateTo, string dueDate,
                                                           string prefixId, string reportType)
        {
            int totalRecords = 0;
            string errorResult = "";
            try
            {
                JsonResult jsonData = PremiumReceivableReports(businessTypeId, branchId, lineBusinessId, agentId, insuredId, cutoffDate, rangeId,
                                                        dateFrom, dateTo, dueDate, prefixId, reportType, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                object totalResult = jsonData.Data;
                if (success)
                {
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }
                else
                {
                    totalRecords = -1;
                    errorResult = (string)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords,
                    errorResult
                };
                return new UifJsonResult(success, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// PremiumReceivableReports
        /// </summary>
        /// <param name="businessTypeId"></param>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="agentId"></param>
        /// <param name="insuredId"></param>
        /// <param name="cutoffDate"></param>
        /// <param name="rangeId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="dueDate"></param>
        /// <param name="reportType"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult PremiumReceivableReports(string businessTypeId, string branchId, string lineBusinessId, string agentId, string insuredId,
                                             string cutoffDate, string rangeId, string dateFrom, string dateTo, string dueDate, string prefixId,
                                             string reportType, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";


            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    #region Antigüedad de Deuda

                    if (reportType == Global.DuePorfolio)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameDuePorfolio"];
                        massiveReport.Description = @Global.DuePorfolio.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameDuePorfolio"];
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@BUSINESS_TYPE_CD",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(businessTypeId)) ? 0 : Convert.ToInt32(businessTypeId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@BRANCH_CD",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                            
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@LINE_BUSINESS_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(lineBusinessId)) ? 0 : Convert.ToInt32(lineBusinessId)
                            
                        });

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@AGENT_CD",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(agentId)) ? 0 : Convert.ToInt32(agentId)                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 5,
                            Description = "@POLICYHOLDER_ID",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(insuredId)) ? 0 : Convert.ToInt32(insuredId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 6,
                            Description = "@DATE_CUT",
                            IsFormula = false,
                            Value = cutoffDate
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 7,
                            Description = "@RANGE_CD",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(rangeId)) ? 0 : Convert.ToInt32(rangeId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 8,
                            Description = "@PREFIX_CD",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(prefixId)) ? 0 : Convert.ToInt32(prefixId)
                            
                        });
                    }

                    #endregion

                    #region Estado de Cuenta

                    if (reportType == Global.AccountState)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameAccountState"];
                        massiveReport.Description = @Global.AccountState.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameAccountState"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@BRANCH_CD",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@AGENT_CD",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(agentId)) ? 0 : Convert.ToInt32(agentId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@POLICYHOLDER_ID",
                            IsFormula = false,                            
                            Value = (string.IsNullOrEmpty(insuredId)) ? 0 : Convert.ToInt32(insuredId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@DATE_CUT",
                            IsFormula = false,
                            Value = dueDate
                        });
                    }

                    #endregion

                    #region Recaudos

                    if (reportType == Global.Collection)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureCollection"];
                        massiveReport.Description = @Global.Collection.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameCollection"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@BUSINESS_TYPE_CD",
                            IsFormula = true,                            
                            Value = (string.IsNullOrEmpty(businessTypeId)) ? 0 : Convert.ToInt32(businessTypeId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@BRANCH_CD",
                            IsFormula = true,                            
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@AGENT_CD",
                            IsFormula = true,                            
                            Value = (string.IsNullOrEmpty(agentId)) ? 0 : Convert.ToInt32(agentId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@POLICYHOLDER_ID",
                            IsFormula = true,                            
                            Value = (string.IsNullOrEmpty(insuredId)) ? 0 : Convert.ToInt32(insuredId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 5,
                            Description = "@PREFIX_CD",
                            IsFormula = true,                            
                            Value = (string.IsNullOrEmpty(prefixId)) ? 0 : Convert.ToInt32(prefixId)
                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 6,
                            Description = "@LINE_BUSINESS_CD",
                            IsFormula = true,                            
                            Value = (string.IsNullOrEmpty(lineBusinessId)) ? 0 : Convert.ToInt32(lineBusinessId)                            
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 7,
                            Description = "@DATE_FROM",
                            IsFormula = true,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 8,
                            Description = "@DATE_TO",
                            IsFormula = true,
                            Value = dateTo
                        });
                    }

                    #endregion

                    #region Detalle de Cobro por Pagador

                    if (reportType == Global.PayerPaymentDetail)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureNamePayerPaymentDetail"];
                        massiveReport.Description = @Global.PayerPaymentDetail.ToUpper();
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNamePayerPaymentDetail"];

                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 1,
                            Description = "@POLICYHOLDER_ID",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(insuredId)) ? 0 : Convert.ToInt32(insuredId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 2,
                            Description = "@BRANCH_CD",
                            IsFormula = false,
                            Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 3,
                            Description = "@DATE_FROM",
                            IsFormula = false,
                            Value = dateFrom
                        });
                        procedureParameters.Add(new Reporting.Parameter
                        {
                            Id = 4,
                            Description = "@DATE_TO",
                            IsFormula = false,
                            Value = dateTo
                        });
                    }

                    #endregion

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    // Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }

                return new UifJsonResult(true, totalRecords);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, Global.UnhandledExceptionMsj);
            }
        }

        #endregion

        #region PortfoliioByBroker

        /// <summary>
        /// GetTotalPortfolioByBroker
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalPortfolioByBroker(string dateFrom, string dateTo)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = PortfolioByBroker(dateFrom, dateTo, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// PortfolioByBroker
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult PortfolioByBroker(string dateFrom, string dateTo, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNamePortfolioByBrokerList"];
                    massiveReport.Description = @Global.PortfolioByBroker.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNamePortfolioByBrokerList"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    // Total de registros
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #region DebtorsByPrime

        /// <summary>
        /// GetTotalDebtorsByPrime
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="dateTo"></param>
        /// <param name="agentId"></param>
        /// <param name="currencyId"></param>">
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalDebtorsByPrime(string branchId, string prefixId, string dateTo, string agentId, string currencyId)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = DebtorsByPrimeReports(branchId, prefixId, dateTo, agentId, -1, currencyId);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// DebtorsByPrimeReports
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="dateTo"></param>
        /// <param name="agentId"></param>
        /// <param name="process"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DebtorsByPrimeReports(string branchId, string prefixId, string dateTo,
                                                string agentId, int? process, string currencyId)
        {
            int totalRecords = 0;
            string storedProcedureName = "";

            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureNameDebtorsByPrime"];
                    massiveReport.Description = @Global.DebtorsByPrime.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameDebtorsByPrime"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@BRANCH_CD",
                        IsFormula = false,                        
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@PREFIX_CD",
                        IsFormula = false,                        
                        Value = (string.IsNullOrEmpty(prefixId)) ? 0 : Convert.ToInt32(prefixId)
                        
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@AGENT_INDIVIDUAL_ID",
                        IsFormula = false,                        
                        Value = !string.IsNullOrEmpty(agentId) ? Convert.ToInt32(agentId) : 0
                        
                    });           

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 5,
                        Description = "@CURRENCY_CD",
                        IsFormula = false,                        
                        Value = (string.IsNullOrEmpty(currencyId)) ? 0 : Convert.ToInt32(currencyId)
                        
                    });

                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    /*Total de registros*/
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,                            
                            Value = 0
                            
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,                            
                            Value = 0                            
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }
        #endregion

        #region CurrentPolicies

        /// <summary>
        /// GetTotalRecordsCurrentPolicies
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="dateTo"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsCurrentPolicies(string branchId, string prefixId, string dateTo, string currencyId)
        {
            int totalRecords = 0;
            try
            {
                JsonResult jsonData = CurrentPoliciesReports(branchId, prefixId, dateTo, currencyId, -1);
                bool success = (bool)TypeModel.GetPropertyValue(jsonData.Data, "success");

                if (success)
                {
                    object totalResult = jsonData.Data;
                    totalRecords = (int)TypeModel.GetPropertyValue(totalResult, "result");
                }

                var jsonResult = new
                {
                    records = totalRecords
                };
                return new UifJsonResult(true, jsonResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }

        /// <summary>
        /// CurrentPoliciesReports
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="dateTo"></param>
        /// <param name="currencyId"></param>
        /// <param name="process"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CurrentPoliciesReports(string branchId, string prefixId, string dateTo, string currencyId, int? process)
        {
            int totalRecords = 0;
            string storedProcedureName = "";
            try
            {
                Reporting.MassiveReport massiveReport = new Reporting.MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Reporting.Report report = new Reporting.Report();
                List<Reporting.Parameter> procedureParameters = new List<Reporting.Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureCurrentPolicy"];
                    massiveReport.Description = @Global.CurrentPolicies.ToUpper();
                    massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameCurrentPolicy"];

                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 1,
                        Description = "@CURRENCY_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(currencyId)) ? 0 : Convert.ToInt32(currencyId)
                    });


                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 2,
                        Description = "@BRANCH_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(branchId)) ? 0 : Convert.ToInt32(branchId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 3,
                        Description = "@PREFIX_CD",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(prefixId)) ? 0 : Convert.ToInt32(prefixId)
                    });
                    procedureParameters.Add(new Reporting.Parameter
                    {
                        Id = 4,
                        Description = "@DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });


                    report.StoredProcedure = new Reporting.StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.ExportType = Reporting.ExportTypes.Excel;

                    // Total de registros
                    if (process == -1)
                    {
                        report.IsAsync = false;
                        int parameterNumber = report.StoredProcedure.ProcedureParameters.Count;
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 1,
                            Description = "@MASSIVE_REPORT_ID",
                            IsFormula = false,
                            Value = 0
                        });
                        report.StoredProcedure.ProcedureParameters.Add(new Reporting.Parameter
                        {
                            Id = parameterNumber + 2,
                            Description = "@EXECUTE",
                            IsFormula = false,
                            Value = 0
                        });

                        totalRecords = DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                    }
                    else
                    {
                        report.IsAsync = true;

                        massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name);
                        massiveReport.EndDate = new DateTime(1900, 1, 1);
                        massiveReport.GenerationDate = DateTime.Now;
                        massiveReport.Id = 0;
                        massiveReport.StartDate = DateTime.Now;
                        massiveReport.Success = false;
                        massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleDateCollecting"]);

                        WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                    }
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, totalRecords);
        }

        #endregion

        #endregion
    }
}