using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Reports.Reconciliation;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran.Core
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.ReconciliationServices;
using Sistran.Core.Application.ReconciliationServices.Models;
using Sistran.Core.Framework.UIF.Web.Services;

//Sistran Company
using Sistran.Company.Application.CommonServices;
using Sistran.Core.Application.AccountingServices.DTOs;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.BankReconciliation
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class BankReconciliationController : Controller
    {
        #region Instance variables
        

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// MainBankReconciliation
        /// Pantalla de conciliación bancaria
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBankReconciliation()
        {
            try
            {          

                ViewBag.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["AccountingModule"]), DateTime.Now).ToString("dd/MM/yyyy");
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy");
                Parameter parameter = DelegateService.commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["MaximumDeviation"]));
                ViewBag.MaximumDeviation = "0";
                if (parameter != null)
                {
                    ViewBag.MaximumDeviation = parameter.NumberParameter;
                }

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainReverseConciliation
        /// Pantalla de reversión de Conciliación
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainReverseReconciliation()
        {
            try
            {
                ViewBag.AccountingDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["AccountingModule"]), DateTime.Now).ToString("dd/MM/yyyy");

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }       
        }

        /// <summary>
        /// PendingBanksList
        /// Pantalla de reversión de Conciliación
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PendingBanksList()
        {
            try
            {
                return View("~/Areas/Accounting/Views/BankReconciliation/PendingBanksList.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            } 
        }

        /// <summary>
        /// MainReconciliationBalance
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainReconciliationBalance()
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

        #endregion

        #region Actions

        /// <summary>
        /// GetMovementPendingByAccountBankId
        /// Obtiene moviemientos pendientes por número de cuenta bancaria
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="dateTo"></param>
        /// <param name="sortingType"></param>
        /// <param name="sortingDate"></param>
        /// <param name="sortingMonth"></param>
        /// <param name="sortingBranch"></param>
        /// <param name="sortingVoucher"></param>
        /// <param name="bank"></param>
        /// <param name="centralAccounting"></param>
        /// <param name="dailyAccounting"></param>
        /// <param name="automaticType"></param>
        /// <param name="automaticDate"></param>
        /// <param name="automaticMonth"></param>
        /// <param name="automaticBranch"></param>
        /// <param name="automaticVoucher"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetMovementPendingByAccountBankId(int accountBankId, string dateTo, int sortingType, int sortingDate,
                                                              int sortingMonth, int sortingBranch, int sortingVoucher,
                                                              string bank, string centralAccounting, string dailyAccounting,
                                                              int automaticType, int automaticDate, int automaticMonth,
                                                              int automaticBranch, int automaticVoucher)
        {
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
            {
                Id = accountBankId,
                Number = ""
            };
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

            bool isBank = bank == "B";
            bool isCentralAccounting = centralAccounting == "C";
            bool isDailyAccounting = dailyAccounting == "D";

            List<Reconciliation> reconciliations = DelegateService.reconciliationService.GetReconciliationsByStatementTypes(bankAccountCompany, Convert.ToDateTime(dateTo), isBank, isCentralAccounting, isDailyAccounting, userId);
            List<object> statementResponse = new List<object>();

            foreach (Reconciliation reconciliation in reconciliations)
            {
                List<Statement> statements = reconciliation.BankStatements;

                statements = SortByOptions(statements, sortingType, sortingDate, sortingMonth, sortingBranch, sortingVoucher);

                foreach (Statement statement in statements)
                {
                    string origin = "";
                    if (statement.StatementType == StatementTypes.Bank)
                    {
                        origin = "B";
                    }
                    if (statement.StatementType == StatementTypes.CentralAccounting)
                    {
                        origin = "C";
                    }
                    if (statement.StatementType == StatementTypes.DailyAccounting)
                    {
                        origin = "D";
                    }

                    statementResponse.Add(new
                    {
                        MovementOrigin = origin,
                        Id = statement.Id,
                        BankId = statement.BankAccountCompany.Id,
                        MovementId = statement.Id,
                        BankingMovementTypeId = statement.ReconciliationMovementType.Id,
                        BankingMovementTypeDescription = statement.ReconciliationMovementType.Description,
                        BranchId = statement.Branch.Id,
                        BranchDescription = statement.Branch.Description,
                        VoucherNumber = statement.DocumentNumber,
                        MovementDate = statement.Date.ToString("dd/MM/yyyy"),
                        MovementAmount = statement.LocalAmount.Value,
                        MovementDescription = statement.Description,
                    });
                }
            }

            return new UifTableResult(statementResponse);
        }

        /// <summary>
        /// GetReconciliationBalance
        /// </summary>
        /// <param name="bankName"></param>
        /// <param name="bankId"></param>
        /// <param name="accountTypeId"></param>
        /// <param name="accountTypeName"></param>
        /// <param name="accountNumber"></param>
        /// <param name="monthName"></param>
        /// <param name="dateTo"></param>
        /// <param name="newAccountingBalance"></param>
        /// <param name="newBankBalance"></param>
        /// <param name="lastAccountingBalance"></param>
        /// <param name="lastBankBalance"></param>
        public void GetReconciliationBalance(string bankName, int bankId, int accountTypeId, string accountTypeName,
                                             string accountNumber, string monthName, string dateTo,
                                             decimal newAccountingBalance, decimal newBankBalance,
                                             decimal lastAccountingBalance, decimal lastBankBalance)
        {
            //IMPLEMENTAR PARA EE
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
            {
                Id = bankId,
                Number = accountNumber
            };

            BankDTO bankUser = new BankDTO()
            {
                Id = bankId,
                Description = Convert.ToString(lastAccountingBalance)
            };
            bankAccountCompany.Bank = bankUser;
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

            bool isBank = true;
            bool isCentralAccounting = true;
            bool isDailyAccounting = true;

            bankAccountCompany.BankAccountType = new BankAccountTypeDTO()
            {
                Description = Convert.ToString(lastAccountingBalance)
            };
            bankAccountCompany.Branch = new SCRDTO.BranchDTO()
            {
                Description = Convert.ToString(newAccountingBalance)
            };
            bankAccountCompany.Currency = new SCRDTO.CurrencyDTO()
            {
                Description = Convert.ToString(newBankBalance)
            };

            List<Reconciliation> reconciliationBalances = DelegateService.reconciliationService.GetReconciliationsByStatementTypes(bankAccountCompany, Convert.ToDateTime(dateTo), isBank, isCentralAccounting, isDailyAccounting, userId);
            List<ReconciliationBalanceModel> balances = new List<ReconciliationBalanceModel>();

            foreach (Reconciliation reconciliation in reconciliationBalances)
            {
                ReconciliationBalanceModel balance = new ReconciliationBalanceModel();
                balance.Id = reconciliation.Id;
                balance.Section = reconciliation.UserId;
                foreach (Statement statement in reconciliation.BankStatements)
                {
                    balance.Description = statement.BankAccountCompany.BankAccountType.Description;
                    balance.Debits = statement.BankAccountCompany.Bank.Description;
                    balance.Credits = statement.BankAccountCompany.Number;
                }

                balance.BankId = bankId;
                balance.BankName = bankName;
                balance.AccountNumber = accountNumber;
                balance.AccountTypeId = accountTypeId;
                balance.AccountTypeName = accountTypeName;
                balance.Month = monthName;
                balance.Year = Convert.ToInt32(Convert.ToDateTime(dateTo).ToString("yyyy"));
                balances.Add(balance);
            }

            TempData["billRptSource"] = balances;
            TempData["BillingReportName"] = "Areas//Accounting//Reports//Reconciliation//ReconciliationBalance.rpt";
        }
        
        /// <summary>
        /// ShowPendingBanks
        /// Visualiza el reporte en formato pdf 
        /// </summary>
        public void ShowReconciliationBalance()
        {
            //IMPLEMENTAR PARA EE
            try
            {
                bool isValid = true;

                var reportSource = TempData["billRptSource"];
                var reportName = TempData["BillingReportName"];

                if (reportSource == null)
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument reportDocument = new ReportDocument();

                    string reportPath = System.Web.HttpContext.Current.Server.MapPath("~/") + reportName;

                    reportDocument.Load(reportPath);

                    //Lena Reporte Principal
                    reportDocument.SetDataSource(reportSource);

                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "PendingBanksReconciliation");

                    TempData["billRptSource"] = null;
                    TempData["BillingReportName"] = null;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

        /// <summary>
        /// GetAmountReconciliationBalance
        /// Obtiene los saldos de la compañía y del banco
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="dateTo"></param>
        /// <param name="accountNumber"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAmountReconciliationBalance(int accountBankId, string dateTo, string accountNumber)
        {
            //IMPLEMENTAR PARA EE
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
            {
                Id = accountBankId
            };

            BankDTO bankUser = new BankDTO()
            {
                Id = _commonController.GetUserIdByName(User.Identity.Name),
                Description = User.Identity.Name.ToUpper()
            };
            bankAccountCompany.Bank = bankUser;

            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

            bankAccountCompany.BankAccountType = new BankAccountTypeDTO()
            {
                Id = -1
            };

            bankAccountCompany.Number = accountNumber;

            bool isBank = false;
            bool isCentralAccounting = false;
            bool isDailyAccounting = false;
            bool byType = false;
            bool byDate = false;
            bool byMonth = false;
            bool byBranch = false;
            bool byVoucher = false;

            List<Reconciliation> reconciliations = DelegateService.reconciliationService.Reconcile(bankAccountCompany, Convert.ToDateTime(dateTo), Convert.ToDateTime(dateTo),
                                                          isBank, isCentralAccounting, isDailyAccounting, byType, byMonth, byVoucher,
                                                          byDate, byBranch, userId);
            List<object> statementBalances = new List<object>();

            if (reconciliations.Count > 0)
            {
                foreach (Reconciliation reconciliation in reconciliations)
                {
                    List<Statement> statements = reconciliation.BankStatements;

                    foreach (Statement statement in statements)
                    {
                        statementBalances.Add(new
                        {
                            AccountingBalance = statement.LocalAmount.Value,
                            BankBalance = statement.Amount.Value,
                            AccountingBalanceNew = statement.ExchangeRate.BuyAmount
                        });
                    }
                }
            }
            else
            {
                statementBalances.Add(new
                {
                    AccountingBalance = 0,
                    BankBalance = 0,
                    AccountingBalanceNew = 0
                });
            }

            return Json(statementBalances, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveAmountReconciliationBalance
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="dateTo"></param>
        /// <param name="accountNumber"></param>
        /// <param name="accountingBalance"></param>
        /// <param name="bankBalance"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveAmountReconciliationBalance(int accountBankId, string dateTo, string accountNumber,
                                                            decimal accountingBalance, decimal bankBalance)
        {
            //IMPLEMENTAR PARA EE
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO()
            {
                Id = accountBankId
            };
            BankDTO bankUser = new BankDTO()
            {
                Id = _commonController.GetUserIdByName(User.Identity.Name),
                Description = User.Identity.Name.ToUpper()
            };
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

            bankAccountCompany.Bank = bankUser;
            bankAccountCompany.BankAccountType = new BankAccountTypeDTO() { Id = 0 };
            bankAccountCompany.Number = accountNumber;
            ExchangeRate exchangeRate = new ExchangeRate()
            {
                BuyAmount = accountingBalance,
                SellAmount = bankBalance
            };

            bool isBank = false;
            bool isCentralAccounting = false;
            bool isDailyAccounting = false;
            bool byType = false;
            bool byDate = false;
            bool byMonth = false;
            bool byBranch = false;
            bool byVoucher = false;

            List<Reconciliation> reconciliations = DelegateService.reconciliationService.Reconcile(bankAccountCompany, Convert.ToDateTime(dateTo), Convert.ToDateTime(dateTo),
                                                          isBank, isCentralAccounting, isDailyAccounting, byType, byMonth, byVoucher,
                                                          byDate, byBranch, userId);
            List<object> reconciliationBalances = new List<object>();

            if (reconciliations.Count > 0)
            {
                foreach (Reconciliation reconciliation in reconciliations)
                {
                    reconciliationBalances.Add(new
                    {
                        Id = 0
                    });
                }
            }
            else
            {
                reconciliationBalances.Add(new
                {
                    Id = -1
                });
            }

            return Json(reconciliationBalances, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetMovementConciliationByAccountBankId
        /// Obtiene los movimientos de conciliación por cuenta bancaria
        /// </summary>
        /// <param name="accountBankId"></param>
        /// <param name="dateTo"></param>
        /// <param name="conciliationDate"></param>
        /// <param name="bank"></param>
        /// <param name="centralAccounting"></param>
        /// <param name="dailyAccounting"></param>
        /// <param name="automaticType"></param>
        /// <param name="automaticDate"></param>
        /// <param name="automaticMonth"></param>
        /// <param name="automaticBranch"></param>
        /// <param name="automaticVoucher"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetMovementConciliationByAccountBankId(int accountBankId, string dateTo, string conciliationDate,
                                                                   string bank, string centralAccounting, string dailyAccounting,
                                                                   int automaticType, int automaticDate, int automaticMonth,
                                                                   int automaticBranch, int automaticVoucher)
        {
            BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO() { Id = accountBankId };
            BankDTO bankUser = new BankDTO()
            {
                Id = _commonController.GetUserIdByName(User.Identity.Name),
                Description = User.Identity.Name.ToUpper()
            };
            bankAccountCompany.Bank = bankUser;
            bankAccountCompany.BankAccountType = new BankAccountTypeDTO()
            {
                Id = Convert.ToInt32(ConfigurationManager.AppSettings["ReconciliationGroupNumber"])
            };
            bankAccountCompany.Number = "";
            int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());

            bool isBank = bank == "B";
            bool isCentralAccounting = centralAccounting == "C";
            bool isDailyAccounting = dailyAccounting == "D";
            bool byType = automaticType == 0;
            bool byDate = automaticDate == 0;
            bool byMonth = automaticMonth == 0;
            bool byBranch = automaticBranch == 0;
            bool byVoucher = automaticVoucher == 0;

            List<Reconciliation> reconciliations = DelegateService.reconciliationService.Reconcile(bankAccountCompany, Convert.ToDateTime(dateTo), Convert.ToDateTime(conciliationDate),
                                                          isBank, isCentralAccounting, isDailyAccounting, byType, byMonth, byVoucher,
                                                          byDate, byBranch, userId);
            List<object> reconciliationMovements = new List<object>();

            foreach (Reconciliation reconciliation in reconciliations)
            {
                List<Statement> statements = reconciliation.BankStatements;

                foreach (Statement statement in statements)
                {
                    reconciliationMovements.Add(new
                    {
                        Id = statement.ReconciliationMovementType.Id,
                        BankingMovementTypeDescription = statement.ReconciliationMovementType.Description,
                        VoucherNumber = statement.DocumentNumber,
                        MovementDate = statement.Date == Convert.ToDateTime("01/01/0001 0:00:00") ? "**" : statement.Date.ToString("dd/MM/yyyy"),
                        MovementBankAmount = statement.LocalAmount.Value,
                        MovementAccountingAmount = statement.Amount.Value,
                        MovementDifference = statement.Description,
                        ConciliationId = statement.Id
                    });
                }
            }

            return new UifTableResult(reconciliationMovements);
        }

        /// <summary>
        /// SaveConciliation
        /// Graba conciliación
        /// </summary>
        /// <param name="conciliationModel"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveConciliation(BankReconciliationModel conciliationModel)
        {
            List<Reconciliation> reconciliations = new List<Reconciliation>();
            Reconciliation reconciliation = new Reconciliation();
            List<Statement> statements = new List<Statement>();
            List<object> reconciliationIds = new List<object>();

            try
            {
                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                reconciliation.Id = conciliationModel.Id;
                reconciliation.ReconciliationType = ReconciliationTypes.Manual;
                reconciliation.Date = conciliationModel.ConciliationDate;
                reconciliation.UserId = userId;

                foreach (BankStatementModel statement in conciliationModel.Statements)
                {
                    StatementTypes statementType = StatementTypes.Bank;

                    if (statement.MovementOrigin == "B")
                    {
                        statementType = StatementTypes.Bank;
                    }
                    else if (statement.MovementOrigin == "C")
                    {
                        statementType = StatementTypes.CentralAccounting;
                    }
                    else if (statement.MovementOrigin == "D")
                    {
                        statementType = StatementTypes.DailyAccounting;
                    }

                    statements.Add(new Statement()
                    {
                        BankAccountCompany = new BankAccountCompanyDTO()
                        {
                            Id = conciliationModel.AccountBankId
                        },
                        Amount = new Amount() { Value = Convert.ToDecimal(statement.MovementAmount) },
                        LocalAmount = new Amount() { Value = Convert.ToDecimal(statement.MovementAmount) },
                        Branch = new Branch() { Id = statement.BranchId },
                        ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                        {
                            Id = statement.BankingMovementTypeId,
                            Description = statement.BankingMovementTypeDescription
                        },
                        Date = Convert.ToDateTime(statement.MovementDate),
                        Description = statement.MovementDescription,
                        DocumentNumber = statement.VoucherNumber,
                        Id = statement.BankStatementId,
                        StatementType = statementType,
                    });
                }
                reconciliation.BankStatements = statements;
                reconciliations.Add(reconciliation);

                reconciliation = DelegateService.reconciliationService.SaveReconciliations(reconciliations);

                reconciliationIds.Add(new
                {
                    ConciliationId = reconciliation.Id
                });
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return Json(reconciliationIds, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetConciliationDetailsByConciliationId
        /// Obtiene el detalle de conciliación por el id de conciliación
        /// </summary>
        /// <param name="conciliationId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetConciliationDetailsByConciliationId(int conciliationId)
        {
            List<Reconciliation> reconciliations =  DelegateService.reconciliationService.GetReconciliationsByReconciliationId(conciliationId);
            List<object> statements = new List<object>();

            foreach (Reconciliation reconciliation in reconciliations)
            {
                foreach (Statement statement in reconciliation.BankStatements)
                {
                    string origin = "";
                    if (statement.StatementType == StatementTypes.Bank)
                    {
                        origin = "B";
                    }
                    else if (statement.StatementType == StatementTypes.CentralAccounting)
                    {
                        origin = "C";
                    }
                    else
                    {
                        origin = "D";
                    }

                    statements.Add(new
                    {
                        MovementOrigin = origin,
                        Id = statement.Id,
                        BankId = statement.BankAccountCompany.Id,
                        MovementId = statement.Id,
                        BankingMovementTypeId = statement.ReconciliationMovementType.Id,
                        BankingMovementTypeDescription = statement.ReconciliationMovementType.Description,
                        BranchId = statement.Branch.Id,
                        BranchDescription = statement.Branch.Description,
                        VoucherNumber = statement.DocumentNumber,
                        MovementDescription = statement.Description,
                        MovementDate = statement.Date.ToString("dd/MM/yyyy"),
                        MovementAmount = String.Format(new CultureInfo("en-US"), "{0:C}", statement.LocalAmount.Value)
                    });
                }
            }

            return new UifTableResult(statements);
        }

        /// <summary>
        /// GetConciliationSummaryByConciliationId
        /// Obtiene el resumen de conciliación por id de conciliación
        /// </summary>
        /// <param name="conciliationId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetConciliationSummaryByConciliationId(int conciliationId)
        {
            int groupId = 0;
            string movementTypeId = "";
            string movementTypeDescription = string.Empty;
            string documentNumber = string.Empty;
            string movementDate = string.Empty;
            decimal bankAmount = 0;
            decimal companyAmount = 0;

            List<Reconciliation> reconciliations = DelegateService.reconciliationService.GetReconciliationsByReconciliationId(conciliationId);

            List<object> conciliationSummaries = new List<object>();

            foreach (Reconciliation reconciliation in reconciliations)
            {
                string sourceMovementTypeId = string.Empty;
                string sourceMovementTypeDescription = string.Empty;
                string sourceVoucherNumber = string.Empty;
                string sourceMovementDate = string.Empty;

                foreach (Statement statement in reconciliation.BankStatements)
                {
                    if (statement.StatementType == StatementTypes.Bank)
                    {
                        bankAmount += Convert.ToDecimal(statement.LocalAmount.Value);
                    }
                    if (statement.StatementType == StatementTypes.CentralAccounting)
                    {
                        companyAmount += Convert.ToDecimal(statement.LocalAmount.Value);
                    }
                    if (statement.StatementType == StatementTypes.DailyAccounting)
                    {
                        companyAmount += Convert.ToDecimal(statement.LocalAmount.Value);
                    }
                    if (sourceMovementTypeId != statement.ReconciliationMovementType.Id.ToString())
                    {
                        movementTypeId = "**";
                    }
                    if (sourceMovementTypeDescription != statement.ReconciliationMovementType.Description)
                    {
                        movementTypeDescription = "**";
                    }
                    if (sourceVoucherNumber != statement.DocumentNumber)
                    {
                        documentNumber = "**";
                    }
                    if (sourceMovementDate != statement.Date.ToString("dd/MM/yyyy"))
                    {
                        movementDate = "**";
                    }

                    sourceMovementTypeId = statement.ReconciliationMovementType.Id.ToString();
                    sourceMovementTypeDescription = statement.ReconciliationMovementType.Description;
                    sourceVoucherNumber = statement.DocumentNumber;
                    sourceMovementDate = statement.Date.ToString("dd/MM/yyyy");
                }

                conciliationSummaries.Add(new
                {
                    ConciliationId = reconciliation.Id,
                    GroupId = groupId,
                    BankingMovementTypeId = movementTypeId,
                    BankingMovementTypeDescription = movementTypeDescription,
                    VoucherNumber = documentNumber,
                    MovementDate = movementDate,
                    MovementBankAmount = bankAmount,
                    MovementAccountingAmount = companyAmount
                });
            }

            return new UifTableResult(conciliationSummaries);
        }

        /// <summary>
        /// ReverseConciliation
        /// Reversión de conciliación
        /// </summary>
        /// <param name="reverseConciliationModel"></param>
        /// <returns>ActionResult</returns>
        public ActionResult ReverseConciliation(ReverseReconciliationModel reverseConciliationModel)
        {
            List<Statement> statements = new List<Statement>();
            int statusId = 0;
            List<object> statusIds = new List<object>();
            try
            {
                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                foreach (BankReconciliationModel reverseConciliation in reverseConciliationModel.Conciliations)
                {
                    Reconciliation reconciliation = new Reconciliation();

                    if (reverseConciliation.Statements != null)
                    {
                        foreach (BankStatementModel statement in reverseConciliation.Statements)
                        {
                            StatementTypes statementType = StatementTypes.Bank;

                            if (statement.MovementOrigin == "B")
                            {
                                statementType = StatementTypes.Bank;
                            }
                            else if (statement.MovementOrigin == "C")
                            {
                                statementType = StatementTypes.CentralAccounting;
                            }
                            else if (statement.MovementOrigin == "D")
                            {
                                statementType = StatementTypes.DailyAccounting;
                            }

                            statements.Add(new Statement()
                            {
                                BankAccountCompany = new BankAccountCompanyDTO() { Id = reverseConciliation.AccountBankId },
                                Amount = new Amount() { Value = Convert.ToDecimal(statement.MovementAmount) },
                                LocalAmount = new Amount() { Value = Convert.ToDecimal(statement.MovementAmount) },
                                Branch = new Branch() { Id = statement.BranchId },
                                ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                                {
                                    Id = statement.BankingMovementTypeId,
                                    Description = statement.BankingMovementTypeDescription
                                },
                                Date = Convert.ToDateTime(statement.MovementDate),
                                Description = statement.MovementDescription,
                                DocumentNumber = statement.VoucherNumber,
                                Id = statement.BankStatementId,
                                StatementType = statementType,
                            });
                        }
                    }

                    reconciliation.BankStatements = statements;
                    reconciliation.CompanyStatements = null;
                    reconciliation.ReconciliationType = ReconciliationTypes.Manual;
                    reconciliation.Date = reverseConciliationModel.ReverseDate;
                    reconciliation.Id = reverseConciliation.Id;
                    reconciliation.UserId = userId;

                    statusId = DelegateService.reconciliationService.ReverseReconciliation(reconciliation);
                }

                statusIds.Add(new
                {
                    StatusId = statusId
                });
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
            return Json(statusIds, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// SortByOptions
        /// Filtro por opciones
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="sortingType"></param>
        /// <param name="sortingDate"></param>
        /// <param name="sortingMonth"></param>
        /// <param name="sortingBranch"></param>
        /// <param name="sortingVoucher"></param>
        /// <returns>List<Statement/></returns>
        private List<Statement> SortByOptions(List<Statement> statements, int sortingType, int sortingDate,
                                              int sortingMonth, int sortingBranch, int sortingVoucher)
        {
            if (sortingType == 1)
            {
                statements = statements.OrderBy(o => o.ReconciliationMovementType.Id).ToList();
                if (sortingDate == 1)
                {
                    statements = statements.OrderBy(o => o.Date).ToList();
                }
                if (sortingBranch == 1)
                {
                    statements = statements.OrderBy(o => o.Branch.Id).ToList();
                }
                if (sortingVoucher == 1)
                {
                    statements = statements.OrderBy(o => o.DocumentNumber).ToList();
                }
            }
            if (sortingDate == 1)
            {
                statements = statements.OrderBy(o => o.Date).ToList();
                if (sortingBranch == 1)
                {
                    statements = statements.OrderBy(o => o.Branch.Id).ToList();
                }
                if (sortingVoucher == 1)
                {
                    statements = statements.OrderBy(o => o.DocumentNumber).ToList();
                }
            }
            if (sortingMonth == 1)
            {
                statements = statements.OrderBy(o => o.Date).ToList();
                if (sortingBranch == 1)
                {
                    statements = statements.OrderBy(o => o.Branch.Id).ToList();
                }
                if (sortingVoucher == 1)
                {
                    statements = statements.OrderBy(o => o.DocumentNumber).ToList();
                }
            }
            if (sortingBranch == 1)
            {
                statements = statements.OrderBy(o => o.Branch.Id).ToList();
                if (sortingVoucher == 1)
                {
                    statements = statements.OrderBy(o => o.DocumentNumber).ToList();
                }
            }
            if (sortingVoucher == 1)
            {
                statements = statements.OrderBy(o => o.DocumentNumber).ToList();
            }

            return statements;
        }

        #endregion

    }
}