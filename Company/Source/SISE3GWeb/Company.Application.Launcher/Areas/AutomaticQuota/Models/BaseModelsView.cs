using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    public class BaseModelsView
    {
        public decimal? Value { get; set; }
        
        public int? Qualification { get; set; }
        
        public decimal? Weighted { get; set; }
        
        public decimal? Score { get; set; }
        
        public int? YearsOfConstitution { get; set; }
        
        public decimal? FinancialScore { get; set; }
    }
}