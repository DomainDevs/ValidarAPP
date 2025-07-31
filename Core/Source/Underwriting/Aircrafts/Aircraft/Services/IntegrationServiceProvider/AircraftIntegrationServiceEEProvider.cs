using Sistran.Core.Integration.AircraftServices;
using Sistran.Core.Integration.AircraftServices.DTOs;
using Sistran.Core.Integration.AirCraftServices.EEProvider.Assemblers;
using System.Collections.Generic;

namespace Sistran.Core.Integration.AirCraftServices.EEProvider
{
    public class AircraftIntegrationServiceEEProvider : IAircraftIntegrationService
    {
        public List<AircraftMakeDTO> GetAircraftMakes()
        {
            return DTOAssembler.CreateAircraftMakes(DelegateService.aircraftBusinessService.GetMakes());
        }

        public List<AicraftModelDTO> GetAircraftModelsByMakeId(int makeId)
        {
            return DTOAssembler.CreateAircraftModels(DelegateService.aircraftBusinessService.GetModelByMakeId(makeId));
        }

        public List<AircraftOperatorDTO> GetAircraftOperators()
        {
            return DTOAssembler.CreateAircraftOperators(DelegateService.aircraftBusinessService.GetOperators());
        }

        public List<AircraftRegisterDTO> GetAircraftRegisters()
        {
            return DTOAssembler.CreateAircraftRegisters(DelegateService.aircraftBusinessService.GetRegisters());
        }

        public List<AircraftTypeDTO> GetAircraftTypes()
        {
            return DTOAssembler.CreateAircraftTypes(DelegateService.aircraftBusinessService.GetAircraftTypes(10));
        }

        public List<AircraftUseDTO> GetAircraftUses()
        {
            return DTOAssembler.CreateAircraftUses(DelegateService.aircraftBusinessService.GetUseByusessByPrefixId(10));
        }

        public AircraftDTO GetRiskAircraftByRiskId(int riskId)
        {
            return DTOAssembler.CreateAircraft(DelegateService.aircraftBusinessService.GetRiskAircraftByRiskId(riskId));
        }

        public List<AircraftDTO> GetRiskAircraftsByEndorsementId(int endorsementId)
        {
            return DTOAssembler.CreateAircrafts(DelegateService.aircraftBusinessService.GetRiskAircraftsByEndorsementId(endorsementId));
        }

        public List<AircraftDTO> GetRiskAircraftsByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateAircrafts(DelegateService.aircraftBusinessService.GetRiskAircraftsByInsuredId(insuredId));
        }
    }
}
