using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ByLineBusinessSubLineBusinessRiskDTO
    {
        [DataMember]
        public int LineBusinessId { get; set; }
        [DataMember]
        public int SubLineBusinessId { get; set; }
        [DataMember]
        public List<InsuredObjectDTO> InsuredObject { get; set; }
    }
}