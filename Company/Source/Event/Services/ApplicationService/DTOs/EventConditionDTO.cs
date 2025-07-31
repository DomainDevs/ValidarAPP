using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class EventConditionDTO : GenericListDTO
    {
        [DataMember]
        public int? OperatorId { get; set; }

        [DataMember]
        public string Operator { get; set; }

        [DataMember]
        public int? ValueId { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember] 
        public List<EntityDTO> entities { get; set; }
    }
}
