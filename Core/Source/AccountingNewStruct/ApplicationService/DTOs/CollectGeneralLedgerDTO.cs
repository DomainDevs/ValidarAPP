using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class CollectGeneralLedgerDTO
    {
        [DataMember]
        public CollectApplicationDTO CollectImputation { get; set; }

        [DataMember]
        public BillDTO Bill { get; set; }

        [DataMember]
        public int BillControlId { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int PreliquidationBranch { get; set; }
    }
}
