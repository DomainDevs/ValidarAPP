using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class LineBusinessDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int ReportLineBusiness { get; set; }

        [DataMember]
        public int IdLineBusinessbyRiskType { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public List<int> ListInsurectObjects { get; set; }
    }
}
