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
        public int PolicyHolderId { get; set; }
        public int EndorsementId { get; set; }
        public int RiskId { get; set; }
        public int RiskStatusId { get; set; }
    }
}
