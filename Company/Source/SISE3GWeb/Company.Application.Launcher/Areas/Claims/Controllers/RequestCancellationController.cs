using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class RequestCancellationController : Controller
    {
        public ActionResult RequestCancellation()
        {
            return View();
        }

        public UifJsonResult GetPaymentChargeRequestByPrefixIdBranchIdNumber(int prefixId,int branchId,int number)
        {
            try
            {
                PaymentRequestDTO paymentRequestDTO = DelegateService.paymentRequestApplicationService.GetPaymentRequestByPrefixIdBranchIdNumber(prefixId, branchId, number);

                if (paymentRequestDTO != null)
                {
                    return new UifJsonResult(true, paymentRequestDTO);
                }

                ChargeRequestDTO chargeRequestDTO = DelegateService.paymentRequestApplicationService.GetChargeRequestByPrefixIdBranchIdNumber(prefixId, branchId, number);

                return new UifJsonResult(true, chargeRequestDTO);

            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRequest);
            }
        }

        public UifJsonResult GetPaymentRequestClaimsByPaymentRequestId(int paymentRequestId)
        {
            try
            {
                List<PaymentRequestDTO> paymentRequestDTO = DelegateService.paymentRequestApplicationService.GetPaymentRequestClaimsByPaymentRequestId(paymentRequestId);
                return new UifJsonResult(true, paymentRequestDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentRequestClaimsByPaymentRequestId);
            }
        }

        public ActionResult SaveRequestCancellation(int paymentRequestId, bool isChargeRequest)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.SaveRequestCancellation(paymentRequestId, isChargeRequest));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveRequestCancellation);
            }
        }
    }
}