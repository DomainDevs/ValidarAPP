using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class CardDepositSlipSearchController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 1;
        public const string SortOrder = "ASC";

        #endregion
        
        #region Instance Variables

        //readonly IPaymentTicketService _paymentTicketService = ServiceManager.Instance.GetService<IPaymentTicketService>();

        readonly CommonController _commonController = new CommonController();

        #endregion

        #region View

        /// <summary>
        /// MainCardDepositSlipSearch
        /// Pantalla búsqueda de boletas de depósito
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainCardDepositSlipSearch()
        {
            try
            {

                // Setear valor por default de la sucursal de acuerdo al usuario que se conecta 
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
        /// Reemplaza con asteriscos la tarjeta
        /// </summary>
        /// <param name="cellValue"></param>
        /// <returns>string</returns>
        public string ReplaceWithAsterisks(string cellValue)
        {
            int row = 0;
            string valueWithAsterisk = "";

            string firstNumber = cellValue.Substring(0, 1);
            string lastNumber = cellValue.Substring(cellValue.Length - 1);

            for (row = 0; row < cellValue.Length - 2; row++)
            {
                valueWithAsterisk += "*";
            }

            valueWithAsterisk = firstNumber + valueWithAsterisk + lastNumber;
            cellValue = valueWithAsterisk;

            return cellValue;
        }

        /// <summary>
        /// SearchInternalBallotCard
        /// Busca boleta interna de tarjetas
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="paymentTicketId"></param>
        /// <param name="creditCardTypeId"></param>
        /// <param name="branchId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchInternalBallotCard(int? bankId, string startDate, string endDate,
                                                   int? paymentTicketId, int? creditCardTypeId, int? branchId)
        {
            List<object> searchInternalBallotCards = new List<object>();

            List<SearchInternalBallotCardDTO> searchInternalBallotCardDTOs = 
            DelegateService.accountingPaymentTicketService.SearchInternalBallotCard((bankId.HasValue) ? bankId.Value : -1, 
                                                          startDate, endDate, (paymentTicketId.HasValue) ? paymentTicketId.Value : -1,
                                                          (creditCardTypeId.HasValue) ? creditCardTypeId.Value : -1,
                                                          (branchId.HasValue) ? branchId.Value : -1);

            string statusDescription = "";

            foreach (SearchInternalBallotCardDTO searchInternalBallotCard in searchInternalBallotCardDTOs)
            {
                switch (searchInternalBallotCard.Status)
                {
                    case 0:
                        statusDescription = @Global.Annulled;
                        break;
                    case 1:
                        statusDescription = @Global.Active;
                        break;
                    case 3:
                        statusDescription = @Global.StatusDeposited;
                        break;
                    default:
                        statusDescription = @Global.Active;
                        break;
                }

                searchInternalBallotCards.Add(new
                {
                    BranchDescription = searchInternalBallotCard.BranchDescription,
                    PaymentTicketCode = searchInternalBallotCard.PaymentTicketCode,
                    BankDescription = searchInternalBallotCard.BankDescription,
                    AccountNumber = searchInternalBallotCard.AccountNumber,
                    CurrencyDescription = searchInternalBallotCard.CurrencyDescription,
                    CreditCardDescription = searchInternalBallotCard.CreditCardDescription,
                    PaymentMethodTypeCode = searchInternalBallotCard.PaymentMethodTypeCode,
                    PaymentMethodTypeDescription = searchInternalBallotCard.PaymentMethodTypeDescription,
                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", searchInternalBallotCard.Amount),
                    Status = searchInternalBallotCard.Status,
                    StatusDescription = statusDescription,
                    RegisterDate = searchInternalBallotCard.RegisterDate,
                    PaymentBallotNumber = searchInternalBallotCard.PaymentBallotNumber,
                    DepositDate = searchInternalBallotCard.DepositDate
                });

            }

            return Json(new { aaData = searchInternalBallotCards, total = searchInternalBallotCards.Count }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// CancelInternalBallot
        /// Cancela una boleta interna enviando como parámetro el paymetTicketCode
        /// </summary>
        /// <param name="paymetTicketCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CancelInternalBallot(int paymetTicketCode)
        {
            // VIENE DE TARJETA
            List<object> paymentTickets = new List<object>();

            Core.Application.AccountingServices.DTOs.PaymentTicketDTO paymentTicket = new Core.Application.AccountingServices.DTOs.PaymentTicketDTO();
            paymentTicket.Id = paymetTicketCode;
            paymentTickets.Add(new
            {
                id = DelegateService.accountingPaymentTicketService.CancelInternalBallot(paymentTicket, _commonController.GetUserIdByName(User.Identity.Name))
            });
            return Json(paymentTickets, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetDetailCards
        /// Devuelve el detalle de una boleta interna de tarjetas enviando como parámetro el paymentTicketId
        /// </summary>
        /// <param name="paymentTicketId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDetailCards(int? paymentTicketId)
        {
            var detailCards = new object();

            if (paymentTicketId.HasValue)
            {
                List<DetailCardDTO> detailCardDTOs = 
                DelegateService.accountingPaymentTicketService.GetDetailCards(paymentTicketId.Value);

                detailCards = (from DetailCardDTO DetailCard in detailCardDTOs
                                select new
                                {
                                    CreditCardDescription = DetailCard.CreditCardDescription,
                                    BankDescription = DetailCard.BankDescription,
                                    DocumentNumber = ReplaceWithAsterisks(DetailCard.DocumentNumber),
                                    CurrencyDescription = DetailCard.CurrencyDescription,
                                    Amount = String.Format(new CultureInfo("en-US"), "{0:C}", DetailCard.Amount),
                                    Tax = String.Format(new CultureInfo("en-US"), "{0:C}", DetailCard.Tax),
                                    Voucher = DetailCard.Voucher,
                                    Holder = DetailCard.Holder,
                                    BillCode = DetailCard.CollectCode
                                }).ToList();
            }

            return Json(new { aaData = detailCards }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}