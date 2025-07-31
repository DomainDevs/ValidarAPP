using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class GetConsulEndorsementPolicyResponse
    {
        [DataMember]
        public List<ListEndorsementPolicyClass> ListEndorsementPolicyClass { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
