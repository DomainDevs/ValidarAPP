using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalConsultPrinterServices.Models
{
    [DataContract]
    public class ConsultCoveragePolicyResponse
    {
        [DataMember]
        public List<ListCoveragePolicyClass> ListCoveragePolicyClass { get; set;  }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
