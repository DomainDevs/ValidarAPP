using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Externals
{
    public class ExternalsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Externals";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Externals_default",
                "Externals/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}