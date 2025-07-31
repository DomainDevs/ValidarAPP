using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ReinsuranceLayerIssuance : ReinsuranceLayer
    {
        [DataMember]
        public decimal LayerPercentage { get; set; }

        [DataMember]
        public decimal LayerAmount { get; set; }

        [DataMember]
        public decimal PremiumPercentage { get; set; }

        [DataMember]
        public decimal PremiumAmount { get; set; }

        [DataMember]
        public List<ReinsuranceLine> Lines { get; set; }

        [DataMember]
        public int TemporaryIssueId { get; set; }
    }
}
