using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.ReportsFasecolda.Fasecolda.Controllers
{
    public class FasecoldaController : Controller
    {
        #region Propiedades
        #endregion

        #region Carga Vistas

        /// <summary>
        /// Carga la vista principal
        /// </summary>
        /// <returns>Action result</returns>
        public ViewResult SearchSisa()
        {
            return View();
        }

        /// <summary>
        /// Carga la vista parcial de Modal Poliza
        /// </summary>
        /// <returns>Action result</returns>
        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalPolicyInformation()
        {
            return View();
        }

        /// <summary>
        /// Carga la vista parcial de Sinietro
        /// </summary>
        /// <returns>Action result</returns>
        public ViewResult ModalSinisterInformation()
        {
            return View();
        }


        /// <summary>
        /// Carga la vista del Log para FaseColda
        /// </summary>
        /// <returns>Action result</returns>
        public ViewResult LogFaseColda()
        {
            return View();
        }

        #endregion

        #region Carga Combos y listas
        #endregion        

    }
}