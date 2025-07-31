using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamEconomicActivity
    {
        [DataMember]
        public List<EconomicActivityClass> EconomicActivityClass { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
