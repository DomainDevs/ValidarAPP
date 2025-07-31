using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class PolicyModel
    {
        public int PolicyId { get; set; }
        public decimal DocumentNumber { get; set; }
        public int BranchId { get; set; }
        public int PrefixId { get; set; }
        public DateTime IssueDate { get; set; }
        public int PolicyHolderId { get; set; }
        public DateTime CurrentFrom { get; set; }
        public DateTime CurrentTo { get; set; }
        public int EndorsementId { get; set; }
        public int RiskId { get; set; }
        public int RiskStatusId { get; set; }
        public int InsuredId { get; set; }
        public int BeneficiaryId { get; set; }
        public int AgentId { get; set; }
    }
}
