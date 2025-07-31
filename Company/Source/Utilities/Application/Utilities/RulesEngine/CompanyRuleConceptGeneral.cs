using Sistran.Core.Application.Utilities.RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Utilities.RulesEngine
{
    public class CompanyRuleConceptGeneral : RuleConceptGeneral
    {

        public static KeyValuePair<string, int> RenewallNum
        {
            get
            {
                return new KeyValuePair<string, int>("RenewallNum", Id);
            }
        }

        public static KeyValuePair<string, int> RenewallHistory
        {
            get
            {
                return new KeyValuePair<string, int>("RenewallHistory", Id);
            }
        }
        public static KeyValuePair<string, int> NewRenovated
        {
            get
            {
                return new KeyValuePair<string, int>("NewRenovated", Id);
            }
        }

        public static KeyValuePair<string, int> IsRequest
        {
            get
            {
                return new KeyValuePair<string, int>("IsRequest", Id);
            }
        }

        public static KeyValuePair<string, int> BirthDate
        {
            get
            {
                return new KeyValuePair<string, int>("BirthDate", Id);
            }
        }
        public static KeyValuePair<string, int> EffectPeriod
        {
            get
            {
                return new KeyValuePair<string, int>("EffectPeriod", Id);
            }
        }      
        public static KeyValuePair<string, int> PrimaryAgentCodePrincipal
        {
            get
            {
                return new KeyValuePair<string, int>("PrimaryAgentCodePrincipal", Id);
            }
        }
        public static KeyValuePair<string, int> BusinessId
        {
            get
            {
                return new KeyValuePair<string, int>("BusinessId", Id);
            }
        }
        public static KeyValuePair<string, int> SinisterQuantity
        {
            get
            {
                return new KeyValuePair<string, int>("SinisterQuantity", Id);
            }
        }
        public static KeyValuePair<string, int> RenewalsQuantity
        {
            get
            {
                return new KeyValuePair<string, int>("RenewalsQuantity", Id);
            }
        }
        public static KeyValuePair<string, int> PortfolioBalance
        {
            get
            {
                return new KeyValuePair<string, int>("PortfolioBalance", Id);
            }
        }
        public static KeyValuePair<string, int> HasTotalLoss
        {
            get
            {
                return new KeyValuePair<string, int>("HasTotalLoss", Id);
            }
        }
        public static KeyValuePair<string, int> SinisterQuantityLastYears
        {
            get
            {
                return new KeyValuePair<string, int>("SinisterQuantityLastYears", Id);
            }
        }
        public static KeyValuePair<string, int> CommitDate
        {
            get
            {
                return new KeyValuePair<string, int>("CommitDate", Id);
            }
        }
        public static KeyValuePair<string, int> PrimaryAgentAgencyId
        {
            get
            {
                return new KeyValuePair<string, int>("PrimaryAgentAgencyId", Id);
            }
        }
        public static KeyValuePair<string, int> IsFlatRate
        {
            get
            {
                return new KeyValuePair<string, int>("IsFlatRate", Id);
            }
        }
        public static KeyValuePair<string, int> IsCollective
        {
            get
            {
                return new KeyValuePair<string, int>("IsCollective", Id);
            }
        }

        public static KeyValuePair<string, int> PreRuleSetId
        {
            get
            {
                return new KeyValuePair<string, int>("PreRuleSetId", Id);
            }
        }

        public static KeyValuePair<string, int> AgentType
        {
            get
            {
                return new KeyValuePair<string, int>("AgentType", Id);
            }
        }
        public static KeyValuePair<string, int> HolderCellPhone
        {
            get
            {
                return new KeyValuePair<string, int>("HolderCellPhone", Id);
            }
        }
        public static KeyValuePair<string, int> HolderEmail
        {
            get
            {
                return new KeyValuePair<string, int>("HolderEmail", Id);
            }
        }
        public static KeyValuePair<string, int> HolderTypeDocument
        {
            get
            {
                return new KeyValuePair<string, int>("HolderTypeDocument", Id);
            }
        }

        
        public static KeyValuePair<string, int> JustificationSarlaft
        {
            get
            {
                return new KeyValuePair<string, int>("JustificationSarlaft", Id);
            }
        }

        public static KeyValuePair<string, int> UserProfileId
        {
            get
            {
                return new KeyValuePair<string, int>("UserProfileId", Id);
            }
        }

        public static KeyValuePair<string, int> GroupBranchId
        {
            get
            {
                return new KeyValuePair<string, int>("GroupBranchId", Id);
            }
        }

        public static KeyValuePair<string, int> TotalPremiumPercentage
        {
            get
            {
                return new KeyValuePair<string, int>("TotalPremiumPercentage", Id);
            }
        }

        public static KeyValuePair<string, int> EmailElectronicBilling
        {
            get
            {
                return new KeyValuePair<string, int>("EmailElectronicBilling", Id);
            }
        }

        public static KeyValuePair<string, int> RegimeType
        {
            get
            {
                return new KeyValuePair<string, int>("RegimeType", Id);
            }
        }

        public static KeyValuePair<string, int> fiscalResponsibility
        {
            get
            {
                return new KeyValuePair<string, int>("fiscalResponsibility", Id);
            }
        }


    }
}