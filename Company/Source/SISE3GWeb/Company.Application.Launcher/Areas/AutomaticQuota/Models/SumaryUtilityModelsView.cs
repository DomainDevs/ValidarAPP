using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    public class SumaryUtilityModelsView
    {
        
        public int Id { get; set; }
        
        public int Utility_Details_Cd { get; set; }

        public int Utility_Cd { get; set; }

        public decimal Stard_Values { get; set; }
        
        public int End_value { get; set; }
    }
}
