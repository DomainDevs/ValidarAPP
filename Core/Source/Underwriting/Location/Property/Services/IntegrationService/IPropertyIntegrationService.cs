using Sistran.Core.Integration.PropertyServices.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Integration.PropertyServices
{
    [ServiceContract]
    public interface IPropertyIntegrationService
    {
        [OperationContract]
        List<RiskLocationDTO> GetRiskPropertiesByInsuredId(int insuredId);

        [OperationContract]
        List<RiskLocationDTO> GetRiskPropertiesByEndorsementId(int endorsementId);

        [OperationContract]
        RiskLocationDTO GetRiskPropertyByRiskId(int riskId);

        [OperationContract]
        List<RiskLocationDTO> GetRiskPropertiesByAddress(string adderess);
    }
}
