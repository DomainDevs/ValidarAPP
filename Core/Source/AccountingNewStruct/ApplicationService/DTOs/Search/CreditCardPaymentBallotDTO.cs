using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
    public class CreditCardPaymentBallotDTO 
    {
        [DataMember]
        public int CreditCardTypeCode { get; set; }
        [DataMember]
        public string CreditCardDescription { get; set; }
        [DataMember]
        public int BankCode { get; set; }
        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public DateTime? RegisterDate { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int Rows { get; set; }
        [DataMember]
        public Decimal Taxes { get; set; }
        [DataMember]
        public Decimal Commission { get; set; }
        [DataMember]
        public int BranchCode { get; set; }              
        [DataMember]
        public string BranchDescription { get; set; }                       
    }
}
