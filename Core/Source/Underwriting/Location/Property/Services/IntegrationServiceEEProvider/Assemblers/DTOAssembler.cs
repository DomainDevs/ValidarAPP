using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Core.Integration.PropertyServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.UnderwritingServices.Models;
namespace Sistran.Core.Integration.PropertyServices.EEProvider.Assemblers
{
    public static class DTOAssembler
    {
        internal static List<RiskLocationDTO> CreateRiskLocations(List<PropertyRisk> propertyRisks)
        {
            List<RiskLocationDTO> riskLocationsDTO = new List<RiskLocationDTO>();

            foreach (PropertyRisk propertyRisk in propertyRisks)
            {
                riskLocationsDTO.Add(CreateRiskLocation(propertyRisk));
            }

            return riskLocationsDTO;
        }

        internal static RiskLocationDTO CreateRiskLocation(PropertyRisk propertyRisk)
        {
            return new RiskLocationDTO
            {
                ConstructionYear = propertyRisk.ConstructionYear,
                FloorNumber = propertyRisk.FloorNumber,
                HasNomenclature = propertyRisk.HasNomenclature,
                Latitude = propertyRisk.Latitude,
                Longitude = propertyRisk.Longitude,
                FullAddress = propertyRisk.Street,
                IsDeclarative = propertyRisk.IsDeclarative,
                PML = propertyRisk.PML,
                Square = propertyRisk.Square,
                RiskAge = propertyRisk.RiskAge,
                RiskId = propertyRisk.Risk.RiskId,
                AmountInsured = propertyRisk.Risk.AmountInsured,
                CoveredRiskType = (int)propertyRisk.Risk.CoveredRiskType,
                Country = propertyRisk.City.State.Country.Description,
                CountryId = Convert.ToInt32(propertyRisk.City.State.Country.Id),
                State = propertyRisk.City.State.Description,
                StateId = Convert.ToInt32(propertyRisk.City.State.Id),
                City = propertyRisk.City.Description,
                CityId = Convert.ToInt32(propertyRisk.City.Id),
                DocumentNum = propertyRisk.Risk.Policy?.DocumentNumber,
                PolicyId = Convert.ToInt32(propertyRisk.Risk.Policy?.Id),
                EndorsementId = Convert.ToInt32(propertyRisk.Risk.Policy?.Endorsement?.Id),
                InsuredId = Convert.ToInt32(propertyRisk.Risk?.MainInsured?.IndividualId),
                RiskNumber = propertyRisk.Risk.Number
            };
        }
    }
}
