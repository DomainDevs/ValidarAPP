using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class AdjustmentController : EndorsementBaseController
    {
        public ActionResult Adjustment()
        {
            return PartialView();
        }

    }
}