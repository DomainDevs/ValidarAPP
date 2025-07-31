using Sistran.Core.Framework.UIF.Web.Areas.Printing.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Controllers
{
    public class MassiveAuthorizationPoliciesController : Controller
    {
        // GET: Massive/MassiveAuthorizationPolicies
        #region ViewResult
        /// <summary>
        /// Pagina principal reasignación de las politicas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult MassiveReassignmentPolicies()
        {
            return View();
        }
        #endregion

        #region JsonResult
        /// <summary>
        ///consulta las autorizacion de politicas segun el filtro
        /// </summary>
        /// <param name="idGroup">id grupo de politica</param>
        /// <param name="idPolicies">id politica</param>
        /// <param name="status"> estado de la politica</param>
        /// <param name="strDateInit">  fecha inicial</param>
        /// <param name="strDateEnd"> fecha final</param>
        /// <param name="sort">  like nombre de la politica</param>
        [HttpPost]
        public JsonResult GetAuthorizationAnswersByFilter(int? idGroup, int? idPolicies, int status, string strDateInit, string strDateEnd, string sort)
        {
            try
            {

                //if (!string.IsNullOrEmpty(strDateInit))
                //    dateInit = DateTime.ParseExact(strDateInit, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //if (!string.IsNullOrEmpty(strDateEnd))
                //    dateEnd = DateTime.ParseExact(strDateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var list = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswersByFilter(idGroup, idPolicies, 0, status, null, null, sort);

                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }
        #endregion
    }
}