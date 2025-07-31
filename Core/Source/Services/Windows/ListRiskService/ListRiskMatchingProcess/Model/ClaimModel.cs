using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class ClaimModel
    {
        public int ClaimCd { get; set; }
        public int ClaimBranchCd { get; set; }
        public int PolicyId { get; set; }
        public int EndorsementId { get; set; }
        public int IndividualId { get; set; }
        public int BusinessTypeCd { get; set; }
        public int ClaimNumber { get; set; }
        public int PrefixCd { get; set; }
        public decimal DocumentNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int EstimationTypeCd { get; set; }
        public int EstimationTypeStatusCd { get; set; }
        public int EstimationTypeStatusReasonCd { get; set; }
        public decimal EstimationAmount { get; set; }
        public decimal EstimateAmountAccumulate { get; set; }
    }
}
