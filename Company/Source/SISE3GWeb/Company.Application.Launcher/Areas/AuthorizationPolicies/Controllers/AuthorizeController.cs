using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{
    [Authorize]
    public class AuthorizeController : Controller
    {
        #region ViewResult
        /// <summary>
        /// Pagina principal de la autorizacion de politicas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Pagina parcial con el formulario de las autorizaciones pendientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult AuthorizationPending()
        {
            return PartialView();
        }

        /// <summary>
        /// Pagina parcial con el formulario de las politicas autorizadas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult AuthorizationAuthorized()
        {
            return PartialView();
        }

        /// <summary>
        /// Pagina parcial con el formulario de las politicas rechazadas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult AuthorizationRejected()
        {
            return PartialView();
        }

        /// <summary>
        /// Pagina parcial con el formulario de las politicas rechazadas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult AuthorizationReasign()
        {
            return PartialView();
        }


        /// <summary>
        /// parcial para aceptar la autorizacion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult PanelAccept()
        {
            return PartialView();

        }

        /// <summary>
        /// parcial para rechazar la autorizacion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult PanelReject()
        {
            return PartialView();

        }

        /// <summary>
        /// parcial para reasignar la autorizacion
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult PanelReassign()
        {
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult AdvancedSearch()
        {
            return PartialView();
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
                var idUser = SessionHelper.GetUserId();
                DateTime? dateInit = null;
                DateTime? dateEnd = null;

                if (!string.IsNullOrEmpty(strDateInit))
                    dateInit = DateTime.ParseExact(strDateInit, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(strDateEnd))
                    dateEnd = DateTime.ParseExact(strDateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                idGroup = idGroup ?? 0;
                idPolicies = idPolicies ?? 0;

                var list = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswersByFilter(idGroup, idPolicies, idUser, status, dateInit, dateEnd, sort);

                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        ///consulta las autorizacion reasignadas de politicas segun el filtro
        /// </summary>
        /// <param name="idGroup">id grupo de politica</param>
        /// <param name="idPolicies">id politica</param>
        /// <param name="status"> estado de la politica</param>
        /// <param name="strDateInit">  fecha inicial</param>
        /// <param name="strDateEnd"> fecha final</param>
        /// <param name="sort">  like nombre de la politica</param>
        [HttpPost]
        public JsonResult GetAuthorizationAnswersReasignByFilter(int? idGroup, int? idPolicies, string strDateInit, string strDateEnd, string sort)
        {
            try
            {
                var idUser = SessionHelper.GetUserId();
                DateTime? dateInit = null;
                DateTime? dateEnd = null;

                if (!string.IsNullOrEmpty(strDateInit))
                    dateInit = DateTime.ParseExact(strDateInit, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(strDateEnd))
                    dateEnd = DateTime.ParseExact(strDateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                idGroup = idGroup ?? 0;
                idPolicies = idPolicies ?? 0;

                //var list = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswersReasignByFilter(idGroup, idPolicies, idUser, dateInit, dateEnd, sort);
                List<AuthorizationAnswerGroup> list = new List<AuthorizationAnswerGroup>();
                list = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswersReasignByFilter(idGroup, idPolicies, idUser, dateInit, dateEnd, sort);
                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        [HttpPost]
        public JsonResult GetAuthorizationAnswerDescriptions(int idPolicies, int idUser, int status, string key)
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswerDescriptions(idPolicies, idUser, status, key);
                return new UifJsonResult(true, list);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetAuthorizationAnswerDescription(int idPolicies, string key)
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswerDescription(idPolicies, key);
                return new UifJsonResult(true, list);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }


        /// <summary>
        /// consultar las jerarquias superiores parametrizadas a la politica
        /// </summary>
        /// <param name="policiesId">id de la politica</param>
        /// <param name="hierarchyId">jerarquia del usuario actual</param>
        /// <param name="userId">id del usuario actual</param>
        /// <returns>lista de las jerarquias autorizadoras</returns>
        [HttpPost]
        public JsonResult GetAuthorizationHierarchy(int policiesId, int hierarchyId, int userId)
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetAuthorizationHierarchy(policiesId, hierarchyId, userId);
                return new UifJsonResult(true, list);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// consultar los usuarios autorizadores de la politica en esa jerarquia
        /// </summary>
        /// <param name="autorizatioAnswerId">id de la autorizacion</param> 
        /// <param name="hierarchyId">jerarquia autorizadora</param>
        /// <returns>lista usuarios autorizadores de la jerarquia</returns>
        [HttpPost]
        public JsonResult GetUsersAuthorizationHierarchy(int autorizatioAnswerId, int hierarchyId)
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetUsersAuthorizationHierarchy(autorizatioAnswerId, hierarchyId);
                return new UifJsonResult(true, list);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Reasigna una autorizacion pendiente
        /// </summary>
        /// <param name="reason">motivo de la reasignacion</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReasingAuthorization(int policiesId, int userAnswerId, string key, int hierarchyId, int userReasignId, string reason, List<int> policiesToReassign, TypeFunction functionType)
        {
            try
            {
                var idUser = SessionHelper.GetUserId();
                policiesToReassign.RemoveAll(x => x == 0);
                policiesToReassign = policiesToReassign.Count > 0 ? policiesToReassign : null;
                DelegateService.AuthorizationPoliciesService.ReasignAuthorizationAnswer(policiesId, idUser, key, hierarchyId, userReasignId, reason, policiesToReassign, idUser, functionType);
                return new UifJsonResult(true, App_GlobalResources.Language.EventReassigned);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Rechaza una  autorizacion pendiente
        /// </summary>
        /// <param name="authorizationsAnswer">lsita de los id de la autorizacion</param>
        /// <param name="reason">motivo del rechazo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RejectAuthorization(int policiesId, int userAnswerId, string key, string reason, int idRejection, List<int> policiesToReject, TypeFunction functionType)
        {
            try
            {
                var idUser = SessionHelper.GetUserId();
                policiesToReject.RemoveAll(x => x == 0);
                policiesToReject = policiesToReject.Count > 0 ? policiesToReject : null;
                DelegateService.AuthorizationPoliciesService.RejectAuthorization(policiesId, idUser, key, reason, idRejection, policiesToReject, functionType);
                return new UifJsonResult(true, App_GlobalResources.Language.EventRejected);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// acepta una  autorizacion pendiente
        /// </summary>
        /// <param name="authorizationsAnswer">lsita de los id de la autorizacion</param>
        /// <param name="reason">motivo de la aceptacion</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AcceptAuthorization(int policiesId, int userAnswerId, string key, string reason, List<int> policiesToAccept, TypeFunction functionType)
        {
            try
            {
                var idUser = SessionHelper.GetUserId();
                policiesToAccept.RemoveAll(x => x == 0);
                policiesToAccept = policiesToAccept.Count > 0 ? policiesToAccept : null;
                DelegateService.AuthorizationPoliciesService.AcceptAuthorization(policiesId, idUser, key, reason, policiesToAccept, functionType);
                return new UifJsonResult(true, App_GlobalResources.Language.EventAccepted);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    return new UifJsonResult(false, ex.Message);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveTemp);
            }
        }

        /// <summary>
        /// Descarga todos los eventos existtentes en un excel
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateFileToExport(int idPolicies, int idUser, int status, string key)
        {

            try
            {
                List<string> policiesList = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswerDescriptions(idPolicies, idUser, status, key);

                string urlFile = DelegateService.AuthorizationPoliciesService.GenerateFileToPolicies(policiesList, "Reporte de politicas");

                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        /// <summary>
        /// Obtiene el historias de las autorizaciones reasignadas
        /// </summary>
        /// <param name="authorizationsAnswer">lista de autorizaciones a consultar</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetHistoryReasign(int policiesId, int userAnswerId, string key)
        {
            try
            {
                var idUser = SessionHelper.GetUserId();
                var reasignList = DelegateService.AuthorizationPoliciesService.GetHistoryReasign(policiesId, idUser, key);
                return new UifJsonResult(true, reasignList);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }


        [HttpPost]
        public JsonResult GetAuthorizeOption()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<AuthorizeOption>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryUploadTypes);
            }
        }

        [HttpPost]
        public JsonResult GetRejectOption()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<RejectOption>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryUploadTypes);
            }
        }

        [HttpPost]
        public JsonResult GetReassignOption()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<ReassignOption>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryUploadTypes);
            }
        }

        [HttpPost]
        public JsonResult DisablePolicies(List<AuthorizationRequest> authorizationRequests)
        {
            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateAuthorizationAnswersByAuthorizationRequests(authorizationRequests, SessionHelper.GetUserName());
                return new UifJsonResult(true, "");
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex);
            }
        }


        [Flags]
        public enum AuthorizeOption
        {
            AuthorizeAll = 1,
            AuthorizeIndividual = 2,
            AuthorizeRange = 3
        }

        [Flags]
        public enum RejectOption
        {
            RejectAll = 1,
            RejectIndividual = 2,
            RejectRange = 3
        }

        [Flags]
        public enum ReassignOption
        {
            ReassignAll = 1,
            ReassignIndividual = 2,
            ReassignRange = 3
        }

        #endregion
    }
}