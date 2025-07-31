using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Controllers
{
    public class ResourceController : Controller
    {
        // GET: /Accounting/Resource/
        public ActionResult Index()
        {
            Response.ContentType = "text/javascript";
            return View();
        }
    }
}