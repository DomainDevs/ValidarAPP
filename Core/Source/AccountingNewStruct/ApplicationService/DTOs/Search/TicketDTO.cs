using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class TicketDTO 
    {
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int BankCode { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int State { get; set; }
    }
}
