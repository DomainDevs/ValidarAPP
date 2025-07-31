using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ClaimCoverageDTO
    {
        [DataMember]
        public List<EstimationDTO> Estimations { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int SubClaim { get; set; }
    }
}