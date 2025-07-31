using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Reversion
{
    [DataContract]
    public class QuotaPremiumDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int TechnicalTransaction { get; set; }
        [DataMember]
        public DateTime AccountingDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal LocalAmount { get; set; }
        [DataMember]
        public decimal LocalAmountCommis { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }
    }
}
