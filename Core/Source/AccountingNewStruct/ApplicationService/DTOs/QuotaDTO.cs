using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class QuotaDTO
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime ExpirationDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Percentage { get; set; }
    }
}
