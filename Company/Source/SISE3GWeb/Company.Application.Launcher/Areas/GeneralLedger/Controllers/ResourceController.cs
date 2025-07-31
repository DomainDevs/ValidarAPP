using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    public class ResourceController : Controller
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            Response.ContentType = "text/javascript";
            return View();
        }
    }
}