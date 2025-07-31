using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ReinsuranceRealClaimByClaimCode
    {
        [DataMember]
        public int ClaimCode { get; set; }

        [DataMember]
        public int ReinsuranceNumber { get; set; }

        [DataMember]
        public int LayerNumber { get; set; }

        [DataMember]
        public string LineDescription { get; set; }

        [DataMember]
        public int LineId { get; set; }

        [DataMember]
        public string CumulusKey { get; set; }

        [DataMember]
        public string Contract { get; set; }

        [DataMember]
        public int LevelNumber { get; set; }

        [DataMember]
        public string MovementSource { get; set; }

        [DataMember]
        public string TradeName { get; set; }

        [DataMember]
        public string Amount { get; set; }
    }
}
