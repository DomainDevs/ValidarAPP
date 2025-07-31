using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims
{
    public class ClaimsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Claims";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Claims_default",
                "Claims/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}