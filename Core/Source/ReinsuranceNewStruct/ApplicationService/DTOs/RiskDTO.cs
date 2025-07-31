using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class RiskDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public List<CoverageDTO> Coverages { get; set; }
    }
}
