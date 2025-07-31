using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class ChangeAgentController : EndorsementBaseController
    {

        public ActionResult ChangeAgent()
        {
            return PartialView();
        }      
    }
}