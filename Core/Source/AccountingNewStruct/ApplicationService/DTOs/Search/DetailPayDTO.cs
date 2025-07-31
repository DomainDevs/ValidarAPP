using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class DetailPayDTO 
    {
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public string PaymentMethodTypeDescription { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public double ExchangeRate { get; set; }
        [DataMember]
        public string DatePayment { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public string IssuingBankDescription { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }

    }
}
