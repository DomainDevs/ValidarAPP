using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// PaymentRequestDTO
    /// </summary>
    [DataContract]
    public class PaymentRequestDTO
    {
        [DataMember]
        public int PaymentRequestId { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public int PaymentRequestNumber { get; set; }
        [DataMember]
        public string ConceptSourceDescription { get; set; }
        [DataMember]
        public DateTime PaymentDate { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public int ClaimNumber { get; set; }
        [DataMember]
        public int ConceptSourceId { get; set; }
        [DataMember]
        public int RiskNumber { get; set; }
        [DataMember]
        public int CoverageNumber { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public int ClaimId { get; set; }
        [DataMember]
        public int SubClaim { get; set; }
        [DataMember]
        public int ClaimCoverageCd { get; set; }
        [DataMember]
        public int PaymentVoucherConceptCd { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}