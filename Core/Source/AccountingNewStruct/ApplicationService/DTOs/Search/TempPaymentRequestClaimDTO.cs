using System;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
    public class TempPaymentRequestClaimDTO 
   {
         [DataMember]
         public int TempClaimPaymentCode { get; set; }
         [DataMember]
         public int TempImputationCode { get; set; }
         [DataMember]
         public int  PaymentRequestCode { get; set; }//
         [DataMember]  
         public int  BranchCode { get; set; } //
         [DataMember]
         public string BranchDescription  { get; set; } //
         [DataMember]
         public int PrefixCode   { get; set; }//
         [DataMember]
         public string PrefixDescription { get; set; }//
         [DataMember]
         public int RequestType  { get; set; }//
         [DataMember]
         public string PaymentSourceDescription { get; set; }//
         [DataMember]  
         public string ClaimNumber { get; set; } //
         [DataMember]
         public int ClaimCode { get; set; } //
         [DataMember]
         public string BeneficiaryId  { get; set; }//
         [DataMember]
         public string DocNumPayBeneficiary  { get; set; } //
         [DataMember]
         public string NamePayBeneficiary  { get; set; }//
         [DataMember]
         public int CurrencyCode { get; set; } //
         [DataMember]
         public string CurrencyDescription  { get; set; } //
         [DataMember]
         public DateTime RegistrationDate  { get; set; }//
         [DataMember]
         public DateTime EstimationDate { get; set; }//
         [DataMember]
         public int BussinessType { get; set; }//
         [DataMember]
         public string BusinessTypeDescription { get; set; } //
         [DataMember]
         public decimal IncomeAmount { get; set; }//
         [DataMember]
         public decimal Amount { get; set; }//
         [DataMember]
         public decimal ExchangeRate { get; set; }//
         [DataMember]
         public int? PaymentNum { get; set; }//
         [DataMember]
         public DateTime? PaymentExpirationDate { get; set; }
         [DataMember]
         public string Branch_Prefix_Request { get; set; }
         [DataMember]
         public string DocumentNumber_Beneficiary { get; set; }
         [DataMember]
         public string Branch_Request { get; set; }
         [DataMember]
         public string PaymentRequestNumber { get; set; }

        

    }
}
