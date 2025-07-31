using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.Models
{
    [DataContract]
    public class PayerPaymentComp
    {
        [DataMember]
        public int PayerPaymentCompId { get; set; }

        [DataMember]
        public int PayerPaymentId { get; set; }

        [DataMember]
        public int ComponentCode { get; set; }

        [DataMember]
        public decimal PaymentPercentage { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public decimal MainAmount { get; set; }

        [DataMember]
        public decimal MainLocalAmount { get; set; }

        [DataMember]
        public string TinyDescription { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }
    }
}
