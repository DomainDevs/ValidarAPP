using System.Runtime.Serialization;

namespace Sistran.Core.Integration.AccountingServices.DTOs.Accounting
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
