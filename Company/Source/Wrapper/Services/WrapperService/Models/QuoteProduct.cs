using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COMO = Sistran.Core.Application.CommonService.Models;
using ProductModel = Sistran.Core.Application.ProductServices.Models;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.WrapperServices.Models
{
    [DataContract]
    public class QuoteProduct
    {
        [DataMember]
        public decimal? AdditDisCommissPercentage { get; set; }
        [DataMember]
        public decimal? AdditionalCommissionPercentage { get; set; }
        [DataMember]
        public ProductModel.CoveredRisk CoveredRisk { get; set; }
        [DataMember]
        public List<COMO.Currency> Currencies { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime? CurrentTo { get; set; }
        [DataMember]
        public decimal? DecrementCommisionAdjustFactorPercentage { get; set; }
        [DataMember]
        public List<QuoteDeductibleProduct> DeductibleProduct { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public List<UNMO.FinancialPlan> FinancialPlans { get; set; }
        [DataMember]
        public List<QuoteGroupCoverage> GroupCoverageAllieds { get; set; }
        [DataMember]
        public List<QuoteGroupCoverage> GroupCoverages { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public decimal? IncrementCommisionAdjustFactorPercentage { get; set; }
        [DataMember]
        public bool IsCollective { get; set; }
        [DataMember]
        public bool IsFlatRate { get; set; }
        [DataMember]
        public bool IsGreen { get; set; }
        [DataMember]
        public bool IsRequest { get; set; }
        [DataMember]
        public bool IsUse { get; set; }
        [DataMember]
        public List<QuoteLimitRCRelation> LimitRC { get; set; }
        [DataMember]
        public List<UNMO.PaymentSchedule> PaymentSchedules { get; set; }
        [DataMember]
        public List<COMO.PolicyType> PolicyTypes { get; set; }
        [DataMember]
        public COMO.Prefix Prefix { get; set; }
        [DataMember]
        public int? PreRuleSetId { get; set; }
        [DataMember]
        public List<ProductModel.ProductAgent> ProductAgents { get; set; }
        [DataMember]
        public List<ProductModel.ProductFinancialPlan> ProductFinancialPlan { get; set; }
        [DataMember]
        public int? RuleSetId { get; set; }
        [DataMember]
        public int? ScriptId { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public decimal? StandardCommissionPercentage { get; set; }
        [DataMember]
        public decimal? StdDiscountCommPercentage { get; set; }
        [DataMember]
        public decimal? SurchargeCommissionPercentage { get; set; }
        [DataMember]
        public int Version { get; set; }

    }
}
