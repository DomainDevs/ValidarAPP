using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class PaymentDistributionDTO
    {
        [DataMember]
        public int PaymentRequestCode { get; set; }

        [DataMember]
        public int ReinsuranceNumber { get; set; }

        [DataMember]
        public int LayerNumber { get; set; }

        [DataMember]
        public string Contract { get; set; }

        [DataMember]
        public int LevelNumber{ get; set; }

        [DataMember]
        public string TradeName { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int MovementSourceId { get; set; }
    }
}
