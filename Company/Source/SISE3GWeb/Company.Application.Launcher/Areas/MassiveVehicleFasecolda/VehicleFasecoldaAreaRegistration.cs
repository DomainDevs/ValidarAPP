using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.VehicleFasecolda
{
    public class VehicleFasecoldaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "MassiveVehicleFasecolda"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MassiveVehicleFasecolda_default",
                "MassiveVehicleFasecolda/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}