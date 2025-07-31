using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class ReinsuranceModel
    {
        public IEnumerable<ReinsuranceClaimModel> ReinsuranceClaims { get; set; }
        public IEnumerable<ReinsurancePolicyModel> ReinsurancePolicies { get; set; }
        public IEnumerable<ReinsurancePaymentModel> ReinsurancePayments { get; set; }
    }
}
