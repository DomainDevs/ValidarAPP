using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class AutomaticDebitSummaryDTO 
    {
        
        [DataMember]
        public int Rows { get; set; }

        [DataMember]
        public int NetId { get; set; }

        [DataMember]
        public string Prefix  { get; set; }

        [DataMember]
        public decimal TotalPremium  { get; set; }

        [DataMember]
        public decimal ValueAddedTax { get; set; }

        [DataMember]
        public int CouponsNumber { get; set; }

        [DataMember]
        public int AcceptedNumber { get; set; }

        [DataMember]
        public int ErrorAcceptedNumber { get; set; }
      
        [DataMember]
        public decimal ErrorAccepted { get; set; }

        [DataMember]
        public decimal Rejections { get; set; }
         
    }

}
