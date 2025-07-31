using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    public class ResourcesController : Controller
    {
        public ActionResult Fonts(string path)
        {
            return RedirectPermanent(Url.Content("~/lib/framework/build/vendors/fonts/" + path));
        }
        public ActionResult Images(string path)
        {
            return RedirectPermanent(Url.Content("~/lib/framework/build/vendors/images/" + path));
        }
    }
}