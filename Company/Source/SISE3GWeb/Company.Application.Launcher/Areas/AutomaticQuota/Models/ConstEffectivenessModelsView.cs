using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    public class ConstEffectivenessModelsView
    {
        
        public int Concept_ConstEffectiveness_cd { get; set; }
        
        public int Type_ConstEffectiveness_cd { get; set; }
        
        public decimal ConstEffectiveness_Ini { get; set; }
        
        public decimal ConstEffectiveness_Fin { get; set; }
        
        public string Observation { get; set; }
    }
}
