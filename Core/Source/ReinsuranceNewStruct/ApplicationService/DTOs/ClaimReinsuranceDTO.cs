using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ClaimReinsuranceDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EndorsmentId { get; set; }

        [DataMember]
        public int ClaimNumber { get; set; }

        [DataMember]
        public DateTime ClaimDate { get; set; }

        [DataMember]
        public EstimationTypeDTO EstimationType { get; set; }

        [DataMember]
        public int RiskNumber { get; set; }

        [DataMember]
        public int CoverageNumber { get; set; }

        [DataMember]
        public CurrencyDTO Currency { get; set; }

        [DataMember]
        public AmountDTO Amount { get; set; }
    }
}
