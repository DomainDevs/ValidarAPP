using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting
{
    public class UnderwritingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Underwriting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Underwriting_default",
                "Underwriting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}