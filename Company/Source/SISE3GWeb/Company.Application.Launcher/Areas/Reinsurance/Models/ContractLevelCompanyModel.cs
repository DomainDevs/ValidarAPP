//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.DTOs;

using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Models
{
    public class ContractLevelCompanyModel
    {
        public int ContractLevelCompanyId { get; set; }

        public int AgentId { get; set; } // se incrementa campo x el ajuste con el launcher desde web no toma el IndividualId del modelo Agent

        //public uniquePersonModels.Agent Agent { get; set; }

        public int AgentIndividual { get; set; }
        public string AgentName { get; set; }


        [Required]
        //public uniquePersonModels.Company Company { get; set; }
        public int CompanyIndividual { get; set; }

        [Required]
        public string CompanyName { get; set; }


        [Required]
        public string Percentage { get; set; }
        
        [Required]
        public string CommissionPercentage { get; set; }
 
        public LevelDTO ContractLevel { get; set; }
        
        [Required]
        public string ReservePremiumPercentage { get; set; }

        [Required]
        public string InterestReserveRelease { get; set; }
        
        public string AdditionalCommission { get; set; }
        
        public string DragLoss { get; set; }
        

        public string ReinsurerExpenditur { get; set; }

        public string ProfitSharingPercentage { get; set; }
        [Required]
        public int Presentation { get; set; }

        [Required]
        public int BrokerCommission { get; set; }
        
        public string LossCommissionPercentage { get; set; }
        
        public string DifferentialCommissionPercentage { get; set; }

        public bool ContractFunctionalityType { get; set; }


    }
}