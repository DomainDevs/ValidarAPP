using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftApplicationServices.DTO
{
    public class EventDTO
    {
        [DataMember]
        public int RecordId { get; set; }

        [DataMember]
        public int ResultId { get; set; }
        
        [DataMember]
        public EventGroupDTO EventGroup { get; set; }

        [DataMember]
        public int EventId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool EnabledStop { get; set; }

        [DataMember]
        public bool EnabledAuthorize { get; set; }

        [DataMember]
        public string DescriptionErrorMessage { get; set; }
    }
}
