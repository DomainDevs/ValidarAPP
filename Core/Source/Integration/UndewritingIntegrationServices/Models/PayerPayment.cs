using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.Models
{
    [DataContract]
    public class PayerPayment
    {
        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int PolicyId { get; set; }

        [DataMember]
        public int PayerId { get; set; }

        [DataMember]
        public int PaymentNum { get; set; }

        [DataMember]
        public DateTime PayExpDate { get; set; }

        [DataMember]
        public decimal PaymentPercentage { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public DateTime AgtPayExpDate { get; set; }

        [DataMember]
        public int PayerPaymentId { get; set; }

        [DataMember]
        public decimal LocalAmount { get; set; }

        [DataMember]
        public decimal MainAmount { get; set; }

        [DataMember]
        public decimal MainLocalAmount { get; set; }

        [DataMember]
        public int PaymentState { get; set; }
    }
}
