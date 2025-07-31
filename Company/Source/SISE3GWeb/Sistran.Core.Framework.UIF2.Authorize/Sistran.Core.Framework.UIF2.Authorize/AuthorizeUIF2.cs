using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace System.Web.Mvc
{
    public class AuthorizeUIF2 : AuthorizeAttribute
    {
        public string AuthorizeAccessGUID { get; set; }

        public string Context { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContextBase)
        {
            try
            {
                if (IsAjaxRequest(httpContextBase.Request))
                {
                    var currentUserSignatureInTicket = "";
                    var currentUserSignatureInPage = httpContextBase.Request.Headers["loaderUserIdentitySISE3G"];

                    if (httpContextBase.User.Identity is FormsIdentity)
                    {
                        currentUserSignatureInTicket = (FormsIdentity)httpContextBase.User.Identity != null ? GetHash(((FormsIdentity)httpContextBase.User.Identity).Ticket.Name) : "";
                    }

                    if (!String.IsNullOrEmpty(currentUserSignatureInPage) && String.IsNullOrEmpty(currentUserSignatureInTicket))
                    {
                        return false;
                    }

                    if (!String.IsNullOrEmpty(currentUserSignatureInPage) && currentUserSignatureInTicket != currentUserSignatureInPage)
                    {
                        return false;
                    }
                }

                var userid = GetUserId(httpContextBase);
                List<AccessPermissions> accessPermissions = (List<AccessPermissions>)HttpContext.Current.Session["USER_ACCESS_PERMISSIONS_KEY"];
                if (accessPermissions == null)
                {
                    accessPermissions = DelegateService.uniqueUserService.GetAccessPermissionsByUserId(userid);
                    HttpContext.Current.Session["USER_ACCESS_PERMISSIONS_KEY"] = accessPermissions;
                }
                var permission = accessPermissions?.Where(m => m.Code == AuthorizeAccessGUID)?.FirstOrDefault();
                var contextPermission = permission?.ContextPermissions?.Where(m => m.Code == Context)?.FirstOrDefault();
                if ((AuthorizeAccessGUID == null || permission != null) && (Context == null || contextPermission != null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string isAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated ? "true" : "false";
           
            filterContext.Result = new HttpStatusCodeResult(403, @"{""IsAuthenticated"" : " + isAuthenticated + "}");
            
        }

        public static bool IsAjaxRequest(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        public static string GetHashWithSalt(string input, string salt)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input+salt));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string GetHash(string input)
        {
            //Read Salt from Config
            return GetHashWithSalt(input, "SISE3GPRV"); 
        }

        public static int GetUserId(HttpContextBase filterContext)
        {
            try
            {
                if (filterContext.User != null && filterContext.User.Identity is FormsIdentity)
                {
                    FormsAuthenticationTicket ticket = ((FormsIdentity)filterContext.User.Identity).Ticket;

                    if (ticket != null)
                    {
                        return Convert.ToInt32(ticket.UserData);
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
    }
}