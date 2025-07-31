using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

// sistran FWK
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class CardInternalDepositSlipController : Controller
    {
        #region Constants

        public const string SortOrder = "ASC";

        #endregion

        #region Instance variables

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// MainCardInternalDepositSlip
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCardInternalDepositSlip()
        {
            try
            {       

                // TIPOS DE TARJETA DE CREDITO ACREDITABLE (EE--> 4  BB --> -1)/ NO ACREDITABLE (EE--> 5  BB --> 0)
                ViewBag.Creditable = Convert.ToInt32(ConfigurationManager.AppSettings["Creditable"]);
                ViewBag.NoCreditable = Convert.ToInt32(ConfigurationManager.AppSettings["NoCreditable"]);
                ViewBag.PaymentMethodCash = Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCash"]);

                //Setear valor por default de la sucursal de acuerdo al usuario que se conecta
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        ///<summary>
        /// Index
        ///</summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Actions

        /// <summary>
        /// ReplaceWithAsterisks
        /// </summary>
        /// <param name="cellValue"></param>
        /// <returns>string</returns>
        public string ReplaceWithAsterisks(string cellValue)
        {
            int row = 0;
            string valueWithAsterisk = "";

            if (cellValue.Length >= 4)
            {
                string lastNumber = cellValue.Substring(cellValue.Length - 4);

                for (row = 0; row < cellValue.Length - 2; row++)
                {
                    valueWithAsterisk += "*";
                }

                valueWithAsterisk = valueWithAsterisk + lastNumber;
                cellValue = valueWithAsterisk;
            }

            return cellValue;
        }

        /// <summary>
        /// GetInternalBallotCardHeader
        /// Obtiene la cabecera de tarjeta de crédito acreditables y no acreditable
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInternalBallotCardHeader(int paymentTicketCode, int paymentMethodTypeCode)
        {
            CreditableCardHeaderDTO creditableCardHeader = DelegateService.accountingPaymentTicketService.GetCreditableHeaderCard(paymentTicketCode, paymentMethodTypeCode);

            List<object> creditableCardHeaders = new List<object>();

            if (creditableCardHeader != null)
            {
                creditableCardHeaders.Add(new
                {
                    AccountNumber = creditableCardHeader.AccountNumber,
                    Amount = creditableCardHeader.Amount,
                    BankCode = creditableCardHeader.BankCode,
                    BankName = creditableCardHeader.BankName,
                    BranchCode = creditableCardHeader.BranchCode,
                    BranchName = creditableCardHeader.BranchName,
                    CommissionAmount = creditableCardHeader.CommissionAmount,
                    CreditCardTypeCode = creditableCardHeader.CreditCardTypeCode,
                    CreditCardTypeName = creditableCardHeader.CreditCardTypeName,
                    CurrencyCode = creditableCardHeader.CurrencyCode,
                    CurrencyName = creditableCardHeader.CurrencyName,
                    PaymentMethodTypeCode = creditableCardHeader.PaymentMethodTypeCode,
                    PaymentTicketCode = creditableCardHeader.PaymentTicketCode
                });
            }
            else
            {
                creditableCardHeaders = null;
            }

            return Json(creditableCardHeaders, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCardsToDepositBallot
        /// Obtiene las tarjetas acreditables y no acreditables a depositar en una boleta interna. Se filtra por el tipo de pago, conducto, sucursal y
        /// fecha ingreso hasta (obligatorio), banco receptor, número de cuenta, moneda de la cuenta (obligatorios si el tipo de tarjeta es acreditable) 
        /// banco emisor y número de voucher (opcionales) 
        /// </summary>
        /// <param name="paymentMethodTypeCode"></param>
        /// <param name="creditCardType"></param>
        /// <param name="branchCode"></param>
        /// <param name="cardDate"></param>
        /// <param name="currencyCode"></param>
        /// <param name="issuingBankCode"></param>
        /// <param name="voucherNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCardsToDepositBallot(string paymentMethodTypeCode, string creditCardType, string branchCode,
                                                  string cardDate, string currencyCode, string issuingBankCode, string voucherNumber)
        {
            if (String.IsNullOrEmpty(paymentMethodTypeCode))
            {
                paymentMethodTypeCode = "-1";
            }
            if (String.IsNullOrEmpty(currencyCode))
            {
                currencyCode = "-1";
            }
            if (String.IsNullOrEmpty(issuingBankCode))
            {
                issuingBankCode = "-1";
            }
            if (String.IsNullOrEmpty(voucherNumber))
            {
                voucherNumber = "-1";
            }
            if (String.IsNullOrEmpty(creditCardType))
            {
                creditCardType = "-1";
            }
            if (String.IsNullOrEmpty(cardDate))
            {
                cardDate = "*";
            }
            if (String.IsNullOrEmpty(branchCode))
            {
                branchCode = "-1";
            }

            List<CardToDepositInternalBallotDTO> cardToDepositInternalBallotDTOs =
            DelegateService.accountingPaymentTicketService.GetCardsToDepositBallot(Convert.ToInt32(paymentMethodTypeCode), Convert.ToInt32(currencyCode),
                                                         Convert.ToInt32(issuingBankCode), Convert.ToInt32(voucherNumber),
                                                         Convert.ToInt32(creditCardType), Convert.ToInt32(branchCode), Convert.ToDateTime(cardDate));

            var cardToDepositInternalBallots = (from CardToDepositInternalBallotDTO cardToDepositInternalBallot in cardToDepositInternalBallotDTOs
                                                       select new
                                                       {
                                                           PaymentCode = cardToDepositInternalBallot.PaymentCode,
                                                           PaymentMethodTypeCode = cardToDepositInternalBallot.PaymentMethodTypeCode,
                                                           CurrencyCode = cardToDepositInternalBallot.CurrencyCode,
                                                           BankName = cardToDepositInternalBallot.BankName,
                                                           VoucherNumber = cardToDepositInternalBallot.VoucherNumber,
                                                           CardNumber = ReplaceWithAsterisks(cardToDepositInternalBallot.CardNumber),
                                                           CardDate = cardToDepositInternalBallot.CardDate.ToString("dd/MM/yyyy"),
                                                           TechnicalTransaction = cardToDepositInternalBallot.TechnicalTransaction,
                                                           ReceiptNumber = cardToDepositInternalBallot.ReceiptNumber,
                                                           CurrencyName = cardToDepositInternalBallot.CurrencyName,
                                                           Amount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.Amount),
                                                           TaxAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.TaxAmount),
                                                           CommissionAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.CommissionAmount),
                                                           Holder = cardToDepositInternalBallot.Holder,
                                                           BranchCode = cardToDepositInternalBallot.BranchCode
                                                       }).ToList();

            return Json(new { aaData = cardToDepositInternalBallots, total = cardToDepositInternalBallots.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetDetailInternalBallotCard
        /// Obtiene el detalle de una boleta interna de depósito de tarjetas
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <param name="paymentMethodTypeCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDetailInternalBallotCard(int? paymentTicketCode, int? paymentMethodTypeCode)
        {
            object cardToDepositInternalBallots;

            if (paymentTicketCode.HasValue && paymentMethodTypeCode.HasValue)
            {
                List<CardToDepositInternalBallotDTO> cardToDepositInternalBallotDTOs =
                DelegateService.accountingPaymentTicketService.GetCreditableDetailCard(Convert.ToInt32(paymentTicketCode), Convert.ToInt32(paymentMethodTypeCode));

                cardToDepositInternalBallots = (from CardToDepositInternalBallotDTO cardToDepositInternalBallot in cardToDepositInternalBallotDTOs
                                                select new
                                                {
                                                    PaymentCode = cardToDepositInternalBallot.PaymentCode,
                                                    CreditCardTypeCode = cardToDepositInternalBallot.CreditCardTypeCode,
                                                    PaymentMethodTypeCode = cardToDepositInternalBallot.PaymentMethodTypeCode,
                                                    PaymentMethodTypeName = cardToDepositInternalBallot.PaymentMethodTypeName,
                                                    IssuingBankCode = cardToDepositInternalBallot.IssuingBankCode,
                                                    BankName = cardToDepositInternalBallot.BankName,
                                                    VoucherNumber = cardToDepositInternalBallot.VoucherNumber,
                                                    CardNumber = ReplaceWithAsterisks(cardToDepositInternalBallot.CardNumber),
                                                    ReceiptNumber = cardToDepositInternalBallot.ReceiptNumber,
                                                    TechnicalTransaction = cardToDepositInternalBallot.TechnicalTransaction,
                                                    CurrencyCode = cardToDepositInternalBallot.CurrencyCode,
                                                    CurrencyName = cardToDepositInternalBallot.CurrencyName,
                                                    ExchangeRate = cardToDepositInternalBallot.ExchangeRate,
                                                    IncomeAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.IncomeAmount),
                                                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.Amount),
                                                    CardDate = cardToDepositInternalBallot.CardDate.ToString("dd/MM/yyyy"),
                                                    Holder = cardToDepositInternalBallot.Holder,
                                                    BranchCode = cardToDepositInternalBallot.BranchCode,
                                                    Status = cardToDepositInternalBallot.Status,
                                                    PaymentTicketItemId = cardToDepositInternalBallot.PaymentTicketItemId,
                                                    TaxAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.TaxAmount),
                                                    CommissionAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.CommissionAmount),
                                                }).ToList();

                return Json(new { aaData = cardToDepositInternalBallots }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                List<CardToDepositInternalBallotDTO> payments = DelegateService.accountingPaymentTicketService.GetCreditableDetailCard(Convert.ToInt32(paymentTicketCode), Convert.ToInt32(paymentMethodTypeCode));

                cardToDepositInternalBallots = (from CardToDepositInternalBallotDTO cardToDepositInternalBallot in payments
                                                select new
                                                {
                                                    PaymentCode = cardToDepositInternalBallot.PaymentCode,
                                                    CreditCardTypeCode = cardToDepositInternalBallot.CreditCardTypeCode,
                                                    PaymentMethodTypeCode = cardToDepositInternalBallot.PaymentMethodTypeCode,
                                                    PaymentMethodTypeName = cardToDepositInternalBallot.PaymentMethodTypeName,
                                                    IssuingBankCode = cardToDepositInternalBallot.IssuingBankCode,
                                                    BankName = cardToDepositInternalBallot.BankName,
                                                    VoucherNumber = cardToDepositInternalBallot.VoucherNumber,
                                                    CardNumber = ReplaceWithAsterisks(cardToDepositInternalBallot.CardNumber),
                                                    ReceiptNumber = cardToDepositInternalBallot.ReceiptNumber,
                                                    TechnicalTransaction = cardToDepositInternalBallot.TechnicalTransaction,
                                                    CurrencyCode = cardToDepositInternalBallot.CurrencyCode,
                                                    CurrencyName = cardToDepositInternalBallot.CurrencyName,
                                                    ExchangeRate = cardToDepositInternalBallot.ExchangeRate,
                                                    IncomeAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.IncomeAmount),
                                                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.Amount),
                                                    CardDate = cardToDepositInternalBallot.CardDate.ToString("dd/MM/yyyy"),
                                                    Holder = cardToDepositInternalBallot.Holder,
                                                    BranchCode = cardToDepositInternalBallot.BranchCode,
                                                    Status = cardToDepositInternalBallot.Status,
                                                    PaymentTicketItemId = cardToDepositInternalBallot.PaymentTicketItemId,
                                                    TaxAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.TaxAmount),
                                                    CommissionAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardToDepositInternalBallot.CommissionAmount),
                                                }).ToList();

                return Json(new { aaData = cardToDepositInternalBallots }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}