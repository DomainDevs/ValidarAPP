using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ByPrefixDTO : LineAssociationTypeDTO
    {
        [DataMember]
        public List<PrefixDTO> Prefix { get; set; }
    }
}
