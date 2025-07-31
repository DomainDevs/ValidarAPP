using Sistran.Core.Integration.AircraftServices.DTOs;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Integration.AircraftServices
{
    [ServiceContract]
    public interface IAircraftIntegrationService
    {
        [OperationContract]
        List<AircraftOperatorDTO> GetAircraftOperators();

        [OperationContract]
        List<AircraftRegisterDTO> GetAircraftRegisters();

        [OperationContract]
        List<AircraftUseDTO> GetAircraftUses();

        [OperationContract]
        List<AircraftTypeDTO> GetAircraftTypes();

        [OperationContract]
        List<AircraftMakeDTO> GetAircraftMakes();

        [OperationContract]
        List<AicraftModelDTO> GetAircraftModelsByMakeId(int makeId);

        [OperationContract]
        AircraftDTO GetRiskAircraftByRiskId(int riskId);

        [OperationContract]
        List<AircraftDTO> GetRiskAircraftsByInsuredId(int insuredId);

        [OperationContract]
        List<AircraftDTO> GetRiskAircraftsByEndorsementId(int endorsementId);
    }
}
