//Sytem
using System.Web.Mvc;
using System.Collections.Generic;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Services;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class LegalPaymentController : Controller
    {
        #region Instance Variables
                
        readonly CommonController _commonController = new CommonController();

        #endregion Interfaz

        #region View

        /// <summary>
        /// GetRejectedPaymentId
        /// Obtiene el paymentId del cheque rechazado en base al banco y número de cheque
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRejectedPaymentId(int bankId, string documentNumber)
        {
            List<RejectedPaymentDTO> rejectedPayments = DelegateService.accountingPaymentService.GetRejectedPaymentByBankIdAndDocumentNumber(bankId, documentNumber);

            return Json(rejectedPayments, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetPaymentBallotInfoByPaymentId
        /// Obtiene información de la boleta de depósito dado el id del pago
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPaymentBallotInfoByPaymentId(int paymentId)
        {
            PaymentBallotDTO paymentBallot = DelegateService.accountingPaymentService.GetPaymentBallotInfoByPaymentId(paymentId);

            return Json(paymentBallot, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveLegalPaymentRequest
        /// Inserta un registro en la tabla ACC.LEGAL_PAYMENT
        /// </summary>
        /// <param name="frmLegalPayment"></param>
        /// <param name="payerId"></param>
        /// <param name="descriptionLegalize"></param>
        /// <returns>int</returns>
        public JsonResult SaveLegalPaymentRequest(Models.LegalPayment.LegalPaymentModel frmLegalPayment, int payerId, string descriptionLegalize, int branchId)
        {           

            LegalPaymentDTO legalPayments = new LegalPaymentDTO();
            int UserId = _commonController.GetUserIdByName(User.Identity.Name);
            int status = Convert.ToInt32(Core.Application.AccountingServices.Enums.CollectControlStatus.Open);
            DateTime accountingDate;

            legalPayments.LegalPaymentId = frmLegalPayment.LegalPaymentId;
            legalPayments.RejectedPaymentId = frmLegalPayment.RejectedPaymentId;
            legalPayments.LegalDate = frmLegalPayment.LegalDate;
            legalPayments.PaymentId = frmLegalPayment.PaymentId;
            legalPayments.CollectId = frmLegalPayment.BillId;
            legalPayments.DatePayment = frmLegalPayment.DatePayment;
            legalPayments.DocumentNumber = frmLegalPayment.DocumentNumber;
            legalPayments.IssuerName = frmLegalPayment.IssuerName;
            legalPayments.IssuingBankId = frmLegalPayment.IssuingBankId;
            legalPayments.IssuingAccountNumber = frmLegalPayment.IssuingAccountNumber;
            legalPayments.UserId = _commonController.GetUserIdByName(User.Identity.Name);

            Core.Application.AccountingServices.DTOs.CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(UserId, Convert.ToInt32(branchId), Convert.ToDateTime(DateTime.Now).Date, status);
            accountingDate = collectControl != null ? collectControl.AccountingDate : DateTime.MinValue;

            Core.Application.AccountingServices.DTOs.MessageSuccessDTO saveCheckingLegalResult = DelegateService.accountingPaymentService.SaveLegalPaymentRequest(legalPayments, payerId, descriptionLegalize, branchId, UserId,accountingDate);

            var saveRegularization = new
            {
                Id = saveCheckingLegalResult.BillId,
                TechnicalTransaction = Convert.ToString(saveCheckingLegalResult.TechnicalTransaction),
                ShowMessage = Convert.ToString(saveCheckingLegalResult.ShowMessage),
                Message = saveCheckingLegalResult.ImputationMessage,
                saveCheckingLegalResult.GeneralLedgerSuccess
            };

            return Json(saveRegularization, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}