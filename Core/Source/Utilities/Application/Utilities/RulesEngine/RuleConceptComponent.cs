using Sistran.Core.Application.Utilities.Helper;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptComponent
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_COMPONENT).ToString());

        public static KeyValuePair<string, int> RateTypeCode => new KeyValuePair<string, int>("RateTypeCode", Id);

        public static KeyValuePair<string, int> Rate => new KeyValuePair<string, int>("Rate", Id);

        public static KeyValuePair<string, int> CalculationBaseAmount => new KeyValuePair<string, int>("CalculationBaseAmount", Id);

        public static KeyValuePair<string, int> ComponentCode => new KeyValuePair<string, int>("ComponentCode", Id);

        public static KeyValuePair<string, int> TempId => new KeyValuePair<string, int>("TempId", Id);

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