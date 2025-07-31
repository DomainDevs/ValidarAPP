using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    class MoveMatchPerson
    {
        public IEnumerable<PolicyModel> Policies { get; set; }
        public IEnumerable<ClaimModel> Claims { get; set; }
        public IEnumerable<PaymentModel> Payments { get; set; }
        public ReinsuranceModel Reinsurances { get; set; }
    }
}
