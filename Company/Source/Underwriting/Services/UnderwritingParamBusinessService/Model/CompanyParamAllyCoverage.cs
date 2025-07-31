using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamAllyCoverage
    {
        [DataMember]
        public int AllyCoverageId { get; set; }
        [DataMember]
        public int CoverageId { get; set; }
        [DataMember]
        public decimal CoveragePct { get; set; }
    }
}
