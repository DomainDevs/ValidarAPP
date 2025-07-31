using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class OperatorConditionDTO
    {
        [DataMember]
        public int ComparatorId { set; get; }

        [DataMember]
        public string Description { set; get; }

        [DataMember]
        public string SmallDesc { set; get; }

        [DataMember]
        public string Symbol { set; get; }

        [DataMember]
        public bool Text { set; get; }

        [DataMember]
        public bool Combo { set; get; }

        [DataMember]
        public bool Query { set; get; }

        [DataMember]
        public decimal NumValues { set; get; }
    }
}
