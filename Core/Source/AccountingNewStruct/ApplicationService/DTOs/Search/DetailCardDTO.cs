using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class DetailCardDTO 
    {
        [DataMember]
        public string CreditCardDescription { get; set; }
        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public string Voucher { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public double Tax { get; set; }
        [DataMember]
        public int Rows { get; set; }
    }
}
