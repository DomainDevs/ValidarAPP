using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    
    public class IndicatorModelsView
    {

        public int ConceptIndicatorcd { get; set; }

        public int TypeIndicatorcd { get; set; }

        public decimal IndicatorIni { get; set; }

        public decimal IndicatorFin { get; set; }

        public string Observation { get; set; }
    }
}
