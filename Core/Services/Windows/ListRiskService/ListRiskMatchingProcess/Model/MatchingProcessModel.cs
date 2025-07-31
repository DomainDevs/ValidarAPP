using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class MatchingProcessModel
    {
        public List<PersonModel> People { get; set; }
        public List<CompanyModel> Company { get; set; }
        public List<PolicyModel> Policies { get; set; }
        public List<ClaimModel> Claims { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public ReinsuranceModel Reinsurances { get; set; }
        public CompanyListRiskModel RiskList { get; set; }
    }
}
