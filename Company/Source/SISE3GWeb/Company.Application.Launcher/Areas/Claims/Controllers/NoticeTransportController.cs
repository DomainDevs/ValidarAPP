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
    public class NoticeTransportController : Controller
    {
        public UifJsonResult ExecuteNoticeOperations(NoticeTransportDTO noticeTransportDTO, ContactInformationDTO contactInformationDTO, TransportDTO transportDTO)
        {
            try
            {
                noticeTransportDTO.UserId = SessionHelper.GetUserId();
                noticeTransportDTO.UserProfileId = SessionHelper.GetUserProfile();
                if (noticeTransportDTO.Id > 0)
                {
                    if (noticeTransportDTO.StateId == (int)NoticeState.CLOSE)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ClosedNotice);
                    }
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateNoticeTransport(noticeTransportDTO, contactInformationDTO, transportDTO));
                }

                return new UifJsonResult(true, DelegateService.claimApplicationService.CreateNoticeTransport(noticeTransportDTO, contactInformationDTO, transportDTO));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {   
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteNoticeTransportOperations);
            }
        }

        public UifJsonResult GetRisksByInsuredId(int insuredId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetTransportsByInsuredId(insuredId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByInsuredId);
            }
        }

        public UifJsonResult GetRiskTransportByClaimNoticeId(int claimNoticeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskTransportByClaimNoticeId(claimNoticeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskTransportByClaimNoticeId);
            }
        }

        public UifJsonResult GetRiskTransportByRiskId(int riskId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskTransportByRiskId(riskId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskTransportByRiskId);
            }
        }
    }
}