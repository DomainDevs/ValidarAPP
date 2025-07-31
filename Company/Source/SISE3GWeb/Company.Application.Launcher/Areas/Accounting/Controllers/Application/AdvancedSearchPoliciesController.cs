using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Application
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class AdvancedSearchPoliciesController : Controller
    {
        // GET: Accounting/AdvancedSearchPolicies
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult AdvancedSearchPolicies()
        {
            return PartialView();
        }
    }
}