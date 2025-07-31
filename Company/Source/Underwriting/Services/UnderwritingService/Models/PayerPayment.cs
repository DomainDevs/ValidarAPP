using Sistran.Company.Application.UnderwritingServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class PayerPayment
    {
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public int PaymentNumber { get; set; }
        [DataMember]
        public ComponentTypePayerPayment ComponentType { get; set; }
        [DataMember]
        public decimal Porcentage { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public DateTime PaymentDate { get; set; }
    }
}
