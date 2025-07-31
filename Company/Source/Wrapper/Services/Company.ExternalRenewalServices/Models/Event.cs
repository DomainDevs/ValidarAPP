using System.Runtime.Serialization;

namespace Sistran.Company.ExternalRenewalServices.Models
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
