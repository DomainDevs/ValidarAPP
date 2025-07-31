using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sistran.Core.Framework.UIF.Web.Filters
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx.Session != null)
            {
                if (ctx.Session["USER_NAME"] == null || !filterContext.HttpContext.Request.IsAuthenticated)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult { Data = "_Logon_" };
                    }
                    else
                    {
                        FormsAuthentication.SignOut();
                        string redirectTo = "~/Account/Login";
                        filterContext.Result = new RedirectResult(redirectTo);
                        return;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}