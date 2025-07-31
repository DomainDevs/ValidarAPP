using Sistran.Core.Application.Aircrafts.AircraftBusinessService;
using Sistran.Core.Framework.SAF;

namespace Sistran.Core.Integration.AirCraftServices.EEProvider
{
    public static class DelegateService
    {
        internal static IAircraftBusinessService aircraftBusinessService = ServiceProvider.Instance.getServiceManager().GetService<IAircraftBusinessService>();
    }
}
