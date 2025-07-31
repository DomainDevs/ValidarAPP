using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class DetailBallotNotDepositedDTO 
    {
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public string PaymentMethodTypeName { get; set; }
        [DataMember]
        public int IssuingBankCode { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public int ReceiptNumber { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public Decimal ExchangeRate { get; set; }
        [DataMember]
        public Decimal IncomeAmount { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public DateTime CheckDate { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int PaymentTicketItemCode { get; set; }
        [DataMember]
        public int Rows { get; set; }
        //***************************************************************
        //BE
        //DDELGADO
        //Campo de correlativo solo existe en 2G
        [DataMember]
        public int Correlative { get; set; }
    }
}
