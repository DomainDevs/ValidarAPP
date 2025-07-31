using Sistran.Company.Application.Location.FidelityServices.DTOs;
using Sistran.Core.Application.Finances.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.Finances.FidelityServices.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        /// <summary>
        /// Conversión entre entidades de profesiones
        /// </summary>
        /// <param name="issuanceOccupation">Profesión tipo core</param>
        /// <returns>Profesión</returns>
        internal static OccupationDTO CreateOccupation(IssuanceOccupation issuanceOccupation)
        {
            if (issuanceOccupation == null)
                return null;

            return new OccupationDTO
            {
                Id = issuanceOccupation.Id,
                Description = issuanceOccupation.Description
            };
        }

        /// <summary>
        /// Conversión de listas de profesiones 
        /// </summary>
        /// <param name="issuanceOccupations">Listado de profesiones tipo core</param>
        /// <returns>Listado de profesiones</returns>
        internal static List<OccupationDTO> CreateOccupations(List<IssuanceOccupation> issuanceOccupations)
        {
            List<OccupationDTO> occupationList = new List<OccupationDTO>();
            foreach (var issuanceOccupation in issuanceOccupations)
            {
                occupationList.Add(CreateOccupation(issuanceOccupation));
            }
            return occupationList;
        }

    }
}
