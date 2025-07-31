using Sistran.Core.Application.BaseEndorsementService.DTOs;
using Sistran.Core.Application.BaseEndorsementService.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;
using System.Linq;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.BaseEndorsementService.EEProvider.DAOs
{
    public class EndorsementTypeDAO
    {
        public List<EndorsementTypeDTO> GetModificationType()
        {
            List<PARAMEN.EndorsementModificationType> endorsementModificationTypes = DataFacadeManager.Instance.GetDataFacade().List(typeof(PARAMEN.EndorsementModificationType), null).Cast<PARAMEN.EndorsementModificationType>().ToList();

            if (endorsementModificationTypes != null)
            {
                return ModelAssembler.CreateEndorsementModificationTypes(endorsementModificationTypes);
            }
            else
            {
                return null;
            }
        }
    }
}
