using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class PrefixTypeDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }
    }
}
