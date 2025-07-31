using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class TempLayerDistributionsDTO
    {
        [DataMember]
        public int TmpReinsuranceProcessId { get; set; }

        [DataMember]
        public int LayerNumber { get; set; }

        [DataMember]
        public decimal LayerPercentage { get; set; }

        [DataMember]
        public decimal SumAmount { get; set; }

        [DataMember]
        public decimal PremiumPercentage { get; set; }

        [DataMember]
        public decimal PremiumAmount { get; set; }

        [DataMember]
        public int TempIssueLayerId { get; set; }
    }
}
