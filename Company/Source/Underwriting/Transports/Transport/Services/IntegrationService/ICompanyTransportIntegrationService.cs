using Sistran.Core.Integration.TransportServices;
using System.ServiceModel;

namespace Sistran.Company.Application.Transports.TransportIntegrationService
{
    [ServiceContract]
    public interface ICompanyTransportIntegrationService : ITransportIntegrationService
    {
    }
}