using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using System.Linq;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class ClaimVehicleController : Controller
    {
        public UifJsonResult ExecuteClaimOperations(ClaimVehicleDTO claimVehicleDTO)
        {
            try
            {
                claimVehicleDTO.Modifications.First().UserId  = SessionHelper.GetUserId();
                claimVehicleDTO.Modifications.First().UserProfileId = SessionHelper.GetUserProfile();

                if (claimVehicleDTO.Number == 0)
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.CreateClaimVehicle(claimVehicleDTO));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateClaimVehicle(claimVehicleDTO));
                }

            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteClaimOperationsVehicle);
            }
        }

        public UifJsonResult GetRisksByEndorsementId(int endorsementId)
        {
            try
            {
                ModuleType moduleType = ModuleType.Claim;
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskVehiclesByEndorsementIdModuleType(endorsementId, moduleType));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByEndorsementId);
            }
        }

        public JsonResult GetVehicleModelsByDescription(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetVehicleModelsByDescription(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetVehicleModelsByDescription, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetVehicleMakesByDescription(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetVehicleMakesByDescription(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetVehicleMakesByDescription, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetDriverByDocumentNumber(string documentNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetDriverByDocumentNumberFullName(documentNumber, Core.Services.UtilitiesServices.Enums.InsuredSearchType.DocumentNumber));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDriverByDocumentNumber);
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

        public JsonResult GetCompanyRiskByPlate(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetRisksVehicleByLicensePlate(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetCompanyRiskByPlate, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetPrefixesByCoveredRiskType()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixesByCoveredRiskType(CoveredRiskType.Vehicle));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixesByCoveredRiskType);
            }
        }
    }
}