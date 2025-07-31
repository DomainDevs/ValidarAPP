using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Sistran.Core.Integration.SuretyServices.DTOs;
using Sistran.Core.Services.UtilitiesServices.Enums;
namespace Sistran.Core.Integration.SuretyServices
{
    [ServiceContract]
    public interface ISuretyIntegrationService
    {
        [OperationContract]
        List<SuretyDTO> GetSuretiesByEndorsementIdPrefixIdModuleType(int endorsementId, int prefixId, ModuleType moduleType);

        [OperationContract]
        List<SuretyDTO> GetRisksSuretyByInsuredId(int insuredId);

        [OperationContract]
        List<SuretyDTO> GetRisksSuretyBySuretyIdPrefixId(int suretyId, int prefixId);

        [OperationContract]
        SuretyDTO GetSuretyByRiskIdPrefixIdModuleType(int riskId, int prefixId, ModuleType moduleType);

        [OperationContract]
        List<SuretyDTO> GetRisksBySurety(string description);
    }
}
