using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.ListRiskPerson
{
    public class ListRiskPersonAreaRegistration: AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ListRiskPerson";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ListRiskPerson_default",
                "ListRiskPerson/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}