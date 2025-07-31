using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive
{
    public class MassiveAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Massive";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Massive_default",
                "Massive/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
