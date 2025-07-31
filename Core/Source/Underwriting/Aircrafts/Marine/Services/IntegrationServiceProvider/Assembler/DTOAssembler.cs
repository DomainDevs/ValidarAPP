using Sistran.Core.Application.Marines.MarineBusinessService.Models;
using Sistran.Core.Integration.MarineServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.MarineServices.EEProvider.Assembler
{
    public class DTOAssembler
    {
        internal static List<AirCraftDTO> CreateCompanyMarines(List<Marine> marines)
        {
            List<AirCraftDTO> airCraftsDTO = new List<AirCraftDTO>();

            foreach (Marine marine in marines)
            {
                airCraftsDTO.Add(CreateMarine(marine));
            }

            return airCraftsDTO;
        }

        internal static AirCraftDTO CreateMarine(Marine marine)
        {
            return new AirCraftDTO
            {
                UseId = marine.UseId,
                Use = marine.UseDescription,
                OperatorId = marine.OperatorId,
                RegisterNumber = marine.NumberRegister,
                RiskId = marine.Risk.RiskId,
                Risk = marine.Risk.Description,
                CoveredRiskType = (int)marine.Risk.CoveredRiskType,
                EndorsementId = marine.Risk.Policy.Endorsement.Id,
                InsuredId = marine.Risk.MainInsured.IndividualId,
                PolicyId = marine.Risk.Policy.Id,
                PolicyDocumentNumber = marine.Risk.Policy.DocumentNumber,
                InsuredAmount = marine.Risk.AmountInsured,
                RiskNumber = marine.Risk.Number
            };
        }


    }
}
