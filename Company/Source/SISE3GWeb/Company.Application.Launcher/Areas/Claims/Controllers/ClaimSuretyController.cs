using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class ClaimSuretyController : Controller
    {
        public UifJsonResult GetRisksByEndorsementIdPrefixId(int endorsementId, int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSuretiesByEndorsementIdPrefixId(endorsementId, prefixId));

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRisksByEndorsementIdPrefixId);
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

        public UifJsonResult ExecuteClaimOperations(ClaimSuretyDTO claimSuretyDTO)
        {
            try
            {
                claimSuretyDTO.Modifications.First().UserId = SessionHelper.GetUserId();
                claimSuretyDTO.Modifications.First().UserProfileId = SessionHelper.GetUserProfile();

                if (claimSuretyDTO.Number == 0)
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.CreateClaimSurety(claimSuretyDTO));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateClaimSurety(claimSuretyDTO));
                }

            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteClaimOperationSurety);
            }
        }
    }
}