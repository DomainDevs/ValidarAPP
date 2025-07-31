using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalParamService.Models
{
    [DataContract]
    public class ParamAddressType
    {
        [DataMember]
        public List<AddressTypeClass> AddressTypeList { get; set; }

        [DataMember]
        public string ProcessMessage { get; set; }
    }
}
