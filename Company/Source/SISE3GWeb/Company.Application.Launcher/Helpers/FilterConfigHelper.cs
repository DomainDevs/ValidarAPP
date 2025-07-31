using System;
using System.Web.Mvc;
using System.Web.Routing;


namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class FilterConfigHelper
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

        public class NoDirectAccessAttribute : System.Web.Mvc.ActionFilterAttribute

        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {

                if (filterContext.HttpContext.Request.UrlReferrer == null || filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                {

                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "LogOff", area = "" }));
                }

            }
        }
    }
}