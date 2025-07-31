using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ByPrefixRiskDTO
    {
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public List<InsuredObjectDTO> InsuredObject { get; set; }
    }
}
