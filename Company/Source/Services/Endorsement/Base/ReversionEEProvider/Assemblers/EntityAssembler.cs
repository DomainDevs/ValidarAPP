using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Common.EndorsementWorkFlow.Entities;

namespace Sistran.Company.Application.ReversionEndorsement.EEProvider.Assemblers
{
    class EntityAssembler
    {
        public static EndorsementWorkFlow CreateEndorsementWorkFlow(int? policyID, int? endorsementID, string nroFiling, DateTime dateFiling)
        {

            return new EndorsementWorkFlow
            {
                PolicyId = policyID,
                EndorsementId = endorsementID,
                FilingNumber = nroFiling,
                FilingDate = dateFiling
            };

        }
    }
}

