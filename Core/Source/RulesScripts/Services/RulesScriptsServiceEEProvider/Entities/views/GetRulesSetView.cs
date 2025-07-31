using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities.views
{
    [Serializable()]
    public class GetRulesSetView : BusinessView
    {
        public BusinessCollection RuleSets
        {
            get
            {
                return this["RuleSet"];
            }
        }

        public BusinessCollection Packages
        {
            get
            {
                return this["Package"];
            }
        }

        public BusinessCollection Levels
        {
            get
            {
                return this["Level"];
            }
        }
    }
}
