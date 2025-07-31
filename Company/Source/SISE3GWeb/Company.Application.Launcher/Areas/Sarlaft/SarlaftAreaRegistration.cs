using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Sarlaft
{
    public class SarlaftAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Sarlaft";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Sarlaft_default",
                "Sarlaft/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}