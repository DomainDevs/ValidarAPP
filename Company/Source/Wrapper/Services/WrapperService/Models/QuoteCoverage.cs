using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Services.UtilitiesServices.Enums;
using COMO = Sistran.Core.Application.CommonService.Models;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteCoverage
    {

        [DataMember]
        public decimal AccumulatedDeductAmount { get; set; }

        [DataMember]
        public decimal AccumulatedLimitAmount { get; set; }

        [DataMember]
        public decimal AccumulatedPremiumAmount { get; set; }

        [DataMember]
        public decimal AccumulatedSubLimitAmount { get; set; }

        [DataMember]
        public Sistran.Core.Application.UnderwritingServices.Enums.CalculationType? CalculationType { get; set; }

        [DataMember]
        public List<UNMO.Clause> Clauses { get; set; }

        [DataMember]
        public decimal ContractAmountPercentage { get; set; }

        [DataMember]
        public List<QuoteCoverage> CoverageAllied { get; set; }

        [DataMember]
        public Sistran.Core.Application.UnderwritingServices.Enums.CoverageStatusType? CoverageOriginalStatus { get; set; }

        [DataMember]
        public int CoverNum { get; set; }

        [DataMember]
        public Sistran.Core.Application.UnderwritingServices.Enums.CoverageStatusType? CoverStatus { get; set; }

        [DataMember]
        public string CoverStatusName { get; set; }

        [DataMember]
        public int CurrencyCode { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public DateTime? CurrentTo { get; set; }

        [DataMember]
        public int Days { get; set; }

        [DataMember]
        public decimal DeclaredAmount { get; set; }

        [DataMember]
        public UNMO.Deductible Deductible { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal? DiffMinPremiumAmount { get; set; }

        [DataMember]
        public int? EndorsementId { get; set; }

        [DataMember]
        public decimal EndorsementLimitAmount { get; set; }

        [DataMember]
        public decimal EndorsementSublimitAmount { get; set; }

        [DataMember]
        public Sistran.Core.Application.UnderwritingServices.Enums.EndorsementType? EndorsementType { get; set; }

        [DataMember]
        public decimal ExcessLimit { get; set; }

        [DataMember]
        public Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType? FirstRiskType { get; set; }

        [DataMember]
        public decimal FlatRatePorcentage { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public UNMO.InsuredObject InsuredObject { get; set; }

        [DataMember]
        public bool IsDeclarative { get; set; }

        [DataMember]
        public bool IsMandatory { get; set; }

        [DataMember]
        public bool IsMinPremiumDeposit { get; set; }

        [DataMember]
        public bool IsPrimary { get; set; }

        [DataMember]
        public bool IsSelected { get; set; }

        [DataMember]
        public bool IsSublimit { get; set; }

        [DataMember]
        public bool IsVisible { get; set; }

        [DataMember]
        public decimal LimitAmount { get; set; }

        [DataMember]
        public decimal LimitClaimantAmount { get; set; }

        [DataMember]
        public decimal LimitOccurrenceAmount { get; set; }

        [DataMember]
        public int? MainCoverageId { get; set; }

        [DataMember]
        public decimal? MainCoveragePercentage { get; set; }

        [DataMember]
        public decimal MaxLiabilityAmount { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public decimal OriginalLimitAmount { get; set; }

        [DataMember]
        public decimal OriginalSubLimitAmount { get; set; }

        [DataMember]
        public decimal PercentageContract { get; set; }

        [DataMember]
        public int? PosRuleSetId { get; set; }

        [DataMember]
        public decimal PremiumAmount { get; set; }

        [DataMember]
        public decimal? Rate { get; set; }

        [DataMember]
        public RateType? RateType { get; set; }

        [DataMember]
        public int RiskCoverageId { get; set; }

        [DataMember]
        public int? RuleSetId { get; set; }

        [DataMember]
        public int? ScriptId { get; set; }

        [DataMember]
        public decimal ShortTermPercentage { get; set; }

        [DataMember]
        public decimal SubLimitAmount { get; set; }

        [DataMember]
        public decimal? SublimitPercentage { get; set; }

        [DataMember]
        public COMO.SubLineBusiness SubLineBusiness { get; set; }

        [DataMember]
        public UNMO.Text Text { get; set; }
    }
}
