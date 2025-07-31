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
    public class ClaimsLackPeriodController : Controller
    {
        // GET: Parametrization/ConfigurationPanels
        public ActionResult ClaimsLackPeriod()
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
    }
}