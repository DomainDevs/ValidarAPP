using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
{
    [DataContract]
    public class PremiumBaseDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public List<PremiumDTO> PremiumDTOs { get; set; }
    }
}
