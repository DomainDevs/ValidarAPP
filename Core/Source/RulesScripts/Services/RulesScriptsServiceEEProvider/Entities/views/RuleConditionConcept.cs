using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class RuleConditionConceptView : BusinessView
    {

        public BusinessCollection RuleCondition
        {
            get { return this["RuleCondition"]; }
        }

        public BusinessCollection RuleConditionConcept
        {
            get { return this["RuleConditionConcept"]; }
        }

        public BusinessCollection Concept
        {
            get { return this["Concept"]; }
        }
    }
}

