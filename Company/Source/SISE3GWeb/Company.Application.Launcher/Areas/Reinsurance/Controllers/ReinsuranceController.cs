using System.Web.Mvc;
using System.Web.SessionState;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Controllers
{

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ReinsuranceController : Controller
    {
       
        //***********************************************************************************
        //ENTERPRISE EDITION

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Parameter()
        {
            return View();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}
