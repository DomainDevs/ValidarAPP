using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class SearchTransactionDTO 
    {
        [DataMember]
        public int UserId {get; set;}
        [DataMember]
        public string AccountName {get; set;}
        [DataMember]
        public int BranchCode {get; set;}
        [DataMember]
        public string DescriptionBranch {get; set;}
        [DataMember]
        public int CollectConceptCode {get; set;}
        [DataMember]
        public string Description {get; set;}
        [DataMember]
        public int PaymentCode {get; set;}
        [DataMember]
        public decimal Amount {get; set;}
        [DataMember]
        public int CurrencyCode {get; set;}
        [DataMember]
        public string DescriptionCurrency {get; set;}
        [DataMember]
        public int PaymentMethodTypeCode {get; set;}
        [DataMember]
        public string AuthorizationNumber {get; set;}
        [DataMember]
        public int? IssuingBankCode {get; set;}
        [DataMember]
        public string IssuingBankName { get; set; }
        [DataMember]
        public int? ReceivingBankCode {get; set;}
        [DataMember]
        public string ReceivingBankName { get; set; }
        [DataMember]
        public decimal ExchangeRate {get; set;}
        [DataMember]
        public DateTime DatePayment {get; set;}
        [DataMember]
        public string Voucher {get; set;}
        [DataMember]
        public string DescriptionPaymentMethodType {get; set;}
        [DataMember]
        public int CollectCode {get; set;}
        [DataMember]
        public decimal PaymentsTotal {get; set;}
        [DataMember]
        public int CollectControlCode {get; set;}
        [DataMember]
        public int Status {get; set;}
        [DataMember]
        public decimal IncomeAmount {get; set;}
        [DataMember]
        public string Holder {get; set;}
        [DataMember] 
        public string DocumentNumber {get; set;}
        [DataMember]
        public int? ValidYear {get; set;}
        [DataMember]
        public int? ValidMonth {get; set;}
        [DataMember]
        public string ReceivingAccountNumber {get; set;}
        [DataMember]
        public string SerialNumber {get; set;}
        [DataMember]
        public string SerialVoucher {get; set;}
        [DataMember]
        public string IssuingAccountNumber {get; set;}
        [DataMember]
        public int? CreditCardTypeCode {get; set;}
        [DataMember]
        public DateTime AccountingDate { get; set; }        
        [DataMember]
        public int Rows { get; set; }
    }
}
