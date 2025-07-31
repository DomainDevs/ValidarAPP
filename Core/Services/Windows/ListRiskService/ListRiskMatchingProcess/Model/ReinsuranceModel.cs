using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class ReinsuranceModel
    {
        public List<ReinsuranceClaimModel> ReinsuranceClaims { get; set; }
        public List<ReinsurancePolicyModel> ReinsurancePolicies { get; set; }
        public List<ReinsurancePaymentModel> ReinsurancePayments { get; set; }
    }
}
