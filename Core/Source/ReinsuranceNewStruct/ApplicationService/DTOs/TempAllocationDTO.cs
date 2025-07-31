using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class TempAllocationDTO
    {
        [DataMember]
        public int TmpLayerLineId { get; set; }

        [DataMember]
        public int TmpIssueAllocationId { get; set; }

        [DataMember]
        public string ContractDescription { get; set; }

        [DataMember]
        public int Layer { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public decimal Premium { get; set; }

        [DataMember]
        public decimal TotSum { get; set; }

        [DataMember]
        public decimal TotPremium { get; set; }

        [DataMember]
        public bool Facultative { get; set; }

    }
}
