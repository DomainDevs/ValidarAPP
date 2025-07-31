using Sistran.Core.Application.Finances.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;

namespace Sistran.Core.Application.Finances.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Occupations
        /// <summary>
        /// Conversión de listado de entidad Occupation a listado de IssuanceOccupation
        /// </summary>
        /// <param name="occupations">Listado de profesiones</param>
        /// <returns>Listado de profesiones</returns>
        public static List<IssuanceOccupation> CreateOccupations(BusinessCollection businessCollection)
        {
            List<IssuanceOccupation> issuanceOccupationsList = new List<IssuanceOccupation>();
            foreach (Occupation occupation in businessCollection)
            {
                issuanceOccupationsList.Add(CreateOccupation(occupation));
            }
            return issuanceOccupationsList;
        }

        /// <summary>
        /// Conversión entre la entidad Occupation a InsuanceOccupation
        /// </summary>
        /// <param name="occupation">Modelo de profesión</param>
        /// <returns>Objeto para profesión</returns>
        public static IssuanceOccupation CreateOccupation(Occupation occupation)
        {
            return new IssuanceOccupation
            {
                Id = occupation.OccupationCode,
                Description = occupation.Description,
                SmallDescription = occupation.SmallDescription
            };
        }
        #endregion
    }
}

