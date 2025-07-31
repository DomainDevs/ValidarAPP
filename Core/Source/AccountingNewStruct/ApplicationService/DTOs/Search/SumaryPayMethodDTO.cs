using System;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
   public class SumaryPayMethodDTO 
   {
       [DataMember]
       public int CollectControlCode { get; set; }
       [DataMember]
       public string Description { get; set; }
       [DataMember]  
       public double Total { get; set; }
       [DataMember]  
       public int PaymentMethodCode { get; set; }
    }
}
