using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class DetailCheckDTO 
    {
        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public string DatePayment { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public int Rows { get; set; }
    }

}
