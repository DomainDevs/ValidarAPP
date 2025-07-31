using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.TempCommonService.DTOs
{
    [DataContract]
    public class RiskDTO
    {
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
