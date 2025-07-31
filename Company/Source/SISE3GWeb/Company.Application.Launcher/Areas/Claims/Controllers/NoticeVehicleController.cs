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
    public class NoticeVehicleController : Controller
    {
        public UifJsonResult GetVehicleMakes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetVehicleMakes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetVehicleMakes);
            }
        }

        public UifJsonResult GetVehicleModelsByMakeId(int vehicleMakeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetVehicleModelsByMakeId(vehicleMakeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetVehicleModelsByMakeId);
            }
        }

        public UifJsonResult GetVehicleVersionsByMakeIdModelId(int vehicleMakeId, int vehicleModelId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetVehicleVersionsByMakeIdModelId(vehicleMakeId, vehicleModelId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetVehicleVersionsByMakeIdModelId);
            }
        }

        public UifJsonResult GetVehicleVersionYearsByMakeIdModelIdVersionId(int vehicleMakeId, int vehicleModelId, int vehicleVersionId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetVehicleYearsByMakeIdModelIdVersionId(vehicleMakeId, vehicleModelId, vehicleVersionId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetVehicleVersionYearsByMakeIdModelIdVersionId);
            }
        }

        public UifJsonResult GetVehicleColors()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetVehicleColors());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetVehicleColors);
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

        public UifJsonResult GetRisksByInsuredId(int insuredId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRisksVehicleByInsuredId(insuredId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByInsuredId);
            }
        }

        public UifJsonResult ExecuteNoticeOperations(NoticeVehicleDTO noticeVehicleDTO, ContactInformationDTO contactInformationDTO, VehicleDTO vehicleDTO)
        {
            try
            {
                noticeVehicleDTO.UserId = SessionHelper.GetUserId();
                noticeVehicleDTO.UserProfileId = SessionHelper.GetUserProfile();
                if (noticeVehicleDTO.Id > 0)
                {
                    if (noticeVehicleDTO.StateId == (int)NoticeState.CLOSE) {
                        return new UifJsonResult(false, App_GlobalResources.Language.ClosedNotice);
                    }

                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateNoticeVehicle(noticeVehicleDTO, contactInformationDTO, vehicleDTO));
                }

                return new UifJsonResult(true, DelegateService.claimApplicationService.CreateNoticeVehicle(noticeVehicleDTO, contactInformationDTO, vehicleDTO));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteNoticeVehicleOperations);
            }
        }

        public UifJsonResult GetRiskVehicleByRiskId(int riskId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskVehicleByRiskId(riskId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskVehicleByRiskId);
            }
        }

        public UifJsonResult GetRiskVehicleByClaimNoticeId(int claimNoticeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskVehicleByClaimNoticeId(claimNoticeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRiskVehicleByClaimNoticeId);
            }
        }
    }
}