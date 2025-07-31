using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Ballot;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Bill;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ACCMOD = Sistran.Core.Application.AccountingServices.DTOs;
using AccountingRuleModels = Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using GLMOD = Sistran.Core.Application.GeneralLedgerServices.DTOs;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ServiceModels = Sistran.Core.Application.AccountingServices.DTOs;
using ACCDTOPAY = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs;
using System.Web;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models;
using Sistran.Core.Framework.UIF.Web.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class TransactionController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 1;
        public const string SortOrder = "ASC";

        #endregion

        #region Instance variables
        readonly CommonController _commonController = new CommonController();
        readonly BillingController _billingController = new BillingController();

        #endregion

        #region BalanceInquiries

        /// <summary>
        /// BalanceInquiries
        /// Levanta la vista BalanceInquiries
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BalanceInquiries()
        {
            try
            {

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
        /// GetBalanceInquiries
        /// Obtiene la consulta de saldos
        /// </summary>
        /// <param name="branch"></param>
        /// <param name="user"></param>
        /// <param name="date"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetBalanceInquiries(int branch, int user, string date)
        {
            int index = 1;
            decimal totalIncomeAmount = 0;
            decimal totalAmount = 0;

            List<SEARCH.BalanceInquieriesDTO> balancesInquieriesDTOs =
            DelegateService.accountingCollectService.GetBalanceInquiries(branch, user, date).OrderBy(o => o.PaymentDescription).ToList();

            List<object> balancesInquieries = new List<object>();

            foreach (SEARCH.BalanceInquieriesDTO balanceInquierie in balancesInquieriesDTOs)
            {
                totalIncomeAmount += balanceInquierie.IncomeAmount;
                totalAmount += balanceInquierie.Amount;

                balancesInquieries.Add(new
                {
                    BranchCode = balanceInquierie.BranchCode,
                    BranchDescription = balanceInquierie.BranchDescription,
                    PaymentMethodTypeCode = balanceInquierie.PaymentMethodTypeCode,
                    PaymentDescription = balanceInquierie.PaymentDescription,
                    CurrencyCode = balanceInquierie.CurrencyCode,
                    CurrencyDescription = balanceInquierie.CurrencyDescription,
                    IncomeAmount = String.Format(new CultureInfo("en-US"), "{0:C}", balanceInquierie.IncomeAmount),
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", balanceInquierie.Amount),
                    UserId = balanceInquierie.UserId,
                    Status = balanceInquierie.Status,
                    RegisterDate = balanceInquierie.RegisterDate
                });
                index++;
            }
            //Se aumenta los totales
            balancesInquieries.Add(new
            {
                BranchCode = 0,
                BranchDescription = "",
                PaymentMethodTypeCode = index + 1000,
                PaymentDescription = "<b>T  O  T  A  L</b>",
                CurrencyCode = -1,
                CurrencyDescription = "",
                IncomeAmount = String.Format(new CultureInfo("en-US"), "{0:C}", totalIncomeAmount),
                Amount = String.Format(new CultureInfo("en-US"), "{0:C}", totalAmount),
                UserId = -1,
                Status = "",
                RegisterDate = ""
            });

            return new UifTableResult(balancesInquieries);
        }

        /// <summary>
        /// GetUserInquiries
        /// Obtiene los usuarios en relación a la sucursal para la consulta de saldos
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetUserInquiries(int branchId)
        {
            List<int> userIds = DelegateService.accountingCollectService.GetUserInquiries(branchId);

            List<object> users = new List<object>();

            foreach (int u in userIds)
            {
                if (DelegateService.uniqueUserService.GetUserById(u).AccountName.ToUpper() == User.Identity.Name.ToUpper())
                {
                    users.Add(new
                    {
                        Id = u,
                        Nick = DelegateService.uniqueUserService.GetUserById(u).AccountName
                    });
                }
            }

            return new UifSelectResult(users);
        }

        #endregion

        #region CheckSearch

        /// <summary>
        /// GetRejectedInformation
        /// Obtiene la información del rechazo de un cheque
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRejectedInformation(int paymentCode)
        {
            try
            {
                List<SEARCH.RejectedPaymentDTO> regularizedInformationsDTOs = DelegateService.accountingPaymentService.GetRejectedInformation(paymentCode);

                return Json(regularizedInformationsDTOs, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetRegularizedInformation
        /// Obtiene la información de la regularización de un cheque
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRegularizedInformation(int paymentCode)
        {
            try
            {
                List<SEARCH.RegularizedPaymentDTO> regularizedPaymentInformationDTOs = DelegateService.accountingPaymentService.GetRegularizedInformation(paymentCode);

                return Json(regularizedPaymentInformationDTOs, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region MainCardDepositSlip

        /// <summary>
        /// Invoca a la vista MainCardDepositSlip
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCardDepositSlip()
        {

            try
            {

                int percentageQuotaParameter = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumPercentageDeposit"]);
                double percentage;

                // Recupera el porcentaje mínimo para pago de Cuotas
                percentage = GetPercentageForPayQuota(percentageQuotaParameter);
                ViewBag.PercentageParameter = percentage;

                int userId = _commonController.GetUserIdByName(User.Identity.Name.ToUpper());
                ViewBag.userId = userId;

                // Setea valor por default de la sucursal de acuerdo al usuario que se conecta 
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
        /// GetCreditCardDepositBallots
        /// Obtiene todas las boletas de tarjetas de crédito para ser depositadas menos las ya depositadas y las anuladas
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="paymentTicketCode"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumberId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="branch"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCreditCardDepositBallots(string creditCardTypeCode, string paymentTicketCode,
                                                      string bankCode, string accountNumberId, string accountNumber, string branch)
        {
            if (String.IsNullOrEmpty(creditCardTypeCode) && String.IsNullOrEmpty(paymentTicketCode) && String.IsNullOrEmpty(bankCode) &&
                String.IsNullOrEmpty(accountNumberId) && String.IsNullOrEmpty(accountNumber) && String.IsNullOrEmpty(branch))
            {
                return Json(new { page = 0, total = 0, records = 0, creditCardPaymentBallot = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int userId = _commonController.GetUserIdByName(User.Identity.Name);

                if (String.IsNullOrEmpty(creditCardTypeCode))
                {
                    creditCardTypeCode = "-1";
                }
                if (String.IsNullOrEmpty(paymentTicketCode))
                {
                    paymentTicketCode = "-1";
                }
                if (String.IsNullOrEmpty(bankCode))
                {
                    bankCode = "-1";
                }

                if (String.IsNullOrEmpty(accountNumberId) || accountNumberId == "null")
                {
                    accountNumber = "-1";
                }

                List<SEARCH.CreditCardPaymentBallotDTO> creditCardPaymentBallotDTOs =
                DelegateService.accountingPaymentBallotService.GetCreditCardDepositBallots(userId, 1, Convert.ToInt32(creditCardTypeCode),
                                                                 Convert.ToInt32(paymentTicketCode),
                                                                 Convert.ToInt32(bankCode), accountNumber,
                                                                 Convert.ToInt32(branch));

                List<object> creditCardPaymentBallots = new List<object>();

                foreach (SEARCH.CreditCardPaymentBallotDTO creditCardPaymentBallot in creditCardPaymentBallotDTOs)
                {
                    creditCardPaymentBallots.Add(new
                    {
                        CreditCardDescription = creditCardPaymentBallot.CreditCardDescription,
                        BankCode = creditCardPaymentBallot.BankCode,
                        BankDescription = creditCardPaymentBallot.BankDescription,
                        AccountNumber = creditCardPaymentBallot.AccountNumber,
                        PaymentTicketCode = creditCardPaymentBallot.PaymentTicketCode,
                        CurrencyDescription = creditCardPaymentBallot.CurrencyDescription,
                        AmountDeposit = string.Format(new CultureInfo("en-US"), "{0:C}", creditCardPaymentBallot.Amount),
                        Taxes = string.Format(new CultureInfo("en-US"), "{0:C}", creditCardPaymentBallot.Taxes),
                        Commission = string.Format(new CultureInfo("en-US"), "{0:C}", creditCardPaymentBallot.Commission),
                        BranchDescription = creditCardPaymentBallot.BranchDescription,
                        CurrencyCode = creditCardPaymentBallot.CurrencyCode
                    });
                }

                return new UifTableResult(creditCardPaymentBallots);
            }
        }

        #endregion

        #region MainCheckPendingDeposit

        /// <summary>
        /// MainCheckPendingDeposit
        /// Invoca a la vista MainCheckPendingDeposit.cshtml
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCheckPendingDeposit()
        {
            try
            {

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
        /// GetChecksDepositingPending
        /// Obtiene los cheques pendientes de depositar
        /// </summary>
        /// <param name="issuingBankCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="branchCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetChecksDepositingPending(string issuingBankCode, string startDate,
                                                     string endDate, string branchCode)
        {
            if (endDate != null && endDate != "")
            {
                endDate = endDate + " 23:59:59";
            }

            DateTime DateFrom = startDate == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(startDate);
            DateTime DateTo = endDate == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(endDate);

            if (String.IsNullOrEmpty(issuingBankCode))
            {
                issuingBankCode = "-1";
            }

            if (String.IsNullOrEmpty(branchCode))
            {
                branchCode = "-1";
            }

            List<SEARCH.CheckInformationDTO> checkInformationDTOs =
            DelegateService.accountingPaymentService.GetChecksDepositingPending(Convert.ToInt32(issuingBankCode), DateFrom,
                                                      DateTo, Convert.ToInt32(branchCode)).OrderBy(o => o.PaymentMethodTypeCode).ToList();

            List<object> checkInformations = new List<object>();

            foreach (SEARCH.CheckInformationDTO checkInformation in checkInformationDTOs)
            {
                checkInformations.Add(new
                {
                    BillCode = checkInformation.CollectCode,
                    TechnicalTransaction = checkInformation.TechnicalTransaction,
                    PaymentCode = checkInformation.PaymentCode,
                    PaymentMethodTypeCode = checkInformation.PaymentMethodTypeCode,
                    BranchCode = checkInformation.BranchCode,
                    BranchDescription = checkInformation.BranchDescription,
                    IssuingBankCode = checkInformation.IssuingBankCode,
                    BankDescription = checkInformation.BankDescription,
                    ReceivingAccountNumber = checkInformation.ReceivingAccountNumber,
                    DocumentNumber = checkInformation.DocumentNumber,
                    DatePayment = Convert.ToDateTime(checkInformation.DatePayment).ToShortDateString(),
                    CurrencyCode = checkInformation.CurrencyCode,
                    CurrencyDescription = checkInformation.CurrencyDescription,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", checkInformation.Amount),
                    Status = checkInformation.Status,
                    StatusDescription = checkInformation.StatusDescription,
                    Holder = checkInformation.Holder,
                    ExchangeRate = checkInformation.ExchangeRate
                });
            }

            return new UifTableResult(checkInformations);
        }

        #endregion

        #region MainCheckDepositSlip

        /// <summary>
        /// MainCheckDepositSlip
        /// Invoca a la vista MainCheckDepositSlip
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCheckDepositSlip()
        {
            try
            {

                int percentageQuotaParameter = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumPercentageDeposit"]);
                double percentage;

                List<object> accountBanks = GetAccountBank();
                ViewBag.AccountBankCompany = accountBanks;

                List<Models.BankAccount> bankAccounts = _commonController.GetAccountByBankId(-1);
                ViewBag.AccountNumberBank = bankAccounts;

                // Recupera el porcentaje mínimo para pago de Cuotas
                percentage = GetPercentageForPayQuota(percentageQuotaParameter);
                ViewBag.PercentageParameter = percentage;

                int userId = _commonController.GetUserIdByName(User.Identity.Name);
                ViewBag.userId = userId;

                // Recupera fecha contable
                DateTime dateAccounting = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                ViewBag.AccountingDate = _commonController.DateFormat(dateAccounting, 1);
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// GetPercentageForPayQuota
        /// Obtiene el porcentaje de parametrización aceptado
        /// </summary>
        /// <param name="numberParameter"></param>
        /// <returns>double</returns>
        public double GetPercentageForPayQuota(int numberParameter)
        {
            List<SEARCH.PercentagePayQuotaDTO> percentagePayQuotaDTOs = DelegateService.accountingCollectControlService.GetPercentageForPayQuota(numberParameter);
            double percentageParameter = Convert.ToDouble(percentagePayQuotaDTOs[0].PercentageParameter);

            return percentageParameter;
        }

        /// <summary>
        /// GetAccountBank
        /// Obtiene las cuentas asociadas a la compañía de la tabla COMM.ACCOUNT_BANK
        /// </summary>
        /// <returns>List<object></returns>
        public List<object> GetAccountBank()
        {
            List<ACCMOD.BankDTO> banks;
            banks = _commonController.GetAccountBank();

            List<object> accountBanks = new List<object>();

            for (int j = 0; j < banks.Count; j++)
            {
                accountBanks.Add(new
                {
                    Id = banks[j].Id.ToString(),
                    Description = banks[j].Description
                });
            }

            return accountBanks;
        }

        /// <summary>
        /// GetCheckBallots
        /// Obtiene las boletas de depósito a excepción de las depositadas
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCheckBallots(int bankCode, string accountNumber, int branchId)
        {
            int userId = _commonController.GetUserIdByName(User.Identity.Name);

            List<SEARCH.PaymentBallotDTO> paymentsBallotDTOs =
            DelegateService.accountingPaymentBallotService.GetCheckBallots(userId, bankCode, accountNumber, branchId
                                ).OrderBy(o => o.DepositBallotRegisterDate).ToList();

            List<object> paymentsBallots = new List<object>();

            foreach (SEARCH.PaymentBallotDTO PaymentBallot in paymentsBallotDTOs)
            {
                paymentsBallots.Add(new
                {
                    DepositBallotAccountNumber = PaymentBallot.DepositBallotAccountNumber,
                    DepositBallotAmount = String.Format(new CultureInfo("en-US"), "{0:C}", PaymentBallot.DepositBallotAmount),
                    DepositBallotBankId = PaymentBallot.DepositBallotBankId,
                    DepositBallotCashAmount = String.Format(new CultureInfo("en-US"), "{0:C}", PaymentBallot.DepositBallotCashAmount),
                    Currency = PaymentBallot.Currency,
                    DepositBallotBankDescription = PaymentBallot.DepositBallotBankDescription,
                    DepositBallotId = PaymentBallot.DepositBallotId,
                    DepositBallotRegisterDate = PaymentBallot.DepositBallotRegisterDate.ToString().Substring(0, 10),
                    Status = PaymentBallot.Status,
                    UserId = PaymentBallot.UserId,
                    Rows = PaymentBallot.Rows,
                    LedgerAccount = PaymentBallot.LedgerAccount,
                });
            }

            return new UifTableResult(paymentsBallots);
        }

        #endregion

        #region SavePaymentBallot

        /// <summary>
        /// SavePaymentBallotRequest
        /// Graba en la tabla PAYMENT_TICKET_BALLOT  y en  PAYMENT_BALLOT
        /// </summary>
        /// <param name="frmPaymentBallot"></param>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SavePaymentBallotRequest(PaymentBallotModel frmPaymentBallot, int userId, int typeId, int branchId)
        {
            try
            {
                int status = Convert.ToInt32(CollectControlStatus.Open);
                PaymentBallotAccountingDTO paymentBallotAccounting = new PaymentBallotAccountingDTO();
                PaymentBallotParametersDTO PaymentBallotParameters = new PaymentBallotParametersDTO();

                PaymentBallotParameters.PaymentAccountNumber = frmPaymentBallot.PaymentAccountNumber;
                PaymentBallotParameters.PaymentBallotAmount = frmPaymentBallot.PaymentBallotAmount;
                PaymentBallotParameters.PaymentBallotBankAmount = frmPaymentBallot.PaymentBallotBankAmount;
                PaymentBallotParameters.PaymentBallotBankId = frmPaymentBallot.PaymentBallotBankId;
                PaymentBallotParameters.PaymentAccountingAccountId = frmPaymentBallot.PaymentAccountingAccountId;
                PaymentBallotParameters.PaymentBallotId = frmPaymentBallot.PaymentBallotId;
                PaymentBallotParameters.PaymentBankDate = frmPaymentBallot.PaymentBankDate;
                PaymentBallotParameters.PaymentCurrency = frmPaymentBallot.PaymentCurrency;
                PaymentBallotParameters.PaymentStatus = frmPaymentBallot.PaymentStatus;
                PaymentBallotParameters.PaymentBallotNumber = frmPaymentBallot.PaymentBallotNumber;

                CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(userId, Convert.ToInt32(branchId), Convert.ToDateTime(DateTime.Now).Date, status);

                List<PaymentTicketBallotDTO> PaymentTicketBallotModels = new List<PaymentTicketBallotDTO>();

                foreach (var item in frmPaymentBallot.PaymentTicketBallotModels)
                {
                    PaymentTicketBallotModels.Add(new PaymentTicketBallotDTO()
                    {
                        PaymentTicketBallotId = item.PaymentTicketBallotId
                    });
                };
                PaymentBallotParameters.PaymentTicketBallotModels = PaymentTicketBallotModels;
                paymentBallotAccounting.PaymentBallotParameters = PaymentBallotParameters;
                paymentBallotAccounting.UserId = userId;
                paymentBallotAccounting.TypeId = typeId;
                paymentBallotAccounting.AccountingDate = collectControl != null ? collectControl.AccountingDate : DateTime.MinValue;

                List<PaymentBallotResponsesDTO> paymentBallotResponses = DelegateService.accountingPaymentBallotService.SaveAccountingPaymentBallot(paymentBallotAccounting);

                return Json(paymentBallotResponses, JsonRequestBehavior.AllowGet);
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

        #region MainCheckSearch

        /// <summary>
        /// MainCheckSearch
        /// Invoca a la vista MainCheckSearch
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCheckSearch()
        {
            try
            {

                List<Branch> branches = _commonController.GetBranchesByUserId(_commonController.GetUserIdByName(User.Identity.Name));
                ViewBag.Branch = branches;

                List<ServiceModels.RejectionDTO> rejections = DelegateService.accountingParameterService.GetRejections();
                ViewBag.RejectionMotive = rejections;

                // Recupera fecha contable
                DateTime dateAccounting = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);

                ViewBag.AccountingDate = _commonController.DateFormat(dateAccounting, 1);
                ViewBag.CurrentDate = DateTime.Now.Date;
                ViewBag.RejectionDate = _commonController.DateFormat(dateAccounting, 0);

                // Setear por defecto la sucursal del usuario 
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
        /// GetChecksUpdated
        /// Obtiene los cheques para su respectiva actualización
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="checkNumber"></param>
        /// <param name="accountNumber"></param>
        /// <param name="technicalTransaction"></param>
        /// <param name="branchCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetChecksUpdated(string bankCode, string checkNumber, string accountNumber, string technicalTransaction, string branchCode)
        {

            if (String.IsNullOrEmpty(bankCode))
            {
                bankCode = "-1";
            }
            if (String.IsNullOrEmpty(checkNumber))
            {
                checkNumber = "-1";
            }
            if (String.IsNullOrEmpty(accountNumber))
            {
                accountNumber = "-1";
            }
            if (String.IsNullOrEmpty(technicalTransaction))
            {
                technicalTransaction = "-1";
            }
            if (String.IsNullOrEmpty(branchCode))
            {
                branchCode = "-1";
            }

            List<SEARCH.CheckInformationDTO> checksInformations =
            DelegateService.accountingPaymentService.GetChecksUpdated(Convert.ToInt32(bankCode), checkNumber, accountNumber, Convert.ToInt32(technicalTransaction),
                                             Convert.ToInt32(branchCode)).OrderBy(o => o.BranchCode).ToList();

            List<object> CheckInformations = new List<object>();

            foreach (SEARCH.CheckInformationDTO checkInformation in checksInformations)
            {
                string formated = String.Format(new CultureInfo("en-US"), "{0:C}", checkInformation.Amount);
                CheckInformations.Add(new
                {
                    Amount = checkInformation.Amount,
                    AmountFormated = formated,
                    BankDescription = checkInformation.BankDescription,
                    BillCode = checkInformation.CollectCode,
                    BillConceptCode = checkInformation.CollectConceptCode,
                    BillConceptDescription = checkInformation.CollectConceptDescription,
                    BranchCode = checkInformation.BranchCode,
                    BranchDescription = checkInformation.BranchDescription,
                    CurrencyCode = checkInformation.CurrencyCode,
                    CurrencyDescription = checkInformation.CurrencyDescription,
                    DatePayment = checkInformation.DatePayment,
                    DocumentNumber = checkInformation.DocumentNumber,
                    ExchangeRate = checkInformation.ExchangeRate,
                    Holder = checkInformation.Holder,
                    IssuingAccountNumber = checkInformation.IssuingAccountNumber,
                    IssuingBankCode = checkInformation.IssuingBankCode,
                    Name = checkInformation.Name,
                    PayerId = checkInformation.PayerId,
                    PaymentBallotCode = checkInformation.PaymentBallotCode,
                    PaymentBallotNumber = checkInformation.PaymentBallotNumber,
                    PaymentCode = checkInformation.PaymentCode,
                    PaymentMethodTypeCode = checkInformation.PaymentMethodTypeCode,
                    PaymentStatus = checkInformation.PaymentStatus,
                    PaymentTicketCode = checkInformation.PaymentTicketCode,
                    PaymentTicketcheckInformationCode = checkInformation.PaymentTicketItemCode,
                    PaymentTicketStatus = checkInformation.PaymentTicketStatus,
                    ReceivingAccountNumber = checkInformation.ReceivingAccountNumber,
                    ReceivingBankCode = checkInformation.ReceivingBankCode,
                    ReceivingBankName = checkInformation.ReceivingBankName,
                    RegisterDate = checkInformation.RegisterDate,
                    Status = checkInformation.Status,
                    StatusDescription = checkInformation.StatusDescription,
                });
            }

            return new UifTableResult(CheckInformations);
        }

        #endregion

        #region CheckSearch

        /// <summary>
        /// GetSearchCheck
        /// Obtiene los cheques dado el código de pago, banco, número y cuenta 
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <param name="bankCode"></param>
        /// <param name="accountNumber"></param>
        /// <param name="checkNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetSearchCheck(int paymentCode, int bankCode, string accountNumber, string checkNumber)
        {
            List<SEARCH.CheckInformationDTO> checkInformationDTOs = DelegateService.accountingPaymentService.GetChecks(paymentCode, bankCode, accountNumber, checkNumber);

            return Json(checkInformationDTOs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetInternalBallotInformation
        /// Obtiene la cabecera de la boleta interna de depósito
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInternalBallotInformation(int paymentCode)
        {
            List<SEARCH.DepositCheckInformationDTO> depositCheckInformationDTOs = DelegateService.accountingPaymentService.GetInternalBallotInformation(paymentCode);

            return Json(depositCheckInformationDTOs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetDepositInformation
        /// Obtiene el detalle de la boleta interna de depósito
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDepositInformation(int paymentCode)
        {
            List<SEARCH.DepositCheckInformationDTO> depositCheckInformationDTOs = DelegateService.accountingPaymentService.GetDepositInformation(paymentCode);

            return Json(depositCheckInformationDTOs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetLegalInformation
        /// Obtiene la información del paso a legal de un cheque
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetLegalInformation(int paymentCode)
        {
            List<SEARCH.LegalPaymentDTO> legalPaymentDTOs = DelegateService.accountingPaymentService.GetLegalInformation(paymentCode);

            return Json(legalPaymentDTOs, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ChangeCheck

        /// <summary>
        /// SaveChangeCheck
        /// Cambia el estado del cheque original a canjeado e inserta el nuevo cheque en la tabla BILL.PAYMENT
        /// </summary>
        /// <param name="frmBill"></param>
        /// <param name="payerId"></param>
        /// <param name="descriptionChange"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveChangeCheck(PaymentSummaryModel frmBill, int payerId, string descriptionChange)
        {

            int billId = 0;

            CheckDTO check = new CheckDTO()
            {
                Amount = new ACCMOD.AmountDTO()
                {
                    Currency = new SEARCH.CurrencyDTO() { Id = frmBill.CurrencyId },
                    Value = frmBill.Amount
                },
                Date = frmBill.CheckPayments[0].Date,
                DocumentNumber = frmBill.CheckPayments[0].DocumentNumber,
                ExchangeRate = new ACCMOD.ExchangeRateDTO()
                {
                    BuyAmount = frmBill.ExchangeRate,
                    SellAmount = frmBill.ExchangeRate
                },
                Id = frmBill.PaymentId,
                IssuerName = frmBill.CheckPayments[0].IssuerName,
                IssuingAccountNumber = frmBill.CheckPayments[0].IssuingAccountNumber,
                IssuingBank = new ACCMOD.BankDTO() { Id = frmBill.CheckPayments[0].IssuingBankId },
                LocalAmount = new ACCMOD.AmountDTO() { Value = (frmBill.Amount * frmBill.ExchangeRate) },
                PaymentMethod = new ACCDTOPAY.PaymentMethodDTO() { Id = Convert.ToInt32(frmBill.PaymentMethodId) },
                Status = Convert.ToInt16(PaymentStatus.Active),
                BranchId = frmBill.BranchId
            };

            billId = frmBill.BillId;

            int userId = _commonController.GetUserIdByName(User.Identity.Name);
            DateTime accountingDate;
            int status = Convert.ToInt32(CollectControlStatus.Open);
            CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(userId, Convert.ToInt32(frmBill.BranchId), Convert.ToDateTime(DateTime.Now).Date, status);
            accountingDate = collectControl != null ? collectControl.AccountingDate : DateTime.MinValue;
            // Graba el nuevo cheque canjeado
            MessageSuccessDTO messageSuccessDTO = DelegateService.accountingPaymentService.SaveChangeCheck(check, frmBill.PaymentId, billId, userId, payerId, descriptionChange, accountingDate);

            var checkInformation = new
            {
                Message = messageSuccessDTO.ImputationMessage,
                BillId = Convert.ToString(messageSuccessDTO.TechnicalTransaction),
                ShowMessage = Convert.ToString(messageSuccessDTO.ShowMessage),
                messageSuccessDTO.GeneralLedgerSuccess
            };

            return Json(checkInformation, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ExportToExcel

        /// <summary>
        /// ExportListToExcel
        /// Contabilizar el depósito de la boleta
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="checkNumber"></param>
        /// <param name="accountNumber"></param>
        /// <param name="technicalTransaction"></param>
        /// <param name="branchCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ExportListToExcel(string bankCode, string checkNumber, string accountNumber,
                                              string technicalTransaction, string branchCode)
        {

            if (String.IsNullOrEmpty(bankCode))
            {
                bankCode = "-1";
            }
            if (String.IsNullOrEmpty(checkNumber))
            {
                checkNumber = "-1";
            }
            if (String.IsNullOrEmpty(accountNumber))
            {
                accountNumber = "-1";
            }
            if (String.IsNullOrEmpty(technicalTransaction))
            {
                technicalTransaction = "-1";
            }
            if (String.IsNullOrEmpty(branchCode))
            {
                branchCode = "-1";
            }

            List<SEARCH.CheckInformationDTO> checkInformationDTOs =
            DelegateService.accountingPaymentService.GetChecksUpdated(Convert.ToInt32(bankCode), checkNumber, accountNumber, Convert.ToInt32(technicalTransaction),
                                             Convert.ToInt32(branchCode)).OrderBy(o => o.BranchCode).ToList();

            List<SEARCH.CheckInformationDTO> checkInformations = new List<SEARCH.CheckInformationDTO>();

            foreach (SEARCH.CheckInformationDTO checkInformation in checkInformationDTOs)
            {
                checkInformations.Add(new SEARCH.CheckInformationDTO()
                {
                    Amount = checkInformation.Amount,
                    BankDescription = checkInformation.BankDescription,
                    CollectCode = checkInformation.CollectCode,
                    CollectConceptCode = checkInformation.CollectConceptCode,
                    CollectConceptDescription = checkInformation.CollectConceptDescription,
                    BranchCode = checkInformation.BranchCode,
                    BranchDescription = checkInformation.BranchDescription,
                    CurrencyCode = checkInformation.CurrencyCode,
                    CurrencyDescription = checkInformation.CurrencyDescription,
                    DatePayment = checkInformation.DatePayment,
                    DocumentNumber = checkInformation.DocumentNumber,
                    ExchangeRate = checkInformation.ExchangeRate,
                    Holder = checkInformation.Holder,
                    IssuingAccountNumber = checkInformation.IssuingAccountNumber,
                    IssuingBankCode = checkInformation.IssuingBankCode,
                    Name = checkInformation.Name,
                    PayerId = checkInformation.PayerId,
                    PaymentBallotCode = checkInformation.PaymentBallotCode,
                    PaymentBallotNumber = checkInformation.PaymentBallotNumber,
                    PaymentCode = checkInformation.PaymentCode,
                    PaymentMethodTypeCode = checkInformation.PaymentMethodTypeCode,
                    PaymentStatus = checkInformation.PaymentStatus,
                    PaymentTicketCode = checkInformation.PaymentTicketCode,
                    PaymentTicketItemCode = checkInformation.PaymentTicketItemCode,
                    PaymentTicketStatus = checkInformation.PaymentTicketStatus,
                    ReceivingAccountNumber = checkInformation.ReceivingAccountNumber,
                    ReceivingBankCode = checkInformation.ReceivingBankCode,
                    ReceivingBankName = checkInformation.ReceivingBankName,
                    RegisterDate = checkInformation.RegisterDate,
                    Status = checkInformation.Status,
                    StatusDescription = checkInformation.StatusDescription,

                });
            }

            try
            {
                MemoryStream dataStream = ExportListToExcel(ConvertCheckListToDataTable(checkInformations));
                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + UploadReport(dataStream.ToArray(), Global.CheckSearchListExportFileName));
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportExcel);
            }
        }

        private string UploadReport(byte[] byteArray, string fileName)
        {
            string path = DelegateService.commonService.GetKeyApplication("SavePathExcel") + @"\" + this.User.Identity.Name + @"\";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += Guid.NewGuid() + Path.GetExtension(fileName);
                System.IO.File.WriteAllBytes(path, byteArray);
                
                return path;
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                throw ex;
            }
        }

        /// <summary>
        /// ConvertCheckListToDataTable
        /// Contabilizar el depósito de la boleta
        /// </summary>
        /// <param name="checkInformations"></param>
        /// <returns>DataTable</returns>
        private DataTable ConvertCheckListToDataTable(List<SEARCH.CheckInformationDTO> checkInformations)
        {
            DataTable dataTable = new DataTable();

            var headerRow = new List<string>(7);

            headerRow.Add(Global.Branch);
            headerRow.Add(Global.IssuingBank);
            headerRow.Add(Global.BankAccountNumber);
            headerRow.Add(Global.CheckNumber);
            headerRow.Add(Global.CheckDate);
            headerRow.Add(Global.Currency);
            headerRow.Add(Global.Amount);
            headerRow.Add(Global.Status);

            for (int j = 0; j < headerRow.Count; j++)
            {
                dataTable.Columns.Add(headerRow[j]);
            }

            try
            {
                foreach (SEARCH.CheckInformationDTO checkInformation in checkInformations)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow[0] = checkInformation.BranchDescription;
                    dataRow[1] = checkInformation.BankDescription;
                    dataRow[2] = checkInformation.ReceivingAccountNumber;
                    dataRow[3] = checkInformation.DocumentNumber;
                    dataRow[4] = checkInformation.DatePayment;
                    dataRow[5] = checkInformation.CurrencyDescription;
                    dataRow[6] = checkInformation.Amount;
                    dataRow[7] = checkInformation.StatusDescription;

                    dataTable.Rows.Add(dataRow);
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }

            return dataTable;
        }

        /// <summary>
        /// ExportListToExcel
        /// Bloque de bytes.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportListToExcel(DataTable dataTable)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            font.FontName = "Tahoma";
            font.FontHeightInPoints = 8;
            font.Boldweight = 3;
            font.Color = HSSFColor.White.Index;

            ICellStyle styleHeader = workbook.CreateCellStyle();
            styleHeader.SetFont(font);
            styleHeader.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
            styleHeader.FillPattern = FillPattern.SolidForeground;

            var fontDetail = workbook.CreateFont();
            fontDetail.FontName = "Tahoma";
            fontDetail.FontHeightInPoints = 8;
            fontDetail.Boldweight = 3;

            ICellStyle styleDetail = workbook.CreateCellStyle();
            styleDetail.SetFont(fontDetail);
            styleDetail.BottomBorderColor = HSSFColor.Black.Index;
            styleDetail.LeftBorderColor = HSSFColor.Black.Index;
            styleDetail.RightBorderColor = HSSFColor.Black.Index;
            styleDetail.TopBorderColor = HSSFColor.Black.Index;
            styleDetail.BorderBottom = BorderStyle.Thin;
            styleDetail.BorderLeft = BorderStyle.Thin;
            styleDetail.BorderRight = BorderStyle.Thin;
            styleDetail.BorderTop = BorderStyle.Thin;

            var headerRow = sheet.CreateRow(0);

            headerRow.CreateCell(0).SetCellValue(Global.Branch);
            headerRow.CreateCell(1).SetCellValue(Global.IssuingBank);
            headerRow.CreateCell(2).SetCellValue(Global.BankAccountNumber);
            headerRow.CreateCell(3).SetCellValue(Global.CheckNumber);
            headerRow.CreateCell(4).SetCellValue(Global.CheckDate);
            headerRow.CreateCell(5).SetCellValue(Global.Currency);
            headerRow.CreateCell(6).SetCellValue(Global.Amount);
            headerRow.CreateCell(7).SetCellValue(Global.Status);
            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.CreateFreezePane(0, 1, 0, 1);
            sheet.CreateFreezePane(0, 1, 0, 1);
            headerRow.GetCell(0).CellStyle = styleHeader;
            headerRow.GetCell(1).CellStyle = styleHeader;
            headerRow.GetCell(2).CellStyle = styleHeader;
            headerRow.GetCell(3).CellStyle = styleHeader;
            headerRow.GetCell(4).CellStyle = styleHeader;
            headerRow.GetCell(5).CellStyle = styleHeader;
            headerRow.GetCell(6).CellStyle = styleHeader;
            headerRow.GetCell(7).CellStyle = styleHeader;

            int rowNumber = 1;

            foreach (DataRow item in dataTable.Rows)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(item.ItemArray[i].ToString());
                    row.GetCell(i).CellStyle = styleDetail;
                }
            }

            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
            anchor.AnchorType = 5;
            MemoryStream memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            return memoryStream;
        }

        #endregion ExportToExcel
    }
}