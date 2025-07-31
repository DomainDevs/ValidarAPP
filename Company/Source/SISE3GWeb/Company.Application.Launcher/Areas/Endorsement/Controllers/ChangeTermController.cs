using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class ChangeTermController : EndorsementBaseController
    {
        public ActionResult ChangeTerm()
        {
            return PartialView();
        }

    }
}