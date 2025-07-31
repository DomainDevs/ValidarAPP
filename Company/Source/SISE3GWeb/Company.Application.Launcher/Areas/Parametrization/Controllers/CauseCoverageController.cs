using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class CauseCoverageController : Controller
    {
        public ActionResult CauseCoverage()
        {
            return View();
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

        public UifJsonResult GetLinesBusinessByPrefixId(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetLinesBusinessByPrefixId(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLineBusiness);
            }
        }

        public UifJsonResult GetSubLinesBusinessByLineBusinessId(int lineBusinessId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSubLinesBusinessByLineBusinessId(lineBusinessId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubLineBusiness);
            }
        }

        public UifJsonResult GetCausesByPrefixId(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCausesByPrefixId(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCauses);
            }
        }

        public UifJsonResult GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(int lineBussinessId, int subLineBussinessId, int causeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCoveragesByLineBusinessIdSubLineBusinessIdCauseId(lineBussinessId, subLineBussinessId, causeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverage);
            }            
        }

        public UifJsonResult GetCauseCoveragesByCauseId(int causeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCoveragesByCauseId(causeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverage);
            }            
        }

        public UifJsonResult CreateCauseCoverage(int causeId, CoverageDTO coverageDTO)
        {
            try
            {
               DelegateService.claimApplicationService.CreateCoverageByCause(causeId, coverageDTO);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateCauseCoverage);
            }
        }

        public UifJsonResult DeleteCauseCoverage(int causeId, int coverageId)
        {
            try
            {
                DelegateService.claimApplicationService.DeleteCoverageByCause(causeId, coverageId);
                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteCauseCoverage);
            }
        }
    }
}