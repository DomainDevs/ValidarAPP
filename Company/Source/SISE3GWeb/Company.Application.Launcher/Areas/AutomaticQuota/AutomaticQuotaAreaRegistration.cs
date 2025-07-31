using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota
{
    public class AutomaticQuotaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AutomaticQuota";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AutomaticQuota_default",
                "AutomaticQuota/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}