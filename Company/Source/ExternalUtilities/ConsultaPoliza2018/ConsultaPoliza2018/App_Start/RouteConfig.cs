using System.Web.Mvc;
using System.Web.Routing;

namespace ConsultaPoliza2018
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "ConsultaPoliza", action = "Create", id = UrlParameter.Optional }
                //defaults: new { controller = "Poliza", action = "Index", id = UrlParameter.Optional }
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
