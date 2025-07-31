using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    public class UtilityDetailsModelsView
    {
        public int Id { get; set; }
        
        public int FormUtilitys { get; set; }
        
        public string Description { get; set; }
      
        public bool Enabled { get; set; }
        
        public int UtilitysTypeCd { get; set; }
        
        public int UtilitysSummaryCd { get; set; }
    }
}