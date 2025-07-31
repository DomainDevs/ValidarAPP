using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Company.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models;
using Sistran.Core.Framework.UIF.Web.Controllers;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{
    using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
    using System.Linq;
    using Application.AuthorizationPoliciesServices.Models;

    public class SummaryController : Controller
    {
        #region ViewResult
        /// <summary>
        /// Pagina principal resumen de las politicas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
        #endregion


        #region JsonResult

        /// <summary>
        /// obtiene los usuarios autorizadores de la politica en la jerarquia 
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUsersAutorizationByIdPoliciesIdHierarchy(int idPolicies, int? idHierarchy, bool withUserGroup)
        {
            try
            {
                List<UserAuthorization> users;
                if (withUserGroup)
                {
                    List<CompanyUserGroup> usersGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(SessionHelper.GetUserId());
                    
                    List<UserGroupModel> userGroups = ModelAssemblerData.CreateUserGroups(usersGroup);
                    users = DelegateService.AuthorizationPoliciesService.GetUsersAutorizationByIdPoliciesIdHierarchy(idPolicies, idHierarchy, userGroups);
                }
                else
                {
                    users = DelegateService.AuthorizationPoliciesService.GetUsersAutorizationByIdPoliciesIdHierarchy(idPolicies, idHierarchy, null);
                }

                return new UifJsonResult(true, UserAuthorizationModelView.GetListModelView(users));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error");
            }
        }



        /// <summary>
        /// obtiene los usuarios notificadores de la politica en la jerarquia
        /// </summary>
        /// <param name="idPolicies">id de la politica</param>
        /// <param name="idHierarchy">id de la jerarquia</param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult GetUsersNotificationByIdPoliciesIdHierarchy(int idPolicies, int? idHierarchy, bool withUserGroup)
        {
            try
            {
                List<UserNotification> users;
                if (withUserGroup)
                {
                    List<CompanyUserGroup> usersGroup = DelegateService.uniqueUserService.GetUsersGroupByUserId(SessionHelper.GetUserId());
                    
                    List<UserGroupModel> userGroups = ModelAssemblerData.CreateUserGroups(usersGroup);
                    users = DelegateService.AuthorizationPoliciesService.GetUsersNotificationByIdPoliciesIdHierarchy(idPolicies, idHierarchy, userGroups);
                }
                else
                {
                    users = DelegateService.AuthorizationPoliciesService.GetUsersNotificationByIdPoliciesIdHierarchy(idPolicies, idHierarchy, null);
                }

                return new UifJsonResult(true, UserNotificationModelView.GetListModelView(users));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        /// <summary>
        /// realiza el guardado de las solicitudes de autorizacion 
        /// </summary>
        /// <param name="authorizationRequests">lista de solicitudes de autorizacion</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendAuthorizationRequests(List<AuthorizationRequestModelView> authorizationRequests)
        {
            try
            {
                var idUser = SessionHelper.GetUserId();
                var userName = SessionHelper.GetUserName();

                var listRequests = AuthorizationRequestModelView.CreateListModel(authorizationRequests);

                listRequests.ForEach(x =>
                {
                    x.DateRequest = DateTime.Now;
                    x.UserRequest.UserId = idUser;
                    x.UserRequest.AccountName = userName;
                });

                DelegateService.AuthorizationPoliciesService.CreateAutorizationRequest(listRequests);

                return new UifJsonResult(true, App_GlobalResources.Language.AuthorizationsEventsSent);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }
        #endregion
    }
}