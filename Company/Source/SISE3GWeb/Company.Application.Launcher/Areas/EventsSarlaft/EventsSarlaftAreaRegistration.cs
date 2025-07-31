using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.EventsSarlaft
{
    public class EventsSarlaftAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "EventsSarlaft"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "EventsSarlaft_default",
                "EventsSarlaft/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}