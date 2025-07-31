using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamAssistanceClass
    {
        [DataMember]
        public List<ParamAssistance> ParamAssistance { get; set; }

        [DataMember]
        public string ProcessMessage { get; set;  }
    }
}
