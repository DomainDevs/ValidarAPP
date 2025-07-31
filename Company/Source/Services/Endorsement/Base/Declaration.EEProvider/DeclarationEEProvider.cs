using Sistran.Company.Application.BaseEndorsementService.EEProvider;
using Sistran.Company.Application.Declaration;
using Sistran.Company.Application.Declaration.DTO;
using Sistran.Company.DeclarationEndorsement.EEProvider.Assembler;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.DeclarationEndorsement.EEProvider
{
    public class CiaDeclarationEndorsementEEProvider : ICiaDeclarationEndorsement
    {
        public EndorsementDTO GetTemporalEndorsementByPolicyId( int policyId) {
            Endorsement endorsement = DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId);
            return DTOAssembler.CreateEndorsementDTO(endorsement);
        }
    }
}
