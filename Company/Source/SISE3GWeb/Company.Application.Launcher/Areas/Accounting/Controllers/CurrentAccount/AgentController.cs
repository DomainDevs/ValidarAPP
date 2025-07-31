//System
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using Sistran.Core.Framework.UIF.Web.Helpers;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.CurrentAccount;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Application.ReportingServices;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Application.TempCommonServices;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;

//Sistran Company
using Sistran.Company.Application.CommonServices;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.CurrentAccount
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class AgentController : Controller
    {
        #region Class

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
            public void CreateWorkerReportByStoreProcedure(Report report, MassiveReport massiveReport)
            {
                IReportingService _reportService = ServiceManager.Instance.GetService<IReportingService>();

                Thread thread = new Thread(() => _reportService.GenerateMassiveReport(report, massiveReport));
                thread.Start();
            }

            public void CreateWorkerStructure(Report report, bool isExcel)
            {
                IReportingService _reportService = ServiceManager.Instance.GetService<IReportingService>();

                if (isExcel)
                {
                    Thread thread = new Thread(() => _reportService.GenerateFileByReport(report));
                    thread.Start();
                }
            }
        }

        #endregion

        #region Instance Variables

       // readonly IImputationService DelegateService.accountingImputationService = ServiceManager.Instance.GetService<IImputationService>();
        //readonly ICommonService DelegateService.commonService = ServiceManager.Instance.GetService<ICommonService>();
        //readonly IReportingService _reportService = ServiceManager.Instance.GetService<IReportingService>();
        //readonly ITempCommonService DelegateService.tempCommonService = ServiceManager.Instance.GetService<ITempCommonService>();

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region CommissionBalance

        /// <summary>
        /// MainCommissionBalance
        /// Pantalla balance de comisiones
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCommissionBalance()
        {
            try
            {
                ViewBag.ParameterMulticompany = _commonController.GetParameterMulticompany();
                int accountingCompanyDefault = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));
                ViewBag.AccountingCompanyDefault = accountingCompanyDefault <= 0 ? 1 : accountingCompanyDefault;

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/CurrentAccount/Agent/MainCommissionBalance.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        #endregion

        #region PartialClousureAgents

        /// <summary>
        /// MainPartialClosure
        /// Pantalla cierre parcial
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainPartialClosure()
        {
            try
            {
                DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.Year = accountingDate.Date.Year;
                ViewBag.Month = accountingDate.Date.Month;
                ViewBag.Day = accountingDate.Date.Day;
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate.Date, 2);
                ViewBag.MonthYear = GetMonthYear();

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["ReleaseCommissionsProrate"]) == true) //Liberacion de comisiones a prorrata
                {
                    ViewBag.TitleClousure = @Global.ReleaseCommissionsProrate;
                    return View("~/Areas/Accounting/Views/CurrentAccount/Agent/MainPartialClosure.cshtml");
                }
                else
                {
                    ViewBag.TitleClousure = @Global.PartialClousureCommissions;
                    return View("~/Areas/Accounting/Views/CurrentAccount/Agent/MainPartialClosure.cshtml");
                }

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// SavePartialClousureAgentsRequest
        /// Graba cierre parcial de agentes
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SavePartialClousureAgentsRequest(string year, string month, string day)
        {
            try
            {
                bool isSavedPartialClousureAgents = false;
                string dateTo = day + "/" + month + "/" + year;
                string dateFrom = "01" + "/" + month + "/" + year;

                int userId = _commonController.GetUserIdByName(User.Identity.Name);
                int typeProcess = 0;

                isSavedPartialClousureAgents = DelegateService.accountingApplicationService.SavePartialClousureAgentsRequest(Convert.ToDateTime(dateTo).Date.AddHours(23).AddMinutes(59).AddSeconds(59), Convert.ToDateTime(dateFrom), userId, typeProcess);

                return Json(new { success = true, result = isSavedPartialClousureAgents }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = Global.UnhandledExceptionMsj
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region CommissionPaymentOrder

        /// <summary>
        /// MainCommissionPaymentOrder
        /// Pantalla Comisión por orden de pago
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCommissionPaymentOrder()
        {
            try
            {
                ViewBag.ParameterMulticompany = _commonController.GetParameterMulticompany();
                int accountingCompanyDefault = DelegateService.accountingApplicationService.GetAccountingCompanyDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name));
                ViewBag.AccountingCompanyDefault = accountingCompanyDefault <= 0 ? 1 : accountingCompanyDefault;
                int branchUserDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchUserDefault = branchUserDefault;

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View("~/Areas/Accounting/Views/CurrentAccount/Agent/MainCommissionPaymentOrder.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// ValidateCommissionDiscountAgreement
        /// Permite saber si un agente tiene convenio de descuento de comisiones
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateCommissionDiscountAgreement(int agentId)
        {
            int existsCommissionDiscountAgreement =  DelegateService.tempCommonService.GetCommissionDiscountAgreementByAgentId(agentId) ? 1 : 0;

            return Json(existsCommissionDiscountAgreement, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveCommissionPaymentOrder
        /// Permite generar una orden de pago comisiones
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="companyId"></param>
        /// <param name="estimatedPaymentDate"></param>
        /// <param name="agentId"></param>
        /// <param name="agentName"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCommissionPaymentOrder(int branchId, int companyId, DateTime estimatedPaymentDate,
                                                     int agentId, string agentName, int year, int month)
        {
            List<PaymentOrderDTO> paymentOrders;
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            DateTime accountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
            int salePointId =  DelegateService.accountingApplicationService.GetSalePointDefaultByUserIdAndBranchId(userId, branchId);

            // Si no envía el mes o el año toma la fecha contable
            if (year > -1 && month > -1)
            {
                accountingDate = new DateTime(year, month, accountingDate.Day);
            }

            paymentOrders = DelegateService.accountingApplicationService.SaveCommissionPaymentOrder(branchId, companyId, estimatedPaymentDate,
                                                                  agentId, agentName, accountingDate,
                                                                  userId, salePointId, 0);

            return Json(paymentOrders, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Actions

        /// <summary>
        /// GetSystemDate
        /// Obtiene fecha del sistema
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetSystemDate()
        {
            return Json(DateTime.Today.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ClosureGeneration

        /// <summary>
        /// MainClosureGeneration
        /// Levanta la vista principal de generacion de cierre
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainClosureGeneration()
        {
            try
            {

                DateTime accountingDate =  DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(accountingDate, 1);
                ViewBag.DateAccounting = _commonController.DateFormat(accountingDate.Date, 2);

                return View("~/Areas/Accounting/Views/CurrentAccount/Coinsurance/MainClosureGeneration.cshtml");

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetYearMonths
        /// Obtiene año y meses
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetYearMonths()
        {
            int module = Convert.ToInt32(ConfigurationManager.AppSettings["CoinsurerCode"].ToString());
            var accountingDate = DelegateService.commonService.GetModuleDateIssue(module, DateTime.Now);
            int days = DateTime.DaysInMonth(accountingDate.Year, accountingDate.Month);

            var yearMonths = new List<object>();
            for (int i = 1; i <= days; i++)
            {
                yearMonths.Add(new
                {
                    Id = i,
                    Description = i.ToString()
                });
            }

            return new UifSelectResult(yearMonths);
        }

        /// <summary>
        /// ExecuteClosureGeneration
        /// Ejecuta el cierre de comisiones coaseguradoras
        /// </summary>
        /// <param name="day"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExecuteClosureGeneration(string closureDate)
        {
            
            int module = Convert.ToInt32(ConfigurationManager.AppSettings["CoinsurerCode"].ToString());      
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());
            var IsSavedPartialClousureAgents = "";
            
            try
            {
      
                int typeProcess = 1;
                DateTime date = Convert.ToDateTime(closureDate);
                string mounth = Convert.ToString(date.Month);

                string monthEnd = mounth.Length == 2 ? mounth : "0" + Convert.ToString(date.Month);
                
                DateTime DateFrom  = Convert.ToDateTime("01/" + monthEnd + "/" + Convert.ToString(date.Year));

                DateTime DateTo = Convert.ToDateTime(Convert.ToString(DateTime.DaysInMonth(date.Year, date.Month)) + "/" + monthEnd + "/" + Convert.ToString(date.Year) + " " + "23:59:59");
               
                IsSavedPartialClousureAgents = Convert.ToString(DelegateService.accountingApplicationService.SavePartialClousureAgentsRequest(DateTo,DateFrom, userId, typeProcess));

            }
            catch (BusinessException businessException)
            {
                return Json(new
                {
                    success = false,
                    result = businessException.ExceptionMessages
                }, JsonRequestBehavior.AllowGet);
            }
            catch (UnhandledException)
            {
                return Json(new
                {
                    success = false,
                    result = Global.UnhandledExceptionMsj
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(IsSavedPartialClousureAgents, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ClosureGenerationReport

        /// <summary>
        /// MainClosureReport
        /// Levanta la vista principal para los reportes de cierre
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainClosureReport()
        {
            try
            {
                return View("~/Areas/Accounting/Views/CurrentAccount/Coinsurance/MainClosureReport.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
           
        }

        /// <summary>
        /// GetMonthlyClosings
        /// Obtiene el mes para el cierre
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetMonthlyClosings()
        {
            var monthlyClosings = new List<object>()
            {
                new { Id = 1, Description = "Subdiario de Cuenta Corriente" },
                new { Id = 2, Description = "Resumen de Cuenta Corriente" },
                new { Id = 3, Description = "Saldo Parcial de Coaseguradora" }
            };

            return new UifSelectResult(monthlyClosings);
        }

        /// <summary>
        /// LoadCoinsuranceReport
        /// Carga los datos para el reporte
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="companyId"></param>
        /// <param name="sortOrder"></param>
        /// <param name="currencyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult LoadCoinsuranceReport(string reportType, string dateFrom, string dateTo,
                                                string companyId, string sortOrder, string currencyId)
        {
            if (Convert.ToDateTime(dateFrom) > Convert.ToDateTime(dateTo))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var commissionBalances = new List<CoinsuranceCurrentAccountModel>();
            var coinsuranceCurrentAccountSummaries = new List<CoinsuranceCurrentAccountSummaryModel>();

            if (Convert.ToInt32(reportType) == 1)
            {
                commissionBalances = DelegateService.accountingApplicationService.GetCommissionBalance(0, -1, 0, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo)).
                 Select(item => new CoinsuranceCurrentAccountModel()
                 {
                     PaymentDate = item.RegisterDate.ToShortDateString(),
                     BillNumber = item.AgentCommissionBalanceItemCode,
                     BranchId = item.BranchCode,
                     PrefixId = item.PrefixId,
                     PolicyNumber = item.DocumentNumPolicy,
                     EndorsmentNumber = item.DocumentNumEndorsement,
                     InsuredId = item.AgentTypeCode,
                     InsuredName = item.AgentTypeDescription,
                     AgentId = item.AgentId,
                     AgentName = item.AgentName,
                     CurrencyDescription = item.CurrencyDescription,
                     IssuancePrimeAmount = item.CommissionPercentage,
                     PrimeAmount = item.CommissionAmount,
                     CompanyParticipation = item.CommissionTypeCode,
                     IssuancePremiumAmount = item.CommissionDiscounted,
                     PremiumAmount = item.CommissionTax,
                     PolicyChange = item.CommissionRetention,
                     IssuanceExpensesAmount = item.CommissionBalance,
                     ExpensesAmount = item.IncomeAmount,
                     IssuanceExpensesTax = item.Amount,
                     ExpensesTax = item.ParticipationPercentage,
                     IssuanceAmount = item.CommissionPct,
                     Amount = item.AdditCommissionPct,
                     BranchDescription = item.Branch,
                     CompanyDescription = item.InsuredName,
                     SubPrefixId = item.SubPrefixId
                 }).ToList();

                var summary = new CoinsuranceCurrentAccountSummaryModel();

                summary.TotalPremiumCharged = commissionBalances.Sum(item => item.IssuancePrimeAmount);
                summary.TotalChangeColletion = commissionBalances.Sum(item => item.PrimeAmount);
                summary.IssuancePremiumPaid = commissionBalances.Sum(item => item.IssuancePremiumAmount * item.PolicyChange);
                summary.PremiumPaid = commissionBalances.Sum(item => item.PremiumAmount * item.PolicyChange);
                summary.IssuanceManagementExpenses = commissionBalances.Sum(item => item.IssuanceExpensesAmount);
                summary.ManagementExpenses = commissionBalances.Sum(item => item.ExpensesAmount);
                summary.IssuanceIVA = commissionBalances.Sum(item => item.IssuanceExpensesTax);
                summary.IVA = commissionBalances.Sum(item => item.ExpensesTax);
                summary.IssuanceTotalDebitCredit = commissionBalances.Sum(row => row.IssuanceAmount);
                summary.TotalDebitCredit = commissionBalances.Sum(row => row.Amount);

                coinsuranceCurrentAccountSummaries.Add(summary);

                TempData["CurrentAccountDaily"] = commissionBalances;
                TempData["CurrentAccountDailySummary"] = coinsuranceCurrentAccountSummaries;
                TempData["CurrentAccountDailyReport"] = "Areas//Accounting//Reports//CurrentAccount//CoinsuranceCurrentAccount.rpt";
            }

            if (Convert.ToInt32(reportType) == 2)
            {
                commissionBalances = DelegateService.accountingApplicationService.GetCommissionBalance(0, -1, 0, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo)).
                    Select(item => new CoinsuranceCurrentAccountModel()
                    {
                        PaymentDate = item.RegisterDate.ToShortDateString(),
                        BillNumber = item.AgentCommissionBalanceItemCode,
                        PrefixId = item.PrefixId,
                        SubPrefixId = item.SubPrefixId,
                        BranchId = item.AgentCommissionBalanceCode,
                        PolicyNumber = item.DocumentNumPolicy,
                        EndorsmentNumber = item.DocumentNumEndorsement,
                        InsuredId = item.AgentId,
                        InsuredName = item.AgentName,
                        IssuancePrimeAmount = item.CommissionPercentage,
                        PrimeAmount = item.CommissionAmount,
                        CompanyParticipation = item.CommissionTypeCode,
                        IssuancePremiumAmount = item.CommissionDiscounted,
                        PremiumAmount = item.CommissionTax,
                        PolicyChange = item.CommissionRetention,
                        IssuanceExpensesAmount = item.CommissionBalance,
                        ExpensesAmount = item.IncomeAmount,
                        IssuanceExpensesTax = item.Amount,
                        ExpensesTax = item.ParticipationPercentage,
                        IssuanceAmount = item.CommissionPct,
                        Amount = item.AdditCommissionPct,
                        CompanyDescription = item.InsuredName,
                        CurrencyDescription = item.CurrencyDescription,
                        BranchDescription = item.Branch

                    }).ToList();

                TempData["CurrentAccountSummary"] = commissionBalances;
                TempData["CurrentAccountSummaryReport"] = "Areas//Accounting//Reports//CurrentAccount//CurrentAccountSummary.rpt";
            }

            if (Convert.ToInt32(reportType) == 3)
            {
                commissionBalances = DelegateService.accountingApplicationService.GetCommissionBalance(0, -1, 0, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo))
                 .Select(item => new CoinsuranceCurrentAccountModel()
                 {
                     CurrencyDescription = item.CurrencyDescription,
                     PaymentDate = item.AccountingDate.ToShortDateString(),
                     IssuancePrimeAmount = item.CommissionPercentage,
                     PrimeAmount = item.CommissionAmount,
                     IssuancePremiumAmount = item.CommissionDiscounted,
                     PremiumAmount = item.CommissionTax,
                     InsuredName = item.InsuredName,
                     PolicyNumber = item.DocumentNumPolicy

                 }).ToList();

                TempData["CurrentAccountPartial"] = commissionBalances;
                TempData["CurrentAccountPartialReport"] = "Areas//Accounting//Reports//CurrentAccount//CurrentAccountPartial.rpt";
            }

            if (commissionBalances.Count == 0)
            {
                return Json(-2, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// LoadCoinsuranceReports
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="companyId"></param>
        /// <param name="sortOrder"></param>
        /// <param name="currencyId"></param>
        /// <param name="reportTypeDescription"></param>
        /// <returns>JsonResult</returns>
        public JsonResult LoadCoinsuranceReports(string reportType, string dateFrom, string dateTo, string companyId,
                                               string sortOrder, string currencyId, string reportTypeDescription)
        {
            string storedProcedureName = "";
            string order = sortOrder.Equals("first") ? Convert.ToString(1) : Convert.ToString(2);
            try
            {
                MassiveReport massiveReport = new MassiveReport();
                ParameterModel parameter = new ParameterModel();
                Report report = new Report();
                List<Parameter> procedureParameters = new List<Parameter>();

                if (!parameter.IsObject)
                {
                    if (Convert.ToInt32(reportType) == 1)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureProcessCheckingAccount"];
                        massiveReport.Description = reportTypeDescription;
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameCheckginAccount"];
                    }

                    if (Convert.ToInt32(reportType) == 2)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureProcessCheckingAccount"];
                        massiveReport.Description = reportTypeDescription;
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNameSumaryCheckginAccount"];
                    }
                    if (Convert.ToInt32(reportType) == 3)
                    {
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureProcessCheckingAccount"];
                        massiveReport.Description = reportTypeDescription;
                        massiveReport.UrlFile = ConfigurationManager.AppSettings["TemplateNamePartialBalanceCoinsurance"];
                    }

                    procedureParameters.Add(new Parameter
                    {
                        Id = 1,
                        Description = "DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 2,
                        Description = "DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 3,
                        Description = "INDIVIDUAL_ID",
                        IsFormula = false,
                        Value = companyId
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 4,
                        Description = "SORT_ORDER",
                        IsFormula = false,
                        Value = order
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 5,
                        Description = "CURRENCY_ID",
                        IsFormula = false,
                        Value = currencyId
                    });
                    report.StoredProcedure = new StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = true;
                    report.ExportType = ExportTypes.Excel;

                    massiveReport.UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());
                    massiveReport.EndDate = new DateTime(1900, 1, 1);
                    massiveReport.GenerationDate = DateTime.Now;
                    massiveReport.Id = 0;
                    massiveReport.StartDate = DateTime.Now;
                    massiveReport.Success = false;
                    massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleCheckingAccount"]);

                    WorkerFactory.Instance.CreateWorkerReportByStoreProcedure(report, massiveReport);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }

            return new UifJsonResult(true, 0);
        }

        /// <summary>
        /// GetTotalRecordsMassive
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="currencyId"></param>
        /// <param name="companyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecordsMassive(string reportType, string dateFrom, string dateTo,
                                                 string currencyId, string companyId)
        {
            string storedProcedureName = "";
            decimal totRecords = 0;

            try
            {
                ParameterModel parameter = new ParameterModel();
                Report report = new Report();
                List<Parameter> procedureParameters = new List<Parameter>();

                if (!parameter.IsObject)
                {
                    storedProcedureName = ConfigurationManager.AppSettings["ProcedureProcessCheckingAccount"];
                    procedureParameters.Add(new Parameter
                    {
                        Id = 1,
                        Description = "DATE_FROM",
                        IsFormula = false,
                        Value = dateFrom
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 2,
                        Description = "DATE_TO",
                        IsFormula = false,
                        Value = dateTo
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 3,
                        Description = "INDIVIDUAL_ID",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(companyId)) ? 0 : Convert.ToInt32(companyId)
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 4,
                        Description = "SORT_ORDER",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 5,
                        Description = "CURRENCY_ID",
                        IsFormula = false,
                        Value = (string.IsNullOrEmpty(currencyId)) ? 0 : Convert.ToInt32(currencyId)
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 6,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = 0
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 7,
                        Description = "@EXECUTE",
                        IsFormula = false,
                        Value = 0
                    });

                    report.StoredProcedure = new StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                    report.IsAsync = false;
                    report.ExportType = ExportTypes.Excel;

                    totRecords =  DelegateService.reportingService.GetTotalRecordsMassiveReport(report);
                }
                var jsonData = new
                {
                    records = totRecords
                };

                return new UifJsonResult(true, jsonData);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, 0);
            }
        }
        /// <summary>
        /// GetMassiveReportProcess
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetMassiveReportProcess(string reportName)
        {
            MassiveReport massiveReport = new MassiveReport();
            List<object> massiveReportsResponses = new List<object>();

            User user = new User() { UserId = SessionHelper.GetUserId(), AccountName = SessionHelper.GetUserName() };

            try
            {
                massiveReport.UserId = user.UserId;
                massiveReport.Description = reportName;
                massiveReport.ModuleId = Convert.ToInt16(ConfigurationManager.AppSettings["ModuleCheckingAccount"]);

                List<MassiveReport>  massiveReports = DelegateService.reportingService.GetMassiveReports(massiveReport);

                List<MassiveReport> orders = massiveReports.OrderByDescending(x => x.Id).ToList();

                if (massiveReports.Count > 0)
                {
                    double elapsed = 0;
                    double minElapsed = 0;
                    double progress = 0;
                    int order = 1;

                    foreach (MassiveReport masiveReport in orders)
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
                                elapsed = System.Math.Abs(System.Math.Round((masiveReport.EndDate.TimeOfDay.TotalHours - masiveReport.StartDate.TimeOfDay.TotalHours), 2));
                                minElapsed = elapsed - System.Math.Truncate(elapsed);
                                minElapsed = minElapsed * 60;
                            }
                        }

                        massiveReportsResponses.Add(new
                        {
                            Order = order,
                            ProcessId = masiveReport.Id,
                            Description = masiveReport.Description,
                            RecordsNumber = masiveReport.RecordsNumber,
                            RecordsProcessed = masiveReport.RecordsProcessed,
                            Progress = progress.ToString("P", CultureInfo.InvariantCulture),
                            Elapsed = System.Math.Truncate(elapsed) + " h " + System.Math.Truncate(minElapsed) + " m",
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
        /// ShowCoinsuranceReport
        /// Muestra los datos del reporte en un informe pdf
        /// </summary>
        /// <param name="reportType"></param>
        public void ShowCoinsuranceReport(int reportType)
        {
            try
            {
                var reportName = "";
                string reportPath = "";

                if (reportType == 1)
                {
                    var reportSource = TempData["CurrentAccountDaily"];
                    var summary = TempData["CurrentAccountDailySummary"];
                    reportName = TempData["CurrentAccountDailyReport"].ToString();

                    ReportDocument reportDocument = new ReportDocument();

                    reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                    reportDocument.Load(reportPath);

                    if (reportSource != null && reportSource.GetType().ToString() != typeof(System.String).ToString())
                    {
                        reportDocument.SetDataSource(reportSource);
                        reportDocument.Subreports[0].SetDataSource(summary);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response,
                                                        false, "Subdiario de Cuenta Corriente");

                    TempData["CurrentAccountDaily"] = null;
                    TempData["CurrentAccountDailySummary"] = null;
                    TempData["CurrentAccountDailyReport"] = null;
                }

                if (reportType == 2)
                {
                    var reportSource = TempData["CurrentAccountSummary"];
                    reportName = TempData["CurrentAccountSummaryReport"].ToString();

                    ReportDocument reportDocument = new ReportDocument();

                    reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);

                    if (reportSource != null && reportSource.GetType().ToString() != typeof(System.String).ToString())
                    {
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response,
                                                        false, "Resumen de Cuenta Corriente");

                    TempData["CurrentAccountSummary"] = null;
                    TempData["CurrentAccountSummaryReport"] = null;
                }

                if (reportType == 3)
                {
                    var reportSource = TempData["CurrentAccountPartial"];
                    reportName = TempData["CurrentAccountPartialReport"].ToString();

                    ReportDocument reportDocument = new ReportDocument();

                    reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;
                    reportDocument.Load(reportPath);

                    if (reportSource != null && reportSource.GetType().ToString() != typeof(System.String).ToString())
                    {
                        reportDocument.SetDataSource(reportSource);
                    }

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response,
                                                        false, "Saldo Parcial de Coaseguradora");

                    TempData["CurrentAccountPartial"] = null;
                    TempData["CurrentAccountPartialReport"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        /// <summary>
        /// GenerateStructureReport
        /// </summary>
        /// <param name="reportType"></param>
        /// <param name="processId"></param>
        /// <param name="reportTypeDescription"></param>
        /// <param name="exportFormatType"></param>
        /// <param name="recordsNumber"></param>
        /// <returns>responsProcess</returns>
        public JsonResult GenerateStructureReport(string reportType, int processId, string reportTypeDescription,
                                                  int exportFormatType, decimal recordsNumber)
        {
            ParameterModel parameter = new ParameterModel();
            parameter.IsObject = false;
            Report report = new Report();
            List<object> responsProcess = new List<object>();
            int formatId = 0;
            string storedProcedureName = "";
            string exportedFileName = "";

            try
            {
                List<Parameter> procedureParameters = new List<Parameter>();

                if (!parameter.IsObject)
                {
                    report.Filter = "";
                    if (Convert.ToInt32(reportType) == 1)
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatCheckingAccount"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameCheckginAccount"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetCheckingAccount"];
                    }

                    if (Convert.ToInt32(reportType) == 2)
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatSumaryCheckingAccount"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNameSumaryCheckginAccount"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetSumaryCheckingAccount"];
                    }

                    if (Convert.ToInt32(reportType) == 3)
                    {
                        formatId = Convert.ToInt32(ConfigurationManager.AppSettings["IdFormatPartialBalanceCoinsurance"]);
                        report.Name = ConfigurationManager.AppSettings["TemplateNamePartialBalanceCoinsurance"];
                        storedProcedureName = ConfigurationManager.AppSettings["ProcedureGetPartialBalanceCoinsurance"];
                    }

                    procedureParameters.Add(new Parameter
                    {
                        Id = 1,
                        Description = "@MASSIVE_REPORT_ID",
                        IsFormula = false,
                        Value = processId
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 2,
                        Description = "@RECORD_COUNT",
                        IsFormula = false,
                        Value = recordsNumber
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 3,
                        Description = "@PAGE_SIZE",
                        IsFormula = false,
                        Value = Int32.Parse(ConfigurationManager.AppSettings["PageSizeReport"])
                    });
                    procedureParameters.Add(new Parameter
                    {
                        Id = 4,
                        Description = "@PAGE_NUMBER",
                        IsFormula = false,
                        Value = 1
                    });

                    report.StoredProcedure = new StoredProcedure()
                    {
                        ProcedureName = storedProcedureName,
                        ProcedureParameters = procedureParameters
                    };

                    report.Parameters = null;
                }

                report.ExportType = ExportTypes.Excel;
                report.IsAsync = true;
                report.Description = @Global.GenerateDocument;
                report.UserId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

                report.Format = new Format()
                {
                    Id = formatId,
                    FileType = FileTypes.Excel
                };

                exportedFileName = "20";

                Format format = new Format();
                format.Id = formatId;
                format.FileType = FileTypes.Text;

                List<FormatDetail> formatDetails = DelegateService.reportingService.GetFormatDetailsByFormat(format);
                if (formatDetails.Count > 0)
                {
                    WorkerFactory.Instance.CreateWorkerStructure(report, true);
                }
                else
                {
                    exportedFileName = "-1";
                }

                responsProcess.Add(new
                {
                    ExportedFileName = exportedFileName
                });
            }
            catch (Exception ex)
            {
                responsProcess = new List<object>();
                List<string> errors = new List<string>();

                errors.Add("Error --> " + ex.Message);
                if (ex.InnerException != null && ex.InnerException.Message != null)
                {
                    errors.Add(ex.InnerException.Message);
                }
                responsProcess.Add(new
                {
                    ExportedFileName = "",
                    ErrorInfo = errors
                });
            }

            return Json(responsProcess, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Method Private

        /// <summary>
        /// GetMonthYear
        /// Obtiene los meses del año
        /// </summary>
        /// <returns>List<object/></returns>
        private List<object> GetMonthYear()
        {
            try
            {
                string[] expirationMonth = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                             "Julio", "Agosto", "Septiembre", "Octubre", "Novembre", "Diciembre" };

                List<object> months = new List<object>();

                for (int i = 0; i < expirationMonth.Length; i++)
                {
                    months.Add(new
                    {
                        Id = i + 1,
                        Description = expirationMonth[i]
                    });
                }

                return months;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

    }
}