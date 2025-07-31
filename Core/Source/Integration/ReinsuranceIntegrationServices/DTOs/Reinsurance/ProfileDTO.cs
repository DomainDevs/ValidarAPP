using Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
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
