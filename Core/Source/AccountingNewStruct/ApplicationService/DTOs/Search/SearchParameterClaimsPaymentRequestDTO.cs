using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;

using System.Runtime.Serialization;




namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchParameterClaimsPaymentRequestDTO 
    {
        [DataMember]
        public int SearchBy { get; set; }
        [DataMember]
        public BranchDTO Branch { get; set; }
        [DataMember]
        public string RequestNumber { get; set; }
        [DataMember]
        public string ClaimNumber { get; set; }
        [DataMember]
        public string DateTo { get; set; }
        [DataMember]
        public string DateFrom { get; set; }
        [DataMember]
        public PrefixDTO Prefix { get; set; }
        [DataMember]
        public ConceptSourceDTO PaymentSource { get; set; }
        [DataMember]
        public string VoucherNumber { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
    
    }
}
