using Sistran.Core.Application.Utilities.RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Utilities.RulesEngine
{
   public  class CompanyRuleConceptCoverage: RuleConceptCoverage
    {
        public static KeyValuePair<string, int> PercentageContract
        {
            get
            {
                return new KeyValuePair<string, int>("PercentageContract", Id);
            }
        }

        public static KeyValuePair<string, int> BillingPeriodDepositPremium
        {
            get
            {
                return new KeyValuePair<string, int>("BillingPeriodDepositPremium", Id);
            }
        }

        public static KeyValuePair<string, int> DeclarationPeriod
        {
            get
            {
                return new KeyValuePair<string, int>("DeclarationPeriod", Id);
            }
        }

        public static KeyValuePair<string, int> MultirriesgoVAseg
        {
            get
            {
                return new KeyValuePair<string, int>("MultirriesgoVAseg", Id);
            }
        }

    }
}
