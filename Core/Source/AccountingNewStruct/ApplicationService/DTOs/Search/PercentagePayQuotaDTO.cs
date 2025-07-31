using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
   [DataContract]
    public class PercentagePayQuotaDTO 
   {
        [DataMember]
         public int ParameterId { get; set; }
         [DataMember]
         public double PercentageParameter { get; set; }
         [DataMember]  
         public double AmountParameter { get; set; }
     
    }
}
