using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class ParamCoCoverageValue:BaseParamCoCoverageValue
    {
        [DataMember]
        public BaseParamPrefix Prefix { get; set;}
        
        [DataMember]
        public BaseParamCoverage Coverage { get; set;}
    }
}
