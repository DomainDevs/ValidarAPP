using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class CollectItemPolicyDTO 
    {
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public string PrefixDescription { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int Endorsement { get; set; }
        [DataMember]
        public int QuoteNum { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public int CollectItemCode { get; set; }
        [DataMember]
        public int Rows { get; set; }
        [DataMember]
        public int TechnicalTransaction { get; set; }
    }

}
