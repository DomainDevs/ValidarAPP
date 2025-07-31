using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees
{
    public class GuaranteeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Guarantees";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Guarantees_default",
                "Guarantees/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}