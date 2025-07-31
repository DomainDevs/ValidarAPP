using System.Web.Http;


namespace Sistran.Core.Framework.UIF.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
              name: "DefaultApiDynamicForms",
              routeTemplate: "api/DynamicForms/{*url}",
              defaults: new { id = RouteParameter.Optional, controller = "DynamicForms", action = "Execute" }
          );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "NotificationApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
        }
    }
}
