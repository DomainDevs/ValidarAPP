using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class VoucherConceptDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        public decimal TaxValue { get; set; }

        [DataMember]
        public decimal Retention { get; set; }

        [DataMember]
        public int CostCenter { get; set; }

        [DataMember]
        public int PaymentConceptId { get; set; }

        [DataMember]
        public string PaymentConcept { get; set; }

        [DataMember]
        public List<VoucherConceptTaxDTO> ConceptTaxes { get; set; }
    }
}
