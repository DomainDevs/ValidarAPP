using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class InternalBallotCardReportDTO 
    {
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public string PaymentMethodTypeName { get; set; }
        [DataMember]
        public int CreditCardTypeCode { get; set; }
        [DataMember]
        public string CreditCardTypeName { get; set; }
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
        public int UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public DateTime? RegisterDate { get; set; }
        [DataMember]
        public Decimal CashAmount { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public Decimal TaxAmount { get; set; }
        [DataMember]
        public Decimal CommissionAmount { get; set; }
        [DataMember]
        public int IssuingBankCode { get; set; }
        [DataMember]
        public string IssuingBankName { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }
        [DataMember]
        public string VoucherNumber { get; set; }
        [DataMember]
        public string CardNumber { get; set; }
        [DataMember]
        public DateTime CardDate { get; set; }
        [DataMember]
        public string AuthorizationNumber { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public string TinyDescription { get; set; }
        [DataMember]
        public Decimal CardAmount { get; set; }
        [DataMember]
        public Decimal IncomeAmount { get; set; }
        [DataMember]
        public Decimal ExchangeRate { get; set; }
        [DataMember]
        public Decimal CardTaxAmount { get; set; }
        [DataMember]
        public Decimal CardCommissionAmount { get; set; }
        [DataMember]
        public int Rows { get; set; }
        //*********************************
        //BE
        //DDV
        [DataMember]
        public string LedgerAccount { get; set; }
        [DataMember]
        public int CardId { get; set; }
        

    }
}
