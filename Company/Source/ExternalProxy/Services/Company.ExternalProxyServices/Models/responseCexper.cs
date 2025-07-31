using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseCexper
    {
        public Error Error { get; set; }
        [DataMember]
        public List<ResponsePoliciesInfo> PoliciesInfo { get; set; }
        [DataMember]
        public List<ResponseSinisterInfo> SinisterInfo { get; set; }
        [DataMember]
        public List<ResponseSimitSISA> Simit { get; set; }
    }
}
