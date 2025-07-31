//System
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;

// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.Imputation;
using Sistran.Core.Framework.UIF.Web.Resources;

// Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Framework.UIF.Web.Services;
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCDTO = Sistran.Core.Application.AccountingServices.DTOs;

//Sistran Company
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class PaymentVariousController : Controller
    {
        #region Constants

        public const string SortOrder = "ASC";

        #endregion        

        #region Views

        /// <summary>
        /// Index
        /// Obtiene las solicitudes de pago
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion
        #region Instancia de Variables
        readonly CommonController _commonController = new CommonController();
        #endregion


        #region Actions

        /// <summary>
        /// SearchPaymentRequestVarious
        /// Obtiene las solicitudes de pago
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="branchId"></param>
        /// <param name="individualId"></param>
        /// <param name="requestNumber"></param>
        /// <param name="voucherNumber"></param>
        ///  <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SearchPaymentRequestVarious(int searchBy, int individualId, int branchId, string requestNumber,
                                                      string voucherNumber, string dateFrom, string dateTo)   {
            

            List<object> paymentRequestVarious = new List<object>();
            SCRDTO.SearchParameterClaimsPaymentRequestDTO searchParameterClaimsPayments = new SCRDTO.SearchParameterClaimsPaymentRequestDTO();
                        
            try
            {            
                searchParameterClaimsPayments.SearchBy = searchBy;
                searchParameterClaimsPayments.IndividualId = individualId;
                searchParameterClaimsPayments.Branch = new SCRDTO.BranchDTO() { Id = branchId };
                searchParameterClaimsPayments.RequestNumber = requestNumber;
                searchParameterClaimsPayments.VoucherNumber = voucherNumber;
                searchParameterClaimsPayments.DateFrom = dateFrom;
                searchParameterClaimsPayments.DateTo = dateTo;

                List<SCRDTO.PaymentRequestVariousDTO> paymentsRequestVariousDTOs =
                DelegateService.accountingApplicationService.SearchClaimsPaymentRequest(searchParameterClaimsPayments, 4).OrderBy(o => o.PaymentRequestCode).ToList();

                foreach (SCRDTO.PaymentRequestVariousDTO paymentRequestVariousDTO in paymentsRequestVariousDTOs)
                {
                    paymentRequestVarious.Add(new
                    {
                        RowNumberUnique = paymentRequestVariousDTO.RowNumberUnique,
                        BranchName = paymentRequestVariousDTO.Branch_Request,
                        BeneficiaryDocumentNumber = paymentRequestVariousDTO.DocumentNumber_Beneficiary,
                        CurrencyName = paymentRequestVariousDTO.CurrencyDescription,                      
                        TotalAmount = String.Format(new CultureInfo("en-US"), "{0:C}", paymentRequestVariousDTO.TotalAmount),
                        RegistrationDate = paymentRequestVariousDTO.RegistrationDate.ToString("dd/MM/yyyy"),
                        EstimatedDate = paymentRequestVariousDTO.EstimatedDate.ToString("dd/MM/yyyy"),
                        PaymentRequestCode = paymentRequestVariousDTO.PaymentRequestCode,
                        BranchId = paymentRequestVariousDTO.BranchCode,
                        PrefixId = paymentRequestVariousDTO.PrefixCode,
                        CurrencyId = paymentRequestVariousDTO.CurrencyCode,
                        BusinessTypeId = paymentRequestVariousDTO.BusinessTypeCode,
                        BusinessTypeName = paymentRequestVariousDTO.BusinessTypeDescription,
                        PaymentRequestNumber = paymentRequestVariousDTO.PaymentRequestNumber,
                        PayerBeneficiaryDocumentNumber = paymentRequestVariousDTO.DocNumPayBeneficiary,
                        PayerBeneficiaryName = paymentRequestVariousDTO.NamePayBeneficiary,
                        PaymentSourceCode = paymentRequestVariousDTO.PaymentSourceCode,
                        PayerBeneficiaryId = paymentRequestVariousDTO.IdPayBeneficiary,
                        ClaimCode = paymentRequestVariousDTO.ClaimCode
                    });
                }

                return Json(new { success = true, result = paymentRequestVarious }, JsonRequestBehavior.AllowGet);
                
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
        /// Llama a función de borrado de autorizaciones varias con nuevos parámetros.
        /// </summary>
        /// <param name="claimsPaymentRequestId"></param>
        /// <param name="imputationId"></param>
        public void DeleteClaimsPaymentRequestItem(int claimsPaymentRequestId, int imputationId)
        {
            DelegateService.accountingApplicationService.DeleteTempClaimPaymentRequestItem(claimsPaymentRequestId, imputationId, true);
        }

        /// <summary>
        /// DeletePaymentRequestItem
        /// Llama a función de borrado de solicitudes pagos varios en temporales de imputación.
        /// </summary>
        /// <param name="claimsPaymentRequestId"></param>
        /// <param name="imputationId"></param>
        public void DeletePaymentRequestItem(int paymentRequestId, int imputationId)
        {
            DelegateService.accountingApplicationService.DeleteTempPaymentRequestItem(paymentRequestId, imputationId);
        }

        /// <summary>
        /// SaveClaimsPaymentRequestItem
        /// </summary>
        /// <param name="claimsPaymentRequestItem"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SavePaymentRequestItem(PaymentRequestItem paymentRequestItem)
        {
            try
            {
                int quota = 0;
                decimal exchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now.Date, paymentRequestItem.CurrencyCode).BuyAmount;

                PaymentRequestTransactionItemDTO paymentRequestTransactionItem = new PaymentRequestTransactionItemDTO()
                {
                    Id = paymentRequestItem.TempPaymentCode,
                    PaymentRequest = new PaymentRequestDTO()
                    {
                        AccountingDate = DateTime.Now,
                        Beneficiary = new ACCDTO.IndividualDTO() { IndividualId = paymentRequestItem.BeneficiaryId },
                        Branch = new SCRDTO.BranchDTO(),
                        Company = new ACCDTO.CompanyDTO(),
                        Currency = new SCRDTO.CurrencyDTO() { Id = paymentRequestItem.CurrencyCode },
                        Description = "",
                        EstimatedDate = paymentRequestItem.EstimationDate,
                        Id = paymentRequestItem.PaymentRequestCode,
                        MovementType = null,
                        PaymentMethod = null,
                        PaymentRequestNumber = new PaymentRequestNumberDTO() { Number = paymentRequestItem.PaymentRequestNumber },
                        PaymentRequestType = Convert.ToInt32(PaymentRequestTypes.Payment),
                        PersonType = null,
                        RegisterDate = paymentRequestItem.RegistrationDate,
                        SalePoint = null,
                        TotalAmount = new ACCDTO.AmountDTO()
                        {
                            Currency = new SCRDTO.CurrencyDTO() { Id = paymentRequestItem.CurrencyCode },
                            Value = paymentRequestItem.IncomeAmount
                        },
                        ExchangeRate = new ACCDTO.ExchangeRateDTO() { BuyAmount = exchangeRate },
                        LocalAmount = new ACCDTO.AmountDTO() { Value = paymentRequestItem.Amount },
                        Transaction = null,
                        UserId = _commonController.GetUserIdByName(User.Identity.Name),
                        Vouchers = null
                    }
                };

                quota = paymentRequestItem.PaymentNumber;

                ApplicationDTO applicationResponse = DelegateService.accountingApplicationService.SaveTempApplicationPremiumItem(paymentRequestTransactionItem, paymentRequestItem.TempImputationCode, exchangeRate);

                var imputation = new
                {
                    Id = applicationResponse.Id,
                    TypeImputation = applicationResponse.ModuleId,
                    PaymentRequestNumber = paymentRequestItem.PaymentRequestNumber,
                    Quota = quota
                };
                return Json(imputation, JsonRequestBehavior.AllowGet);
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
        /// GetTempPaymentRequestItems
        /// Obtiene las temporales grabadas solicitudes de pago o cobros
        /// </summary>
        /// <param name="imputationId"></param>
        /// <param name="isPaymentVarious"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GetTempPaymentRequestItems(int imputationId, bool isPaymentVarious)
        {
            List<object> tempPaymentRequestItems = new List<object>();

            if (imputationId > 0)
            {
                List<SCRDTO.TempPaymentRequestClaimDTO> tempPaymentRequestClaimDTOs =
                    DelegateService.accountingApplicationService.GetTempPremiumAmountByApplicationId(imputationId).OrderBy(o => o.TempClaimPaymentCode).ToList();

                int numberUnique = 0;

                foreach (SCRDTO.TempPaymentRequestClaimDTO paymentRequestClaims in tempPaymentRequestClaimDTOs)
                {
                    tempPaymentRequestItems.Add(new
                    {
                        RowNumberUnique = numberUnique,
                        BranchName = paymentRequestClaims.Branch_Request,
                        Branch = paymentRequestClaims.BranchDescription,
                        BeneficiaryDocumentNumber = paymentRequestClaims.DocumentNumber_Beneficiary,
                        CurrencyName = paymentRequestClaims.CurrencyDescription,
                        IncomeAmount = String.Format(new CultureInfo("en-US"), "{0:C}", paymentRequestClaims.IncomeAmount),                  
                        Amount = String.Format(new CultureInfo("en-US"), "{0:C}", paymentRequestClaims.Amount),
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
                        ClaimCode = paymentRequestClaims.ClaimCode,
                        RegisterDateLabel = Global.RegisterDate + ": ",
                        EstimatedDateLabel = Global.EstimatedDate + ": ",
                        PaymentRequestNumber = Global.RequestNumber + ": " + paymentRequestClaims.PaymentRequestNumber
                    });
                    numberUnique++;
                }
            }

            return Json(tempPaymentRequestItems, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}