using System;
using System.Web.Mvc;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class NoticeLocationController : Controller
    {
        public UifJsonResult GetRisksByInsuredId(int insuredId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskPropertiesByInsuredId(insuredId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByInsuredId);
            }
        }

        public UifJsonResult ExecuteNoticeOperations(NoticeLocationDTO noticeLocationDTO, ContactInformationDTO contactInformationDTO, RiskLocationDTO locationDTO)
        {
            try
            {
                noticeLocationDTO.UserId = SessionHelper.GetUserId();
                noticeLocationDTO.UserProfileId = SessionHelper.GetUserProfile();
                if (noticeLocationDTO.Id > 0)
                {
                    if (noticeLocationDTO.StateId == (int)NoticeState.CLOSE)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ClosedNotice);
                    }
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateNoticeLocation(noticeLocationDTO, contactInformationDTO, locationDTO));
                }

                return new UifJsonResult(true, DelegateService.claimApplicationService.CreateNoticeLocation(noticeLocationDTO, contactInformationDTO, locationDTO));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteNoticeOperations);
            }
        }

        public UifJsonResult GetRiskLocationByRiskId(int riskId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskPropertyByRiskId(riskId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskLocationByRiskId);
            }
        }


        public UifJsonResult GetRiskLocationByClaimNoticeId(int claimNoticeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskLocationByClaimNoticeId(claimNoticeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskLocationByClaimNoticeId);
            }
        }

        public JsonResult GetRisksLocationByAddress(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetRiskPropertiesByAddress(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetRisksLocationByAddress, JsonRequestBehavior.DenyGet);
            }
        }
    }
}