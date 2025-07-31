using Sistran.Core.Application.BaseEndorsementService.DTOs;
using Sistran.Core.Application.BaseEndorsementService.EEProvider.DAOs;
using System.Collections.Generic;
namespace Sistran.Core.Application.BaseEndorsementService.EEProvider.Business
{
    public class EndorsementTypeBusiness
    {
        /// <summary>
        /// Gets the type of the modification.
        /// </summary>
        /// <returns></returns>
        public static List<EndorsementTypeDTO> GetModificationType()
        {
            EndorsementTypeDAO endorsementTypeDAO = new EndorsementTypeDAO();
            return endorsementTypeDAO.GetModificationType();
        }
    }
}
