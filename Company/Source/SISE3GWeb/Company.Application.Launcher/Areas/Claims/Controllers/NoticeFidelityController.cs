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
    public class NoticeFidelityController : Controller
    {
        public UifJsonResult ExecuteNoticeOperations(NoticeFidelityDTO noticeFidelityDTO, ContactInformationDTO contactInformationDTO, FidelityDTO fidelityDTO)
        {
            try
            {
                noticeFidelityDTO.UserId = SessionHelper.GetUserId();
                noticeFidelityDTO.UserProfileId = SessionHelper.GetUserProfile();
                if (noticeFidelityDTO.Id > 0)
                {
                    if (noticeFidelityDTO.StateId == (int)NoticeState.CLOSE)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ClosedNotice);
                    }
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateNoticeFidelity(noticeFidelityDTO, contactInformationDTO, fidelityDTO));
                }

                return new UifJsonResult(true, DelegateService.claimApplicationService.CreateNoticeFidelity(noticeFidelityDTO, contactInformationDTO, fidelityDTO));
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

        public UifJsonResult GetRisksByInsuredId(int insuredId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskFidelitiesByInsuredId(insuredId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByInsuredId);
            }
        }

        public UifJsonResult GetRiskFidelityByClaimNoticeId(int claimNoticeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskFidelityByClaimNoticeId(claimNoticeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskFidelityByClaimNoticeId);
            }
        }

        public UifJsonResult GetRiskFidelityByRiskId(int riskId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskFidelityByRiskId(riskId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskFidelityByRiskId);
            }
        }

        public UifJsonResult GetRiskCommercialClasses()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskCommercialClasses());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskCommercialClasses);
            }
        }

        public UifJsonResult GetOccupations()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetOccupations());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetOccupations);
            }
        }
    }
}