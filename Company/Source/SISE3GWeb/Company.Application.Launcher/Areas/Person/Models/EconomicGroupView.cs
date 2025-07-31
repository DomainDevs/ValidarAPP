using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class EconomicGroupDetailView
    {
       
        public int EconomicGroupId { get; set; }
     
        public int IndividualId { get; set; }
  
        public bool Enabled { get; set; }

        public DateTime DeclinedDate { get; set; }

        public string Description { get; set; }
    }
}