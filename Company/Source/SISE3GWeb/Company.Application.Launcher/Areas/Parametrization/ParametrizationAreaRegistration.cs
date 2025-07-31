using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization
{
    public class ParametrizationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Parametrization";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Parametrization_default",
                "Parametrization/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
