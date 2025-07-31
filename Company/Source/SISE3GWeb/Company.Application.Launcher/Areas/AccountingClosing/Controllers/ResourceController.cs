
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.AccountingClosing.Controllers
{
    public class ResourceController : Controller
    {
        /// <summary>
        /// Index
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            Response.ContentType = "text/javascript";
            return View();
        }
    }
}