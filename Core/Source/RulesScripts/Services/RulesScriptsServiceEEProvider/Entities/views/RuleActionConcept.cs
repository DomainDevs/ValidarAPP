using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class RuleActionConceptView : BusinessView
    {

        public BusinessCollection RuleAction
        {
            get { return this["RuleAction"]; }
        }

        public BusinessCollection RuleActionConcept
        {
            get { return this["RuleActionConcept"]; }
        }

        public BusinessCollection Concept
        {
            get { return this["Concept"]; }
        }
    }
}
