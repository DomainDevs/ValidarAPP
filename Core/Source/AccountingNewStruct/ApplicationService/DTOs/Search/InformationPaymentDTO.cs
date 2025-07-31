using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
    public class InformationPaymentDTO 
   {
         [DataMember]
         public double Amount { get; set; }
         [DataMember]
         public string AuthorizationNumber { get; set; }
         [DataMember]
         public int BankCode { get; set; }
         [DataMember]
         public string BankDescription { get; set; }
         [DataMember]
         public int CollectCode { get; set; }
         [DataMember]
         public int CollectConceptCode { get; set; }
         [DataMember]
         public string CollectConceptDescription { get; set; }
         [DataMember]
         public int BranchCode { get; set; }
         [DataMember]
         public string BranchDescription { get; set; } 
         [DataMember]
         public string CardDate { get; set; }
         [DataMember]
         public int CreditCardTypeCode { get; set; }
         [DataMember]
         public string DatePayment { get; set; }
         [DataMember]
         public string Description { get; set; } //CardDescription
         [DataMember]
         public int CurrencyCode { get; set; }
         [DataMember]
         public string CurrencyDescription { get; set; }
         [DataMember]
         public string DocumentNumber { get; set; }
         [DataMember]
         public double ExchangeRate { get; set; }
         [DataMember]
         public int PaymentCode { get; set; }
         [DataMember]
         public string StatusDescription { get; set; }
         [DataMember]
         public double IncomeAmount { get; set; }
         [DataMember]
         public int IndividualId { get; set; }
         [DataMember]
         public int IssuingAccountNumber { get; set; }
         [DataMember]
         public string Name { get; set; }
         [DataMember]
         public int StatusPayment { get; set; }
         [DataMember]
         public string Holder { get; set; }
         [DataMember]
         public int PaymentMethodTypeCode { get; set; }
         [DataMember]
         public string ReceivingAccountNumber { get; set; }
         [DataMember]
         public int ReceivingBankCode { get; set; }
         [DataMember]
         public string SerialNumber { get; set; }
         [DataMember]
         public string SerialVoucher { get; set; }
         [DataMember]
         public double Taxes { get; set; }
         [DataMember]
         public double Commission { get; set; }
         [DataMember]
         public string Voucher { get; set; }
         [DataMember]
         public int ValidMonth { get; set; }
         [DataMember]
         public int ValidYear { get; set; }
        [DataMember]
        public int TechnicalTransaction { get; set; }

    }
}
