using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.ReportsFasecolda
{
    public class ReportsFasecoldaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ReportsFasecolda";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ReportsFasecolda_default",
                "ReportsFasecolda/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}