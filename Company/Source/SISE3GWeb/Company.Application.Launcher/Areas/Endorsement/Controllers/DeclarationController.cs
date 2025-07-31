using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class DeclarationController : EndorsementBaseController
    {
        public ActionResult Declaration()
        {
            return PartialView();
        }      
    }
}