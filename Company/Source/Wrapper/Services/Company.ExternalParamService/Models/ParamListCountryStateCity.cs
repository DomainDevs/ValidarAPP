using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamListCountryStateCity
    {
        [DataMember]
        public List<ListCountryStateCityClass> ListCountryStateCityClass { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
