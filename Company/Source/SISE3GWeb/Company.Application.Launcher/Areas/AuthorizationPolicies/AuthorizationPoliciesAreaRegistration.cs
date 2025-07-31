using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies
{
    public class AuthorizationPoliciesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AuthorizationPolicies";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AuthorizationPolicies_default",
                "AuthorizationPolicies/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}