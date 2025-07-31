using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptPolicies
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_EVENT).ToString());

        public static KeyValuePair<string, int> GenerateEvent => new KeyValuePair<string, int>("GenerateEvent", Id);

        public static KeyValuePair<string, int> Hierarchy => new KeyValuePair<string, int>("Hierarchy", Id);

        public static KeyValuePair<string, int> EventId => new KeyValuePair<string, int>("EventId", Id);

        public static KeyValuePair<string, int> UserId => new KeyValuePair<string, int>("UserId", Id);

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