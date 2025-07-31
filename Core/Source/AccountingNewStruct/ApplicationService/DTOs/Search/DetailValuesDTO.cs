using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{   
    [DataContract]
    public class DetailValuesDTO 
    {
        [DataMember]
        public int PaymentTypeId { get; set; }
        [DataMember]
        public string PaymentTypeDescription { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double Exchange { get; set; }
        [DataMember]
        public double LocalAmount { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string Voucher { get; set; }
        [DataMember]
        public int CardType { get; set; }
        [DataMember]
        public string CardTypeName { get; set; }
        [DataMember]
        public string AuthorizationNumber { get; set; }
        [DataMember]
        public string ValidThruMonth { get; set; }
        [DataMember]
        public string ValidThruYear { get; set; }
        [DataMember]
        public int IssuingBankId { get; set; }
        [DataMember]
        public string IssuinBankName { get; set; }
        [DataMember]
        public string IssuingBankAccountNumber { get; set; }
        [DataMember]
        public string IssuerName { get; set; }
        [DataMember]
        public int RecievingBankId { get; set; }
        [DataMember]
        public string RecievingBankAccountNumber { get; set; }
        [DataMember]
        public string SerialVoucher { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public int PaymentStatus { get; set; }
        [DataMember]
        public double Tax { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public int Rows { get; set; }
    }
}

