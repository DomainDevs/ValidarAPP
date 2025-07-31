using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class ReinsuranceClaimModel
    {
        public int ClaimReinsuranceId { get; set; }
        public int ClaimId { get; set; }
        public int ClaimModifyId { get; set; }
        public int ReinsuranceNumber { get; set; }
        public int MovementTypeId { get; set; }
    }
}
