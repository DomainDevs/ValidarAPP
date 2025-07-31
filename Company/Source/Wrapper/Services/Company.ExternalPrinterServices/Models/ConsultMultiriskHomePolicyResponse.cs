using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    [DataContract]
    public class ConsultMultiriskHomePolicyResponse
    {
        [DataMember]
        public List<MultiriskHomeClass> MultiriskHomeClass { get; set; }
        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
