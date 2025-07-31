using Sistran.Core.Integration.AircraftServices;
using System.ServiceModel;

namespace Sistran.Company.Application.Aircrafts.AircraftIntegrationService
{
    [ServiceContract]
    public interface ICompanyAircraftIntegrationService : IAircraftIntegrationService
    {
    }
}