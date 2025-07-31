using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class BallotNotDepositedDTO 
    {
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
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
        public Decimal CashAmount { get; set; }
        [DataMember]
        public Decimal CheckAmount { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public int Rows { get; set; }
    }
}
