using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    [DataContract]
    public class CompanyParamCoCoverageValue
    {        
        [DataMember]
        public decimal? Percentage { get; set;}

        [DataMember]
        public CompanyParamPrefix Prefix { get; set;}

        [DataMember]
        public CompanyParamCoverage Coverage { get; set; }
    }
}
