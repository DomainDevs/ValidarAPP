using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptIntermediary
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_INTERMEDIARY).ToString());

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
