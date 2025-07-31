using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseTempRiskSuretyPost
    {
        [DataMember]
        public int TempId { get; set; }
        [DataMember]
        public int RiskId { get; set; }
        [DataMember]
        public DateTime? IssueDate { get; set; }
        [DataMember]
        public DateTime? ContractDate { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public bool ChkContractDate { get; set; }
        [DataMember]
        public bool ChkContractFinalyDate { get; set; }
    }

}
