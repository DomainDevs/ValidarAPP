using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using static Sistran.Core.Framework.UIF.Web.Helpers.FilterConfigHelper;

// Sistran Core
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class CheckingRejectionController : Controller
    {
        #region Instance Variables

        readonly CommonController _commonController = new CommonController();
        readonly BillingController _billingController = new BillingController();

        #endregion
        
        #region CheckingRejection

        /// <summary>
        /// SaveCheckingRejection
        /// Realiza una grabación de rechazo
        /// </summary>
        /// <param name="checkingRejectionModel"></param>
        /// <param name="billId"></param>
        /// <param name="payerId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCheckingRejection(Models.CheckingRejection.CheckingRejectionModel checkingRejectionModel, int billId, int payerId, int branchId)
        {
            string message = "";
            CollectDTO newCollect = new CollectDTO();
            CollectDTO collect = new CollectDTO();
            PaymentDTO payment = new PaymentDTO();
            //Flag para mostrar mensaje de contabilidad en EE
            bool showMessage = true;

            Core.Application.AccountingServices.DTOs.RejectedPaymentDTO rejectedPayment = new Core.Application.AccountingServices.DTOs.RejectedPaymentDTO();
            if (checkingRejectionModel != null)
            {
                rejectedPayment.Id = checkingRejectionModel.Id;
                rejectedPayment.Rejection = new RejectionDTO();
                rejectedPayment.Rejection.Id = checkingRejectionModel.RejectionId;
                rejectedPayment.Date = Convert.ToDateTime(checkingRejectionModel.RejectionDate);
                rejectedPayment.Payment = new PaymentDTO();
                rejectedPayment.Payment.Id = checkingRejectionModel.PaymentId;
                rejectedPayment.Commission = new AmountDTO();
                rejectedPayment.Commission.Value = checkingRejectionModel.Commission;
                rejectedPayment.TaxCommission = new AmountDTO();
                rejectedPayment.TaxCommission.Value = checkingRejectionModel.TaxCommission;
                rejectedPayment.Description = checkingRejectionModel.Description;
                payment.Id = checkingRejectionModel.PaymentId;
            }

            payment = DelegateService.accountingPaymentService.GetPayment(payment);

            //Tarjeta
            if (payment.PaymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"]) ||
               payment.PaymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"]))
            {
                newCollect.Id = DelegateService.accountingPaymentService.SaveRejectedPayment(rejectedPayment,
                                                                    _commonController.GetUserIdByName(User.Identity.Name),
                                                                    DateTime.Now, billId, payerId,
                                                                    @Global.VoucherRejection, branchId);

                collect.Id = newCollect.Id;
                collect = DelegateService.accountingCollectService.GetCollect(collect);

                #region Accounting

                //disparo la contabilización del movimiento
                if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                {
                    message = _billingController.RecordBill(newCollect.Id, 4, _commonController.GetUserIdByName(User.Identity.Name), collect.Transaction.TechnicalTransaction);
                }
                else
                {
                    message = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                    showMessage = false;
                }

                #endregion Accounting

                var saveCheckingRejectionResponse = new
                {
                    Message = message,
                    BillId = Convert.ToString(newCollect.Id),
                    ShowMessage = Convert.ToString(showMessage),
                    TechnicalTransaction = Convert.ToString(collect.Transaction.TechnicalTransaction)
                };

                return Json(saveCheckingRejectionResponse, JsonRequestBehavior.AllowGet);

            }
            //Cheque
            if (payment.PaymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"]) ||
                payment.PaymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"]))

            {
                //Grabación
                newCollect.Id = DelegateService.accountingPaymentService.SaveRejectedPayment(rejectedPayment,
                                                                _commonController.GetUserIdByName(User.Identity.Name),
                                                                DateTime.Now, billId, payerId,
                                                                @Global.ForCheckRejection, branchId);

                collect.Id = newCollect.Id;

                collect = DelegateService.accountingCollectService.GetCollect(collect);

                #region Accounting

                //disparo la contabilización del movimiento
                if (ConfigurationManager.AppSettings["EnabledGeneralLedger"] == "true")
                {
                    message = _billingController.RecordBill(newCollect.Id, 2, _commonController.GetUserIdByName(User.Identity.Name),collect.Transaction.TechnicalTransaction);
                }
                else
                {
                    message = Convert.ToString(@Global.IntegrationServiceDisabledLabel);
                    showMessage = false;
                }

                #endregion Accounting

                var saveCheckingRejectionResponse = new
                {
                    Message = "test",
                    BillId = Convert.ToString(newCollect.Id),
                    ShowMessage = Convert.ToString(showMessage),
                    TechnicalTransaction = Convert.ToString(collect.Transaction.TechnicalTransaction)
                };

                return Json(saveCheckingRejectionResponse, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SaveCheckingRejection
        /// Realiza una grabación de rechazo
        /// </summary>
        /// <param name="checkingRejectionModel"></param>
        /// <param name="billId"></param>
        /// <param name="payerId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveCheckingRejectionNew(Models.CheckingRejection.CheckingRejectionModel checkingRejectionModel, int billId, int payerId, int branchId)
        {
            PaymentDTO payment = new PaymentDTO();
            //Flag para mostrar mensaje de contabilidad en EE
            int UserId = _commonController.GetUserIdByName(User.Identity.Name);
            int status = Convert.ToInt32(CollectControlStatus.Open);
            DateTime accountingDate;

            Core.Application.AccountingServices.DTOs.RejectedPaymentDTO rejectedPayment = new Core.Application.AccountingServices.DTOs.RejectedPaymentDTO();
            if (checkingRejectionModel != null)
            {
                rejectedPayment.Id = checkingRejectionModel.Id;
                rejectedPayment.Rejection = new RejectionDTO();
                rejectedPayment.Rejection.Id = checkingRejectionModel.RejectionId;
                rejectedPayment.Date = Convert.ToDateTime(checkingRejectionModel.RejectionDate);
                rejectedPayment.Payment = new PaymentDTO();
                rejectedPayment.Payment.Id = checkingRejectionModel.PaymentId;
                rejectedPayment.Commission = new AmountDTO();
                rejectedPayment.Commission.Value = checkingRejectionModel.Commission;
                rejectedPayment.TaxCommission = new AmountDTO();
                rejectedPayment.TaxCommission.Value = checkingRejectionModel.TaxCommission;
                rejectedPayment.Description = checkingRejectionModel.Description;
                payment.Id = checkingRejectionModel.PaymentId;
            }

            payment = DelegateService.accountingPaymentService.GetPayment(payment);

            CollectControlDTO collectControl = DelegateService.accountingCollectControlService.NeedCloseCollect(UserId, Convert.ToInt32(branchId), Convert.ToDateTime(DateTime.Now).Date, status);
            accountingDate = collectControl != null ? collectControl.AccountingDate : DateTime.MinValue;

            //Grabación
            var saveCheckingRejectionResponse = DelegateService.accountingPaymentService.SaveCheckingRejection(rejectedPayment,
                                                             billId, payerId, branchId,
                                                             UserId,
                                                             @Global.VoucherRejection,
                                                             @Global.ForCheckRejection, accountingDate);
            return Json(saveCheckingRejectionResponse, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// GetUser
        /// Función que devuelve el nombre del usuario que esta logueado
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetUser()
        {
            List<object> users = new List<object>();
            users.Add(new
            {
                user = User.Identity.Name.ToUpper()
            });
            return Json(users, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Payment
        
        /// <summary>
        /// GetCheckInformation
        /// Devuelve información del cheque enviando como parámetros el bankid y el documentNumber
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="documentNumber"></param>
        /// <param name="paymentCode"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCheckInformation(int bankId, string documentNumber, string paymentCode)
        {
            bool existsPaymentByBank = DelegateService.accountingPaymentService.GetPaymentByBankIdAndDocumentNumber(bankId, documentNumber);
            List<CheckInformationDTO> informationChecks = new List<CheckInformationDTO>();
            if (existsPaymentByBank)
            {
                informationChecks = DelegateService.accountingPaymentService.GetCheckInformationByDocumentNumber(paymentCode);
            }
            return Json(informationChecks, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region BillItem

        /// <summary>
        /// GetPoliciesByBillId
        /// Devuelve la lista de polizas enviando como parámetro el billId
        /// </summary>
        /// <param name="billId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPoliciesByBillId(string billId)
        {

            List<CollectItemPolicyDTO> collectItemPolicies =
                DelegateService.accountingCollectService.GetPoliciesByCollectId(Convert.ToInt32(billId)).OrderBy(o => o.QuoteNum).ToList();

            var collectItemPolicyResponse = new
            {
                page = collectItemPolicies,
                total = collectItemPolicies.Count,
                records = collectItemPolicies,
                rows = collectItemPolicies
            };
            return new UifTableResult(collectItemPolicyResponse.records);
        }

        #endregion

    }
}