using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement
{
    public class EndorsementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Endorsement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Endorsement_default",
                "Endorsement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
