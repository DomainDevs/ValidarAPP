using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
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
