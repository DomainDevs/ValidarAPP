using System.Runtime.Serialization;

namespace Sistran.Company.ExternalRenewalServices.Models
{
    [DataContract]
    public class PolicyRenewalByAllianceRequest
    {
        [DataMember]
        public int AgentCd { get; set; }
        [DataMember]
        public int AgentTypeCd { get; set; }
        [DataMember]
        public int AllianceCode { get; set; }
        [DataMember]
        public string ProposalNumber { get; set; }
        [DataMember]
        public string AccountName { get; set; }
    }
}
