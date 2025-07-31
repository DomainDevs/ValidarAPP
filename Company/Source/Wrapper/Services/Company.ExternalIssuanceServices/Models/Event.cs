using System.Runtime.Serialization;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    [DataContract]
    public class Event
    {
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public string EventDescription { get; set; }
    }
}
