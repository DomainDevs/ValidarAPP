using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class GetConsulSinisterPolicyResponse
    {
        [DataMember]
        public List<ListSinisterPolicyClass> ListSinisterPolicyClass { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
