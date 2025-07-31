using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class DetailPaymentBallotSearchDTO
    {
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public Decimal IncomeAmount { get; set; }
        [DataMember]
        public Decimal ExchangeRate { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int IssuingBankCode { get; set; }
        [DataMember]
        public string IssuingBankName { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }
        [DataMember]
        public DateTime DatePayment { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public int CreditCardTypeCode { get; set; }
        [DataMember]
        public int ValidYear { get; set; }
        [DataMember]
        public int ValidMonth { get; set; }
        [DataMember]
        public string AuthorizationNumber { get; set; }
        [DataMember]
        public string Voucher { get; set; }
        [DataMember]
        public int ReceivingBankCode { get; set; }
        [DataMember]
        public string ReceivingBankName { get; set; }
        [DataMember]
        public string ReceivingAccountNumber { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public string SerialVoucher { get; set; }
        [DataMember]
        public int Rows { get; set; }

    }
}
