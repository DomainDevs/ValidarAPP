using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.ModelsMapper
{
    public class CompanyPolicyMapper
    {
        public ISSEN.Policy policy { get; set; }
        public ISSEN.CoPolicy coPolicy { get; set; }
        public ISSEN.Endorsement endorsement { get; set; }
        public ISSEN.EndorsementPayer endorsementPayer { get; set; }
        public ISSEN.TempPolicyControl tempPolicyControl { get; set; }
        public ISSEN.EndorsementOperation entityEndorsementOperation { get; set; }
    }
}
