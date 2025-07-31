using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesAndScripts
{
    public class RulesAndScriptsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RulesAndScripts";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RulesAndScripts_default",
                "RulesAndScripts/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}