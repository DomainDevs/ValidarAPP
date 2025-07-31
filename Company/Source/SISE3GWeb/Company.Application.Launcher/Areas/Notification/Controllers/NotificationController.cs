

namespace Sistran.Core.Framework.UIF.Web.Areas.Notification.Controllers
{
    using Sistran.Company.AuthorizationPoliciesServices.Models;
    using Sistran.Core.Application.UniqueUserServices.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Framework.UIF2.Security;
    using System;
using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    public class NotificationController : Controller
    {
        /// <summary>
        /// Renderiza la vista Notification/Index
        /// </summary>
        /// <returns>vista Notification/Index</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Actualiza el estado de una notificacion
        /// </summary>
        /// <param name="notificationUser">Notificacion a actualizar</param>
        /// <returns>Resultado de la operacion</returns>
        public JsonResult UpdateNotification(NotificationUser notificationUser)
        {
            try
            {
                DelegateService.uniqueUserService.UpdateNotification(notificationUser);//commonService.UpdateNotification(notificationUser);
                return new UifJsonResult(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Obtiene las notificaciones del usuario logueado
        /// </summary>
        /// <returns>lista de notificaciones</returns>
        public JsonResult GetNotification()
        {
            try
            {
                List<NotificationUser> notification = DelegateService.uniqueUserService.GetNotificationByUser(SessionHelper.GetUserId(), true, true);//commonService.GetNotificationByUser(SessionHelper.GetUserId(), true, true);
                return new UifJsonResult(true, notification);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el la cantidad de notificaciones del usuario logueado
        /// </summary>
        /// <returns>lista de notificaciones</returns>
        public JsonResult GetNotificationCount()
        {
            try
           {
                int notification = DelegateService.uniqueUserService.GetNotificationCountByUser(SessionHelper.GetUserId(), true);//commonService.GetNotificationCountByUser(SessionHelper.GetUserId(), true);
                return new UifJsonResult(true, notification);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// actualiza el estado de una notificacion
        /// </summary>
        /// <param name="notificationId">Id de la notificacion</param>
        public void UpdateNotificationParameter(int notificationId)
        {
            DelegateService.uniqueUserService.UpdateNotificationParameter(notificationId);//commonService.UpdateNotificationParameter(notificationId);
        }

        /// <summary>
        /// Actualiza el estado de todas las notificacion
        /// </summary>
        /// <param name="notificationUser">Notificacion a actualizar</param>
        /// <returns>Resultado de la operacion</returns>
        public JsonResult UpdateAllNotificationDisabledByUser()
        {
            try
            {
                List<NotificationUser> notification = DelegateService.uniqueUserService.UpdateAllNotificationDisabledByUser(SessionHelper.GetUserId());//commonService.UpdateAllNotificationDisabledByUser(SessionHelper.GetUserId());
                return new UifJsonResult(true, notification);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el la cantidad de notificaciones del usuario logueado
        /// </summary>
        /// <returns>lista de notificaciones</returns>
        public JsonResult GetAuthorizationParametres()
        {
            try
            {
                List<CompanyEventAuthorization> total = DelegateService.AuthorizationPoliciesService.GetAuthorizationsByUserId(SessionHelper.GetUserId(), 3);
                List<string> resutl = new List<string>();
                if (total.Count > 0)
                {
                    resutl.Add(total.Count.ToString());
                    ISecurityManager securityManager = new SecurityManager();
                    List<UIF2.Services.Security.Module> modules = securityManager.GetModules(User.Identity.Name)?.ToList();
                    var paths = modules.Where(module => module.Description == "Políticas").FirstOrDefault().SubModules.Where(x => x.Description.Contains("Administración")).FirstOrDefault().SubModules.Where(x => x.Description.Contains("WorkFlow Entrega")).FirstOrDefault().Path;
                    
                    resutl.Add(paths);
                    return new UifJsonResult(true, resutl);
                }
                else {
                    resutl.Add("0");
                    return new UifJsonResult(false, resutl);
                }
               
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
    }
}