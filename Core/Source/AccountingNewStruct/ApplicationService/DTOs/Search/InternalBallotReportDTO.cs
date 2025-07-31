using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class InternalBallotReportDTO 
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
        public int IssuingBankCode { get; set; }
        [DataMember]
        public string IssuingBankName { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public string TinyDescription { get; set; }
        [DataMember]
        public Decimal CheckAmount { get; set; }
        [DataMember]
        public Decimal IncomeAmount { get; set; }
        [DataMember]
        public Decimal ExchangeRate { get; set; }
        [DataMember]
        public int Rows { get; set; }

    }
}
