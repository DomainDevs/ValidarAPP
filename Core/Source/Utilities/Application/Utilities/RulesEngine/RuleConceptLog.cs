using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptLog
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_LOG).ToString());

        public static KeyValuePair<string, int> RuleSetId => new KeyValuePair<string, int>("RuleSetId", Id);
        public static KeyValuePair<string, int> RuleSetName => new KeyValuePair<string, int>("RuleSetName", Id);
        public static KeyValuePair<string, int> RuleName => new KeyValuePair<string, int>("RuleName", Id);
        public static KeyValuePair<string, int> Condition => new KeyValuePair<string, int>("Condition", Id);
        public static KeyValuePair<string, int> Action => new KeyValuePair<string, int>("Action", Id);

        public static KeyValuePair<string, int> Facade => new KeyValuePair<string, int>("Facade", Id);

        public static KeyValuePair<int, int> DynamicConcept(int id)
        {
            return new KeyValuePair<int, int>(id, Id);
        }
        public static KeyValuePair<int, int> DynamicConcept(int id, int entityId)
        {
            return new KeyValuePair<int, int>(id, entityId);
        }
    }
}