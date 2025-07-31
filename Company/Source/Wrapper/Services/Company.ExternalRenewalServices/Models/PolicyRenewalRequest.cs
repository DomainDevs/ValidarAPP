using System.Runtime.Serialization;

namespace Sistran.Company.ExternalRenewalServices.Models
{
    [DataContract]
    public class PolicyRenewalRequest
    {
        [DataMember]
        public int BranchCd { get; set; }
        [DataMember]
        public int PrefixCd { get; set; }
        [DataMember]
        public long DocumentNumber { get; set; }
        [DataMember]
        public string AccountName { get; set; }
    }
}
