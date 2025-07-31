using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
    public class CardVoucherDTO 
   {
         [DataMember]
         public decimal Amount { get; set; }
         [DataMember]
         public decimal IncomeAmount { get; set; }
         [DataMember]
         public int CollectCode { get; set; }
         [DataMember]
         public int BranchCode { get; set; }
         [DataMember]
         public string CardDate { get; set; }
         [DataMember]
         public string CardDescription { get; set; }
         [DataMember]
         public int CreditCardTypeCode { get; set; }
         [DataMember]
         public int CurrencyCode { get; set; }
         [DataMember]
         public string CurrencyDescription { get; set; }
         [DataMember]
         public string Description { get; set; } //BranchDescription
         [DataMember]
         public string DocumentNumber { get; set; }
         [DataMember]
         public int PaymentCode { get; set; }
         [DataMember]
         public DateTime PaymentDate { get; set; }
         [DataMember]
         public int Status { get; set; }
         [DataMember]
         public string StatusDescription { get; set; }
         [DataMember]
         public decimal Taxes { get; set; }
         [DataMember]
         public decimal Retention { get; set; }
         [DataMember]
         public decimal Commission { get; set; }
         [DataMember]
         public string Voucher { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

    }
}
