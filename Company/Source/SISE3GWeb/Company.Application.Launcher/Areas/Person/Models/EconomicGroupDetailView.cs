using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class EconomicGroupView
    {
       
        public int EconomicGroupId { get; set; }
     
        public string EconomicGroupName { get; set; }
  
        public int TributaryIdType { get; set; }

        public string TributaryIdNo { get; set; }
     
        public int VerifyDigit { get; set; }
  
        public DateTime EnteredDate { get; set; }
 
        public decimal OperationQuoteAmount { get; set; }
    
        public bool Enabled { get; set; }
   
        public DateTime? DeclinedDate { get; set; }
    
        public int UserId { get; set; }

        public List<EconomicGroupDetailView> EconomicGroupDetail { get; set; }
    }
}