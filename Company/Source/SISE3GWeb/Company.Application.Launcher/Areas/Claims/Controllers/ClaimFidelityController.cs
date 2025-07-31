using System;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Linq;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class ClaimFidelityController : Controller
    {
        public UifJsonResult ExecuteClaimOperations(ClaimFidelityDTO claimFidelityDTO)
        {
            try
            {
                claimFidelityDTO.Modifications.First().UserId = SessionHelper.GetUserId();
                claimFidelityDTO.Modifications.First().UserProfileId = SessionHelper.GetUserProfile();

                if (claimFidelityDTO.Number == 0)
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.CreateClaimFidelity(claimFidelityDTO));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateClaimFidelity(claimFidelityDTO));
                }
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteClaimOperationsFidelity);
            }
        }

        public UifJsonResult GetRisksByEndorsementId(int endorsementId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskFidelitiesByEndorsementId(endorsementId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByEndorsementId);
            }
        }

        public UifJsonResult GetPrefixesByCoveredRiskType()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixesByCoveredRiskType(CoveredRiskType.Surety));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixesByCoveredRiskType);
            }
        }
    }
}