using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
{
    [DataContract]
    public class PremiumDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public List<PremiumComponentDTO> PremiumComponentDTOs { get; set; }
    }
}
