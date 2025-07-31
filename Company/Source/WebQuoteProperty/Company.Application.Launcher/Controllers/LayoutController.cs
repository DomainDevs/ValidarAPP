using Sistran.Core.Framework.UIF2.Security;
using Sistran.Core.Framework.UIF2.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    public class LayoutController : Controller
    {       
        public ActionResult SideBar()
        {
            return View();
        }

        public ActionResult SideBarR2()
        {
            if (User.Identity.Name == "" || User.Identity.Name == null)
            {
                ViewBag.Modules = null;
                return View();
            }
            
            return View();
        }
    }
}