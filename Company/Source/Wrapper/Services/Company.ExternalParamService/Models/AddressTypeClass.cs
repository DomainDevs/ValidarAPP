using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class AddressTypeClass
    {
        [DataMember]
        public int AddressTypeCd { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
