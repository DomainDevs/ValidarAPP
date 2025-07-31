using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ReinsuranceClaimDistributionDTO
    {
        [DataMember]
        public int ClaimLayerId { get; set; }

        [DataMember]
        public string Line { get; set; }

        [DataMember]
        public string CumulusKey { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string CurrencyDescription { get; set; }

        [DataMember]
        public string IsFacultative { get; set; }

        [DataMember]
        public string Contract { get; set; }

        [DataMember]
        public int LevelNumber { get; set; }

        [DataMember]
        public string Broker { get; set; }

        [DataMember]
        public string Reinsurer { get; set; }

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public string MovementSource { get; set; }

        [DataMember]
        public int TempClaimReinsSourceId { get; set; }

        [DataMember]
        public string SourceAmount { get; set; }

        [DataMember]
        public string NewAmount { get; set; }

        [DataMember]
        public string Variance { get; set; }
        [DataMember]
        public int MovementSourceId { get; set; }
    }
}
