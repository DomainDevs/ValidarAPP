using Sistran.Core.Framework.UIF.Web;
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

namespace Sistran.Core.Framework.UIF.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            string userLanguage = "";
            CultureInfo cultureInfo = null;

            if (HttpContext.Current.Request.UserLanguages != null)
            {
                userLanguage = HttpContext.Current.Request.UserLanguages[0];
                cultureInfo = new CultureInfo(userLanguage.Substring(0, 2));
            }
            else
            {
                cultureInfo = CultureInfo.InstalledUICulture;
            }

            try
            {
                cultureInfo.NumberFormat.NumberGroupSeparator = DelegateService.commonService.GetKeyApplication("NumberGroupSeparator");
                cultureInfo.NumberFormat.NumberDecimalSeparator = DelegateService.commonService.GetKeyApplication("NumberDecimalSeparator");
                cultureInfo.DateTimeFormat.ShortDatePattern = DelegateService.commonService.GetKeyApplication("ShortDatePattern");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
        }
    }
}