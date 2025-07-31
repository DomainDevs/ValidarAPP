using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class AutomaticSalaryUpdateController : Controller
    {
        // GET: Claims/AutomaticSalaryUpdate
        public ActionResult AutomaticSalaryUpdate()
        {
            return View();
        }
        
        public UifJsonResult GetBranches()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetBranches());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranches);
            }
        }

        public UifJsonResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        public JsonResult SearchClaimsBySalaryEstimation(SearchClaimDTO searchClaimDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.SearchClaimsBySalaryEstimationCurrentYear(searchClaimDTO,DateTime.Now.Year));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchClaims);
            }

        }

        public JsonResult UpdateEstimationsSalaries(List<SubClaimDTO> subClaimsDTO)
        {
            try
            {
                subClaimsDTO.ForEach(x => { x.CreationDate = DateTime.Now; x.UserId = SessionHelper.GetUserId(); });

                return new UifJsonResult(true, DelegateService.claimApplicationService.UpdateEstimationsSalaries(subClaimsDTO,DateTime.Now.Year));
            }
            catch (Exception)
            {                
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorUpdateEstimationsSalaries);
            }
        }

    }
}