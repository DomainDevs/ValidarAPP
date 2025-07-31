using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class ChangeCoinsuranceController : EndorsementBaseController
    {
        // GET: Endorsement/ChangeCoinsurance
        public ActionResult ChangeCoinsurance()
        {
            return PartialView();
        }
    }
}
