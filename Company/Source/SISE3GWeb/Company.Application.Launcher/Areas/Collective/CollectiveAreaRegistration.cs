using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Collective
{
    public class CollectiveAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Collective";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Collective_default",
                "Collective/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
