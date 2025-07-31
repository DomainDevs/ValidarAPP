using Sistran.Core.Application.Utilities.RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Utilities.RulesEngine
{
    public class CompanyRuleConceptRisk : RuleConceptRisk
    {      

        public static KeyValuePair<string, int> BeneficiariesOnerousCount
        {
            get
            {
                return new KeyValuePair<string, int>("BeneficiariesOnerousCount", Id);
            }
        }
        public static KeyValuePair<string, int> BeneficiariesNoOnerousCount
        {
            get
            {
                return new KeyValuePair<string, int>("BeneficiariesNoOnerousCount", Id);
            }
        }
        public static KeyValuePair<string, int> FasecoldaCode
        {
            get
            {
                return new KeyValuePair<string, int>("FasecoldaCode", Id);
            }
        }

        public static KeyValuePair<string, int> IdLastCoverage
        {
            get
            {
                return new KeyValuePair<string, int>("IdLastCoverage", Id);

            }
        }
        public static KeyValuePair<string, int> TonsQty
        {
            get
            {
                return new KeyValuePair<string, int>("TonsQty", Id);

            }
        }

        public static KeyValuePair<string, int> CoverageGroupIdPreview
        {
            get
            {
                return new KeyValuePair<string, int>("CoverageGroupIdPreview", Id);

            }
        }

        public static KeyValuePair<string, int> LimitsRcCodePreview
        {
            get
            {
                return new KeyValuePair<string, int>("LimitsRcCodePreview", Id);

            }
        }

        public static KeyValuePair<string, int> Accesories
        {
            get
            {
                return new KeyValuePair<string, int>("Accesories", Id);
            }
        }

        public static KeyValuePair<string, int> ExecuteMinimumPremiumCoverageFunction
        {
            get
            {
                return new KeyValuePair<string, int>("ExecuteMinimumPremiumCoverageFunction", Id);
            }
        }
        public static KeyValuePair<string, int> IsFacultative
        {
            get
            {
                return new KeyValuePair<string, int>("IsFacultative", Id);
            }
        }

        public static KeyValuePair<string, int> TechnicalCard
        {
            get
            {
                return new KeyValuePair<string, int>("TechnicalCard", Id);
            }
        }
        public static KeyValuePair<string, int> ContractorAssociationType
        {
            get
            {
                return new KeyValuePair<string, int>("ContractorAssociationType", Id);
            }
        }
        public static KeyValuePair<string, int> SinesterCount
        {
            get
            {
                return new KeyValuePair<string, int>("SinesterCount", Id);
            }
        }
        
        public static KeyValuePair<string, int> PileAmount
        {
            get
            {
                return new KeyValuePair<string, int>("PileAmount", Id);
            }
        }
        public static KeyValuePair<string, int> LimitPileAmount
        {
            get
            {
                return new KeyValuePair<string, int>("LimitPileAmount", Id);
            }
        }

        public static KeyValuePair<string, int> DeliveryDate
        {
            get
            {
                return new KeyValuePair<string, int>("DeliveryDate", Id);
            }
        }

        public static KeyValuePair<string, int> ContractDate
        {
            get
            {
                return new KeyValuePair<string, int>("ContractDate", Id);
            }
        }
        public static KeyValuePair<string, int> CurrentFromRisk
        {
            get
            {
                return new KeyValuePair<string, int>("CurrentFromRisk", Id);
            }
        }
    }
}
