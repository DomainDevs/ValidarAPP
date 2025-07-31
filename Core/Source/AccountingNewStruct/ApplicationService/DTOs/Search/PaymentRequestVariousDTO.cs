using System;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
   public class PaymentRequestVariousDTO : PaymentRequestClaimDTO 
   {
       [DataMember]
       public int StatusPayment { get; set; }
       [DataMember]
       public string Branch_Request { get; set; }
       
    }
}
