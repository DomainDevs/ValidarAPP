//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Grid;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using Sistran.Core.Framework.UIF.Web.Services;

// Sistran Core
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.DTOs;
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;

//Sistran Company
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class PaymentClaimsController : Controller
    {
        #region Constants

        public const int PageSize = 1000;
        public const int PageIndex = 1;
        public const string SortOrder = "ASC";

        #endregion

        #region Views

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Actions

        /// <summary>
        /// SearchClaimsPaymentRequest
        /// Obtiene las solicitudes de pago
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="individualId"></param>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="claimNumber"></param>
        /// <param name="requestNumber"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="paymentSourceId"></param>
        /// <param name="grid"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchClaimsPaymentRequest(int searchBy, int individualId, int prefixId, int branchId,
                                                     string claimNumber, string requestNumber, string dateFrom, string dateTo,
                                                     int paymentSourceId, GridSettings grid)
        {
            SCRDTO.SearchParameterClaimsPaymentRequestDTO searchParameterClaimsPayment = new SCRDTO.SearchParameterClaimsPaymentRequestDTO();

            searchParameterClaimsPayment.SearchBy = searchBy;
            searchParameterClaimsPayment.IndividualId = individualId;
            searchParameterClaimsPayment.Prefix = new PrefixDTO() { Id = prefixId };
            searchParameterClaimsPayment.Branch = new SCRDTO.BranchDTO() { Id = branchId };
            searchParameterClaimsPayment.ClaimNumber = claimNumber;
            searchParameterClaimsPayment.RequestNumber = requestNumber;
            searchParameterClaimsPayment.DateFrom = dateFrom;
            searchParameterClaimsPayment.DateTo = dateTo;
            searchParameterClaimsPayment.PaymentSource = new ConceptSourceDTO() { Id = paymentSourceId };

            List<SCRDTO.PaymentRequestVariousDTO> paymentRequestVariousDTOs =
                DelegateService.accountingApplicationService.SearchClaimsPaymentRequest(searchParameterClaimsPayment, 1).OrderBy(o => o.PaymentRequestNumber).ToList();//Convert.ToInt16(CommonModelPayments.PaymentSources.ClaimsPaymentRequest)


            List<object> paymentRequestClaims = new List<object>();

            foreach (SCRDTO.PaymentRequestClaimDTO paymentRequestClaim in paymentRequestVariousDTOs)
            {
                paymentRequestClaims.Add(new
                {
                    RowNumberUnique = paymentRequestClaim.RowNumberUnique,
                    BranchName = paymentRequestClaim.BranchDescription,
                    BeneficiaryDocumentNumber = paymentRequestClaim.DocumentNumber_Beneficiary,
                    CurrencyName = paymentRequestClaim.CurrencyDescription,
                    TotalAmount = paymentRequestClaim.TotalAmount,
                    Quota = paymentRequestClaim.Quota,
                    RegistrationDate = paymentRequestClaim.RegistrationDate.ToString("dd/MM/yyyy"),
                    EstimatedDate = paymentRequestClaim.EstimatedDate.ToString("dd/MM/yyyy"),
                    ExpirationDateQuota = Convert.ToDateTime(paymentRequestClaim.ExpirationDateQuota).ToString("dd/MM/yyyy"),
                    PaymentRequestCode = paymentRequestClaim.PaymentRequestCode,
                    BranchId = paymentRequestClaim.BranchCode,
                    PrefixId = paymentRequestClaim.PrefixCode,
                    CurrencyId = paymentRequestClaim.CurrencyCode,
                    BusinessTypeId = paymentRequestClaim.BusinessTypeCode,
                    BusinessTypeName = paymentRequestClaim.BusinessTypeDescription,
                    PaymentRequestNumber = paymentRequestClaim.ClaimCode,
                    PayerBeneficiaryDocumentNumber = paymentRequestClaim.DocNumPayBeneficiary,
                    PayerBeneficiaryName = paymentRequestClaim.NamePayBeneficiary,
                    PaymentSourceCode = paymentRequestClaim.PaymentSourceCode,
                    PaymentSourceDescription = paymentRequestClaim.PaymentSourceDescription,
                    PayerBeneficiaryId = paymentRequestClaim.IdPayBeneficiary,
                    ClaimCode = paymentRequestClaim.ClaimCode
                });
            }

            return Json(paymentRequestClaims, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveClaimsPaymentRequestItem
        /// </summary>
        /// <param name="claimsPaymentRequestItem"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveClaimsPaymentRequestItem(ClaimsPaymentRequestItem claimsPaymentRequestItem)
        {
            try
            {
                ClaimsPaymentRequestTransactionItemDTO claimsPaymentRequestTransactionItem = new ClaimsPaymentRequestTransactionItemDTO();

                int quota = 0;
                int paymentRequestNumber = claimsPaymentRequestItem.PaymentRequestNumber;
                decimal exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now.Date, claimsPaymentRequestItem.CurrencyCode).SellAmount;

                ACCDTO.AccountsPayables.ClaimDTO claim = new ACCDTO.AccountsPayables.ClaimDTO()
                {

                    Id = claimsPaymentRequestItem.ClaimCode,
                    CoveredRiskType = Convert.ToInt32(CoveredRiskType.Vehicle)
                };
                ACCDTO.AccountsPayables.PaymentRequestDTO paymentRequest = new ACCDTO.AccountsPayables.PaymentRequestDTO()
                {
                    Claim = claim,
                    Currency = new SCRDTO.CurrencyDTO() { Id = claimsPaymentRequestItem.CurrencyCode },
                    EstimatedDate = claimsPaymentRequestItem.EstimationDate,
                    IndividualId = claimsPaymentRequestItem.BeneficiaryId,
                    MovementType = new MovementTypeDTO()
                    {
                        Id = new PaymentSourceDTO() { Id = claimsPaymentRequestItem.RequestType }.Id
                    },
                    Number = claimsPaymentRequestItem.PaymentRequestCode,
                    RegistrationDate = claimsPaymentRequestItem.RegistrationDate,
                    TotalAmount = new ACCDTO.AmountDTO
                    {
                        Value = Convert.ToDecimal(claimsPaymentRequestItem.IncomeAmount)
                    },
                    Type = new ACCDTO.AccountsPayables.PaymentRequestTypeDTO() { Id = claimsPaymentRequestItem.RequestType },
                };

                if (claimsPaymentRequestItem.RequestType == 3) //Recobro
                {
                    paymentRequest.PaymentDate = claimsPaymentRequestItem.PaymentExpirationDate;
                    paymentRequest.TemporalId = claimsPaymentRequestItem.PaymentNum;
                    paymentRequest.Type = new ACCDTO.AccountsPayables.PaymentRequestTypeDTO() { Id = 3 };
                }
                else if (claimsPaymentRequestItem.RequestType == 2) //Salvamento
                {
                    paymentRequest.PaymentDate = claimsPaymentRequestItem.PaymentExpirationDate;
                    paymentRequest.TemporalId = claimsPaymentRequestItem.PaymentNum;
                    paymentRequest.Type = new ACCDTO.AccountsPayables.PaymentRequestTypeDTO() { Id = 2 };
                }

                quota = claimsPaymentRequestItem.PaymentNum;

                claimsPaymentRequestTransactionItem.PaymentRequest = paymentRequest; //este modelo es de Claims
                claimsPaymentRequestTransactionItem.BussinessType = claimsPaymentRequestItem.BussinessType;
                claimsPaymentRequestTransactionItem.Id = claimsPaymentRequestItem.TempClaimPaymentCode;

                ApplicationDTO applicationResponse = DelegateService.accountingApplicationService.SaveTempClaimPaymentRequestItem(claimsPaymentRequestTransactionItem,
                                                                           claimsPaymentRequestItem.TempImputationCode,
                                                                           exchangeRate);
                var imputation = new
                {
                    Id = applicationResponse.Id,
                    TypeImputation = applicationResponse.ModuleId,
                    PaymentRequestNumber = paymentRequestNumber,
                    Quota = quota
                };

                return Json(new { success = true, result = imputation }, JsonRequestBehavior.AllowGet);
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

        /// <summary>
        /// DeleteClaimsPaymentRequestItem
        /// </summary>
        /// <param name="claimsPaymentRequestId"></param>
        /// <param name="imputationId"></param>
        public void DeleteClaimsPaymentRequestItem(int claimsPaymentRequestId, int imputationId)
        {
            DelegateService.accountingApplicationService.DeleteTempClaimPaymentRequestItem(claimsPaymentRequestId, imputationId, false);
        }

        /// <summary>
        /// GetTempClaimsPaymentRequest
        /// Obtiene las temporales grabadas solicitudes de pago o cobros
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTempClaimsPaymentRequest(int imputationId, bool isPaymentVarious)
        {
            List<object> tempPaymentRequestClaims = new List<object>();

            if (imputationId > 0)
            {

                List<SCRDTO.TempPaymentRequestClaimDTO> tempPaymentRequestClaimDTOs =
                    DelegateService.accountingApplicationService.GetTempClaimsPaymentRequest(
                    imputationId, isPaymentVarious).OrderBy(o => o.TempClaimPaymentCode).ToList();

                int numberUnique = 0;

                foreach (SCRDTO.TempPaymentRequestClaimDTO paymentRequestClaims in tempPaymentRequestClaimDTOs)
                {
                    tempPaymentRequestClaims.Add(new
                    {
                        RowNumberUnique = numberUnique,
                        BranchName = paymentRequestClaims.Branch_Request,
                        BeneficiaryDocumentNumber = paymentRequestClaims.DocumentNumber_Beneficiary,
                        CurrencyName = paymentRequestClaims.CurrencyDescription,
                        IncomeAmount = paymentRequestClaims.IncomeAmount,
                        Amount = paymentRequestClaims.Amount,
                        RegistrationDate = paymentRequestClaims.RegistrationDate.ToString("dd/MM/yyyy"),
                        EstimatedDate = paymentRequestClaims.EstimationDate.ToString("dd/MM/yyyy"),
                        RequestTypeId = paymentRequestClaims.RequestType,
                        PaymentRequestCode = paymentRequestClaims.PaymentRequestCode,
                        BranchId = paymentRequestClaims.BranchCode,
                        PrefixId = paymentRequestClaims.PrefixCode,
                        CurrencyId = paymentRequestClaims.CurrencyCode,
                        BusinessTypeId = paymentRequestClaims.BussinessType,
                        BusinessTypeName = paymentRequestClaims.BusinessTypeDescription,
                        PayerBeneficiaryDocumentNumber = paymentRequestClaims.DocNumPayBeneficiary,
                        PayerBeneficiaryName = paymentRequestClaims.NamePayBeneficiary,
                        TempClaimPaymentCode = paymentRequestClaims.TempClaimPaymentCode,
                        PayerBeneficiaryId = paymentRequestClaims.BeneficiaryId,
                        ClaimCode = paymentRequestClaims.ClaimCode
                    });
                    numberUnique++;
                }
            }


            return Json(tempPaymentRequestClaims, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}