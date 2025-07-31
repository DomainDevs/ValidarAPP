using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class PaymentAllocationDTO
    {
        [DataMember]
        public int TmpPaymentReinsSourceId { get; set; }

        [DataMember]
        public int LineId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string CumulusKey { get; set; }

        [DataMember]
        public int LayerNumber { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public int LevelNumber { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int MovementSourceId { get; set; }

        [DataMember]
        public int ClaimCoverageCd { get; set; }

        [DataMember]
        public int VoucherConceptCd { get; set; }
    }
}
