using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    public class StatusPaymentDTO 
    {
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string MethodDescription { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        
    }
}
