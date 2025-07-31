using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ClaimAllocationDTO
    {
        [DataMember]
        public int TmpReinsuranceProcessId { get; set; }

        [DataMember]
        public int TmpClaimReinsSourceId { get; set; }

        [DataMember]
        public int LineId { get; set; }

        [DataMember]
        public string CumulusKey { get; set; }

        [DataMember]
        public int LayerNumber { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int LevelNumber { get; set; }

        [DataMember]
        public string Contract { get; set; }

        [DataMember]
        public string MovementSource { get; set; }

        [DataMember]
        public int MovementSourceId { get; set; }
    }
}
