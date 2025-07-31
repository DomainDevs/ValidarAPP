using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class RuleProcessRulsetView : BusinessView
    {

        public BusinessCollection RuleProcessGroups
        {
            get
            {
                return this["RuleProcessGroup"];
            }
        }

        public BusinessCollection RuleProcessRulesets
        {
            get
            {
                return this["RuleProcessRuleset"];
            }
        }

        
    }
}

