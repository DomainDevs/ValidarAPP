using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Product
{
    public class ProductAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Product";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Product_default",
                "Product/{controller}/{action}/{id}",
                new { action = "Product", id = UrlParameter.Optional }
            );
        }
    }
}