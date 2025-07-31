using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class VoucherConceptTaxDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int TaxId { get; set; }
        
        [DataMember]
        public string TaxDescription { get; set; }

        [DataMember]
        public int ConditionId { get; set; }
        
        [DataMember]
        public string ConditionDescription { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string CategoryDescription { get; set; }

        [DataMember]
        public decimal Retention { get; set; }

        [DataMember]
        public decimal TaxRate { get; set; }

        [DataMember]
        public decimal TaxBaseAmount { get; set; }

        [DataMember]
        public decimal TaxValue { get; set; }

        [DataMember]
        public int AccountingConceptId { get; set; }
    }
}
