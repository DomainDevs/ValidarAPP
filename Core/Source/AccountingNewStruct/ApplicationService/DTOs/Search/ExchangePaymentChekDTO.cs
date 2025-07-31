using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class ExchangePaymentCheckDTO 
    {
        [DataMember]
        public int PaymentId { get; set; }
        [DataMember]
        public int CollectId { get; set; }
        [DataMember]
        public int CollectStatus { get; set; }
        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public int? PaymentTicket { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public DateTime DateCheck { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public Decimal ExchangeRate { get; set; }
        [DataMember] 
        public string Holder { get; set; }
        [DataMember] 
        public Decimal IncomeAmount { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public int BankId { get; set; }
        [DataMember] 
        public int PaymentMethodTypeCode { get; set; }
        [DataMember] 
        public string StateCheck { get; set; }
        [DataMember]
        public string StateCheckId { get; set; }


                       
    }
}
