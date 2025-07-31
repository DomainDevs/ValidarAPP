using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class AgentDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public List<AgencyDTO> Agencies { get; set; }
    }
}
