using Sistran.Company.Application.Adjustment.DTO;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.AdjustmentEndorsement.EEProvider.Assemblers
{
  public  class DTOAssembler
    {
        public static EndorsementDTO CreateEndorsementDTO(Endorsement endorsement)
        {

            if (endorsement == null)
            {
                return null;
            }

            EndorsementDTO endorsementDTO = new EndorsementDTO()
            {
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo,
                EndorsementType = endorsement.EndorsementType,
                IdEndorsement = endorsement.Id,
                IsCurrent = endorsement.IsCurrent,
                Number = endorsement.Number,
                PolicyNumber = endorsement.PolicyId,
                TemporalId = endorsement.TemporalId
            };

            return endorsementDTO;

        }
    }
}
