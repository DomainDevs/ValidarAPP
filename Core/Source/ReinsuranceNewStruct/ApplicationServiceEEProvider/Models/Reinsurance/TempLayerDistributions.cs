using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class TempLayerDistributions
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
