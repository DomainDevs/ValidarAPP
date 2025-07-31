using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.UniqueUser
{
    public class UniqueUserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UniqueUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UniqueUser_default",
                "UniqueUser/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}