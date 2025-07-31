using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class GetConsulRecoveriesPolicyResponse
    {
        [DataMember]
        public List<ListRecoveriesPolicyClass> ListRecoveriesPolicyClass { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
