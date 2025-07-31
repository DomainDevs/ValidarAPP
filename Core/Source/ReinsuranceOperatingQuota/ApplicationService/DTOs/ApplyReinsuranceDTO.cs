using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceOperatingQuotaServices.DTOs
{
    [DataContract]
    public class ApplyReinsuranceDTO
    {
        [DataMember]
        public int PolicyID { get; set; }
        [DataMember]
        public int DocumentNum { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int CoverageId { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int ConsortiumId { get; set; }
        [DataMember]
        public int EconomicGroupId { get; set; }
        [DataMember]
        public int EndorsementType { get; set; }
        [DataMember]
        public List<ContractCoverageDTO> ContractCoverage { get; set; }
        [DataMember]
        public int CurrencyType { get; set; }
        [DataMember]
        public string CurrencyTypeDesc { get; set; }
        [DataMember]
        public decimal ParticipationPercentage { get; set; }
    }
}
