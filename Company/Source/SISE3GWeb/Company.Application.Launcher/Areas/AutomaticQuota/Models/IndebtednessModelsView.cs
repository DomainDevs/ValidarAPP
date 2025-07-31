using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    
    public class IndebtednessModelsView
    {

        public int Concept_Indebtedness_cd { get; set; }

        public int Type_Indebtedness_cd { get; set; }

        public decimal Indebtedness_Ini { get; set; }

        public decimal Indebtedness_Fin { get; set; }

        public string Observation { get; set; }
    }
}
