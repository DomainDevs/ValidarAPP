using Sistran.Core.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Integration.TransportServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSistran.Core.Integration.TransportServices.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        internal static List<TransportDTO> CreateTransports(List<Transport> transports)
        {
            List<TransportDTO> transportsDTO = new List<TransportDTO>();

            foreach (Transport transport in transports)
            {
                transportsDTO.Add(CreateTransport(transport));
            }

            return transportsDTO;
        }

        internal static TransportDTO CreateTransport(Transport transport)
        {
            return new TransportDTO
            {
                RiskId = transport.Risk.RiskId,
                Risk = transport.Risk.Description,
                CargoTypeDescription = transport.CargoType.Description,
                CargoTypeId = transport.CargoType.Id,
                PackagingTypeDescription = transport.PackagingType.Description,
                PackagingTypeId = transport.PackagingType.Id,
                CityFromDescription = transport.CityFrom?.Description,
                CityToDescription = transport.CityTo?.Description,
                CityFromId = transport.CityFrom.Id,
                CityToId = transport.CityTo.Id,
                InsuredAmount = transport.ReleaseAmount,
                CoveredRiskType = (int)transport.Risk.CoveredRiskType,
                ViaTypeDescription = transport.ViaType.Description,
                ViaTypeId = transport.ViaType.Id,
                EndorsementId = Convert.ToInt32(transport.Risk.Policy?.Endorsement?.Id),
                PolicyId = Convert.ToInt32(transport.Risk.Policy?.Id),
                PolicyDocumentNumber = Convert.ToDecimal(transport.Risk.Policy?.DocumentNumber),
                InsuredId = Convert.ToInt32(transport.Risk.MainInsured?.IndividualId),
                RiskNumber = transport.Risk.Number
            };
        }

    }
}
