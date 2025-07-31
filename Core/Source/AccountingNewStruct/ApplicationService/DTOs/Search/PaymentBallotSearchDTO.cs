using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PaymentBallotSearchDTO 
    {
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int PaymentTicketNumber { get; set; }
        [DataMember]
        public int BankBallotCode { get; set; }
        [DataMember]
        public string BankBallotName { get; set; }
        [DataMember]
        public int BankCode { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public Decimal AmountBallot { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int PaymentBallotNumber { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public string PaymentMethodTicket { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int Rows { get; set; }

    }
}
