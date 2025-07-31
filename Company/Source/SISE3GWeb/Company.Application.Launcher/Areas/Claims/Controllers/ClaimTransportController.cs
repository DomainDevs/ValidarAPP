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
    public class ClaimTransportController : Controller
    {
        public UifJsonResult ExecuteClaimOperations(ClaimTransportDTO claimTransportDTO)
        {
            try
            {
                claimTransportDTO.Modifications.First().UserId = SessionHelper.GetUserId();
                claimTransportDTO.Modifications.First().UserProfileId = SessionHelper.GetUserProfile();

                if (claimTransportDTO.Number == 0)
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.CreateClaimTransport(claimTransportDTO));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateClaimTransport(claimTransportDTO));
                }

            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteClaimOperationsTransport);
            }
        }

        public UifJsonResult GetRisksByEndorsementId(int endorsementId)
        {
            try
            {
                ModuleType moduleType = ModuleType.Claim;
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetTransportByEndorsementIdModuleType(endorsementId, moduleType));
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
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixesByCoveredRiskType(CoveredRiskType.Transport));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixesByCoveredRiskType);
            }
        }
    }
}