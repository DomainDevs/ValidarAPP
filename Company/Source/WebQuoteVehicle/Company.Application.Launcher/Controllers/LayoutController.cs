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
            ViewBag.Modules = DelegateService.authorizationService.GetModules(User.Identity.Name);
            return View();
        }

        public ActionResult SideBarR2()
        {
            try
            {
                int userId = SessionHelper.GetUserId();

                if (User.Identity.Name == "" || User.Identity.Name == null)
                {
                    ViewBag.Modules = null;
                    return View();
                }

                ViewBag.Modules = DelegateService.authorizationService.GetModules(User.Identity.Name);                
                return View();
            }
            catch (TimeoutException)
            {
                return View();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Servicio no Disponible");
                ViewBag.Modules = null;
                return View();
            }
        }
    }
}
