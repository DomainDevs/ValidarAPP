using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.AutomaticQuota.Models
{
    public class AutomaticQuotaModelsView
    {
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]

        public int AutomaticQuotaId { get; set; }
        
        public ThirdModelsView Third { get; set; }
        
        public List<UtilityModelsView> Utility { get; set; }
        
        public List<IndicatorModelsView> Indicator { get; set; }
        
        public int Prospectid { get; set; }
        
        public int Indicatorid { get; set; }
        
        public ProspectModelsView Prospect { get; set; }
       
        public AgentProgramModelsView AgentProgram { get; set; }
       
        public decimal SuggestedQuota { get; set; }
      
        public decimal QuotaReconsideration { get; set; }
     
        public decimal LegalizedQuota { get; set; }
    
        public decimal CurrentQuota { get; set; }

        public decimal CurrentCluster { get; set; }

        public DateTime QuotaPreparationDate { get; set; }
        
        public int RequestedBy { get; set; }
        
        public string Elaborated { get; set; }
   
        public int TypeCollateral { get; set; }
     
        public int CollateralStatus { get; set; }
  
        public string Observations { get; set; }

        public BaseModelsView Capacity { get; set; }
        
        public BaseModelsView RestrictiveList { get; set; }
        
        public BaseModelsView RiskCenter { get; set; }
        
        public BaseModelsView SignatureOrLetter { get; set; }
        
        public BaseModelsView CurrentReason { get; set; }
        
        public BaseModelsView AcidTest { get; set; }
        
        public BaseModelsView Indebtedness { get; set; }
        
        public BaseModelsView SalesGrowth { get; set; }
        
        public BaseModelsView Etibda { get; set; }
        
        public BaseModelsView EquityVariation { get; set; }
        
        public BaseModelsView NetIncomeVariation { get; set; }
        
        public BaseModelsView AverageSales { get; set; }
        
        public BaseModelsView ActiveAverage { get; set; }
        
        public BaseModelsView AverageEquity { get; set; }
        
        public BaseModelsView AverageUtility { get; set; }
        
        public BaseModelsView AverageNetIncome { get; set; }
        
        public decimal? ScoreCustomerVSMarket { get; set; }
        
        public decimal? ScoreObjectiveCriteria { get; set; }
        
        public decimal? ScoreSubjectiveCriteria { get; set; }
        
        public decimal? TargetWeightedScore { get; set; }
        
        public decimal? SubjectiveWeightedScore { get; set; }
        
        public decimal? ScaleWeighted { get; set; }
        
        public decimal? EquityPercentage { get; set; }
        
        public decimal? OperatingIncomePercentage { get; set; }
        
        public decimal? ShareCapitalPercentage { get; set; }
        
        public decimal? AverageRevenuePercentage { get; set; }
        
        public decimal? QuotaA { get; set; }
        
        public decimal? QuotaB { get; set; }
        
        public BaseModelsView Experience { get; set; }
        
        public BaseModelsView KnowledgeClient { get; set; }
        
        public BaseModelsView CorporateGovernance { get; set; }
        
        public BaseModelsView CustomerReputation { get; set; }
        
        public BaseModelsView MoralSolvency { get; set; }
        
        public BaseModelsView CustomerKnowledgeAccess { get; set; }
        
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }
    }
}