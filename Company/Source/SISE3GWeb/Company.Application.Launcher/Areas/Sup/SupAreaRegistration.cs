using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Sup
{
    public class SupAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Sup"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sup_default",
                "Sup/{controller}/{action}/{id}",
                new { Action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}