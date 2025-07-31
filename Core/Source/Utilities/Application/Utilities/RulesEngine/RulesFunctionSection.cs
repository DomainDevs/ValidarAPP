using System.Configuration;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RulesFunctionSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public RulesFunctionCollection functionRuleToMethods
        {
            get
            {
                return (RulesFunctionCollection)base[""];
            }
        }

        public RulesFunctionSection()
        {

        }
    }
}