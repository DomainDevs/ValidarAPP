using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using System;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Business;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class NoticeController : Controller
    {
        public ActionResult MasterNotice(int? id)
        {
            switch (id)
            {
                case 1:
                    ViewBag.Page = "../NoticeVehicle/NoticeVehicle";
                    break;
                case 2:
                    ViewBag.Page = "../NoticeLocation/NoticeLocation";
                    break;
                case 3:
                    ViewBag.Page = "../NoticeSurety/NoticeSurety";
                    break;
                case 4:
                    ViewBag.Page = "../NoticeTransport/NoticeTransport";
                    break;
                case 5:
                    ViewBag.Page = "../NoticeAirCraft/NoticeAirCraft";
                    break;
                case 6:
                    ViewBag.Page = "../NoticeFidelity/NoticeFidelity";
                    break;
                default:
                    break;
            }
            return View();
        }

        public UifJsonResult GetDamageTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetDamageTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDamageTypes);
            }
        }

        public UifJsonResult GetDamageResponsibilities()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetDamageResponsibilities());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDamageResponsibilities);
            }
        }

        public UifJsonResult GetPolicyByEndorsementIdModuleType(int endorsementId)
        {
            try
            {
                PolicyDTO policyDTO = DelegateService.claimApplicationService.GetPolicyByEndorsementIdModuleType(endorsementId);
                if (!DelegateService.commonService.GetParameterByParameterId(12191).BoolParameter.GetValueOrDefault())
                {
                    policyDTO.IsReinsurance = ClaimBusiness.GetPolicyReinsurance2G(policyDTO);
                }

                return new UifJsonResult(true, policyDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPolicyByEndorsementIdModuleType);
            }
        }

        public UifJsonResult GetClaimBranchesbyUserId()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetBranchesByUserId(SessionHelper.GetUserId()));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimBranchesbyUserId);
            }
        }

        public UifJsonResult GetDocumentTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.companyUniquePersonParamService.GetDocumentTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypes);
            }
        }

        public UifJsonResult GetInsuredsByIndividualId(string individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAffectedByDescriptionInsuredSearchTypeCustomerType(individualId, Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId, Core.Services.UtilitiesServices.Enums.CustomerType.Individual));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredsByIndividualId);
            }
        }

        public UifJsonResult GetEstimationsType()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetEstimationTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEstimationsType);
            }
        }

        public UifJsonResult GetNoticeByNoticeId(int noticeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetNoticeByNoticeId(noticeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetNoticeByNoticeId);
            }
        }

        public UifJsonResult GetClaimsByPolicyId(int policyId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimsByPolicyId(policyId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimsByPolicyId);
            }
        }

        public UifJsonResult SendEmailToAgendNotice(string subject, string message, string mailDestination)
        {
            try
            {
                DelegateService.claimApplicationService.SendEmailToAgendNotice(subject, message, mailDestination);
                return new UifJsonResult(true, App_GlobalResources.Language.SendEmailToAgendNotice);
            }
            catch (Exception)
            {   
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSendEmailToAgendNotice);
            }
        }

        public ActionResult ScheduleNotice(string subject, string message, DateTime startEventDate, DateTime finishEventDate)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.ScheduleNotice(subject, message, startEventDate, finishEventDate), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorScheduleNotice);
            }
        }

        public ActionResult GetRiskCoveragesByDescription(int riskId, string description)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetCoveragesByRiskIdDescription(riskId, description.ToUpper()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskCoveragesByDescription);
            }
        }

        public UifJsonResult DeleteNoticeCoverageByCoverage(int noticeId, int coverageId, int individualId, int estimateTypeId)
        {
            try
            {
                DelegateService.claimApplicationService.DeleteNoticeCoverageByCoverage(noticeId, coverageId, individualId, estimateTypeId);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskCoveragesByDescription);
            }
        }

        public UifJsonResult GetDefaultCountry()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetParameterByDescription("DefaultCountryId").NumberParameter);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSetDefaultCountry);
            }
        }

    }
}