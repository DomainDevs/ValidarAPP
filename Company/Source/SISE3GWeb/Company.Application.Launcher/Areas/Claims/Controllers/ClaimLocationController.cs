using System;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Framework.UIF.Web.Helpers;
using System.Linq;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class ClaimLocationController : Controller
    {
        // GET: Claims/ClaimLocation
        public UifJsonResult GetPrefixesByCoveredRiskType()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixesByCoveredRiskType(CoveredRiskType.Location));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixesByCoveredRiskType);
            }
        }

        public UifJsonResult GetRisksByEndorsementId(int endorsementId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskPropertiesByEndorsementId(endorsementId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByEndorsementId);
            }
        }

        public UifJsonResult ExecuteClaimOperations(ClaimLocationDTO claimLocationDTO)
        {
            try
            {
                claimLocationDTO.Modifications.First().UserId = SessionHelper.GetUserId();
                claimLocationDTO.Modifications.First().UserProfileId = SessionHelper.GetUserProfile();

                if (claimLocationDTO.Number == 0)
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.CreateClaimLocation(claimLocationDTO));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateClaimLocation(claimLocationDTO));
                }

            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteClaimOperationsLocation);
            }
        }
    }
}