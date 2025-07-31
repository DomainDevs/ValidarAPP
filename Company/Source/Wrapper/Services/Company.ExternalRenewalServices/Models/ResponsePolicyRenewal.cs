using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalRenewalServices.Models
{
    [DataContract]
    public class ResponsePolicyRenewal
    {
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public bool HasEvents { get; set; }
        [DataMember]
        public List<Event> Events { get; set; }
        [DataMember]
        public int TempId { get; set; }
        [DataMember]
        public int BranchCd { get; set; }
        [DataMember]
        public int PrefixCd { get; set; }
        [DataMember]
        public long DocumentNumber { get; set; }
        [DataMember]
        public int EndorsementNumber { get; set; }
        [DataMember]
        public decimal InsuredAmount { get; set; }
        [DataMember]
        public decimal PremiumAmount { get; set; }
        [DataMember]
        public decimal TaxAmount { get; set; }
        [DataMember]
        public decimal ExpensesAmount { get; set; }
    }
}
