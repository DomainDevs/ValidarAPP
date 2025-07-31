using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class ValidationPhoneType : Validation
    {
        [DataMember]
        public int PhoneType { get; set; }

        [DataMember]
        public string Number { get; set; }
    }
}
