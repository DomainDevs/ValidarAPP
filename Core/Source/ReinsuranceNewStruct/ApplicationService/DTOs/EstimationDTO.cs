using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class EstimationDTO
    {
        [DataMember]
        public EstimationTypeDTO Type { get; set; }
        [DataMember]
        public ReasonDTO Reason { get; set; }
        [DataMember]
        public CurrencyDTO Currency { get; set; }
        [DataMember]
        public decimal? PaymentAmount { get; set; }
        [DataMember]
        public int Version { get; set; }
    }
}