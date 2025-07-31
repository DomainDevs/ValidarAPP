using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers
{
    [Authorize]
    [HandleError]
    public class AuthorizationSectorController : Controller
    {
        #region Constants

        public const string SortOrder = "ASC";
        
        #endregion

        #region AuthorizationSector

        /// <summary>
        /// MainAuthorizationSector
        /// Muestra la pagina principal de autorizaciones del sector
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainAuthorizationSector()
        {
            return View();
        }
        
        #endregion

	}
}