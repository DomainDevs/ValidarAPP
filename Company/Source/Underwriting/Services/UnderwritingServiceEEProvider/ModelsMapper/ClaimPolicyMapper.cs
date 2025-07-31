using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using COMMENT = Sistran.Core.Application.Common.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.ModelsMapper
{
    public class ClaimPolicyMapper
    {
        public ISSEN.Policy policy { get; set; }
        public ISSEN.CoPolicy coPolicy { get; set; }
        public ISSEN.Endorsement endorsement { get; set; }
        public PARAMEN.BusinessType businessType { get; set; }
        public COMMENT.Currency entityCurrency { get; set; }
        public COMMENT.CoPolicyType entityCoPolicyType { get; set; }
        public PARAMEN.EndorsementType entityEndorsementType { get; set; }
    }
}
