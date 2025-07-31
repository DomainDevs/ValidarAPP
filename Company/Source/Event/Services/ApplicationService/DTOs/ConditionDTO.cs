using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class ConditionDTO
    {
        [DataMember]
        public int GroupEventId { get; set; }

        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public int DelegationId { get; set; }

        [DataMember]
        public int EntityId { get; set; }

        [DataMember]
        public int ConditionQuantity { get; set; }

        [DataMember]
        public int EventQuantity { get; set; }

        [DataMember]
        public int ComparatorId { get; set; }

        [DataMember]
        public string Condition { get; set; }
    }
}