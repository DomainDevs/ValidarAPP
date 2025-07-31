using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamPhoneType
    {
        [DataMember]
        public PhoneTypeClass [] PhoneTypeClass { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
