using Sistran.Core.Integration.FidelityService.EEProvider.Assemblers;
using Sistran.Core.Integration.FidelityServices;
using Sistran.Core.Integration.FidelityServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.FidelityService.EEProvider
{
    public class FidelityIntegrationServiceEEProvider : IFidelityIntegrationService
    {
        public List<OccupationDTO> GetOccupations()
        {
            return DTOAssembler.CreateOccupations(DelegateService.fidelityServiceCore.GetIssuanceOccupations());
        }

        public List<FidelityDTO> GetRiskFidelitiesByEndorsementId(int endorsementId)
        {
            return DTOAssembler.CreateFidelities(DelegateService.fidelityServiceCore.GetRiskFidelitiesByEndorsementId(endorsementId));
        }

        public List<FidelityDTO> GetRiskFidelitiesByInsuredId(int insuredId)
        {
            return DTOAssembler.CreateFidelities(DelegateService.fidelityServiceCore.GetRiskFidelitiesByInsuredId(insuredId));
        }

        public FidelityDTO GetRiskFidelityByRiskId(int riskId)
        {
            return DTOAssembler.CreateFidelity(DelegateService.fidelityServiceCore.GetRiskFidelityByRiskId(riskId));
        }
    }
}
