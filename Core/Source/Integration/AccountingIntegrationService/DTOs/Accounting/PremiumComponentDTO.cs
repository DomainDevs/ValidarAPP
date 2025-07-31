using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
{
    [DataContract]
    public class PremiumComponentDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public List<PremiumComponentLbSbDTO> PremiumComponentLbSbDTOs { get; set; }
    }
}
