using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers

{
    public class ReassignPoliciesController : Controller
    {
        #region Vistas
        /// <summary>
        /// Retorna a la vista principal
        /// </summary>
        /// <returns></returns>
        public ActionResult ReassignPolicies()
        {
            return View();
        }
        #endregion Vistas

        /// <summary>
        /// retorna todos los grupos de politicas de la BD
        /// </summary>
        /// <returns>Json</returns>
        public ActionResult GetAllStatus()
        {
            StatusServicesModel statusServicesModel = DelegateService.AuthorizationPoliciesService.GetAllStates();
            return new UifJsonResult(true, statusServicesModel.StatusServicemodel.OrderBy(b => b.Description));
        }

        /// <summary>
        /// Consulta las solicitudes de autorizacion por el usuario que solicita, fecha inicio/fin y los estados
        /// </summary>
        /// <param name="status">lista de estados</param>
        /// <param name="strDateInit">fecha inicial</param>
        /// <param name="strDateEnd">fecha final</param>
        /// <returns></returns>
        public ActionResult GetAuthorizationRequestPendingGroups(int groupPolicies, int policies, int idUser, int userAuthorization, string strDateInit, string strDateEnd)
        {
            try
            {
                DateTime dateInit = DateTime.ParseExact(strDateInit, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dateEnd = DateTime.ParseExact(strDateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                List<AuthorizationRequestGroup> requestGroups = DelegateService.AuthorizationPoliciesService.GetAuthorizationRequestPendingGroups(groupPolicies, policies, idUser, userAuthorization, dateInit, dateEnd);
                return new UifJsonResult(true, requestGroups);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetDetailsAuthorizationRequestGroups(string key, int policiesId, int idUser)
        {
            try
            {

                List<AuthorizationRequestGroup> requestGroups = DelegateService.AuthorizationPoliciesService.GetDetailsAuthorizationRequestGroups(idUser, key, policiesId);
                return new UifJsonResult(true, requestGroups);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetAuthorizationAnswersByRequestId(int requestId)
        {
            try
            {
                List<AuthorizationAnswer> answers = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswersByRequestId(requestId);
                return new UifJsonResult(true, answers);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetGroupPolicies()
        {
            try
            {
                List<GroupPolicies> groupPolicies = DelegateService.AuthorizationPoliciesService.GetGroupsPolicies();
                return new UifJsonResult(true, groupPolicies.OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetPoliciesByGroupPolicies(int groupId)
        {
            try
            {
                List<PoliciesAut> policies = DelegateService.AuthorizationPoliciesService.GetPoliciesByIdGroup(groupId);
                return new UifJsonResult(true, policies.OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetUsersAuthorization(int groupId, int policiesId)
        {
            try
            {
                List<User> userAuthorization = DelegateService.AuthorizationPoliciesService.GetUsersAuthorization(groupId, policiesId);
                return new UifJsonResult(true, userAuthorization.OrderBy(x => x.AccountName));

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }


        public ActionResult GetUsers()
        {
            try
            {
                List<User> users = DelegateService.uniqueUserService.GetUsers();
                return new UifJsonResult(true, users.OrderBy(x => x.AccountName));

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetHierarchyByGroupPolicies(int groupId)
        {
            try
            {
                List<CoHierarchyAssociation> coHierarchyAssociations = DelegateService.AuthorizationPoliciesService.GetHierarchyByGroupPolicies(groupId);
                return new UifJsonResult(true, coHierarchyAssociations.OrderBy(x => x.Description));

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetUsersAuthorizationHierarchy(int policiesid, int hierarchyId, int userId)
        {
            try
            {
                List<UserAuthorization> users = DelegateService.AuthorizationPoliciesService.GetUsersAutorizationByIdPoliciesIdHierarchy(policiesid, hierarchyId, null);
                var userHierarchy = users.Where(x => x.User.UserId != userId);
                return new UifJsonResult(true, userHierarchy);
                //return new UifJsonResult(true, users.OrderBy(x => x.User.AccountName));

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult ReasignAuthorizationAnswer(int policiesId, int userAnswerId, string key, int hierarchyId, int userReasignId, string reason, TypeFunction functionType)
        {
            try
            {
                var user = SessionHelper.GetUserId();
                DelegateService.AuthorizationPoliciesService.ReasignAuthorizationAnswer(policiesId, userAnswerId, key, hierarchyId, userReasignId, reason, null, user, functionType);
                return new UifJsonResult(true, App_GlobalResources.Language.EventReassigned);

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
    }
}