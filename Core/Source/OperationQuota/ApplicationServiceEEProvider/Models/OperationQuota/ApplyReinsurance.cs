using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class ApplyReinsurance
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
        public int EndorsementType { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int ConsortiumId { get; set; }
        [DataMember]
        public int EconomicGroupId { get; set; }
        [DataMember]
        public List<ContractCoverage> ContractCoverage { get; set; }
        [DataMember]
        public int CurrencyType { get; set; }
        [DataMember]
        public string CurrencyTypeDesc { get; set; }
        [DataMember]
        public decimal ParticipationPercentage { get; set; }
        [DataMember]
        public int PrefixId { get; set; }
        [DataMember]
        public int BranchId { get; set; }
    }
}