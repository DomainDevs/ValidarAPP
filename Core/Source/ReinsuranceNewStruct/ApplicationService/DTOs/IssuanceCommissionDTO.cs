using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class IssuanceCommissionDTO
    {
        [DataMember]
        public SubLineBusinessDTO SubLineBusiness { get; set; }
        [DataMember]
        public decimal Percentage { get; set; }
        [DataMember]
        public decimal PercentageAdditional { get; set; }
        [DataMember]
        public decimal CalculateBase { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
