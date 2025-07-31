using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
namespace Sistran.Core.Framework.UIF.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BundleTable.EnableOptimizations = false;
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BindersConfig.Register();
            AuthConfig.RegisterAuth();
            AutoMapperInitialize.Run();
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DataNotationValidationAttribute), typeof(RegularExpressionAttributeAdapter));
        }

        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            var ipAddress = Context.Request?.UserHostAddress;
            string userLanguage = "";
            CultureInfo cultureInfo = null;

            if (HttpContext.Current.Request.UserLanguages != null)
            {
                userLanguage = HttpContext.Current.Request.UserLanguages[0];
                cultureInfo = new CultureInfo(userLanguage.Substring(0, 2));
            }
            else
                cultureInfo = CultureInfo.InstalledUICulture;
            CreateContext(userLanguage);
            try
            {
                cultureInfo.NumberFormat.NumberGroupSeparator = DelegateService.commonService.GetKeyApplication("NumberGroupSeparator");
                cultureInfo.NumberFormat.NumberDecimalSeparator = DelegateService.commonService.GetKeyApplication("NumberDecimalSeparator");
                cultureInfo.DateTimeFormat.ShortDatePattern = DelegateService.commonService.GetKeyApplication("ShortDatePattern");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                EventLog.WriteEntry("ErrorAutentication", $"Application_BeginRequest: {ex.GetBaseException().Message}");
            }
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
        private void CreateContext(string userLanguage)
        {
            try
            {
                var ipAddress = Context.Request?.UserHostAddress;
                if (Context.Request?.Cookies[FormsAuthentication.FormsCookieName]?.Value != null)
                {
                    BusinessContext bc = new BusinessContext();
                    if (FormsAuthentication.Decrypt(Context.Request.Cookies[FormsAuthentication.FormsCookieName]?.Value)?.UserData != null)
                    {
                        bc.UserId = String.IsNullOrEmpty(FormsAuthentication.Decrypt(Context.Request.Cookies?[FormsAuthentication.FormsCookieName]?.Value)?.UserData) ? 0 : Convert.ToInt32(FormsAuthentication.Decrypt(Context.Request.Cookies?[FormsAuthentication.FormsCookieName]?.Value)?.UserData);
                        bc.IPAddress = ipAddress;
                        bc.AccountName = FormsAuthentication.Decrypt(Context.Request.Cookies?[FormsAuthentication.FormsCookieName]?.Value)?.Name;
                        bc.CultureName = userLanguage;
                    }
                    BusinessContext.Current = bc;
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ErrorAutentication", $"CreateContext: {ex.GetBaseException().Message}");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                FormsAuthentication.SignOut();
            }
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
        }
        private void Application_EndRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            //if (Response.IsRequestBeingRedirected)
            //{
            //Exception ex = Server.GetLastError();
            //if (ex is HttpAntiForgeryException)
            //{
            //    string redirectTo = "~/";
            //    Response.Redirect(redirectTo);
            //    return;
            //}
            //else
            //{
            //    string redirectTo = "~/Account/ExternalLoginFailure";
            //    Response.Redirect(redirectTo, true);
            //    return;
            //}
            //}

        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                if (!string.IsNullOrEmpty(User?.Identity?.Name))
                {
                    var uniqueUserSession = DelegateService.uniqueUserService.GetUserInSession(User.Identity.Name);
                    if (uniqueUserSession == null || DateTime.Now > uniqueUserSession.ExpirationDate)
                    {
                        if (uniqueUserSession != null)
                            DelegateService.uniqueUserService.DeletetUserSession(uniqueUserSession.UserId);
                        FormsAuthentication.SignOut();
                        Response.Redirect("~/Account/Login");
                    }
                    else
                    {
                        List<UniqueUserSession> UsersSession = UsersSessionHelper.UsersSession;
                        if (!UsersSession.Where(f => f != null && f.AccountName == User.Identity.Name).Any())
                        {
                            uniqueUserSession.SessionId = Session.SessionID;
                            DelegateService.uniqueUserService.TryInitSession(uniqueUserSession);
                            UsersSessionHelper.Add(uniqueUserSession);
                        }
                    }
                }
            }
            else if (HttpContext.Current.Session != null)
            {
                if (Session["Id"] == null)
                    HttpContext.Current.Session.Add("Id", null);

                HttpContext.Current.Session.Add("LockPassword", new int());
            }
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            List<UniqueUserSession> UsersSession = UsersSessionHelper.UsersSession;
            if (UsersSession?.Count > 0)
            {
                try
                {
                    UniqueUserSession model = UsersSessionHelper.GetBySessionId(Session.SessionID);
                    if (model != null)
                    {
                        DelegateService.uniqueUserService.DeletetUserSession(model.UserId);
                        UsersSessionHelper.Remove(model);
                        Session.Clear();
                        Session.Abandon();
                        Session.RemoveAll();
                    }
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("ErrorAutentication", $"Session_End: {ex.GetBaseException().Message}");
                }
               
            }
        }
    }
}
