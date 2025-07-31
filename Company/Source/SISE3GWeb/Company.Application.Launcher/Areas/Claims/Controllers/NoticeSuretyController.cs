using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.Enums;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class NoticeSuretyController : Controller
    {
        public JsonResult GetRisksByInsuredId(int insuredId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRisksSuretyByInsuredId(insuredId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByInsuredId);
            }
        }

        public UifJsonResult ExecuteNoticeOperations(NoticeSuretyDTO noticeSuretyDTO, ContactInformationDTO contactInformationDTO, SuretyDTO suretyDTO)
        {
            try
            {
                noticeSuretyDTO.UserId = SessionHelper.GetUserId();
                noticeSuretyDTO.UserProfileId = SessionHelper.GetUserProfile();
                if (noticeSuretyDTO.Id > 0)
                {
                    if (noticeSuretyDTO.StateId == (int)NoticeState.CLOSE)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ClosedNotice);
                    }
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateNoticeSurety(noticeSuretyDTO, contactInformationDTO, suretyDTO));
                }

                return new UifJsonResult(true, DelegateService.claimApplicationService.CreateNoticeSurety(noticeSuretyDTO, contactInformationDTO, suretyDTO));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteNoticeSuretyOperations);
            }
        }

        public UifJsonResult GetRiskSuretyByRiskIdPrefixId(int riskId, int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSuretyByRiskIdPrefixId(riskId, prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskSuretyByRiskId);
            }
        }

        public UifJsonResult GetRiskSuretyByClaimNoticeId(int claimNoticeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskSuretyByClaimNoticeId(claimNoticeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskSuretyByClaimNoticeId);
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
    }
}