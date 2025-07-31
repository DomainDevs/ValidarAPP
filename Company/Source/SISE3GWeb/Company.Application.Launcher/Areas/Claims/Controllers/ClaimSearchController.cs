using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class ClaimSearchController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        public JsonResult SearchClaims(SearchClaimDTO searchClaimDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.SearchClaims(searchClaimDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchClaims);
            }
        }

        public JsonResult GetClaimPrefixCoveredRiskTypeByPrefixCode(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimPrefixCoveredRiskTypeByPrefixCode(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimPrefixCoveredRiskTypeByPrefixCode);
            }
        }

        public JsonResult SearchNotices(SearchNoticeDTO SearchClaimNoticeDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.SearchNotices(SearchClaimNoticeDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchNotices);
            }
        }

        public JsonResult SearchPolicy(PolicyDTO policyDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimPoliciesByPolicy(policyDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicy);
            }
        }

        public JsonResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetPrefixes().OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        public JsonResult GetBranchesByUserId()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetBranchesByUserId(SessionHelper.GetUserId()).OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchesByUserId);
            }
        }

        public JsonResult GetHoldersByName(string query)
        {
            try
            {
               return Json(DelegateService.claimApplicationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetHoldersByName, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetHoldersByIndividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(individualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetHoldersByName);
            }
        }

        public JsonResult ObjectNotice(NoticeDTO notice)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.ObjectNotice(notice));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorObjectNotice);
            }
        }

        public JsonResult GetRiskLocationByAddress(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetRiskPropertiesByAddress(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetRiskLocationByAddress, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetRiskByPlate(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetRisksVehicleByLicensePlate(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetRiskByPlate, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetSuretiesByDescription(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetRisksBySurety(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetSuretiesByDescription, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetInsuredsByDescriptionIndividualSearchType(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetInsuredsByDescriptionIndividualSearchType, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetSeachTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSearchTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSeachTypes);
            }
        }

        public UifJsonResult SearchPaymentRequests(PaymentRequestDTO paymentRequestDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.SearchPaymentRequests(paymentRequestDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPaymentRequests);
            }
        }

        public UifJsonResult SearchChargeRequests(ChargeRequestDTO chargeRequestDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.SearchChargeRequests(chargeRequestDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchChargeRequests);
            }
        }

        public JsonResult GetInsuredsByName(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetHoldersByName, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetInsuredsByIndividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(individualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetHoldersByName);
            }
        }

        public JsonResult GetParticipantsByName(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetParticipantsByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetHoldersByName, JsonRequestBehavior.DenyGet);
            }
        }
        
        public JsonResult GetThirdsByName(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetThirdPartyByDescriptionInsuredSearchType(query, InsuredSearchType.DocumentNumber), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetHoldersByName, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetParticipantsByIndividualId(int individualId)
        {
            try
            {
                return new UifJsonResult(true ,DelegateService.claimApplicationService.GetParticipantsByDescriptionInsuredSearchTypeCustomerType(individualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetHoldersByName);
            }
        }
    }
}