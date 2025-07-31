using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class BranchDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public List<SalePointDTO> SalePoints { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
