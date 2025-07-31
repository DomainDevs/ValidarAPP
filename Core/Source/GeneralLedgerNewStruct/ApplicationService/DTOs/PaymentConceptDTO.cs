using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class PaymentConceptDTO
    {
        [DataMember]
        public int PaymentConceptId { get; set; }
        [DataMember]
        public string PaymentConceptDescription { get; set; }
        [DataMember]
        public int GeneralLedgerId { get; set; }
        [DataMember]
        public string AccountingNumber { get; set; }
        [DataMember]
        public string AccountingName { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
    }
}
