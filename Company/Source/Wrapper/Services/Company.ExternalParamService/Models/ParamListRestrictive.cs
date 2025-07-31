using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamListRestrictive
    {
        [DataMember]
        public List<ListRestrictiveClass> ListRestrictiveClass { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
