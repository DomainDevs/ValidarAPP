using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// ReinsuranceDistributionHeaderDTO
    /// </summary>
    [DataContract]
    public class ReinsuranceDistributionHeaderDTO
    {
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int LayerNumber { get; set; }
        [DataMember]
        public decimal LayerPercentage { get; set; }
        [DataMember]
        public string Line { get; set; }
        [DataMember]
        public string CumulusKey { get; set; }
        [DataMember]
        public int ContractId { get; set; }
        [DataMember]
        public string ContractDescription { get; set; }
        [DataMember]
        public int LevelNumber { get; set; }
        [DataMember]
        public string TradeName { get; set; }
        [DataMember]
        public string AmountSum { get; set; }
        [DataMember]
        public string PremiumSum { get; set; }
    }
}