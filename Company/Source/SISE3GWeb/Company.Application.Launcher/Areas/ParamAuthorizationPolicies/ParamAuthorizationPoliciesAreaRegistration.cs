using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.ParamAuthorizationPolicies
{
    public class ParamAuthorizationPoliciesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ParamAuthorizationPolicies";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ParamAuthorizationPolicies_default",
                "ParamAuthorizationPolicies/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}