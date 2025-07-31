using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.OperationQuotaServices.DTOs
{
    [DataContract]
    public class AutomaticQuotaDTO
    {
        [DataMember]
        public int AutomaticQuotaId { get; set; }
        [DataMember]
        public ThirdDTO ThirdDTO { get; set; }

        [DataMember]
        public int utilityId { get; set; }

        [DataMember]
        public List<UtilityDTO> UtilityDTO { get; set; }
        [DataMember]
        public List<IndicatorDTO> indicatorDTO { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int CustomerTpeId { get; set; }

        [DataMember]
        public ProspectDTO ProspecDTO { get; set; }
        [DataMember]
        public AgentProgramDTO AgentProgramDTO { get; set; }
        [DataMember]
        public decimal SuggestedQuota { get; set; }
        [DataMember]
        public decimal QuotaReConsideration { get; set; }
        [DataMember]
        public decimal LegalizedQuota { get; set; }
        [DataMember]
        public decimal CurrentQuota { get; set; }
        [DataMember]
        public decimal CurrentCluster { get; set; }
        [DataMember]
        public DateTime QuotaPreparationDate { get; set; }
        [DataMember]
        public int RequestedById { get; set; }

        [DataMember]
        public string RequestedByName { get; set; }
        [DataMember]
        public int ElaboratedId { get; set; }

        [DataMember]
        public string ElaboratedName { get; set; }

        [DataMember]
        public int TypeCollateral { get; set; }
        [DataMember]
        public int CollateralStatus { get; set; }
        [DataMember]
        public string Observations { get; set; }

        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public BaseDTO Capacity { get; set; }

        [DataMember]
        public BaseDTO RestrictiveList { get; set; }

        [DataMember]
        public BaseDTO RiskCenter { get; set; }

        [DataMember]
        public BaseDTO SignatureOrLetter { get; set; }

        [DataMember]
        public BaseDTO CurrentReason { get; set; }

        [DataMember]
        public BaseDTO AcidTest { get; set; }

        [DataMember]
        public BaseDTO Indebtedness { get; set; }

        [DataMember]
        public BaseDTO SalesGrowth { get; set; }

        [DataMember]
        public BaseDTO Etibda { get; set; }

        [DataMember]
        public BaseDTO EquityVariation { get; set; }

        [DataMember]
        public BaseDTO NetIncomeVariation { get; set; }

        [DataMember]
        public BaseDTO AverageSales { get; set; }

        [DataMember]
        public BaseDTO ActiveAverage { get; set; }

        [DataMember]
        public BaseDTO AverageEquity { get; set; }

        [DataMember]
        public BaseDTO AverageUtility { get; set; }

        [DataMember]
        public BaseDTO AverageNetIncome { get; set; }

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
        public BaseDTO Experience { get; set; }

        [DataMember]
        public BaseDTO KnowledgeClient { get; set; }

        [DataMember]
        public BaseDTO CorporateGovernance { get; set; }

        [DataMember]
        public BaseDTO CustomerReputation { get; set; }

        [DataMember]
        public BaseDTO MoralSolvency { get; set; }

        [DataMember]
        public BaseDTO CustomerKnowledgeAccess { get; set; }
        [DataMember]
        public List<DynamicConcept> DynamicProperties { get; set; }

        [DataMember]
        public AgentDTO Agent { get; set; }

        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int StateId { get; set; }
        [DataMember]
        public int CityId { get; set; }
    }
}
