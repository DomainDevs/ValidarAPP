using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class ConsulBenefPolicyResponse
    {
        [DataMember]
        public List<ListBenefPolicyClass> ListBenefPolicyClass { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
