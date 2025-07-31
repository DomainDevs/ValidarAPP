using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        public ActionResult Index2()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetCumulusQSise(int individualId)
        {
            var cumulo = DelegateService.underwritingService.GetCumulusQSise(individualId);

            return Json(cumulo, JsonRequestBehavior.AllowGet);
        }

        //vista para sitio de consultas
        public ActionResult BusinessIntelligence()
        {
            //Obtiene la url del modulo de consultas
            ViewBag.IdentityEnvironment = System.Configuration.ConfigurationManager.AppSettings["IdentityEnvironment"];
            return View();
        }
    }
}