
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class GroupEventDTO
    {
        [DataMember]
        public int GroupEventId { set; get; }

        [DataMember]
        public int EventId { set; get; }

        [DataMember]
        public int? PrefixId { set; get; }

        [DataMember]
        public int? AccessId { set; get; }
    }
}
