using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts
{
    public class RulesScriptsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RulesScripts";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RulesScripts_default",
                "RulesScripts/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
