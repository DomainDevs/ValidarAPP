using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
    public class PolicyQuotaDTO 
   {
         [DataMember]
         public double Amount { get; set; }
         [DataMember]
         public decimal AmountOriginal { get; set; }
         [DataMember]
         public int BranchCode { get; set; }
         [DataMember]  
         public string Description { get; set; }
         [DataMember]
         public string DocumentNum  { get; set; }
         [DataMember]
         public string DocumentNumber   { get; set; }
         [DataMember]
         public int  EndorsementId { get; set; }
         [DataMember]
         public int EndoTypeCode { get; set; }
         [DataMember]
         public int IndividualId   { get; set; }
         [DataMember]
         public string Name  { get; set; }
         [DataMember]
         public int PayerId { get; set; }
         [DataMember]  
         public DateTime PayExpDate { get; set; }
         [DataMember]
         public int PaymentNum  { get; set; }
         [DataMember]
         public int PolicyId  { get; set; }
         [DataMember]
         public int PrefixCode  { get; set; }
         [DataMember]
         public int PolicyholderId { get; set; }
         [DataMember]
         public string NamePolicy  { get; set; }
         [DataMember]
         public string DocNumPolicy { get; set; }
         [DataMember]
         public string ItemId  { get; set; }
         [DataMember]
         public string Num { get; set; }
         [DataMember]
         public int ItemTypeId { get; set; }
         [DataMember]
         public int CurrencyCode { get; set; }
         [DataMember]
         public decimal ExchangeRate { get; set; }
         [DataMember]
         public int DocNumEndorsement { get; set; }
    }
}
