using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.ReportsFasecolda.Cexper.Controllers
{
    public class CexperController : Controller
    {
        #region Propiedades
        #endregion

        #region Carga Vistas

        /// <summary>
        /// Carga la vista principal
        /// </summary>
        /// <returns>Action result</returns>
        public ViewResult SearchCexper()
        {
            return View();
        }

        /// <summary>
        /// Carga la vista parcial de Modal Poliza
        /// </summary>
        /// <returns>Action result</returns>
        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalDetailCexperPolicy()
        {
            return View();
        }

        #endregion

        #region Carga Combos y listas
        #endregion        

    }
}