using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class ComboListDTO
    {
        [DataMember]
        public List<ComboDTO> ContractTypes { get; set; }

        [DataMember]
        public List<ComboDTO> ContractCategories { get; set; }

        [DataMember]
        public List<ComboDTO> GroupCoverages { get; set; }
    }
}
