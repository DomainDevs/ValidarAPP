using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Printing
{
    public class PrintingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Printing";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Printing_default",
                "Printing/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
