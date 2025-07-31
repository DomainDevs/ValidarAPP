using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;
// Sistran Core
using DTOs=Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class DepositSlipSearchController : Controller
    {
        #region Instance Variables
        readonly CommonController _commonController = new CommonController();
        #endregion

        #region MainBallotSearch

        /// <summary>
        /// DepositSlipSearch
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult DepositSlipSearch()
        {
            try
            {
                ViewBag.ParamPaymentMethodCash = ConfigurationManager.AppSettings["ParamPaymentMethodCash"];
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion

        #region Search

        /// <summary>
        /// SearchInternalBallotCard
        /// Realiza la búsqueda de Boletas Internas
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="paymentTicketId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SearchInternalBallot(int bankId, string startDate, string endDate, int paymentTicketId)
        {
            List<object> searchInternalBallots = new List<object>();

            if (endDate != null && endDate != "")
            {
                endDate = endDate + " 23:59:59";
            }

            List<DTOs.Search.SearchInternalBallotDTO> searchInternalBallotDTOs =
            DelegateService.accountingPaymentTicketService.DepositSlipSearch(bankId, startDate, endDate, paymentTicketId).OrderBy(o => o.PaymentTicketCode).ToList();

            if (searchInternalBallotDTOs.Count > 0)
            {
                foreach (DTOs.Search.SearchInternalBallotDTO searchInternalBallot in searchInternalBallotDTOs)
                {
                    string statusDescription = "";

                    if (searchInternalBallot.Status == 1)
                    {
                        statusDescription = @Global.Active;
                    }
                    if (searchInternalBallot.Status == 0)
                    {
                        statusDescription = @Global.Annulled;
                    }

                    if (searchInternalBallot.Status == 3)
                    {
                        statusDescription = @Global.StatusDeposited;
                    }

                    searchInternalBallots.Add(new
                    {
                        PaymentTicketCode = searchInternalBallot.PaymentTicketCode,
                        BankCode = searchInternalBallot.BankCode,
                        BankDescription = searchInternalBallot.BankDescription,
                        AccountNumber = searchInternalBallot.AccountNumber,
                        CurrencyId = searchInternalBallot.CurrencyId,
                        CurrencyDescription = searchInternalBallot.CurrencyDescription,
                        CashAmount = String.Format(new CultureInfo("en-US"), "{0:C}", searchInternalBallot.CashAmount),
                        Amount = String.Format(new CultureInfo("en-US"), "{0:C}", searchInternalBallot.Amount),
                        Status = searchInternalBallot.Status,
                        StatusDescription = statusDescription,
                        RegisterDate = searchInternalBallot.RegisterDate,
                        UserId = searchInternalBallot.UserId,
                        User = searchInternalBallot.User,
                        PaymentBallotNumber = searchInternalBallot.PaymentBallotNumber,
                        DespositDate = searchInternalBallot.DespositDate
                    });
                }
            }

            return new UifTableResult(searchInternalBallots);
        }

        /// <summary>
        /// GetDetailChecks
        /// Devuelve el detalle de una boleta interna de cheques enviando como parámetro el paymentTicketId
        /// </summary>
        /// <param name="paymentTicketId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetDetailChecks(int paymentTicketId)
        {
            List<object> detailChecks = new List<object>();

            List<DTOs.Search.DetailCheckDTO> detailCheckDTOs =
            DelegateService.accountingPaymentTicketService.GetDetailChecks(paymentTicketId).OrderBy(o => o.BankDescription).ToList();

            if (detailCheckDTOs.Count > 0)
            {
                foreach (DTOs.Search.DetailCheckDTO detailCheck in detailCheckDTOs)
                {
                    detailChecks.Add(new
                    {
                        BankDescription = detailCheck.BankDescription,
                        IssuingAccountNumber = detailCheck.IssuingAccountNumber,
                        DocumentNumber = detailCheck.DocumentNumber,
                        BillCode = detailCheck.CollectCode,
                        CurrencyDescription = detailCheck.CurrencyDescription,
                        Amount = String.Format(new CultureInfo("en-US"), "{0:C}", detailCheck.Amount),
                        DatePayment = Convert.ToDateTime(detailCheck.DatePayment).ToString("dd/MM/yyyy"),
                        Holder = detailCheck.Holder
                    });
                }
            }

            return new UifTableResult(detailChecks);
        }

        /// <summary>
        /// CancelInternalBallot
        /// Permite anular la boleta interna de tarjetas, cambia estado de boleta a "anulada", actualiza la fecha de anulación y libera los vouchers 
        /// de tarjetas asociadas
        /// </summary>
        /// <param name="paymetTicketCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult CancelInternalBallot(int paymetTicketCode)
        {
            List<object> paymentTickets = new List<object>();

            DTOs.PaymentTicketDTO paymentTicket = new DTOs.PaymentTicketDTO();
            paymentTicket.Id = paymetTicketCode;
            paymentTicket.PaymentMethod = (int)PaymentMethods.CurrentCheck;

            paymentTickets.Add(new
            {
                id = DelegateService.accountingPaymentTicketService.CancelInternalBallot(paymentTicket,
                                 _commonController.GetUserIdByName(User.Identity.Name))
            });
            return Json(paymentTickets, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidateExternalBallotDeposited
        /// Revisa si una boleta esta depositada en boleta externa enviando como parámetro el paymentTicketCode
        /// </summary>
        /// <param name="paymentTicketCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateExternalBallotDeposited(int paymentTicketCode)
        {
            bool existsExternalBallotDeposited = DelegateService.accountingPaymentBallotService.ValidateExternalBallotDeposited(paymentTicketCode);
            List<object> existsExternalBallots = new List<object>();

            existsExternalBallots.Add(new
            {
                resp = existsExternalBallotDeposited
            });
            return Json(existsExternalBallots, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PaymentMethodType

        /// <summary>
        /// GetEnabledPaymentMethodTypes
        /// Obtiene los tipos de pago habilitados para la caja
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetEnabledPaymentMethodTypes()
        {
            List<DTOs.Payments.PaymentMethodDTO> paymentMethods = new List<DTOs.Payments.PaymentMethodDTO>();

            List<DTOs.Search.PaymentMethodTypeDTO> paymentMethodTypeDTOs = DelegateService.accountingParameterService.GetEnablePaymentMethodType(false, false, true);

            if (paymentMethodTypeDTOs.Count != 0)
            {
                foreach (DTOs.Search.PaymentMethodTypeDTO paymentMethodType in paymentMethodTypeDTOs)
                {
                    DTOs.Payments.PaymentMethodDTO paymentMethod = new DTOs.Payments.PaymentMethodDTO();

                    paymentMethod.Id = paymentMethodType.PaymentTypeCode;
                    paymentMethod.Description = paymentMethodType.Description;

                    paymentMethods.Add(paymentMethod);
                }
            }

            return new UifSelectResult(paymentMethods);
        }

        #endregion
    }
}