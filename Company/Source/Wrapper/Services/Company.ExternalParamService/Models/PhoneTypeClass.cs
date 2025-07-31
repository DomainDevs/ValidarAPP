using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class PhoneTypeClass
    {
        [DataMember]
        public int PhoneTypeCd { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
