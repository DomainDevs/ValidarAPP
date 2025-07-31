using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class StatusDTO
    {
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public InternalStatusDTO InternalStatus { get; set; }
    }
}