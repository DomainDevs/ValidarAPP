using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Notification
{
    public class NotificationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Notification";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Notification_default",
                "Notification/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}