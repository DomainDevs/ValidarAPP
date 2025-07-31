using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class IdentityCardTypeClass
    {
        [DataMember]
        public int IdentityCardTypeId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
