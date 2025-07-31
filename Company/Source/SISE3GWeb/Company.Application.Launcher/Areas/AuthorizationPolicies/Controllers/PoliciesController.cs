using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Helpers;
using MPolicies = Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Web;
using System.IO;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{
    [Authorize]
    public class PoliciesController : Controller
    {

        #region ViewResult

        /// <summary>
        /// obtiene la pantalla principal de parametrizacion de politicas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// obtiene la pantalla de las reglas de la politica
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Rules()
        {
            return View();
        }

        /// <summary>
        /// obtiene la modal de datos basicos de la Poliza
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalPolicie()
        {
            return View();
        }

        /// <summary>
        /// obtiene la modal de Authorized Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalAuthorizedUsers()
        {
            return View();
        }

        /// <summary>
        /// obtiene la modal de Notificated Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalNotificatedUsers()
        {
            return View();
        }

        /// <summary>
        /// obtiene la modal de Concepts Description
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ChildActionOnly]
        public ViewResult ModalConceptsDescription()
        {
            return View();
        }

        /// <summary>
        /// obtiene la partial para la busqueda avanzada de politicas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult AdvancedSearch()
        {
            return PartialView();
        }

        #endregion

        #region JsonResult
        /// <summary>
        /// obtiene los grupos de politicas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetGroupsPolicies()
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetGroupsPolicies();
                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// obtiene las de politicas del grupo 
        /// </summary>
        /// <param name="idGroup">id del grupo de politicas</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPoliciesByIdGroup(int idGroup)
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetPoliciesByIdGroup(idGroup);
                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Obtiene las politicas con su respectiva regla segun el filtro
        /// </summary>
        /// <param name="idPackage">id del paquete</param>
        /// <param name="idGroup">id del grupo</param>
        /// <param name="type">tipo de politica</param>
        /// <param name="position">posicion de la politica</param>
        /// <param name="filter">filtro tipo like</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRulesPoliciesByFilter(int? idPackage, int idGroup, int? type, int? position, string filter)
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetRulesPoliciesByFilter(idPackage, idGroup, type, position, filter);
                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// consulta los paquetes asociados a politicas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPackagePolicies()
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetPackagePolicies();
                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Obtierne los tipos de politicas 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTypePolicies()
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetTypePolicies();
                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Obtiene los niveles asociados al grupo de politicas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetLevelsByIdGroupPolicies(int idGroupPolicies, int? level)
        {
            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetLevelsByIdGroupPolicies(idGroupPolicies, level);
                return new UifJsonResult(true, list.Select(x => new { Position = x.Key[0], EntityId = x.Key[1], Description = x.Value }).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Realiza la creacion de una politica con su respactiva regla
        /// </summary>
        /// <param name="policies">politica a crear</param>
        /// <param name="ruleSet">regla a crear</param>
        [HttpPost]
        public JsonResult CreateRulePolicies(MPolicies.PoliciesAut policies, string ruleSet)
        {
            try
            {
                var rulesetObj = RuleHelper.GetRuleSet(ruleSet);
                DelegateService.AuthorizationPoliciesService.CreateRulePolicies(policies, rulesetObj);
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Realiza la modificacion de una politica con su respactiva regla
        /// </summary>
        /// <param name="policies">politica a modificar</param>
        /// <param name="ruleSet">regla a modificar</param>
        [HttpPost]
        public JsonResult UpdateRulePolicies(MPolicies.PoliciesAut policies, string ruleSet)
        {
            try
            {
                var rulesetObj = RuleHelper.GetRuleSet(ruleSet);
                DelegateService.AuthorizationPoliciesService.UpdateRulePolicies(policies, rulesetObj);
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }


        [HttpPost]
        public ActionResult ImportRuleSet(MPolicies.PoliciesAut policies, string ruleSetName)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase fileBase = Request.Files[0];
                    if (fileBase != null)
                    {
                        string[] nameFile = fileBase.FileName.Split('.');
                        if (nameFile.Last() == "rule")
                        {
                            string ruleSet = "";

                            using (StreamReader sr = new StreamReader(fileBase.InputStream))
                            {
                                while (sr.Peek() >= 0) ruleSet += sr.ReadLine();
                            }

                            MRules._RuleSet rulesetObj = RuleHelper.GetRuleSet(EncryptHelper.DecryptKey(ruleSet));
                            if (rulesetObj.IsEvent == false)
                            {
                                return new UifJsonResult(false, Language.ErrorImporRuleNotPolicie);
                            }
                            if (!string.IsNullOrEmpty(ruleSetName))
                            {
                                rulesetObj.Description = ruleSetName;
                            }
                            policies.RuleSet = rulesetObj;
                            policies = DelegateService.AuthorizationPoliciesService.ImportRulePolicies(policies);
                            if (!string.IsNullOrEmpty(policies.RuleSet.FileExceptions))
                            {
                                policies.RuleSet.FileExceptions = DelegateService.commonService.GetKeyApplication("TransferProtocol") + policies.RuleSet.FileExceptions;
                            }
                            return new UifJsonResult(true, policies);
                        }
                        return new UifJsonResult(false, Language.ErrorInvalidFormat);
                    }
                    return new UifJsonResult(false, Language.ErrorUploadFile);
                }
                return new UifJsonResult(false, Language.ErrorUploadFile);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }


        /// <summary>
        /// guarda la politica regla
        /// </summary>
        /// <param name="policies">politica a guardar</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <param name="idHierarchyDt">id de la tabla de decision</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveRulesPolicies(MPolicies.PoliciesAut policies, int? idHierarchyDt)
        {
            try
            {
                DelegateService.AuthorizationPoliciesService.UpdateRulesPolicies(policies, idHierarchyDt);
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Elimina la politica regla y regla
        /// </summary>
        /// <param name="idpolicies">politica a guardar</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteRulePolicies(int idpolicies)
        {
            try
            {
                DelegateService.AuthorizationPoliciesService.DeleteRulePolicies(idpolicies);
                return new UifJsonResult(true, Language.MsgCorrectRemoval);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Crea los usuarios autorizadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="users">lista de usuarios</param>
        /// <param name="countMin">numero minimo de autorizadores</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateUsersAutorization(int idPolicies, List<MPolicies.UserAuthorization> users, int countMin)
        {
            try
            {
                DelegateService.AuthorizationPoliciesService.CreateUsersAutorization(idPolicies, users, countMin);
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }


        /// <summary>
        /// Crea los usuarios notificadores para la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="users">lista de usuarios</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateUsersNotification(int idPolicies, List<MPolicies.UserNotification> users)
        {
            try
            {
                DelegateService.AuthorizationPoliciesService.CreateUsersNotification(idPolicies, users);
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Obtiene los conceptos asignados a la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetConceptDescriptionsByIdPolicies(int idPolicies)
        {
            try
            {
                var listConcepts = DelegateService.AuthorizationPoliciesService.GetConceptDescriptionsByIdPolicies(idPolicies);
                return new UifJsonResult(true, listConcepts);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Guarda los conceptos asociados a la politica
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="conceptDescriptions">lista de conceptos</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveConceptDescriptions(int idPolicies, List<MPolicies.ConceptDescription> conceptDescriptions)
        {
            try
            {
                DelegateService.AuthorizationPoliciesService.SaveConceptDescriptions(idPolicies, conceptDescriptions);
                return new UifJsonResult(true, Language.MessageSavedSuccessfully);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        [HttpPost]
        public ActionResult GetPoliciesByFilter(int? groupPolicyId, int? typePolicyId, int? levelId, string name, string message, bool enabled)
        {

            try
            {
                var list = DelegateService.AuthorizationPoliciesService.GetPoliciesByFilter(groupPolicyId, typePolicyId, levelId, name, message, enabled);
                return new UifJsonResult(true, list);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }



        #endregion

        #region temporalPolicies
        public ActionResult GetPoliciesTemporalByTemporalId(int id)
        {
            try
            {
                var authorizationRequests = DelegateService.AuthorizationPoliciesService.GetAuthorizationRequestsByKey(id.ToString());

                if (authorizationRequests.Any(x => x.Status == TypeStatus.Rejected))
                {
                    return new UifJsonResult(true, new { Message = string.Format(Language.MessageModifyRejectedPolicies, authorizationRequests.Count(x => x.Status == TypeStatus.Rejected)) });
                }
                else if (authorizationRequests.Any(x => x.Status == TypeStatus.Authorized))
                {
                    if (authorizationRequests.Count(x => x.Status == TypeStatus.Authorized) == authorizationRequests.Count)
                    {
                        return new UifJsonResult(true, new { Message = string.Format(Language.MessageModifyAcceptPolicies, authorizationRequests.Count) });
                    }

                    var countAuthorized = authorizationRequests.Count(x => x.Status == TypeStatus.Authorized);
                    var countPending = authorizationRequests.Count(x => x.Status == TypeStatus.Pending);
                    return new UifJsonResult(false, new { Message = string.Format(Language.MessageModifyPendingAcceptPolicies, countPending, countAuthorized) });
                }
                else if (authorizationRequests.Any(x => x.Status == TypeStatus.Pending))
                {
                    return new UifJsonResult(true, new { Message = string.Format(Language.MessageModifyPendingPolicies, authorizationRequests.Count) });
                }

                return new UifJsonResult(true, null);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetTemporalPolcies);
            }
        }

        public ActionResult DeleteNotificationByTemporalId(int id, int functionId)
        {

            try
            {
                var list = DelegateService.AuthorizationPoliciesService.DeleteNotificationByTemporalId(id, functionId);
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