using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    
    public class UtilityModelsView
    {
        
        public int Id { get; set; }
        
        public UtilityDetailsModelsView UtilityDetails { get; set; }

        public decimal Start_Values { get; set; }
        
        public decimal End_value { get; set; }
       
        public decimal Var_Abs { get; set; }
        
        public decimal Var_Relativa { get; set; }
    }
}
