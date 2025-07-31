using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class InternalBallotCashReportDTO 
    {
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int BankCode { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public Decimal CashAmount { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public string TinyDescription { get; set; }
        [DataMember]
        public int Rows { get; set; }

    }
}
