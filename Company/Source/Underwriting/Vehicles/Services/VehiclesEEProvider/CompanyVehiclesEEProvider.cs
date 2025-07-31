using Sistran.Core.Application.Vehicles.EEProvider;
using System.ServiceModel;
namespace Sistran.Company.Application.Vehicles.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyVehiclesEEProvider : VehiclesEEProvider, ICompanyVehicles
    {

    }
}