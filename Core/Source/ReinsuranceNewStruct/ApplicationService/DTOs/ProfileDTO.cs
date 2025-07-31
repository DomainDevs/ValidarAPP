using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ProfileDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool Static { get; set; }
        [DataMember]
        public bool HasAccess { get; set; }
        [DataMember]
        public string EnabledDescription { get; set; }

        [DataMember]
        public List<AccessProfileDTO> profileAccesses { get; set; }
    }
}
