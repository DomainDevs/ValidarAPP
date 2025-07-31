using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class PolicyRiskDTO
    {        
        [DataMember]
        public decimal DocumentNumber { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public string PrefixDescription { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public int RiskId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
    }
}
