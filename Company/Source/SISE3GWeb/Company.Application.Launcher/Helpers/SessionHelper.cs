using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Managers;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class SessionHelper
    {
        public static bool ExistUserInSession()
        {
            return HttpContextManager.Current.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Error de Autenticación
        /// or
        /// Error de Autenticación
        /// or
        /// Error de Autenticación
        /// </exception>
        public static int GetUserId(bool thrwo = true)
        {
            try
            {
                if (HttpContextManager.Current?.User != null && HttpContextManager.Current.User.Identity is FormsIdentity)
                {
                    FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContextManager.Current.User.Identity).Ticket;

                    if (ticket != null)
                    {
                        return Convert.ToInt32(ticket.UserData);
                    }
                    else
                    {
                        if (thrwo)
                            throw new Exception("Error de Autenticación");
                        else
                            return -1;
                    }
                }
                else
                {
                    if (thrwo)
                        throw new Exception("Error de Autenticación");
                    else
                        return -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error de Autenticación", ex);
            }
        }

        public static string GetUserName(bool thrwo = true)
        {
            try
            {
                if (HttpContextManager.Current != null && HttpContextManager.Current.User != null && HttpContextManager.Current.User.Identity is FormsIdentity)
                {
                    FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContextManager.Current.User.Identity).Ticket;
                    if (ticket != null)
                    {
                        return ticket.Name.ToUpper();
                    }
                    else
                    {
                        if (thrwo)
                            throw new Exception("Error de Autenticación");
                        else
                            return "";
                    }
                }
                else
                {
                    if (thrwo)
                        throw new Exception("Error de Autenticación");
                    else
                        return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error de Autenticación", ex);
            }
        }

        public static int GetUserProfile()
        {
            try
            {
                if (HttpContextManager.Current != null && HttpContextManager.Current.User != null && HttpContextManager.Current.User.Identity is FormsIdentity)
                {
                    FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContextManager.Current.User.Identity).Ticket;

                    if (ticket != null)
                    {
                        return UsersSessionHelper.UsersSession.FirstOrDefault(x => x.UserId == Convert.ToInt32(ticket.UserData)).ProfileId;
                    }
                    else
                    {
                        throw new Exception("Error de Autenticación");
                    }
                }
                else
                {
                    throw new Exception("Error de Autenticación");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error de Autenticación", ex);
            }
        }

        public static bool IsAuthorize(string authorizeAccessGUID)
        {
            List<AccessPermissions> accessPermissions = (List<AccessPermissions>)HttpContext.Current.Session["USER_ACCESS_PERMISSIONS_KEY"];
            if (accessPermissions == null)
            {
                accessPermissions = DelegateService.uniqueUserService.GetAccessPermissionsByUserId(GetUserId());
                HttpContext.Current.Session["USER_ACCESS_PERMISSIONS_KEY"] = accessPermissions;
            }
            var permission = accessPermissions?.Where(m => m.Code == authorizeAccessGUID)?.FirstOrDefault();
            var context = (string)HttpContext.Current.Session["USER_ACCESS_PERMISSIONS_KEY_CONTEXT"];
            var contextPermission = permission?.ContextPermissions?.Where(m => m.Code == context)?.FirstOrDefault();
            if ((authorizeAccessGUID == null || permission != null) && (context == null || permission?.ContextPermissions?.Count == 0 || contextPermission != null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsAuthorize(string authorizeAccessGUID, string context)
        {
            return true;
            List<AccessPermissions> accessPermissions = (List<AccessPermissions>)HttpContext.Current.Session["USER_ACCESS_PERMISSIONS_KEY"];
            if (accessPermissions == null)
            {
                accessPermissions = DelegateService.uniqueUserService.GetAccessPermissionsByUserId(GetUserId());
                HttpContext.Current.Session["USER_ACCESS_PERMISSIONS_KEY"] = accessPermissions;
            }
            var permission = accessPermissions?.Where(m => m.Code == authorizeAccessGUID)?.FirstOrDefault();
            var contextPermission = permission?.ContextPermissions?.Where(m => m.Code == context)?.FirstOrDefault();
            if ((authorizeAccessGUID == null || permission != null) && (context == null || contextPermission != null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}