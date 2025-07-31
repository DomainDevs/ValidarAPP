using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamQueryAllyCoverage
    {
        [DataMember]
        public CompanyParamQueryCoverage AllyCoverage { get; set; }
        [DataMember]
        public CompanyParamQueryCoverage Coverage { get; set; }
        [DataMember]
        public decimal CoveragePct { get; set; }
    }
}
