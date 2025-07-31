using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class ReinsurancePolicyModel
    {
        public int IssueReinsuranceId { get; set; }
        public int EndorsementId { get; set; }
        public int ReinsuranceNumber { get; set; }
        public int MovementTypeId { get; set; }
        public int BranchId { get; set; }
        public int PrefixId { get; set; }
        public int DocumentNumber { get; set; }
        public int PoliciyId { get; set; }
        public int InsuredIndividualId { get; set; }
    }
}
