using Sistran.Company.Application.CommonAplicationServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class AgentAgencyDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int AgencyAgencyId { get; set; }
        [DataMember]
        public int AllianceId { get; set; }
        [DataMember]
        public bool IsSpecialImpression { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
