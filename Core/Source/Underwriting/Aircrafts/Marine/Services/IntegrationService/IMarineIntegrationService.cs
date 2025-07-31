using Sistran.Core.Integration.MarineServices.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Integration.MarineServices
{
    [ServiceContract]
    public interface IMarineIntegrationService
    {
        [OperationContract]
        List<AirCraftDTO> GetMarinesByEndorsementIdModuleType(int endorsementId);

    }
}