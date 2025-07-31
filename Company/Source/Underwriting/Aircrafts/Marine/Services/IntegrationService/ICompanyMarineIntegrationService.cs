using Sistran.Core.Integration.MarineServices;
using System.ServiceModel;

namespace Sistran.Company.Application.Marines.MarineIntegrationService
{
    [ServiceContract]
    public interface ICompanyMarineIntegrationService : IMarineIntegrationService
    {
    }
}