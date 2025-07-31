using Sistran.Core.Application.Finances.FidelityServices.Models;
using Sistran.Core.Application.Finances.Models;
using Sistran.Core.Integration.FidelityServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.FidelityService.EEProvider.Assemblers
{
    public static class DTOAssembler
    {
        internal static FidelityDTO CreateFidelity(FidelityRisk fidelityRisk)
        {
            return new FidelityDTO
            {
                CommercialClassId = fidelityRisk.Risk.RiskActivity.Id,
                CommercialClassDescription = fidelityRisk.Risk.RiskActivity.Description,
                OccupationId = fidelityRisk.IdOccupation,
                OccupationDescription = fidelityRisk.Description,
                Description = fidelityRisk.Description,
                DiscoveryDate = fidelityRisk.DiscoveryDate,
                RiskId = fidelityRisk.Risk.RiskId,
                Risk = fidelityRisk.Risk.Description,
                CoveredRiskType = (int)fidelityRisk.Risk.CoveredRiskType,
                EndorsementId = fidelityRisk.Risk.Policy.Endorsement.Id,
                InsuredId = fidelityRisk.Risk.MainInsured.IndividualId,
                PolicyId = fidelityRisk.Risk.Policy.Id,
                PolicyDocumentNumber = fidelityRisk.Risk.Policy.DocumentNumber,
                InsuredAmount = fidelityRisk.Risk.AmountInsured,
                RiskNumber = fidelityRisk.Risk.Number
            };
        }

        internal static List<FidelityDTO> CreateFidelities(List<FidelityRisk> fidelityRisks)
        {
            List<FidelityDTO> fidelitiesDTO = new List<FidelityDTO>();

            foreach (FidelityRisk fidelityRisk in fidelityRisks)
            {
                fidelitiesDTO.Add(CreateFidelity(fidelityRisk));
            }

            return fidelitiesDTO;
        }

        internal static OccupationDTO CreateOccupation(IssuanceOccupation occupation)
        {
            return new OccupationDTO
            {
                Id = occupation.Id,
                Description = occupation.Description
            };
        }

        internal static List<OccupationDTO> CreateOccupations(List<IssuanceOccupation> occupations)
        {
            List<OccupationDTO> occupationsDTO = new List<OccupationDTO>();

            foreach (IssuanceOccupation occupation in occupations)
            {
                occupationsDTO.Add(CreateOccupation(occupation));
            }

            return occupationsDTO;
        }
    }
}
