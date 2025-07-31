using System;
using System.Web.Mvc;
using System.Collections.Generic;
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
    public class ClaimAirCraftController : Controller
    {
        public UifJsonResult ExecuteClaimOperations(ClaimAirCraftDTO claimAirCraftDTO)
        {
            try
            {
                claimAirCraftDTO.Modifications.First().UserId = SessionHelper.GetUserId();

                if (claimAirCraftDTO.Number == 0)
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.CreateClaimAirCraft(claimAirCraftDTO));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateClaimAirCraft(claimAirCraftDTO));
                }
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteClaimOperationsAircraft);
            }
        }

        public UifJsonResult GetRisksByEndorsementIdPrefixId(int endorsementId, int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetRiskAirCraftByEndorsementIdPrefixId(endorsementId, prefixId));
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
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixesByCoveredRiskType(CoveredRiskType.Aircraft));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixesByCoveredRiskType);
            }
        }
    }
}