using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest
{
    /// <summary>
    /// Concepto Comprobante
    /// </summary>
    [DataContract]
    public class VoucherConceptTax
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int TaxId { get; set; }

        [DataMember]
        public int ConditionId { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public decimal Retention { get; set; }

        [DataMember]
        public decimal TaxRate { get; set; }

        [DataMember]
        public decimal TaxBaseAmount { get; set; }

        [DataMember]
        public decimal TaxValue { get; set; }
    }
}
