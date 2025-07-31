using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class IssueAllocationRiskCover
    {
        [DataMember]
        public int IssueAllocationRiskCoverId { get; set; }
        [DataMember]
        public int IssueCumulusRiskCoverId { get; set; }
        [DataMember]
        public int CoverageId { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public bool IsFacultative { get; set; }
        [DataMember]
        public int ContractCompanyId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public int ContractCurrencyCd { get; set; }
        [DataMember]
        public int ContractId { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public int LevelId { get; set; }
        [DataMember]
        public decimal LevelLimit { get; set; }
        [DataMember]
        public int LineBusinessCd { get; set; }
        [DataMember]
        public int SubLineBusinessCd { get; set; }
        [DataMember]
        public DateTime CoverCurrentFrom { get; set; }
        [DataMember]
        public DateTime CoverCurrentTo { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public decimal EndorsementNumber { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public decimal DocumentNum { get; set; }
        [DataMember]
        public int InsuredCd { get; set; }
        [DataMember]
        public int IndividualCd { get; set; }
        [DataMember]
        public DateTime PolicyCurrentFrom { get; set; }
        [DataMember]
        public DateTime PolicyCurrentTo { get; set; }
        [DataMember]
        public int BranchCd { get; set; }
        [DataMember]
        public int PrefixCd { get; set; }
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public int RiskId { get; set; }
    }
}
