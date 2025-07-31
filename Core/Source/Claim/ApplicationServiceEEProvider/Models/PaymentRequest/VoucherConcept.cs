
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    /// <summary>
    /// Concepto Comprobante
    /// </summary>
    [DataContract]
    public class VoucherConcept
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
        public PaymentConcept PaymentConcept { get; set; }

        [DataMember]
        public List<VoucherConceptTax> VoucherConceptTaxes { get; set; }
    }
}
