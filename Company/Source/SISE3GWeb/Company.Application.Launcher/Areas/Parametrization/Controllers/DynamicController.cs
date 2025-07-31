using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class DynamicController : Controller
    {
        // GET: Parametrization/Country
        public ActionResult Dynamic()
        {
            return PartialView();
        }
    }
}