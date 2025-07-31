using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonListRiskBusinessService.Model
{
    public class RiskListMatch
    {
        public string jModel { get; set; }
        public int listVersion { get; set; }
        public string listType { get; set; }
        public double percentage { get; set; }
        public DateTime registrationDate { get; set; }
    }
}
