using Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class AutomaticQuota
    {
        [DataMember]
        public int AutomaticQuotaId { get; set; }

        [DataMember]
        public Third Third { get; set; }

        [DataMember]
        public List<Utility> Utility { get; set; }

        [DataMember]
        public List<Indicator> Indicator { get; set; }

        [DataMember]
        public int Prospectid { get; set; }
        [DataMember]
        public int Indicatorid { get; set; }
        [DataMember]
        public Prospect Prospect { get; set; }
        [DataMember]
        public AgentProgram AgentProgram { get; set; }
        [DataMember]
        public Agent Agent { get; set; }
        [DataMember]
        public decimal SuggestedQuota { get; set; }
        [DataMember]
        public decimal QuotaReconsideration { get; set; }
        [DataMember]
        public decimal LegalizedQuota { get; set; }
        [DataMember]
        public decimal CurrentQuota { get; set; }
        [DataMember]
        public decimal CurrentCluster { get; set; }
        [DataMember]
        public DateTime QuotaPreparationDate { get; set; }
        [DataMember]
        public int RequestedBy { get; set; }

        [DataMember]
        public int Elaborated { get; set; }
        [DataMember]
        public int TypeCollateral { get; set; }
        [DataMember]
        public int CollateralStatus { get; set; }
        [DataMember]
        public string Observations { get; set; }

        [DataMember]
        public Base Capacity { get; set; }

        [DataMember]
        public Base RestrictiveList { get; set; }

        [DataMember]
        public Base RiskCenter { get; set; }

        [DataMember]
        public Base SignatureOrLetter { get; set; }

        [DataMember]
        public Base CurrentReason { get; set; }

        [DataMember]
        public Base AcidTest { get; set; }

        [DataMember]
        public Base Indebtedness { get; set; }

        [DataMember]
        public Base SalesGrowth { get; set; }

        [DataMember]
        public Base Etibda { get; set; }

        [DataMember]
        public Base EquityVariation { get; set; }

        [DataMember]
        public Base NetIncomeVariation { get; set; }

        [DataMember]
        public Base AverageSales { get; set; }

        [DataMember]
        public Base ActiveAverage { get; set; }

        [DataMember]
        public Base AverageEquity { get; set; }

        [DataMember]
        public Base AverageUtility { get; set; }

        [DataMember]
        public Base AverageNetIncome { get; set; }

        [DataMember]
        public decimal? ScoreCustomerVSMarket { get; set; }

        [DataMember]
        public decimal? ScoreObjectiveCriteria { get; set; }

        [DataMember]
        public decimal? ScoreSubjectiveCriteria { get; set; }

        [DataMember]
        public decimal? TargetWeightedScore { get; set; }

        [DataMember]
        public decimal? SubjectiveWeightedScore { get; set; }

        [DataMember]
        public decimal? ScaleWeighted { get; set; }

        [DataMember]
        public decimal? EquityPercentage { get; set; }

        [DataMember]
        public decimal? OperatingIncomePercentage { get; set; }

        [DataMember]
        public decimal? ShareCapitalPercentage { get; set; }

        [DataMember]
        public decimal? AverageRevenuePercentage { get; set; }

        [DataMember]
        public decimal? QuotaA { get; set; }

        [DataMember]
        public decimal? QuotaB { get; set; }

        [DataMember]
        public Base Experience { get; set; }

        [DataMember]
        public Base KnowledgeClient { get; set; }

        [DataMember]
        public Base CorporateGovernance { get; set; }

        [DataMember]
        public Base CustomerReputation { get; set; }

        [DataMember]
        public Base MoralSolvency { get; set; }

        [DataMember]
        public Base CustomerKnowledgeAccess { get; set; }

        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }
    }
}
