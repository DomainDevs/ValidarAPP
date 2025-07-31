using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.QuotaOperational
{
    public class QuotaOperationalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QuotaOperational";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QuotaOperational_default",
                "QuotaOperational/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}