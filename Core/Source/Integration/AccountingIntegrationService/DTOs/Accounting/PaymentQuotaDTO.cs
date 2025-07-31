using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
{
    [DataContract]
    public class PaymentQuotaDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public decimal Expenses { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal Discounts { get; set; }
    }
}
