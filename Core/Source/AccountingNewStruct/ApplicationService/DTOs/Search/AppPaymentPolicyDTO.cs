using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class AppPaymentPolicyDTO 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int PaymentNum { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public int CollectCode { get; set; }
    }
}
