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
    public class NoticeAirCraftController : Controller
    {
        public UifJsonResult GetAirCraftMakes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAirCraftMakes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAirCraftMakes);                
            }
        }

        public UifJsonResult GetAirCraftModelsByMakeId(int makeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAirCraftModelsByMakeId(makeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAirCraftModelsByMakeId);
            }
        }

        public UifJsonResult GetAirCraftUses()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAirCraftUses());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAirCraftUsesByPrefixId);
            }
        }

        public UifJsonResult GetAirCraftRegisters()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAircraftRegisters());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAirCraftRegisters);
            }
        }

        public UifJsonResult GetAirCraftOperators()
        {
            try 
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAircraftOperators());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAirCraftOperators);                
            }
        }

        public UifJsonResult ExecuteNoticeOperations(NoticeAirCraftDTO noticeAirCraftDTO, ContactInformationDTO contactInformationDTO, AirCraftDTO airCraftDTO)
        {
            try
            {
                noticeAirCraftDTO.UserId = SessionHelper.GetUserId();
                noticeAirCraftDTO.UserProfileId = SessionHelper.GetUserProfile();
                if (noticeAirCraftDTO.Id > 0)
                {
                    if (noticeAirCraftDTO.StateId == (int)NoticeState.CLOSE)
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ClosedNotice);
                    }
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateNoticeAirCraft(noticeAirCraftDTO, contactInformationDTO, airCraftDTO));
                }

                return new UifJsonResult(true, DelegateService.claimApplicationService.CreateNoticeAirCraft(noticeAirCraftDTO, contactInformationDTO, airCraftDTO));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteNoticeOperationsNoticeAircraft);
            }
        }

        public UifJsonResult GetRisksByInsuredId(int insuredId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskAirCraftsByInsuredId(insuredId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByInsuredId);                
            }
        }

        public UifJsonResult GetRiskAirCraftByClaimNoticeId(int claimNoticeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskAirCraftByClaimNoticeId(claimNoticeId));
            }
            catch (Exception)
            {   
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskAirCraftByClaimNoticeId);
            }
        }

        public UifJsonResult GetRiskAirCraftByRiskId(int riskId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskAirCraftByRiskId(riskId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskAirCraftByRiskId);                
            }
        }
    }
}