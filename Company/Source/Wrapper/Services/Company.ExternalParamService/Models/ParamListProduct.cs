using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamListProduct
    {
        [DataMember]
        public List<ListProductClass> ListProductClass { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
