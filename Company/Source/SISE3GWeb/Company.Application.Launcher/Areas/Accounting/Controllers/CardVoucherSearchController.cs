using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;

// Sistran Core
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.AccountingServices.DTOs;
//using  = Sistran.Core.Application.AccountingServices.Models;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [FilterConfigHelper.NoDirectAccessAttribute]
    public class CardVoucherSearchController : Controller
    {
        #region Instance Variables

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// MainCardVoucherSearch
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCardVoucherSearch()
        {

            try
            {
     
                ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ViewBag.RegularizationCards = ConfigurationManager.AppSettings["RegularizationCards"];

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

        /// <summary>
        /// MainCardPendingDeposit
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCardPendingDeposit()
        {
            try
            {

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
        /// GetCardVoucherSearch
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="voucher"></param>
        /// <param name="documentNumber"></param>
        /// <param name="branchCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <param name="technicalTransaction"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCardVoucherSearch(string creditCardTypeCode, string voucher, string documentNumber,
                                               string technicalTransaction, string branchCode, string startDate,
                                               string endDate, string status)
        {
            List<object> cardVouchers = new List<object>();

            if (!String.IsNullOrEmpty(endDate))
            {
                endDate = endDate + " 23:59:59";
            }

            if (String.IsNullOrEmpty(creditCardTypeCode))
            {
                creditCardTypeCode = "-1";
            }
            if (String.IsNullOrEmpty(voucher))
            {
                voucher = "-1";
            }
            if (String.IsNullOrEmpty(documentNumber))
            {
                documentNumber = "-1";
            }
            if (String.IsNullOrEmpty(technicalTransaction))
            {
                technicalTransaction = "-1";
            }
            if (String.IsNullOrEmpty(branchCode))
            {
                branchCode = "-1";
            }

            DateTime dateFrom = Convert.ToDateTime("01/01/1900");
            DateTime dateTo = Convert.ToDateTime("01/01/1900");

            if (String.IsNullOrEmpty(status))
            {
                status = "-1";
            }

            List<CardVoucherDTO> cardVoucherDTOs =
            DelegateService.accountingPaymentService.GetCardVoucher(Convert.ToInt32(creditCardTypeCode),voucher,
                                          Convert.ToInt32(documentNumber), Convert.ToInt32(technicalTransaction),
                                          Convert.ToInt32(branchCode), dateFrom, dateTo, Convert.ToInt32(status)).OrderBy(o => o.Voucher).ToList();

            foreach (CardVoucherDTO cardVoucher in cardVoucherDTOs)
            {
                cardVouchers.Add(new
                {
                    BranchCode = cardVoucher.BranchCode,
                    Description = cardVoucher.Description,
                    CardDescription = cardVoucher.CardDescription,
                    DocumentNumber = ReplaceWithAsterisks(cardVoucher.DocumentNumber),
                    Voucher = cardVoucher.Voucher,
                    CurrencyCode = cardVoucher.CurrencyCode,
                    CurrencyDescription = cardVoucher.CurrencyDescription,
                    Amount = string.Format(new CultureInfo("en-US"), "{0:C}", cardVoucher.Amount),
                    Taxes = string.Format(new CultureInfo("en-US"), "{0:C}", cardVoucher.Taxes),
                    Retention = string.Format(new CultureInfo("en-US"), "{0:C}", cardVoucher.Retention),
                    TechnicalTransaction = cardVoucher.TechnicalTransaction,
                    CollectCode = cardVoucher.CollectCode,
                    CardDate = cardVoucher.CardDate,
                    Status = cardVoucher.Status,
                    StatusDescription = cardVoucher.StatusDescription,
                    CreditCardTypeCode = cardVoucher.CreditCardTypeCode,
                    PaymentCode = cardVoucher.PaymentCode
                });
            }

            return new UifTableResult(cardVouchers);
        }

        /// <summary>
        /// GetCardVoucher
        /// </summary>
        /// <param name="creditCardTypeCode"></param>
        /// <param name="voucher"></param>
        /// <param name="documentNumber"></param>
        /// <param name="technicalTransaction"></param>
        /// <param name="branchCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCardVoucher(string creditCardTypeCode, string voucher, string documentNumber,
                                         string technicalTransaction, string branchCode, string startDate,
                                         string endDate, string status)
        {
            if (!String.IsNullOrEmpty(endDate))
            {
                endDate = endDate + " 23:59:59";
            }
            if (String.IsNullOrEmpty(creditCardTypeCode))
            {
                creditCardTypeCode = "-1";
            }
            if (String.IsNullOrEmpty(voucher))
            {
                voucher = "-1";
            }
            if (String.IsNullOrEmpty(documentNumber))
            {
                documentNumber = "-1";
            }
            if (String.IsNullOrEmpty(technicalTransaction))
            {
                technicalTransaction = "-1";
            }
            if (String.IsNullOrEmpty(branchCode))
            {
                branchCode = "-1";
            }

            DateTime dateFrom = startDate == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(startDate);
            DateTime dateTo = endDate == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(endDate);

            if (String.IsNullOrEmpty(status))
            {
                status = "-1";
            }

            List<CardVoucherDTO> cardVoucherDTOs = DelegateService.accountingPaymentService.GetCardVoucher(
                                                      Convert.ToInt32(creditCardTypeCode), voucher,
                                                      Convert.ToInt64(documentNumber),
                                                      Convert.ToInt32(technicalTransaction),
                                                      Convert.ToInt32(branchCode), dateFrom,
                                                      dateTo, Convert.ToInt32(status));

            var cardVouchers = (from CardVoucherDTO cardVoucher in cardVoucherDTOs
                                where cardVoucher.Status == Convert.ToInt32(PaymentStatus.Active)
                                select new
                                {
                                    BranchCode = cardVoucher.BranchCode,
                                    Description = cardVoucher.Description,
                                    CardDescription = cardVoucher.CardDescription,
                                    DocumentNumber = ReplaceWithAsterisks(cardVoucher.DocumentNumber),
                                    Voucher = cardVoucher.Voucher,
                                    CurrencyCode = cardVoucher.CurrencyCode,
                                    CurrencyDescription = cardVoucher.CurrencyDescription,
                                    IncomeAmount = String.Format(new CultureInfo("en-US"), "{0:C}", cardVoucher.IncomeAmount),
                                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", cardVoucher.Amount),
                                    Taxes = String.Format(new CultureInfo("en-US"), "{0:C}", cardVoucher.Taxes),
                                    Retention = String.Format(new CultureInfo("en-US"), "{0:C}", cardVoucher.Retention),
                                    CollectCode = cardVoucher.CollectCode,
                                    TechnicalTransaction = cardVoucher.TechnicalTransaction,
                                    CardDate = cardVoucher.CardDate,
                                    PaymentDate = cardVoucher.PaymentDate.ToString("dd/MM/yyyy"),
                                    Status = cardVoucher.Status,
                                    StatusDescription = cardVoucher.StatusDescription,
                                    CreditCardTypeCode = cardVoucher.CreditCardTypeCode,
                                    PaymentCode = cardVoucher.PaymentCode
                                }).ToList();

            return Json(new { aaData = cardVouchers }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetInformationPayment
        /// Obtiene información de un pago
        /// </summary>
        /// <param name="paymentCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetInformationPayment(int paymentCode)
        {
            List<InformationPaymentDTO> informationPayments = DelegateService.accountingPaymentService.GetInformationPayment(paymentCode);

            return Json(informationPayments, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetRejections
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetRejections()
        {
            try
            {
                List<object> rejectionsResponses = new List<object>();

                List<RejectionDTO> rejections = DelegateService.accountingParameterService.GetRejections();

                foreach (RejectionDTO rejection in rejections)
                {
                    rejectionsResponses.Add(new
                    {
                        Id = rejection.Id,
                        Description = rejection.Description
                    });
                }

                return new UifSelectResult(rejectionsResponses);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetTaxInformationByPaymentId
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTaxInformationByPaymentId(int paymentId)
        {
            List<TaxInformationDTO> taxInformationDTOs = DelegateService.accountingPaymentService.GetTaxInformationByPaimentId(paymentId);

            var taxInformations = (from TaxInformationDTO taxInformation in taxInformationDTOs
                                    select new
                                    {
                                        Description = taxInformation.Description,
                                        PaymentId = taxInformation.PaymentCode,
                                        PaymentTaxCode = taxInformation.PaymentTaxCode,
                                        TaxAmount = taxInformation.TaxAmount,
                                        TaxBase = taxInformation.TaxBase,
                                        TaxCode = taxInformation.TaxCode,
                                        TaxRate = taxInformation.TaxRate
                                  }).ToList();

            return new UifTableResult(taxInformations);
        }

        #endregion

    }
}