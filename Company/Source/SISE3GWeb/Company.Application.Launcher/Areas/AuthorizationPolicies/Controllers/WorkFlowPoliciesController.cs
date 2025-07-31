using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.DTOs;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Controllers;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{

    using UNSE = Sistran.Company.Application.UnderwritingServices.Enums;
    [Authorize]
    public class WorkFlowPoliciesController : Controller
    {
       
        /// <summary>
        /// Pagina principal del WorkFlow de entrega de polizas        
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkFlowPolicies()
        {
            return View();
        }
        
        /// <summary>
        /// Busqueda para usuarios       
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUsersByDescription(string query)
        {
            try
            {
                return Json(DelegateService.authorizationPoliciesApplicationService.GetUsersByDescription(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetUsers, JsonRequestBehavior.DenyGet);
            }
        }

        /// <summary>
        /// Obtiene el grupo de eventos       
        /// </summary>
        /// <returns></returns>
        public UifJsonResult GetEventGroups()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.authorizationPoliciesApplicationService.GetEventGroups());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEventGroup);
            }
        }
        
        /// <summary>
        /// Obtiene las polizas que se van a entregar        
        /// </summary>
        /// <returns></returns>
        public UifJsonResult GetEventAuthorizationsByUserId(int? userId, int eventGroupId, DateTime startDate, DateTime finishDate)
        {
            try
            {
                var sesionId = SessionHelper.GetUserId();
                return new UifJsonResult(true, DelegateService.authorizationPoliciesApplicationService.GetEventAuthorizationsByUserId(userId, eventGroupId, startDate, finishDate, sesionId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicy);
            }
        }

        /// <summary>
        /// Entrega las polizas
        /// </summary>
        /// <returns></returns>
        public UifJsonResult CreateEventAuthorizations(List<EventAuthorizationDTO> eventAuthorizationDTOs, string description)
        {
            try
            {
               var result = DelegateService.authorizationPoliciesApplicationService.CreateBaseEventAuthorization(eventAuthorizationDTOs, description);
               return new UifJsonResult(true, result);

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateEventAuthorizations);
            }
        }

        

        /// <summary>
        /// Obtiene las polizas que se van a entregar        
        /// </summary>
        /// <returns></returns>
        public UifJsonResult GetEventAuthorizationsByUserIdInitial(int eventGroupId)
        {
            try
            {
                var sesionId = SessionHelper.GetUserId();
                return new UifJsonResult(true, DelegateService.authorizationPoliciesApplicationService.GetEventAuthorizationsByUserId(sesionId, eventGroupId, DateTime.Now.AddMonths(-6), DateTime.Now, sesionId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicy);
            }
        }
    }
}