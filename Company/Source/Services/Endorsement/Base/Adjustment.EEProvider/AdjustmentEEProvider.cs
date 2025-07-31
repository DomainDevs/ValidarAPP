using Sistran.Company.AdjustmentEndorsement.EEProvider.Assemblers;
using Sistran.Company.Application.Adjustment;
using Sistran.Company.Application.Adjustment.DTO;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.AdjustmentEndorsement.EEProvider
{
    public class CiaAdjustmentEndorsemntEEProvider : ICiaAdjustmentEndorsement
    {
      

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            Endorsement endorsement = DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId);
            return DTOAssembler.CreateEndorsementDTO(endorsement);
        }
    }
}
